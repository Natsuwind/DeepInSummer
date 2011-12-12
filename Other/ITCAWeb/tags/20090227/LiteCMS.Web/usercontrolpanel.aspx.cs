using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class usercontrolpanel : BasePage
    {
        protected UserInfo userinfo;
        protected List<ArticleInfo> myarticlelist;
        protected string pagecounthtml;
        protected override void Page_Show()
        {
            userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("用户中心", "身份验证失败,请登录后再访问用户中心,谢谢~", "", "login.aspx");
            }
            else
            {
                pagetitle = string.Format("{0}的用户中心", userinfo.Username);
                int pageid = YRequest.GetInt("pageid", 1);
                int pagecount = Articles.GetUserArticleCollectionPageCount(userinfo.Uid, 8);
                pagecounthtml = Utils.GetPageNumbersHtml(pageid, pagecount, "usercontrolpanel.aspx", 8, "pageid", "");
                myarticlelist = Articles.GetUserArticleCollection(userinfo.Uid, 8, pageid);

                if (ispost)
                {
                    string oldpassword = YRequest.GetString("oldpassword");
                    string newpassword = YRequest.GetString("newpassword");
                    string newpassword2 = YRequest.GetString("newpassword2");
                    if (newpassword == newpassword2)
                    {
                        string newMD5Password = Natsuhime.Common.Utils.MD5(oldpassword);
                        if (newMD5Password == userinfo.Password)
                        {
                            userinfo.Password = newMD5Password;
                            Users.EditUser(userinfo);
                            ShowMsg("用户中心", "", "修改密码修改成功.", "");
                        }
                        else
                        {
                            ShowError("用户中心", "修改密码失败,旧密码验证错误!请检查是否输入正确,大小写锁定键是否被打开等.", "", "");
                        }
                    }
                    else
                    {
                        ShowError("用户中心", "修改密码失败,两次输入的新密码不一致.", "", "");
                    }
                }
            }
        }
    }
}
