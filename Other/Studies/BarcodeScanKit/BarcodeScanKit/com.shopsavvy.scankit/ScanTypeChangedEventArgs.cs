using com.shopsavvy.scankit.Helpers;
using System;
namespace com.shopsavvy.scankit
{
	public class ScanTypeChangedEventArgs : EventArgs
	{
		public CodeType NewScanType
		{
			get;
			private set;
		}
		public ScanTypeChangedEventArgs(CodeType newScanType)
		{
			this.NewScanType = newScanType;
		}
	}
}
