using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime.Web;
using Natsuhime;

namespace Suwako
{
    public partial class MainForm : Form
    {
        NewHttper httper = null;


        public MainForm()
        {
            InitializeComponent();
            this.httper = new NewHttper();
            this.httper.Cookie = new System.Net.CookieContainer();
            this.httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_RequestStringCompleted);
        }

        void httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string[] urls = Utils.GetImageUrls(e.ResponseString);
                if (urls.Length > 0)
                {
                    MessageBox.Show(string.Format("{0} Images Getted~", urls.Length));
                    foreach (string url in urls)
                    {
                        tbxResult.Text += url + Environment.NewLine;
                    }
                }
                else
                {
                    MessageBox.Show("No Image Getted!");
                }
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.httper.Url = this.tbxUrl.Text.Trim();
            this.httper.Charset = this.httper.GetPageLanguageCode();
            this.httper.RequestStringAsync(EnumRequestMethod.GET, this.httper.Url);
        }
    }
}
