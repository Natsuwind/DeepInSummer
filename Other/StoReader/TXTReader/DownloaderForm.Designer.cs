namespace Natsuhime.TXTReader
{
    partial class DownloaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpList = new System.Windows.Forms.TabPage();
            this.tpContent = new System.Windows.Forms.TabPage();
            this.tbxListTitle = new System.Windows.Forms.TextBox();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.lvList = new System.Windows.Forms.ListView();
            this.tbxListRegex = new System.Windows.Forms.TextBox();
            this.cbbxListUrl = new System.Windows.Forms.ComboBox();
            this.tbxContentTitle = new System.Windows.Forms.TextBox();
            this.tbxContentRegex = new System.Windows.Forms.TextBox();
            this.cbbxContentUrl = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tbxContent = new System.Windows.Forms.TextBox();
            this.btnGetList = new System.Windows.Forms.Button();
            this.btnGetContent = new System.Windows.Forms.Button();
            this.tbxPreContentRegex = new System.Windows.Forms.TextBox();
            this.tbxPreListRegex = new System.Windows.Forms.TextBox();
            this.cbbxCharset = new System.Windows.Forms.ComboBox();
            this.tcMain.SuspendLayout();
            this.tpList.SuspendLayout();
            this.tpContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpList);
            this.tcMain.Controls.Add(this.tpContent);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(506, 390);
            this.tcMain.TabIndex = 6;
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.cbbxCharset);
            this.tpList.Controls.Add(this.tbxPreListRegex);
            this.tpList.Controls.Add(this.btnGetList);
            this.tpList.Controls.Add(this.tbxListTitle);
            this.tpList.Controls.Add(this.btnNextPage);
            this.tpList.Controls.Add(this.btnPrevPage);
            this.tpList.Controls.Add(this.lvList);
            this.tpList.Controls.Add(this.tbxListRegex);
            this.tpList.Controls.Add(this.cbbxListUrl);
            this.tpList.Location = new System.Drawing.Point(4, 22);
            this.tpList.Name = "tpList";
            this.tpList.Padding = new System.Windows.Forms.Padding(3);
            this.tpList.Size = new System.Drawing.Size(498, 364);
            this.tpList.TabIndex = 0;
            this.tpList.Text = "列表";
            this.tpList.UseVisualStyleBackColor = true;
            // 
            // tpContent
            // 
            this.tpContent.Controls.Add(this.tbxPreContentRegex);
            this.tpContent.Controls.Add(this.btnGetContent);
            this.tpContent.Controls.Add(this.tbxContent);
            this.tpContent.Controls.Add(this.progressBar1);
            this.tpContent.Controls.Add(this.tbxContentTitle);
            this.tpContent.Controls.Add(this.tbxContentRegex);
            this.tpContent.Controls.Add(this.cbbxContentUrl);
            this.tpContent.Location = new System.Drawing.Point(4, 22);
            this.tpContent.Name = "tpContent";
            this.tpContent.Padding = new System.Windows.Forms.Padding(3);
            this.tpContent.Size = new System.Drawing.Size(498, 364);
            this.tpContent.TabIndex = 1;
            this.tpContent.Text = "内容";
            this.tpContent.UseVisualStyleBackColor = true;
            // 
            // tbxListTitle
            // 
            this.tbxListTitle.Location = new System.Drawing.Point(79, 129);
            this.tbxListTitle.Name = "tbxListTitle";
            this.tbxListTitle.Size = new System.Drawing.Size(411, 21);
            this.tbxListTitle.TabIndex = 11;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(415, 338);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 23);
            this.btnNextPage.TabIndex = 10;
            this.btnNextPage.Text = "下";
            this.btnNextPage.UseVisualStyleBackColor = true;
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(334, 338);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 23);
            this.btnPrevPage.TabIndex = 9;
            this.btnPrevPage.Text = "上";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            // 
            // lvList
            // 
            this.lvList.Location = new System.Drawing.Point(79, 156);
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(411, 179);
            this.lvList.TabIndex = 8;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.SmallIcon;
            // 
            // tbxListRegex
            // 
            this.tbxListRegex.Location = new System.Drawing.Point(79, 79);
            this.tbxListRegex.Multiline = true;
            this.tbxListRegex.Name = "tbxListRegex";
            this.tbxListRegex.Size = new System.Drawing.Size(411, 44);
            this.tbxListRegex.TabIndex = 7;
            this.tbxListRegex.Text = "<li><a href=\"(.*?)\">(.*?)</a></li>";
            // 
            // cbbxListUrl
            // 
            this.cbbxListUrl.FormattingEnabled = true;
            this.cbbxListUrl.Items.AddRange(new object[] {
            "http://www.daomubiji.com/dao-mu-bi-ji-quan-ji"});
            this.cbbxListUrl.Location = new System.Drawing.Point(79, 10);
            this.cbbxListUrl.Name = "cbbxListUrl";
            this.cbbxListUrl.Size = new System.Drawing.Size(293, 20);
            this.cbbxListUrl.TabIndex = 6;
            // 
            // tbxContentTitle
            // 
            this.tbxContentTitle.Location = new System.Drawing.Point(79, 122);
            this.tbxContentTitle.Name = "tbxContentTitle";
            this.tbxContentTitle.Size = new System.Drawing.Size(411, 21);
            this.tbxContentTitle.TabIndex = 15;
            // 
            // tbxContentRegex
            // 
            this.tbxContentRegex.Location = new System.Drawing.Point(79, 73);
            this.tbxContentRegex.Multiline = true;
            this.tbxContentRegex.Name = "tbxContentRegex";
            this.tbxContentRegex.Size = new System.Drawing.Size(411, 43);
            this.tbxContentRegex.TabIndex = 13;
            this.tbxContentRegex.Text = "<p>(.*?)</p>";
            // 
            // cbbxContentUrl
            // 
            this.cbbxContentUrl.FormattingEnabled = true;
            this.cbbxContentUrl.Location = new System.Drawing.Point(79, 3);
            this.cbbxContentUrl.Name = "cbbxContentUrl";
            this.cbbxContentUrl.Size = new System.Drawing.Size(361, 20);
            this.cbbxContentUrl.TabIndex = 12;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(79, 335);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(306, 23);
            this.progressBar1.TabIndex = 16;
            // 
            // tbxContent
            // 
            this.tbxContent.Location = new System.Drawing.Point(79, 149);
            this.tbxContent.Multiline = true;
            this.tbxContent.Name = "tbxContent";
            this.tbxContent.Size = new System.Drawing.Size(411, 180);
            this.tbxContent.TabIndex = 17;
            // 
            // btnGetList
            // 
            this.btnGetList.Location = new System.Drawing.Point(442, 8);
            this.btnGetList.Name = "btnGetList";
            this.btnGetList.Size = new System.Drawing.Size(48, 23);
            this.btnGetList.TabIndex = 12;
            this.btnGetList.Text = "开始";
            this.btnGetList.UseVisualStyleBackColor = true;
            this.btnGetList.Click += new System.EventHandler(this.btnGetList_Click);
            // 
            // btnGetContent
            // 
            this.btnGetContent.Location = new System.Drawing.Point(444, 1);
            this.btnGetContent.Name = "btnGetContent";
            this.btnGetContent.Size = new System.Drawing.Size(48, 23);
            this.btnGetContent.TabIndex = 18;
            this.btnGetContent.Text = "开始";
            this.btnGetContent.UseVisualStyleBackColor = true;
            this.btnGetContent.Click += new System.EventHandler(this.btnGetContent_Click);
            // 
            // tbxPreContentRegex
            // 
            this.tbxPreContentRegex.Location = new System.Drawing.Point(79, 29);
            this.tbxPreContentRegex.Multiline = true;
            this.tbxPreContentRegex.Name = "tbxPreContentRegex";
            this.tbxPreContentRegex.Size = new System.Drawing.Size(411, 38);
            this.tbxPreContentRegex.TabIndex = 19;
            this.tbxPreContentRegex.Text = "<div class=\\\"cmt\\\">.*</div>";
            // 
            // tbxPreListRegex
            // 
            this.tbxPreListRegex.Location = new System.Drawing.Point(79, 35);
            this.tbxPreListRegex.Multiline = true;
            this.tbxPreListRegex.Name = "tbxPreListRegex";
            this.tbxPreListRegex.Size = new System.Drawing.Size(411, 38);
            this.tbxPreListRegex.TabIndex = 20;
            this.tbxPreListRegex.Text = "<h2>(.*?)</h2>.*?<ul>(.*?)</ul>";
            // 
            // cbbxCharset
            // 
            this.cbbxCharset.FormattingEnabled = true;
            this.cbbxCharset.Items.AddRange(new object[] {
            "UTF-8",
            "GBK",
            "BIG5"});
            this.cbbxCharset.Location = new System.Drawing.Point(378, 10);
            this.cbbxCharset.Name = "cbbxCharset";
            this.cbbxCharset.Size = new System.Drawing.Size(58, 20);
            this.cbbxCharset.TabIndex = 21;
            // 
            // DownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 390);
            this.Controls.Add(this.tcMain);
            this.Name = "DownloaderForm";
            this.Text = "DownloaderForm";
            this.tcMain.ResumeLayout(false);
            this.tpList.ResumeLayout(false);
            this.tpList.PerformLayout();
            this.tpContent.ResumeLayout(false);
            this.tpContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpList;
        private System.Windows.Forms.TextBox tbxListTitle;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.ListView lvList;
        private System.Windows.Forms.TextBox tbxListRegex;
        private System.Windows.Forms.ComboBox cbbxListUrl;
        private System.Windows.Forms.TabPage tpContent;
        private System.Windows.Forms.TextBox tbxContentTitle;
        private System.Windows.Forms.TextBox tbxContentRegex;
        private System.Windows.Forms.ComboBox cbbxContentUrl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox tbxContent;
        private System.Windows.Forms.Button btnGetList;
        private System.Windows.Forms.Button btnGetContent;
        private System.Windows.Forms.TextBox tbxPreListRegex;
        private System.Windows.Forms.TextBox tbxPreContentRegex;
        private System.Windows.Forms.ComboBox cbbxCharset;

    }
}