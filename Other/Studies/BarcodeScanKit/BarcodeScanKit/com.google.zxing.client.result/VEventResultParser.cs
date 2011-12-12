using System;
namespace com.google.zxing.client.result
{
	internal sealed class VEventResultParser : ResultParser
	{
		private VEventResultParser()
		{
		}
		public static CalendarParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null)
			{
				return null;
			}
			int num = text.IndexOf("BEGIN:VEVENT");
			if (num < 0)
			{
				return null;
			}
			int num2 = text.IndexOf("END:VEVENT");
			if (num2 < 0)
			{
				return null;
			}
			string summary = VCardResultParser.matchSingleVCardPrefixedField("SUMMARY", text, true);
			string start = VCardResultParser.matchSingleVCardPrefixedField("DTSTART", text, true);
			string end = VCardResultParser.matchSingleVCardPrefixedField("DTEND", text, true);
			CalendarParsedResult result2;
			try
			{
				result2 = new CalendarParsedResult(summary, start, end, null, null, null);
			}
			catch (ArgumentException)
			{
				result2 = null;
			}
			return result2;
		}
	}
}
