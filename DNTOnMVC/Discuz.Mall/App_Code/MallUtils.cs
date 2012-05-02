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
    /// �̳ǹ�����
    /// </summary>
    public class MallUtils
    {
        /// <summary>
        /// �������з�ö��
        /// </summary>
        public enum OperaCode
        {
            Equal = 1, //����
            NoEuqal = 2, //������
            Morethan = 3, //����
            MorethanOrEqual = 4, //���ڻ����
            Lessthan = 5,  //С��
            LessthanOrEqual = 6 //С�ڻ����
        }


        /// <summary>
        /// �����ϴ����ļ�
        /// </summary>
        /// <param name="categoryid">��Ʒ����id</param>
        /// <param name="MaxAllowFileCount">���������ϴ��ļ�����</param>
        /// <param name="MaxSizePerDay">ÿ������ĸ�����С����</param>
        /// <param name="MaxFileSize">�������������ļ��ֽ���</param>/// 
        /// <param name="TodayUploadedSize">�����Ѿ��ϴ��ĸ����ֽ�����</param>
        /// <param name="AllowFileType">������ļ�����, ��string[]��ʽ�ṩ</param>
        /// <param name="config">�������淽ʽ 0=����/��/�մ��벻ͬĿ¼ 1=����/��/��/��̳���벻ͬĿ¼ 2=����̳���벻ͬĿ¼ 3=���ļ����ʹ��벻ͬĿ¼</param>
        /// <param name="watermarkstatus">ͼƬˮӡλ��</param>
        /// <param name="filekey">File�ؼ���Key(��Name����)</param>
        /// <returns>�ļ���Ϣ�ṹ</returns>
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

                    // �ж� �ļ���չ��/�ļ���С/�ļ����� �Ƿ����Ҫ��
                    if (!(Utils.IsImgFilename(fileName) && !fileType.StartsWith("image")))
                    {
                        int extNameId = Utils.GetInArrayID(fileExtName, AllowFileExtName);

                        if (extNameId >= 0 && (fileSize <= MaxSize[extNameId]) && (MaxFileSize >= fileSize /*|| MaxAllSize == 0*/) && (MaxSizePerDay - TodayUploadedSize >= fileSize))
                        {
                            TodayUploadedSize = TodayUploadedSize + fileSize;
                            string UploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/mall/");
                            StringBuilder saveDir = new StringBuilder("");
                            //�������淽ʽ 0=����/��/�մ��벻ͬĿ¼ 1=����/��/��/��̳���벻ͬĿ¼ 2=����̳���벻ͬĿ¼ 3=���ļ����ʹ��벻ͬĿ¼
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

                            //��ʱ�ļ����Ʊ���. ���ڵ�����Զ�̸���֮��,���ϴ���������ʱ�ļ��е�·����Ϣ
                            string tempFileName = "";
                            //��֧��FTP�ϴ������Ҳ��������ظ���ʱ
                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                            {
                                // ���ָ��Ŀ¼������������ʱ·��
                                if (!Directory.Exists(UploadDir + "temp\\"))
                                    Utils.CreateDir(UploadDir + "temp\\");

                                tempFileName = "temp\\" + newFileName;
                            }
                            else
                            {
                                // ���ָ��Ŀ¼����������
                                if (!Directory.Exists(UploadDir + saveDir.ToString()))
                                    Utils.CreateDir(UploadDir + saveDir.ToString());
                            }
                            newFileName = saveDir.ToString() + newFileName;

                            try
                            {
                                // �����bmp jpg pngͼƬ����
                                if ((fileExtName == "bmp" || fileExtName == "jpg" || fileExtName == "jpeg" || fileExtName == "png") && fileType.StartsWith("image"))
                                {

                                    Image img = Image.FromStream(HttpContext.Current.Request.Files[i].InputStream);
                                    if (config.Attachimgmaxwidth > 0 && img.Width > config.Attachimgmaxwidth)
                                        attachmentInfo[saveFileCount].Sys_noupload = "ͼƬ���Ϊ" + img.Width + ", ϵͳ����������Ϊ" + config.Attachimgmaxwidth;

                                    if (config.Attachimgmaxheight > 0 && img.Height > config.Attachimgmaxheight)
                                        attachmentInfo[saveFileCount].Sys_noupload = "ͼƬ�߶�Ϊ" + img.Width + ", ϵͳ��������߶�Ϊ" + config.Attachimgmaxheight;

                                    if (attachmentInfo[saveFileCount].Sys_noupload == "")
                                    {
                                        if (waterMarkStatus == 0)
                                        {
                                            //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
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
                                                //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
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

                                                //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
                                                if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                                    ForumUtils.AddImageSignText(img, UploadDir + tempFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                                else
                                                    ForumUtils.AddImageSignText(img, UploadDir + newFileName, watermarkText, config.Watermarkstatus, config.Attachimgquality, config.Watermarkfontname, config.Watermarkfontsize);
                                            }

                                            //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,���ȡ��ʱĿ¼�µ��ļ���Ϣ
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
                                    //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }
                            catch
                            {
                                //���ϴ�Ŀ¼����ʱ�ļ��ж�û���ϴ����ļ�ʱ
                                if (!(Utils.FileExists(UploadDir + tempFileName)) && (!(Utils.FileExists(UploadDir + newFileName))))
                                {
                                    attachmentInfo[saveFileCount].Filesize = fileSize;
                                    //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempFileName);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }

                            try
                            {
                                //�����ļ�Ԥ����ָ������
                                IPreview preview = PreviewProvider.GetInstance(fileExtName.Trim());
                                if (preview != null)
                                {
                                    preview.UseFTP = (FTPs.GetMallAttachInfo.Allowupload == 1) ? true : false;
                                    //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        preview.OnSaved(UploadDir + tempFileName);
                                    else
                                        preview.OnSaved(UploadDir + newFileName);
                                }
                            }
                            catch
                            { }

                            //��֧��FTP�ϴ�����ʱ,ʹ��FTP�ϴ�Զ�̸���
                            if (FTPs.GetMallAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();
                                //�����������ظ���ģʽʱ,���ϴ����֮��ɾ������tempfilename�ļ�
                                if (FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + tempFileName, FTPs.FTPUploadEnum.ForumAttach);
                                else
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + newFileName, FTPs.FTPUploadEnum.ForumAttach);
                            }
                        }
                        else
                        {
                            if (extNameId < 0)
                                attachmentInfo[saveFileCount].Sys_noupload = "�ļ���ʽ��Ч";
                            else if (MaxSizePerDay - TodayUploadedSize < fileSize)
                                attachmentInfo[saveFileCount].Sys_noupload = "�ļ����ڽ��������ϴ����ֽ���";
                            else if (fileSize > MaxSize[extNameId])
                                attachmentInfo[saveFileCount].Sys_noupload = "�ļ����ڸ����͸���������ֽ���";
                            else
                                attachmentInfo[saveFileCount].Sys_noupload = "�ļ����ڵ����ļ������ϴ����ֽ���";
                        }
                    }
                    else
                        attachmentInfo[saveFileCount].Sys_noupload = "�ļ���ʽ��Ч";

                    //��֧��FTP�ϴ�����ʱ
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
        /// �ϴ�����ļ�
        /// </summary>
        /// <param name="MaxFileSize">����ļ��ϴ��ߴ�</param>
        /// <param name="AllowFileType">�����ϴ��ļ�����</param>
        /// <param name="config">���ö�����Ϣ</param>
        /// <param name="filekey">File�ؼ���Key(��Name����)</param>
        /// <returns>�ļ���Ϣ�ṹ</returns>
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
                   
                    // �ж� �ļ���չ��/�ļ���С/�ļ����� �Ƿ����Ҫ��
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

                            //��ʱ�ļ����Ʊ���. ���ڵ�����Զ�̸���֮��,���ϴ���������ʱ�ļ��е�·����Ϣ
                            string tempfilename = "";
                            //��֧��FTP�ϴ������Ҳ��������ظ���ʱ
                            if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                            {
                                // ���ָ��Ŀ¼������������ʱ·��
                                if (!Directory.Exists(UploadDir + "temp\\"))
                                    Utils.CreateDir(UploadDir + "temp\\");

                                tempfilename = "temp\\" + newFileName;
                            }
                            else
                            {
                                // ���ָ��Ŀ¼����������
                                if (!Directory.Exists(UploadDir + savedir.ToString()))
                                    Utils.CreateDir(UploadDir + savedir.ToString());
                            }
                            newFileName = savedir.ToString() + newFileName;

                            try
                            {
                                //���ϴ�Ŀ¼����ʱ�ļ��ж�û���ϴ����ļ�ʱ
                                if (!(Utils.FileExists(UploadDir + tempfilename)) && (!(Utils.FileExists(UploadDir + newFileName))))
                                {

                                    //��֧��FTP�ϴ������Ҳ��������ظ���ģʽʱ,�����ϴ�����ʱĿ¼��
                                    if (FTPs.GetMallAttachInfo.Allowupload == 1 && FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + tempfilename);
                                    else
                                        HttpContext.Current.Request.Files[i].SaveAs(UploadDir + newFileName);
                                }
                            }
                            catch
                            {}

                            //��֧��FTP�ϴ�����ʱ,ʹ��FTP�ϴ�Զ�̸���
                            if (FTPs.GetMallAttachInfo.Allowupload == 1)
                            {
                                FTPs ftps = new FTPs();

                                //�����������ظ���ģʽʱ,���ϴ����֮��ɾ������tempfilename�ļ�
                                if (FTPs.GetMallAttachInfo.Reservelocalattach == 0)
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + tempfilename, FTPs.FTPUploadEnum.ForumAttach);
                                else
                                    ftps.UpLoadFile(newFileName.Substring(0, newFileName.LastIndexOf("\\")), UploadDir + newFileName, FTPs.FTPUploadEnum.ForumAttach);
                            }
                        }
                        else
                           return (extNameId < 0) ? "�ļ���ʽ��Ч" : "�ļ����ڵ����ļ������ϴ����ֽ���";
                    }
                    else
                        return "�ļ���ʽ��Ч";

                    //��֧��FTP�ϴ�����ʱ
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
