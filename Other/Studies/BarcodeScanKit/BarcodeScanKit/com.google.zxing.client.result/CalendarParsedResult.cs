using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class CalendarParsedResult : ParsedResult
	{
		private string summary;
		private string start;
		private string end;
		private string location;
		private string attendee;
		private string title;
		public string Summary
		{
			get
			{
				return this.summary;
			}
		}
		public string Start
		{
			get
			{
				return this.start;
			}
		}
		public string End
		{
			get
			{
				return this.end;
			}
		}
		public string Location
		{
			get
			{
				return this.location;
			}
		}
		public string Attendee
		{
			get
			{
				return this.attendee;
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
				ParsedResult.maybeAppend(this.summary, stringBuilder);
				ParsedResult.maybeAppend(this.start, stringBuilder);
				ParsedResult.maybeAppend(this.end, stringBuilder);
				ParsedResult.maybeAppend(this.location, stringBuilder);
				ParsedResult.maybeAppend(this.attendee, stringBuilder);
				ParsedResult.maybeAppend(this.title, stringBuilder);
				return stringBuilder.ToString();
			}
		}
		public CalendarParsedResult(string summary, string start, string end, string location, string attendee, string title) : base(ParsedResultType.CALENDAR)
		{
			if (start == null)
			{
				throw new ArgumentException();
			}
			CalendarParsedResult.validateDate(start);
			CalendarParsedResult.validateDate(end);
			this.summary = summary;
			this.start = start;
			this.end = end;
			this.location = location;
			this.attendee = attendee;
			this.title = title;
		}
		private static void validateDate(string date)
		{
			if (date != null)
			{
				int length = date.Length;
				if (length != 8 && length != 15 && length != 16)
				{
					throw new ArgumentException();
				}
				for (int i = 0; i < 8; i++)
				{
					if (!char.IsDigit(date[i]))
					{
						throw new ArgumentException();
					}
				}
				if (length > 8)
				{
					if (date[8] != 'T')
					{
						throw new ArgumentException();
					}
					for (int j = 9; j < 15; j++)
					{
						if (!char.IsDigit(date[j]))
						{
							throw new ArgumentException();
						}
					}
                    if (length == 16 && date[15] != 'Z')
					{
						throw new ArgumentException();
					}
				}
			}
		}
	}
}
