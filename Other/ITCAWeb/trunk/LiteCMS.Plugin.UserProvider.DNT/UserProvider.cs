using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Toolkit;

namespace LiteCMS.Plugin.UserProvider
{
    public class DNT : IUserProvider
    {
        string api = "30a9be036086fc3bb961ee83644069fb";
        string secret = "d9ef0eac971573a2ceb331a867aa6d06";
        string url = "http://www.92acg.cn/board/";

        public void Login(string loginid, string password, bool isMD5Password, int expires, string cookieDomain)
        {
            DiscuzSession ds = new DiscuzSession(api, secret, url);
            int uid = ds.GetUserID(loginid);
            if (uid > 0)
            {
                ds.Login(uid, password, isMD5Password, expires, "www.92acg.cn");
            }
        }

        public void Logout(string cookieDomain)
        {
            DiscuzSession ds = new DiscuzSession(api, secret, url);
            ds.Logout("92acg.cn");
        }
        public int IsUserExits(string loginid)
        {
            DiscuzSession ds = new DiscuzSession(api, secret, url);
            return ds.GetUserID(loginid);
        }

        public void Register(string username, string password, string email, bool isMD5Password)
        {
            DiscuzSession ds = new DiscuzSession(api, secret, url);
            ds.Register(username, password, email, isMD5Password);
        }

        public void EditPassword(string username, string newMD5Password)
        {
            DiscuzSession ds = new DiscuzSession(api, secret, url);
            int uid = ds.GetUserID(username);
            if (uid > 0)
            {
                UserForEditing ufe = new UserForEditing();
                ufe.Password = newMD5Password;
                ds.SetUserInfo(uid, ufe);
            }
        }
    }
}
