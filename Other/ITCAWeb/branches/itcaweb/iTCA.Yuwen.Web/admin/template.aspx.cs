using System;
using System.Data;
using System.IO;
using System.Collections;
using Natsuhime;

namespace iTCA.Yuwen.Web.Admin
{
    public partial class template : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cbxlTemplateFileList.DataTextField = "filename";
            cbxlTemplateFileList.DataValueField = "fullfilename";
            cbxlTemplateFileList.DataSource = LoadTemplateFileList();
            cbxlTemplateFileList.DataBind();
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

            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("~/templates/"));
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
            NewTemplate ntp = new NewTemplate("test","");
            ntp.CreateFromFolder(Server.MapPath("~/templates/"), Server.MapPath("~/"));
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
    }
}
