using com.google.zxing.oned;
using System;
namespace com.google.zxing.client.result
{
	internal sealed class ProductResultParser : ResultParser
	{
		private ProductResultParser()
		{
		}
		public static ProductParsedResult parse(Result result)
		{
			BarcodeFormat barcodeFormat = result.BarcodeFormat;
			if (!BarcodeFormat.UPC_A.Equals(barcodeFormat) && !BarcodeFormat.UPC_E.Equals(barcodeFormat) && !BarcodeFormat.EAN_8.Equals(barcodeFormat) && !BarcodeFormat.EAN_13.Equals(barcodeFormat))
			{
				return null;
			}
			string text = result.Text;
			if (text == null)
			{
				return null;
			}
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (c < '0' || c > '9')
				{
					return null;
				}
			}
			string normalizedProductID;
			if (BarcodeFormat.UPC_E.Equals(barcodeFormat))
			{
				normalizedProductID = UPCEReader.convertUPCEtoUPCA(text);
			}
			else
			{
				normalizedProductID = text;
			}
			return new ProductParsedResult(text, normalizedProductID);
		}
	}
}
