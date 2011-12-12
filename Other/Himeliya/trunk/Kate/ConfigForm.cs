using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime.Common;
using System.IO;
using Himeliya.Kate.Entity;
using Natsuhime.Data;

namespace Himeliya.Kate
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void btnSaveNetwork_Click(object sender, EventArgs e)
        {
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            BindProjectList();

            #region old file config
            //this.config = Config.LoadConfig(this.configPath);
            //if (config.ContainsKey("UseProxy"))
            //{
            //    this.ckbxIsUserProxy.Checked = bool.Parse(this.config["UseProxy"].ToString());
            //}
            //if (config.ContainsKey("ProxyAddress"))
            //{
            //    this.tbxProxyAddress.Text = this.config["ProxyAddress"].ToString();
            //}
            //if (config.ContainsKey("ProxyPort"))
            //{
            //    this.tbxProxyPort.Text = this.config["ProxyPort"].ToString();
            //}


            //if (config.ContainsKey("Projects"))
            //{
            //    List<ProjectInfo> projects = this.config["Projects"] as List<ProjectInfo>;
            //    this.cbbxProjects.DataSource = projects;
            //    //this.cbbxProjects.ValueMember = "Key";
            //    this.cbbxProjects.DisplayMember = "Name";
            //}
            #endregion
        }

        private void BindProjectList()
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM projects");
            List<ProjectInfo> projects = new List<ProjectInfo>();
            while (dr.Read())
            {
                ProjectInfo pi = new ProjectInfo();
                pi.Id = Convert.ToInt32(dr["id"]);
                pi.Name = dr["name"].ToString();
                pi.Url = dr["fetch_url"].ToString();
                pi.Charset = dr["charset"].ToString();
                pi.TotalPageCount = Convert.ToInt32(dr["total_page_count"]);
                pi.CurrentPageId = Convert.ToInt32(dr["current_page_id"]);
                pi.CurrentPostId = Convert.ToInt32(dr["current_post_id"]);
                pi.IsActivate = Convert.ToInt32(dr["is_activate"]);

                projects.Add(pi);
            }
            dr.Close();
            this.cbbxProjects.DataSource = projects;
            this.cbbxProjects.DisplayMember = "Name";
        }

        private void btnNewProject_Click(object sender, EventArgs e)
        {
            ChangeInputStatus(false);
        }

        private void btnDelProject_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                this, 
                "确认删除?", 
                "确认 - Kate", 
                MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                ProjectInfo pi = this.cbbxProjects.SelectedItem as ProjectInfo;
                if (pi != null)
                {
                    string sql = string.Format("DELETE FROM projects WHERE id={0}", pi.Id);
                    try
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                        this.BindProjectList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }


        private void btnSaveProjectInfo_Click(object sender, EventArgs e)
        {
            if (this.ckbxEditProject.Checked)
            {
                ProjectInfo pi = this.cbbxProjects.SelectedItem as ProjectInfo;

                string sql = string.Format(
                    "UPDATE projects SET `name`='{0}',`fetch_url`='{1}',`charset`='{2}',`is_activate`={3} WHERE `id`={4}",
                    this.tbxProjectName.Text.Trim(),
                    this.tbxUrl.Text.Trim(),
                    this.cbbxCharset.Text.Trim(),
                    Convert.ToInt32(this.ckbxIsActivate.Checked),
                    pi.Id
                    );

                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                    this.ckbxEditProject.Checked = false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                string sql = string.Format(
                    "INSERT INTO projects(`name`,`fetch_url`,`charset`,`is_activate`) VALUES('{0}','{1}','{2}',{3})",
                    this.tbxProjectName.Text.Trim(),
                    this.tbxUrl.Text.Trim(),
                    this.cbbxCharset.Text.Trim(),
                    Convert.ToInt32(this.ckbxIsActivate.Checked)
                    );
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            this.BindProjectList();


            #region old file config
            //List<ProjectInfo> projects = this.config.ContainsKey("Projects") ? this.config["Projects"] as List<ProjectInfo> : null;

            //if (projects == null)
            //{
            //    projects = new List<ProjectInfo>();
            //}

            //ProjectInfo pi = new ProjectInfo();
            //pi.Name = this.tbxProjectName.Text.Trim();
            //projects.Add(pi);

            //if (this.config.ContainsKey("Projects"))
            //{
            //    this.config["Projects"] = projects;
            //}
            //else
            //{
            //    this.config.Add("Projects", projects);
            //}
            //Config.SaveConfig(this.configPath, config);
            #endregion
        }

        private void ckbxEditProject_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeInputStatus(!this.ckbxEditProject.Checked);
        }

        private void cbbxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectInfo pi = this.cbbxProjects.SelectedItem as ProjectInfo;
            if (pi != null)
            {
                this.tbxProjectName.Text = pi.Name;
                this.ckbxIsActivate.Checked = Convert.ToBoolean(pi.IsActivate);
                this.tbxUrl.Text = pi.Url;
                this.cbbxCharset.Text = pi.Charset;
                this.tbxTotalPageCount.Text = pi.TotalPageCount.ToString();
                this.tbxCurrentPageId.Text = pi.CurrentPageId.ToString();
                this.tbxCurrentPostId.Text = pi.CurrentPostId.ToString();
            }
        }

        void ChangeInputStatus(bool isReadOnly)
        {
            this.tbxCurrentPostId.ReadOnly =
                this.tbxCurrentPageId.ReadOnly =
                this.tbxTotalPageCount.ReadOnly =
                this.tbxProjectName.ReadOnly =
                this.tbxUrl.ReadOnly = isReadOnly;

            this.ckbxIsActivate.Enabled =
                this.btnSaveProjectInfo.Enabled =
                this.cbbxCharset.Enabled = !isReadOnly;
        }
    }
}
