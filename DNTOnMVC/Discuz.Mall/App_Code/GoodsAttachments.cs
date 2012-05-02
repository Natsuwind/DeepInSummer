using System;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Config;
using Discuz.Mall.Data;
using Discuz.Entity;

namespace Discuz.Mall
{
    /// <summary>
    /// ��Ʒ�������������
    /// </summary>
    public class GoodsAttachments
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="attachmentinfo">��������������</param>
        /// <returns>����id����</returns>
        public static int[] CreateAttachments(Goodsattachmentinfo[] attachmentInfo)
        {
            int acount = attachmentInfo.Length;
            int icount = 0;
            int[] aid = new int[acount];
            for (int i = 0; i < acount; i++)
            {
                if (attachmentInfo[i] != null && attachmentInfo[i].Sys_noupload.Equals(""))
                {
                    aid[i] = DbProvider.GetInstance().CreateGoodsAttachment(attachmentInfo[i]);
                    icount++;
                }
            }
            return aid;
        }

      
        /// <summary>
        /// ����Ʒ���������еĲ�����������Ч��Ʒ��������
        /// </summary>
        /// <param name="attachmentinfo">��������</param>
        /// <param name="goodsid">��Ʒid</param>
        /// <param name="msg">ԭ����ʾ��Ϣ</param>
        /// <param name="categoryid">��Ʒ����id</param>
        /// <param name="userid">�û�id</param>
        /// <returns>��Ч��Ʒ��������</returns>
        public static int BindAttachment(Goodsattachmentinfo[] attachmentInfo, int goodsId, StringBuilder msg, int categoryId, int userId)
        {
            int acount = attachmentInfo.Length;
            // �����鿴Ȩ��
            string[] readperm = DNTRequest.GetString("readperm") == null ? null : DNTRequest.GetString("readperm").Split(',');
            string[] attachdesc = DNTRequest.GetString("attachdesc") == null ? null : DNTRequest.GetString("attachdesc").Split(',');
            string[] localid = DNTRequest.GetString("localid") == null ? null : DNTRequest.GetString("localid").Split(',');

            //������Ч����������
            int errorAttachment = 0;
            for (int i = 0; i < acount; i++)
            {
                if (attachmentInfo[i] != null)
                {
                    if (Utils.IsNumeric(localid[i + 1]))
                        attachmentInfo[i].Sys_index = TypeConverter.ObjectToInt(localid[i + 1]);

                    attachmentInfo[i].Uid = userId;
                    attachmentInfo[i].Goodsid = goodsId;
                    attachmentInfo[i].Categoryid = categoryId;
                    attachmentInfo[i].Postdatetime = Utils.GetDateTime(); ;
                    
                    if (attachdesc != null && !attachdesc[i + 1].Equals(""))
                        attachmentInfo[i].Description = Utils.HtmlEncode(attachdesc[i + 1]);

                    if (!attachmentInfo[i].Sys_noupload.Equals(""))
                    {
                        msg.Append("<tr><td align=\"left\">");
                        msg.Append(attachmentInfo[i].Attachment);
                        msg.Append("</td>");
                        msg.Append("<td align=\"left\">");
                        msg.Append(attachmentInfo[i].Sys_noupload);
                        msg.Append("</td></tr>");
                        errorAttachment++;
                    }
                }
            }
            return errorAttachment;
        }

        /// <summary>
        /// ������ʱ�����еı�����ʱ��ǩ
        /// </summary>
        /// <param name="aid">���id</param>
        /// <param name="attachmentinfo">������Ϣ�б�</param>
        /// <param name="tempMessage">��ʱ��Ϣ����</param>
        /// <returns>���˽��</returns>
        public static string FilterLocalTags(int[] aid, Goodsattachmentinfo[] attachmentInfo, string tempMessage)
        {
            Match m;
            Regex r;
            for (int i = 0; i < aid.Length; i++)
            {
                if (aid[i] > 0)
                {
                    r = new Regex(@"\[localimg=(\d{1,}),(\d{1,})\]" + attachmentInfo[i].Sys_index + @"\[\/localimg\]", RegexOptions.IgnoreCase);
                    for (m = r.Match(tempMessage); m.Success; m = m.NextMatch())
                    {
                        tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attachimg]" + aid[i] + "[/attachimg]");
                    }

                    r = new Regex(@"\[local\]" + attachmentInfo[i].Sys_index + @"\[\/local\]", RegexOptions.IgnoreCase);
                    for (m = r.Match(tempMessage); m.Success; m = m.NextMatch())
                    {
                        tempMessage = tempMessage.Replace(m.Groups[0].ToString(), "[attach]" + aid[i] + "[/attach]");
                    }
                }
            }

