using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using Natsuhime.Data;
using LiteCMS.Entity;

namespace LiteCMS.Data.MySql
{
    public partial class DataProvider : IDataProvider
    {
        public void CreateAttachment(AttachmentInfo info)
        {
            DbParameter[] sqlparams =
            {
                DbHelper.MakeInParam("?filename", (DbType)MySqlDbType.String, 100, info.Filename), 
                DbHelper.MakeInParam("?filepath", (DbType)MySqlDbType.String, 100, info.Filepath), 
                DbHelper.MakeInParam("?filetype", (DbType)MySqlDbType.Int32, 4, info.Filetype), 
                DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, info.Posterid), 
                DbHelper.MakeInParam("?description", (DbType)MySqlDbType.String, 100, info.Description)
            };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_attachments(filename,filepath,filetype,posterid,description) VALUES(?filename,?filepath,?filetype,?posterid,?description)", sqlparams);
        }

        [Obsolete("此方法是危险的,请在调用前检查attachmentids是否为数字用逗号分割的.避免SQL注入")]
        public IDataReader GetAttachments(string filenames)
        {
            IDataReader dr;
            //DbParameter[] sqlparams =
            //{
            //    DbHelper.MakeInParam("", (DbType)MySqlDbType.String, 100, attachmentids)
            //};
            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_attachments WHERE filename IN (" + filenames + ")");
            return dr;
        }

        public IDataReader GetAttachments(int posterid, int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;
            DbParameter[] sqlparams =
            {
                DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4, posterid),
                DbHelper.MakeInParam("?recordoffset", (DbType)MySqlDbType.Int32, 4, recordoffset),
                DbHelper.MakeInParam("?pagesize", (DbType)MySqlDbType.Int32, 4, pagesize),
            };

            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_attachments WHERE posterid=?posterid ORDER BY attachmentid DESC LIMIT ?recordoffset,?pagesize", sqlparams);
            return dr;
        }
    }
}
