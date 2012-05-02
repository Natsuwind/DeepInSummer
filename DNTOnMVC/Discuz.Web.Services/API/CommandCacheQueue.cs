using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Discuz.Web.Services.API
{
    /// <summary>
    /// 任务缓存队列类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandCacheQueue<T>
    {
        private static Queue<T> _comQueue = new Queue<T>();

        public const int queueLength = 400;

        public static bool EnQueue(T item)
        {
            while (_comQueue.Count > queueLength)
            {
                _comQueue.Dequeue();
            }

            if (_comQueue.Contains(item))
                return false;
            _comQueue.Enqueue(item);
            return true;
        }

        #region high speed more memory

        //private static Hashtable ht = new Hashtable();

        ///// <summary>
        ///// 将发主题缓存信息入队,若队列中已存在相同信息,返回fasle,入队成功返回true
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public static bool EnQueue(TopicItem item)
        //{
        //    if (ht.Contains(item))
        //        return false;

        //    while (_comQ.Count > 200)
        //    {
        //        TopicItem ti = _comQ.Dequeue();
        //        ht.Remove(ti.Uid + ti.ContextHash);
        //    }
        //    ht.Add(item.Uid + item.ContextHash,  item);
        //    _comQ.Enqueue(item);
        //    return true;           
        //}

        #endregion
    }

    /// <summary>
    /// 发送主题缓存信息
    /// </summary>
    public class TopicItem
    {
        public int Uid;

        public int ContextHash = 0;

        public TopicItem(int uid, int contextHash)
        {
            Uid = uid;
            ContextHash = contextHash;
        }

        public override bool Equals(object obj)
        {
            TopicItem objj = obj as TopicItem;
            if (objj == null)
                return false;
            return this.Uid == objj.Uid && this.ContextHash == objj.ContextHash;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 发送主题和帖子的时间缓存信息
    /// </summary>
    public class PostTimeItem
    {
        public int Uid;

        public long Ticks;

        public PostTimeItem(int uid,long ticks)
        {
            Uid = uid;
            Ticks = ticks;
        }

        public override bool Equals(object obj)
        {
            PostTimeItem objj = obj as PostTimeItem;
            if (objj == null)
                return false;
            //关联论坛发帖时间间隔
            return this.Uid == objj.Uid && this.Ticks > objj.Ticks - Discuz.Config.GeneralConfigs.GetConfig().Postinterval * 10000000;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 操作用户积分缓存信息
    /// </summary>
    public class SetExtCreditItem
    {
        public int Uid;

        public long Ticks;

        public SetExtCreditItem(int uid, long ticks)
        {
            this.Uid = uid;
            this.Ticks = ticks;
        }

        public override bool Equals(object obj)
        {
            SetExtCreditItem item = obj as SetExtCreditItem;
            if (item == null)
                return false;
            //若用户ID一致且时间戳的时间差距在15秒因,则命中
            return this.Uid == item.Uid && this.Ticks > item.Ticks - 150000000;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 发送短消息操作缓存信息
    /// </summary>
    public class SendMessageItem
    {
        public int FromId;

        public long Ticks;

        public SendMessageItem(int fromId, long ticks)
        {
            this.FromId = fromId;
            this.Ticks = ticks;
        }

        public override bool Equals(object obj)
        {
            SendMessageItem item = obj as SendMessageItem;
            if (item == null)
                return false;
            //发送用户一致,收件用户一致,时间戳差距在30秒以内,返回true
            return this.FromId == item.FromId && this.Ticks > item.Ticks - 300000000;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
