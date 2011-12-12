using System;
namespace com.google.zxing.client.result
{
	public class ISBNResultParser : ResultParser
	{
		private ISBNResultParser()
		{
		}
		public static ISBNParsedResult parse(Result result)
		{
			BarcodeFormat barcodeFormat = result.BarcodeFormat;
			if (!BarcodeFormat.EAN_13.Equals(barcodeFormat))
			{
				return null;
			}
			string text = result.Text;
			if (text == null)
			{
				return null;
			}
			int length = text.Length;
			if (length != 13)
			{
				return null;
			}
			if (!text.StartsWith("978") && !text.StartsWith("979"))
			{
				return null;
			}
			return new ISBNParsedResult(text);
		}
	}
}
