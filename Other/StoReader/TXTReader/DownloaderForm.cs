using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Natsuhime.StoReader.Entities;
using System.IO;

namespace Natsuhime.TXTReader
{
    public partial class DownloaderForm : Form
    {
        NewHttper httper = null;
        List<WebContentListInfo> WebContentLists = null;
        public DownloaderForm()
        {
            InitializeComponent();
            httper = new NewHttper();
            httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_RequestStringCompleted);
        }

        void httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                if (e.ResponseString != string.Empty)
                {
                    if (e.UserState.ToString() == "GETLIST")
                    {
                        #region 获取列表
                        #region 正则
                        MatchCollection mc = RegexUtility.GetMatchFull(e.ResponseString, tbxPreListRegex.Text.Trim(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        if (mc != null)
                        {
                            foreach (Match m in mc)
                            {
                                WebContentListInfo wcli = new WebContentListInfo();
                                wcli.Title = m.Groups[1].Value.Trim();
                                wcli.ContentList = new List<WebContentInfo>();

                                MatchCollection mcContent = RegexUtility.GetMatchFull(m.Groups[2].Value, tbxListRegex.Text.Trim());
                                if (mcContent != null)
                                {
                                    foreach (Match mContent in mcContent)
                                    {
                                        WebContentInfo wci = new WebContentInfo();
                                        wci.Title = mContent.Groups[2].Value.Trim();
                                        wci.Url = mContent.Groups[1].Value.Trim();

                                        wcli.ContentList.Add(wci);
                                    }

                                    WebContentLists.Add(wcli);
                                }
                            }
                        }
                        #endregion
                        #region 显示
                        this.lvList.Items.Clear();
                        foreach (WebContentInfo info in WebContentLists[0].ContentList)
                        {
                            ListViewItem lv = new ListViewItem(info.Title);

                            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem(lv, info.Url);
                            ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem(lv, info.Title);

                            lv.SubItems.Add(lvsi);
                            lv.SubItems.Add(lvsi2);

                            this.lvList.Items.Add(lv);
                        }
                        MessageBox.Show(this, string.Format("{0}章完成！", WebContentLists.Count));
                        this.btnGetList.Enabled = true;
                        #endregion}
                        #endregion
                    }
                    else
                    {
                        #region 获取内容
                        WebContentInfo wci = (WebContentInfo)e.UserState;

                        string content = RegexUtility.ReplaceRegex(
                            tbxPreContentRegex.Text.Trim(),
                            e.ResponseString,
                            "",
                            RegexOptions.Singleline | RegexOptions.IgnoreCase
                            );
                        MatchCollection mc = RegexUtility.GetMatchFull(
                            content,
                            tbxContentRegex.Text.Trim(),
                            RegexOptions.Singleline | RegexOptions.IgnoreCase
                            );
                        if (mc != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(wci.Title + Environment.NewLine);
                            foreach (Match m in mc)
                            {
                                if (m.Groups[1].Value.IndexOf("盗墓笔记网友留言") < 0)
                                {
                                    sb.Append(m.Groups[1].Value + Environment.NewLine);
                                }
                            }
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);


                            string savepath = Path.Combine(
                                AppDomain.CurrentDomain.BaseDirectory,
                                "down"
                                );
                            if (!Directory.Exists(savepath))
                            {
                                Directory.CreateDirectory(savepath);
                            }

                            try
                            {
                                FileStream fs = new FileStream(
                                    Path.Combine(savepath, this.WebContentLists[0].Title + ".txt"),
                                    FileMode.Append
                                    );
                                StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true, true));
                                sw.Write(Natsuhime.Web.Utils.RemoveHtml(sb.ToString()));
                                sw.Flush();
                                sw.Close();
                                fs.Close();


                                this.WebContentLists[0].ContentList.Remove(wci);
                                DownNextContent();
                            }
                            catch (Exception ex)
                            {
                                this.btnGetContent.Enabled = true;
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    MessageBox.Show("返回空");
                }
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            this.WebContentLists = new List<WebContentListInfo>();
            this.httper.Url = this.cbbxListUrl.Text;
            this.httper.Charset = this.cbbxCharset.Text;
            this.httper.RequestStringAsync(EnumRequestMethod.GET, "GETLIST");
            this.btnGetList.Enabled = false;
        }

        private void btnGetContent_Click(object sender, EventArgs e)
        {
            DownNextContent();
        }

        private void DownNextContent()
        {
            WebContentInfo wci = GetNextContent();
            if (wci != null)
            {
                this.httper.Url = wci.Url;
                this.httper.Charset = this.cbbxCharset.Text;
                this.httper.RequestStringAsync(EnumRequestMethod.GET, wci);
                this.btnGetContent.Enabled = false;
            }
            else
            {
                MessageBox.Show(this, "全部完成！");
            }
        }

        WebContentInfo GetNextContent()
        {
            while (this.WebContentLists.Count > 0)
            {
                WebContentListInfo wcli = this.WebContentLists[0];
                if (wcli.ContentList.Count > 0)
                {
                    return wcli.ContentList[0];
                }
                else
                {
                    this.WebContentLists.Remove(wcli);
                }
            }
            return null;
        }
    }
}
