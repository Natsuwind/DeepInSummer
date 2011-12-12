using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace KeyboardHook
{
    public partial class Hk_Form : Form
    {
        private Hocy_Hook hook_Main = new Hocy_Hook();
        public Hk_Form()
        {
            InitializeComponent();

            this.hook_Main.OnMouseActivity += new MouseEventHandler(hook_MainMouseMove);
            this.hook_Main.OnKeyDown += new KeyEventHandler(hook_MainKeyDown);
            this.hook_Main.OnKeyPress += new KeyPressEventHandler(hook_MainKeyPress);
            this.hook_Main.OnKeyUp += new KeyEventHandler(hook_MainKeyUp);
        }

        private void Hk_Form_Load(object sender, EventArgs e)
        {

        }

        private void Hk_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.hook_Main.UnInstallHook();
        }
        private void HookMain_OnKeyDown(object sender, KeyEventArgs e)
        {
            // MessageBox.Show("33");
            if (e.KeyCode == Keys.Escape && Control.ModifierKeys == Keys.Shift)
            {
                this.Close();
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            hook_Main.InstallHook("1");
        }

        private void stop_Click(object sender, EventArgs e)
        {
            this.hook_Main.UnInstallHook();
        }

        private void stopkeyboard_Click(object sender, EventArgs e)
        {
            hook_Main.InstallHook("2");
        }

        void ShowM(string message, bool isNewLine)
        {
            if (isNewLine)
            {
                this.resultinfo.AppendText(
                    string.Format("{0}{1}{0}", Environment.NewLine, message)
                    );
            }
            else
            {
                this.resultinfo.AppendText(message);
            }
            this.resultinfo.SelectionStart = this.resultinfo.Text.Length;
        }

        private void ShowM(string message)
        {
            this.ShowM(message, false);
        }

        private void hook_MainKeyDown(object sender, KeyEventArgs e)
        {
            //LogWrite("KeyDown 	- " + e.KeyData.ToString());
        }

        private void hook_MainKeyPress(object sender, KeyPressEventArgs e)
        {
            ShowM(e.KeyChar.ToString());
        }

        private void hook_MainKeyUp(object sender, KeyEventArgs e)
        {
            //LogWrite("KeyUp 		- " + e.KeyData.ToString());
        }

        private void hook_MainMouseMove(object sender, MouseEventArgs e)
        {
            //labelMousePosition.Text = String.Format("x={0}  y={1} wheel={2}", e.X, e.Y, e.Delta);
            if (e.Clicks > 0)
            {
                ShowM(
                    string.Format(
                        "[Mouse {0}]x={1},y={2},wheel={3}",
                        e.Button,
                        e.X,
                        e.Y,
                        e.Delta
                        ),
                    true
                    );
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (File.Exists("d:\\temp\\show.kids"))
            {
                this.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.resultinfo.Visible = !this.resultinfo.Visible;
        }


    }
}