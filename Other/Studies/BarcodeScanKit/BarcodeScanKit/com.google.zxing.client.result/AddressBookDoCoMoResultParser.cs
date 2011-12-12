using com.shopsavvy.mango.Model.QRModels;
using System;
namespace com.google.zxing.client.result
{
	internal sealed class AddressBookDoCoMoResultParser : AbstractDoCoMoResultParser
	{
		public static AddressBookParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || !text.StartsWith("MECARD:"))
			{
				return null;
			}
			string[] array = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("N:", text, true);
			if (array == null)
			{
				return null;
			}
			string value_Renamed = AddressBookDoCoMoResultParser.parseName(array[0]);
			string pronunciation = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("SOUND:", text, true);
			string[] phoneNumbers = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("TEL:", text, true);
			string[] emails = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("EMAIL:", text, true);
			string note = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("NOTE:", text, false);
			string[] addresses = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("ADR:", text, true);
			string text2 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("BDAY:", text, true);
			if (text2 != null && !ResultParser.isStringOfDigits(text2, 8))
			{
				text2 = null;
			}
			string url = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("URL:", text, true);
			string org = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("ORG:", text, true);
			return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed), pronunciation, phoneNumbers, emails, note, addresses, org, text2, null, url);
		}
		private static string parseName(string name)
		{
			int num = name.IndexOf(',');
			if (num >= 0)
			{
				return name.Substring(num + 1) + ' ' + name.Substring(0, num);
			}
			return name;
		}
	}
}
