using System;
using System.Collections.Generic;
using System.Text;
using Natsuhime;
using System.Net;
using Natsuhime.Web;
using Natsuhime.Events;
using Himeliya.Kate.EventArg;
using System.Text.RegularExpressions;
using Himeliya.Kate.Entity;

namespace Himeliya.Kate.Analyze
{
    class TitleListFetcher : BaseFetcher
    {
        public TitleListFetcher()
        {
            base.httper = new NewHttper();
            base.httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_StringCompleted);
        }

        void httper_StringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.GetTitleUrlsComplete(e.ResponseString, e.UserState, e.Cancelled);
            }
            else
            {
                OnCompleted(new ReturnCompletedEventArgs(null, e.Error, e.Cancelled, e.UserState));
            }
        }

        void GetTitleUrlsComplete(string sourceHtml, object userstate, bool cancelled)
        {
            List<PostInfo> posts = new List<PostInfo>();
            int pageCount = 0;

            string baseUrl = base.httper.Url.Substring(0, base.httper.Url.LastIndexOf('/') + 1);
            MatchCollection urlList = Natsuhime.Web.Plugin.Discuz.TextAnalyze.GetThreadsInBoard(sourceHtml);
            Exception error = null;
            if (urlList != null)
            {
                foreach (Match key in urlList)
                {
                    PostInfo pi = new PostInfo();
                    pi.Url = Utils.CompleteRelativeUrl(baseUrl, key.Groups[1].Value);
                    pi.Title = Utils.HtmlDecode(key.Groups[2].Value);
                    posts.Add(pi);
                }
                pageCount = Natsuhime.Web.Plugin.Discuz.TextAnalyze.GetBoardPageCount(sourceHtml);
            }
            else
            {
                error = new Exception(sourceHtml);
            }

            OnCompleted(new ReturnCompletedEventArgs(urlList, error, cancelled, userstate));//兼容测试方法用 以后移除
            OnFetchPostCompleted(new FetchTitleCompletedEventArgs(posts, pageCount, error, cancelled, userstate));

        }

        protected void OnFetchPostCompleted(FetchTitleCompletedEventArgs e)
        {
            if (this.FetchPostCompleted != null)
            {
                this.FetchPostCompleted(this, e);
            }
        }
        public event EventHandler<FetchTitleCompletedEventArgs> FetchPostCompleted;
    }
}
