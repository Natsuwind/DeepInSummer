using com.shopsavvy.scankit.Helpers;
using System;
namespace com.shopsavvy.scankit
{
	public class ScanSuccessfulEventArgs : EventArgs
	{
		public CodeType ScanType
		{
			get;
			private set;
		}
		public string RawScanResult
		{
			get;
			private set;
		}
		public object ScanResult
		{
			get;
			private set;
		}
		public bool IsScanTimeCalculated
		{
			get;
			private set;
		}
		public TimeSpan ScanTime
		{
			get;
			private set;
		}
		public ScanSuccessfulEventArgs(CodeType scanType, string rawScanResult, object scanResult, bool isScanTimeCalculated, TimeSpan scanTime)
		{
			this.ScanType = scanType;
			this.RawScanResult = rawScanResult;
			this.ScanResult = scanResult;
			this.IsScanTimeCalculated = isScanTimeCalculated;
			this.ScanTime = scanTime;
		}
	}
}
