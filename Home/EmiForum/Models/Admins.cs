using System;
using System.Collections.Generic;
using Natsuhime.Data;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace EmiForum.Models
{
    public class Admins
    {
        public static int ExecSql(string SqlScript)
        {
            return DbHelper.ExecuteNonQuery(SqlScript);
        }

        public static bool IsLogined()
        {
            return false;
        }
    }
}