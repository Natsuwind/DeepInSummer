using System;
using System.Data;
using System.Data.Common;
using LiteCMS.Entity;

namespace LiteCMS.Data
{
    public partial interface IDataProvider
    {
        IDataReader GetArticleColumnList();    
        void CreateColumn(ColumnInfo columninfo);
        void DeleteColumn(int columnid);
        void EditColumn(ColumnInfo columninfo);
    }
}