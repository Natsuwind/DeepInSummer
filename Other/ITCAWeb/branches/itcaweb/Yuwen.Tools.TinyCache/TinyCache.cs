using System;
using System.Web.Caching;

namespace Yuwen.Tools.TinyCache
{
    public class TinyCache
    {
        private int m_TimeOut = 1440;//Ĭ�ϻ�������Ϊ1440����(24Сʱ)

        /// <summary>
        /// ���õ������ʱ��[��λ��������]
        /// </summary>
        public int TimeOut
        {
            get { return m_TimeOut; }
            set { m_TimeOut = value > 0 ? value : 1440; }
        }


        private static System.Web.Caching.Cache webcache = System.Web.HttpRuntime.Cache;


        /// <summary>
        /// ���뵱ǰ���󵽻�����
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        public void AddObject(string CacheName, object o)
        {

            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            if (TimeOut == 6000)
            {
                webcache.Insert(CacheName, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// ���뵱ǰ���󵽻�����,��������ļ���������
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        /// <param name="files">���ӵ�·���ļ�</param>
        public void AddObjectWithFileChange(string CacheName, object o, string[] files)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(files, DateTime.Now);

            webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddHours(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
        }

        /// <summary>
        /// ���뵱ǰ���󵽻�����,��ʹ��������
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        /// <param name="dependKey">���������ļ�ֵ</param>
        public void AddObjectWithDepend(string CacheName, object o, string[] DependKey)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(null, DependKey, DateTime.Now);

            webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
        }



        /// <summary>
        /// ɾ���������
        /// </summary>
        /// <param name="CacheName">����Ĺؼ���</param>
        public void RemoveObject(string CacheName)
        {
            if (CacheName == null || CacheName.Length == 0)
            {
                return;
            }
            webcache.Remove(CacheName);
        }


        /// <summary>
        /// ����һ��ָ���Ķ���
        /// </summary>
        /// <param name="CacheName">����Ĺؼ���</param>
        /// <returns>����</returns>
        public object RetrieveObject(string CacheName)
        {
            if (CacheName == null || CacheName.Length == 0)
            {
                return null;
            }

            return webcache.Get(CacheName);
        }



        /// <summary>
        /// ����ʧЧ�ص�ί�е�һ��ʵ��,�Ժ�������־��¼
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        public void CacheOnRemove(string Key, object Val, CacheItemRemovedReason Reason)
        {
            switch (Reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Removed:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Underused:
                    {
                        break;
                    }
                default: break;
            }
            //����Ҫʹ�û�����־,����Ҫʹ���������
            //myLogVisitor.WriteLog(this,key,val,reason);

        }
    }
}
