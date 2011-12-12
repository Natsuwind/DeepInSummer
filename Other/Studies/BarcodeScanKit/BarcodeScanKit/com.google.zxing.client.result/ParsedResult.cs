using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public abstract class ParsedResult
	{
		private ParsedResultType type;
		public virtual ParsedResultType Type
		{
			get
			{
				return this.type;
			}
		}
		public abstract string DisplayResult
		{
			get;
		}
		protected internal ParsedResult(ParsedResultType type)
		{
			this.type = type;
		}
		public override string ToString()
		{
			return this.DisplayResult;
		}
		public static void maybeAppend(string value_Renamed, StringBuilder result)
		{
			if (value_Renamed != null && value_Renamed.Length > 0)
			{
				if (result.Length > 0)
				{
					result.Append('\n');
				}
				result.Append(value_Renamed);
			}
		}
		public static void maybeAppend(string[] value_Renamed, StringBuilder result)
		{
			if (value_Renamed != null)
			{
				for (int i = 0; i < value_Renamed.Length; i++)
				{
					if (value_Renamed[i] != null && value_Renamed[i].Length > 0)
					{
						if (result.Length > 0)
						{
							result.Append('\n');
						}
						result.Append(value_Renamed[i]);
					}
				}
			}
		}
	}
}
