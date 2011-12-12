using com.shopsavvy.mango.Model.QRModels;
using System;
using System.Collections;
namespace com.google.zxing.client.result
{
	internal sealed class AddressBookAUResultParser : ResultParser
	{
		public static AddressBookParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || text.IndexOf("MEMORY") < 0 || text.IndexOf("\r\n") < 0)
			{
				return null;
			}
			string value_Renamed = ResultParser.matchSinglePrefixedField("NAME1:", text, '\r', true);
			string pronunciation = ResultParser.matchSinglePrefixedField("NAME2:", text, '\r', true);
			string[] phoneNumbers = AddressBookAUResultParser.matchMultipleValuePrefix("TEL", 3, text, true);
			string[] emails = AddressBookAUResultParser.matchMultipleValuePrefix("MAIL", 3, text, true);
			string note = ResultParser.matchSinglePrefixedField("MEMORY:", text, '\r', false);
			string text2 = ResultParser.matchSinglePrefixedField("ADD:", text, '\r', true);
			string[] addresses = (text2 == null) ? null : new string[]
			{
				text2
			};
			return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed), pronunciation, phoneNumbers, emails, note, addresses, null, null, null, null);
		}
		private static string[] matchMultipleValuePrefix(string prefix, int max, string rawText, bool trim)
		{
			ArrayList arrayList = null;
			for (int i = 1; i <= max; i++)
			{
				string text = ResultParser.matchSinglePrefixedField(prefix + i + ':', rawText, '\r', trim);
				if (text == null)
				{
					break;
				}
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				arrayList.Add(text);
			}
			if (arrayList == null)
			{
				return null;
			}
			return ResultParser.toStringArray(arrayList);
		}
	}
}
