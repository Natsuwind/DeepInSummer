using System;
using System.Data;
using System.Data.Common;
using LiteCMS.Entity;

namespace LiteCMS.Data
{
    public partial interface IDataProvider
    {
        void CreateAttachment(AttachmentInfo info);
        IDataReader GetAttachments(string filenames);
        IDataReader GetAttachments(int posterid, int pagesize, int currentpage);
    }
}
