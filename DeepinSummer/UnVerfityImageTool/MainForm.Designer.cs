namespace UnVerfityImageTool
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
            this.button1 = new System.Windows.Forms.Button();
            this.cbbxVerfityImageUrl = new System.Windows.Forms.ComboBox();
            this.btnGetVerfityImage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pbVerfityImageSource = new System.Windows.Forms.PictureBox();
            this.pbVerfityImageClear = new System.Windows.Forms.PictureBox();
            this.btnClearSource = new System.Windows.Forms.Button();
            this.tbxVerfityCodeCharCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.plClearedImageList = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerfityImageSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerfityImageClear)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(756, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cbbxVerfityImageUrl
            // 
            this.cbbxVerfityImageUrl.FormattingEnabled = true;
            this.cbbxVerfityImageUrl.Items.AddRange(new object[] {
            "http://fw8.99081.com/SysWeb/VfyImage.aspx"});
            this.cbbxVerfityImageUrl.Location = new System.Drawing.Point(84, 10);
            this.cbbxVerfityImageUrl.Name = "cbbxVerfityImageUrl";
            this.cbbxVerfityImageUrl.Size = new System.Drawing.Size(452, 20);
            this.cbbxVerfityImageUrl.TabIndex = 1;
            // 
            // btnGetVerfityImage
            // 
            this.btnGetVerfityImage.Location = new System.Drawing.Point(542, 10);
            this.btnGetVerfityImage.Name = "btnGetVerfityImage";
            this.btnGetVerfityImage.Size = new System.Drawing.Size(75, 23);
            this.btnGetVerfityImage.TabIndex = 2;
            this.btnGetVerfityImage.Text = "获取图片";
            this.btnGetVerfityImage.UseVisualStyleBackColor = true;
            this.btnGetVerfityImage.Click += new System.EventHandler(this.btnGetVerfityImage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "图片地址: ";
            // 
            // pbVerfityImageSource
            // 
            this.pbVerfityImageSource.Location = new System.Drawing.Point(15, 36);
            this.pbVerfityImageSource.Name = "pbVerfityImageSource";
            this.pbVerfityImageSource.Size = new System.Drawing.Size(289, 116);
            this.pbVerfityImageSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVerfityImageSource.TabIndex = 4;
            this.pbVerfityImageSource.TabStop = false;
            // 
            // pbVerfityImageClear
            // 
            this.pbVerfityImageClear.Location = new System.Drawing.Point(328, 36);
            this.pbVerfityImageClear.Name = "pbVerfityImageClear";
            this.pbVerfityImageClear.Size = new System.Drawing.Size(289, 116);
            this.pbVerfityImageClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVerfityImageClear.TabIndex = 5;
            this.pbVerfityImageClear.TabStop = false;
            // 
            // btnClearSource
            // 
            this.btnClearSource.Location = new System.Drawing.Point(542, 159);
            this.btnClearSource.Name = "btnClearSource";
            this.btnClearSource.Size = new System.Drawing.Size(75, 23);
            this.btnClearSource.TabIndex = 6;
            this.btnClearSource.Text = "处理";
            this.btnClearSource.UseVisualStyleBackColor = true;
            this.btnClearSource.Click += new System.EventHandler(this.btnClearSource_Click);
            // 
            // tbxVerfityCodeCharCount
            // 
            this.tbxVerfityCodeCharCount.Location = new System.Drawing.Point(84, 159);
            this.tbxVerfityCodeCharCount.Name = "tbxVerfityCodeCharCount";
            this.tbxVerfityCodeCharCount.Size = new System.Drawing.Size(31, 21);
            this.tbxVerfityCodeCharCount.TabIndex = 7;
            this.tbxVerfityCodeCharCount.Text = "4";
            this.tbxVerfityCodeCharCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "字符长度: ";
            // 
            // plClearedImageList
            // 
            this.plClearedImageList.Location = new System.Drawing.Point(15, 186);
            this.plClearedImageList.Name = "plClearedImageList";
            this.plClearedImageList.Size = new System.Drawing.Size(602, 147);
            this.plClearedImageList.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 434);
            this.Controls.Add(this.plClearedImageList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxVerfityCodeCharCount);
            this.Controls.Add(this.btnClearSource);
            this.Controls.Add(this.pbVerfityImageClear);
            this.Controls.Add(this.pbVerfityImageSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGetVerfityImage);
            this.Controls.Add(this.cbbxVerfityImageUrl);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbVerfityImageSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerfityImageClear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbbxVerfityImageUrl;
        private System.Windows.Forms.Button btnGetVerfityImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbVerfityImageSource;
        private System.Windows.Forms.PictureBox pbVerfityImageClear;
        private System.Windows.Forms.Button btnClearSource;
        private System.Windows.Forms.TextBox tbxVerfityCodeCharCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel plClearedImageList;
    }
}

