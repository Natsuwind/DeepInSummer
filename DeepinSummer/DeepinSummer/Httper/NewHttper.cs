using System.IO;
using System.Net;
using System.Text;

using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Natsuhime
{
    /// <summary>
    /// 对Http协议的封装（post,get）,提供同步和异步调用的方法
    /// </summary>
    [ToolboxItem(false)]
    public class NewHttper : Component
    {
        #region 属性
        private string m_Url;
        private string m_PostData = "";
        private int m_Timeout = 130000;
        private string m_ContentType = "application/x-www-form-urlencoded";
        private string m_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private string m_Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, */*";
        private string m_Referer = "";
        private string m_X_FORWARDED_FOR = "";
        private string m_Charset = "UTF-8";
        private WebProxy m_Proxy;
        private CookieContainer m_Cookie;

        public int Name
        {
            get;
            set;
        }
        // private HttpWebRequest objHWR;

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        public string PostData
        {
            get { return m_PostData; }
            set { m_PostData = value; }
        }
        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }
        public string ContentType
        {
            get { return m_ContentType; }
            set { m_ContentType = value; }
        }
        public string UserAgent
        {
            get { return m_UserAgent; }
            set { m_UserAgent = value; }
        }
        public string Accept
        {
            get { return m_Accept; }
            set { m_Accept = value; }
        }
        public string Referer
        {
            get { return m_Referer; }
            set { m_Referer = value; }
        }
        public string X_FORWARDED_FOR
        {
            get { return m_X_FORWARDED_FOR.Trim(); }
            set { m_X_FORWARDED_FOR = value; }
        }
        public string Charset
        {
            get { return m_Charset; }
            set { m_Charset = value; }
        }
        public WebProxy Proxy
        {
            get { return m_Proxy; }
            set { m_Proxy = value; }
        }
        public CookieContainer Cookie
        {
            get { return m_Cookie; }
            set { m_Cookie = value; }
        }
        #endregion


        public delegate void RequestDataCompletedEventHandler(
            object sender,
            RequestDataCompletedEventArgs e);
        public delegate void RequestStringCompleteEventHandler(
            object sender,
            RequestStringCompletedEventArgs e
            );


        public event RequestDataCompletedEventHandler RequestDataCompleted;
        public event RequestStringCompleteEventHandler RequestStringCompleted;



        private SendOrPostCallback onRequestDataCompletedDelegate;
        private SendOrPostCallback onRequestStringCompletedDelegate;

        private delegate void WorkerEventHandler(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp);
        private HybridDictionary userStateToLifetime =
            new HybridDictionary();

        private System.ComponentModel.Container components = null;

        /////////////////////////////////////////////////////////////
        #region Construction and destruction

        public NewHttper(IContainer container)
            : this()
        {
            container.Add(this);
        }

        public NewHttper()
        {
            InitializeComponent();
            InitializeDelegates();
            //objHWR = new HttpWebRequest();

        }

        protected virtual void InitializeDelegates()
        {

            onRequestDataCompletedDelegate =
                new SendOrPostCallback(ReportRequestDataCompleted);
            onRequestStringCompletedDelegate =
            new SendOrPostCallback(ReportRequestStringCompleted);
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

        #endregion // Construction and destruction

        /////////////////////////////////////////////////////////////
        #region 异步的模式的实现
        /// <summary>
        /// Http异步请求，事件中返回byte数组
        /// </summary>
        /// <param name="requestMethod">http请求的方法</param>
        /// <returns>任务Id,唯一标识一次任务</returns>
        public virtual Guid RequestDataAsync(EnumRequestMethod requestMethod)
        {
            Guid taskId = Guid.NewGuid();
            RequestDataAsync(requestMethod, taskId);
            return taskId;

        }

        // This method starts an asynchronous calculation. 
        // First, it checks the supplied task ID for uniqueness.
        // If taskId is unique, it creates a new WorkerEventHandler 
        // and calls its BeginInvoke method to start the calculation.
        public virtual void RequestDataAsync(
            EnumRequestMethod requestMethod,
            object taskId)
        {
            //HttpWebRequest requestObj = GetRequestObj(requestMethod);
            //RequestState requestState = new RequestState();
            //requestState.request = requestObj;
            //requestState.RequestId = taskId;
            //requestState.IsRequestString = false;
            //requestState.Context = SynchronizationContext.Current;
            //if (requestMethod == EnumRequestMethod.POST)
            //{
            //    requestState.PostData = this.PostData;
            //}
            //requestState.RequestMethod = requestMethod;
            //this.BeginHttpReqeust(requestState);

            // Create an AsyncOperation for taskId.

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

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(RequestDataWorker);
            workerDelegate.BeginInvoke(
                requestMethod,
                asyncOp,
                null,
                null);

        }

        public virtual Guid RequestStringAsync(EnumRequestMethod requestMethod)
        {
            Guid taskId = Guid.NewGuid();
            RequestStringAsync(requestMethod, taskId);
            return taskId;
        }

        public virtual void RequestStringAsync(
            EnumRequestMethod requestMethod,
            object taskId)
        {

            //HttpWebRequest requestObj = GetRequestObj(requestMethod);
            //RequestState requestState = new RequestState();
            //requestState.request = requestObj;
            //requestState.RequestId = taskId;
            //requestState.IsRequestString = true;
            //requestState.Context = SynchronizationContext.Current;
            //if (requestMethod == EnumRequestMethod.POST)
            //{
            //    requestState.PostData = this.PostData;
            //}
            //requestState.RequestMethod = requestMethod;
            //this.BeginHttpReqeust(requestState);
            // Create an AsyncOperation for taskId.

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

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(RequestStringWorker);
            workerDelegate.BeginInvoke(
                requestMethod,
                asyncOp,
                null,
                null);

        }

        private HttpWebRequest GetRequestObj(EnumRequestMethod requestMethod)
        {
            HttpWebRequest objHWR = (HttpWebRequest)HttpWebRequest.Create(Url);
            objHWR.Timeout = Timeout;
            objHWR.UserAgent = UserAgent;
            objHWR.Accept = "*/*";// Accept;
            objHWR.Referer = Referer;
            objHWR.KeepAlive = true;

            if (X_FORWARDED_FOR != string.Empty)
            {
                objHWR.Headers.Set("X_FORWARDED_FOR", X_FORWARDED_FOR);
            }
            if (Proxy != null)
            {
                objHWR.Proxy = Proxy;
            }
            if (Cookie != null)
            {
                objHWR.CookieContainer = Cookie;
            }

            if (requestMethod == EnumRequestMethod.GET)
            {
                objHWR.Method = "GET";
            }
            else
            {
                objHWR.Method = "POST";
                objHWR.ContentType = this.ContentType;

                //Stream newStream = null;
                //try
                //{
                //    byte[] byteData = Encoding.ASCII.GetBytes(PostData);
                //    objHWR.ContentLength = byteData.Length;
                //    newStream = objHWR.GetRequestStream();
                //    // Send the data.
                //    newStream.Write(byteData, 0, byteData.Length);
                //    newStream.Close();
                //}
                //catch (WebException ex)
                //{
                //    if (newStream != null)
                //    {
                //        newStream.Close();
                //    }
                //    if (objHWR != null)
                //    {
                //        objHWR.Abort();
                //    }
                //    throw ex;
                //}
            }
            return objHWR;
        }

        // 这个方法开始真正的请求过程
        // 运行在单独的线程中
        private void RequestStringWorker(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp)
        {

            Stream responseStream;
            string responseString = string.Empty;
            Exception e = null;

            // Check that the task is still active.
            // The operation may have been canceled before
            // the thread was scheduled.
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    switch (requestMethod)
                    {
                        case EnumRequestMethod.GET:
                            responseStream = this.HttpGetStream();
                            break;
                        case EnumRequestMethod.POST:
                            responseStream = this.HttpPostStream();
                            break;
                        default: //默认是POST
                            responseStream = this.HttpPostStream();
                            break;
                    }
                    if (responseStream != null)
                    {
                        responseString = GetResponseString(responseStream);
                    }
                    else
                    {
                        throw new Exception("返回的网络流为空！");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    e = ex;
                }
            }

            this.CompletionMethod(
                responseString,
                e,
                TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);

            //completionMethodDelegate(calcState);
        }

        // 这个方法开始真正的请求过程
        // 运行在单独的线程中
        private void RequestDataWorker(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp)
        {

            byte[] responseData = null;
            HttpWebResponse response = null;
            Exception e = null;
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    switch (requestMethod)
                    {
                        case EnumRequestMethod.GET:
                            response = this.HttpGetMethod();
                            break;
                        case EnumRequestMethod.POST:
                            response = this.HttpPostMethod();
                            break;
                        default: //默认是POST
                            response = this.HttpPostMethod();
                            break;
                    }
                    if (response != null)
                    {
                        responseData = GetResponseData(response);
                    }
                    else
                    {
                        throw new Exception("返回的Response对象为空！");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    e = ex;
                }
            }

            this.CompletionMethod(
                responseData,
                e,
                TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);

            //completionMethodDelegate(calcState);
        }



        // This is the method that the underlying, free-threaded 
        // asynchronous behavior will invoke.  This will happen on
        // an arbitrary thread.
        private void CompletionMethod(
            string responseString,
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

            RequestStringCompletedEventArgs e =
                new RequestStringCompletedEventArgs(
                responseString,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            // End the task. The asyncOp object is responsible 
            // for marshaling the call.
            asyncOp.PostOperationCompleted(onRequestStringCompletedDelegate, e);

            // Note that after the call to OperationCompleted, 
            // asyncOp is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }

        private void CompletionMethod(
           byte[] responseData,
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

            RequestDataCompletedEventArgs e =
                new RequestDataCompletedEventArgs(
                responseData,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            // End the task. The asyncOp object is responsible 
            // for marshaling the call.
            asyncOp.PostOperationCompleted(onRequestDataCompletedDelegate, e);

            // Note that after the call to OperationCompleted, 
            // asyncOp is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportRequestDataCompleted(object operationState)
        {
            RequestDataCompletedEventArgs e =
                operationState as RequestDataCompletedEventArgs;

            OnRequestDataCompleted(e);
        }

        private void ReportRequestStringCompleted(object operationState)
        {
            RequestStringCompletedEventArgs e =
                operationState as RequestStringCompletedEventArgs;

            OnRequestStringCompleted(e);
        }

        private void OnRequestStringCompleted(RequestStringCompletedEventArgs e)
        {
            if (RequestStringCompleted != null)
            {
                System.Diagnostics.Debug.WriteLine("￥￥￥￥￥OnRequestStringCompleted￥￥￥￥￥");
                RequestStringCompleted(this, e);
            }
        }



        protected void OnRequestDataCompleted(
            RequestDataCompletedEventArgs e)
        {
            if (RequestDataCompleted != null)
            {
                RequestDataCompleted(this, e);
            }
        }



        /// <summary>
        /// 判断一次请求任务是否被取消
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>是否被取消</returns>
        private bool TaskCanceled(object taskId)
        {
            return (userStateToLifetime[taskId] == null);
        }

        /// <summary>
        /// 取消一个任务的执行
        /// </summary>
        /// <param name="taskId">任务ID</param>
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

        #endregion 异步的模式的实现

        private string GetResponseString(Stream responseStream)
        {
            StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(Charset));
            string Content = string.Empty;
            try
            {
                Content = sr.ReadToEnd();
            }
            finally
            {
                if (null != sr)
                {
                    sr.Close();
                }
                if (null != responseStream)
                {
                    responseStream.Close();
                }
            }
            return Content;

        }

        private byte[] GetResponseData(HttpWebResponse response)
        {


            long contentLength = response.ContentLength;
            Stream readStream = response.GetResponseStream();
            byte[] resultBytes = null;
            try
            {

                if (null != readStream)
                {
                    resultBytes = ReadFully(readStream, (int)contentLength);

                }


            }
            finally
            {
                if (null != readStream)
                {
                    readStream.Close();
                }

            }
            return resultBytes;
        }




        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            try
            {
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;

                    // If we've reached the end of our buffer, check to see if there's
                    // any more information
                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        // End of stream? If so, we're done
                        if (nextByte == -1)
                        {
                            return buffer;
                        }
                        // Nope. Resize the buffer, put in the byte we've just
                        // read, and continue
                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }
            }
            catch { }

            //using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) 
            //{ 
            //    StringBuilder sb = new StringBuilder(); 
            //    try { while (!sr.EndOfStream) { sb.Append((char)sr.Read()); } } 
            //    catch (System.IO.IOException) { } 
            //    string content = sb.ToString(); 
            //}

            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        public void SetPostParam(Dictionary<string, string> postParams)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> param in postParams)
            {
                sb.Append(
                    string.Format("&{0}={1}",
                        param.Key,
                        UrlEncode(param.Value)
                        )
                );
            }
            PostData = sb.ToString().Trim('&');
        }

        /// <summary>
        /// 暂时不要用这个方法.
        /// </summary>
        /// <param name="PostParamName"></param>
        /// <param name="PostParamValue"></param>
        public void AddPostParam(string PostParamName, string PostParamValue)
        {
            PostData += string.Format("&{0}={1}", UrlEncode(PostParamName), UrlEncode(PostParamValue));
        }
        /// <summary>
        /// 取得页面编码
        /// </summary>
        /// <returns></returns>
        public string GetPageLanguageCode()
        {
            string pageLanguageCode = HttpGet();
            return RegexUtility.GetMatch(pageLanguageCode, "charset=(.*)\"");
        }
        public string HttpGet()
        {

            Stream s = HttpGetStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
            string Content = sr.ReadToEnd();
            //s.Close();
            sr.Close();
            s.Close();
            return Content;
        }

        private HttpWebResponse HttpGetMethod()
        {
            HttpWebRequest objHWR = (HttpWebRequest)HttpWebRequest.Create(Url);
            objHWR.Timeout = Timeout;
            objHWR.UserAgent = UserAgent;
            objHWR.Accept = Accept;
            objHWR.Referer = Referer;
            objHWR.Method = "GET";

            if (X_FORWARDED_FOR != string.Empty)
            {
                objHWR.Headers.Set("X_FORWARDED_FOR", X_FORWARDED_FOR);
            }

            if (Proxy != null)
            {
                objHWR.Proxy = Proxy;
            }
            if (Cookie != null)
            {
                objHWR.CookieContainer = Cookie;
            }

            HttpWebResponse objResponse = null;
            try
            {
                objResponse = (HttpWebResponse)objHWR.GetResponse();
            }
            catch (WebException ex)
            {
                if (objHWR != null)
                {
                    objHWR.Abort();
                }
                throw ex;
            }
            return objResponse;

        }

        public Stream HttpGetStream()
        {
            HttpWebResponse response = HttpGetMethod();
            Stream s = null;
            if (null != response)
            {
                s = response.GetResponseStream();
            }
            return s;
        }
        public string HttpPost()
        {

            Stream s = HttpPostStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
            string Content = sr.ReadToEnd();
            sr.Close();
            s.Close();
            return Content;
        }
        /// <summary>
        /// Post实现
        /// </summary>
        /// <returns>Response对象</returns>
        private HttpWebResponse HttpPostMethod()
        {

            HttpWebRequest objHWR = (HttpWebRequest)HttpWebRequest.Create(Url);
            objHWR.Timeout = Timeout;
            objHWR.ContentType = ContentType;
            objHWR.UserAgent = UserAgent;
            objHWR.Accept = Accept;
            objHWR.Referer = Referer;
            objHWR.Method = "POST";

            if (X_FORWARDED_FOR != string.Empty)
            {
                objHWR.Headers.Set("X_FORWARDED_FOR", X_FORWARDED_FOR);
            }

            if (Proxy != null)
            {
                objHWR.Proxy = Proxy;
            }
            if (Cookie != null)
            {
                objHWR.CookieContainer = Cookie;
            }
            Stream newStream = null;
            HttpWebResponse objResponse = null;
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(PostData);
                objHWR.ContentLength = byteData.Length;
                newStream = objHWR.GetRequestStream();
                // Send the data.
                newStream.Write(byteData, 0, byteData.Length);
                newStream.Close();
                objResponse = (HttpWebResponse)objHWR.GetResponse();
            }
            catch (WebException ex)
            {
                if (newStream != null)
                {
                    newStream.Close();
                }
                if (objHWR != null)
                {
                    objHWR.Abort();
                }
                throw ex;
            }
            return objResponse;

        }

        /// <summary>
        /// HttpPost()重载返回流的方法
        /// </summary>
        /// <returns></returns>
        public Stream HttpPostStream()
        {

            HttpWebResponse response = HttpPostMethod();
            Stream s = null;
            if (null != response)
            {
                s = response.GetResponseStream();
            }
            return s;
        }

        //Url编码
        string UrlEncode(string value)
        {
            return System.Web.HttpUtility.UrlEncode(value, Encoding.GetEncoding(this.m_Charset));
        }
        /////////////////////////////////////////////////////////////////////////

        private void BeginHttpReqeust(object requestState)
        {
            RequestState myRequestState = requestState as RequestState;
            HttpWebRequest myHttpWebRequest = myRequestState.request;

            if (myRequestState.RequestMethod == EnumRequestMethod.POST)
            {
                if (!string.IsNullOrEmpty(myRequestState.PostData))
                {
                    byte[] byteData = Encoding.ASCII.GetBytes(myRequestState.PostData);
                    myHttpWebRequest.ContentLength = byteData.Length;
                    myRequestState.RequestData = byteData;
                }
                myHttpWebRequest.BeginGetRequestStream(new AsyncCallback(ReadCallback), myRequestState);
            }
            else
            {
                IAsyncResult result =
                        (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
                    new WaitOrTimerCallback(TimeoutCallback), myRequestState, m_Timeout, true);
            }


        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
            HttpWebRequest myHttpWebRequest = myRequestState.request;
            Stream newStream = myHttpWebRequest.EndGetRequestStream(asynchronousResult);
            newStream.Write(myRequestState.RequestData, 0, myRequestState.RequestData.Length);
            newStream.Close();
            IAsyncResult result =
             (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
                new WaitOrTimerCallback(TimeoutCallback), myRequestState, m_Timeout, true);


        }


        private void RespCallback(IAsyncResult asynchronousResult)
        {
            Exception ex = null;
            RequestState myRequestState = null;
            try
            {
                myRequestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.request;

                myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.streamResponse = responseStream;
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                return;
            }
            catch (WebException e)
            {
                ex = e;
            }
            //触发请求出错事件
            if (myRequestState.IsRequestString)
            {
                myRequestState.Context.Post(onRequestStringCompletedDelegate,
                  new RequestStringCompletedEventArgs(string.Empty, ex, false, myRequestState.RequestId));
            }
            else
            {
                myRequestState.Context.Post(onRequestDataCompletedDelegate,
                  new RequestDataCompletedEventArgs(null, ex, false, myRequestState.RequestId));
            }
        }

        private static int BUFFER_SIZE = 1024;
        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                RequestState requestState = state as RequestState;
                if (requestState.request != null)
                {
                    requestState.request.Abort();
                }
                Exception ex = new Exception("超时");
                //触发超时事件
                if (requestState.IsRequestString)
                {
                    requestState.Context.Post(onRequestStringCompletedDelegate,
                       new RequestStringCompletedEventArgs(string.Empty, ex, false, requestState.RequestId));
                }
                else
                {
                    requestState.Context.Post(onRequestDataCompletedDelegate,
                      new RequestDataCompletedEventArgs(null, ex, false, requestState.RequestId));
                }
            }
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            Exception ex = null;
            RequestState myRequestState = null;

            try
            {
                myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.streamResponse;
                int read = responseStream.EndRead(asyncResult);
                if (read > 0)
                {

                    byte[] tempBuffer = new byte[read];
                    Array.Copy(myRequestState.BufferRead, tempBuffer, read);
                    myRequestState.ResponseData.AddRange(tempBuffer);
                    //Array.Copy(myRequestState.BufferRead, myRequestState.RequestData, read);
                    myRequestState.ResponseString.Append(Encoding.ASCII.GetString(myRequestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                    return;
                }
                else
                {
                    if (myRequestState.ResponseString.Length > 1)
                    {
                        string stringContent;
                        stringContent = myRequestState.ResponseString.ToString();
                    }
                    //关闭流
                    responseStream.Close();
                    //关闭响应
                    myRequestState.response.Close();
                }

            }
            catch (Exception e)
            {
                ex = e;
            }
            //触发完成请求
            if (myRequestState.IsRequestString)
            {
                myRequestState.Context.Post(onRequestStringCompletedDelegate,
                    new RequestStringCompletedEventArgs(myRequestState.ResponseString.ToString(), ex, false, myRequestState.RequestId));

            }
            else
            {
                byte[] requestData = new byte[myRequestState.ResponseData.Count];
                myRequestState.ResponseData.CopyTo(requestData);
                myRequestState.Context.Post(onRequestDataCompletedDelegate,
                    new RequestDataCompletedEventArgs(requestData, ex, false, myRequestState.RequestId));

            }
        }


        //////////////////////////////////////////////////////////////////////// 
        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }



    #region RequestCompletedEventArgs 参数类

    public class RequestStringCompletedEventArgs :
        AsyncCompletedEventArgs
    {


        private string responseStringValue = string.Empty;

        public RequestStringCompletedEventArgs(
            string responseString,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {

            this.responseStringValue = responseString;

        }



        public string ResponseString
        {
            get
            {
                // Raise an exception if the operation failed or 
                // was canceled.
                RaiseExceptionIfNecessary();

                // If the operation was successful, return the 
                // property value.
                return responseStringValue;
            }
        }


    }

    public class RequestDataCompletedEventArgs :
       AsyncCompletedEventArgs
    {
        private byte[] responseDataValue;
        public RequestDataCompletedEventArgs(
            byte[] responseData,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.responseDataValue = responseData;
        }



        public byte[] ResponseData
        {
            get
            {

                RaiseExceptionIfNecessary();
                return responseDataValue;
            }
        }


    }
    #endregion RequestStringCompletedEventArgs 参数类

    public enum EnumRequestMethod
    {
        GET = 0,
        POST = 1
    }


    public class RequestState
    {

        const int BUFFER_SIZE = 1024;

        public StringBuilder ResponseString;

        public ArrayList ResponseData;

        public byte[] BufferRead;

        public HttpWebRequest request;

        public HttpWebResponse response;

        public Stream streamResponse;

        public object RequestId;

        public int ThreadIndex;

        public string Url;

        public bool IsRequestString;

        public SynchronizationContext Context;

        public string PostData = string.Empty;

        public EnumRequestMethod RequestMethod;

        public byte[] RequestData;

        public RequestState()
        {
            ResponseData = new ArrayList();
            BufferRead = new byte[BUFFER_SIZE];
            ResponseString = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }


}
