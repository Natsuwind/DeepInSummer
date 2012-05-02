using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Browser;

namespace HaoRan.WebCam
{
    /// <summary>
    /// 图片浏览页
    /// </summary>
    public partial class ImageBrowser : Page
    {      
        /// <summary>
        /// 选择的本地图片文件信息
        /// </summary>
        FileInfo fileInfo = null;  

        public ImageBrowser()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ImageBrowser_Loaded);
        }

        void ImageBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = ZoomInOut.IsEnabled = false;
        }


        private void BtnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false, //Filter = "Jpeg Files (*.jpg)|*.jpg|All Files(*.*)|*.*",
                Filter = "JPEG Images (*.jpeg *.jpg)|*.jpeg;*.jpg|Png Images (*.png)|*.png",
            };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    LoadingInfo.Visibility = System.Windows.Visibility.Visible;
                    focusRectangle.Viewport.Visibility = System.Windows.Visibility.Collapsed;
                    focusRectangle.LoadImageStream(ofd.File.OpenRead(), ZoomInOut);
                    fileInfo = ofd.File;
                    BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
                    ZoomInOut.IsEnabled = true;
                    System.Threading.Timer timer = new System.Threading.Timer(TimerCallback,
                         new LoadingState()
                         {
                             focusRectangle = focusRectangle,
                             LoadingInfo = LoadingInfo,
                             BtnUploadImage = BtnUploadImage,
                             BtnAdvanceMode = BtnAdvanceMode
                         } as object,
                         1000, 500);   
                }
                catch (Exception exception)
                {
                    Utils.ShowMessageBox("文件无效:" + exception.Message);
                    ZoomInOut.IsEnabled = false;
                    BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
                }
            }
        }
     

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            if (fileInfo == null)
                Utils.ShowMessageBox("\r\n请选择要上传的头像.");
            else
            {
                Utils.UploadUserFile(fileInfo.Name, focusRectangle.imageScroll, focusRectangle.FocusRect,
                     //定制UserFile的PropertyChanged 属性，如BytesUploaded，Percentage，IsDeleted
                     new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged));       
            }
        }

   
        void FileRowControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UserFile userFile = sender as UserFile;
            if (e.PropertyName == "Percentage")
            {
                Percentage.Value = userFile.Percentage;
                Percentage.Visibility = Visibility.Visible;       
            }
            //当前文件上传完毕
            if (userFile.State == Constants.FileStates.Finished)
            {
                Percentage.Visibility = Visibility.Collapsed;
                CWViewUploadedImage cw = new CWViewUploadedImage();
                cw.Closed += (o, eventArgs) =>
                {
                    if (cw.DialogResult == true)//确定并就隐藏当前sl应用窗口
                        NavPage.javaScriptableObject.OnCloseAvatar(null); //调用js端注册事件
                        //Utils.ShowMessageBox("op: 确定并就隐藏当前sl应用窗口");
                };
                cw.LargeImageWidth.Text = focusRectangle.FocusRect.Width.ToString();
                cw.LargeImageHeight.Text = focusRectangle.FocusRect.Height.ToString().ToString();               
                cw.Show();
            }
        }
       
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        #region 高级模式事件代码
        private void BtnAdvanceMode_Click(object sender, RoutedEventArgs e)
        {
            if (fileInfo == null)
                Utils.ShowMessageBox("\r\n请选择要上传的头像.");
            else
            {               
                Utils.UploadUserFile(fileInfo.Name, focusRectangle.imageScroll, focusRectangle.FocusRect,
                    (o, eventArgs) => //定制UserFile的PropertyChanged 属性，如BytesUploaded,Percentage,IsDeleted
                    {
                        UserFile userFile = o as UserFile;
                        if (eventArgs.PropertyName == "Percentage")
                        {
                            Percentage.Value = userFile.Percentage;
                            Percentage.Visibility = Percentage.Value == 100 ? Visibility.Collapsed : Visibility.Visible;
                        }
                        //当前文件上传完毕
                        if (userFile.State == Constants.FileStates.Finished)
                        {
                            this.NavigationService.Navigate(
                                new Uri(
                                        string.Format("/AdvanceMode?focusWidth={0}&focusHeight={1}&fileName={2}&random=" + new Random().Next(1, 10000),
                                        focusRectangle.FocusRect.Width,
                                        focusRectangle.FocusRect.Height,
                                        userFile.FileName),
                                    UriKind.Relative));
                        }
                    });
            }            
        }
        #endregion
        
        #region FocusRectangleDrop事件
        /// <summary>
        /// 线程回调方法类
        /// </summary>
        /// <param name="state"></param>
        private static void TimerCallback(object state)
        {
            LoadingState loadingState = (LoadingState)state;
            loadingState.LoadingInfo.Dispatcher.BeginInvoke(delegate()
            {
                loadingState.LoadingInfo.Visibility = System.Windows.Visibility.Collapsed;
                loadingState.focusRectangle.Viewport.Visibility = System.Windows.Visibility.Visible;
                loadingState.BtnUploadImage.IsEnabled = loadingState.BtnAdvanceMode.IsEnabled = true;
            });
        }
        /// <summary>
        /// 图片加载信息对象
        /// </summary>
        public class LoadingState
        {
            public FocusRectangle focusRectangle;
            public StackPanel LoadingInfo;
            public ImageButton BtnUploadImage;
            public ImageButton BtnAdvanceMode;
        }

        private void focusRectangle_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                System.IO.FileInfo[] files = e.Data.GetData(DataFormats.FileDrop) as System.IO.FileInfo[];
                if (files != null && files.Count() > 0)
                {
                    System.IO.FileStream fs = files[0].OpenRead();
                    {
                        try
                        {
                            LoadingInfo.Visibility = System.Windows.Visibility.Visible;
                            focusRectangle.Viewport.Visibility = System.Windows.Visibility.Collapsed;
                            BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
                            ZoomInOut.IsEnabled = true;
                            focusRectangle.LoadImageStream(fs, ZoomInOut);                  
                            fs.Close();
                            fileInfo = files[0];                            

                            System.Threading.Timer timer = new System.Threading.Timer(TimerCallback, 
                                 new LoadingState() 
                                 { 
                                     focusRectangle = focusRectangle,
                                     LoadingInfo = LoadingInfo,
                                     BtnUploadImage = BtnUploadImage,
                                     BtnAdvanceMode = BtnAdvanceMode
                                 } as object,
                                 1000, 500);                                    
                        } 
                        catch (Exception exception)
                        {
                            Utils.ShowMessageBox("文件无效:" + exception.Message);
                            ZoomInOut.IsEnabled = false;
                            BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
