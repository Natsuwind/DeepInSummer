using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using PostAlbum.PostAlbumService;

namespace PostAlbum
{
    public partial class SliderShow : UserControl
    {
        /// <summary>
        /// 附件图片地址列表
        /// </summary>
        ShowtopicPageAttachmentInfo[] attachList = new ShowtopicPageAttachmentInfo[0];
        /// <summary>
        /// 当前显示图片列表位置
        /// </summary>
        private int position = 0;

        public static string ServiceUrl
        {
            get
            {
                string path = Application.Current.Host.Source.AbsoluteUri;
                path = path.Substring(0, path.IndexOf("/silverlight/PostAlbum"));
                // Application.Current.Host.Source.AbsoluteUri.Replace(path, "");
                //System.Windows.Browser.HtmlPage.Window.Alert(path + "/services/MixObjects.asmx");
                //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsolutePath);
                //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsoluteUri);
                //System.Windows.Browser.HtmlPage.Window.Alert(path + "/services/Album.asmx");
                //return path;
                return path + "/services/Album.asmx";
            }
        }

        public SliderShow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            int olid = -1, topicid = -1, forumid = -1, posterid = -1;
            if (App.GetInitParmas.ContainsKey("olid") && !string.IsNullOrEmpty(App.GetInitParmas["olid"]))
                olid = TypeConverter.StrToInt(App.GetInitParmas["olid"], -1);
            if (App.GetInitParmas.ContainsKey("topicid") && !string.IsNullOrEmpty(App.GetInitParmas["topicid"]))
                topicid = TypeConverter.StrToInt(App.GetInitParmas["topicid"], -1);
            if (App.GetInitParmas.ContainsKey("forumid") && !string.IsNullOrEmpty(App.GetInitParmas["forumid"]))
                forumid = TypeConverter.StrToInt(App.GetInitParmas["forumid"], -1);
            if (App.GetInitParmas.ContainsKey("posterid") && !string.IsNullOrEmpty(App.GetInitParmas["posterid"]))
                posterid = TypeConverter.StrToInt(App.GetInitParmas["posterid"], -1);

            string onlyauthor = "";
            if (App.GetInitParmas.ContainsKey("onlyauthor")
               && !string.IsNullOrEmpty(App.GetInitParmas["onlyauthor"]))
                onlyauthor = App.GetInitParmas["onlyauthor"];


            CredentialInfo credentialInfo = Utils.GetCredentialInfo();
            AlbumSoapClient albumsSoapClient = Utils.CreateServiceClient();
            albumsSoapClient.GetAttachListAsync(topicid, forumid, onlyauthor, posterid, credentialInfo);
            albumsSoapClient.GetAttachListCompleted += new EventHandler<GetAttachListCompletedEventArgs>(albumsSoapClient_GetAttachListCompleted);

            CurImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(bitmapImage_ImageFailed);
            //LoadInfo.Content = "正在加载数据...";
        }

        void bitmapImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            CurImage.Source = new BitmapImage(new Uri("/Images/NoPhoto.png", UriKind.Relative));
            CurImage.Width = 380;
            CurImage.Height = 250;
        }

        void albumsSoapClient_GetAttachListCompleted(object sender, GetAttachListCompletedEventArgs e)
        {
            attachList = e.Result;
            if (attachList != null && attachList.Length > 0)
            {
                BindImage(attachList[0]);

                Prev.IsEnabled = false; //加载时先让'上一张图'按钮失效
                Next.IsEnabled = attachList.Length > 1;//如果列表中有两条以上数据时，则让'下一张图'按钮有效   
            }
            else
                Prev.IsEnabled = Next.IsEnabled = false;
        }

        /// <summary>
        /// 绑定图片信息
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void SetPositionInfo()
        {
            LoadInfo.Text = (position + 1) + "/" + attachList.Length;
        }

        /// <summary>
        /// 下一张图按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            position++;
            if (attachList.Length > position)
            {
                BindImage(attachList[position]);
                Prev.IsEnabled = true;
            }

            if (attachList.Length <= (position + 1))
                Next.IsEnabled = false;
        }

        /// <summary>
        /// 上一张图按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            position--;
            if (position >= 0)
            {
                BindImage(attachList[position]);
                Next.IsEnabled = true;
            }

            if (position <= 0)
                Prev.IsEnabled = false;
        }

        /// <summary>
        /// 绑定指定URL图片
        /// </summary>
        /// <param name="imageUrl"></param>
        public void BindImage(ShowtopicPageAttachmentInfo attachinfo)
        {
            //ucGif.Visibility = Visibility.Visible;
            //CurImage.Visibility = Visibility.Collapsed;
            ////System.Net.WebClient wcashx = new System.Net.WebClient();
            ////Uri endpoint = new Uri("", UriKind.Relative);
            ////wcashx.OpenReadCompleted += new OpenReadCompletedEventHandler(wcashx_OpenReadCompleted);
            ////wcashx.OpenReadAsync(endpoint);
            //System.Windows.Resources.StreamResourceInfo sr = Application.GetResourceStream(new Uri("../Images/Loading.gif", UriKind.Relative));
            //ucGif.SetSoruce(sr.Stream); 

            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(attachinfo.Filename, UriKind.RelativeOrAbsolute));
                bitmapImage.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
                CurImage.Source = bitmapImage;
                LoadingBar.Visibility = System.Windows.Visibility.Visible;
                SetPositionInfo();
                bitmapImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(bitmapImage_ImageFailed);
        
            }
            catch { }
        }

       

        /// <summary>
        /// 下载图片进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bitmapImage_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if (e.Progress == 100)
            {
                BitmapImage bitmapImage = sender as BitmapImage;
                int height = bitmapImage.PixelHeight;
                int width = bitmapImage.PixelWidth;

                //当下载的图片有效时
                if (height > 0 && width > 0)
                {
                    if (width > bitmapImage.PixelHeight)
                        CurImage.Width = (width < CurImage.MaxWidth) ? width : CurImage.MaxWidth;
                    else
                        CurImage.Height = (height < CurImage.MaxHeight) ? height : CurImage.MaxHeight;

                    
                    CurImageFadeIn.Begin();
                }
                else //不存在或路径无效时
                {
                    CurImage.Source = new BitmapImage(new Uri("/Images/NoPhoto.png", UriKind.RelativeOrAbsolute));
                    CurImage.Width = 380;
                    CurImage.Height = 250;
                }
                LoadingBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                //LoadInfo.Content = "正在加载数据" + e.Progress + "...";
                LoadingBar.Value = e.Progress;
                LoadingBar.Visibility = System.Windows.Visibility.Visible; 
            }
        }

        private void CurImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            if (attachList.Length <= (position + 1))
            {
                Next.IsEnabled = false;
                return;
            }
            position++;
            if (attachList.Length > position)
            {
                BindImage(attachList[position]);
                Prev.IsEnabled = true;
            }
        }
    }
}
