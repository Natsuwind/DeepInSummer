using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kids
{
    public partial class Form1 : Form
    {
        public static List<Keys> keykids = new List<Keys>();
        static List<int> keykidsint = new List<int>();
        public Form1()
        {
            InitializeComponent();
        }


        protected override void WndProc(ref Message m)//监视Windows消息
        {
            const int WM_HOTKEY = 0x0312;//按快捷键的消息值
            keykidsint.Add(m.Msg);
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    ProcessHotkey();
                    break;
            }
            base.WndProc(ref m);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                //switch (keyData)
                //{
                //    case Keys.Escape:
                //        ProcessHotkey();
                //        break;
                //    case Keys.Control | Keys.F:
                //        //tsmiFindText_Click(this, new EventArgs());
                //        break;
                //}
                keykids.Add(keyData);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void ProcessHotkey()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
