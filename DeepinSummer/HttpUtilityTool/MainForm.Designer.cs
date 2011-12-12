namespace HttpUtilityTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.cbxUrl = new System.Windows.Forms.ComboBox();
            this.tbxPostData = new System.Windows.Forms.TextBox();
            this.tcMessage = new System.Windows.Forms.TabControl();
            this.tpHtml = new System.Windows.Forms.TabPage();
            this.wbMain = new System.Windows.Forms.WebBrowser();
            this.tpText = new System.Windows.Forms.TabPage();
            this.tbxMain = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSaveResult = new System.Windows.Forms.Button();
            this.btnClearResult = new System.Windows.Forms.Button();
            this.plPostDataBtn = new System.Windows.Forms.Panel();
            this.btnSavePostData = new System.Windows.Forms.Button();
            this.btnClearPostData = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelFavUrl = new System.Windows.Forms.Button();
            this.btnFavUrl = new System.Windows.Forms.Button();
            this.ckbxIsPost = new System.Windows.Forms.CheckBox();
            this.ckbxUserWebBrowser = new System.Windows.Forms.CheckBox();
            this.ckbxKeepCookie = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.cbxPageCharset = new System.Windows.Forms.ComboBox();
            this.tlpMain.SuspendLayout();
            this.tcMessage.SuspendLayout();
            this.tpHtml.SuspendLayout();
            this.tpText.SuspendLayout();
            this.panel2.SuspendLayout();
            this.plPostDataBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tlpMain.Controls.Add(this.cbxUrl, 0, 0);
            this.tlpMain.Controls.Add(this.tbxPostData, 0, 2);
            this.tlpMain.Controls.Add(this.tcMessage, 0, 3);
            this.tlpMain.Controls.Add(this.panel2, 1, 3);
            this.tlpMain.Controls.Add(this.plPostDataBtn, 1, 2);
            this.tlpMain.Controls.Add(this.panel1, 0, 1);
            this.tlpMain.Controls.Add(this.btnSend, 1, 1);
            this.tlpMain.Controls.Add(this.cbxPageCharset, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(656, 420);
            this.tlpMain.TabIndex = 0;
            // 
            // cbxUrl
            // 
            this.cbxUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxUrl.FormattingEnabled = true;
            this.cbxUrl.Location = new System.Drawing.Point(3, 3);
            this.cbxUrl.Name = "cbxUrl";
            this.cbxUrl.Size = new System.Drawing.Size(564, 20);
            this.cbxUrl.TabIndex = 10;
            // 
            // tbxPostData
            // 
            this.tbxPostData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxPostData.Location = new System.Drawing.Point(3, 58);
            this.tbxPostData.Multiline = true;
            this.tbxPostData.Name = "tbxPostData";
            this.tbxPostData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxPostData.Size = new System.Drawing.Size(564, 82);
            this.tbxPostData.TabIndex = 9;
            // 
            // tcMessage
            // 
            this.tcMessage.Controls.Add(this.tpHtml);
            this.tcMessage.Controls.Add(this.tpText);
            this.tcMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMessage.Location = new System.Drawing.Point(3, 146);
            this.tcMessage.Name = "tcMessage";
            this.tcMessage.SelectedIndex = 0;
            this.tcMessage.Size = new System.Drawing.Size(564, 271);
            this.tcMessage.TabIndex = 8;
            // 
            // tpHtml
            // 
            this.tpHtml.Controls.Add(this.wbMain);
            this.tpHtml.Location = new System.Drawing.Point(4, 22);
            this.tpHtml.Name = "tpHtml";
            this.tpHtml.Padding = new System.Windows.Forms.Padding(3);
            this.tpHtml.Size = new System.Drawing.Size(556, 245);
            this.tpHtml.TabIndex = 0;
            this.tpHtml.Text = "网页";
            this.tpHtml.UseVisualStyleBackColor = true;
            // 
            // wbMain
            // 
            this.wbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbMain.Location = new System.Drawing.Point(3, 3);
            this.wbMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMain.Name = "wbMain";
            this.wbMain.Size = new System.Drawing.Size(550, 239);
            this.wbMain.TabIndex = 0;
            // 
            // tpText
            // 
            this.tpText.Controls.Add(this.tbxMain);
            this.tpText.Location = new System.Drawing.Point(4, 22);
            this.tpText.Name = "tpText";
            this.tpText.Padding = new System.Windows.Forms.Padding(3);
            this.tpText.Size = new System.Drawing.Size(556, 245);
            this.tpText.TabIndex = 1;
            this.tpText.Text = "内容";
            this.tpText.UseVisualStyleBackColor = true;
            // 
            // tbxMain
            // 
            this.tbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxMain.Location = new System.Drawing.Point(3, 3);
            this.tbxMain.Multiline = true;
            this.tbxMain.Name = "tbxMain";
            this.tbxMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMain.Size = new System.Drawing.Size(550, 239);
            this.tbxMain.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSaveResult);
            this.panel2.Controls.Add(this.btnClearResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(573, 357);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(80, 60);
            this.panel2.TabIndex = 7;
            // 
            // btnSaveResult
            // 
            this.btnSaveResult.Location = new System.Drawing.Point(3, 3);
            this.btnSaveResult.Name = "btnSaveResult";
            this.btnSaveResult.Size = new System.Drawing.Size(75, 23);
            this.btnSaveResult.TabIndex = 1;
            this.btnSaveResult.Text = "Save";
            this.btnSaveResult.UseVisualStyleBackColor = true;
            // 
            // btnClearResult
            // 
            this.btnClearResult.Location = new System.Drawing.Point(3, 32);
            this.btnClearResult.Name = "btnClearResult";
            this.btnClearResult.Size = new System.Drawing.Size(75, 23);
            this.btnClearResult.TabIndex = 0;
            this.btnClearResult.Text = "Clear";
            this.btnClearResult.UseVisualStyleBackColor = true;
            this.btnClearResult.Click += new System.EventHandler(this.btnClearResult_Click);
            // 
            // plPostDataBtn
            // 
            this.plPostDataBtn.Controls.Add(this.btnSavePostData);
            this.plPostDataBtn.Controls.Add(this.btnClearPostData);
            this.plPostDataBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plPostDataBtn.Location = new System.Drawing.Point(573, 81);
            this.plPostDataBtn.Name = "plPostDataBtn";
            this.plPostDataBtn.Size = new System.Drawing.Size(80, 59);
            this.plPostDataBtn.TabIndex = 6;
            // 
            // btnSavePostData
            // 
            this.btnSavePostData.Location = new System.Drawing.Point(2, 3);
            this.btnSavePostData.Name = "btnSavePostData";
            this.btnSavePostData.Size = new System.Drawing.Size(75, 23);
            this.btnSavePostData.TabIndex = 1;
            this.btnSavePostData.Text = "Save";
            this.btnSavePostData.UseVisualStyleBackColor = true;
            // 
            // btnClearPostData
            // 
            this.btnClearPostData.Location = new System.Drawing.Point(3, 31);
            this.btnClearPostData.Name = "btnClearPostData";
            this.btnClearPostData.Size = new System.Drawing.Size(75, 23);
            this.btnClearPostData.TabIndex = 0;
            this.btnClearPostData.Text = "Clear";
            this.btnClearPostData.UseVisualStyleBackColor = true;
            this.btnClearPostData.Click += new System.EventHandler(this.btnClearPostData_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelFavUrl);
            this.panel1.Controls.Add(this.btnFavUrl);
            this.panel1.Controls.Add(this.ckbxIsPost);
            this.panel1.Controls.Add(this.ckbxUserWebBrowser);
            this.panel1.Controls.Add(this.ckbxKeepCookie);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 24);
            this.panel1.TabIndex = 1;
            // 
            // btnDelFavUrl
            // 
            this.btnDelFavUrl.Location = new System.Drawing.Point(256, 0);
            this.btnDelFavUrl.Name = "btnDelFavUrl";
            this.btnDelFavUrl.Size = new System.Drawing.Size(25, 23);
            this.btnDelFavUrl.TabIndex = 7;
            this.btnDelFavUrl.Text = "-";
            this.btnDelFavUrl.UseVisualStyleBackColor = true;
            this.btnDelFavUrl.Click += new System.EventHandler(this.btnDelFavUrl_Click);
            // 
            // btnFavUrl
            // 
            this.btnFavUrl.Location = new System.Drawing.Point(214, 0);
            this.btnFavUrl.Name = "btnFavUrl";
            this.btnFavUrl.Size = new System.Drawing.Size(28, 23);
            this.btnFavUrl.TabIndex = 6;
            this.btnFavUrl.Text = "+";
            this.btnFavUrl.UseVisualStyleBackColor = true;
            this.btnFavUrl.Click += new System.EventHandler(this.btnFavUrl_Click);
            // 
            // ckbxIsPost
            // 
            this.ckbxIsPost.AutoSize = true;
            this.ckbxIsPost.Location = new System.Drawing.Point(4, 5);
            this.ckbxIsPost.Name = "ckbxIsPost";
            this.ckbxIsPost.Size = new System.Drawing.Size(48, 16);
            this.ckbxIsPost.TabIndex = 5;
            this.ckbxIsPost.Text = "Post";
            this.ckbxIsPost.UseVisualStyleBackColor = true;
            // 
            // ckbxUserWebBrowser
            // 
            this.ckbxUserWebBrowser.AutoSize = true;
            this.ckbxUserWebBrowser.Location = new System.Drawing.Point(124, 5);
            this.ckbxUserWebBrowser.Name = "ckbxUserWebBrowser";
            this.ckbxUserWebBrowser.Size = new System.Drawing.Size(84, 16);
            this.ckbxUserWebBrowser.TabIndex = 4;
            this.ckbxUserWebBrowser.Text = "UseBrowser";
            this.ckbxUserWebBrowser.UseVisualStyleBackColor = true;
            // 
            // ckbxKeepCookie
            // 
            this.ckbxKeepCookie.AutoSize = true;
            this.ckbxKeepCookie.Checked = true;
            this.ckbxKeepCookie.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxKeepCookie.Location = new System.Drawing.Point(58, 5);
            this.ckbxKeepCookie.Name = "ckbxKeepCookie";
            this.ckbxKeepCookie.Size = new System.Drawing.Size(60, 16);
            this.ckbxKeepCookie.TabIndex = 1;
            this.ckbxKeepCookie.Text = "Cookie";
            this.ckbxKeepCookie.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(573, 28);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // cbxPageCharset
            // 
            this.cbxPageCharset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxPageCharset.FormattingEnabled = true;
            this.cbxPageCharset.Items.AddRange(new object[] {
            "UTF-8",
            "GBK",
            "GB2312",
            "BIG5"});
            this.cbxPageCharset.Location = new System.Drawing.Point(573, 3);
            this.cbxPageCharset.Name = "cbxPageCharset";
            this.cbxPageCharset.Size = new System.Drawing.Size(80, 20);
            this.cbxPageCharset.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 420);
            this.Controls.Add(this.tlpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "HttpUtilityTool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tcMessage.ResumeLayout(false);
            this.tpHtml.ResumeLayout(false);
            this.tpText.ResumeLayout(false);
            this.tpText.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.plPostDataBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel plPostDataBtn;
        private System.Windows.Forms.Button btnSavePostData;
        private System.Windows.Forms.Button btnClearPostData;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSaveResult;
        private System.Windows.Forms.Button btnClearResult;
        private System.Windows.Forms.ComboBox cbxUrl;
        private System.Windows.Forms.TextBox tbxPostData;
        private System.Windows.Forms.TabControl tcMessage;
        private System.Windows.Forms.TabPage tpHtml;
        private System.Windows.Forms.WebBrowser wbMain;
        private System.Windows.Forms.TabPage tpText;
        private System.Windows.Forms.TextBox tbxMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ckbxIsPost;
        private System.Windows.Forms.CheckBox ckbxUserWebBrowser;
        private System.Windows.Forms.CheckBox ckbxKeepCookie;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cbxPageCharset;
        private System.Windows.Forms.Button btnDelFavUrl;
        private System.Windows.Forms.Button btnFavUrl;
    }
}

