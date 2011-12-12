using System;
using System.Text;
namespace com.google.zxing.client.result
{
	public sealed class GeoParsedResult : ParsedResult
	{
		private string geoURI;
		private double latitude;
		private double longitude;
		private double altitude;
		public string GeoURI
		{
			get
			{
				return this.geoURI;
			}
		}
		public double Latitude
		{
			get
			{
				return this.latitude;
			}
		}
		public double Longitude
		{
			get
			{
				return this.longitude;
			}
		}
		public double Altitude
		{
			get
			{
				return this.altitude;
			}
		}
		public override string DisplayResult
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(50);
				stringBuilder.Append(this.latitude);
				stringBuilder.Append(", ");
				stringBuilder.Append(this.longitude);
				if (this.altitude > 0.0)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(this.altitude);
					stringBuilder.Append('m');
				}
				return stringBuilder.ToString();
			}
		}
		internal GeoParsedResult(string geoURI, double latitude, double longitude, double altitude) : base(ParsedResultType.GEO)
		{
			this.geoURI = geoURI;
			this.latitude = latitude;
			this.longitude = longitude;
			this.altitude = altitude;
		}
	}
}
