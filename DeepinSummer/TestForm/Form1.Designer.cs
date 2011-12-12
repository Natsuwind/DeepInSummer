namespace TestForm
{
    partial class Form1
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
            this.btnSelectTemplateFolder = new System.Windows.Forms.Button();
            this.btnSelectPagefileFolder = new System.Windows.Forms.Button();
            this.tbxTemplateFolder = new System.Windows.Forms.TextBox();
            this.tbxPagefileFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnOldVersion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelectTemplateFolder
            // 
            this.btnSelectTemplateFolder.Location = new System.Drawing.Point(364, 10);
            this.btnSelectTemplateFolder.Name = "btnSelectTemplateFolder";
            this.btnSelectTemplateFolder.Size = new System.Drawing.Size(83, 23);
            this.btnSelectTemplateFolder.TabIndex = 0;
            this.btnSelectTemplateFolder.Text = "选择目录...";
            this.btnSelectTemplateFolder.UseVisualStyleBackColor = true;
            this.btnSelectTemplateFolder.Click += new System.EventHandler(this.btnSelectTemplateFolder_Click);
            // 
            // btnSelectPagefileFolder
            // 
            this.btnSelectPagefileFolder.Location = new System.Drawing.Point(364, 40);
            this.btnSelectPagefileFolder.Name = "btnSelectPagefileFolder";
            this.btnSelectPagefileFolder.Size = new System.Drawing.Size(83, 23);
            this.btnSelectPagefileFolder.TabIndex = 1;
            this.btnSelectPagefileFolder.Text = "选择目录...";
            this.btnSelectPagefileFolder.UseVisualStyleBackColor = true;
            this.btnSelectPagefileFolder.Click += new System.EventHandler(this.btnSelectPagefileFolder_Click);
            // 
            // tbxTemplateFolder
            // 
            this.tbxTemplateFolder.Location = new System.Drawing.Point(75, 12);
            this.tbxTemplateFolder.Name = "tbxTemplateFolder";
            this.tbxTemplateFolder.Size = new System.Drawing.Size(283, 21);
            this.tbxTemplateFolder.TabIndex = 2;
            this.tbxTemplateFolder.Text = "F:\\Doctments\\Works\\Comsenz\\DotNet\\Discuz!NT\\3\\Discuz.Web\\templates\\default\\";
            // 
            // tbxPagefileFolder
            // 
            this.tbxPagefileFolder.Location = new System.Drawing.Point(75, 40);
            this.tbxPagefileFolder.Name = "tbxPagefileFolder";
            this.tbxPagefileFolder.Size = new System.Drawing.Size(283, 21);
            this.tbxPagefileFolder.TabIndex = 3;
            this.tbxPagefileFolder.Text = "G:\\temptemp\\aspx\\new";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "模板目录:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "生成目录:";
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(12, 75);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessage.Size = new System.Drawing.Size(435, 178);
            this.tbxMessage.TabIndex = 6;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(364, 259);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(82, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "开始生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnOldVersion
            // 
            this.btnOldVersion.Location = new System.Drawing.Point(276, 259);
            this.btnOldVersion.Name = "btnOldVersion";
            this.btnOldVersion.Size = new System.Drawing.Size(82, 23);
            this.btnOldVersion.TabIndex = 8;
            this.btnOldVersion.Text = "旧版生成";
            this.btnOldVersion.UseVisualStyleBackColor = true;
            this.btnOldVersion.Click += new System.EventHandler(this.btnOldVersion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 287);
            this.Controls.Add(this.btnOldVersion);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.tbxMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxPagefileFolder);
            this.Controls.Add(this.tbxTemplateFolder);
            this.Controls.Add(this.btnSelectPagefileFolder);
            this.Controls.Add(this.btnSelectTemplateFolder);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectTemplateFolder;
        private System.Windows.Forms.Button btnSelectPagefileFolder;
        private System.Windows.Forms.TextBox tbxTemplateFolder;
        private System.Windows.Forms.TextBox tbxPagefileFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOldVersion;
    }
}

