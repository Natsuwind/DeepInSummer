using System;
namespace com.google.zxing.client.result
{
	internal sealed class BookmarkDoCoMoResultParser : AbstractDoCoMoResultParser
	{
		private BookmarkDoCoMoResultParser()
		{
		}
		public static URIParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || !text.StartsWith("MEBKM:"))
			{
				return null;
			}
			string title = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("TITLE:", text, true);
			string[] array = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("URL:", text, true);
			if (array == null)
			{
				return null;
			}
			string uri = array[0];
			if (!URIResultParser.isBasicallyValidURI(uri))
			{
				return null;
			}
			return new URIParsedResult(uri, title);
		}
	}
}
