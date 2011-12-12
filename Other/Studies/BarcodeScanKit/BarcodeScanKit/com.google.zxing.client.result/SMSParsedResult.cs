using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class SMSParsedResult : ParsedResult
	{
		private string smsURI;
		private string number;
		private string via;
		private string subject;
		private string body;
		private string title;
		public string SMSURI
		{
			get
			{
				return this.smsURI;
			}
		}
		public string Number
		{
			get
			{
				return this.number;
			}
		}
		public string Via
		{
			get
			{
				return this.via;
			}
		}
		public string Subject
		{
			get
			{
				return this.subject;
			}
		}
		public string Body
		{
			get
			{
				return this.body;
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
				StringBuilder stringBuilder = new StringBuilder(100);
				ParsedResult.maybeAppend(this.number, stringBuilder);
				ParsedResult.maybeAppend(this.via, stringBuilder);
				ParsedResult.maybeAppend(this.subject, stringBuilder);
				ParsedResult.maybeAppend(this.body, stringBuilder);
				ParsedResult.maybeAppend(this.title, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		public SMSParsedResult(string smsURI, string number, string via, string subject, string body, string title) : base(ParsedResultType.SMS)
		{
			this.smsURI = smsURI;
			this.number = number;
			this.via = via;
			this.subject = subject;
			this.body = body;
			this.title = title;
		}
	}
}
