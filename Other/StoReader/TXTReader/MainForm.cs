using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Natsuhime.StoReader.Entities;
using Natsuhime.StoReader.Core;

namespace Natsuhime.TXTReader
{
    public partial class MainForm : Form
    {
        static Dictionary<string, object> cache = new Dictionary<string, object>();
        string currentfilepath;
        List<BookmarkInfo> bookmarklist;
        object GetCache(string cachekey)
        {
            if (cache.ContainsKey(cachekey))
            {
                return cache[cachekey];
            }
            else
            {
                return null;
            }
        }
        void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "打开文件:";
            //ofd.Filter = "Microsoft Excel files (*.xls;*.csv)|*.xls;*.csv|All files (*.*)|*.*";
            ofd.Filter = "已知格式(*.txt;*.rtf;*.doc;*.docx)|*.txt;*.rtf;*.doc;*.docx|文本格式(*.txt)|*.txt|RTF格式(*.rtf)|*.rtf|Word格式(*.doc;*.docx)|*.doc;*.docx|所有格式|*.*";
            if (GetCache("lastdir") != null)
            {
                ofd.InitialDirectory = GetCache("lastdir").ToString();
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filepath = ofd.FileName;
                OpenFile(filepath, 0);
            }
        }
        void OpenFile(string filepath, int selectionstart)
        {
            cache["lastdir"] = Path.GetDirectoryName(filepath);
            string fileext = Path.GetExtension(filepath).ToLower();
            if (fileext == ".doc" || fileext == ".docx")
            {
                object oMissing = System.Reflection.Missing.Value;
                object isReadOnly = true;
                Microsoft.Office.Interop.Word._Application oWord;
                Microsoft.Office.Interop.Word._Document oDoc;

                oWord = new Microsoft.Office.Interop.Word.Application();
                oWord.Visible = false;
                object fileName = filepath;
                oDoc = oWord.Documents.Open(ref fileName,
                ref oMissing, ref isReadOnly, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                this.rtbxMain.Text = oDoc.Content.Text;
                oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                if (MessageBox.Show("转换为RTF文档并重新打开?", "转换格式", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string newrtfpath = filepath + ".rtf";
                    this.rtbxMain.SaveFile(newrtfpath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("转换为rtf成功.\r\n保存位置:" + newrtfpath, "转换格式");
                    OpenFile(newrtfpath, selectionstart);
                    return;
                }
            }
            else
            {
                FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default, true);

                if (fileext == ".rtf")
                {
                    this.rtbxMain.Rtf = sr.ReadToEnd();
                    this.rtbxMain.Font = new Font("微软雅黑", 14f);
                }
                else
                {
                    rtbxMain.Text = sr.ReadToEnd();
                }
                sr.Close();
                fs.Close();
                fs.Dispose();
                sr.Dispose();
            }
            rtbxMain.SelectionStart = selectionstart;
            currentfilepath = filepath;
            this.Text = Path.GetFileNameWithoutExtension(currentfilepath);
            this.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(MainForm)).GetObject("$this.Icon")));
            tsmiReplaceWindowsTitle.Text = "隐藏标题";
            this.tsmiCurrentFilename.Text = this.Text;
            this.notifyIcon1.Text = this.Text;
        }
        void InitBookmarkMenu()
        {
            this.tsmiBookmark.DropDownItems.Clear();
            this.tsmiBookMark2.DropDownItems.Clear();
            bookmarklist = Bookmark.GetBookmarkList();

            foreach (BookmarkInfo info in bookmarklist)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(info.BookName, null, new EventHandler(this.DynMenuItem_Click), info.id);
                ToolStripMenuItem item2 = new ToolStripMenuItem(info.BookName, null, new EventHandler(this.DynMenuItem_Click), info.id);
                this.tsmiBookmark.DropDownItems.Add(item);
                this.tsmiBookMark2.DropDownItems.Add(item2);
            }
        }

        void TXTReaderExit()
        {
            this.FormClosing -= new FormClosingEventHandler(MainForm_FormClosing);
            UnRegGloableHotKey();
            Application.Exit();
        }
        public MainForm()
        {
            InitializeComponent();
            InitBookmarkMenu();
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.Deactivate += new EventHandler(MainForm_Deactivate);

            this.plFind.Enabled = false;
            this.plFind.Visible = false;

            RegGloableHotKey();
        }

        #region 快捷键
        int gloableHotKeyID;
        void RegGloableHotKey()
        {
            gloableHotKeyID = new Random(999).Next(100, 9999);
            WinHotKey.KeyModifiers keyMod = WinHotKey.KeyModifiers.Windows;
            WinHotKey.RegisterHotKey(Handle, gloableHotKeyID, keyMod, Keys.Oem3);//Windows图标键+`
        }
        void UnRegGloableHotKey()
        {
            WinHotKey.UnregisterHotKey(Handle, gloableHotKeyID);
        }
        protected override void WndProc(ref Message m)//监视Windows消息
        {
            const int WM_HOTKEY = 0x0312;//按快捷键的消息值
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
                switch (keyData)
                {
                    case Keys.Escape:
                        ProcessHotkey();
                        break;
                    case Keys.Control | Keys.F:
                        tsmiFindText_Click(this, new EventArgs());
                        break;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void ProcessHotkey()
        {
            if (this.Visible == true)
            {
                tsmiMiniSize_Click(this, new EventArgs());
                this.notifyIcon1.Visible = false;
            }
            else
            {
                notifyIcon1_MouseDoubleClick(this, null);
            }
        }
        #endregion

        void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
            }
        }


        #region 菜单栏
        private void miOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        //打开下载器
        private void miOpenDownloader_Click(object sender, EventArgs e)
        {
            new DownloaderForm().Show();
        }
        private void miExit_Click(object sender, EventArgs e)
        {
            TXTReaderExit();
        }
        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }


        //查找文本
        private void tsmiFindText_Click(object sender, EventArgs e)
        {
            if (this.plFind.Visible)
            {
                this.plFind.Enabled = false;
                this.plFind.Visible = false;
            }
            else
            {
                this.plFind.Enabled = true;
                this.plFind.Visible = true;
                this.tbxFindText.Focus();
            }
        }
        //查找条关闭按钮
        private void btnCloseSearchPanel_Click(object sender, EventArgs e)
        {
            tsmiFindText_Click(sender, e);
        }
        //从书签打开
        private void DynMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            List<BookmarkInfo> bmlist = bookmarklist.FindAll(
                delegate(BookmarkInfo info)
                {
                    return info.id == item.Name;
                }
                );
            OpenFile(bmlist[0].FilePath, bmlist[0].MarkedIndex);
        }
        //设置
        private void tsmiMoreSetting_Click(object sender, EventArgs e)
        {
            DialogResult dr = new SettingForm().ShowDialog();
            //new SettingForm().Show();
            if (dr == DialogResult.OK)
            {
                this.rtbxMain.SuspendLayout();
                this.rtbxMain.BackColor = Color.FromArgb(Convert.ToInt32(G.BackColor, 16));//.FromName(G.BackColor);
                this.rtbxMain.ForeColor = Color.FromArgb(Convert.ToInt32(G.FontColor, 16));
                this.rtbxMain.Font = new Font(this.rtbxMain.Font.Name, G.FontSize, this.rtbxMain.Font.Style);
                //this.rtbxMain.Refresh();                
                this.rtbxMain.ResumeLayout();
            }
        }
        #endregion
        //查找
        private void btnFind_Click(object sender, EventArgs e)
        {
            string searchtext = tbxFindText.Text.Trim();
            this.rtbxMain.HideSelection = false;
            int indexOfText = this.rtbxMain.Find(searchtext, this.rtbxMain.SelectionStart + this.rtbxMain.SelectionLength, RichTextBoxFinds.None);
            if (indexOfText > -1)
            {
                //rtbxMain.Select(indexOfText, this.rtbxMain.SelectionLength);
            }
            else
            {
                MessageBox.Show(String.Format("找不到“{0}”...", searchtext));
                this.rtbxMain.SelectionStart = 0;
                this.rtbxMain.SelectionLength = 0;
            }
        }
        #region 右键
        //显示菜单栏
        private void tsmiShowMenu_Click(object sender, EventArgs e)
        {
            if (msMainMenu.Visible)
            {
                msMainMenu.Visible = false;
                tsmiShowMenu.Text = "显示菜单";
            }
            else
            {
                msMainMenu.Visible = true;
                tsmiShowMenu.Text = "隐藏菜单";
            }
        }
        //显示窗口标题
        private void tsmiReplaceWindowsTitle_Click(object sender, EventArgs e)
        {
            if (this.Text == "测试窗体")
            {
                this.Text = this.tsmiCurrentFilename.Text;
                this.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(MainForm)).GetObject("$this.Icon")));
                tsmiReplaceWindowsTitle.Text = "隐藏标题";
            }
            else
            {
                this.Text = "测试窗体";
                this.Icon = null;
                tsmiReplaceWindowsTitle.Text = "显示标题";
            }
            this.notifyIcon1.Text = this.Text;
        }
        //显示边框和标题栏
        private void tsmiShowFormBorder_Click(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;//重置为正常
                //this.rtbxMain.ScrollBars = RichTextBoxScrollBars.Vertical;
                tsmiShowFormBorder.Text = "隐藏边框";
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                //this.rtbxMain.ScrollBars = RichTextBoxScrollBars.None;
                tsmiShowFormBorder.Text = "显示边框";
            }
        }
        //添加书签
        private void tsmiAddBookmark_Click(object sender, EventArgs e)
        {
            if (currentfilepath != null && currentfilepath != string.Empty)
            {
                int markedindex = rtbxMain.GetCharIndexFromPosition(new Point(0, 0));
                //int xx2 = rtbxMain.GetFirstCharIndexFromLine(1);

                List<BookmarkInfo> bmlist = bookmarklist.FindAll(
                delegate(BookmarkInfo info)
                {
                    return info.FilePath == currentfilepath;
                }
                );
                if (bmlist.Count > 0)
                {
                    bmlist[0].MarkedIndex = markedindex;
                    Bookmark.Update(bmlist[0]);
                    MessageBox.Show("更新书签成功.");
                }
                else
                {
                    BookmarkInfo info;
                    info = new BookmarkInfo();
                    info.BookName = this.tsmiCurrentFilename.Text;
                    info.ClassID = 0;
                    info.MarkedIndex = markedindex;
                    info.FilePath = currentfilepath;
                    Bookmark.Add(info);
                    MessageBox.Show("添加书签成功.");
                }
                InitBookmarkMenu();
            }
            else
            {
                MessageBox.Show("还没有打开任何文件,无法加入书签.");
            }
        }
        //复制
        private void tsmiCopySelected_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbxMain.SelectedText, TextDataFormat.UnicodeText);
        }
        //打开文件(显示标题)
        private void tsmiCurrentFilename_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        //最小化
        private void tsmiMiniSize_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon1.Visible = true;
        }
        #endregion

        //关闭到任务栏
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tsmiMiniSize_Click(sender, e);
            e.Cancel = true;
        }
        //从任务栏显示
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.notifyIcon1.Visible = false;
        }
        //任务栏右键显示
        private void tsmiNotifyShow_Click(object sender, EventArgs e)
        {
            this.notifyIcon1_MouseDoubleClick(this, null);
        }
        //任务栏右键退出
        private void tsmiNotifyExit_Click(object sender, EventArgs e)
        {
            TXTReaderExit();
            //System.Windows.Forms.Application.AddMessageFilter
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            OpenFile(path, 0);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            //检查是否是允许拖拽类型(文件型)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tbxFindText_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.btnFind.PerformClick();
                    return;
                //case Keys.Escape:
                //    this.plFind.Visible = false;
                //    return;
            }
        }

        private void tbxFindText_Enter(object sender, EventArgs e)
        {
            this.tbxFindText.SelectAll();
        }


    }
}
