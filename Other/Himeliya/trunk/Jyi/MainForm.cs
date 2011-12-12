using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Threading;
using Jyi.Entity;
using Jyi.Config;
using Jyi.Utility;

namespace Jyi
{
    public partial class MainForm : Form
    {
        #region 全局静态变量

        //文档路径
        public static string configFilePath = "";
        public static string ProjectName;
        Thread mythread;


        //设置消息跨线程执行
        public static MainForm objMainForm;//在启动线程之前把本窗口的示例赋值给objMainForm
        delegate void SetTextCallback(string text);
        public void SetText(string text)
        {
            if (this.tbMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tbMessage.Text += text;
                if (!this.cbStopScroll.Checked)
                {
                    this.tbMessage.SelectionStart = this.tbMessage.TextLength;//设置光标的位置到文本尾
                    this.tbMessage.ScrollToCaret();//滚动到控件光标处
                    if (this.tbMessage.TextLength >= 50000)
                    {
                        this.tbMessage.Text = "";
                    }
                }
                //this.tbMessage.Focused = false;
            }

        }


        #endregion

        #region 初始化以及刷新列表,清空消息

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //初始化开始...
            //配置文档路径初始化
            configFilePath = System.IO.Path.Combine(Application.StartupPath, "Jyi.xml");
            if (!System.IO.File.Exists(configFilePath))
            {
                MessageBox.Show(string.Format("没有找到配置文件：{0}\r\n确定后自重执行重启!", configFilePath));
                Application.ExitThread();
                Application.Restart();
            }

            this.BindcbbProject();
            this.BindcbbSchedule();
        }

        private void BindcbbSchedule()
        {
            cbbSchedule.DataSource = XmlNodeAttribList.GetNodeAttribList("TaskSchedule", "Name");
        }
        private void BindcbbProject()
        {
            cbbProject.DataSource = XmlNodeAttribList.GetNodeAttribList("PostProject", "Name");
        }

        private void btnRefreshProjectList_Click(object sender, EventArgs e)
        {
            this.BindcbbProject();
        }
        private void btnRefreshScheduleList_Click(object sender, EventArgs e)
        {
            this.BindcbbSchedule();
        }

        private void btnClearMessageBox_Click(object sender, EventArgs e)
        {
            this.tbMessage.Text = "";
        }
        #endregion

        #region 线程启动和停止
        private void btnStartProject_Click(object sender, EventArgs e)
        {
            objMainForm = this;
            Project objProject = new Project(cbbProject.Text.Trim(), objMainForm);
            ThreadStart myThreadDelegate = new ThreadStart(objProject.ExecuteProject);
            mythread = new Thread(myThreadDelegate);
            mythread.Start();
            btnStartProject.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mythread.Abort();
            btnStartProject.Enabled = true;
            btnStop.Enabled = false;
            this.SetText("已经停止了线程!");
        }
        #endregion

