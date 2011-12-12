using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Himeliya.Kate
{
    class DownLoader
    {
        private List<string> m_Url;
        private string m_SavePath;

        WebClient wc = new WebClient();


        public DownLoader(List<string> url, string savepath)
        {
            wc.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            m_Url = url;
            m_SavePath = savepath;
        }

        void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (DownloadChanged != null)
                DownloadChanged(this, e);
        }
        public void AsyncDownload()
        {
            foreach (string url in m_Url)
            {
                wc.DownloadFileAsync(new Uri(url), m_SavePath + url.Substring(url.LastIndexOf('/') + 1, url.Length - url.LastIndexOf('/') - 1), url);
            }
        }
        public void Download()
        {
            foreach (string url in m_Url)
            {
                //WebRequest hwr = WebRequest.Create(url);
                //hwr.BeginGetResponse(new AsyncCallback(AsyncDownLoad), hwr);

                wc.DownloadFile(url, m_SavePath + url.Substring(url.LastIndexOf('/') + 1, url.Length - url.LastIndexOf('/') - 1));

                if (DownloadChanged != null)
                    DownloadChanged(this, new System.ComponentModel.AsyncCompletedEventArgs(null, false, url));

            }
        }

        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DownloadChanged;
    }
}
