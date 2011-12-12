using System;
namespace com.google.zxing.client.result
{
	internal sealed class EmailDoCoMoResultParser : AbstractDoCoMoResultParser
	{
		private static readonly char[] ATEXT_SYMBOLS = new char[]
		{
			'@', 
			'.', 
			'!', 
			'#', 
			'$', 
			'%', 
			'&', 
			'\'', 
			'*', 
			'+', 
			'-', 
			'/', 
			'=', 
			'?', 
			'^', 
			'_', 
			'`', 
			'{', 
			'|', 
			'}', 
			'~'
		};
		public static EmailAddressParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || !text.StartsWith("MATMSG:"))
			{
				return null;
			}
			string[] array = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("TO:", text, true);
			if (array == null)
			{
				return null;
			}
			string text2 = array[0];
			if (!EmailDoCoMoResultParser.isBasicallyValidEmailAddress(text2))
			{
				return null;
			}
			string subject = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("SUB:", text, false);
			string body = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("BODY:", text, false);
			return new EmailAddressParsedResult(text2, subject, body, "mailto:" + text2);
		}
		internal static bool isBasicallyValidEmailAddress(string email)
		{
			if (email == null)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < email.Length; i++)
			{
                char c = email[i];
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && !EmailDoCoMoResultParser.isAtextSymbol(c))
				{
					return false;
				}
				if (c == '@')
				{
					if (flag)
					{
						return false;
					}
					flag = true;
				}
			}
			return flag;
		}
		private static bool isAtextSymbol(char c)
		{
			for (int i = 0; i < EmailDoCoMoResultParser.ATEXT_SYMBOLS.Length; i++)
			{
				if (c == EmailDoCoMoResultParser.ATEXT_SYMBOLS[i])
				{
					return true;
				}
			}
			return false;
		}
	}
}
