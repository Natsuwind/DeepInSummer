using com.shopsavvy.mango.Model.QRModels;
using System;
using System.Collections;
using System.Text;
namespace com.google.zxing.client.result
{
	internal sealed class VCardResultParser : ResultParser
	{
		private VCardResultParser()
		{
		}
		public static AddressBookParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || !text.StartsWith("BEGIN:VCARD"))
			{
				return null;
			}
			string[] array = VCardResultParser.matchVCardPrefixedField("FN", text, true);
			if (array == null)
			{
				array = VCardResultParser.matchVCardPrefixedField("N", text, true);
				VCardResultParser.formatNames(array);
			}
			string[] phoneNumbers = VCardResultParser.matchVCardPrefixedField("TEL", text, true);
			string[] emails = VCardResultParser.matchVCardPrefixedField("EMAIL", text, true);
			string note = VCardResultParser.matchSingleVCardPrefixedField("NOTE", text, false);
			string[] array2 = VCardResultParser.matchVCardPrefixedField("ADR", text, true);
			if (array2 != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = VCardResultParser.formatAddress(array2[i]);
				}
			}
			string org = VCardResultParser.matchSingleVCardPrefixedField("ORG", text, true);
			string text2 = VCardResultParser.matchSingleVCardPrefixedField("BDAY", text, true);
			if (!VCardResultParser.isLikeVCardDate(text2))
			{
				text2 = null;
			}
			string title = VCardResultParser.matchSingleVCardPrefixedField("TITLE", text, true);
			string url = VCardResultParser.matchSingleVCardPrefixedField("URL", text, true);
			return new AddressBookParsedResult(array, null, phoneNumbers, emails, note, array2, org, text2, title, url);
		}
		private static string[] matchVCardPrefixedField(string prefix, string rawText, bool trim)
		{
			ArrayList arrayList = null;
			int i = 0;
			int length = rawText.Length;
			while (i < length)
			{
				i = rawText.IndexOf(prefix, i);
				if (i < 0)
				{
					break;
				}
				if (i > 0 && rawText[i - 1] != '\n')
				{
					i++;
				}
				else
				{
					i += prefix.Length;
					if (rawText[i] == ':' || rawText[i] == ';')
					{
						while (rawText[i] != ':')
						{
							i++;
						}
						i++;
						int num = i;
						i = rawText.IndexOf('\n', i);
						if (i < 0)
						{
							i = length;
						}
						else
						{
							if (i > num)
							{
								if (arrayList == null)
								{
									arrayList = new ArrayList();
								}
								string text = rawText.Substring(num, i - num);
								if (trim)
								{
									text = text.Trim();
								}
								arrayList.Add(text);
								i++;
							}
							else
							{
								i++;
							}
						}
					}
				}
			}
			if (arrayList == null || arrayList.Count == 0)
			{
				return null;
			}
			return ResultParser.toStringArray(arrayList);
		}
		internal static string matchSingleVCardPrefixedField(string prefix, string rawText, bool trim)
		{
			string[] array = VCardResultParser.matchVCardPrefixedField(prefix, rawText, trim);
			if (array != null)
			{
				return array[0];
			}
			return null;
		}
		private static bool isLikeVCardDate(string value_Renamed)
		{
            return value_Renamed == null || ResultParser.isStringOfDigits(value_Renamed, 8) || (value_Renamed.Length == 10 && value_Renamed[4] == '-' && value_Renamed[7] == '-' && VCardResultParser.isSubstringOfDigits(value_Renamed, 0, 4) && VCardResultParser.isSubstringOfDigits(value_Renamed, 5, 2) && VCardResultParser.isSubstringOfDigits(value_Renamed, 8, 2));
		}
		private static bool isSubstringOfDigits(string s, int startIndex, int length)
		{
			if (s.Length >= startIndex + length)
			{
				string text = s.Substring(startIndex, length);
				try
				{
					int num = Convert.ToInt32(text);
					bool result;
					if (num != 0)
					{
						result = true;
						return result;
					}
					result = false;
					return result;
				}
				catch
				{
					bool result = false;
					return result;
				}
				return false;
			}
			return false;
		}
		private static string formatAddress(string address)
		{
			if (address == null)
			{
				return null;
			}
			int length = address.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
                char c = address[i];
				if (c == ';')
				{
					stringBuilder.Append(' ');
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString().Trim();
		}
		private static void formatNames(string[] names)
		{
			if (names != null)
			{
				for (int i = 0; i < names.Length; i++)
				{
					string text = names[i];
					string[] array = new string[5];
					int num = 0;
					int num2 = 0;
					int num3;
					while ((num3 = text.IndexOf(';', num)) > 0)
					{
						array[num2] = text.Substring(num, num3 - num);
						num2++;
						num = num3 + 1;
					}
					array[num2] = text.Substring(num);
					StringBuilder stringBuilder = new StringBuilder(100);
					VCardResultParser.maybeAppendComponent(array, 3, stringBuilder);
					VCardResultParser.maybeAppendComponent(array, 1, stringBuilder);
					VCardResultParser.maybeAppendComponent(array, 2, stringBuilder);
					VCardResultParser.maybeAppendComponent(array, 0, stringBuilder);
					VCardResultParser.maybeAppendComponent(array, 4, stringBuilder);
					names[i] = stringBuilder.ToString().Trim();
				}
			}
		}
		private static void maybeAppendComponent(string[] components, int i, StringBuilder newName)
		{
			if (components[i] != null)
			{
				newName.Append(' ');
				newName.Append(components[i]);
			}
		}
	}
}
