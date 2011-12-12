using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Natsuhime.Common;

namespace Comsenz.Ywen.SWMUpdateCreator
{
    public partial class MainForm : Form
    {
        UpdateListXmlInfo ulxiLastXML;
        List<string> excludeExt;
        List<string> excludeFiles;
        public MainForm()
        {
            InitializeComponent();

            excludeExt = new List<string>();
            excludeExt.Add(".xml");
            excludeExt.Add(".pdb");
            excludeExt.Add(".sdf");
            excludeExt.Add(".txt");
            excludeExt.Add(".rtf");
            excludeExt.Add(".bat");

            excludeFiles = new List<string>();
            excludeFiles.Add("Yuwen.Reference.dll");
            excludeFiles.Add("SuperWebmaster.vshost.exe");
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.cbbxReleaseFolder.SelectedIndex = 0;
            this.cbbxCreateFolder.SelectedIndex = 0;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            string releaseFolder = cbbxReleaseFolder.Text.Trim();
            ulxiLastXML = SerializationHelper.LoadXml(
                typeof(UpdateListXmlInfo),
                Path.Combine(releaseFolder, "updatelist.xml")
                )
                as UpdateListXmlInfo;

            BindAllFilesList(releaseFolder, releaseFolder);

            MessageBox.Show("Load Completed!");
        }

