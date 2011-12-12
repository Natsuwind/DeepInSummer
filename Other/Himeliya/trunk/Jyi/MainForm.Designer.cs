namespace Jyi
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
            this.cbbSchedule = new System.Windows.Forms.ComboBox();
            this.cbbProject = new System.Windows.Forms.ComboBox();
            this.btnStartProject = new System.Windows.Forms.Button();
            this.btnStartSchedule = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnClearMessageBox = new System.Windows.Forms.Button();
            this.btnProxySet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbFinishedCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnRefreshProjectList = new System.Windows.Forms.Button();
            this.btnRefreshScheduleList = new System.Windows.Forms.Button();
            this.cbStopScroll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbbSchedule
            // 
            this.cbbSchedule.FormattingEnabled = true;
            this.cbbSchedule.Location = new System.Drawing.Point(12, 12);
            this.cbbSchedule.Name = "cbbSchedule";
            this.cbbSchedule.Size = new System.Drawing.Size(227, 20);
            this.cbbSchedule.TabIndex = 0;
            // 
            // cbbProject
            // 
            this.cbbProject.FormattingEnabled = true;
            this.cbbProject.Location = new System.Drawing.Point(245, 12);
            this.cbbProject.Name = "cbbProject";
            this.cbbProject.Size = new System.Drawing.Size(227, 20);
            this.cbbProject.TabIndex = 1;
            // 
            // btnStartProject
            // 
            this.btnStartProject.Location = new System.Drawing.Point(559, 9);
            this.btnStartProject.Name = "btnStartProject";
            this.btnStartProject.Size = new System.Drawing.Size(75, 23);
            this.btnStartProject.TabIndex = 2;
            this.btnStartProject.Text = "开始项目";
            this.btnStartProject.UseVisualStyleBackColor = true;
            this.btnStartProject.Click += new System.EventHandler(this.btnStartProject_Click);
            // 
            // btnStartSchedule
            // 
            this.btnStartSchedule.Location = new System.Drawing.Point(559, 36);
            this.btnStartSchedule.Name = "btnStartSchedule";
            this.btnStartSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnStartSchedule.TabIndex = 3;
            this.btnStartSchedule.Text = "开始任务";
            this.btnStartSchedule.UseVisualStyleBackColor = true;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(12, 38);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMessage.Size = new System.Drawing.Size(460, 259);
            this.tbMessage.TabIndex = 4;
            // 
            // btnClearMessageBox
            // 
            this.btnClearMessageBox.Location = new System.Drawing.Point(559, 148);
            this.btnClearMessageBox.Name = "btnClearMessageBox";
            this.btnClearMessageBox.Size = new System.Drawing.Size(75, 23);
            this.btnClearMessageBox.TabIndex = 5;
            this.btnClearMessageBox.Text = "清除记录";
            this.btnClearMessageBox.UseVisualStyleBackColor = true;
            this.btnClearMessageBox.Click += new System.EventHandler(this.btnClearMessageBox_Click);
            // 
            // btnProxySet
            // 
            this.btnProxySet.Location = new System.Drawing.Point(559, 214);
            this.btnProxySet.Name = "btnProxySet";
            this.btnProxySet.Size = new System.Drawing.Size(75, 23);
            this.btnProxySet.TabIndex = 6;
            this.btnProxySet.Text = "设置";
            this.btnProxySet.UseVisualStyleBackColor = true;
            this.btnProxySet.Click += new System.EventHandler(this.btnProxySet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(478, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "次    数：";
            // 
            // lbFinishedCount
            // 
            this.lbFinishedCount.AutoSize = true;
            this.lbFinishedCount.Location = new System.Drawing.Point(557, 70);
            this.lbFinishedCount.Name = "lbFinishedCount";
            this.lbFinishedCount.Size = new System.Drawing.Size(11, 12);
            this.lbFinishedCount.TabIndex = 8;
            this.lbFinishedCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(478, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "当前项目：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(557, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "当前项目";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(559, 119);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(480, 187);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(156, 21);
            this.textBox2.TabIndex = 12;
            // 
            // btnRefreshProjectList
            // 
            this.btnRefreshProjectList.Location = new System.Drawing.Point(480, 9);
            this.btnRefreshProjectList.Name = "btnRefreshProjectList";
            this.btnRefreshProjectList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshProjectList.TabIndex = 13;
            this.btnRefreshProjectList.Text = "刷新项目";
            this.btnRefreshProjectList.UseVisualStyleBackColor = true;
            this.btnRefreshProjectList.Click += new System.EventHandler(this.btnRefreshProjectList_Click);
            // 
            // btnRefreshScheduleList
            // 
            this.btnRefreshScheduleList.Location = new System.Drawing.Point(480, 36);
            this.btnRefreshScheduleList.Name = "btnRefreshScheduleList";
            this.btnRefreshScheduleList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshScheduleList.TabIndex = 14;
            this.btnRefreshScheduleList.Text = "刷新任务";
            this.btnRefreshScheduleList.UseVisualStyleBackColor = true;
            this.btnRefreshScheduleList.Click += new System.EventHandler(this.btnRefreshScheduleList_Click);
            // 
            // cbStopScroll
            // 
            this.cbStopScroll.AutoSize = true;
            this.cbStopScroll.Location = new System.Drawing.Point(480, 152);
            this.cbStopScroll.Name = "cbStopScroll";
            this.cbStopScroll.Size = new System.Drawing.Size(72, 16);
            this.cbStopScroll.TabIndex = 15;
            this.cbStopScroll.Text = "停止滚屏";
            this.cbStopScroll.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 309);
            this.Controls.Add(this.cbStopScroll);
            this.Controls.Add(this.btnRefreshScheduleList);
            this.Controls.Add(this.btnRefreshProjectList);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbFinishedCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProxySet);
            this.Controls.Add(this.btnClearMessageBox);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.btnStartSchedule);
            this.Controls.Add(this.btnStartProject);
            this.Controls.Add(this.cbbProject);
            this.Controls.Add(this.cbbSchedule);
            this.Name = "MainForm";
            this.Text = "Jyi - Himeliya Project";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbSchedule;
        private System.Windows.Forms.ComboBox cbbProject;
        private System.Windows.Forms.Button btnStartProject;
        private System.Windows.Forms.Button btnStartSchedule;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnClearMessageBox;
        private System.Windows.Forms.Button btnProxySet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbFinishedCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnRefreshProjectList;
        private System.Windows.Forms.Button btnRefreshScheduleList;
        private System.Windows.Forms.CheckBox cbStopScroll;
    }
}

