using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;


namespace Natsuhime.Data.DbProviders
{
    
    /// <summary>
    /// MySql数据库的Discuz!NT接口, 有关MySql的更多信息见 http://dev.mysql.com/downloads/connector/net/5.0.html
    /// </summary>
    public class MySql : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return MySqlClientFactory.Instance;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if ((cmd as MySqlCommand) != null)
            {
                MySqlCommandBuilder.DeriveParameters(cmd as MySqlCommand);
            }
        }


        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            MySqlParameter param;

            if (Size > 0)
                param = new MySqlParameter(ParamName, (MySqlDbType)DbType, Size);
            else
                param = new MySqlParameter(ParamName, (MySqlDbType)DbType);

            return param;
        }

        public bool IsFullTextSearchEnabled()
        {
            return false;
        }

        public bool IsCompactDatabase()
        {
            return false;
        }

        public bool IsBackupDatabase()
        {
            return false;
        }

        public string GetLastIdSql()
        {
            return "SELECT LAST_INSERT_ID()";
        }


        public bool IsDbOptimize()
        {

            return true;
        }

        public bool IsShrinkData()
        {


            return false;
        }


        public bool IsStoreProc()
        {

            return false;
        }
    }
}