            tempMessage = Regex.Replace(tempMessage, @"\[localimg=(\d{1,}),\s*(\d{1,})\][\s\S]+?\[/localimg\]", string.Empty, RegexOptions.IgnoreCase);
            tempMessage = Regex.Replace(tempMessage, @"\[local\][\s\S]+?\[/local\]", string.Empty, RegexOptions.IgnoreCase);
            return tempMessage;
        }

        /// <summary>
        /// ��ȡָ����Ʒid�����и�����Ϣ
        /// </summary>
        /// <param name="goodsid">��Ʒid</param>
        /// <returns>������Ϣ����</returns>
        public static GoodsattachmentinfoCollection GetGoodsAttachmentsByGoodsid(int goodsId)
        {
            return DTO.GetGoodsAttachmentInfoList(DbProvider.GetInstance().GetGoodsAttachmentsByGoodsid(goodsId));
        }

        /// <summary>
        /// ��ȡָ������id����ظ�����Ϣ
        /// </summary>
        /// <param name="aid">����id</param>
        /// <returns>������Ϣ</returns>
        public static Goodsattachmentinfo GetGoodsAttachmentsByAid(int aid)
        {
            return DTO.GetGoodsattachmentInfo(DbProvider.GetInstance().GetGoodsAttachmentsByAid(aid));
        }

        /// <summary>
        /// ���渽����Ϣ
        /// </summary>
        /// <param name="goodsattachmentinfo">Ҫ����ĸ�����Ϣ</param>
        /// <returns>�Ƿ񱣴�ɹ�</returns>
        public static bool SaveGoodsAttachment(Goodsattachmentinfo goodsAttachmentInfo)
        {
            return DbProvider.GetInstance().SaveGoodsAttachment(goodsAttachmentInfo);
        }

        /// <summary>
        /// ���¸�����Ϣ
        /// </summary>
        /// <param name="aid">����Id</param>
        /// <param name="readperm">�Ķ�Ȩ��</param>
        /// <param name="description">����</param>
        /// <returns>���ر����µ�����</returns>
        public static bool SaveGoodsAttachment(int aid, int readPerm, string description)
        {
            return DbProvider.GetInstance().SaveGoodsAttachment(aid, readPerm, description);
        }

        /// <summary>
        /// ɾ��ָ������id�б�ĸ�����Ϣ���������ļ�
        /// </summary>
        /// <param name="aidList">����id�б�</param>
        /// <returns>ɾ��������</returns>
        public static int DeleteGoodsAttachment(string aidList)
        {
            GoodsattachmentinfoCollection goodsAttchmentInfoColl = DTO.GetGoodsAttachmentInfoList(DbProvider.GetInstance().GetGoodsAttachmentListByAidList(aidList));
            int goodsId = 0;
            if (goodsAttchmentInfoColl != null)
            {
                foreach(Goodsattachmentinfo goodsattachmentinfo in goodsAttchmentInfoColl)
                {
                    if (goodsattachmentinfo.Filename.Trim().ToLower().IndexOf("http") < 0)
                    {
                         string attachmentFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + goodsattachmentinfo.Filename);
                         if (Utils.FileExists(attachmentFilePath))
                             File.Delete(attachmentFilePath);
                    }
                    goodsId = Utils.StrToInt(goodsattachmentinfo.Goodsid, 0);
                }
            }
            return DbProvider.GetInstance().DeleteGoodsAttachment(aidList);
        }

        /// <summary>
        /// ɾ��ָ������id�ĸ�����Ϣ���������ļ�
        /// </summary>
        /// <param name="aid">����id</param>
        /// <returns>ɾ��������</returns>
        public static bool DeleteGoodsAttachment(int aid)
        {
            Goodsattachmentinfo goodsAttachmentInfo = GetGoodsAttachmentsByAid(aid);
            if (goodsAttachmentInfo != null)
            {
                if (goodsAttachmentInfo.Filename.ToLower().IndexOf("http") < 0)
                {
                    string attachmentFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + goodsAttachmentInfo.Filename);
                    if (Utils.FileExists(attachmentFilePath))
                        File.Delete(attachmentFilePath);
                }
            }
            return Discuz.Data.DatabaseProvider.GetInstance().DeleteAttachment(aid) > 0 ? true : false;
        }


        /// <summary>
        /// ����ת��������
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// �����Ʒ������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>������Ʒ������Ϣ</returns>
            public static Goodsattachmentinfo GetGoodsattachmentInfo(IDataReader reader)
            {
                if (reader.Read())
                {
                    Goodsattachmentinfo goodsAttachmentsInfo = LoadGoodsAttachmentinfo(reader);

                    reader.Close();
                    return goodsAttachmentsInfo;
                }
                return null;
            }

            /// <summary>
            /// �����Ʒ��Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>������Ʒ��Ϣ</returns>
            public static GoodsattachmentinfoCollection GetGoodsAttachmentInfoList(IDataReader reader)
            {
                GoodsattachmentinfoCollection goodsAttachmentInfoColl = new GoodsattachmentinfoCollection();
                while (reader.Read())
                {
                    Goodsattachmentinfo goodsattachmentinfo = LoadGoodsAttachmentinfo(reader);
                    goodsAttachmentInfoColl.Add(goodsattachmentinfo);
                }
                reader.Close();
                return goodsAttachmentInfoColl;
            }

            #region Helper
            private static Goodsattachmentinfo LoadGoodsAttachmentinfo(IDataReader reader)
            {
                Goodsattachmentinfo goodsAttachmentInfo = new Goodsattachmentinfo();
                goodsAttachmentInfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
                goodsAttachmentInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                goodsAttachmentInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                goodsAttachmentInfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                goodsAttachmentInfo.Postdatetime = reader["postdatetime"].ToString();
                goodsAttachmentInfo.Filename = reader["filename"].ToString().Trim();
                goodsAttachmentInfo.Description = reader["description"].ToString().Trim();
                goodsAttachmentInfo.Filetype = reader["filetype"].ToString().Trim();
                goodsAttachmentInfo.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
                goodsAttachmentInfo.Attachment = reader["attachment"].ToString().Trim();
                return goodsAttachmentInfo;
            }
            #endregion

            /// <summary>
            /// �����Ʒ������Ϣ(DTO)
            /// </summary>
            /// <param name="dt">Ҫת�������ݱ�</param>
            /// <returns>������Ʒ������Ϣ</returns>
            public static Goodsattachmentinfo[] GetGoodsattachmentArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodsattachmentinfo[] goodsAttachmentsInfoArray = new Goodsattachmentinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsAttachmentsInfoArray[i] = new Goodsattachmentinfo();
                    goodsAttachmentsInfoArray[i].Aid = TypeConverter.ObjectToInt(dt.Rows[i]["aid"]);
                    goodsAttachmentsInfoArray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
                    goodsAttachmentsInfoArray[i].Goodsid = TypeConverter.ObjectToInt(dt.Rows[i]["goodsid"]);
                    goodsAttachmentsInfoArray[i].Categoryid = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
                    goodsAttachmentsInfoArray[i].Postdatetime = dt.Rows[i]["postdatetime"].ToString();
                    goodsAttachmentsInfoArray[i].Filename = dt.Rows[i]["filename"].ToString();
                    goodsAttachmentsInfoArray[i].Description = dt.Rows[i]["description"].ToString();
                    goodsAttachmentsInfoArray[i].Filetype = dt.Rows[i]["filetype"].ToString();
                    goodsAttachmentsInfoArray[i].Filesize = TypeConverter.ObjectToInt(dt.Rows[i]["filesize"]);
                    goodsAttachmentsInfoArray[i].Attachment = dt.Rows[i]["attachment"].ToString();
                }
                dt.Dispose();
                return goodsAttachmentsInfoArray;
            }
        }
    }
}
