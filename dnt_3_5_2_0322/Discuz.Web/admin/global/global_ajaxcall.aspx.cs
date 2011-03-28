using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Mall;
using System.Text;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ������̳ͳ��
    /// </summary>
    
    public class ajaxcall : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int pertask = DNTRequest.GetInt("pertask", 0);
                int lastnumber = DNTRequest.GetInt("lastnumber", 0);
                int startvalue = DNTRequest.GetInt("startvalue", 0);
                int endvalue = DNTRequest.GetInt("endvalue", 0);
                string resultmessage = "";
                switch (Request.Params["opname"])
                {
                    case "UpdatePostSP":
                        AdminForumStats.UpdatePostSP(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "UpdateMyPost":
                        AdminForumStats.UpdateMyPost(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetFourmTopicAPost":
                        //AdminForumStats.ReSetFourmTopicAPost(pertask, ref lastnumber);
                        AdminForumStats.ReSetFourmTopicAPost();
                        resultmessage = "-1";
                        break;
                    case "ReSetUserDigestPosts":
                        //AdminForumStats.ReSetUserDigestPosts(pertask, ref lastnumber);
                        //resultmessage = lastnumber.ToString();
                        AdminForumStats.ReSetUserDigestPosts();
                        resultmessage = "-1";
                        break;
                    case "ReSetUserPosts":
                        AdminForumStats.ReSetUserPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetTopicPosts":
                        AdminForumStats.ReSetTopicPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetFourmTopicAPost_StartEnd":
                        AdminForumStats.ReSetFourmTopicAPost(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserDigestPosts_StartEnd":
                        AdminForumStats.ReSetUserDigestPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserPosts_StartEnd":
                        AdminForumStats.ReSetUserPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetTopicPosts_StartEnd":
                        AdminForumStats.ResetLastRepliesInfoOfTopics(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ftptest":
                        FTPs ftps = new FTPs();
                        string message = "";
                        bool ok = ftps.TestConnect(DNTRequest.GetString("serveraddress"), DNTRequest.GetInt("serverport", 0), DNTRequest.GetString("username"), 
                            DNTRequest.GetString("password"), DNTRequest.GetInt("timeout", 0), DNTRequest.GetString("uploadpath"), ref message);
                        resultmessage = ok ? "ok" : "Զ�̸������ò��Գ��ִ���\n������" + message;
                        break;
                    case "setapp":
                        APIConfigInfo aci = APIConfigs.GetConfig();
                        aci.Enable = DNTRequest.GetString("allowpassport") == "1";
                        APIConfigs.SaveConfig(aci);
                        resultmessage = "ok";
                        break;
                    case "location":
                        string city = DNTRequest.GetString("city");
                        resultmessage = "ok";
                        DataTable dt = MallPluginProvider.GetInstance().GetLocationsTable();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["country"].ToString() == DNTRequest.GetString("country") && dr["state"].ToString() == DNTRequest.GetString("state") && dr["city"].ToString() == city)
                            {
                               resultmessage = "<img src='../images/false.gif' title='" + city + "�Ѿ�����!'>";
                                break;
                            }
                        }
                        break;
                    case "goodsinfo":
                        int goodsid = DNTRequest.GetInt("goodsid", 0);
                        Goodsinfo goodsinfo = MallPluginProvider.GetInstance().GetGoodsInfo(goodsid);
                        if (goodsinfo == null)
                        {
                            resultmessage = "��Ʒ������!";
                            break;
                        }
                        //GoodsattachmentinfoCollection attachmentinfos = GoodsAttachments.GetGoodsAttachmentsByGoodsid(goodsinfo.Goodsid);
                        //string img = "";
                        //if (attachmentinfos != null)
                        //{
                        //    img = attachmentinfos[0].Filename;
                        //}
                        PostpramsInfo param = new PostpramsInfo();
                        param.Allowhtml = 1;
                        param.Showimages = 1;
                        param.Sdetail = goodsinfo.Message;
                        resultmessage = "<table width='100%'><tr><td>" + UBB.UBBToHTML(param) + "</td></tr></table>";
                        break;
                    case "downloadword":
                        dt = BanWords.GetBanWordList();
                        string words = "";
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                words += dt.Rows[i][2].ToString() + "=" + dt.Rows[i][3].ToString() + "\r\n";
                            }
                        }

                        string filename = "words.txt";
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = false;
                        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));
                        HttpContext.Current.Response.ContentType = "text/plain";
                        HttpContext.Current.Response.Write(words);
                        HttpContext.Current.Response.End();
                        break;

                    case "gettopicinfo":
                        StringBuilder sb = new StringBuilder();
                        TopicInfo info = Topics.GetTopicInfo(DNTRequest.GetInt("tid", 0));
                        sb.Append("[");
                        if (info != null)
                        {

                           sb.Append(string.Format("{{'tid':{0},'title':'{1}'}}", info.Tid, info.Title));
                        }

                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.ContentType = "application/json";
                        System.Web.HttpContext.Current.Response.Expires = 0;
                        System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                        System.Web.HttpContext.Current.Response.Write(sb.Append("]").ToString());
                        System.Web.HttpContext.Current.Response.End();
                        break;
                }
                Response.Write(resultmessage);
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.Expires = -1;
                Response.End();
            }
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}