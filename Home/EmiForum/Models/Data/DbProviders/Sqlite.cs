using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Natsuhime.Data.DbProviders
{
    public class Sqlite : IDbProvider
    {
        public DbProviderFactory Instance()
        {
            return SQLiteFactory.Instance;
        }

        public void DeriveParameters(IDbCommand cmd)
        {
            if ((cmd as SQLiteCommand) != null)
            {
                throw new Exception("Sqlite Database DO NOT support StoreProc");
                //SQLiteCommandBuilder.DeriveParameters(cmd as SQLiteCommand);
            }
        }

        public DbParameter MakeParam(string ParamName, DbType DbType, Int32 Size)
        {
            SQLiteParameter param;

            if (Size > 0)
                param = new SQLiteParameter(ParamName, DbType, Size);
            else
                param = new SQLiteParameter(ParamName, DbType);

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
            return "SELECT SCOPE_IDENTITY()";
        }

        public bool IsDbOptimize()
        {

            return false;
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
