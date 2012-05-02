using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Mall;
using Discuz.Plugin.Payment;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// ����״̬֪ͨҳ��
    /// </summary>
    public class tradenotify : PageBase
    {
        protected override void ShowPage()
        {
            if (CheckPayment())
            {
                Goodstradeloginfo goodstradeloginfo = TradeLogs.GetGoodsTradeLogInfo(DNTRequest.GetString("out_trade_no"));

                if (goodstradeloginfo != null && goodstradeloginfo.Id > 0)
                {
                    switch (DNTRequest.GetString("trade_status"))
                    {
                        case "WAIT_BUYER_PAY": // �ȴ���Ҹ���
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_BUYER_PAY; break; 
                            }
                        case "WAIT_SELLER_CONFIRM_TRADE": // �����Ѵ������ȴ�����ȷ��
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_CONFIRM_TRADE; break; 
                            }
                        case "WAIT_SYS_CONFIRM_PAY":��// ȷ����Ҹ����У����𷢻�
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SYS_CONFIRM_PAY; break; 
                            }
                        case "WAIT_SELLER_SEND_GOODS": // ֧�����յ���Ҹ�������ҷ���
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_SEND_GOODS; break; 
                            }
                        case "WAIT_BUYER_CONFIRM_GOODS": //  �����ѷ��������ȷ����
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_BUYER_CONFIRM_GOODS; break; 
                            }
                        case "WAIT_SYS_PAY_SELLER": // ���ȷ���յ������ȴ�֧������������
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SYS_PAY_SELLER; break; 
                            }
                        case "TRADE_FINISHED": // ���׳ɹ�����
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.TRADE_FINISHED; break; 
                            }
                        case "TRADE_CLOSED": //  ������;�ر�(δ���)
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.TRADE_CLOSED; break; 
                            }
                        case "WAIT_SELLER_AGREE": //  �ȴ�����ͬ���˿�
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_AGREE; break; 
                            }
                        case "SELLER_REFUSE_BUYER": // ���Ҿܾ�����������ȴ�����޸�����
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.SELLER_REFUSE_BUYER; break; 
                            }
                        case "WAIT_BUYER_RETURN_GOODS": // ����ͬ���˿�ȴ�����˻�
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_BUYER_RETURN_GOODS; break; 
                            }
                        case "WAIT_SELLER_CONFIRM_GOODS": // �ȴ������ջ�
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_CONFIRM_GOODS; break;
                            }
                        case "REFUND_SUCCESS": //  �˿�ɹ�
                            { 
                                goodstradeloginfo.Status = (int)TradeStatusEnum.REFUND_SUCCESS; break; 
                            }
                    }

                    goodstradeloginfo.Lastupdate = DateTime.Now;

                    TradeLogs.UpdateTradeLog(goodstradeloginfo, goodstradeloginfo.Status, true);
                }
                HttpContext.Current.Response.Write("success");     //���ظ�֧������Ϣ,�ɹ�
            }
            else
            {
                HttpContext.Current.Response.Write("fail");
            }
        }

        /// <summary>
        /// ��ȡԶ�̷�����ATN���
        /// </summary>
        /// <param name="a_strUrl"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public String Get_Http(string strUrl, int timeout, string postData)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                if (Utils.StrIsNullOrEmpty(postData))
                {
                    myReq.Method = "GET";
                }
                else
                {
                    myReq.Method = "POST";
                    byte[] data = new System.Text.UTF8Encoding().GetBytes(postData);
                    myReq.ContentType = "application/x-www-form-urlencoded";
                    myReq.ContentLength = data.Length;
                    Stream newStream = myReq.GetRequestStream();

                    //��������
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);      
                strResult = sr.ReadToEnd();

            }
            catch (Exception exp)
            {
                strResult = "����:" + exp.Message;
            }
            return strResult;
        }

       

        /// <summary>
        /// ���֧�����
        /// </summary>
        /// <returns></returns>
        private bool CheckPayment()
        {
            AliPayConfigInfo aliPayConfigInfo = TradeConfigs.GetConfig().Alipayconfiginfo;
            string aliPayNotifyUrl = "http://notify.alipay.com/trade/notify_query.do?"; 

            aliPayNotifyUrl = aliPayNotifyUrl + "partner=" + aliPayConfigInfo.Partner + "&notify_id=" + DNTRequest.GetString("notify_id");

            //��ȡ֧����ATN���ؽ����true����ȷ�Ķ�����Ϣ��false ����Ч��
            if (Get_Http(aliPayNotifyUrl, 120000, null) != "true")
                return false;

            aliPayNotifyUrl = "http://pay.discuz.net/gateway/alipay.php?_type=alipay&_action=verify&_product=Discuz!NT&_version=" + Discuz.Common.Utils.GetAssemblyVersion();
            //����
            string[] sortedStr = System.Web.HttpContext.Current.Request.Form.AllKeys;

            //����Post�����ݴ�
            StringBuilder prestr = new StringBuilder();
            for (int i = 0; i < sortedStr.Length; i++)
            {
                if (DNTRequest.GetString(sortedStr[i]) != "")
                {
                    if (i == sortedStr.Length - 1)
                        prestr.Append(sortedStr[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(sortedStr[i])));
                    else
                        prestr.Append(sortedStr[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(sortedStr[i])) + "&");
                }
            }

            //�ύ��֧������
            return Get_Http(aliPayNotifyUrl, 120000, prestr.ToString()) == "true";     
        }
    }
}
