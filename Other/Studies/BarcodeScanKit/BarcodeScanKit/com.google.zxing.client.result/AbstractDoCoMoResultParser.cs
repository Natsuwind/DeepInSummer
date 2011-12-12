using System;
namespace com.google.zxing.client.result
{
	internal abstract class AbstractDoCoMoResultParser : ResultParser
	{
		internal static string[] matchDoCoMoPrefixedField(string prefix, string rawText, bool trim)
		{
			return ResultParser.matchPrefixedField(prefix, rawText, ';', trim);
		}
		internal static string matchSingleDoCoMoPrefixedField(string prefix, string rawText, bool trim)
		{
			return ResultParser.matchSinglePrefixedField(prefix, rawText, ';', trim);
		}
	}
}
