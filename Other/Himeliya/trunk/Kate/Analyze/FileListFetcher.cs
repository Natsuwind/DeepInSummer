using System;
using System.Collections.Generic;
using System.Text;
using Natsuhime;
using Natsuhime.Events;

namespace Himeliya.Kate.Analyze
{
    class FileListFetcher : BaseFetcher
    {
        public FileListFetcher()
        {
            base.httper = new NewHttper();
            base.httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_RequestStringCompleted);
        }

        void httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.GetFileUrlsComplete(e.ResponseString, e.UserState, e.Cancelled);
            }
            else
            {
                OnCompleted(new ReturnCompletedEventArgs(null, e.Error, e.Cancelled, e.UserState));
            }
        }

        void GetFileUrlsComplete(string sourceHtml, object userstate, bool cancelled)
        {
            string baseUrl = base.httper.Url.Substring(0, base.httper.Url.LastIndexOf('/') + 1);
            List<string> urlList = Natsuhime.Web.Plugin.Discuz.TextAnalyze.GetFilesInPost(
                sourceHtml,
                baseUrl
                );


            OnCompleted(new ReturnCompletedEventArgs(urlList, null, cancelled, userstate));
        }
    }
}