        private void BindAllFilesList(string folder, string baseFolder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            DirectoryInfo[] dirList = di.GetDirectories();
            foreach (DirectoryInfo info in dirList)
            {
                BindAllFilesList(info.FullName, baseFolder);
            }
            FileInfo[] fileList = di.GetFiles();
            foreach (FileInfo fi in fileList)
            {
                if (this.excludeExt.Contains(fi.Extension.ToLower()) || this.excludeFiles.Contains(fi.Name))
                {
                    continue;
                }
                string newMD5 = HashTools.GenerateFileHashCode(fi.FullName);
                string newVersion = FileVersionInfo.GetVersionInfo(fi.FullName).FileVersion;

                string relativeFileName = fi.FullName.Replace(baseFolder, string.Empty).Trim(Path.DirectorySeparatorChar);
                string swmFileName = relativeFileName.Replace(Path.DirectorySeparatorChar, '/');
                string swmCreateName = relativeFileName;
                if (fi.Extension.ToLower() == ".php")
                {
                    swmFileName += "_bak";
                    swmCreateName += "_bak";
                }
                ListViewItem lvi = new ListViewItem(swmFileName);

                SWMFileInfo lastSWMFileInfo = ulxiLastXML.Files.Find(
                    delegate(SWMFileInfo swmfi)
                    {
                        return swmfi.Name == swmFileName;
                    }
                    );
                if (lastSWMFileInfo != null)
                {
                    if (newVersion == null || newVersion == string.Empty)
                    {
                        if (newMD5 == lastSWMFileInfo.MD5)
                        {
                            newVersion = lastSWMFileInfo.Ver;
                        }
                        else
                        {
                            Version ver = new Version(lastSWMFileInfo.Ver);
                            newVersion = new Version(ver.Major, ver.Minor, ver.Build, ver.Revision + 1).ToString();
                        }
                    }

                    if (lastSWMFileInfo.Ver != newVersion)
                    {
                        lvi.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lastSWMFileInfo = new SWMFileInfo();
                    if (newVersion == null || newVersion == string.Empty)
                    {
                        newVersion = "0.2.0.0";
                    }
                    lvi.ForeColor = Color.Purple;
                }
                lvi.Checked = true;


                ListViewItem.ListViewSubItem lvsiNewVersion = new ListViewItem.ListViewSubItem(lvi, newVersion);
                ListViewItem.ListViewSubItem lvsiLastVersion = new ListViewItem.ListViewSubItem(lvi, lastSWMFileInfo.Ver);
                ListViewItem.ListViewSubItem lvsiNewMD5 = new ListViewItem.ListViewSubItem(lvi, newMD5);
                ListViewItem.ListViewSubItem lvsiLastMD5 = new ListViewItem.ListViewSubItem(lvi, lastSWMFileInfo.MD5);
                ListViewItem.ListViewSubItem lvsiFullFileName = new ListViewItem.ListViewSubItem(lvi, fi.FullName);
                ListViewItem.ListViewSubItem lvsiCreateName = new ListViewItem.ListViewSubItem(lvi, swmCreateName);

                lvi.SubItems.Add(lvsiNewVersion);
                lvi.SubItems.Add(lvsiLastVersion);
                lvi.SubItems.Add(lvsiNewMD5);
                lvi.SubItems.Add(lvsiLastMD5);
                lvi.SubItems.Add(lvsiFullFileName);
                lvi.SubItems.Add(lvsiCreateName);
                lvMain.Items.Add(lvi);
            }
        }

        private void btnConfirmCreate_Click(object sender, EventArgs e)
        {
            string createFolder = this.cbbxCreateFolder.Text.Trim();
            if (!Directory.Exists(createFolder))
            {
                Directory.CreateDirectory(createFolder);
            }


            List<SWMFileInfo> updateFileList = new List<SWMFileInfo>();
            foreach (ListViewItem lvi in this.lvMain.CheckedItems)
            {
                SWMFileInfo swmfi = new SWMFileInfo();
                swmfi.Name = lvi.Text;
                swmfi.Ver = lvi.SubItems[1].Text;
                swmfi.MD5 = lvi.SubItems[3].Text;
                swmfi.FullFileName = lvi.SubItems[5].Text;
                swmfi.RelativeCreateName = lvi.SubItems[6].Text;
                updateFileList.Add(swmfi);

                string currentCreateFullFileName = Path.Combine(createFolder, swmfi.RelativeCreateName);
                string currentCreateFolder = Directory.GetParent(currentCreateFullFileName).FullName;
                if (!Directory.Exists(currentCreateFolder))
                {
                    Directory.CreateDirectory(currentCreateFolder);
                }
                File.Copy(swmfi.FullFileName, currentCreateFullFileName, true);
            }
            if (updateFileList.Count > 0)
            {
                CreateXML(updateFileList, createFolder);
                MessageBox.Show("Create Completed!\r\n" + updateFileList.Count.ToString() + " Files Created!");
            }
            else
            {
                MessageBox.Show("没有文件被选中!");
            }
            /*
            string releaseFolder = cbbxReleaseFolder.Text.Trim();
            if (Directory.Exists(releaseFolder))
            {
                ulxi = SerializationHelper.LoadXml(typeof(UpdateListXmlInfo), Path.Combine(releaseFolder, "updatelist.xml")) as UpdateListXmlInfo;
            }
            else
            {
                MessageBox.Show("路径呀");
            }
             */
        }

        private static void CreateXML(List<SWMFileInfo> updateFileList, string xmlPath)
        {
            UpdateListXmlInfo ulxi = new UpdateListXmlInfo();

            ulxi.Description = "SuperWebmaster AutoUpdate";
            ulxi.Updater = new UpdaterInfo();
            ulxi.Updater.Url = "http://zzzs.comsenz.com/data/update/0.2";
            ulxi.Updater.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd");
            ulxi.Application = new ApplicationInfo();
            ulxi.Application.ApplicationId = "ProductManager";
            ulxi.Application.EntryPoint = "SuperWebmaster.exe";
            ulxi.Application.Location = ".";
            ulxi.Application.Version = updateFileList.Find(
                delegate(SWMFileInfo f)
                {
                    return f.Name == "SuperWebmaster.exe";
                }
            ).Ver;
            ulxi.Files = updateFileList;
            ulxi.ChangeLog = "";

            Natsuhime.Common.SerializationHelper.SaveXml(ulxi, Path.Combine(xmlPath, "updatelist.xml"));
        }

    }
}
