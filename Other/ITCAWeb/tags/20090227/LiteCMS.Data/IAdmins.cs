using System;
using System.Data;
using System.Data.Common;
using LiteCMS.Entity;

namespace LiteCMS.Data
{
    public partial interface IDataProvider
    {
        IDataReader GetAdmins(int pagesize, int currentpage);
        int GetAdminCollectionPageCount(int pagesize);

        IDataReader GetAdminInfo(string name, string password);
        IDataReader GetAdminInfo(int adminid, string password);
        IDataReader GetAdminInfo(int adminid);


        void AddAdmin(AdminInfo info);
        void EditAdmin(AdminInfo info);
    }
}
