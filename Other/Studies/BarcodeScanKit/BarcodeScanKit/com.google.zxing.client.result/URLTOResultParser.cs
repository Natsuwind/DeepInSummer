using System;
namespace com.google.zxing.client.result
{
	internal sealed class URLTOResultParser
	{
		private URLTOResultParser()
		{
		}
		public static URIParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || (!text.StartsWith("urlto:") && !text.StartsWith("URLTO:")))
			{
				return null;
			}
			int num = text.IndexOf(':', 6);
			if (num < 0)
			{
				return null;
			}
			string title = (num <= 6) ? null : text.Substring(6, num - 6);
			string uri = text.Substring(num + 1);
			return new URIParsedResult(uri, title);
		}
	}
}
