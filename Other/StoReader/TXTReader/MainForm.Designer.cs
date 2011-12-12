namespace Natsuhime.TXTReader
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenDownloader = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFindText = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoreSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsContentBoxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMiniSize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiShowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReplaceWindowsTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowFormBorder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBookMark2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurrentFilename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbxMain = new System.Windows.Forms.RichTextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiNotifyShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
            this.plFind = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbxFindText = new System.Windows.Forms.TextBox();
            this.btnCloseSearchPanel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.msMainMenu.SuspendLayout();
            this.cmsContentBoxMenu.SuspendLayout();
            this.cmsNotifyMenu.SuspendLayout();
            this.plFind.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMainMenu
            // 
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.tsmiBookmark,
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new System.Drawing.Size(793, 25);
            this.msMainMenu.TabIndex = 0;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenFile,
            this.miOpenDownloader,
            this.tsmiFindText,
            this.miExit});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // miOpenFile
            // 
            this.miOpenFile.Name = "miOpenFile";
            this.miOpenFile.Size = new System.Drawing.Size(124, 22);
            this.miOpenFile.Text = "打开文件";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // miOpenDownloader
            // 
            this.miOpenDownloader.Name = "miOpenDownloader";
            this.miOpenDownloader.Size = new System.Drawing.Size(124, 22);
            this.miOpenDownloader.Text = "打开抓取";
            this.miOpenDownloader.Click += new System.EventHandler(this.miOpenDownloader_Click);
            // 
            // tsmiFindText
            // 
            this.tsmiFindText.Name = "tsmiFindText";
            this.tsmiFindText.Size = new System.Drawing.Size(124, 22);
            this.tsmiFindText.Text = "查找文本";
            this.tsmiFindText.Click += new System.EventHandler(this.tsmiFindText_Click);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(124, 22);
            this.miExit.Text = "退出";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // tsmiBookmark
            // 
            this.tsmiBookmark.Name = "tsmiBookmark";
            this.tsmiBookmark.Size = new System.Drawing.Size(44, 21);
            this.tsmiBookmark.Text = "书签";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMoreSetting});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // tsmiMoreSetting
            // 
            this.tsmiMoreSetting.Name = "tsmiMoreSetting";
            this.tsmiMoreSetting.Size = new System.Drawing.Size(100, 22);
            this.tsmiMoreSetting.Text = "选项";
            this.tsmiMoreSetting.Click += new System.EventHandler(this.tsmiMoreSetting_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(164, 22);
            this.tsmiAbout.Text = "关于&TXTReader";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // cmsContentBoxMenu
            // 
            this.cmsContentBoxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMiniSize,
            this.toolStripSeparator3,
            this.tsmiShowMenu,
            this.tsmiReplaceWindowsTitle,
            this.tsmiShowFormBorder,
            this.toolStripSeparator2,
            this.tsmiAddBookmark,
            this.tsmiBookMark2,
            this.tsmiCurrentFilename,
            this.toolStripSeparator1,
            this.tsmiCopySelected});
            this.cmsContentBoxMenu.Name = "cmsDisplayMainMenu";
            this.cmsContentBoxMenu.Size = new System.Drawing.Size(125, 198);
            this.cmsContentBoxMenu.Text = "右键";
            // 
            // tsmiMiniSize
            // 
            this.tsmiMiniSize.Name = "tsmiMiniSize";
            this.tsmiMiniSize.Size = new System.Drawing.Size(124, 22);
            this.tsmiMiniSize.Text = "最小化";
            this.tsmiMiniSize.Click += new System.EventHandler(this.tsmiMiniSize_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(121, 6);
            // 
            // tsmiShowMenu
            // 
            this.tsmiShowMenu.Name = "tsmiShowMenu";
            this.tsmiShowMenu.Size = new System.Drawing.Size(124, 22);
            this.tsmiShowMenu.Text = "隐藏菜单";
            this.tsmiShowMenu.Click += new System.EventHandler(this.tsmiShowMenu_Click);
            // 
            // tsmiReplaceWindowsTitle
            // 
            this.tsmiReplaceWindowsTitle.Name = "tsmiReplaceWindowsTitle";
            this.tsmiReplaceWindowsTitle.Size = new System.Drawing.Size(124, 22);
            this.tsmiReplaceWindowsTitle.Text = "隐藏标题";
            this.tsmiReplaceWindowsTitle.Click += new System.EventHandler(this.tsmiReplaceWindowsTitle_Click);
            // 
            // tsmiShowFormBorder
            // 
            this.tsmiShowFormBorder.Name = "tsmiShowFormBorder";
            this.tsmiShowFormBorder.Size = new System.Drawing.Size(124, 22);
            this.tsmiShowFormBorder.Text = "隐藏边框";
            this.tsmiShowFormBorder.Click += new System.EventHandler(this.tsmiShowFormBorder_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // tsmiAddBookmark
            // 
            this.tsmiAddBookmark.Name = "tsmiAddBookmark";
            this.tsmiAddBookmark.Size = new System.Drawing.Size(124, 22);
            this.tsmiAddBookmark.Text = "添加书签";
            this.tsmiAddBookmark.Click += new System.EventHandler(this.tsmiAddBookmark_Click);
            // 
            // tsmiBookMark2
            // 
            this.tsmiBookMark2.Name = "tsmiBookMark2";
            this.tsmiBookMark2.Size = new System.Drawing.Size(124, 22);
            this.tsmiBookMark2.Text = "书签";
            // 
            // tsmiCurrentFilename
            // 
            this.tsmiCurrentFilename.Image = global::Natsuhime.TXTReader.Properties.Resources.book_256;
            this.tsmiCurrentFilename.Name = "tsmiCurrentFilename";
            this.tsmiCurrentFilename.Size = new System.Drawing.Size(124, 22);
            this.tsmiCurrentFilename.Text = "标题";
            this.tsmiCurrentFilename.Click += new System.EventHandler(this.tsmiCurrentFilename_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // tsmiCopySelected
            // 
            this.tsmiCopySelected.Name = "tsmiCopySelected";
            this.tsmiCopySelected.Size = new System.Drawing.Size(124, 22);
            this.tsmiCopySelected.Text = "复制";
            this.tsmiCopySelected.Click += new System.EventHandler(this.tsmiCopySelected_Click);
            // 
            // rtbxMain
            // 
            this.rtbxMain.BackColor = System.Drawing.Color.Silver;
            this.rtbxMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbxMain.ContextMenuStrip = this.cmsContentBoxMenu;
            this.rtbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbxMain.Font = new System.Drawing.Font("Microsoft YaHei", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbxMain.Location = new System.Drawing.Point(0, 25);
            this.rtbxMain.Name = "rtbxMain";
            this.rtbxMain.ReadOnly = true;
            this.rtbxMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbxMain.ShowSelectionMargin = true;
            this.rtbxMain.Size = new System.Drawing.Size(793, 419);
            this.rtbxMain.TabIndex = 3;
            this.rtbxMain.Text = "";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.cmsNotifyMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "TXTReader";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // cmsNotifyMenu
            // 
            this.cmsNotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNotifyShow,
            this.tsmiNotifyExit});
            this.cmsNotifyMenu.Name = "cmsNotifyMenu";
            this.cmsNotifyMenu.Size = new System.Drawing.Size(101, 48);
            // 
            // tsmiNotifyShow
            // 
            this.tsmiNotifyShow.Name = "tsmiNotifyShow";
            this.tsmiNotifyShow.Size = new System.Drawing.Size(100, 22);
            this.tsmiNotifyShow.Text = "恢复";
            this.tsmiNotifyShow.Click += new System.EventHandler(this.tsmiNotifyShow_Click);
            // 
            // tsmiNotifyExit
            // 
            this.tsmiNotifyExit.Name = "tsmiNotifyExit";
            this.tsmiNotifyExit.Size = new System.Drawing.Size(100, 22);
            this.tsmiNotifyExit.Text = "退出";
            this.tsmiNotifyExit.Click += new System.EventHandler(this.tsmiNotifyExit_Click);
            // 
            // plFind
            // 
            this.plFind.Controls.Add(this.tableLayoutPanel2);
            this.plFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.plFind.Location = new System.Drawing.Point(0, 25);
            this.plFind.Name = "plFind";
            this.plFind.Size = new System.Drawing.Size(793, 29);
            this.plFind.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Controls.Add(this.tbxFindText, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCloseSearchPanel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnFind, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(793, 29);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // tbxFindText
            // 
            this.tbxFindText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxFindText.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tbxFindText.Location = new System.Drawing.Point(48, 3);
            this.tbxFindText.Name = "tbxFindText";
            this.tbxFindText.Size = new System.Drawing.Size(642, 21);
            this.tbxFindText.TabIndex = 1;
            this.tbxFindText.Enter += new System.EventHandler(this.tbxFindText_Enter);
            this.tbxFindText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxFindText_KeyDown);
            // 
            // btnCloseSearchPanel
            // 
            this.btnCloseSearchPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCloseSearchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseSearchPanel.Location = new System.Drawing.Point(766, 3);
            this.btnCloseSearchPanel.Name = "btnCloseSearchPanel";
            this.btnCloseSearchPanel.Size = new System.Drawing.Size(24, 23);
            this.btnCloseSearchPanel.TabIndex = 3;
            this.btnCloseSearchPanel.Text = "X";
            this.btnCloseSearchPanel.UseVisualStyleBackColor = false;
            this.btnCloseSearchPanel.Click += new System.EventHandler(this.btnCloseSearchPanel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找:";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(696, 3);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(64, 23);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "查找";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 444);
            this.Controls.Add(this.plFind);
            this.Controls.Add(this.rtbxMain);
            this.Controls.Add(this.msMainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMainMenu;
            this.Name = "MainForm";
            this.Text = "空白工程";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            this.cmsContentBoxMenu.ResumeLayout(false);
            this.cmsNotifyMenu.ResumeLayout(false);
            this.plFind.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miOpenFile;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoreSetting;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ContextMenuStrip cmsContentBoxMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowMenu;
        private System.Windows.Forms.RichTextBox rtbxMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiCurrentFilename;
        private System.Windows.Forms.ToolStripMenuItem tsmiReplaceWindowsTitle;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddBookmark;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopySelected;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyShow;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyExit;
        private System.Windows.Forms.Panel plFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox tbxFindText;
        private System.Windows.Forms.ToolStripMenuItem tsmiFindText;
        private System.Windows.Forms.ToolStripMenuItem tsmiBookmark;
        private System.Windows.Forms.ToolStripMenuItem tsmiMiniSize;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowFormBorder;
        private System.Windows.Forms.Button btnCloseSearchPanel;
        private System.Windows.Forms.ToolStripMenuItem miOpenDownloader;
        private System.Windows.Forms.ToolStripMenuItem tsmiBookMark2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

