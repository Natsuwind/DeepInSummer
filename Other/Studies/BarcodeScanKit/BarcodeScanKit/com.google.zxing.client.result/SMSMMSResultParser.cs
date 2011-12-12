using System;
using System.Collections;
namespace com.google.zxing.client.result
{
	internal sealed class SMSMMSResultParser : ResultParser
	{
		private SMSMMSResultParser()
		{
		}
		public static SMSParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null)
			{
				return null;
			}
			int num;
			if (text.StartsWith("sms:") || text.StartsWith("SMS:") || text.StartsWith("mms:") || text.StartsWith("MMS:"))
			{
				num = 4;
			}
			else
			{
				if (!text.StartsWith("smsto:") && !text.StartsWith("SMSTO:") && !text.StartsWith("mmsto:") && !text.StartsWith("MMSTO:"))
				{
					return null;
				}
				num = 6;
			}
			Hashtable hashtable = ResultParser.parseNameValuePairs(text);
			string subject = null;
			string text2 = null;
			bool flag = false;
			if (hashtable != null && hashtable.Count != 0)
			{
				subject = (string)hashtable["subject"];
				text2 = (string)hashtable["body"];
				flag = true;
			}
			int num2 = text.IndexOf('?', num);
			string text3;
			if (num2 < 0 || !flag)
			{
				text3 = text.Substring(num);
			}
			else
			{
				text3 = text.Substring(num, num2 - num);
			}
			int num3 = text3.IndexOf(';');
			string text4;
			string via;
			if (num3 < 0)
			{
				text4 = text3;
				via = null;
			}
			else
			{
				text4 = text3.Substring(0, num3);
				string text5 = text3.Substring(num3 + 1);
				if (text5.StartsWith("via="))
				{
					via = text5.Substring(4);
				}
				else
				{
					via = null;
				}
			}
			if (text2 == null)
			{
				int num4 = text4.IndexOf(':');
				if (num4 >= 0)
				{
					text2 = text4.Substring(num4 + 1);
					text4 = text4.Substring(0, num4);
				}
			}
			return new SMSParsedResult("sms:" + text4, text4, via, subject, text2, null);
		}
	}
}
