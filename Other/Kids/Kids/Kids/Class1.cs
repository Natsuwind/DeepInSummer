﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices; //必须引用
using System.Reflection; //必须引用
namespace Kids
{
    class Hocy_Hook
    {
         #region 私有常量
         ///<summary>
         ///按键状态数组
         ///</summary>
         private readonly byte[] m_KeyState = new byte[ 256 ];
         private string flags;
        //flag=0 正常 flag=1 监控状态 flag=2 屏蔽键盘//
         #endregion 私有常量
         #region 私有变量
         ///<summary>
         ///鼠标钩子句柄
         ///</summary>
         private IntPtr m_pMouseHook = IntPtr.Zero;
         ///<summary>
         ///键盘钩子句柄
         ///</summary>
         private IntPtr m_pKeyboardHook = IntPtr.Zero;
         ///<summary>
         ///鼠标钩子委托实例
         ///</summary>
         ///<remarks>
         ///不要试图省略此变量,否则将会导致
         ///激活CallbackOnCollectedDelegate 托管调试助手(MDA)。
         ///详细请参见MSDN中关于CallbackOnCollectedDelegate 的描述
         ///</remarks>
         //private HookProc m_MouseHookProcedure;
         ///<summary>
         ///键盘钩子委托实例
         ///</summary>
         ///<remarks>
         ///不要试图省略此变量,否则将会导致
         ///激活CallbackOnCollectedDelegate 托管调试助手(MDA)。
         ///详细请参见MSDN中关于CallbackOnCollectedDelegate 的描述
         ///</remarks>
         //private HookProc m_KeyboardHookProcedure;
        // 添加
        public event MouseEventHandler OnMouseActivity;
        private const byte VK_SHIFT = 0x10 ;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;
         #endregion 私有变量
         #region 事件定义
         ///<summary>
         ///鼠标更新事件
         ///</summary>
         ///<remarks>当鼠标移动或者滚轮滚动时触发</remarks>
         //public event MouseUpdateEventHandler OnMouseUpdate;
         ///<summary>
         ///按键按下事件
         ///</summary>
         public event KeyEventHandler OnKeyDown;
         ///<summary>
         ///按键按下并释放事件
         ///</summary>
         public event KeyPressEventHandler OnKeyPress;
         ///<summary>
         ///按键释放事件
         ///</summary>
         public event KeyEventHandler OnKeyUp;
         #endregion 事件定义
         #region 私有方法
         ///<summary>
         ///鼠标钩子处理函数
         ///</summary>
         ///<param name="nCode"></param>
         ///<param name="wParam"></param>
         ///<param name="lParam"></param>
         ///<returns>鼠标钩子处理函数</returns>
         private int MouseHookProc( int nCode, Int32 wParam, IntPtr lParam )
         {
 if ((nCode >= 0) && (OnMouseActivity != null))
            {
                //Marshall the data from callback.
                MouseHookStruct mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                //detect button clicked
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                switch (wParam)
                {
                    case (int)WM_MOUSE.WM_LBUTTONDOWN:
                        //case WM_LBUTTONUP:
                        //case WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        break;
                    case (int)WM_MOUSE.WM_RBUTTONDOWN:
                        //case WM_RBUTTONUP:
                        //case WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        break;
                    case (int)WM_MOUSE.WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of mouseData member is the wheel delta.
                        //One wheel click is defined as WHEEL_DELTA, which is 120.
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData>> 16) & 0xffff);
                        //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
                        //and the low-order word is reserved. This value can be one or more of the following values.
                        //Otherwise, mouseData is not used.
                        break;
                }
                //double clicks
                int clickCount = 0;
                if (button != MouseButtons.None)
                    if (wParam == (int)WM_MOUSE.WM_LBUTTONDBLCLK || wParam == (int)WM_MOUSE.WM_RBUTTONDBLCLK) clickCount = 2;
                    else clickCount = 1;
                //generate event
                MouseEventArgs e = new MouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta);
                //raise it
                OnMouseActivity(this, e);
            }
           
            //*
              return Win32API.CallNextHookEx( this.m_pMouseHook, nCode, wParam, lParam );
         }
         ///<summary>
         ///键盘钩子处理函数
         ///</summary>
         ///<param name="nCode"></param>
         ///<param name="wParam"></param>
         ///<param name="lParam"></param>
         ///<returns></returns>
         ///<remarks></remarks>
         private int KeyboardHookProc( int nCode, Int32 wParam, IntPtr lParam )
         {
          
            switch (flags)
            {
                case "2":
                    return 1;
                    break;
                case "1":
                    break;
            }
            bool handled = false;
            //it was ok and someone listens to events
            if ((nCode >= 0) && (this.OnKeyDown != null || this.OnKeyUp!= null || this.OnKeyPress!= null))
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (this.OnKeyDown != null && (wParam == (int)WM_KEYBOARD.WM_KEYDOWN || wParam == (int)WM_KEYBOARD.WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                   this.OnKeyDown(this, e);
                    handled = handled || e.Handled;
                }
                // raise KeyPress
                    if (this.OnKeyPress != null && wParam == (int)WM_KEYBOARD.WM_KEYDOWN)
                    {
                        bool isDownShift, isDownCapslock;
                        try
                        {
                             isDownShift = ((Win32API.GetKeyStates(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                            isDownCapslock = (Win32API.GetKeyStates(VK_CAPITAL) != 0 ? true : false);
                        }
                        catch
                        {
                            isDownCapslock = false;
                            isDownShift= false;
                        }
                        byte[] keyState = new byte[256];
                       Win32API.GetKeyboardState(keyState);
                        byte[] inBuffer = new byte[2];
                        if (Win32API.ToAscii(MyKeyboardHookStruct.VKCode,
                                  MyKeyboardHookStruct.ScanCode,
                                  keyState,
                                  inBuffer,
                                  MyKeyboardHookStruct.Flags) == 1)
                        {
                            char key = (char)inBuffer[0];
                            if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                            KeyPressEventArgs e = new KeyPressEventArgs(key);
                            this.OnKeyPress(this, e);
                            handled = handled || e.Handled;
                        }
                    }
                // raise KeyUp
                if (this.OnKeyUp != null && (wParam == (int)WM_KEYBOARD.WM_KEYUP || wParam == (int)WM_KEYBOARD.WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.VKCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    this.OnKeyUp(this, e);
                    handled = handled || e.Handled;
                }
            }
            //if event handled in application do not handoff to other listeners
            if (handled)
                return 1;
            else
                return Win32API.CallNextHookEx(this.m_pKeyboardHook, nCode, wParam, lParam);
        }
            
         #endregion 私有方法
         #region 公共方法
         ///<summary>
         ///安装钩子
         ///</summary>
         ///<returns></returns>
         public bool InstallHook(string flagsinfo)
         {
            this.flags = flagsinfo;
IntPtr pInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
//pInstance = (IntPtr)4194304;
          //注意：很多时候得到的pInstanc无法安装钩子，请检查值是否为4194304，在应用程序中可以直接取得pinstance的
           // IntPtr pInstanc2 = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly());
           // Assembly.GetExecutingAssembly().GetModules()[0]
              //安装鼠标钩子
              if ( this.m_pMouseHook == IntPtr.Zero )
              {
                   this.m_MouseHookProcedure = new HookProc( this.MouseHookProc );
                   this.m_pMouseHook = Win32API.SetWindowsHookEx( WH_Codes.WH_MOUSE_LL,this.m_MouseHookProcedure, pInstance, 0 );
// WH_Codes.WH_MOUSE_LL为全局钩子即系统钩子，否则应该为WH_Codes.WH_KEYBOARD，即普通钩子
                   if ( this.m_pMouseHook == IntPtr.Zero )
                   {
                       this.UnInstallHook();
                       return false;
                   }
              }
              if ( this.m_pKeyboardHook == IntPtr.Zero ) //安装键盘钩子
              {
                   this.m_KeyboardHookProcedure = new HookProc( this.KeyboardHookProc );
                  // WH_Codes.WH_KEYBOARD_LL为全局钩子即系统钩子，否则应该为WH_Codes.WH_KEYBOARD，即普通钩子
                   this.m_pKeyboardHook = Win32API.SetWindowsHookEx( WH_Codes.WH_KEYBOARD_LL,this.m_KeyboardHookProcedure, pInstance, 0 );
                   if ( this.m_pKeyboardHook == IntPtr.Zero )
                   {
                       this.UnInstallHook();
                       return false;
                   }
              }
              return true;
         }
         ///<summary>
         ///卸载钩子
         ///</summary>
         ///<returns></returns>
         public bool UnInstallHook()
         {
              bool result = true;
              if ( this.m_pMouseHook != IntPtr.Zero )
              {
                   result = ( Win32API.UnhookWindowsHookEx( this.m_pMouseHook ) && result );
                   this.m_pMouseHook = IntPtr.Zero;
              }
              if ( this.m_pKeyboardHook != IntPtr.Zero )
              {
                   result = ( Win32API.UnhookWindowsHookEx( this.m_pKeyboardHook ) && result );
                   this.m_pKeyboardHook = IntPtr.Zero;
              }
              return result;
         }
         #endregion 公共方法
         #region 构造函数
         ///<summary>
         ///钩子类
         ///</summary>
         ///<remarks>本类仅仅简单实现了WH_KEYBOARD_LL 以及WH_MOUSE_LL </remarks>
         public Hocy_Hook()
         {
              Win32API.GetKeyboardState( this.m_KeyState );
         }
         #endregion 构造函数
    }
}