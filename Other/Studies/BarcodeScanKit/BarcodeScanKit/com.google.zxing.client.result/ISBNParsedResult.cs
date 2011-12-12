using System;
namespace com.google.zxing.client.result
{
	public sealed class ISBNParsedResult : ParsedResult
	{
		private string isbn;
		public string ISBN
		{
			get
			{
				return this.isbn;
			}
		}
		public override string DisplayResult
		{
			get
			{
				return this.isbn;
			}
		}
		internal ISBNParsedResult(string isbn) : base(ParsedResultType.ISBN)
		{
			this.isbn = isbn;
		}
	}
}
