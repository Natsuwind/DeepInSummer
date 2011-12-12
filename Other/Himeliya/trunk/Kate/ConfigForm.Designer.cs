namespace Himeliya.Kate
{
    partial class ConfigForm
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
            this.lblProxyAdd = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbxIsUserProxy = new System.Windows.Forms.CheckBox();
            this.btnSaveNetwork = new Himeliya.Controls.Button();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.tbxProxyPort = new System.Windows.Forms.TextBox();
            this.tbxProxyAddress = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelProject = new Himeliya.Controls.Button();
            this.ckbxIsActivate = new System.Windows.Forms.CheckBox();
            this.cbbxCharset = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ckbxEditProject = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxProjectName = new System.Windows.Forms.TextBox();
            this.btnNewProject = new Himeliya.Controls.Button();
            this.btnSaveProjectInfo = new Himeliya.Controls.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxCurrentPostId = new System.Windows.Forms.TextBox();
            this.tbxCurrentPageId = new System.Windows.Forms.TextBox();
            this.tbxTotalPageCount = new System.Windows.Forms.TextBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbxProjects = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProxyAdd
            // 
            this.lblProxyAdd.AutoSize = true;
            this.lblProxyAdd.Location = new System.Drawing.Point(7, 23);
            this.lblProxyAdd.Name = "lblProxyAdd";
            this.lblProxyAdd.Size = new System.Drawing.Size(35, 12);
            this.lblProxyAdd.TabIndex = 1;
            this.lblProxyAdd.Text = "代理:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbxIsUserProxy);
            this.groupBox1.Controls.Add(this.btnSaveNetwork);
            this.groupBox1.Controls.Add(this.lblProxyPort);
            this.groupBox1.Controls.Add(this.tbxProxyPort);
            this.groupBox1.Controls.Add(this.tbxProxyAddress);
            this.groupBox1.Controls.Add(this.lblProxyAdd);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 57);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "网络";
            // 
            // ckbxIsUserProxy
            // 
            this.ckbxIsUserProxy.AutoSize = true;
            this.ckbxIsUserProxy.Location = new System.Drawing.Point(362, 22);
            this.ckbxIsUserProxy.Name = "ckbxIsUserProxy";
            this.ckbxIsUserProxy.Size = new System.Drawing.Size(72, 16);
            this.ckbxIsUserProxy.TabIndex = 5;
            this.ckbxIsUserProxy.Text = "使用代理";
            this.ckbxIsUserProxy.UseVisualStyleBackColor = true;
            // 
            // btnSaveNetwork
            // 
            this.btnSaveNetwork.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveNetwork.Location = new System.Drawing.Point(440, 20);
            this.btnSaveNetwork.Name = "btnSaveNetwork";
            this.btnSaveNetwork.Size = new System.Drawing.Size(84, 23);
            this.btnSaveNetwork.TabIndex = 0;
            this.btnSaveNetwork.Text = "应用";
            this.btnSaveNetwork.UseVisualStyleBackColor = true;
            this.btnSaveNetwork.Click += new System.EventHandler(this.btnSaveNetwork_Click);
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(200, 23);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(35, 12);
            this.lblProxyPort.TabIndex = 4;
            this.lblProxyPort.Text = "端口:";
            // 
            // tbxProxyPort
            // 
            this.tbxProxyPort.Location = new System.Drawing.Point(241, 20);
            this.tbxProxyPort.Name = "tbxProxyPort";
            this.tbxProxyPort.Size = new System.Drawing.Size(41, 21);
            this.tbxProxyPort.TabIndex = 3;
            // 
            // tbxProxyAddress
            // 
            this.tbxProxyAddress.Location = new System.Drawing.Point(48, 20);
            this.tbxProxyAddress.Name = "tbxProxyAddress";
            this.tbxProxyAddress.Size = new System.Drawing.Size(136, 21);
            this.tbxProxyAddress.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelProject);
            this.groupBox2.Controls.Add(this.ckbxIsActivate);
            this.groupBox2.Controls.Add(this.cbbxCharset);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.ckbxEditProject);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbxProjectName);
            this.groupBox2.Controls.Add(this.btnNewProject);
            this.groupBox2.Controls.Add(this.btnSaveProjectInfo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbxCurrentPostId);
            this.groupBox2.Controls.Add(this.tbxCurrentPageId);
            this.groupBox2.Controls.Add(this.tbxTotalPageCount);
            this.groupBox2.Controls.Add(this.tbxUrl);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbbxProjects);
            this.groupBox2.Location = new System.Drawing.Point(12, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(536, 205);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "项目";
            // 
            // btnDelProject
            // 
            this.btnDelProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelProject.Location = new System.Drawing.Point(70, 176);
            this.btnDelProject.Name = "btnDelProject";
            this.btnDelProject.Size = new System.Drawing.Size(55, 23);
            this.btnDelProject.TabIndex = 23;
            this.btnDelProject.Text = "删除";
            this.btnDelProject.UseVisualStyleBackColor = true;
            this.btnDelProject.Click += new System.EventHandler(this.btnDelProject_Click);
            // 
            // ckbxIsActivate
            // 
            this.ckbxIsActivate.AutoSize = true;
            this.ckbxIsActivate.Checked = true;
            this.ckbxIsActivate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxIsActivate.Enabled = false;
            this.ckbxIsActivate.Location = new System.Drawing.Point(408, 22);
            this.ckbxIsActivate.Name = "ckbxIsActivate";
            this.ckbxIsActivate.Size = new System.Drawing.Size(60, 16);
            this.ckbxIsActivate.TabIndex = 6;
            this.ckbxIsActivate.Text = "活动的";
            this.ckbxIsActivate.UseVisualStyleBackColor = true;
            // 
            // cbbxCharset
            // 
            this.cbbxCharset.Enabled = false;
            this.cbbxCharset.FormattingEnabled = true;
            this.cbbxCharset.Items.AddRange(new object[] {
            "GBK",
            "UTF-8",
            "BIG5"});
            this.cbbxCharset.Location = new System.Drawing.Point(408, 46);
            this.cbbxCharset.Name = "cbbxCharset";
            this.cbbxCharset.Size = new System.Drawing.Size(116, 20);
            this.cbbxCharset.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(366, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "编码:";
            // 
            // ckbxEditProject
            // 
            this.ckbxEditProject.AutoSize = true;
            this.ckbxEditProject.Location = new System.Drawing.Point(415, 180);
            this.ckbxEditProject.Name = "ckbxEditProject";
            this.ckbxEditProject.Size = new System.Drawing.Size(48, 16);
            this.ckbxEditProject.TabIndex = 6;
            this.ckbxEditProject.Text = "修改";
            this.ckbxEditProject.UseVisualStyleBackColor = true;
            this.ckbxEditProject.CheckedChanged += new System.EventHandler(this.ckbxEditProject_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "名字:";
            // 
            // tbxProjectName
            // 
            this.tbxProjectName.Location = new System.Drawing.Point(47, 46);
            this.tbxProjectName.Name = "tbxProjectName";
            this.tbxProjectName.ReadOnly = true;
            this.tbxProjectName.Size = new System.Drawing.Size(313, 21);
            this.tbxProjectName.TabIndex = 18;
            // 
            // btnNewProject
            // 
            this.btnNewProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewProject.Location = new System.Drawing.Point(9, 176);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(55, 23);
            this.btnNewProject.TabIndex = 17;
            this.btnNewProject.Text = "新建";
            this.btnNewProject.UseVisualStyleBackColor = true;
            this.btnNewProject.Click += new System.EventHandler(this.btnNewProject_Click);
            // 
            // btnSaveProjectInfo
            // 
            this.btnSaveProjectInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveProjectInfo.Enabled = false;
            this.btnSaveProjectInfo.Location = new System.Drawing.Point(469, 176);
            this.btnSaveProjectInfo.Name = "btnSaveProjectInfo";
            this.btnSaveProjectInfo.Size = new System.Drawing.Size(55, 23);
            this.btnSaveProjectInfo.TabIndex = 16;
            this.btnSaveProjectInfo.Text = "应用";
            this.btnSaveProjectInfo.UseVisualStyleBackColor = true;
            this.btnSaveProjectInfo.Click += new System.EventHandler(this.btnSaveProjectInfo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "此id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "此页:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "总页:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "地址:";
            // 
            // tbxCurrentPostId
            // 
            this.tbxCurrentPostId.Location = new System.Drawing.Point(47, 152);
            this.tbxCurrentPostId.Name = "tbxCurrentPostId";
            this.tbxCurrentPostId.ReadOnly = true;
            this.tbxCurrentPostId.Size = new System.Drawing.Size(477, 21);
            this.tbxCurrentPostId.TabIndex = 9;
            // 
            // tbxCurrentPageId
            // 
            this.tbxCurrentPageId.Location = new System.Drawing.Point(47, 125);
            this.tbxCurrentPageId.Name = "tbxCurrentPageId";
            this.tbxCurrentPageId.ReadOnly = true;
            this.tbxCurrentPageId.Size = new System.Drawing.Size(477, 21);
            this.tbxCurrentPageId.TabIndex = 8;
            // 
            // tbxTotalPageCount
            // 
            this.tbxTotalPageCount.Location = new System.Drawing.Point(47, 98);
            this.tbxTotalPageCount.Name = "tbxTotalPageCount";
            this.tbxTotalPageCount.ReadOnly = true;
            this.tbxTotalPageCount.Size = new System.Drawing.Size(477, 21);
            this.tbxTotalPageCount.TabIndex = 7;
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(47, 71);
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.ReadOnly = true;
            this.tbxUrl.Size = new System.Drawing.Size(477, 21);
            this.tbxUrl.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "项目:";
            // 
            // cbbxProjects
            // 
            this.cbbxProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbxProjects.FormattingEnabled = true;
            this.cbbxProjects.Location = new System.Drawing.Point(47, 20);
            this.cbbxProjects.Name = "cbbxProjects";
            this.cbbxProjects.Size = new System.Drawing.Size(313, 20);
            this.cbbxProjects.TabIndex = 0;
            this.cbbxProjects.SelectedIndexChanged += new System.EventHandler(this.cbbxProjects_SelectedIndexChanged);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 292);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConfigForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Himeliya.Controls.Button btnSaveNetwork;
        private System.Windows.Forms.Label lblProxyAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbxIsUserProxy;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.TextBox tbxProxyPort;
        private System.Windows.Forms.TextBox tbxProxyAddress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbxProjects;
        private Himeliya.Controls.Button btnNewProject;
        private Himeliya.Controls.Button btnSaveProjectInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxCurrentPostId;
        private System.Windows.Forms.TextBox tbxCurrentPageId;
        private System.Windows.Forms.TextBox tbxTotalPageCount;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxProjectName;
        private System.Windows.Forms.CheckBox ckbxEditProject;
        private System.Windows.Forms.ComboBox cbbxCharset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox ckbxIsActivate;
        private Himeliya.Controls.Button btnDelProject;
    }
}