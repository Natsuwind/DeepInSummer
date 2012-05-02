using System;
using System.Text;
using System.IO;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Mall.Data;
using Discuz.Forum;
using Discuz.Forum.ScheduledEvents;

namespace Discuz.Mall
{
    /// <summary>
    /// ��Ʒ��ǩ
    /// </summary>
    public class GoodsTags
    {
        public const string GoodsHotTagJSONPCacheFileName = "cache\\tag\\hottags_mall_cache_jsonp.txt";

        /// <summary>
        /// д����Ʒ���ű�ǩ�����ļ�
        /// </summary>
        /// <param name="count">����</param>
        public static void WriteHotTagsListForGoodsJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + GoodsHotTagJSONPCacheFileName;
            List<TagInfo> tags = GetHotTagsListForGoods(count);
            Tags.WriteTagsCacheFile(filename, tags, "mallhottag_callback", true);
        }

        /// <summary>
        /// ��ȡָ����������Ʒ���ű�ǩ
        /// </summary>
        /// <param name="count">ָ������</param>
        /// <returns></returns>
        private static List<TagInfo> GetHotTagsListForGoods(int count)
        {
            List<TagInfo> tags = new List<TagInfo>();
            IDataReader reader = DbProvider.GetInstance().GetHotTagsListForGoods(count);

            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }



        /// <summary>
        /// ��ȡָ����ǩ����Ʒ����
        /// </summary>
        /// <param name="tagid">��ǩid</param>
        /// <returns></returns>
        public static int GetGoodsCountWithSameTag(int tagId)
        {
            return DbProvider.GetInstance().GetGoodsCountWithSameTag(tagId);
        }

        /// <summary>
        /// д�������ǩ�����ļ�
        /// </summary>
        /// <param name="tagsArray">��ǩ����</param>
        /// <param name="topicid">����Id</param>
        public static void WriteGoodsTagsCacheFile(int goodsId)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/goods/magic/");
            dir.Append((goodsId / 1000 + 1));
            dir.Append("/");

            string filename = Utils.GetMapPath(dir.ToString() + goodsId + "_tags.config");

            List<TagInfo> tags = GetTagsListByGoods(goodsId);

            Tags.WriteTagsCacheFile(filename, tags, string.Empty, false);
        }


        /// <summary>
        /// ��ȡ��Ʒ��������Tag
        /// </summary>
        /// <param name="goodsid">��ƷId</param>
        /// <returns></returns>
        public static List<TagInfo> GetTagsListByGoods(int goodsid)
        {
            List<TagInfo> tags = new List<TagInfo>();
            IDataReader reader = DbProvider.GetInstance().GetTagsListByGoods(goodsid);

            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }
    }
}
