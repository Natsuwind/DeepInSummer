using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;


namespace Natsuhime.Proxy
{
    /////////////////////////////////////////////////////////////
    #region ProxyValidater2 的实现

    public delegate void ProgressChangedEventHandler(
        ProgressChangedEventArgs e);

    public delegate void StatusChangedEventHandler(
        Natsuhime.Events.MessageEventArgs e);

    public delegate void CompletedEventHandler(
        object sender,
        CompletedEventArgs e);

    // 这个类实现了基于事件的异步模式
    // 异步链接代理验证页面验证代理是否有效
    public class ProxyValidater : Component
    {
        private delegate void WorkerEventHandler(
            ref List<ProxyInfo> proxyList,
            AsyncOperation asyncOp);

        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        private SendOrPostCallback onStatusChangeDelegate;

        private HybridDictionary userStateToLifetime =
            new HybridDictionary();

        private System.ComponentModel.Container components = null;

        /////////////////////////////////////////////////////////////
        #region 公共事件

        public event ProgressChangedEventHandler ProgressChanged;
        public event CompletedEventHandler Completed;
        public event StatusChangedEventHandler StatusChanged;

        #endregion

        /////////////////////////////////////////////////////////////
        #region 构造和销毁

        public ProxyValidater(ProxyValidateUrlInfo validatePageInfo, IContainer container)
            : this(validatePageInfo)
        {
            container.Add(this);
        }

        public ProxyValidater(ProxyValidateUrlInfo validatePageInfo)
        {
            if (validatePageInfo == null)
            {
                throw new ArgumentNullException("validatePageInfo");
            }
            _ProxyValidateUrlInfo = validatePageInfo;

            InitializeComponent();

            InitializeDelegates();
        }

        protected virtual void InitializeDelegates()
        {
            onProgressReportDelegate =
                new SendOrPostCallback(ReportProgress);
            onCompletedDelegate =
                new SendOrPostCallback(CalculateCompleted);
            onStatusChangeDelegate =
                new SendOrPostCallback(SendStatusChange);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        /////////////////////////////////////////////////////////////
        ///
        #region 实现

        // 这个方法开始一个异步的计算 
        // 首先,它检查提供的taskId是否为unique的,如果是,就创建一个新的 WorkerEventHandler,并调用BeginInvoke方法开始计算
        public virtual void ValidateAsync(
            ref List<ProxyInfo> proxyList,
            object taskId)
        {
            // 为taskId创建一个AsyncOperation对象
            AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }

            // 开始异步操作
            WorkerEventHandler workerDelegate = new WorkerEventHandler(CalculateWorker);
            workerDelegate.BeginInvoke(
                ref proxyList,
                asyncOp,
                null,
                null);
        }

        // Utility method for determining if a 
        // task has been canceled.
        private bool TaskCanceled(object taskId)
        {
            return (userStateToLifetime[taskId] == null);
        }

