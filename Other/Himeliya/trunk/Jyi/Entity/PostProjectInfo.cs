using System;
using System.Collections.Generic;

namespace Jyi.Entity
{
    class PostProjectInfo
    {
        private BaseInfo m_BaseInfo;
        internal BaseInfo BaseInfo
        {
            get { return m_BaseInfo; }
            set { m_BaseInfo = value; }
        }

        private List<UrlInfo> m_UrlInfoList;
        internal List<UrlInfo> UrlInfoList
        {
            get { return m_UrlInfoList; }
            set { m_UrlInfoList = value; }
        }
    }

    class BaseInfo
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private int m_Count;
        public int Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }

        private string m_Charset;
        public string Charset
        {
            get { return m_Charset; }
            set { m_Charset = value; }
        }

        private int m_UseProxy;
        public int UseProxy
        {
            get { return m_UseProxy; }
            set { m_UseProxy = value; }
        }

        

        private int m_UseCookie;
        public int UseCookie
        {
            get { return m_UseCookie; }
            set { m_UseCookie = value; }
        }

        private int m_ClearCookieEverytime;
        public int ClearCookieEverytime
        {
            get { return m_ClearCookieEverytime; }
            set { m_ClearCookieEverytime = value; }
        }

        private int m_WaitTime;
        public int WaitTime
        {
            get { return m_WaitTime; }
            set { m_WaitTime = value; }
        }
    }

    class UrlInfo
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Url;
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        private int m_Count;
        public int Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }

        private int m_WaitTime;
        public int WaitTime
        {
            get { return m_WaitTime; }
            set { m_WaitTime = value; }
        }

        private string m_Referer;
        public string Referer
        {
            get { return m_Referer; }
            set { m_Referer = value; }
        }

        private string m_SuccessText;
        public string SuccessText
        {
            get { return m_SuccessText; }
            set { m_SuccessText = value; }
        }

        private string m_Method;
        public string Method
        {
            get { return m_Method; }
            set { m_Method = value; }
        }

        private string m_RemoveParms;
        /// <summary>
        /// 需要从PreParmsList预处理参数列表中移除的参数名,用","分割
        /// </summary>
        public string RemoveParms
        {
            get { return m_RemoveParms; }
            set { m_RemoveParms = value; }
        }

        private int m_HavePostParms;
        /// <summary>
        /// 是否有POST参数
        /// </summary>
        public int HavePostParms
        {
            get { return m_HavePostParms; }
            set { m_HavePostParms = value; }
        }

        private List<ParmsInfo> m_PostParmsList;
        public List<ParmsInfo> PostParmsList
        {
            get { return m_PostParmsList; }
            set { m_PostParmsList = value; }
        }

        private int m_HaveGetParms;
        /// <summary>
        /// 是否有GetParms参数
        /// </summary>
        public int HaveGetParms
        {
            get { return m_HaveGetParms; }
            set { m_HaveGetParms = value; }
        }

        private List<GetParmsInfo> m_GetParmsList;
        public List<GetParmsInfo> GetParmsList
        {
            get { return m_GetParmsList; }
            set { m_GetParmsList = value; }
        }

        private int m_UseVCode;
        public int UseVCode
        {
            get { return m_UseVCode; }
            set { m_UseVCode = value; }
        }

        //如果UseVCode=1,读取验证码地址
        private string m_VCodeUrl;
        public string VCodeUrl
        {
            get { return m_VCodeUrl; }
            set { m_VCodeUrl = value; }
        }

        //如果UserVCode=1,读取验证码识别列表
        private List<UnCodeInfo> m_UnCodeList;
        internal List<UnCodeInfo> UnCodeList
        {
            get { return m_UnCodeList; }
            set { m_UnCodeList = value; }
        }
    }
    class UnCodeInfo
    {
        /// <summary>
        /// 实际字符串
        /// </summary>
        private string m_Code;
        /// <summary>
        /// 实际支付传
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        /// <summary>
        /// 图片码
        /// </summary>
        private string m_ImgCode;
        public string ImgCode
        {
            get { return m_ImgCode; }
            set { m_ImgCode = value; }
        }
    }

    class ParmsInfo
    {
        private string m_Name;
        private string m_Type;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
    }

    class GetParmsInfo
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Regex;
        /// <summary>
        /// 取值的正则表达
        /// </summary>
        public string Regex
        {
            get { return m_Regex; }
            set { m_Regex = value; }
        }
    }
}
