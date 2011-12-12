using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Natsuhime.WebApp.Discuz.Entity;
using Natsuhime.Events;
using Natsuhime.Common;

namespace Natsuhime.WebApp.Discuz
{
    public class Online
    {
        public string Charset
        {
            get
            {
                return _httper.Charset;
            }
            set
            {
                _httper.Charset = value;
            }
        }
        public WebProxy Proxy
        {
            get
            {
                return _httper.Proxy;
            }
            set
            {
                _httper.Proxy = value;
            }
        }
        public CookieContainer Cookie
        {
            get
            {
                return _httper.Cookie;
            }
            set
            {
                _httper.Cookie = value;
            }

        }

        public LoginInfo LoginInfos { get; set; }


        NewHttper _httper;
        string _formhash;

        public event EventHandler<Events.MoreReturnCompletedEventArgs> OnLoginCompleted;
        public event EventHandler<Events.MoreReturnCompletedEventArgs> OnCreateThreadCompleted;
        public event EventHandler<Events.MoreProgressChangedEventArgs> OnCreateThreadProgressChanged;

        public Online()
        {
            this._httper = new NewHttper();
            this._httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(_httper_RequestStringCompleted);
        }


        void _httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            AsyncObject ao = e.UserState as AsyncObject;
            string step = ao.MySetId.ToString();

            if (step == "InitCompleted")
            {
                this.InitCompleted(sender, e);
            }
            else if (step == "SendLoginInfoCompleted")
            {
                this.SendLoginInfoCompleted(sender, e);
            }
            else if (step == "InitCreateThreadCompleted")
            {
                this.InitCreateThreadCompleted(sender, e);
            }
            else if (step == "SendCreateThreadInfoCompleted")
            {
                this.SendCreateThreadInfoCompleted(sender, e);
            }
        }

        #region 登录
        void _SendLoginComplete(object sender, bool isSuccess, string message, Exception ex)
        {
            if (this.OnLoginCompleted != null)
            {
                this.OnLoginCompleted(this,
                    new Natsuhime.Events.MoreReturnCompletedEventArgs(
                        isSuccess,
                        message,
                        null,
                        ex,
                        false,
                        null
                        )
                    );
            }
        }

        public bool IsLogined()
        {
            return this.Cookie != null;
        }

        public void Login()
        {
            if (LoginInfos == null)
            {
                _SendLoginComplete(this, false, "没有设定登录信息{this.LoginInfos}", null);
            }

            if (_httper.Cookie == null)
            {
                _httper.Cookie = new CookieContainer();
            }

            //if (IsLogined())
            //{
            //    _SendLoginComplete(this, false, "已经登录过了", null);
            //    return;
            //}

            Init(this.LoginInfos.LoginUrl);
        }


        void Init(string Url)
        {
            this._httper.Url = Url;
            this._httper.RequestStringAsync(EnumRequestMethod.GET, new AsyncObject(Guid.NewGuid(), "InitCompleted", null));
        }

