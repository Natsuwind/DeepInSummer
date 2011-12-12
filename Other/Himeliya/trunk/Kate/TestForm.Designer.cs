namespace Himeliya.Kate
{
    partial class TestForm
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
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetPosts = new Himeliya.Controls.Button();
            this.btnGetLinks = new Himeliya.Controls.Button();
            this.btnStartDownload = new Himeliya.Controls.Button();
            this.btnConfig = new Himeliya.Controls.Button();
            this.btnGetPostsTest = new Himeliya.Controls.Button();
            this.btnGetFilesTest = new Himeliya.Controls.Button();
            this.SuspendLayout();
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(77, 12);
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(460, 21);
            this.tbxUrl.TabIndex = 2;
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(12, 39);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessage.Size = new System.Drawing.Size(525, 258);
            this.tbxMessage.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "版块链接:";
            // 
            // btnGetPosts
            // 
            this.btnGetPosts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetPosts.Location = new System.Drawing.Point(543, 41);
            this.btnGetPosts.Name = "btnGetPosts";
            this.btnGetPosts.Size = new System.Drawing.Size(88, 23);
            this.btnGetPosts.TabIndex = 5;
            this.btnGetPosts.Text = "分析页面链接";
            this.btnGetPosts.UseVisualStyleBackColor = true;
            this.btnGetPosts.Click += new System.EventHandler(this.btnGetPosts_Click);
            // 
            // btnGetLinks
            // 
            this.btnGetLinks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetLinks.Location = new System.Drawing.Point(543, 12);
            this.btnGetLinks.Name = "btnGetLinks";
            this.btnGetLinks.Size = new System.Drawing.Size(88, 23);
            this.btnGetLinks.TabIndex = 1;
            this.btnGetLinks.Text = "分析列表链接";
            this.btnGetLinks.UseVisualStyleBackColor = true;
            this.btnGetLinks.Click += new System.EventHandler(this.btnGetThreadLinks_Click);
            // 
            // btnStartDownload
            // 
            this.btnStartDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartDownload.Location = new System.Drawing.Point(543, 70);
            this.btnStartDownload.Name = "btnStartDownload";
            this.btnStartDownload.Size = new System.Drawing.Size(88, 23);
            this.btnStartDownload.TabIndex = 6;
            this.btnStartDownload.Text = "开始下载";
            this.btnStartDownload.UseVisualStyleBackColor = true;
            this.btnStartDownload.Click += new System.EventHandler(this.btnStartDownload_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfig.Location = new System.Drawing.Point(543, 274);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(88, 23);
            this.btnConfig.TabIndex = 7;
            this.btnConfig.Text = "设置";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnGetPostsTest
            // 
            this.btnGetPostsTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetPostsTest.Location = new System.Drawing.Point(543, 180);
            this.btnGetPostsTest.Name = "btnGetPostsTest";
            this.btnGetPostsTest.Size = new System.Drawing.Size(88, 23);
            this.btnGetPostsTest.TabIndex = 8;
            this.btnGetPostsTest.Text = "T_GetPosts";
            this.btnGetPostsTest.UseVisualStyleBackColor = true;
            this.btnGetPostsTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnGetFilesTest
            // 
            this.btnGetFilesTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetFilesTest.Location = new System.Drawing.Point(543, 209);
            this.btnGetFilesTest.Name = "btnGetFilesTest";
            this.btnGetFilesTest.Size = new System.Drawing.Size(88, 23);
            this.btnGetFilesTest.TabIndex = 9;
            this.btnGetFilesTest.Text = "T_GetFiles";
            this.btnGetFilesTest.UseVisualStyleBackColor = true;
            this.btnGetFilesTest.Click += new System.EventHandler(this.btnGetFilesTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 309);
            this.Controls.Add(this.btnGetFilesTest);
            this.Controls.Add(this.btnGetPostsTest);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnStartDownload);
            this.Controls.Add(this.btnGetPosts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxMessage);
            this.Controls.Add(this.tbxUrl);
            this.Controls.Add(this.btnGetLinks);
            this.Name = "MainForm";
            this.Text = "Kate - Himeliya Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Himeliya.Controls.Button btnGetLinks;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Label label1;
        private Himeliya.Controls.Button btnGetPosts;
        private Himeliya.Controls.Button btnStartDownload;
        private Himeliya.Controls.Button btnConfig;
        private Himeliya.Controls.Button btnGetPostsTest;
        private Himeliya.Controls.Button btnGetFilesTest;

    }
}

