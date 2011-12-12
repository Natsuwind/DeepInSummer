using System;
namespace com.google.zxing.client.result
{
	public sealed class ParsedResultType
	{
		public static readonly ParsedResultType ADDRESSBOOK = new ParsedResultType("ADDRESSBOOK");
		public static readonly ParsedResultType EMAIL_ADDRESS = new ParsedResultType("EMAIL_ADDRESS");
		public static readonly ParsedResultType PRODUCT = new ParsedResultType("PRODUCT");
		public static readonly ParsedResultType URI = new ParsedResultType("URI");
		public static readonly ParsedResultType TEXT = new ParsedResultType("TEXT");
		public static readonly ParsedResultType ANDROID_INTENT = new ParsedResultType("ANDROID_INTENT");
		public static readonly ParsedResultType GEO = new ParsedResultType("GEO");
		public static readonly ParsedResultType TEL = new ParsedResultType("TEL");
		public static readonly ParsedResultType SMS = new ParsedResultType("SMS");
		public static readonly ParsedResultType CALENDAR = new ParsedResultType("CALENDAR");
		public static readonly ParsedResultType NDEF_SMART_POSTER = new ParsedResultType("NDEF_SMART_POSTER");
		public static readonly ParsedResultType MOBILETAG_RICH_WEB = new ParsedResultType("MOBILETAG_RICH_WEB");
		public static readonly ParsedResultType ISBN = new ParsedResultType("ISBN");
		private string name;
		private ParsedResultType(string name)
		{
			this.name = name;
		}
		public override string ToString()
		{
			return this.name;
		}
	}
}