        void InitCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string formhash = Security.GetFormHash(e.ResponseString);
                if (formhash != null && formhash != string.Empty)
                {
                    this._formhash = formhash;
                    SendLoginInfo();
                }
                else
                {
                    this._SendLoginComplete(this, false, "获取FormHash失败", null);
                }
            }
            else
            {
                _SendLoginComplete(sender, false, "获取FormHash异常", e.Error);
            }
        }

        void SendLoginInfo()
        {
            Dictionary<string, string> postParams = new Dictionary<string, string>();
            postParams.Add("formhash", _formhash);
            postParams.Add("referer", "index.php");
            postParams.Add("loginfield", LoginInfos.LoginType.ToString().ToLower());
            postParams.Add("username", LoginInfos.LoginName);
            postParams.Add("password", LoginInfos.Password);
            postParams.Add("questionid", LoginInfos.Questionid);
            postParams.Add("answer", LoginInfos.Answer);
            postParams.Add("cookietime", "2592000");
            postParams.Add("loginsubmit", "%CC%E1%BD%BB");

            _httper.SetPostParam(postParams);
            //param.Add("", );
            /*this._httper.PostData = string.Format("&formhash={0}&referer=index.php&loginfield={1}&username={2}&password={3}&questionid={4}&answer={5}&cookietime=2592000&loginsubmit=%CC%E1%BD%BB",
            this._formhash,
            LoginInfos.LoginType.ToString().ToLower(),
            Web.Utils.UrlEncode(LoginInfos.LoginName, this._httper.Charset),
            LoginInfos.Password,
            LoginInfos.Questionid,
            LoginInfos.Answer
            );
             */
            this._httper.RequestStringAsync(EnumRequestMethod.POST, new AsyncObject(Guid.NewGuid(), "SendLoginInfoCompleted", null));
        }
        void SendLoginInfoCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.ResponseString.IndexOf("欢迎您回来") > -1)
                {
                    this._SendLoginComplete(this, true, e.ResponseString, null);
                }
                else
                {
                    this._SendLoginComplete(this, false, "没有匹配到成功字符串：\r\n" + e.ResponseString, null);
                }
            }
            else
            {
                _SendLoginComplete(sender, false, "发送登录信息异常", e.Error);
            }
        }
        #endregion




        #region 发主题

        void _SendCreateThreadInofProgressChange(object sender, bool isSuccess, string message, Exception ex)
        {
            if (this.OnCreateThreadProgressChanged != null)
            {
                this.OnCreateThreadProgressChanged(this,
                    new MoreProgressChangedEventArgs(
                        isSuccess,
                        message,
                        ex,
                        0,
                        0,
                        null)
                );
            }
        }

        void _SendCreateThreadInfoComplete(object sender, bool isSuccess, string message, Exception ex)
        {
            if (this.OnCreateThreadCompleted != null)
            {
                this.OnCreateThreadCompleted(this,
                    new Natsuhime.Events.MoreReturnCompletedEventArgs(
                        isSuccess,
                        message,
                        null,
                        ex,
                        false,
                        null
                        )
                    );
            }
        }
        public void CreateThread(List<CreateThreadInfo> createThreadInfoList)
        {
            InitCreateThread("http://dev.discuz.org/memcp.php", createThreadInfoList);
        }

        void InitCreateThread(string Url, List<CreateThreadInfo> createThreadInfoList)
        {
            this._httper.Url = Url;
            this._httper.RequestStringAsync(
                EnumRequestMethod.GET,
                new AsyncObject(Guid.NewGuid(), "InitCreateThreadCompleted", createThreadInfoList)
                );
        }

        void InitCreateThreadCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string formhash = Security.GetFormHash(e.ResponseString);
                if (formhash != null && formhash != string.Empty)
                {
                    this._formhash = formhash;

                    AsyncObject ao = e.UserState as AsyncObject;
                    SendCreateThreadInfo(ao.MySetObject as List<CreateThreadInfo>);
                }
                else
                {
                    this._SendCreateThreadInfoComplete(this, false, "获取FormHash失败", null);
                }
            }
            else
            {
                _SendCreateThreadInfoComplete(sender, false, "获取FormHash异常", e.Error);
            }
        }

        void SendCreateThreadInfo(List<CreateThreadInfo> createThreadInfoList)
        {
            if (createThreadInfoList.Count > 0)
            {
                DoCreateThreadInfo(createThreadInfoList);
            }
            else
            {
                _SendCreateThreadInfoComplete(this, true, "", null);
            }
        }

        private void DoCreateThreadInfo(List<CreateThreadInfo> createThreadInfoList)
        {
            CreateThreadInfo cti = createThreadInfoList[0];
            this._httper.Url = string.Format("http://dev.discuz.org/post.php?action=newthread&fid={0}&topicsubmit=yes", cti.fid);

            Dictionary<string, string> postParams = new Dictionary<string, string>();
            postParams.Add("formhash", _formhash);
            //postParams.Add("referer", "index.php");
            postParams.Add("subject", cti.Subject);
            postParams.Add("message", cti.Message);
            postParams.Add("app[do_username]", cti.DoUsername);
            postParams.Add("app[do_status]", cti.DoStatus);
            postParams.Add("app[pd_username]", cti.PdUsername);
            postParams.Add("app[rd_username]", cti.RdUsername);
            postParams.Add("app[ued_username]", cti.UedUsername);
            postParams.Add("app[qa_username]", cti.QaUsername);
            //postParams.Add("", );
            _httper.SetPostParam(postParams);

            this._httper.RequestStringAsync(
                EnumRequestMethod.POST,
                new AsyncObject(Guid.NewGuid(), "SendCreateThreadInfoCompleted", createThreadInfoList)
                );
        }

        void SendCreateThreadInfoCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            AsyncObject ao = e.UserState as AsyncObject;
            List<CreateThreadInfo> ctiList = ao.MySetObject as List<CreateThreadInfo>;
            if (e.Error == null)
            {
                if (e.ResponseString.IndexOf("您的帖子已经发布") > -1)
                {
                    _SendCreateThreadInofProgressChange(this, true, "", null);
                }
                else
                {
                    string errorMessage = string.Format(
                        "没有捕获到发帖成功文字：\r\n{0}\r\n\r\n\r\nfid:{1}\r\ntid:{2}\r\ntitle:{3}",
                        e.ResponseString,
                        ctiList[0].fid,
                        ctiList[0].tid,
                        ctiList[0].Subject
                        );
                    _SendCreateThreadInofProgressChange(this, false, errorMessage, null);
                }
            }
            else
            {
                string errorMessage = string.Format(
                    "发布主题异常：\r\n{0}\r\n\r\n\r\nfid:{1}\r\ntid:{2}\r\ntitle:{3}",
                    e.ResponseString,
                    ctiList[0].fid,
                    ctiList[0].tid,
                    ctiList[0].Subject
                    );
                _SendCreateThreadInofProgressChange(sender, false, errorMessage, e.Error);
            }
            ctiList.Remove(ctiList[0]);
            if (ctiList.Count > 0)
            {
                DoCreateThreadInfo(ctiList);
            }
            else
            {
                _SendCreateThreadInfoComplete(this, true, "", null);
            }

        }
        #endregion
    }
}
