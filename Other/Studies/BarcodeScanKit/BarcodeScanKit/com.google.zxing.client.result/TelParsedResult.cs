using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class TelParsedResult : ParsedResult
	{
		private string number;
		private string telURI;
		private string title;
		public string Number
		{
			get
			{
				return this.number;
			}
		}
		public string TelURI
		{
			get
			{
				return this.telURI;
			}
		}
		public string Title
		{
			get
			{
				return this.title;
			}
		}
		public override string DisplayResult
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(20);
				ParsedResult.maybeAppend(this.number, stringBuilder);
				ParsedResult.maybeAppend(this.title, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		public TelParsedResult(string number, string telURI, string title) : base(ParsedResultType.TEL)
		{
			this.number = number;
			this.telURI = telURI;
			this.title = title;
		}
	}
}
