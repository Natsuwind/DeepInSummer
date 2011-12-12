using System;
namespace com.google.zxing.client.result
{
	public sealed class ProductParsedResult : ParsedResult
	{
		private string productID;
		private string normalizedProductID;
		public string ProductID
		{
			get
			{
				return this.productID;
			}
		}
		public string NormalizedProductID
		{
			get
			{
				return this.normalizedProductID;
			}
		}
		public override string DisplayResult
		{
			get
			{
				return this.productID;
			}
		}
		internal ProductParsedResult(string productID) : this(productID, productID)
		{
		}
		internal ProductParsedResult(string productID, string normalizedProductID) : base(ParsedResultType.PRODUCT)
		{
			this.productID = productID;
			this.normalizedProductID = normalizedProductID;
		}
	}
}
