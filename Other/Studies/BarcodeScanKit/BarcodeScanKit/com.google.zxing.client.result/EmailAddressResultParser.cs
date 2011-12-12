using System;
using System.Collections;
namespace com.google.zxing.client.result
{
	internal sealed class EmailAddressResultParser : ResultParser
	{
		public static EmailAddressParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null)
			{
				return null;
			}
			string text2;
			if (text.StartsWith("mailto:") || text.StartsWith("MAILTO:"))
			{
				text2 = text.Substring(7);
				int num = text2.IndexOf('?');
				if (num >= 0)
				{
					text2 = text2.Substring(0, num);
				}
				Hashtable hashtable = ResultParser.parseNameValuePairs(text);
				string subject = null;
				string body = null;
				if (hashtable != null)
				{
					if (text2.Length == 0)
					{
						text2 = (string)hashtable["to"];
					}
					subject = (string)hashtable["subject"];
					body = (string)hashtable["body"];
				}
				return new EmailAddressParsedResult(text2, subject, body, text);
			}
			if (!EmailDoCoMoResultParser.isBasicallyValidEmailAddress(text))
			{
				return null;
			}
			text2 = text;
			return new EmailAddressParsedResult(text2, null, null, "mailto:" + text2);
		}
	}
}
