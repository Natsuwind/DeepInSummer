using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Entity
{
    /// <summary>
    /// Discuz云平台返回信息格式基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseCloudResponse<T>
    {
        private int errCode;

        [JsonPropertyAttribute("errCode")]
        public int ErrCode
        {
            get { return errCode; }
            set { errCode = value; }
        }
        private string errMessage;

        [JsonPropertyAttribute("errMessage")]
        public string ErrMessage
        {
            get { return errMessage; }
            set { errMessage = value; }
        }

        private T result;

        [JsonPropertyAttribute("result")]
        public T Result
        {
            get { return result; }
            set { result = value; }
        }
    }

    /// <summary>
    /// 注册云平台返回类型
    /// </summary>
    public class RegisterCloud
    {
        private string cloudSiteId;

        [JsonPropertyAttribute("sId")]
        public string CloudSiteId
        {
            get { return cloudSiteId; }
            set { cloudSiteId = value; }
        }
        private string cloudSiteKey;

        [JsonPropertyAttribute("sKey")]
        public string CloudSiteKey
        {
            get { return cloudSiteKey; }
            set { cloudSiteKey = value; }
        }
    }

    #region QQ Connect

    public class ConnectResponse<T>
    {
        private int _status;

        [JsonPropertyAttribute("status")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private T _result;

        [JsonPropertyAttribute("result")]
        public T Result
        {
            get { return _result; }
            set { _result = value; }
        }


    }

    public class OAuthTokenInfo
    {
        private string _token;

        [JsonPropertyAttribute("oauth_token")]
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private string _secret;

        [JsonPropertyAttribute("oauth_token_secret")]
        public string Secret
        {
            get { return _secret; }
            set { _secret = value; }
        }
    }

    public class OAuthAccessTokenInfo : OAuthTokenInfo
    {
        private string _openid;

        [JsonPropertyAttribute("openid")]
        public string Openid
        {
            get { return _openid; }
            set { _openid = value; }
        }
    }

    #endregion
}
