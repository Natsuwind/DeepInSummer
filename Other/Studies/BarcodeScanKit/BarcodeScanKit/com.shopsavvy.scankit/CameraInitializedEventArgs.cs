using System;
namespace com.shopsavvy.scankit
{
	public class CameraInitializedEventArgs : EventArgs
	{
		public bool IsCameraInitialized
		{
			get;
			private set;
		}
		public CameraInitializedEventArgs(bool isCameraInitialized)
		{
			this.IsCameraInitialized = isCameraInitialized;
		}
	}
}
