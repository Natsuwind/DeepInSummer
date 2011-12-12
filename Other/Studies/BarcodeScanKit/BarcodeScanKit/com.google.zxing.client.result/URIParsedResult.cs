using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class URIParsedResult : ParsedResult
	{
		private string uri;
		private string title;
		public string URI
		{
			get
			{
				return this.uri;
			}
		}
		public string Title
		{
			get
			{
				return this.title;
			}
		}
		public bool PossiblyMaliciousURI
		{
			get
			{
				return this.containsUser();
			}
		}
		public override string DisplayResult
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(30);
				ParsedResult.maybeAppend(this.title, stringBuilder);
				ParsedResult.maybeAppend(this.uri, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		public URIParsedResult(string uri, string title) : base(ParsedResultType.URI)
		{
			this.uri = URIParsedResult.massageURI(uri);
			this.title = title;
		}
		private bool containsUser()
		{
			int num = this.uri.IndexOf(':');
			num++;
			int length = this.uri.Length;
			while (num < length && this.uri[num] == '/')
			{
				num++;
			}
			int num2 = this.uri.IndexOf('/', num);
			if (num2 < 0)
			{
				num2 = length;
			}
			int num3 = this.uri.IndexOf('@', num);
			return num3 >= num && num3 < num2;
		}
		private static string massageURI(string uri)
		{
			int num = uri.IndexOf(':');
			if (num < 0)
			{
				uri = "http://" + uri;
			}
			else
			{
				if (URIParsedResult.isColonFollowedByPortNumber(uri, num))
				{
					uri = "http://" + uri;
				}
				else
				{
					uri = uri.Substring(0, num).ToLower() + uri.Substring(num);
				}
			}
			return uri;
		}
		private static bool isColonFollowedByPortNumber(string uri, int protocolEnd)
		{
			int num = uri.IndexOf('/', protocolEnd + 1);
			if (num < 0)
			{
				num = uri.Length;
			}
			if (num <= protocolEnd + 1)
			{
				return false;
			}
			for (int i = protocolEnd + 1; i < num; i++)
			{
                if (uri[i] < '0' || uri[i] > '9')
				{
					return false;
				}
			}
			return true;
		}
	}
}
