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
using System.Windows.Controls.Primitives;
using System.Windows.Printing;

using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace HaoRan.WebCam
{
    /// <summary>
    /// 浏览上传头像子窗口
    /// </summary>
    public partial class CWViewUploadedImage : ChildWindow
    {
        /// <summary>
        /// Focus区域宽度
        /// </summary>
        double FocusWidth = 0;
        /// <summary>
        /// Focus区域高度
        /// </summary>
        double FocusHeight = 0;

        public CWViewUploadedImage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(CWViewUploadedImage_Loaded);
        }

        void CWViewUploadedImage_Loaded(object sender, RoutedEventArgs e)
        {
            double.TryParse(LargeImageWidth.Text, out FocusWidth);
            double.TryParse(LargeImageHeight.Text, out FocusHeight);

            if (FocusWidth > FocusHeight)
            {
                LargeImage.Width = 120;
                MediumImage.Width = 100;
                SmallImage.Width = 80;
            }
            else
            {
                LargeImage.Height = 120;
                MediumImage.Height = 100;
                SmallImage.Height = 80;
            }

            ObservableCollection<BitmapImage> previewImage = Utils.GetPreviewImage(Utils.ServiceUrl, Utils.GetUserId());
            // Utils.ShowMessageBox(previewImage[0].UriSource.AbsolutePath + " " + previewImage[0].UriSource.AbsoluteUri);
            LargeImage.Source = previewImage[0];
            MediumImage.Source = previewImage[1];
            SmallImage.Source = previewImage[2];
            PrintImage.Source = previewImage[0];

            this.MouseLeftButtonDown += new MouseButtonEventHandler(CW_MouseLeftButtonDown);
            this.LargeImage.MouseLeftButtonDown += new MouseButtonEventHandler(CW_MouseLeftButtonDown);
            this.MediumImage.MouseLeftButtonDown += new MouseButtonEventHandler(CW_MouseLeftButtonDown);
            this.SmallImage.MouseLeftButtonDown += new MouseButtonEventHandler(CW_MouseLeftButtonDown);

            InitPopMenu();
        }

        /// <summary>
        /// 加载菜单项
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="imageUrl">图片</param>
        /// <param name="eventHandler">处理事件</param>
        /// <returns></returns>
        Grid AddMenuItem(string menuName, string imageUrl, RoutedEventHandler eventHandler)
        {
            Grid grid = new Grid();// { Margin = new Thickness(1) };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromArgb(255, 233, 238, 238)) });
            grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromArgb(255, 226, 228, 231)), HorizontalAlignment = HorizontalAlignment.Right, Width = 1 });

            Button roButton = new Button()
            {
                Height = 22,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Style = Application.Current.Resources["ContextMenuButton"] as Style
            };
            roButton.Click += eventHandler;

            Grid.SetColumnSpan(roButton, 2);
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            Image roImage = new Image() { HorizontalAlignment = HorizontalAlignment.Left, Width = 16, Height = 16, Margin = new Thickness(1, 0, 0, 0) };
            roImage.Source = new BitmapImage(new Uri("/HaoRan.WebCam;component/" + imageUrl, UriKind.RelativeOrAbsolute));
            sp.Children.Add(roImage);
            sp.Children.Add(new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(16, 0, 0, 0), Text = menuName });

            roButton.Content = sp;
            grid.Children.Add(roButton);
            return grid;
        }

        /// <summary>
        /// 初始化右键菜单
        /// </summary>
        void InitPopMenu()
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 167, 171, 176)),
                CornerRadius = new CornerRadius(2),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.White),
                Effect = new DropShadowEffect() { BlurRadius = 3, Color = Color.FromArgb(255, 230, 227, 236) }
            };

            StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Vertical };
            stackPanel.Children.Insert(0, AddMenuItem("打印头像", "images/print.png", PrintButton_Click));
            stackPanel.Children.Insert(1, AddMenuItem("保存到本地", "images/save.png", DownLoadAvatar_Click));

            border.Child = stackPanel;
            popMenu.Child = border;
        }

        #region 隐藏Popup菜单
        /// <summary>
        /// 隐藏Popup菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CW_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popMenu.IsOpen = false;
        }
        #endregion


        #region 返回事件
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion


        #region 打印代码
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument doc = new PrintDocument();
            //doc.StartPrint += new EventHandler<StartPrintEventArgs>(doc_StartPrint);
            doc.EndPrint += OnEndPrint;
            doc.PrintPage += new EventHandler<PrintPageEventArgs>(doc_PrintPage);
            doc.Print("打印头像");
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintImage.Height = FocusHeight;
            PrintImage.Width = FocusWidth;
            if (popMenu.Tag.ToString() == "MediumImageScrollViewer")
            {
                PrintImage.Height *= 0.8;
                PrintImage.Width *= 0.8;
            }
            else if (popMenu.Tag.ToString() == "SmallImageScrollViewer")
            {
                PrintImage.Height *= 0.6;
                PrintImage.Width *= 0.6;
            }
     
            ImageInf.Text = "头像类型:" + popMenu.Tag.ToString().Replace("ScrollViewer", "") + "  宽:" + PrintImage.Width + "px  高:" + PrintImage.Height + "px";
            AppInf.Text = "Product Details: HaoRan.WebCam Beta2";
            PrintArea.Width = e.PrintableArea.Width;
            PrintArea.Height = e.PrintableArea.Height;
           
            e.PageVisual = PrintArea;
            // 指定是否再次调用另一个页
            e.HasMorePages = false;
        }

        Action<Exception> completedCallback = (ex) =>
        {
            if (ex != null)
            {
                Utils.ShowMessageBox("打印错误", ex.Message);
            }
        };

        void OnEndPrint(object sender, EndPrintEventArgs e)
        {
            if (completedCallback != null)
            {
                completedCallback(e.Error);
            }
        }


        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region 右键菜单代码
        private void ScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            int offSetX = 1;
            if (element.Name == "MediumImageScrollViewer")
                offSetX = 150;
            else if (element.Name == "SmallImageScrollViewer")
                offSetX = 279;

            popMenu.HorizontalOffset = (double)element.GetValue(Canvas.LeftProperty) + e.GetPosition(element).X + offSetX;
            popMenu.VerticalOffset = (double)element.GetValue(Canvas.TopProperty) + e.GetPosition(element).Y;
            popMenu.IsOpen = true;
            popMenu.Tag = (sender as FrameworkElement).Name;
            e.Handled = true;//使用sl自带的右键屏蔽
        }
        #endregion


        #region 保存到本地操作
        /// <summary>
        /// 保存文件对话框
        /// </summary>
        private SaveFileDialog saveFileDlg = new SaveFileDialog
        {
            DefaultExt = ".jpg",
            Filter = "JPEG Images (*jpeg *.jpg)|*.jpeg;*.jpg",
        };

        /// <summary>
        /// 保存到本地
        /// </summary>
        private void DownLoadAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (saveFileDlg.ShowDialog().Value)
            {
                using (Stream dstStream = saveFileDlg.OpenFile())
                {
                    try
                    {
                        Image image;
                        double Size = FocusWidth > FocusHeight ? FocusWidth : FocusHeight;//hack:将高宽转为size，这样就可以将ui元素中的内容保存到本地了

                        if (popMenu.Tag.ToString() == "LargeImageScrollViewer")
                            image = new Image() { Width = Size, Height = Size, Source = LargeImage.Source };
                        else if (popMenu.Tag.ToString() == "MediumImageScrollViewer")
                            image = new Image() { Width = Size * 0.8, Height = Size * 0.6, Source = MediumImage.Source };
                        else
                            image = new Image() { Width = Size * 0.8, Height = Size * 0.6, Source = SmallImage.Source };

                        WriteableBitmap bmp = new WriteableBitmap(image, null);
                        JpegHelper.EncodeJpeg(bmp, dstStream);
                    }
                    catch (Exception ex)
                    {
                        Utils.ShowMessageBox("Error saving snapshot", ex.Message);
                    }
                }
            }
        }

        #endregion

    }
}

