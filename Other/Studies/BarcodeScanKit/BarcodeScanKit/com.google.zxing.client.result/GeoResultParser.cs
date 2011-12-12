using System;
namespace com.google.zxing.client.result
{
	internal sealed class GeoResultParser : ResultParser
	{
		private GeoResultParser()
		{
		}
		public static GeoParsedResult parse(Result result)
		{
			string text = result.Text;
			if (text == null || (!text.StartsWith("geo:") && !text.StartsWith("GEO:")))
			{
				return null;
			}
			int num = text.IndexOf('?', 4);
			string text2 = (num < 0) ? text.Substring(4) : text.Substring(4, num - 4);
			int num2 = text2.IndexOf(',');
			if (num2 < 0)
			{
				return null;
			}
			int num3 = text2.IndexOf(',', num2 + 1);
			double latitude;
			double longitude;
			double altitude;
			try
			{
				latitude = double.Parse(text2.Substring(0, num2));
				if (num3 < 0)
				{
					longitude = double.Parse(text2.Substring(num2 + 1));
					altitude = 0.0;
				}
				else
				{
					longitude = double.Parse(text2.Substring(num2 + 1, num3 - (num2 + 1)));
					altitude = double.Parse(text2.Substring(num3 + 1));
				}
			}
			catch (FormatException)
			{
				return null;
			}
			return new GeoParsedResult(text.StartsWith("GEO:") ? ("geo:" + text.Substring(4)) : text, latitude, longitude, altitude);
		}
	}
}
