using System;
using System.Collections.Generic;
using System.Text;
using Jyi.Entity;
using System.Xml;

namespace Jyi.Config
{
    class PostProjectConfig
    {
        public static PostProjectInfo GetConfig(string ProjectName)
        {
            PostProjectInfo objPostProjectInfo = new PostProjectInfo();
            objPostProjectInfo.BaseInfo = new BaseInfo();
            objPostProjectInfo.UrlInfoList = new List<UrlInfo>();


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MainForm.configFilePath);//加载文件
            XmlNode xnProject = xmlDoc.SelectSingleNode(string.Format("/JyiConfig/PostProject/Item[@Name='{0}']", ProjectName));//查找节点
            objPostProjectInfo.BaseInfo.Name=xnProject.Attributes["Name"].Value;//设置项目名称



            XmlNode xnBase=xnProject.SelectSingleNode("Base");//寻找到Base配置节,开始读取基本配置
            objPostProjectInfo.BaseInfo.Count = int.Parse(xnBase.SelectSingleNode("Count").InnerText);//执行周期次数
            objPostProjectInfo.BaseInfo.Charset = xnBase.SelectSingleNode("Charset").InnerText;//页面编码
            objPostProjectInfo.BaseInfo.UseProxy = int.Parse(xnBase.SelectSingleNode("UseProxy").InnerText);//是否使用代理
            objPostProjectInfo.BaseInfo.UseCookie = int.Parse(xnBase.SelectSingleNode("UseCookie").InnerText);//是否使用Cookie
            objPostProjectInfo.BaseInfo.ClearCookieEverytime = int.Parse(xnBase.SelectSingleNode("ClearCookieEverytime").InnerText);//每个周期循环前是否清除Cookie
            objPostProjectInfo.BaseInfo.WaitTime = int.Parse(xnBase.SelectSingleNode("WaitTime").InnerText);//每个周期间隔时间(毫秒)


            //寻找到Url配置节,开始读取配置
            XmlNodeList xnlUrl = xnProject.SelectSingleNode("Url").ChildNodes;
            foreach (XmlNode xnUrlItem in xnlUrl)//循环读取每一个Item节点
            {
                //XmlNode xnUrl = xnProject.SelectSingleNode("Url");
                UrlInfo objUrlInfo = new UrlInfo();
                objUrlInfo.Name = xnUrlItem.Attributes["Name"].Value;//此Url的代号名称
                objUrlInfo.Url = xnUrlItem.Attributes["Url"].Value;//此Url的地址
                objUrlInfo.Count = int.Parse(xnUrlItem.SelectSingleNode("Count").InnerText);//Url提交次数
                objUrlInfo.WaitTime = int.Parse(xnUrlItem.SelectSingleNode("WaitTime").InnerText);//Url提交间隔时间(毫秒)
                objUrlInfo.Referer = xnUrlItem.SelectSingleNode("Referer").InnerText;
                objUrlInfo.SuccessText = xnUrlItem.SelectSingleNode("SuccessText").InnerText;//成功标志
                objUrlInfo.Method = xnUrlItem.SelectSingleNode("Method").InnerText;//提交方式

                objUrlInfo.HavePostParms = int.Parse(xnUrlItem.SelectSingleNode("HavePostParms").InnerText);//是否有Post提交参数
                if (objUrlInfo.HavePostParms > 0)//如果有Post提交参数,加载PostParms列表
                {
                    objUrlInfo.PostParmsList = new List<ParmsInfo>();//初始化PostParms参数泛型列表
                    XmlNodeList xnlPostParms = xnUrlItem.SelectSingleNode("PostParms").ChildNodes;
                    foreach (XmlNode xnf in xnlPostParms)
                    {
                        ParmsInfo objParm = new ParmsInfo();
                        XmlElement xe = (XmlElement)xnf;
                        objParm.Name = xnf.InnerText;
                        objParm.Type = System.Web.HttpUtility.UrlEncode(xnf.Attributes["Type"].Value, Encoding.GetEncoding(objPostProjectInfo.BaseInfo.Charset));
                        objUrlInfo.PostParmsList.Add(objParm);
                    }
                }

                objUrlInfo.UseVCode = int.Parse(xnUrlItem.SelectSingleNode("UseVCode").InnerText);//是否使用验证码
                if (objUrlInfo.UseVCode > 0)//加载验证码识别列表
                {
                    objUrlInfo.UnCodeList = new List<UnCodeInfo>();//初始化UnCode参数泛型列表
                    objUrlInfo.VCodeUrl = xnUrlItem.SelectSingleNode("UnCode").Attributes["Url"].Value;//验证码图片地址
                    XmlNodeList xnlUnCode = xnUrlItem.SelectSingleNode("UnCode").ChildNodes;
                    foreach (XmlNode xnf in xnlUnCode)
                    {
                        UnCodeInfo objUnCode = new UnCodeInfo();
                        XmlElement xe = (XmlElement)xnf;
                        objUnCode.ImgCode = xnf.InnerText;//图片码
                        objUnCode.Code = xnf.Attributes["Code"].Value;//实际码
                        objUrlInfo.UnCodeList.Add(objUnCode);
                    }

                }


                objUrlInfo.RemoveParms = xnUrlItem.SelectSingleNode("RemoveParms").InnerText.Trim();//需要移除的预处理参数,移除本次Url使用后别人不再使用的预处理参数,避免在后面的Url的Post中遇到重名的参数被错误Post

                objUrlInfo.HaveGetParms = int.Parse(xnUrlItem.SelectSingleNode("HaveGetParms").InnerText);//是否有GetParms预处理参数
                if (objUrlInfo.HaveGetParms > 0)//如果有预处理的Parms
                {
                    objUrlInfo.GetParmsList = new List<GetParmsInfo>();//初始化GetParms参数泛型列表
                    XmlNodeList xnlUnCode = xnUrlItem.SelectSingleNode("GetParms").ChildNodes;
                    foreach (XmlNode xnf in xnlUnCode)
                    {
                        GetParmsInfo objGetParmsInfo = new GetParmsInfo();
                        XmlElement xe = (XmlElement)xnf;
                        objGetParmsInfo.Name = xnf.InnerText;//预处理参数名
                        objGetParmsInfo.Regex = xnf.Attributes["Regex"].Value;//分析的正则
                        objUrlInfo.GetParmsList.Add(objGetParmsInfo);
                    }
                }



                objPostProjectInfo.UrlInfoList.Add(objUrlInfo);//循环完毕一个Url的Item节点,加入列表 继续循环.
            }

            return objPostProjectInfo;
        }
    }
}
