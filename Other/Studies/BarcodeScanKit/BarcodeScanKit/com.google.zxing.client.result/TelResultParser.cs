using System;
namespace com.google.zxing.client.result
{
	internal sealed class TelResultParser : ResultParser
	{
		private TelResultParser()
		{
		}
		public static TelParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || (!text.StartsWith("tel:") && !text.StartsWith("TEL:")))
			{
				return null;
			}
			string telURI = text.StartsWith("TEL:") ? ("tel:" + text.Substring(4)) : text;
			int num = text.IndexOf('?', 4);
			string number = (num < 0) ? text.Substring(4) : text.Substring(4, num - 4);
			return new TelParsedResult(number, telURI, null);
		}
	}
}
