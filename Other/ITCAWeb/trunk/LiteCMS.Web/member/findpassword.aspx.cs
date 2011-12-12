using System;
using System.Web;
using Natsuhime.Web;
using LiteCMS.Entity;
using LiteCMS.Core;

namespace LiteCMS.Web
{
    public partial class findpassword : BasePage
    {
        protected string findusername = "";
        protected string secques = "";
        protected override void Page_Show()
        {
            if (userid > 0)
            {
                ShowError("找回密码失败!错误原因:", "您已经登录了本站,如果需要修改密码,请在用户中心修改!", "", "usercontrolpanel.aspx");  
            }
            if (ispost)
            {
                findusername = YRequest.GetString("loginid");

                if (findusername == string.Empty)
                {
                    ShowError("找回密码失败!错误原因:", "输入框为空,请填写完整表格!", "", "");                    
                }
                UserInfo info = Users.GetUserInfo(findusername, 0);
                if (info != null)
                {
                    string secans = YRequest.GetString("secretanswer");
                    if (secans == string.Empty)
                    {
                        findusername = info.Email;
                        secques = info.Secquestion;
                    }
                    else
                    {
                        string newpassword = YRequest.GetString("password");
                        if (newpassword == string.Empty)
                        {
                            ShowError("找回密码失败!错误原因:", "密码框为空,请填写新的密码!", "", "");
                        }
                        if (Natsuhime.Common.Utils.MD5(secans) == info.Secanswer)
                        {
                            info.Password = Natsuhime.Common.Utils.MD5(newpassword);
                            Users.EditUser(info);
                            ShowMsg("找回密码消息", "重设密码成功,请用新的密码登录系统.", "", "login.aspx");
                        }
                        else
                        {
                            ShowError("找回密码失败!错误原因:", "验证问答错误!", "", "");
                        }
                    }
                }

            }
        }
    }
}
