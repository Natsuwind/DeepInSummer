using System;
namespace com.google.zxing.client.result
{
	internal sealed class URIResultParser : ResultParser
	{
		private URIResultParser()
		{
		}
		public static URIParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text != null && text.StartsWith("URL:"))
			{
				text = text.Substring(4);
			}
			if (!URIResultParser.isBasicallyValidURI(text))
			{
				return null;
			}
			return new URIParsedResult(text, null);
		}
		internal static bool isBasicallyValidURI(string uri)
		{
			if (uri == null || uri.IndexOf(' ') >= 0 || uri.IndexOf('\n') >= 0)
			{
				return false;
			}
			int num = uri.IndexOf('.');
			if (num >= uri.Length - 2)
			{
				return false;
			}
			int num2 = uri.IndexOf(':');
			if (num < 0 && num2 < 0)
			{
				return false;
			}
			if (num2 >= 0)
			{
				if (num < 0 || num > num2)
				{
					for (int i = 0; i < num2; i++)
					{
						char c = uri[i];
						if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
						{
							return false;
						}
					}
				}
				else
				{
					if (num2 >= uri.Length - 2)
					{
						return false;
					}
					for (int j = num2 + 1; j < num2 + 3; j++)
					{
                        char c2 = uri[j];
						if (c2 < '0' || c2 > '9')
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
