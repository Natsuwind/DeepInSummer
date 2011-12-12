using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Jyi.Entity;
using Jyi.Config;
using Jyi.Utility;

using System.Threading;

namespace Jyi
{
    public partial class ProxyForm : Form
    {
        //设置消息跨线程执行
        public static ProxyForm objProxyForm;//在启动线程之前把本窗口的示例赋值给objMainForm
        internal List<ProxyInfo> objProxyInfoListOK;
        public int threadCount;
        public int finishedThreadCount;
        
        delegate void SetTextCallback(string text);
        public void SetText(string text)
        {
            if (this.tbMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.tbMessage.Text += text;
                if (!this.cbStopScroll.Checked)
                {
                    this.tbMessage.Focus();//让文本框获取焦点
                    this.tbMessage.Select(this.tbMessage.TextLength, 0);//设置光标的位置到文本尾
                    this.tbMessage.ScrollToCaret();//滚动到控件光标处
                    if (this.tbMessage.TextLength >= 5000)
                    {
                        this.tbMessage.Text = "";
                    }
                }
                //this.tbMessage.Focused = false;
            }

        }


        Thread[] checkThreads;


        public ProxyForm()
        {
            InitializeComponent();
        }

        private void btnClearProxy_Click(object sender, EventArgs e)
        {
            objProxyForm = this;
            List<ProxyInfo> objProxyList = BaseConfig.GetBaseConfig().ProxyList;
            objProxyInfoListOK = new List<ProxyInfo>();

            CheckingProxy objCheckingProxy = new CheckingProxy(objProxyList, objProxyForm,tbTryUrl.Text,tbCharset.Text,tbSuccessText.Text);
            ThreadStart checkingThreadDelegate = new ThreadStart(objCheckingProxy.ExecuteChecking);
            threadCount = 5;
            finishedThreadCount = 0;
            checkThreads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                checkThreads[i] = new Thread(checkingThreadDelegate);
                checkThreads[i].Name = i.ToString();
                checkThreads[i].Start();
            }
            ThreadStart ojbWriteXmlDelegate = new ThreadStart(objCheckingProxy.WriteCheckedProxy);
            Thread objWriteThread = new Thread(ojbWriteXmlDelegate);
            objWriteThread.Start();

            //foreach (Thread checkThread in checkThreads)
            //{

            //}
            //Utility.Utility.ClearXmlProxyList();

            //Utility.Utility.WriteProxyListToXml(objProxyList);
        }

        private void btnGetProxy_Click(object sender, EventArgs e)
        {
            objProxyForm = this;

            List<ProxyGetInfo> objProxyGetList = new List<ProxyGetInfo>();
            objProxyGetList = ProxyGetConfig.GetProxyGetPageUrlList();
            string returnData;
            
            foreach (ProxyGetInfo objProxyGetInfo in objProxyGetList)
            {

                objProxyForm.SetText(string.Format("Url：{0}分析开始\r\n",objProxyGetInfo.PageUrl));
                returnData = Conn.GetData(objProxyGetInfo.PageUrl, objProxyGetInfo.Charset,objProxyGetInfo.PageUrl,10000,null,null);
                if (cbxDebugMode.Checked == true)
                {
                    tbMessage.Text += string.Format("{0}\r\n", returnData);
                }
                else
                {
                    RegexFunc rFunc = new RegexFunc();
                    System.Text.RegularExpressions.MatchCollection m = rFunc.GetMatchFull(returnData, objProxyGetInfo.Regex);
                    List<ProxyInfo> objProxyList = new List<ProxyInfo>();
                    foreach (System.Text.RegularExpressions.Match objMatch in m)
                    {
                        ProxyInfo objProxyInfo = new ProxyInfo();
                        objProxyInfo.Address = objMatch.Groups[1].Value.Split(':')[0];
                        objProxyInfo.Port = int.Parse(objMatch.Groups[1].Value.Split(':')[1]);
                        objProxyInfo.Name = "暂无";
                        objProxyList.Add(objProxyInfo);
                        tbMessage.Text += string.Format("获得地址:{0}:{1}\r\n", objProxyInfo.Address, objProxyInfo.Port);
                    }

                    Utility.Utility.WriteProxyListToXml(objProxyList);
                }
            }
        }
    }

    class CheckingProxy
    {
        ProxyForm m_ProxyForm;
        List<ProxyInfo> m_ProxyInfoList;
        string m_TestPageUrl, m_TestPageCharset, m_TestPageSuccessText;

        public CheckingProxy(List<ProxyInfo> proxyInfoList, ProxyForm proxyForm, string testPageUrl, string testPageCharset, string testPageSuccessText)
        {
            m_ProxyForm = proxyForm;
            m_ProxyInfoList = proxyInfoList;
            m_TestPageUrl = testPageUrl;
            m_TestPageCharset = testPageCharset;
            m_TestPageSuccessText = testPageSuccessText;
        }

        public void ExecuteChecking()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ProxyInfo objProxyInfo in m_ProxyInfoList)            
            {                
                sb.Remove(0, sb.Length);

                if (!objProxyInfo.HaveGet)
                {
                    objProxyInfo.HaveGet = true;
                    m_ProxyForm.SetText(string.Format("线程[{3}]{0}={1}：{2}校验中...\r\n", objProxyInfo.Name, objProxyInfo.Address, objProxyInfo.Port,Thread.CurrentThread.Name));
                    /*
                     * 校验过程
                     */
                    try
                    {
                        System.Net.WebProxy objWebProxy = new System.Net.WebProxy(objProxyInfo.Address, objProxyInfo.Port);
                        sb.Append(Conn.PostData(m_TestPageUrl, m_TestPageCharset, "", m_TestPageUrl, 10000, objWebProxy));

                        RegexFunc rFunc = new RegexFunc();
                        if (sb.ToString().IndexOf("Jyi链接失败") < 0 && "124.207.144.194" != rFunc.GetMatch(sb.ToString(), "您的IP地址是：\\[(.*)\\] 来自\\："))
                        {
                            m_ProxyForm.objProxyInfoListOK.Add(objProxyInfo);//添加成功的代理
                            m_ProxyForm.SetText(string.Format("{0}={1}：{2}成功!\r\n", objProxyInfo.Name, objProxyInfo.Address, objProxyInfo.Port));
                        }
                        else
                        {
                            m_ProxyForm.SetText(string.Format("{0}={1}：{2}失败!\r\n", objProxyInfo.Name, objProxyInfo.Address, objProxyInfo.Port));
                        }
                    }
                    catch (System.UriFormatException ex)
                    {
                        m_ProxyForm.SetText(string.Format("{0}={1}：{2}无效!\r\n", objProxyInfo.Name, objProxyInfo.Address, objProxyInfo.Port));
                    }
                }

            }
            m_ProxyForm.SetText(string.Format("没有待校验的列表,线程[{0}]结束!\r\n",Thread.CurrentThread.Name));
            m_ProxyForm.finishedThreadCount++;//报告线程结束.
            Thread.CurrentThread.Abort();

            //测试是否终止
            m_ProxyForm.SetText(string.Format("没有待校验的列表,当前线程结束了么?"));
        }
        public void WriteCheckedProxy()
        {
            while (true)
            {
                if (m_ProxyForm.finishedThreadCount == m_ProxyForm.threadCount) 
                {
                    Utility.Utility.ClearXmlProxyList();
                    Utility.Utility.WriteProxyListToXml(m_ProxyForm.objProxyInfoListOK);
                    m_ProxyForm.SetText("成功写入配置文件!\r\n");
                    break;
                }
                Thread.Sleep(10000);
            }
        }


    }
}
