using System;
using System.Collections.Generic;
using System.Text;
using Natsuhime;
using System.Net;
using Natsuhime.Events;

namespace Himeliya.Kate.Analyze
{
    class BaseFetcher
    {
        protected NewHttper httper = null;
        public CookieContainer Cookie
        {
            get
            {
                return this.httper.Cookie;
            }
            set
            {
                this.httper.Cookie = value;
            }
        }
        public WebProxy WebProxy
        {
            get
            {
                return this.httper.Proxy;
            }
            set
            {
                this.httper.Proxy = value;
            }
        }
        public string Url
        {
            get
            {
                return this.httper.Url;
            }
            set
            {
                this.httper.Url = value;
            }
        }
        public string Charset
        {
            get
            {
                return this.httper.Charset;
            }
            set
            {
                this.httper.Charset = value;
            }
        }

        public void FetchListAnsy()
        {
            this.FetchListAnsy(Guid.NewGuid());
        }

        public void FetchListAnsy(object userState)
        {
            this.httper.RequestStringAsync(EnumRequestMethod.GET, userState);
        }

        protected void OnCompleted(ReturnCompletedEventArgs e)
        {
            if (this.FetchCompleted != null)
            {
                this.FetchCompleted(this, e);
            }
        }
        public event EventHandler<ReturnCompletedEventArgs> FetchCompleted;
    }
}
