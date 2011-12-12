using System;
using System.Data;
using System.Collections.Generic;

using LiteCMS.Data;
using LiteCMS.Entity;
using Natsuhime.Common;
using Natsuhime;

namespace LiteCMS.Core
{
    public class Attachments
    {
        static AttachmentInfo DataReader2AttachmentInfo(IDataReader reader)
        {
            AttachmentInfo info = new AttachmentInfo();
            info.Attachmentid = Convert.ToInt32(reader["attachmentid"]);
            info.Filename = reader["filename"].ToString();
            info.Filepath = reader["filepath"].ToString();
            info.Filetype = Convert.ToInt32(reader["filetype"]);
            info.Posterid = Convert.ToInt32(reader["posterid"]);
            info.Description = reader["description"].ToString();
            return info;
        }

        public static void CreateAttachment(AttachmentInfo info)
        {
            if (info != null)
            {
                DatabaseProvider.GetInstance().CreateAttachment(info);
            }
        }
        public static List<AttachmentInfo> GetAttachmentList(string filenames)
        {
            List<AttachmentInfo> coll = new List<AttachmentInfo>();
            if (TypeParse.IsNumericString(filenames))
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetAttachments(filenames);
                while (reader.Read())
                {
                    coll.Add(DataReader2AttachmentInfo(reader));
                }
                reader.Close();
            }
            return null;
        }
    }
}
