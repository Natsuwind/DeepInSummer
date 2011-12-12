using System;
using System.Data;
using System.Collections.Generic;
using Natsuhime;
using LiteCMS.Data;
using LiteCMS.Entity;

namespace LiteCMS.Core
{
    public class Admins
    {
        private static AdminInfo DataReader2AdminInfo(IDataReader reader)
        {
            AdminInfo info = new AdminInfo();
            info.Adminid = Convert.ToInt32(reader["adminid"]);
            info.Name = reader["name"].ToString();
            info.Password = reader["password"].ToString();
            info.Uid = Convert.ToInt32(reader["uid"]);
            info.Allowip = reader["allowip"].ToString();
            info.Lastlogindate = Convert.ToDateTime(reader["lastlogindate"]).ToString("yyyy-MM-dd");
            info.Lastloginip = reader["lastloginip"].ToString();
            return info;
        }


        public static AdminInfo GetAdminInfo(string name, string password)
        {
            AdminInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetAdminInfo(name, password);
            if (reader.Read())
            {
                info = DataReader2AdminInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        public static AdminInfo GetAdminInfo(int adminid, string password)
        {
            AdminInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetAdminInfo(adminid, password);
            if (reader.Read())
            {
                info = DataReader2AdminInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
    }
}