        private void btnProxySet_Click(object sender, EventArgs e)
        {
            ProxyForm objForm = new ProxyForm();
            objForm.Show();
        }
    }



    class Project
    {
        static string m_ProjectName;//传递过来的项目名
        static MainForm m_MainForm;//传递过来的父窗体对象
        static PostProjectInfo objPostProjectInfo;//声明项目实例
        static BaseConfigInfo objBaseConfigInfo;//声明基本配置类,目前代理列表在其中
        static CookieContainer cookieContainer;//声明Cookie   
        static List<ProxyInfo> objGoodProxyInfoList=new List<ProxyInfo>();//成功了的代理列表
        static int urlThreadSuccessFlag = 1;//执行Url任务时候,如果为0,就跳出当前Url列表,重新开始新的列表.默认为成功(1)


        public Project(string ProjectName, MainForm objMainForm)
        {
            m_ProjectName = ProjectName;
            m_MainForm = objMainForm;
        }

        public void ExecuteProject()
        {
            m_MainForm.SetText(string.Format("项目『{0}』启动线程成功~\r\n", m_ProjectName));
            objPostProjectInfo = PostProjectConfig.GetConfig(m_ProjectName);//初始化项目实例
            objBaseConfigInfo = BaseConfig.GetBaseConfig();//初始化基本配置类,目前代理列表在其中
            cookieContainer = new CookieContainer();//初始化Cookie
            ProxyInfo objProxyInfo;//声明代理信息对象
            WebProxy objWebProxy=null;//声明代理对象
            

            //需要使用的预处理参数列表
            List<ParmsInfo> objGetParmsInfoList = new List<ParmsInfo>();


            for (int i = 0; i < objPostProjectInfo.BaseInfo.Count; i++)
            {
                urlThreadSuccessFlag = 1;//执行Url任务时候,如果为0,就跳出当前Url列表,重新开始新的列表.默认为成功(1,由于会在后面被重置为0,每个列表开始循环是先初始化为1,否则后面无法进行)
                if (objPostProjectInfo.BaseInfo.ClearCookieEverytime > 0)//如果每个周期循环前都需要清除Cookie
                {
                    cookieContainer = new CookieContainer();
                }

                if (objPostProjectInfo.BaseInfo.UseCookie <= 0)
                {
                    cookieContainer = null;//如果cookie为空,在conn类中将自动判断为不使用cookie
                }


                objWebProxy = null;//初始化代理对象为空(因为如果代理为空,在conn类中将自动判断为不使用代理.如果要使用代理,则通过下面的语句赋值)
                if (objPostProjectInfo.BaseInfo.UseProxy > 0)
                {
                    objProxyInfo = Proxy.GetNextProxy(objBaseConfigInfo.ProxyList);//获取未使用过的代理信息对象

                    if (objProxyInfo != null)//如果配置为需要使用代理且代理没有循环完,则赋值给代理对象,但是代理却用完了,则终止
                    {
                        objWebProxy = new WebProxy(objProxyInfo.Address, objProxyInfo.Port);
                    }
                    else
                    {
                        m_MainForm.SetText("无可用代理,自动停止项目执行!\r\n");
                        Utility.Utility.WriteGoodProxyListToXml(objGoodProxyInfoList);//写入goodproxy
                        Thread.CurrentThread.Abort();
                    }
                }


                //开始处理Url列表并链接
                foreach (UrlInfo objUrlInfo in objPostProjectInfo.UrlInfoList)
                {
                    if (urlThreadSuccessFlag == 0)
                    {
                        m_MainForm.SetText(string.Format("\r\n===========在上个Url连接中失败,重新开始新一轮的列表循环===========\r\n"));
                        //continue;
                        break;
                    }
                    m_MainForm.SetText(string.Format("Url：[{0}]开始\r\n", objUrlInfo.Name));
                    for (int j = 0; j < objUrlInfo.Count; j++)//目前不管Count是多少还只能使用一个代理连接,想到的解决方案是,拷贝一份objProxyInfoList,然后再当前Url里面循环
                    {

                        if (objUrlInfo.Method == "POST")
                        {
                            StringBuilder sbParms = new StringBuilder();
                            foreach (ParmsInfo objPostParmsInfo in objUrlInfo.PostParmsList)//目前Url参数全部post,然后从objGetParmsList中得到预处理参数post
                            {
                                if (objPostParmsInfo.Type == "Rand")
                                {
                                    sbParms.Append(string.Format("&{0}={1}", objPostParmsInfo.Name, Utility.Utility.GenerateRandom(8,Jyi.Utility.Utility.RandomType.Lowercased)));

                                }
                                else if (objPostParmsInfo.Type == "RandEmail")
                                {
                                    sbParms.Append(string.Format("&{0}={1}@qq.com", objPostParmsInfo.Name, Utility.Utility.GenerateRandom(8)));
                                }
                                else if (objPostParmsInfo.Type == "VCode")
                                {
                                    #region 得到验证码
                                    HttpWebRequest objImgRequest = (HttpWebRequest)HttpWebRequest.Create(objUrlInfo.VCodeUrl);
                                    objImgRequest.Method = "GET";
                                    objImgRequest.CookieContainer = cookieContainer;
                                    if (objWebProxy != null)
                                    {
                                        objImgRequest.Proxy = objWebProxy;
                                    }
                                    WebResponse wr2 = objImgRequest.GetResponse();
                                    System.IO.Stream s = wr2.GetResponseStream();
                                    System.Drawing.Image numPic = System.Drawing.Image.FromStream(s);// 得到验证码图片

                                    UnCode objUnCode = new UnCode((System.Drawing.Bitmap)numPic);
                                    #endregion

                                    sbParms.Append(string.Format("&{0}={1}", objPostParmsInfo.Name, objUnCode.getPicnum(objUrlInfo.UnCodeList)));
                                }
                                else if (objPostParmsInfo.Type == "PreParm")
                                {
                                    foreach (ParmsInfo objGetParms in objGetParmsInfoList)
                                    {
                                        if (objPostParmsInfo.Name == objGetParms.Name)
                                        {
                                            sbParms.Append(string.Format("&{0}={1}", objPostParmsInfo.Name, objGetParms.Type));
                                            m_MainForm.SetText(string.Format("取得预处理参数：{0}={1}\r\n", objPostParmsInfo.Name, objGetParms.Type));
                                        }
                                    }
                                }
                                else
                                {
                                    sbParms.Append(string.Format("&{0}={1}", objPostParmsInfo.Name, objPostParmsInfo.Type));
                                }
                            }

                            //执行RemoveParms,移除本次使用后别人不再使用的预处理参数,避免在后面的Url的Post中遇到重名的参数被错误Post
                            if (objUrlInfo.RemoveParms != "")
                            {
                                int listCount = objGetParmsInfoList.Count;
                                //copy副本
                                //List<ParmsInfo> objGetParmsInfoListTemp = new List<ParmsInfo>() ;                                
                                //foreach (ParmsInfo objGetParmInfo in objGetParmsInfoList)
                                //{
                                //    ParmsInfo objCopyGetParmsInfo = new ParmsInfo();
                                //    objCopyGetParmsInfo.Name = objGetParmInfo.Name;
                                //    objCopyGetParmsInfo.Type = objGetParmInfo.Type;
                                //    objGetParmsInfoListTemp.Add(objCopyGetParmsInfo);
                                //}

                                foreach (string strRemoveParm in objUrlInfo.RemoveParms.Split(','))
                                {
                                    for (int k = 0; k < listCount; k++)
                                    {
                                        if (objGetParmsInfoList[k].Name == strRemoveParm)
                                        {
                                            objGetParmsInfoList.Remove(objGetParmsInfoList[k]);
                                            m_MainForm.SetText(string.Format("移除预处理参数{0}\r\n", strRemoveParm));
                                        }
                                    }
                                    //foreach (ParmsInfo objGetParms in objGetParmsInfoListTemp)
                                    //{
                                    //    if (objGetParms.Name == strRemoveParm)
                                    //    {
                                    //        objGetParmsInfoList.Remove(objGetParms);
                                    //        m_MainForm.SetText(string.Format("移除预处理参数{0}\r\n", objGetParms.Name));
                                    //    }
                                    //}
                                }
                            }
                            m_MainForm.SetText(string.Format("(POST){0}\r\n", objUrlInfo.Url));

                            //POST数据
                            string returnData = Conn.PostData(objUrlInfo.Url, objPostProjectInfo.BaseInfo.Charset, sbParms.ToString(), objUrlInfo.Referer, 0, objWebProxy, cookieContainer);

                            if (returnData.IndexOf("Jyi链接失败")>=0)
                            {
                                m_MainForm.SetText("链接失败!\r\n");
                                m_MainForm.SetText(string.Format("===\r\n{0}\r\n===\r\n", returnData));
                                //Thread.CurrentThread.Abort();
                                urlThreadSuccessFlag = 0;
                                //continue;
                                break;
                            }
                            else if (returnData.IndexOf(objUrlInfo.SuccessText) >= 0)
                            {
                                m_MainForm.SetText("(POST)成功!\r\n");
                                urlThreadSuccessFlag = 1;
                            }
                            else
                            {
                                m_MainForm.SetText("(POST)失败!\r\n");
                                m_MainForm.SetText(string.Format("===\r\n{0}\r\n===\r\n", returnData));
                                //Thread.CurrentThread.Abort();
                                urlThreadSuccessFlag = 0;
                                //continue;
                                break;
                            }


                            //得到预处理参数,添加到预处理列表中
                            if (objUrlInfo.HaveGetParms > 0)
                            {
                                foreach (GetParmsInfo objGetParm in objUrlInfo.GetParmsList)
                                {
                                    ParmsInfo objAddGetParm = new ParmsInfo();
                                    objAddGetParm.Name = objGetParm.Name;
                                    RegexFunc rFunc = new RegexFunc();
                                    objAddGetParm.Type = System.Web.HttpUtility.UrlEncode(rFunc.GetMatch(returnData, objGetParm.Regex), Encoding.GetEncoding(objPostProjectInfo.BaseInfo.Charset));
                                    objGetParmsInfoList.Add(objAddGetParm);

                                    m_MainForm.SetText(string.Format("获取参数{0}的值为{1}\r\n", objAddGetParm.Name, objAddGetParm.Type));
                                }
                            }

                            m_MainForm.SetText(string.Format("(POST)结束\r\n"));

                            Thread.Sleep(objUrlInfo.WaitTime);//Url间隔时间

                        }
                        else
                        {
                            //TODO GETDATA TODAY



                            m_MainForm.SetText(string.Format("(GET){0}\r\n", objUrlInfo.Url));

                            //POST数据
                            string returnData = Conn.GetData(objUrlInfo.Url, objPostProjectInfo.BaseInfo.Charset, objUrlInfo.Referer, 0, objWebProxy, cookieContainer);

                            if (returnData.IndexOf("Jyi链接失败") >= 0)
                            {
                                m_MainForm.SetText("链接失败!\r\n");
                                m_MainForm.SetText(string.Format("===\r\n{0}\r\n===\r\n", returnData));
                                //Thread.CurrentThread.Abort();
                                urlThreadSuccessFlag = 0;
                                //continue;
                                break;
                            }
                            else if (returnData.IndexOf(objUrlInfo.SuccessText) >= 0)
                            {
                                m_MainForm.SetText("(GET)成功!\r\n");
                                urlThreadSuccessFlag = 1;
                            }
                            else
                            {
                                m_MainForm.SetText("(GET)失败!\r\n");
                                m_MainForm.SetText(string.Format("===\r\n{0}\r\n===\r\n", returnData));
                                //Thread.CurrentThread.Abort();
                                urlThreadSuccessFlag = 0;
                                //continue;
                                break;
                            }

                            if (objUrlInfo.HaveGetParms > 0)
                            {

                                //得到预处理参数,添加到预处理列表中
                                foreach (GetParmsInfo objGetParm in objUrlInfo.GetParmsList)
                                {
                                    ParmsInfo objAddGetParm = new ParmsInfo();
                                    objAddGetParm.Name = objGetParm.Name;
                                    RegexFunc rFunc = new RegexFunc();
                                    objAddGetParm.Type = System.Web.HttpUtility.UrlEncode(rFunc.GetMatch(returnData, objGetParm.Regex), Encoding.GetEncoding(objPostProjectInfo.BaseInfo.Charset));
                                    objGetParmsInfoList.Add(objAddGetParm);

                                    m_MainForm.SetText(string.Format("获取参数{0}的值为{1}\r\n", objAddGetParm.Name, objAddGetParm.Type));
                                }
                            }

                            m_MainForm.SetText(string.Format("(GET)结束\r\n"));

                            Thread.Sleep(objUrlInfo.WaitTime);//Url间隔时间




                        }
                    }
                    m_MainForm.SetText(string.Format("Url：[{0}]完毕\r\n", objUrlInfo.Name));
                }

                if (urlThreadSuccessFlag == 1&&objWebProxy!=null)
                {
                    ProxyInfo objGoodProxyInfo = new ProxyInfo();
                    objGoodProxyInfo.Address = objWebProxy.Address.Host;
                    objGoodProxyInfo.Port = objWebProxy.Address.Port;
                    objGoodProxyInfo.Name = "GoodProxy";
                    objGoodProxyInfoList.Add(objGoodProxyInfo);
                }
                m_MainForm.SetText(string.Format("项目『{0}』线程完成一次循环~\r\n", m_ProjectName));
                Thread.Sleep(objPostProjectInfo.BaseInfo.WaitTime);//项目间隔时间                
            }






            if (objWebProxy != null)
            {
                Utility.Utility.WriteGoodProxyListToXml(objGoodProxyInfoList);//写入goodproxy
            }
            m_MainForm.SetText(string.Format("项目『{0}』成功完成任务~\r\n", m_ProjectName));


            //TODO:处理点X关闭的时候自动结束线程,避免线程访问窗体出错
        }
    }





    class XmlNodeAttribList
    {
        /// <summary>
        /// 列出节点下，所有子节点的属性名组成的列表
        /// </summary>
        /// <param name="NodeRelativePath">相对于 /JyiConfig 的路径</param>
        /// <param name="AtrribName">要列出的属性</param>
        /// <returns></returns>
        public static List<string> GetNodeAttribList(string NodeRelativePath, string AtrribName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MainForm.configFilePath);
            XmlNode xn = xmlDoc.SelectSingleNode(string.Format("/JyiConfig/{0}", NodeRelativePath));
            XmlNodeList xnl = xn.ChildNodes;
            List<string> nodeName = new List<string>();
            foreach (XmlNode xn2 in xnl)
            {
                nodeName.Add(xn2.Attributes[AtrribName].Value);
            }
            return nodeName;
        }
    }
}
