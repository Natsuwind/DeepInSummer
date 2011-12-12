namespace Jyi
{
    partial class ProxyForm
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
            this.tbCharset = new System.Windows.Forms.TextBox();
            this.tbSuccessText = new System.Windows.Forms.TextBox();
            this.tbTryUrl = new System.Windows.Forms.TextBox();
            this.btnClearProxy = new System.Windows.Forms.Button();
            this.cbxDebugMode = new System.Windows.Forms.CheckBox();
            this.tbDebugRegex = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnGetProxy = new System.Windows.Forms.Button();
            this.cbbPageUrl = new System.Windows.Forms.ComboBox();
            this.cbStopScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbCharset
            // 
            this.tbCharset.Location = new System.Drawing.Point(209, 35);
            this.tbCharset.Name = "tbCharset";
            this.tbCharset.Size = new System.Drawing.Size(82, 21);
            this.tbCharset.TabIndex = 15;
            this.tbCharset.Text = "gb2312";
            // 
            // tbSuccessText
            // 
            this.tbSuccessText.Location = new System.Drawing.Point(306, 35);
            this.tbSuccessText.Name = "tbSuccessText";
            this.tbSuccessText.Size = new System.Drawing.Size(112, 21);
            this.tbSuccessText.TabIndex = 14;
            this.tbSuccessText.Text = "您的IP地址";
            // 
            // tbTryUrl
            // 
            this.tbTryUrl.Location = new System.Drawing.Point(6, 35);
            this.tbTryUrl.Name = "tbTryUrl";
            this.tbTryUrl.Size = new System.Drawing.Size(197, 21);
            this.tbTryUrl.TabIndex = 13;
            this.tbTryUrl.Text = "http://www.ip138.com/ips.asp";
            // 
            // btnClearProxy
            // 
            this.btnClearProxy.Location = new System.Drawing.Point(478, 33);
            this.btnClearProxy.Name = "btnClearProxy";
            this.btnClearProxy.Size = new System.Drawing.Size(75, 23);
            this.btnClearProxy.TabIndex = 12;
            this.btnClearProxy.Text = "整理";
            this.btnClearProxy.UseVisualStyleBackColor = true;
            this.btnClearProxy.Click += new System.EventHandler(this.btnClearProxy_Click);
            // 
            // cbxDebugMode
            // 
            this.cbxDebugMode.AutoSize = true;
            this.cbxDebugMode.Location = new System.Drawing.Point(424, 9);
            this.cbxDebugMode.Name = "cbxDebugMode";
            this.cbxDebugMode.Size = new System.Drawing.Size(48, 16);
            this.cbxDebugMode.TabIndex = 11;
            this.cbxDebugMode.Text = "调试";
            this.cbxDebugMode.UseVisualStyleBackColor = true;
            // 
            // tbDebugRegex
            // 
            this.tbDebugRegex.Location = new System.Drawing.Point(237, 7);
            this.tbDebugRegex.Name = "tbDebugRegex";
            this.tbDebugRegex.Size = new System.Drawing.Size(181, 21);
            this.tbDebugRegex.TabIndex = 10;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(6, 62);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMessage.Size = new System.Drawing.Size(547, 213);
            this.tbMessage.TabIndex = 9;
            // 
            // btnGetProxy
            // 
            this.btnGetProxy.Location = new System.Drawing.Point(478, 5);
            this.btnGetProxy.Name = "btnGetProxy";
            this.btnGetProxy.Size = new System.Drawing.Size(75, 23);
            this.btnGetProxy.TabIndex = 8;
            this.btnGetProxy.Text = "连接";
            this.btnGetProxy.UseVisualStyleBackColor = true;
            this.btnGetProxy.Click += new System.EventHandler(this.btnGetProxy_Click);
            // 
            // cbbPageUrl
            // 
            this.cbbPageUrl.FormattingEnabled = true;
            this.cbbPageUrl.Location = new System.Drawing.Point(6, 7);
            this.cbbPageUrl.Name = "cbbPageUrl";
            this.cbbPageUrl.Size = new System.Drawing.Size(225, 20);
            this.cbbPageUrl.TabIndex = 16;
            // 
            // cbStopScroll
            // 
            this.cbStopScroll.AutoSize = true;
            this.cbStopScroll.Location = new System.Drawing.Point(424, 37);
            this.cbStopScroll.Name = "cbStopScroll";
            this.cbStopScroll.Size = new System.Drawing.Size(48, 16);
            this.cbStopScroll.TabIndex = 17;
            this.cbStopScroll.Text = "滚屏";
            this.cbStopScroll.UseVisualStyleBackColor = true;
            // 
            // ProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 283);
            this.Controls.Add(this.cbStopScroll);
            this.Controls.Add(this.cbbPageUrl);
            this.Controls.Add(this.tbCharset);
            this.Controls.Add(this.tbSuccessText);
            this.Controls.Add(this.tbTryUrl);
            this.Controls.Add(this.btnClearProxy);
            this.Controls.Add(this.cbxDebugMode);
            this.Controls.Add(this.tbDebugRegex);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.btnGetProxy);
            this.Name = "ProxyForm";
            this.Text = "ProxyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCharset;
        private System.Windows.Forms.TextBox tbSuccessText;
        private System.Windows.Forms.TextBox tbTryUrl;
        private System.Windows.Forms.Button btnClearProxy;
        private System.Windows.Forms.CheckBox cbxDebugMode;
        private System.Windows.Forms.TextBox tbDebugRegex;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnGetProxy;
        private System.Windows.Forms.ComboBox cbbPageUrl;
        private System.Windows.Forms.CheckBox cbStopScroll;
    }
}