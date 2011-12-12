using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Natsuhime.StoReader.Core
{
    public class WinHotKey
    {
        [DllImport("user32.dll",SetLastError=true)]
		public static extern bool RegisterHotKey(
			IntPtr hWnd,
			int id,
			KeyModifiers fsModifiers,
			Keys vk
			);

		[DllImport("user32.dll",SetLastError=true)]
		public static extern bool UnregisterHotKey(
			IntPtr hWnd,
			int id
			);

		[Flags()]
		public enum KeyModifiers
		{
			None = 0,
			Alt = 1,
			Control =2,
			Shift = 4,
			Windows = 8
		}
    }
}
