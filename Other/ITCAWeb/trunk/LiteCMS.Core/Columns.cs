using System;
using System.Data;
using System.Collections.Generic;

using LiteCMS.Data;
using LiteCMS.Entity;
using Natsuhime.Common;
using Natsuhime;
using LiteCMS.Config;

namespace LiteCMS.Core
{
    public class Columns
    {        
        /// <summary>
        /// 将DataReader的Column转换为ColumnInfo泛型列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ColumnInfo DataReader2ColumnInfo(IDataReader reader)
        {
            ColumnInfo columninfo = new ColumnInfo();
            columninfo.Columnid = Convert.ToInt32(reader["columnid"]);
            columninfo.Columnname= reader["columnname"].ToString();
            columninfo.Parentid = Convert.ToInt32(reader["parentid"]);
            return columninfo;
        }
        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        private static SortedList<int, object> GetArticleColumnArray()
        {
            SortedList<int, object> columnlist = new SortedList<int,object>();
            List<ColumnInfo> coll = GetColumnCollection();
            foreach (ColumnInfo columninfo in coll)
            {
                columnlist.Add(columninfo.Columnid, columninfo.Columnname);
            }
            //TinyCache cache = new TinyCache();
            //columnlist = cache.RetrieveObject("ColumnList") as SortedList<int, object>;

            //if (columnlist == null)
            //{
            //    columnlist = new SortedList<int, object>();
            //    DataTable dt = DatabaseProvider.GetInstance().GetArticleColumnList();
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            if ((dr["columnid"].ToString() != "") && (dr["columnname"].ToString() != ""))
            //            {
            //                columnlist.Add(Convert.ToInt32(dr["columnid"]), dr["columnname"]);
            //            }
            //        }
            //    }
            //    cache.AddObject("ColumnList", columnlist);
            //}
            return columnlist;
        }
        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnCollection()
        {            
            List<ColumnInfo> coll;
            TinyCache cache = new TinyCache();
            coll = cache.RetrieveObject("ColumnList") as List<ColumnInfo>;
            if (coll == null)
            {
                coll = new List<ColumnInfo>();
                IDataReader reader = DatabaseProvider.GetInstance().GetArticleColumnList();

                while (reader.Read())
                {
                    coll.Add(DataReader2ColumnInfo(reader));
                }
                reader.Close();
                cache.AddObject("ColumnList", coll, MainConfigs.GetConfig().Globalcachetimeout);
            }
            return coll;
        }

        /// <summary>
        /// 取得栏目名称
        /// </summary>
        /// <param name="columnid"></param>
        /// <returns></returns>
        public static string GetColumnName(int columnid)
        {
            SortedList<int, object> columnarray = GetArticleColumnArray();
            object columnname = null;
            if (columnarray.ContainsKey(columnid))
            {
                columnname = columnarray[columnid];
            }
            if (columnname == null)
            {
                return "";
            }
            else
            {
                return columnname.ToString().Trim();
            }
        }
        /// <summary>
        /// 新建栏目
        /// </summary>
        /// <param name="columninfo"></param>
        public static void CreateColumn(ColumnInfo columninfo)
        {
            DatabaseProvider.GetInstance().CreateColumn(columninfo);
        }
        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="columnid"></param>
        public static void DeleteColumn(int columnid)
        {
            DatabaseProvider.GetInstance().DeleteColumn(columnid);
        }
        /// <summary>
        /// 编辑栏目
        /// </summary>
        /// <param name="columninfo"></param>
        public static void EditColumn(ColumnInfo columninfo)
        {
            DatabaseProvider.GetInstance().EditColumn(columninfo);
        }
    }
}
