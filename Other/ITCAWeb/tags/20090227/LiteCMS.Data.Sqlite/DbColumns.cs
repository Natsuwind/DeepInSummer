using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Natsuhime.Data;
using LiteCMS.Entity;

namespace LiteCMS.Data.Sqlite
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetArticleColumnList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_columns WHERE del=0 ORDER BY [columnid]");
        }
        public void CreateColumn(ColumnInfo columninfo)
        {
            DbParameter[] prams = 
		    {
			    //DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columninfo.Columnid),
			    DbHelper.MakeInParam("@columnname", DbType.String, 50,columninfo.Columnname),
			    DbHelper.MakeInParam("@parentid", DbType.Int32, 4,columninfo.Parentid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_columns(columnname,parentid) VALUES(@columnname,@columnname)", prams);
        }
        public void DeleteColumn(int columnid)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columnid)
		    };
            //DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM wy_columns WHERE columnid=@columnid", prams);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_columns SET del=1 WHERE columnid=@columnid", prams);
        }
        public void EditColumn(ColumnInfo columninfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnname", DbType.String, 100,columninfo.Columnname),
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columninfo.Columnid),
			    DbHelper.MakeInParam("@parentid", DbType.Int32, 4,columninfo.Parentid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_columns SET columnname=@columnname,parentid=@parentid WHERE columnid=@columnid", prams);
        }
    }
}
