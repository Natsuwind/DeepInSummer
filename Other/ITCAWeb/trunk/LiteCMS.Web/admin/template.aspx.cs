using System;
using System.Data;
using System.IO;
using System.Collections;
using Natsuhime;
using System.Reflection;
using System.Diagnostics;
using LiteCMS.Config;
using System.Collections.Generic;

namespace LiteCMS.Web.Admin
{
    public partial class template : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Natsuhime.Web.YRequest.GetString("action");
            if (action == "browser")
            {
                cbxlTemplateFileList.DataTextField = "filename";
                cbxlTemplateFileList.DataValueField = "fullfilename";
                cbxlTemplateFileList.DataSource = LoadTemplateFileList();
                cbxlTemplateFileList.DataBind();
            }
            else if (action == "create")
            {
                List<KeyValuePair<string, string>> templateFileList = new List<KeyValuePair<string, string>>();
                string rootTemplatePath = Server.MapPath(string.Format(@"~\templates\{0}", MainConfigs.Load().Templatefolder));

                string folder = Natsuhime.Web.YRequest.GetString("folder");
                if (folder.Length > 0)
                {
                    string[] baseRefTemplateList = Directory.GetFiles(rootTemplatePath, "_*.htm");
                    foreach (string file in baseRefTemplateList)
                    {
                        templateFileList.Add(new KeyValuePair<string, string>(file, ""));
                    }

                    string folderTemplatePath = Path.Combine(rootTemplatePath, folder);
                    string[] mainTemplateList = Directory.GetFiles(folderTemplatePath, "*.htm");
                    foreach (string file in mainTemplateList)
                    {
                        templateFileList.Add(new KeyValuePair<string, string>(file, Server.MapPath("~/" + folder)));
                    }
                }
                else
                {
                    string[] allTemplateList = Directory.GetFiles(rootTemplatePath, "*.htm", SearchOption.AllDirectories);
                    foreach (string file in allTemplateList)
                    {
                        string currentFolder = Path.GetDirectoryName(file);
                        string outputPath = Server.MapPath("~\\" + currentFolder.Replace(rootTemplatePath, ""));
                        templateFileList.Add(new KeyValuePair<string, string>(file, outputPath));
                    }
                }
                CreateTemplate(templateFileList);
                ShowMsg("模板管理", "生成模板成功.", "", "frame.aspx?action=template", true);
            }
            else if (action == "settemplate")
            {
                string folder = Natsuhime.Web.YRequest.GetString("folder");

                MainConfigInfo info = MainConfigs.Load();
                info.Templatefolder = folder;
                MainConfigs.Save(info);
                MainConfigs.ResetConfig();
                ShowMsg("模板管理", "设置默认模板成功.", "", "frame.aspx?action=template", true);
            }
            else
            {
                rptTemplateList.DataSource = LoadTemplateFolder();
                rptFolderList.DataSource = LoadSubFolder();
                rptTemplateList.DataBind();
                rptFolderList.DataBind();
            }
        }

        private DataTable LoadSubFolder()
        {
            string[] folders = Directory.GetDirectories(Server.MapPath("~/templates/" + MainConfigs.Load().Templatefolder));

            DataTable folderlist = new DataTable("folderlist");
            folderlist.Columns.Add("folder", Type.GetType("System.String"));

            foreach (string folder in folders)
            {
                DataRow dr = folderlist.NewRow();
                dr["folder"] = folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                folderlist.Rows.Add(dr);
            }
            return folderlist;
        }
        private DataTable LoadTemplateFolder()
        {
            string[] folders = Directory.GetDirectories(Server.MapPath("~/templates/"));

            DataTable folderlist = new DataTable("folderlist");
            folderlist.Columns.Add("template_folder", Type.GetType("System.String"));

            foreach (string folder in folders)
            {
                DataRow dr = folderlist.NewRow();
                dr["template_folder"] = folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                folderlist.Rows.Add(dr);
            }
            return folderlist;
        }
        private DataTable LoadTemplateFileList()
        {
            #region 装入模板文件
            DataTable templatefilelist = new DataTable("templatefilelist");

            templatefilelist.Columns.Add("fullfilename", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filename", Type.GetType("System.String"));
            templatefilelist.Columns.Add("id", Type.GetType("System.Int32"));
            templatefilelist.Columns.Add("extension", Type.GetType("System.String"));
            templatefilelist.Columns.Add("parentid", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filepath", Type.GetType("System.String"));
            templatefilelist.Columns.Add("filedescription", Type.GetType("System.String"));

            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("~/templates/" + MainConfigs.Load().Templatefolder));
            int i = 1;
            string extname;
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    extname = file.Extension.ToLower();
                    if (extname == ".htm" || extname == ".config")
                    {
                        DataRow dr = templatefilelist.NewRow();

                        dr["id"] = i;
                        dr["filename"] = file.Name.Substring(0, file.Name.LastIndexOf("."));
                        if (extname == ".htm")
                            dr["fullfilename"] = "\\" + dr["filename"] + ".htm";
                        else
                            dr["fullfilename"] = "\\" + dr["filename"] + ".config";
                        dr["extension"] = file.Extension.ToLower();
                        dr["parentid"] = "0";
                        dr["filepath"] = "";
                        dr["filedescription"] = "";
                        templatefilelist.Rows.Add(dr);
                        i++;
                    }
                }
            }

            foreach (DataRow dr in templatefilelist.Rows)
            {
                foreach (DataRow childdr in templatefilelist.Select("filename like '" + dr["filename"].ToString() + "_%%'"))
                {
                    if (dr["filename"].ToString() != childdr["filename"].ToString())
                    {
                        childdr["parentid"] = dr["id"].ToString();
                    }
                }
            }

            return templatefilelist;
            #endregion
        }

        protected void btnCreateAll_Click(object sender, EventArgs e)
        {
            string folder = "";
            //CreateTemplate(folder);
            /*
            Hashtable ht = new Hashtable();
            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("~/templates/"));
            Template tp = new Template();

            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null && file.Extension.ToLower().Equals(".htm") && file.Name.IndexOf("_") != 0)
                {
                    ht[file.Name] = file;
                }
            }

            foreach (string key in ht.Keys)
            {
                string fullfilename = key.Split('.')[0];
                tp.CreateTemplate(Server.MapPath("~/"), fullfilename, 1);
            }
             */
            Response.Write("OK!");
        }

        private void CreateTemplate(List<KeyValuePair<string, string>> templateFileList)
        {
            NewTemplate ntp = new NewTemplate("LiteCMS.Web", "");
            ntp.Productname = "LiteCMS";
            ntp.Productversion = Config.Versions.GetProductVersionFromAssembly();
            ntp.CreateFromFileList(templateFileList);
            //if (folder.Length > 0)
            //{
            //    ntp.CreateFromFolder(Server.MapPath(string.Format("~/templates/{0}/{1}/", MainConfigs.Load().Templatefolder, folder)), Server.MapPath(string.Format("~/{0}/", folder)));
            //}
            //else
            //{
            //}
        }
    }
}
