using System;
using System.Web.Caching;

namespace Natsuhime
{
    public class TinyCache
    {
        private static System.Web.Caching.Cache webcache = System.Web.HttpRuntime.Cache;

        private bool _IsWriteLogs = true;
        /// <summary>
        /// �Ƿ��¼���浽��־�ļ�(Ĭ����)
        /// </summary>
        public bool IsWriteLogs
        {
            get { return _IsWriteLogs; }
            set { _IsWriteLogs = value; }
        }

        /// <summary>
        /// ���뵱ǰ���󵽻�����
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        /// <param name="TimeOut">����ʱ��(0��ʾ������)</param>
        public void AddObject(string CacheName, object o, int TimeOut)
        {

            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// ���뵱ǰ���󵽻�����,��������ļ���������
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        /// <param name="files">���ӵ�·���ļ�</param>
        /// <param name="TimeOut">����ʱ��(0��ʾ������)</param>
        public void AddObjectWithFileChange(string CacheName, object o, string[] files, int TimeOut)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(files, DateTime.Now);
            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// ���뵱ǰ���󵽻�����,��ʹ��������
        /// </summary>
        /// <param name="CacheName">����ļ�ֵ</param>
        /// <param name="o">����Ķ���</param>
        /// <param name="dependKey">���������ļ�ֵ</param>
        /// <param name="TimeOut">����ʱ��(0��ʾ������)</param>
        public void AddObjectWithDepend(string CacheName, object o, string[] DependKey, int TimeOut)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(null, DependKey, DateTime.Now);
            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
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
            //switch (Reason)
            //{
            //    case CacheItemRemovedReason.DependencyChanged:
            //        break;
            //    case CacheItemRemovedReason.Expired:
            //        {
            //            break;
            //        }
            //    case CacheItemRemovedReason.Removed:
            //        {
            //            break;
            //        }
            //    case CacheItemRemovedReason.Underused:
            //        {
            //            break;
            //        }
            //    default: break;
            //}
            //����Ҫʹ�û�����־,����Ҫʹ���������
            //myLogVisitor.WriteLog(this,key,val,reason);
            if (this._IsWriteLogs)
            {
                string logfilepath = Common.Utils.GetMapPath(string.Format("~\\LiteCMSLogs\\{0}.txt", DateTime.Now.ToString("yyMMdd")));
                Val = Val == null ? "" : Val.ToString();
                string message = string.Format("Key:{0},Value:{1},Reason:{2}", Key, Val, Reason.ToString());
                Logs.TinyLogs.InsertLog(logfilepath, DateTime.Now, "Cache_Removed", message);
            }
        }
    }
}
