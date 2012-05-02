using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace HaoRan.WebCam
{
    /// <summary>
    /// WebCam页
    /// </summary>
    public partial class WebCam : Page
    {
        /// <summary>
        /// 初始化视频捕捉设备
        /// </summary>
        private CaptureSource captureSource = new CaptureSource()
        {
            VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice(),
            AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice()
        };
        /// <summary>
        /// 保存文件对话框
        /// </summary>
        private SaveFileDialog saveFileDlg = new SaveFileDialog
        {
            DefaultExt = ".jpg",
            Filter = "JPEG Images (*jpeg *.jpg)|*.jpeg;*.jpg",
        };

        public WebCam()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WebCam_Loaded);
        }

        void WebCam_Loaded(object sender, RoutedEventArgs e)
        {
            BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
            BtnCapture.IsEnabled = goBack.IsEnabled = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice() != null;      
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {}
              
        private void BtnCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {   // 开始捕捉             
                if (captureSource.State != CaptureState.Started)
                {
                    captureSource.Stop();
                    // 创建 video brush 并填充到 rectangle 
                    VideoBrush vidBrush = new VideoBrush();
                    vidBrush.Stretch = Stretch.UniformToFill;
                    vidBrush.SetSource(captureSource);
                    focusRectangle.Viewport.Fill = vidBrush;
               

                    // 询问是否接入
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                    {
                        focusRectangle.Viewport.MaxHeight = focusRectangle.Viewport.MaxWidth = ZoomInOut.Maximum = 400;
                        ZoomInOut.Value = 270;
                        ZoomInOut.Minimum = 16;
                        ZoomInOut.ValueChanged += new RoutedPropertyChangedEventHandler<double>(focusRectangle.ViewportSlider_ValueChanged);
                        captureSource.Start();

                        BtnCapture.Text = "打开摄像头";
                        BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = true;
                    }                    
                }
                else
                {
                    captureSource.Stop();
                    BtnCapture.Text = "关闭摄像头";
                    BtnUploadImage.IsEnabled = BtnAdvanceMode.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Utils.ShowMessageBox("Error using webcam", ex.Message);
            }
        }

        #region 注释代码
        /// <summary>
        /// 保存到本地.
        /// </summary>
       // private void BtnSnapshot_Click(object sender, RoutedEventArgs e)
       // {
       //     if (saveFileDlg.ShowDialog().Value)
       //     {
       //         using (Stream dstStream = saveFileDlg.OpenFile())
       //         {
       //             try
       //             {
       //                 WriteableBitmap bmp = new WriteableBitmap(focusRectangle.imageScroll, null);
       //                 JpegHelper.EncodeJpeg(bmp, dstStream);
       //             }
       //             catch (Exception ex)
       //             {
       //                 Utils.ShowMessageBox("Error saving snapshot", ex.Message);
       //             }
       //         }
       //     }
        //}
        #endregion

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            #region 注释代码
            //FileCollection _files = new FileCollection("1", 1024000);
            //FrameworkElement image = focusRectangle.imageScroll;
            //FrameworkElement focus = focusRectangle.FocusRect;
            //WriteableBitmap bmp = new WriteableBitmap(image, null);
            //Stream dstStream = new MemoryStream();
            //JpegHelper.EncodeJpeg(bmp, dstStream);
            //dstStream.Position = 0;//用于上传时从新读取

            //MixObjectsSoapClient.Point point = new MixObjectsSoapClient.Point();
            //point.X = Int32.Parse(focus.GetValue(Canvas.LeftProperty).ToString());
            //point.Y = Int32.Parse(focus.GetValue(Canvas.TopProperty).ToString());

            //MixObjectsSoapClient.Size size = new MixObjectsSoapClient.Size();
            //size.Width = Int32.Parse(focus.Width.ToString());
            //size.Height = Int32.Parse(focus.Height.ToString());

            
            //UserFile imageFile = new UserFile() { FileName = userid + ".jpg", FileStream = dstStream, GrabPoint = point, GrabSize = size };
            //imageFile.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged);            
            //_files.Add(imageFile);
            //_files.UploadFiles();
            //_files.RemoveAt(0);
            #endregion

            captureSource.Stop();

            Utils.UploadUserFile(Utils.GetUserId() + ".jpg", focusRectangle.imageScroll, focusRectangle.FocusRect,
                //定制UserFile的PropertyChanged 属性，如BytesUploaded，Percentage，IsDeleted
                new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged));        
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

                captureSource.Start();
            }
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        #region 高级模式事件代码
        private void BtnAdvanceMode_Click(object sender, RoutedEventArgs e)
        {
            captureSource.Stop();

            Utils.UploadUserFile(Utils.GetUserId() + ".jpg", focusRectangle.imageScroll, focusRectangle.FocusRect,
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
                        this.NavigationService.Navigate(
                             new Uri(
                                        string.Format("/AdvanceMode?focusWidth={0}&focusHeight={1}&fileName={2}",
                                        focusRectangle.FocusRect.Width,
                                        focusRectangle.FocusRect.Height,
                                        userFile.FileName),
                                    UriKind.Relative));
                });
        }
        #endregion
    }
}
