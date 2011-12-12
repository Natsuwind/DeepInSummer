namespace Yuwen.Tools.CreateUpdateTools
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConfirmCreate = new System.Windows.Forms.Button();
            this.cbbxCreateFolder = new System.Windows.Forms.ComboBox();
            this.cbbxReleaseFolder = new System.Windows.Forms.ComboBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.lvMain = new System.Windows.Forms.ListView();
            this.lvchIsCreate = new System.Windows.Forms.ColumnHeader();
            this.lvchLastVersion = new System.Windows.Forms.ColumnHeader();
            this.lvchNewVersion = new System.Windows.Forms.ColumnHeader();
            this.lvchLastMD5 = new System.Windows.Forms.ColumnHeader();
            this.lvchNewMD5 = new System.Windows.Forms.ColumnHeader();
            this.tlpMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate.Location = new System.Drawing.Point(752, 1);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 24);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "生成更新";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(3, 405);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(908, 154);
            this.txtMessage.TabIndex = 1;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.panel1, 0, 0);
            this.tlpMain.Controls.Add(this.txtMessage, 0, 2);
            this.tlpMain.Controls.Add(this.lvMain, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.Size = new System.Drawing.Size(914, 562);
            this.tlpMain.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConfirmCreate);
            this.panel1.Controls.Add(this.cbbxCreateFolder);
            this.panel1.Controls.Add(this.cbbxReleaseFolder);
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(908, 24);
            this.panel1.TabIndex = 3;
            // 
            // btnConfirmCreate
            // 
            this.btnConfirmCreate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirmCreate.Location = new System.Drawing.Point(833, 1);
            this.btnConfirmCreate.Name = "btnConfirmCreate";
            this.btnConfirmCreate.Size = new System.Drawing.Size(75, 24);
            this.btnConfirmCreate.TabIndex = 4;
            this.btnConfirmCreate.Text = "确认生成";
            this.btnConfirmCreate.UseVisualStyleBackColor = true;
            this.btnConfirmCreate.Click += new System.EventHandler(this.btnConfirmCreate_Click);
            // 
            // cbbxCreateFolder
            // 
            this.cbbxCreateFolder.DropDownWidth = 660;
            this.cbbxCreateFolder.FormattingEnabled = true;
            this.cbbxCreateFolder.Items.AddRange(new object[] {
            "E:\\Doctments\\Desktop\\update\\"});
            this.cbbxCreateFolder.Location = new System.Drawing.Point(367, 2);
            this.cbbxCreateFolder.Name = "cbbxCreateFolder";
            this.cbbxCreateFolder.Size = new System.Drawing.Size(296, 24);
            this.cbbxCreateFolder.TabIndex = 3;
            // 
            // cbbxReleaseFolder
            // 
            this.cbbxReleaseFolder.DropDownWidth = 660;
            this.cbbxReleaseFolder.FormattingEnabled = true;
            this.cbbxReleaseFolder.Items.AddRange(new object[] {
            "E:\\Doctments\\Documents\\Works\\Comsenz\\SuperWebMaster\\Code2k5\\branches\\0.2\\Main\\bin" +
                "\\x86\\Release"});
            this.cbbxReleaseFolder.Location = new System.Drawing.Point(4, 1);
            this.cbbxReleaseFolder.Name = "cbbxReleaseFolder";
            this.cbbxReleaseFolder.Size = new System.Drawing.Size(357, 24);
            this.cbbxReleaseFolder.TabIndex = 2;
            // 
            // btnScan
            // 
            this.btnScan.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnScan.Location = new System.Drawing.Point(672, 1);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 24);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "开始分析";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lvMain
            // 
            this.lvMain.AllowColumnReorder = true;
            this.lvMain.CheckBoxes = true;
            this.lvMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvchIsCreate,
            this.lvchLastVersion,
            this.lvchNewVersion,
            this.lvchLastMD5,
            this.lvchNewMD5});
            this.lvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMain.Location = new System.Drawing.Point(3, 33);
            this.lvMain.Name = "lvMain";
            this.lvMain.Size = new System.Drawing.Size(908, 366);
            this.lvMain.TabIndex = 4;
            this.lvMain.UseCompatibleStateImageBehavior = false;
            this.lvMain.View = System.Windows.Forms.View.Details;
            // 
            // lvchIsCreate
            // 
            this.lvchIsCreate.Text = "挂载";
            this.lvchIsCreate.Width = 300;
            // 
            // lvchLastVersion
            // 
            this.lvchLastVersion.Text = "最后版本";
            this.lvchLastVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lvchLastVersion.Width = 70;
            // 
            // lvchNewVersion
            // 
            this.lvchNewVersion.Text = "现在版本";
            this.lvchNewVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lvchNewVersion.Width = 70;
            // 
            // lvchLastMD5
            // 
            this.lvchLastMD5.Text = "最后MD5";
            this.lvchLastMD5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lvchLastMD5.Width = 225;
            // 
            // lvchNewMD5
            // 
            this.lvchNewMD5.Text = "现在MD5";
            this.lvchNewMD5.Width = 225;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(914, 562);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "MainForm";
            this.Text = "更新生成工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ListView lvMain;
        private System.Windows.Forms.ComboBox cbbxReleaseFolder;
        private System.Windows.Forms.ColumnHeader lvchIsCreate;
        private System.Windows.Forms.ColumnHeader lvchLastVersion;
        private System.Windows.Forms.ColumnHeader lvchNewVersion;
        private System.Windows.Forms.ColumnHeader lvchLastMD5;
        private System.Windows.Forms.ColumnHeader lvchNewMD5;
        private System.Windows.Forms.ComboBox cbbxCreateFolder;
        private System.Windows.Forms.Button btnConfirmCreate;
    }
}

