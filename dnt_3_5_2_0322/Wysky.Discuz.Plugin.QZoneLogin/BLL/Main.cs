using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Forum;

namespace Wysky.Discuz.Plugin.QZoneLogin.BLL
{
    public class Main
    {
        public static int GetUIDByQqOpenid(string openid)
        {
            return Data.Sqlserver.DbGetUIDByQqOpenid(openid);
        }

        public static void DeleteQqLoginInfo(string openid, int uid)
        {
            Data.Sqlserver.DbDeleteQqLoginInfo(openid, uid);
        }

        public static string GetQqOpenidByUID(int uid)
        {
            return Data.Sqlserver.DbGetQqOpenidByUID(uid);
        }

        public static bool IsNullPasswordUser(int uid)
        {
            return Data.Sqlserver.DbIsNullPasswordUser(uid);
        }

        public static UserInfo CreateUser(string tmpUsername, string email)
        {
            // 如果找不到0积分的用户组则用户自动成为待验证用户
            UserInfo userinfo = new UserInfo();
            userinfo.Username = tmpUsername;
            userinfo.Nickname = tmpUsername;
            userinfo.Password = "qqlogin_by_wysky.org";
            userinfo.Secques = "";
            userinfo.Gender = 0;
            userinfo.Adminid = 0;
            userinfo.Groupexpiry = 0;
            userinfo.Extgroupids = "";
            userinfo.Regip = DNTRequest.GetIP();
            userinfo.Joindate = Utils.GetDateTime();
            userinfo.Lastip = DNTRequest.GetIP();
            userinfo.Lastvisit = Utils.GetDateTime();
            userinfo.Lastactivity = Utils.GetDateTime();
            userinfo.Lastpost = Utils.GetDateTime();
            userinfo.Lastpostid = 0;
            userinfo.Lastposttitle = "";
            userinfo.Posts = 0;
            userinfo.Digestposts = 0;
            userinfo.Oltime = 0;
            userinfo.Pageviews = 0;
            userinfo.Credits = 0;
            userinfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
            userinfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
            userinfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
            userinfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
            userinfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
            userinfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
            userinfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
            userinfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
            userinfo.Email = email;
            userinfo.Bday = "1900-1-1";
            userinfo.Sigstatus = 1;
            userinfo.Tpp = 0;
            userinfo.Ppp = 0;
            userinfo.Templateid = 0;
            userinfo.Pmsound = 0;
            userinfo.Showemail = 0;
            userinfo.Salt = "";



            userinfo.Newsletter = ReceivePMSettingType.ReceiveAllPMWithHint;
            userinfo.Invisible = 0;
            userinfo.Newpm = 1;
            userinfo.Medals = "";
            userinfo.Accessmasks = 0;
            userinfo.Website = "";
            userinfo.Icq = "";
            userinfo.Qq = "";
            userinfo.Yahoo = "";
            userinfo.Msn = "";
            userinfo.Skype = "";
            userinfo.Location = "";
            userinfo.Customstatus = "";
            userinfo.Bio = "";
            userinfo.Signature = "";
            userinfo.Sightml = "";
            userinfo.Authtime = Utils.GetDateTime();


            userinfo.Authstr = "";
            userinfo.Authflag = 0;
            userinfo.Groupid = UserCredits.GetCreditsUserGroupId(0).Groupid;
            userinfo.Realname = "";
            userinfo.Idcard = "";
            userinfo.Mobile = "";
            userinfo.Phone = "";
            userinfo.Uid = Users.CreateUser(userinfo);
            return userinfo;
        }

        public static int CreateQqUserInfo(string openid, int uid)
        {
            return Data.Sqlserver.DbCreateQqUserInfo(openid, uid);
        }

        public static int Install()
        {
            return Data.Sqlserver.DbInstall();
        }
    }
}