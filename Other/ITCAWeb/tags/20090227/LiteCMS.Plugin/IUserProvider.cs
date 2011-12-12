using System;
using System.Collections.Generic;
using System.Text;

namespace LiteCMS.Plugin
{
    public interface IUserProvider
    {
        void Login(string loginid, string password, bool isMD5Password, int expires, string cookieDomain);
        void Logout(string cookieDomain);
        int IsUserExits(string loginid);
        void Register(string username, string password, string email, bool isMD5Password);
        void EditPassword(string username, string newPassword);
    }
}
