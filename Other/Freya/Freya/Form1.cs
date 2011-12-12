using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Freya
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.ShowInTaskbar = false;
            //this.Opacity = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "test.lnk");


            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(targetPath);
            shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            shortcut.WorkingDirectory = System.Environment.CurrentDirectory;
            shortcut.WindowStyle = 1;
            shortcut.Description = "test";
            shortcut.IconLocation = Path.Combine(System.Environment.SystemDirectory, "shell32.dll, 165");

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }
            shortcut.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
            this.BackgroundImage = img;
        }
    }
}