        // 这个方法用于取消执行中的异步操作
        public void CancelAsync(object taskId)
        {
            AsyncOperation asyncOp = userStateToLifetime[taskId] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(taskId);
                }
            }
        }

        // 这个方法提供实际的计算流程.它在worker线程上被执行.
        private void CalculateWorker(
            ref List<ProxyInfo> proxyList,
            AsyncOperation asyncOp)
        {
            _ProxyList = proxyList;
            _ProxyListOK = new List<ProxyInfo>();
            Exception e = null;

            // 检查taskId是否被取消.因为操作可能已经在之前被预先取消了.
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    string currentIP = ProxyUtility.GetCurrentIP_RegexPage(ConnectValidatePage(asyncOp, null), _ProxyValidateUrlInfo.RegexString);
                    Natsuhime.Events.MessageEventArgs me = new Natsuhime.Events.MessageEventArgs("", string.Format("[校验]{0}[当前IP]", currentIP), "", asyncOp.UserSuppliedState);
                    asyncOp.Post(this.onStatusChangeDelegate, me);
                    Validate(currentIP, asyncOp);
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            this.CompletionMethod(
                _ProxyListOK,
                e,
                TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);
        }

        #region 具体方法
        Httper httper;
        ProxyValidateUrlInfo _ProxyValidateUrlInfo;
        List<ProxyInfo> _ProxyList;
        List<ProxyInfo> _ProxyListOK;
        private void Validate(string currentIP, AsyncOperation asyncOp)
        {
            if (currentIP == null || currentIP.Trim() == string.Empty)
            {
                throw new ArgumentNullException("currentIP");
            }

            Natsuhime.Events.MessageEventArgs e = null;
            while (true)
            {
                ProxyInfo info = GetProxy();
                if (info == null)
                {
                    break;
                }
                e = new Natsuhime.Events.MessageEventArgs("", string.Format("[校验]{0}：{1}", info.Address, info.Port), "", asyncOp.UserSuppliedState);
                asyncOp.Post(this.onStatusChangeDelegate, e);

                string returnData;
                try
                {
                    returnData = ConnectValidatePage(asyncOp, info);
                }
                catch (Exception ex)
                {
                    returnData = string.Empty;
                }
                if (returnData == string.Empty)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}：{1} - Failed", info.Address, info.Port));

                    e = new Natsuhime.Events.MessageEventArgs("", string.Format("[失败]{0}：{1}", info.Address, info.Port), "", asyncOp.UserSuppliedState);
                    asyncOp.Post(this.onStatusChangeDelegate, e);
                    continue;
                }
                if (currentIP == ProxyUtility.GetCurrentIP_RegexPage(returnData, _ProxyValidateUrlInfo.RegexString))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}：{1} - Bad", info.Address, info.Port));

                    e = new Natsuhime.Events.MessageEventArgs("", string.Format("[透明]{0}：{1}", info.Address, info.Port), "", asyncOp.UserSuppliedState);
                    asyncOp.Post(this.onStatusChangeDelegate, e);
                    continue;
                }

                Monitor.Enter(_ProxyListOK);
                _ProxyListOK.Add(info);
                Monitor.Exit(_ProxyListOK);
                System.Diagnostics.Debug.WriteLine(string.Format("{0}：{1} - OK", info.Address, info.Port));

                e = new Natsuhime.Events.MessageEventArgs("", string.Format("[成功]{0}：{1}", info.Address, info.Port), "", asyncOp.UserSuppliedState);
                asyncOp.Post(this.onStatusChangeDelegate, e);
            }
        }

        private string ConnectValidatePage(AsyncOperation asyncOp, ProxyInfo info)
        {
            Natsuhime.Events.MessageEventArgs e = null;
            if (info != null)
            {
                try
                {
                    httper.Proxy = new System.Net.WebProxy(info.Address, info.Port);
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}：{1}", info.Address, info.Port));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write("error uri" + info.Address + "-" + info.Port);
                    e = new Natsuhime.Events.MessageEventArgs("", string.Format("[代理错误:{2}]{0}：{1}", info.Address, info.Port, ex.Message), "", asyncOp.UserSuppliedState);
                    asyncOp.Post(this.onStatusChangeDelegate, e);
                }
            }

            httper.Url = _ProxyValidateUrlInfo.Url;
            httper.Charset = _ProxyValidateUrlInfo.Charset;
            return httper.HttpGet();
        }

        private ProxyInfo GetProxy()
        {
            Monitor.Enter(_ProxyList);
            ProxyInfo info;
            if (_ProxyList.Count > 0)
            {
                info = _ProxyList[0];
                _ProxyList.Remove(info);
            }
            else
            {
                info = null;
            }
            Monitor.Exit(_ProxyList);
            return info;
        }
        #endregion

        // 这个方法通过AsyncOperation对象被invoked,所以它肯定能被执行在正确的线程上.
        // This method is invoked via the AsyncOperation object,so it is guaranteed to be executed on the correct thread.
        private void CalculateCompleted(object operationState)
        {
            CompletedEventArgs e =
                operationState as CompletedEventArgs;

            OnCalculatePrimeCompleted(e);
        }

        // 这个方法通过AsyncOperation对象被invoked,所以它肯定能被执行在正确的线程上.
        // This method is invoked via the AsyncOperation object,so it is guaranteed to be executed on the correct thread.
        private void ReportProgress(object state)
        {
            ProgressChangedEventArgs e =
                state as ProgressChangedEventArgs;

            OnProgressChanged(e);
        }

        private void SendStatusChange(object state)
        {
            Natsuhime.Events.MessageEventArgs e =
                state as Natsuhime.Events.MessageEventArgs;

            onStatusChanged(e);
        }

        protected void OnCalculatePrimeCompleted(
            CompletedEventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
            }
        }

        protected void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(e);
            }
        }

        protected void onStatusChanged(Natsuhime.Events.MessageEventArgs e)
        {
            if (StatusChanged != null)
            {
                StatusChanged(e);
            }
        }

        // This is the method that the underlying, free-threaded 
        // asynchronous behavior will invoke.  This will happen on
        // an arbitrary thread.
        private void CompletionMethod(
            List<ProxyInfo> proxylistOk,
            Exception exception,
            bool canceled,
            AsyncOperation asyncOp)
        {
            // If the task was not previously canceled,
            // remove the task from the lifetime collection.
            if (!canceled)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }

            // Package the results of the operation in a 
            // CalculatePrimeCompletedEventArgs.
            CompletedEventArgs e =
                new CompletedEventArgs(
                proxylistOk,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            // End the task. The asyncOp object is responsible 
            // for marshaling the call.
            asyncOp.PostOperationCompleted(onCompletedDelegate, e);

            // Note that after the call to OperationCompleted, 
            // asyncOp is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }


        #endregion

        /////////////////////////////////////////////////////////////
        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            httper = new Httper();
        }

        #endregion

    }

    public class CalculatePrimeProgressChangedEventArgs :
            ProgressChangedEventArgs
    {
        private int latestPrimeNumberValue = 1;

        public CalculatePrimeProgressChangedEventArgs(
            int latestPrime,
            int progressPercentage,
            object userToken)
            : base(progressPercentage, userToken)
        {
            this.latestPrimeNumberValue = latestPrime;
        }

        public int LatestPrimeNumber
        {
            get
            {
                return latestPrimeNumberValue;
            }
        }
    }

    public class CompletedEventArgs :
        AsyncCompletedEventArgs
    {
        public List<ProxyInfo> ProxyList { get; set; }
        public CompletedEventArgs(
            List<ProxyInfo> proxylistOk,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.ProxyList = proxylistOk;
        }
    }


    #endregion
}