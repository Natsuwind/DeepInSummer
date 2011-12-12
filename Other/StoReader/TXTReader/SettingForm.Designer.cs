namespace Natsuhime.TXTReader
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnSet = new System.Windows.Forms.Button();
            this.tbxBlankColor = new System.Windows.Forms.TextBox();
            this.lbChooseBlackColor = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tbxFont = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.lbForeColorSelector = new System.Windows.Forms.Label();
            this.tbxForeColor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUnSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "背景色 :";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(122, 114);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 10;
            this.btnSet.Text = "确定";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // tbxBlankColor
            // 
            this.tbxBlankColor.Location = new System.Drawing.Point(73, 13);
            this.tbxBlankColor.Name = "tbxBlankColor";
            this.tbxBlankColor.Size = new System.Drawing.Size(100, 21);
            this.tbxBlankColor.TabIndex = 1;
            // 
            // lbChooseBlackColor
            // 
            this.lbChooseBlackColor.AutoSize = true;
            this.lbChooseBlackColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbChooseBlackColor.Image = global::Natsuhime.TXTReader.Properties.Resources.color;
            this.lbChooseBlackColor.Location = new System.Drawing.Point(180, 16);
            this.lbChooseBlackColor.Name = "lbChooseBlackColor";
            this.lbChooseBlackColor.Size = new System.Drawing.Size(17, 12);
            this.lbChooseBlackColor.TabIndex = 3;
            this.lbChooseBlackColor.Text = "  ";
            this.lbChooseBlackColor.Click += new System.EventHandler(this.label2_Click);
            // 
            // tbxFont
            // 
            this.tbxFont.Location = new System.Drawing.Point(73, 67);
            this.tbxFont.Name = "tbxFont";
            this.tbxFont.Size = new System.Drawing.Size(27, 21);
            this.tbxFont.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "字  号 :";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // lbForeColorSelector
            // 
            this.lbForeColorSelector.AutoSize = true;
            this.lbForeColorSelector.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbForeColorSelector.Image = global::Natsuhime.TXTReader.Properties.Resources.color;
            this.lbForeColorSelector.Location = new System.Drawing.Point(180, 43);
            this.lbForeColorSelector.Name = "lbForeColorSelector";
            this.lbForeColorSelector.Size = new System.Drawing.Size(17, 12);
            this.lbForeColorSelector.TabIndex = 9;
            this.lbForeColorSelector.Text = "  ";
            this.lbForeColorSelector.Click += new System.EventHandler(this.lbForeColorSelector_Click);
            // 
            // tbxForeColor
            // 
            this.tbxForeColor.Location = new System.Drawing.Point(73, 40);
            this.tbxForeColor.Name = "tbxForeColor";
            this.tbxForeColor.Size = new System.Drawing.Size(100, 21);
            this.tbxForeColor.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "前景色 :";
            // 
            // btnUnSet
            // 
            this.btnUnSet.Location = new System.Drawing.Point(16, 114);
            this.btnUnSet.Name = "btnUnSet";
            this.btnUnSet.Size = new System.Drawing.Size(75, 23);
            this.btnUnSet.TabIndex = 9;
            this.btnUnSet.Text = "恢复默认";
            this.btnUnSet.UseVisualStyleBackColor = true;
            this.btnUnSet.Click += new System.EventHandler(this.btnUnSet_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 147);
            this.Controls.Add(this.btnUnSet);
            this.Controls.Add(this.lbForeColorSelector);
            this.Controls.Add(this.tbxForeColor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxFont);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbChooseBlackColor);
            this.Controls.Add(this.tbxBlankColor);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.label1);
            this.Name = "SettingForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox tbxBlankColor;
        private System.Windows.Forms.Label lbChooseBlackColor;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox tbxFont;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Label lbForeColorSelector;
        private System.Windows.Forms.TextBox tbxForeColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUnSet;
    }
}