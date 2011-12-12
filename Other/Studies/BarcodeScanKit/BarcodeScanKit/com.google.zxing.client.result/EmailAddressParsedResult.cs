using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class EmailAddressParsedResult : ParsedResult
	{
		private string emailAddress;
		private string subject;
		private string body;
		private string mailtoURI;
		public string EmailAddress
		{
			get
			{
				return this.emailAddress;
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
		public string MailtoURI
		{
			get
			{
				return this.mailtoURI;
			}
		}
		public override string DisplayResult
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(30);
				ParsedResult.maybeAppend(this.emailAddress, stringBuilder);
				ParsedResult.maybeAppend(this.subject, stringBuilder);
				ParsedResult.maybeAppend(this.body, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		internal EmailAddressParsedResult(string emailAddress, string subject, string body, string mailtoURI) : base(ParsedResultType.EMAIL_ADDRESS)
		{
			this.emailAddress = emailAddress;
			this.subject = subject;
			this.body = body;
			this.mailtoURI = mailtoURI;
		}
	}
}
