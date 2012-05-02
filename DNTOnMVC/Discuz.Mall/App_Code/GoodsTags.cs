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
    /// 商品标签
    /// </summary>
    public class GoodsTags
    {
        public const string GoodsHotTagJSONPCacheFileName = "cache\\tag\\hottags_mall_cache_jsonp.txt";

        /// <summary>
        /// 写入商品热门标签缓存文件
        /// </summary>
        /// <param name="count">数量</param>
        public static void WriteHotTagsListForGoodsJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + GoodsHotTagJSONPCacheFileName;
            List<TagInfo> tags = GetHotTagsListForGoods(count);
            Tags.WriteTagsCacheFile(filename, tags, "mallhottag_callback", true);
        }

        /// <summary>
        /// 获取指定数量的商品热门标签
        /// </summary>
        /// <param name="count">指定数量</param>
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
        /// 获取指定标签的商品数量
        /// </summary>
        /// <param name="tagid">标签id</param>
        /// <returns></returns>
        public static int GetGoodsCountWithSameTag(int tagId)
        {
            return DbProvider.GetInstance().GetGoodsCountWithSameTag(tagId);
        }

        /// <summary>
        /// 写入主题标签缓存文件
        /// </summary>
        /// <param name="tagsArray">标签数组</param>
        /// <param name="topicid">主题Id</param>
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
        /// 获取商品所包含的Tag
        /// </summary>
        /// <param name="goodsid">商品Id</param>
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
