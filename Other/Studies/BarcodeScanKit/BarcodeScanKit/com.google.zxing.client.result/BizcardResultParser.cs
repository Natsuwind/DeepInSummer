using com.shopsavvy.mango.Model.QRModels;
using System;
using System.Collections;
namespace com.google.zxing.client.result
{
	internal sealed class BizcardResultParser : AbstractDoCoMoResultParser
	{
		public static AddressBookParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || !text.StartsWith("BIZCARD:"))
			{
				return null;
			}
			string firstName = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("N:", text, true);
			string lastName = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("X:", text, true);
			string value_Renamed = BizcardResultParser.buildName(firstName, lastName);
			string title = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("T:", text, true);
			string org = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("C:", text, true);
			string[] addresses = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("A:", text, true);
			string number = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("B:", text, true);
			string number2 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("M:", text, true);
			string number3 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("F:", text, true);
			string value_Renamed2 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("E:", text, true);
			return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed), null, BizcardResultParser.buildPhoneNumbers(number, number2, number3), ResultParser.maybeWrap(value_Renamed2), null, addresses, org, null, title, null);
		}
		private static string[] buildPhoneNumbers(string number1, string number2, string number3)
		{
			ArrayList arrayList = new ArrayList();
			if (number1 != null)
			{
				arrayList.Add(number1);
			}
			if (number2 != null)
			{
				arrayList.Add(number2);
			}
			if (number3 != null)
			{
				arrayList.Add(number3);
			}
			int count = arrayList.Count;
			if (count == 0)
			{
				return null;
			}
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (string)arrayList[i];
			}
			return array;
		}
		private static string buildName(string firstName, string lastName)
		{
			if (firstName == null)
			{
				return lastName;
			}
			if (lastName != null)
			{
				return firstName + ' ' + lastName;
			}
			return firstName;
		}
	}
}
