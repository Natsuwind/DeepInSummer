using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Discuz.Common
{
    /// <summary>
    /// QQ Connect OAuth辅助类
    /// </summary>
    public class DiscuzOAuth
    {
        /// <summary>
        /// 随机数辅助对象
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 获取OAuth地址
        /// </summary>
        /// <param name="url">API Url</param>
        /// <param name="httpMethod">HTTP METHOD GET OR POST</param>
        /// <param name="customAppId">应用程序ID</param>
        /// <param name="customAppSecret">应用程序加密key</param>
        /// <param name="tokenKey">请求获取到的oauth_token</param>
        /// <param name="tokenSecrect">请求获取到的oauth_token_secret</param>
        /// <param name="verify">请求获取到的oauth_verifier</param>
        /// <param name="callbackUrl">认证成功返回</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="queryString">返回queryString格式的请求参数</param>
        /// <returns></returns>
        public string GetOAuthUrl(string url, string httpMethod, string customAppId, string customAppSecret, string tokenKey,
    string tokenSecrect, string verify, string callbackUrl, List<DiscuzOAuthParameter> parameters, out string queryString)
        {
            parameters.Add(new DiscuzOAuthParameter("oauth_nonce", GenerateNonce()));
            parameters.Add(new DiscuzOAuthParameter("oauth_timestamp", GenerateTimeStamp()));
            parameters.Add(new DiscuzOAuthParameter("oauth_signature_method", "HMAC_SHA1"));
            parameters.Add(new DiscuzOAuthParameter("oauth_consumer_key", customAppId));

            if (!string.IsNullOrEmpty(tokenKey))
                parameters.Add(new DiscuzOAuthParameter("oauth_token", tokenKey));

            if (!string.IsNullOrEmpty(verify))
                parameters.Add(new DiscuzOAuthParameter("oauth_verifier", verify));

            if (!string.IsNullOrEmpty(callbackUrl))
                parameters.Add(new DiscuzOAuthParameter("oauth_callback", callbackUrl));

            string paramStr = NormalizeRequestParameters(parameters);

            string urlWithParameter = url;
            if (!string.IsNullOrEmpty(paramStr))
                urlWithParameter += "?" + paramStr;

            Uri uri = new Uri(urlWithParameter);

            string normalizedUrl = null;
            string signature = GenerateSignature(uri, customAppSecret, tokenSecrect, httpMethod, parameters,
                out normalizedUrl);

            queryString = paramStr + "&response_format=json&oauth_signature=" + signature;
            return normalizedUrl;
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>		
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>
        /// <param name="consumerSecret">The consumer seceret</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <returns>A base64 string of the hash value</returns>
        private string GenerateSignature(Uri url, string consumerSecret, string tokenSecret, string httpMethod, List<DiscuzOAuthParameter> parameters,
            out string normalizedUrl)
        {
            string signatureBase = GenerateSignatureBase(url, httpMethod, parameters, out normalizedUrl);

            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", consumerSecret, string.IsNullOrEmpty(tokenSecret) ? "" : tokenSecret));

            return GenerateSignatureUsingHash(signatureBase, hmacsha1);
        }

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="consumerKey">The consumer key</param>        
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
        /// <returns>The signature base</returns>
        private string GenerateSignatureBase(Uri url, string httpMethod, List<DiscuzOAuthParameter> parameters,
            out string normalizedUrl)
        {
            normalizedUrl = null;
            string normalizedRequestParameters = null;

            parameters.Sort(new ParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Authority);
            normalizedUrl += url.AbsolutePath;

            normalizedRequestParameters = FormEncodeParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&{1}&{2}", httpMethod.ToUpper(), Utils.PHPUrlEncode(normalizedUrl),
                Utils.PHPUrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Get request parameters in signaturebase when generate signature(ignore params that prefix is not "oauth_")
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string FormEncodeParameters(List<DiscuzOAuthParameter> parameters)
        {
            List<DiscuzOAuthParameter> encodeParams = new List<DiscuzOAuthParameter>();
            foreach (DiscuzOAuthParameter a in parameters)
            {
                if (a.Name.IndexOf("oauth_") != -1)
                    encodeParams.Add(new DiscuzOAuthParameter(a.Name, Utils.PHPUrlEncode(a.Value)));
            }
            return NormalizeRequestParameters(encodeParams);
        }

        /// <summary>
        /// Generate the signature value based on the given signature base and hash algorithm
        /// </summary>
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
        /// <returns>A base64 string of the hash value</returns>
        private string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <param name="data">The data to hash</param>
        /// <returns>a Hex string of the hash value(腾讯即通平台部标准,返回byte[]中每个字节的16进制码字串)</returns>
        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
                throw new ArgumentNullException("hashAlgorithm");

            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("data");

            byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                sb.Append((b < 16 ? "0" : "") + Convert.ToString(b, 16));
                //string code = Convert.ToString(b, 16);
                //sb.Append((code.Length < 2 ? "0" : "") + code);
            }
            return sb.ToString();
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        //private string UrlEncode(string value)
        //{
        //    StringBuilder result = new StringBuilder();
        //    byte[] byStr = System.Text.Encoding.UTF8.GetBytes(value);

        //    foreach (byte symbol in byStr)
        //    {
        //        if (unreservedChars.IndexOf((char)symbol) != -1)
        //        {
        //            result.Append((char)symbol);
        //        }
        //        else
        //        {
        //            result.Append('%' + Convert.ToString((char)symbol, 16).ToUpper());
        //        }
        //    }

        //    return result.ToString();
        //}

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        private string NormalizeRequestParameters(List<DiscuzOAuthParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            DiscuzOAuthParameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, Utils.PHPUrlEncode(p.Value));

                if (i < parameters.Count - 1)
                    sb.Append("&");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        private string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        private string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(123400, 9999999).ToString();
        }
    }

    /// <summary>
    /// Discuz Cloud OAuth Request Parameters Class
    /// </summary>
    public class DiscuzOAuthParameter
    {
        private string name = null;
        private string value = null;

        public DiscuzOAuthParameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public string Value
        {
            get { return value; }
        }
    }

    /// <summary>
    /// Comparer class used to perform the sorting of the query parameters
    /// </summary>
    public class ParameterComparer : IComparer<DiscuzOAuthParameter>
    {
        public int Compare(DiscuzOAuthParameter x, DiscuzOAuthParameter y)
        {
            return x.Name == y.Name ? string.Compare(x.Value, y.Value) : string.Compare(x.Name, y.Name);
        }
    }
}
