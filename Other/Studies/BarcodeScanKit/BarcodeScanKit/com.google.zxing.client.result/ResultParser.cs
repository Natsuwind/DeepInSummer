using System;
using System.Collections;
using System.Text;
namespace com.google.zxing.client.result
{
	public abstract class ResultParser
	{
		public static ParsedResult parseResult(Result theResult)
		{
			ParsedResult result;
			if ((result = BookmarkDoCoMoResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = AddressBookDoCoMoResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = EmailDoCoMoResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = EmailAddressResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = AddressBookAUResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = VCardResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = BizcardResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = VEventResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = TelResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = SMSMMSResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = GeoResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = URLTOResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = URIResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = ISBNResultParser.parse(theResult)) != null)
			{
				return result;
			}
			if ((result = ProductResultParser.parse(theResult)) != null)
			{
				return result;
			}
			return new TextParsedResult(theResult.Text, null);
		}
		protected internal static void maybeAppend(string value_Renamed, StringBuilder result)
		{
			if (value_Renamed != null)
			{
				result.Append('\n');
				result.Append(value_Renamed);
			}
		}
		protected internal static void maybeAppend(string[] value_Renamed, StringBuilder result)
		{
			if (value_Renamed != null)
			{
				for (int i = 0; i < value_Renamed.Length; i++)
				{
					result.Append('\n');
					result.Append(value_Renamed[i]);
				}
			}
		}
		protected internal static string[] maybeWrap(string value_Renamed)
		{
			if (value_Renamed != null)
			{
				return new string[]
				{
					value_Renamed
				};
			}
			return null;
		}
		protected internal static string unescapeBackslash(string escaped)
		{
			if (escaped != null)
			{
				int num = escaped.IndexOf('\\');
				if (num >= 0)
				{
					int length = escaped.Length;
					StringBuilder stringBuilder = new StringBuilder(length - 1);
					stringBuilder.Append(escaped.ToCharArray(), 0, num);
					bool flag = false;
					for (int i = num; i < length; i++)
					{
                        char c = escaped[i];
						if (flag || c != '\\')
						{
							stringBuilder.Append(c);
							flag = false;
						}
						else
						{
							flag = true;
						}
					}
					return stringBuilder.ToString();
				}
			}
			return escaped;
		}
		internal static string urlDecode(string escaped)
		{
			if (escaped == null)
			{
				return null;
			}
			char[] array = escaped.ToCharArray();
			int num = ResultParser.findFirstEscape(array);
			if (num < 0)
			{
				return escaped;
			}
			int num2 = array.Length;
			StringBuilder stringBuilder = new StringBuilder(num2 - 2);
			stringBuilder.Append(array, 0, num);
			for (int i = num; i < num2; i++)
			{
				char c = array[i];
				if (c == '+')
				{
					stringBuilder.Append(' ');
				}
				else
				{
					if (c == '%')
					{
						if (i >= num2 - 2)
						{
							stringBuilder.Append('%');
						}
						else
						{
							int num3 = ResultParser.parseHexDigit(array[++i]);
							int num4 = ResultParser.parseHexDigit(array[++i]);
							if (num3 < 0 || num4 < 0)
							{
								stringBuilder.Append('%');
								stringBuilder.Append(array[i - 1]);
								stringBuilder.Append(array[i]);
							}
							stringBuilder.Append((char)((num3 << 4) + num4));
						}
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
			}
			return stringBuilder.ToString();
		}
		private static int findFirstEscape(char[] escapedArray)
		{
			int num = escapedArray.Length;
			for (int i = 0; i < num; i++)
			{
				char c = escapedArray[i];
				if (c == '+' || c == '%')
				{
					return i;
				}
			}
			return -1;
		}
		private static int parseHexDigit(char c)
		{
			if (c >= 'a')
			{
				if (c <= 'f')
				{
					return (int)('\n' + (c - 'a'));
				}
			}
			else
			{
				if (c >= 'A')
				{
					if (c <= 'F')
					{
						return (int)('\n' + (c - 'A'));
					}
				}
				else
				{
					if (c >= '0' && c <= '9')
					{
						return (int)(c - '0');
					}
				}
			}
			return -1;
		}
		protected internal static bool isStringOfDigits(string value_Renamed, int length)
		{
			if (value_Renamed == null)
			{
				return false;
			}
			int length2 = value_Renamed.Length;
			if (length != length2)
			{
				return false;
			}
			for (int i = 0; i < length; i++)
			{
                char c = value_Renamed[i];
				if (c < '0' || c > '9')
				{
					return false;
				}
			}
			return true;
		}
		internal static Hashtable parseNameValuePairs(string uri)
		{
			int num = uri.IndexOf('?');
			if (num < 0)
			{
				return null;
			}
			Hashtable result = new Hashtable();
			num++;
			int num2;
			while ((num2 = uri.IndexOf('&', num)) >= 0)
			{
				ResultParser.appendKeyValue(uri, num, num2, result);
				num = num2 + 1;
			}
			ResultParser.appendKeyValue(uri, num, uri.Length, result);
			return result;
		}
		private static void appendKeyValue(string uri, int paramStart, int paramEnd, Hashtable result)
		{
			int num = uri.IndexOf('=', paramStart);
			if (num >= 0)
			{
				string text = uri.Substring(paramStart, num - paramStart);
				string text2 = uri.Substring(num + 1, paramEnd - (num + 1));
				text2 = ResultParser.urlDecode(text2);
				result.Add(text, text2);
			}
		}
		internal static string[] matchPrefixedField(string prefix, string rawText, char endChar, bool trim)
		{
			ArrayList arrayList = null;
			int num = 0;
			int length = rawText.Length;
			string text = rawText;
			int num2 = 0;
			while (num < length || arrayList == null)
			{
				num = text.IndexOf(prefix);
				if (num < 0)
				{
					break;
				}
				text = text.Substring(num);
				num2 += num;
				num = num2;
				num += prefix.Length;
				int num3 = num;
				bool flag = false;
				while (!flag)
				{
					num = rawText.IndexOf(endChar, num);
					if (num < 0)
					{
						num = rawText.Length;
						flag = true;
					}
					else
					{
                        if (rawText[num - 1] != '\\')
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							string text2 = ResultParser.unescapeBackslash(rawText.Substring(num3, num - num3));
							if (trim)
							{
								text2 = text2.Trim();
							}
							arrayList.Add(text2);
							return ResultParser.toStringArray(arrayList);
						}
						num++;
					}
				}
			}
			if (arrayList == null || arrayList.Count == 0)
			{
				return null;
			}
			return ResultParser.toStringArray(arrayList);
		}
		internal static string matchSinglePrefixedField(string prefix, string rawText, char endChar, bool trim)
		{
			string[] array = ResultParser.matchPrefixedField(prefix, rawText, endChar, trim);
			if (array != null)
			{
				return array[0];
			}
			return null;
		}
		internal static string[] toStringArray(ArrayList strings)
		{
			int count = strings.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (string)strings[i];
			}
			return array;
		}
	}
}
