using System;
using System.Web;
using System.IO;
using System.Text;
using System.Drawing;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Preview;

namespace Discuz.Mall
{
    /// <summary>
    /// 商城工具类
    /// </summary>
    public class MallUtils
    {
        /// <summary>
        /// 操作运行符枚举
        /// </summary>
        public enum OperaCode
        {
            Equal = 1, //等于
            NoEuqal = 2, //不等于
            Morethan = 3, //大于
            MorethanOrEqual = 4, //大于或等于
            Lessthan = 5,  //小于
            LessthanOrEqual = 6 //小于或等于
        }


        /// <summary>
        /// 保存上传的文件
        /// </summary>
        /// <param name="categoryid">商品分类id</param>
        /// <param name="MaxAllowFileCount">最大允许的上传文件个数</param>
        /// <param name="MaxSizePerDay">每天允许的附件大小总数</param>
        /// <param name="MaxFileSize">单个最大允许的文件字节数</param>/// 
        /// <param name="TodayUploadedSize">今天已经上传的附件字节总数</param>
        /// <param name="AllowFileType">允许的文件类型, 以string[]形式提供</param>
        /// <param name="config">附件保存方式 0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录</param>
        /// <param name="watermarkstatus">图片水印位置</param>
        /// <param name="filekey">File控件的Key(即Name属性)</param>
        /// <returns>文件信息结构</returns>
        public static Goodsattachmentinfo[] SaveRequestFiles(int categoryId, int MaxAllowFileCount, int MaxSizePerDay, int MaxFileSize, int TodayUploadedSize, string AllowFileType, int waterMarkStatus, GeneralConfigInfo config, string fileKey)
        {
            string[] tmp = Utils.SplitString(AllowFileType, "\r\n");
            string[] AllowFileExtName = new string[tmp.Length];
            int[] MaxSize = new int[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                AllowFileExtName[i] = Utils.CutString(tmp[i], 0, tmp[i].LastIndexOf(","));
                MaxSize[i] = Utils.StrToInt(Utils.CutString(tmp[i], tmp[i].LastIndexOf(",") + 1), 0);
            }

            int saveFileCount = 0;

            int fCount = HttpContext.Current.Request.Files.Count;

            for (int i = 0; i < fCount; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(fileKey))
                    saveFileCount++;
            }

            Goodsattachmentinfo[] attachmentInfo = new Goodsattachmentinfo[saveFileCount];
            if (saveFileCount > MaxAllowFileCount)
                return attachmentInfo;

