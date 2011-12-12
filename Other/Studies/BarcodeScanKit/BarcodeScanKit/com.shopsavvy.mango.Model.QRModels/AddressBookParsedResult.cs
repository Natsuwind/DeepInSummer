using com.google.zxing.client.result;
using System;
using System.Text;
namespace com.shopsavvy.mango.Model.QRModels
{
	public sealed class AddressBookParsedResult : ParsedResult
	{
		private string[] names;
		private string pronunciation;
		private string[] phoneNumbers;
		private string[] emails;
		private string note;
		private string[] addresses;
		private string org;
		private string birthday;
		private string title;
		private string url;
		public string[] Names
		{
			get
			{
				return this.names;
			}
		}
		public string Pronunciation
		{
			get
			{
				return this.pronunciation;
			}
		}
		public string[] PhoneNumbers
		{
			get
			{
				return this.phoneNumbers;
			}
		}
		public string[] Emails
		{
			get
			{
				return this.emails;
			}
		}
		public string Note
		{
			get
			{
				return this.note;
			}
		}
		public string[] Addresses
		{
			get
			{
				return this.addresses;
			}
		}
		public string Title
		{
			get
			{
				return this.title;
			}
		}
		public string Org
		{
			get
			{
				return this.org;
			}
		}
		public string URL
		{
			get
			{
				return this.url;
			}
		}
		public string Birthday
		{
			get
			{
				return this.birthday;
			}
		}
		public override string DisplayResult
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(100);
				ParsedResult.maybeAppend(this.names, stringBuilder);
				ParsedResult.maybeAppend(this.pronunciation, stringBuilder);
				ParsedResult.maybeAppend(this.title, stringBuilder);
				ParsedResult.maybeAppend(this.org, stringBuilder);
				ParsedResult.maybeAppend(this.addresses, stringBuilder);
				ParsedResult.maybeAppend(this.phoneNumbers, stringBuilder);
				ParsedResult.maybeAppend(this.emails, stringBuilder);
				ParsedResult.maybeAppend(this.url, stringBuilder);
				ParsedResult.maybeAppend(this.birthday, stringBuilder);
				ParsedResult.maybeAppend(this.note, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		public AddressBookParsedResult(string[] names, string pronunciation, string[] phoneNumbers, string[] emails, string note, string[] addresses, string org, string birthday, string title, string url) : base(ParsedResultType.ADDRESSBOOK)
		{
			this.names = names;
			this.pronunciation = pronunciation;
			this.phoneNumbers = phoneNumbers;
			this.emails = emails;
			this.note = note;
			this.addresses = addresses;
			this.org = org;
			this.birthday = birthday;
			this.title = title;
			this.url = url;
		}
	}
}
