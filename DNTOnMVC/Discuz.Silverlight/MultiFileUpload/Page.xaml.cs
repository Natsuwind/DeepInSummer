using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MultiFileUpload.Classes;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Browser;
using System.Xml;
using System.Runtime.Serialization.Json;
using MultiFileUpload.UploadServiceAsmx;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace MultiFileUpload
{
    public partial class Page : UserControl
    {
        /// <summary>
        /// 上传文件的最大尺寸
        /// </summary>
        private int _maxFileSize = int.MaxValue;

        private FileCollection _files;
        private int _maxUpload = 2;       
        private string _customParams;
        private string _fileFilter;
        /// <summary>
        /// 今天可上传的大小
        /// </summary>
        private long _todayAttachSize;
        /// <summary>
        /// 今天已上传文件大小之和
        /// </summary>
        private double _todayUploadSize;
        /// <summary>
        /// 当前上传列表中要上传的文件大小之和
        /// </summary>
        private double _wantUploadSize;
        /// <summary>
        /// 当前版块id （此参数在创建上传插件时在initparam中提供）
        /// </summary>
        private int _forumid;
        /// <summary>
        /// 已上传的附件列表
        /// </summary>
        public static List<AttachmentInfo> AttachmentList = new List<AttachmentInfo>();
        /// <summary>
        /// 最大允许上传的附件数
        /// </summary>
        private int _maxAttachments;
        /// <summary>
        /// 页面文档变量
        /// </summary>
        public static HtmlDocument _htmlDocument = HtmlPage.Document;

        public static string ServiceUrl
        {
           get
           {
               string path = Application.Current.Host.Source.AbsoluteUri;
               path =path.Substring(0, path.IndexOf("/silverlight/UploadFile"));
               // Application.Current.Host.Source.AbsoluteUri.Replace(path, "");
               //System.Windows.Browser.HtmlPage.Window.Alert(path + "/services/MixObjects.asmx");
               //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsolutePath);
               //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsoluteUri);
               return path + "/services/MixObjects.asmx";
           }
        }

        public Page()
        {            
            InitializeComponent();

            //加载上传配置信息
            LoadConfiguration(App.GetInitParmas);

            _files = new FileCollection(_customParams, _maxUpload);
            FileList.ItemsSource = _files;
            FilesCount.DataContext = _files;
            TotalProgress.DataContext = _files;
            TotalKB.DataContext = _files;

            HtmlPage.RegisterScriptableObject("MultiFileUpload", this);
            HtmlPage.RegisterScriptableObject("JavaScriptObject", javaScriptableObject);
            //HtmlPage.Window.Alert(ServiceUrl);
        }


        void MouseEnterFile(object sender, FileMouseEventArgs e)
        {
            //只对下面的图标文件类型生成缩略图
            if ((".jpg.jpeg.png.bmp").IndexOf(Utils.CutString(e.FileInfo.FileName, e.FileInfo.FileName.LastIndexOf(".") + 1).ToLower()) >= 0)
            {
                BitmapImage bitimage = new BitmapImage();
                try
                {
                    bitimage.SetSource(e.FileInfo.ViewStream);
                    ThumbnailImage.Source = bitimage;
                    ThumbnailImage.Visibility = Visibility.Visible;
                    Canvas.SetLeft(ThumbnailImageBorder, e.X + 50);
                    Canvas.SetTop(ThumbnailImageBorder, e.Y - 30);
                    object o = ThumbnailImage.GetValue(Canvas.ActualHeightProperty);
                    ThumbnailImageBorder.Height = ThumbnailImage.Height + 10;
                    expandImage.Begin();
                }
                catch{ }
            }
        }

        void MouseLeaveFile(object sender, FileMouseEventArgs e)
        {
            ThumbnailImage.Source = null;
            ThumbnailImage.Visibility = Visibility.Collapsed;
        }

        void CancelUploadFile(object sender, UploadArgs e)
        {
            _wantUploadSize -= e.FileInfo.FileSize;
            filecount--;
        }

        void UploadFileFish(object sender, UploadArgs e)
        {
            _todayUploadSize += e.FileInfo.FileSize;
            _wantUploadSize -= e.FileInfo.FileSize;
            OtherMessage.Text = "已上传大小:" + Math.Round((decimal)_todayUploadSize / 1024 / 1024, 2) + "MB";
        }
        
        void _client_GetAttachmentUploadSetCompleted(object sender, GetAttachmentUploadSetCompletedEventArgs e)
        {
            try
            {
                UploadSetInfo uploadsetinfo = e.Result;

                if (!string.IsNullOrEmpty(uploadsetinfo.ErrMessage))
                {
                    ShowMessageBox("\r\n" + uploadsetinfo.ErrMessage);
                    return;
                }

                if (!uploadsetinfo.CanPostAttach || uploadsetinfo.AttachSize <= 0)
                {
                    SetUploadButton(false);
                    return;
                }

                //设置最大上传参数
                _maxFileSize = uploadsetinfo.MaxAttachSize;
                _todayAttachSize = uploadsetinfo.AttachSize;
                _maxAttachments = uploadsetinfo.Maxattachments < _maxAttachments ? uploadsetinfo.Maxattachments : _maxAttachments;
                string allAllowExtname = "";
                //设置选择框参数
                foreach (string extname in uploadsetinfo.AttachExtensionsNoSize.ToLower().Split(','))
                {
                    if (string.IsNullOrEmpty(_fileFilter))
                    {
                        _fileFilter += extname + "Files (*." + extname + ")|*." + extname;
                        allAllowExtname += "*." + extname ;
                    }
                    else
                    {
                        _fileFilter += "|" + extname + "Files (*." + extname + ")|*." + extname;
                       allAllowExtname += ";*." + extname ;
                    }
                }
                _fileFilter = "所有格式(" + allAllowExtname + ")|" + allAllowExtname + "|" + _fileFilter;

                Message.Text = "单个附件大小:" + Math.Round((decimal)uploadsetinfo.MaxAttachSize / 1024 / 1024, 2) + "MB\r\n";
                Message.Text +="今天可上传大小:" + Math.Round((decimal)uploadsetinfo.AttachSize / 1024 / 1024, 2) + "MB\r\n";
                Message.Text +="附件类型: " + uploadsetinfo.AttachExtensionsNoSize;
                OtherMessage.Text = "已上传大小:" + Math.Round((decimal)_todayUploadSize / 1024 / 1024, 2) + "MB";

                SetUploadButton(true);
            }
            catch (Exception ex)
            {
                ShowMessageBox("\r\n错误的文件过滤配置: " + ex.Message);
            }
            finally{}
        }

      

        /// <summary>
        /// 加载配置参数
        /// </summary>
        /// <param name="initParams"></param>
        private void LoadConfiguration(IDictionary<string, string> initParams)
        {
            string tryTest = string.Empty;

            //加载定制配置信息串
            _customParams = initParams["forumid"];

            if (initParams.ContainsKey("MaxUploads") && !string.IsNullOrEmpty(initParams["MaxUploads"]))
                int.TryParse(initParams["MaxUploads"], out _maxUpload);            

            if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
            {
                if (int.TryParse(initParams["MaxFileSizeKB"], out _maxFileSize))
                    _maxFileSize = _maxFileSize * 1024;
            }

            if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
                _fileFilter = initParams["FileFilter"];          

            if (initParams.ContainsKey("forumid") && !string.IsNullOrEmpty(initParams["forumid"]))
                _forumid = Utils.StrToInt(initParams["forumid"], 0);

            if (initParams.ContainsKey("max") && !string.IsNullOrEmpty(initParams["max"]))
                _maxAttachments = Utils.StrToInt(initParams["max"], 0);

            CredentialInfo _creInfo= Utils.GetCredentialInfo();
            if (_creInfo.UserID <= 0)
            {
                ShowMessageBox("您未登陆系统");
                SetUploadButton(false);
                return;
            }
            else
            {
                MixObjectsSoapClient _client = Utils.CreateServiceClient();
                _client.GetAttachmentUploadSetCompleted += new EventHandler<GetAttachmentUploadSetCompletedEventArgs>(_client_GetAttachmentUploadSetCompleted);
                _client.GetAttachmentUploadSetAsync(_creInfo, _forumid);
            }
        }

        private void SetUploadButton(bool setValue)
        {
            SelectFilesButton.IsEnabled = setValue;
            UploadButton.IsEnabled = setValue;
            ClearButton.IsEnabled = setValue;
            FileList.IsEnabled = setValue;
        }

        /// <summary>
        /// 上传的文件数（包括已上传和被加载到上传列表中进行上传的文件数之和）
        /// </summary>
        public int filecount = 0;

        /// <summary>
        /// 选择文件对话框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {

            if (AttachmentList.Count >= _maxAttachments)
            {
                ShowMessageBox("\r\n您上传的文件数已达到系统规定的上限: " + _maxAttachments + ".");
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            try
            {
                if(!string.IsNullOrEmpty(_fileFilter))
                    ofd.Filter = _fileFilter;
            }
            catch(ArgumentException ex)
            {
                ShowMessageBox("错误的文件过滤配置: " + ex.Message);
            }

            if (ofd.ShowDialog() == true)
            {
                if (filecount == 0)
                    filecount = AttachmentList.Count;

                foreach (FileInfo file in ofd.Files)
                {
                    if ((filecount + 1) > _maxAttachments)
                    {
                        ShowMessageBox("\r\n您上传的文件数已达到系统规定的上限: " + _maxAttachments + ".");
                        return;
                    }
                    filecount++;

                    string fileName = file.Name;
                    UserFile userFile = new UserFile();
                    userFile.FileName = file.Name;
                    userFile.FileStream = file.OpenRead();
                    userFile.ViewStream = file.OpenRead();

                    //总上传值在规定范围内时
                    if (_todayAttachSize < (_todayUploadSize + _wantUploadSize + userFile.FileStream.Length))
                    {
                        ShowMessageBox("\r\n当前附件大小: " + Math.Round((decimal)userFile.FileStream.Length / 1024 / 1024, 2) + " MB, 而今天还可以上传:" + Math.Round((decimal)(_todayAttachSize - _todayUploadSize) / 1024 / 1024, 2) + " MB.");
                        break;
                    }
                    //当单个文件大小大于最大上传附件尺寸时
                    if (userFile.FileStream.Length > _maxFileSize)
                    {
                        ShowMessageBox("\r\n当前附件大小: " + Math.Round((decimal)userFile.FileStream.Length / 1024 / 1024, 2) + " MB, 而单个附件允许最大尺寸为: " + Math.Round((decimal)_maxFileSize / 1024 / 1024, 2) + " MB.\r\n");
                        break;
                    }

                    //只对下面的图标文件类型生成缩略图
                    if ((".jpg.jpeg.png.bmp").IndexOf(Utils.CutString(userFile.FileName, userFile.FileName.LastIndexOf(".") + 1).ToLower()) >= 0)
                    {
                        BitmapImage bitimage = new BitmapImage();
                        try
                        {
                            bitimage.SetSource(userFile.ViewStream);
                            //ThumbnailImage.Source = bitimage;
                            //ThumbnailImage.Visibility = Visibility.Visible;
                            //Canvas.SetLeft(ThumbnailImageBorder, e.X + 50);
                            //Canvas.SetTop(ThumbnailImageBorder, e.Y - 30);
                            //object o = ThumbnailImage.GetValue(Canvas.ActualHeightProperty);
                            //ThumbnailImageBorder.Height = ThumbnailImage.Height + 10;
                            //expandImage.Begin();
                            _files.Add(userFile);
                            _wantUploadSize += userFile.FileStream.Length;  
                        }
                        catch 
                        {
                            ShowMessageBox("\r\n上传图片信息错误，请检查!");
                            break;
                        }
                    }
                    else
                    {
                        //向文件列表中添加文件信息
                        _files.Add(userFile);
                        _wantUploadSize += userFile.FileStream.Length;  
                    }
                    
                }
            }
        }       

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_files.Count == 0)
                ShowMessageBox("\r\n请选择要上传的文件.");
            else
                _files.UploadFiles();
        }


        private void ShowMessageBox(string context)
        {
            MessageBoxControl.Message = context;
            MessageBoxControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 清空上传文件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {          
            _files.Clear();
            _wantUploadSize = 0;            
        }

        [ScriptableMember]
        public void GetAttachmentList()
        {
            StringBuilder sb_attachments = new StringBuilder();
            sb_attachments.Append("[");
            foreach (AttachmentInfo attachmentInfo in AttachmentList)
            {
                sb_attachments.Append(string.Format("{{'aid' : {0}, 'attachment' : '{1}', 'description' : '{2}',  'filename' : '{3}', 'filesize' :{4}, 'filetype' : '{5}', 'Uid' : {6}}},",
                    attachmentInfo.Aid,
                    attachmentInfo.Attachment,
                    attachmentInfo.Description.Trim(),
                    attachmentInfo.Filename.Trim(),
                    attachmentInfo.Filesize,
                    attachmentInfo.Filetype,
                    attachmentInfo.Uid
                    ));
            }
            if (sb_attachments.ToString().EndsWith(","))
                sb_attachments.Remove(sb_attachments.Length - 1, 1);

            sb_attachments.Append("]");
            //System.Windows.Browser.HtmlPage.Window.Alert("/services/MixObjects.asmx");

            //调用js端注册事件
            javaScriptableObject.OnUploadAttchmentList(JsonCharFilter(sb_attachments.ToString()));
        }

        /// <summary>
        /// 返回按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishUpload_Click(object sender, RoutedEventArgs e)
        {
            GetAttachmentList();
        }

        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns></returns>
        public string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }

        JavaScriptableObject javaScriptableObject = new JavaScriptableObject();

        /// <summary>
        /// 雇员事件参数（用于完成与js绑定事件参数）
        /// </summary>
        [ScriptableType]
        public class AttachmentListEventArgs : EventArgs
        {
            [ScriptableMember]
            public string AttchmentList { get; set; }             
        }

        /// <summary>
        /// 要注册并在页面中使用的js调用脚本对象
        /// </summary>
        [ScriptableType]
        public class JavaScriptableObject
        {
            /// <summary>
            /// js捆绑的事件处理器
            /// </summary>
            [ScriptableMember]
            public event EventHandler<AttachmentListEventArgs> UploadAttchmentList;

            public void OnUploadAttchmentList(string attchmentList)
            {
                if (UploadAttchmentList != null)
                {
                    UploadAttchmentList(this, new AttachmentListEventArgs()
                    {
                        AttchmentList = attchmentList
                    });
                }
            }
        }     
    }
}
