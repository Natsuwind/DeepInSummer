using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using Natsuhime.Data;
using LiteCMS.Entity;
using LiteCMS.Config;

namespace LiteCMS.Data.MySql
{
    public partial class DataProvider : IDataProvider
    {
        public IDataReader GetAdmins(int pagesize, int currentpage)
        {
            throw new NotImplementedException();
        }

        public int GetAdminCollectionPageCount(int pagesize)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetAdminInfo(string name, string password)
        {
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("?name", (DbType)MySqlDbType.String, 50, name),
                DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32, password)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}admins WHERE name=?name AND password=?password", BaseConfigs.GetConfig().Tableprefix), prams);
            return dr;
        }

        public IDataReader GetAdminInfo(int adminid, string password)
        {
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("?adminid", (DbType)MySqlDbType.Int32, 4, adminid),
                DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32, password)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}admins WHERE adminid=?adminid AND password=?password", BaseConfigs.GetConfig().Tableprefix), prams);
            return dr;
        }

        public IDataReader GetAdminInfo(int adminid)
        {
            throw new NotImplementedException();
        }

        public void AddAdmin(AdminInfo info)
        {
            throw new NotImplementedException();
        }

        public void EditAdmin(AdminInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