            saveFileCount = 0;

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < fCount; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(fileKey))
                {
                    string fileName = Path.GetFileName(HttpContext.Current.Request.Files[i].FileName);
                    string fileExtName = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();
                    string fileType = HttpContext.Current.Request.Files[i].ContentType.ToLower();
                    int fileSize = HttpContext.Current.Request.Files[i].ContentLength;
                    string newFileName = "";

                    attachmentInfo[saveFileCount] = new Goodsattachmentinfo();

                    attachmentInfo[saveFileCount].Sys_noupload = "";

                    // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                    if (!(Utils.IsImgFilename(fileName) && !fileType.StartsWith("image")))
                    {
                        int extNameId = Utils.GetInArrayID(fileExtName, AllowFileExtName);

                        if (extNameId >= 0 && (fileSize <= MaxSize[extNameId]) && (MaxFileSize >= fileSize /*|| MaxAllSize == 0*/) && (MaxSizePerDay - TodayUploadedSize >= fileSize))
                        {
                            TodayUploadedSize = TodayUploadedSize + fileSize;
                            string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/mall/");
                            StringBuilder saveDir = new StringBuilder("");
                            //附件保存方式 0=按年/月/日存入不同目录 1=按年/月/日/论坛存入不同目录 2=按论坛存入不同目录 3=按文件类型存入不同目录
                            if (config.Attachsave == 1)
                            {
                                saveDir.Append(DateTime.Now.ToString("yyyy"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                                saveDir.Append(DateTime.Now.ToString("MM"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                                saveDir.Append(DateTime.Now.ToString("dd"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                                saveDir.Append(categoryId.ToString());
                                saveDir.Append(Path.DirectorySeparatorChar);
                            }
                            else if (config.Attachsave == 2)
                            {
                                saveDir.Append(categoryId);
                                saveDir.Append(Path.DirectorySeparatorChar);
                            }
                            else if (config.Attachsave == 3)
                            {
                                saveDir.Append(fileExtName);
                                saveDir.Append(Path.DirectorySeparatorChar);
                            }
                            else
                            {
                                saveDir.Append(DateTime.Now.ToString("yyyy"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                                saveDir.Append(DateTime.Now.ToString("MM"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                                saveDir.Append(DateTime.Now.ToString("dd"));
                                saveDir.Append(Path.DirectorySeparatorChar);
                            }


                            newFileName = (Environment.TickCount & int.MaxValue).ToString() + i + random.Next(1000, 9999) + "." + fileExtName;

                            //临时文件名称变量. 用于当启动远程附件之后,先上传到本地临时文件夹的路径信息
                            string tempFileName = "";
                            //当支持FTP上传附件且不保留本地附件时
                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                            {
                                // 如果指定目录不存在则建立临时路径
                                if (!Directory.Exists(UploadDir + "temp\\"))
                                    Utils.CreateDir(UploadDir + "temp\\");

                                tempFileName = "temp\\" + newFileName;
                            }
                            else
                            {
                                // 如果指定目录不存在则建立
                                if (!Directory.Exists(UploadDir + saveDir.ToString()))
                                    Utils.CreateDir(UploadDir + saveDir.ToString());
                            }
                            newFileName = saveDir.ToString() + newFileName;

                            try
                            {
                                // 如果是bmp jpg png图片类型
                                if ((fileExtName == "bmp" || fileExtName == "jpg" || fileExtName == "jpeg" || fileExtName == "png") && fileType.StartsWith("image"))
                                {

                                    Image img = Image.FromStream(HttpContext.Current.Request.Files[i].InputStream);
                                    if (config.Attachimgmaxwidth > 0 && img.Width > config.Attachimgmaxwidth)
                                        attachmentInfo[saveFileCount].Sys_noupload = "图片宽度为" + img.Width + ", 系统允许的最大宽度为" + config.Attachimgmaxwidth;

                                    if (config.Attachimgmaxheight > 0 && img.Height > config.Attachimgmaxheight)
                                        attachmentInfo[saveFileCount].Sys_noupload = "图片高度为" + img.Width + ", 系统允许的最大高度为" + config.Attachimgmaxheight;

                                    if (attachmentInfo[saveFileCount].Sys_noupload == "")
                                    {
                                        if (waterMarkStatus == 0)
                                        {
                                            //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                                HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                            else
                                                HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);

                                            attachmentInfo[saveFileCount].Filesize = fileSize;
                                        }
                                        else
                                        {
                                            if (config.Watermarktype == 1 && File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic)))
                                            {
                                                //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                                if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                                    ForumUtils.AddImageSignPic(img, UploadDir + tempFileName, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                                                else
                                                    ForumUtils.AddImageSignPic(img, UploadDir + newFileName, Utils.GetMapPath(BaseConfigs.GetForumPath + "watermark/" + config.Watermarkpic), config.Watermarkstatus, config.Attachimgquality, config.Watermarktransparency);
                                            }
                                            else
                                            {
                                                string watermarkText;
                                                watermarkText = config.Watermarktext.Replace("{1}", config.Forumtitle);
                                                watermarkText = watermarkText.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                                                watermarkText = watermarkText.Replace("{3}", Utils.GetDate());
                                                watermarkText = watermarkText.Replace("{4}", Utils.GetTime());

                                                //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                                if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                                    ForumUtils.AddImageSignText(img, UploadDir + tempFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                                else
                                                    ForumUtils.AddImageSignText(img, UploadDir + newFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                            }

                                            //当支持FTP上传附件且不保留本地附件模式时,则读取临时目录下的文件信息
                                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                                attachmentInfo[saveFileCount].Filesize = new FileInfo(UploadDir + tempFileName).Length;
                                            else
                                                attachmentInfo[saveFileCount].Filesize = new FileInfo(UploadDir + newFileName).Length;
                                        }
                                    }
                                }
                                else
                                {

                                    attachmentInfo[saveFileCount].Filesize = fileSize;
                                    //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }
                            catch
                            {
                                //当上传目录和临时文件夹都没有上传的文件时
                                if (!(Utils.FileExists(UploadDir + tempFileName)) && (!(Utils.FileExists(UploadDir + newFileName))))
                                {
                                    attachmentInfo[saveFileCount].Filesize = fileSize;
                                    //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }

                            try
                            {
                                //加载文件预览类指定方法
                                IPreview preview = PreviewProvider.GetInstance(fileExtName.Trim());
                                if (preview != null)
                                {
                                    preview.UseFTP = (FTPs.GetMallAttachInfo.Allowupload == 1) ? true : false;
                                    //当支持FTP上传附件且不保留本地附件模式时
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        preview.OnSaved(UploadDir + tempFileName);
                                    else
                                        preview.OnSaved(UploadDir + newFileName);
                                }
                            }
                            catch
                            { }

                            //当支持FTP上传附件时,使用FTP上传远程附件
                            if (FTPs.GetMallAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();
                                //当不保留本地附件模式时,在上传完成之后删除本地tempfilename文件
                                if (FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + tempFileName, FTPs.FTPUploadEnum.ForumAttach);
                                else
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + newFileName, FTPs.FTPUploadEnum.ForumAttach);
                            }
                        }
                        else
                        {
                            if (extNameId < 0)
                                attachmentInfo[saveFileCount].Sys_noupload = "文件格式无效";
                            else if (MaxSizePerDay - TodayUploadedSize < fileSize)
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于今天允许上传的字节数";
                            else if (fileSize > MaxSize[extNameId])
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于该类型附件允许的字节数";
                            else
                                attachmentInfo[saveFileCount].Sys_noupload = "文件大于单个文件允许上传的字节数";
                        }
                    }
                    else
                        attachmentInfo[saveFileCount].Sys_noupload = "文件格式无效";

                    //当支持FTP上传附件时
                    if (FTPs.GetMallAttachInfo.Allowupload == 1)
                        attachmentInfo[saveFileCount].Filename = FTPs.GetMallAttachInfo.Remoteurl + "/" + newFileName.Replace("\\", "/");
                    else
                        attachmentInfo[saveFileCount].Filename = "mall/" + newFileName;

                    attachmentInfo[saveFileCount].Description = fileExtName;
                    attachmentInfo[saveFileCount].Filetype = fileType;
                    attachmentInfo[saveFileCount].Attachment = fileName;
                    attachmentInfo[saveFileCount].Postdatetime = DateTime.Now.ToString();
                    attachmentInfo[saveFileCount].Sys_index = i;
                    saveFileCount++;
                }
            }
            return attachmentInfo;
        }

        /// <summary>
        /// 上传店标文件
        /// </summary>
        /// <param name="MaxFileSize">最大文件上传尺寸</param>
        /// <param name="AllowFileType">允许上传文件类型</param>
        /// <param name="config">配置对象信息</param>
        /// <param name="filekey">File控件的Key(即Name属性)</param>
        /// <returns>文件信息结构</returns>
        public static string SaveRequestFile(int MaxFileSize, string AllowFileType, GeneralConfigInfo config, string filekey)
        {            
            string[] tmp = Utils.SplitString(AllowFileType, "\r\n");
     
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (!HttpContext.Current.Request.Files[i].FileName.Equals("") && HttpContext.Current.Request.Files.AllKeys[i].Equals(filekey))
                {
                    string fileName = Path.GetFileName(HttpContext.Current.Request.Files[i].FileName);
                    string fileExtName = Utils.CutString(fileName, fileName.LastIndexOf(".") + 1).ToLower();
                    string fileType = HttpContext.Current.Request.Files[i].ContentType.ToLower();
                    int fileSize = HttpContext.Current.Request.Files[i].ContentLength;
                    string newFileName = "";
                   
                    // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                    if (!(Utils.IsImgFilename(fileName) && !fileType.StartsWith("image")))
                    {
                        int extNameId = Utils.GetInArrayID(fileExtName, tmp);

                        if (extNameId >= 0 && MaxFileSize >= fileSize)
                        {
                            string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/mall/");
                            StringBuilder savedir = new StringBuilder("");
                                savedir.Append(DateTime.Now.ToString("yyyy"));
                                savedir.Append(Path.DirectorySeparatorChar);
                                savedir.Append(DateTime.Now.ToString("MM"));
                                savedir.Append(Path.DirectorySeparatorChar);
                                savedir.Append(DateTime.Now.ToString("dd"));
                                savedir.Append(Path.DirectorySeparatorChar);

                            newFileName = (Environment.TickCount & int.MaxValue).ToString() + i.ToString() + random.Next(1000, 9999).ToString() + "." + fileExtName;

                            //临时文件名称变量. 用于当启动远程附件之后,先上传到本地临时文件夹的路径信息
                            string tempfilename = "";
                            //当支持FTP上传附件且不保留本地附件时
                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                            {
                                // 如果指定目录不存在则建立临时路径
                                if (!Directory.Exists(UploadDir + "temp\\"))
                                    Utils.CreateDir(UploadDir + "temp\\");

                                tempfilename = "temp\\" + newFileName;
                            }
                            else
                            {
                                // 如果指定目录不存在则建立
                                if (!Directory.Exists(UploadDir + savedir.ToString()))
                                    Utils.CreateDir(UploadDir + savedir.ToString());
                            }
                            newFileName = savedir.ToString() + newFileName;

                            try
                            {
                                //当上传目录和临时文件夹都没有上传的文件时
                                if (!(Utils.FileExists(UploadDir + tempfilename)) && (!(Utils.FileExists(UploadDir + newFileName))))
                                {

                                    //当支持FTP上传附件且不保留本地附件模式时,则先上传到临时目录下
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempfilename);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }
                            catch
                            {}

                            //当支持FTP上传附件时,使用FTP上传远程附件
                            if (FTPs.GetMallAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();

                                //当不保留本地附件模式时,在上传完成之后删除本地tempfilename文件
                                if (FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + tempfilename, FTPs.FTPUploadEnum.ForumAttach);
                                else
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + newFileName, FTPs.FTPUploadEnum.ForumAttach);
                            }
                        }
                        else
                           return (extNameId < 0) ? "文件格式无效" : "文件大于单个文件允许上传的字节数";
                    }
                    else
                        return "文件格式无效";

                    //当支持FTP上传附件时
                    if (FTPs.GetMallAttachInfo.Allowupload == 1)
                        return FTPs.GetMallAttachInfo.Remoteurl + "/" + newFileName.Replace("\\", "/");
                    else
                        return "mall/" + newFileName;
                }
            }
            return "";
        }
    }
}
