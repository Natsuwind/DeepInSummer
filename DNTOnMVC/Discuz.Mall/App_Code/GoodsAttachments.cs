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
    /// 商品附件管理操作类
    /// </summary>
    public class GoodsAttachments
    {
        /// <summary>
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类数组</param>
        /// <returns>附件id数组</returns>
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
        /// 绑定商品附件数组中的参数，返回无效商品附件个数
        /// </summary>
        /// <param name="attachmentinfo">附件类型</param>
        /// <param name="goodsid">商品id</param>
        /// <param name="msg">原有提示信息</param>
        /// <param name="categoryid">商品分类id</param>
        /// <param name="userid">用户id</param>
        /// <returns>无效商品附件个数</returns>
        public static int BindAttachment(Goodsattachmentinfo[] attachmentInfo, int goodsId, StringBuilder msg, int categoryId, int userId)
        {
            int acount = attachmentInfo.Length;
            // 附件查看权限
            string[] readperm = DNTRequest.GetString("readperm") == null ? null : DNTRequest.GetString("readperm").Split(',');
            string[] attachdesc = DNTRequest.GetString("attachdesc") == null ? null : DNTRequest.GetString("attachdesc").Split(',');
            string[] localid = DNTRequest.GetString("localid") == null ? null : DNTRequest.GetString("localid").Split(',');

            //设置无效附件计数器
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
        /// 过滤临时内容中的本地临时标签
        /// </summary>
        /// <param name="aid">广告id</param>
        /// <param name="attachmentinfo">附件信息列表</param>
        /// <param name="tempMessage">临时信息内容</param>
        /// <returns>过滤结果</returns>
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
        /// 获取指定商品id的所有附件信息
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <returns>附件信息集合</returns>
        public static GoodsattachmentinfoCollection GetGoodsAttachmentsByGoodsid(int goodsId)
        {
            return DTO.GetGoodsAttachmentInfoList(DbProvider.GetInstance().GetGoodsAttachmentsByGoodsid(goodsId));
        }

        /// <summary>
        /// 获取指定附件id的相关附件信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>附件信息</returns>
        public static Goodsattachmentinfo GetGoodsAttachmentsByAid(int aid)
        {
            return DTO.GetGoodsattachmentInfo(DbProvider.GetInstance().GetGoodsAttachmentsByAid(aid));
        }

        /// <summary>
        /// 保存附件信息
        /// </summary>
        /// <param name="goodsattachmentinfo">要保存的附件信息</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveGoodsAttachment(Goodsattachmentinfo goodsAttachmentInfo)
        {
            return DbProvider.GetInstance().SaveGoodsAttachment(goodsAttachmentInfo);
        }

        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="aid">附件Id</param>
        /// <param name="readperm">阅读权限</param>
        /// <param name="description">描述</param>
        /// <returns>返回被更新的数量</returns>
        public static bool SaveGoodsAttachment(int aid, int readPerm, string description)
        {
            return DbProvider.GetInstance().SaveGoodsAttachment(aid, readPerm, description);
        }

        /// <summary>
        /// 删除指定附件id列表的附件信息及其物理文件
        /// </summary>
        /// <param name="aidList">附件id列表</param>
        /// <returns>删除附件数</returns>
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
        /// 删除指定附件id的附件信息及其物理文件
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>删除附件数</returns>
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
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得商品附件信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品附件信息</returns>
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
            /// 获得商品信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品信息</returns>
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
            /// 获得商品附件信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品附件信息</returns>
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
