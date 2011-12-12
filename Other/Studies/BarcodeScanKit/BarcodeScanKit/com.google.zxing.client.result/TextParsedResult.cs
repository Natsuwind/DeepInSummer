using System;
namespace com.google.zxing.client.result
{
	public sealed class TextParsedResult : ParsedResult
	{
		private string text;
		private string language;
		public string Text
		{
			get
			{
				return this.text;
			}
		}
		public string Language
		{
			get
			{
				return this.language;
			}
		}
		public override string DisplayResult
		{
			get
			{
				return this.text;
			}
		}
		public TextParsedResult(string text, string language) : base(ParsedResultType.TEXT)
		{
			this.text = text;
			this.language = language;
		}
	}
}
