using System;
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
using System.Windows.Media.Imaging;
using System.IO;

namespace HaoRan.WebCam
{
    #region 方形中八个点的相对位置
    /// <summary>
    /// 方形中八个点的相对位置
    /// </summary>
    enum HitDownSquare
    {
        HDS_NONE = 0,
        /// <summary>
        /// 顶
        /// </summary>
        HDS_TOP = 1,
        /// <summary>
        /// 右
        /// </summary>
        HDS_RIGHT = 2,
        /// <summary>
        /// 底
        /// </summary>
        HDS_BOTTOM = 3,
        /// <summary>
        /// 左
        /// </summary>
        HDS_LEFT = 4,
        /// <summary>
        /// 左上
        /// </summary>
        HDS_TOPLEFT = 5,
        /// <summary>
        /// 右上
        /// </summary>
        HDS_TOPRIGHT = 6,
        /// <summary>
        /// 左下
        /// </summary>
        HDS_BOTTOMLEFT = 7,
        /// <summary>
        /// 右下
        /// </summary>
        HDS_BOTTOMRIGHT = 8
    }
    #endregion

    public partial class FocusRectangle : UserControl
    {
        #region 属性设置
        /// <summary>
        /// 8个允许调整控件大小的小正方形
        /// </summary>
        Rectangle[] SmallRect = new Rectangle[8];
        /// <summary>
        /// 8个小正方形的大小
        /// </summary>
        Size Square = new Size(6, 6);
        /// <summary>
        /// 上一次鼠标点击的位置
        /// </summary>
        Point prevLeftClick;
        /// <summary>
        /// 8个小正方形的填充色
        /// </summary>
        Color color = Colors.White;
        /// <summary>
        /// 标识鼠标左键已被按下并且已开始移动
        /// </summary>
        bool trackingMouseMove = false;
        /// <summary>
        /// 当前鼠标点击位置信息
        /// </summary>
        HitDownSquare CurrHitPlace = new HitDownSquare();
        #endregion

        public FocusRectangle()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FocusRectangle_Loaded);
        }
            
        #region 初始化相应元素信息
        /// <summary>
        /// 初始化相应元素信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FocusRectangle_Loaded(object sender, RoutedEventArgs e)
        {  
            Viewport.MinHeight = Viewport.MinWidth = 16;
            Viewport.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + (ViewportHost.Height - Viewport.Height) / 2);
            Viewport.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + (ViewportHost.Width - Viewport.Width) / 2);

            //初始设置FocusRect
            FocusRect.Width = FocusRect.Height = 100;
            FocusRect.MaxWidth = ViewportHost.Width;
            FocusRect.MaxHeight = ViewportHost.Height;
            FocusRect.MinHeight = FocusRect.MinWidth = 8;
            FocusRect.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + (ViewportHost.Height - FocusRect.Height) / 2);
            FocusRect.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + (ViewportHost.Width - FocusRect.Width) / 2);

            #region 8个小正方形位置
            //左上
            SmallRect[0] = new Rectangle() { Name = "SmallRect0", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //上中间        
            SmallRect[4] = new Rectangle() { Name = "SmallRect4", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //右上
            SmallRect[1] = new Rectangle() { Name = "SmallRect1", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //左下
            SmallRect[2] = new Rectangle() { Name = "SmallRect2", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //下中间
            SmallRect[5] = new Rectangle() { Name = "SmallRect5", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //右下
            SmallRect[3] = new Rectangle() { Name = "SmallRect3", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //左中间
            SmallRect[6] = new Rectangle() { Name = "SmallRect6", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };
            //右中间
            SmallRect[7] = new Rectangle() { Name = "SmallRect7", Width = Square.Width, Height = Square.Height, Fill = new SolidColorBrush(color) };

            SetRectangles();
            #endregion                       

            #region 事件绑定
            foreach (Rectangle smallRectangle in SmallRect)
            {
                smallRectangle.Fill = new SolidColorBrush(color);
                smallRectangle.MouseMove += new MouseEventHandler(smallRectangle_MouseMove);
                smallRectangle.MouseLeftButtonUp += new MouseButtonEventHandler(smallRectangle_MouseLeftButtonUp);
                smallRectangle.MouseLeftButtonDown += new MouseButtonEventHandler(smallRectangle_MouseLeftButtonDown);
                smallRectangle.MouseEnter += new MouseEventHandler(smallRectangle_MouseEnter);
                LayoutRoot.Children.Add(smallRectangle);
            }
            FocusRect.MouseMove += new MouseEventHandler(FocusRect_MouseMove);
            FocusRect.MouseLeftButtonDown += new MouseButtonEventHandler(FocusRect_MouseLeftButtonDown);
            FocusRect.MouseLeftButtonUp += new MouseButtonEventHandler(FocusRect_MouseLeftButtonUp);
            FocusRect.MouseEnter += new MouseEventHandler(FocusRect_MouseEnter);
            FocusRect.MouseLeave += new MouseEventHandler(FocusRect_MouseLeave);
            #endregion
        }  
        #endregion
   

        #region FocusRect鼠标事件

        void FocusRect_MouseLeave(object sender, MouseEventArgs e)
        {
            customCursors.Visibility = System.Windows.Visibility.Collapsed;
        }

        void FocusRect_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            element.Cursor = Cursors.None;
            customCursors.Visibility = System.Windows.Visibility.Visible;
            customCursors.SetPostion(e.GetPosition(LayoutRoot));       
        }

        void FocusRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();
            prevLeftClick.X = prevLeftClick.Y = 0;
            element.Cursor =  Cursors.None;
  
            if (Viewport.Width < FocusRect.Width)
                FocusRect.Width = Viewport.Width;
            if (Viewport.Height < FocusRect.Height)
                FocusRect.Height = Viewport.Height;

            AssureFocusRectMoveInZone(element.Name);
            SetRectangles();
        }

        void FocusRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            prevLeftClick = e.GetPosition(element);
            trackingMouseMove = true;
            if (null != element)
            {
                element.CaptureMouse();
                element.Cursor = Cursors.None;
            }
        }

        /// <summary>
        /// 计算并设置Scroll的偏移量
        /// </summary>
        /// <param name="offSetX">鼠标X轴移动的偏移量</param>
        /// <param name="offSetY">鼠标Y轴移动的偏移量</param>
        void ComputeScrollOffSet(double offSetX, double offSetY)
        {
            double FocusRectTop = (double)FocusRect.GetValue(Canvas.TopProperty);
            double FocusRectLeft = (double)FocusRect.GetValue(Canvas.LeftProperty);
            double ViewportHostTop = (double)ViewportHost.GetValue(Canvas.TopProperty);
            double ViewportHostLeft = (double)ViewportHost.GetValue(Canvas.LeftProperty);
            //msgBox.Text = "FocusRect.Height" + FocusRect.Height + "   ViewportHost.Height" + ViewportHost.Height;
            if (offSetY > 0 && (FocusRect.Height + 8) < ViewportHost.Height && (ViewportHostTop + ViewportHost.Height) > (FocusRectTop + FocusRect.Height)) //鼠标向下且未出ViewportHost区域时
                imageScroll.ScrollToVerticalOffset(imageScroll.VerticalOffset + (offSetY / 2) * (Viewport.Height / (ViewportHost.Height - FocusRectTop - FocusRect.Height)));

            if (offSetY < 0 && (FocusRect.Height + 8) < ViewportHost.Height && ViewportHostTop < FocusRectTop) //鼠标向上且未出ViewportHost区域时
                imageScroll.ScrollToVerticalOffset(imageScroll.VerticalOffset + (offSetY / 2) * ((Viewport.Height / FocusRectTop)));

            if (offSetX > 0 && (FocusRect.Width + 8) < ViewportHost.Width && (ViewportHostLeft + ViewportHost.Width) > (FocusRectLeft + FocusRect.Width)) //鼠标向右且未出ViewportHost区域时
                imageScroll.ScrollToHorizontalOffset(imageScroll.HorizontalOffset + (offSetX /2) * (Viewport.Width / (ViewportHost.Width - FocusRectLeft - FocusRect.Width)));

            if (offSetX < 0 && (FocusRect.Width + 8) < ViewportHost.Width && ViewportHostLeft < FocusRectLeft) //鼠标向左且未出ViewportHost区域时
                imageScroll.ScrollToHorizontalOffset(imageScroll.HorizontalOffset + (offSetX/2) * ((Viewport.Width / FocusRectLeft)));
        }

         /// <summary>
        /// FocusRect鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FocusRect_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
                element.Cursor = Cursors.None;

            if (trackingMouseMove)
            {
                double offSetX = e.GetPosition(element).X - prevLeftClick.X;
                double offSetY = e.GetPosition(element).Y - prevLeftClick.Y;
         
                if (((double)element.GetValue(Canvas.TopProperty) + offSetY) >=4 && (((double)FocusRect.GetValue(Canvas.TopProperty) + FocusRect.Height) + offSetY + 3) <= ViewportHost.Height)
                    element.SetValue(Canvas.TopProperty, (double)element.GetValue(Canvas.TopProperty) + offSetY);

                if (((double)element.GetValue(Canvas.LeftProperty) + offSetX) >=4 && (((double)FocusRect.GetValue(Canvas.LeftProperty) + FocusRect.Width) + offSetX + 3) <= ViewportHost.Width)
                    element.SetValue(Canvas.LeftProperty, (double)element.GetValue(Canvas.LeftProperty) + offSetX);

                ComputeScrollOffSet(offSetX, offSetY);
                SetRectangles();
            }
            customCursors.SetPostion(e.GetPosition(LayoutRoot));         
        }
        #endregion


        #region 确保FocusRect在Viewport中进行移动和缩放
        /// <summary>
        /// 确保FocusRect在Viewport中进行缩放
        /// </summary>
        public void AssureFocusRectZoomInZone(double zoom, double mininum)
        {
            double ViewPortTop = (double)Viewport.GetValue(Canvas.TopProperty);
            double ViewPortLeft = (double)Viewport.GetValue(Canvas.LeftProperty);
            double FocusRectTop = (double)FocusRect.GetValue(Canvas.TopProperty);
            double FocusRectLeft = (double)FocusRect.GetValue(Canvas.LeftProperty);

            if (zoom == mininum)
            {
                FocusRect.SetValue(Canvas.LeftProperty, ViewPortLeft);
                FocusRect.Width = Viewport.Width;
            }
            else
            {
                //确保顶部不越界
                if (ViewPortTop > FocusRectTop)
                    FocusRect.SetValue(Canvas.TopProperty, ViewPortTop);

                //确保左侧不越界
                if (ViewPortLeft > FocusRectLeft)
                    FocusRect.SetValue(Canvas.LeftProperty, ViewPortLeft);

                //判断x是否右侧越界
                if ((Viewport.Width + ViewPortLeft) < (FocusRect.Width + FocusRectLeft))
                {
                    //如果已越界，但左侧未越界
                    if (Viewport.Width > FocusRect.Width)
                        FocusRect.SetValue(Canvas.LeftProperty, ViewPortLeft + Viewport.Width - FocusRect.Width);
                    else
                        FocusRect.Width = Viewport.Width;
                }

                //判断是否底部越界
                if ((Viewport.Height + ViewPortTop) < (FocusRect.Height + FocusRectTop))
                {
                    //如果已越界，但顶部未越界
                    if (Viewport.Height > FocusRect.Height)
                        FocusRect.SetValue(Canvas.TopProperty, ViewPortTop + Viewport.Height - FocusRect.Height);
                    else
                        FocusRect.Height = Viewport.Height;
                }
            }
            SetRectangles();
        }
       
        /// <summary>
        /// FocusRect是否在Viewport中，如不在，则确保其不超出Viewport区域
        /// </summary>
        /// <returns></returns>
        bool AssureFocusRectMoveInZone(string elementName)
        {
            bool result = true;
            //try
            //{
                double ViewPortTop = (double)Viewport.GetValue(Canvas.TopProperty);
                double ViewPortLeft = (double)Viewport.GetValue(Canvas.LeftProperty);
                double FocusRectTop = (double)FocusRect.GetValue(Canvas.TopProperty);
                double FocusRectLeft = (double)FocusRect.GetValue(Canvas.LeftProperty);
            
                if (Viewport.Height > ViewportHost.Height)//已使用放大功能,向上拖动
                {
                    if (0 > FocusRectTop)
                    {
                        FocusRect.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + 4);
                        result = false;
                    }
                }
                else
                {
                    if (ViewPortTop > FocusRectTop)
                    {
                        FocusRect.SetValue(Canvas.TopProperty, ViewPortTop);
                        result = false;
                    }
                }

                if (Viewport.Width >= ViewportHost.Width)//已使用放大功能,向左拖动
                {
                    if (0 > FocusRectLeft)
                    {
                        FocusRect.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + 4);
                        result = false;
                    }
                }
                else
                {
                    if (ViewPortLeft > FocusRectLeft)
                    {
                        FocusRect.SetValue(Canvas.LeftProperty, ViewPortLeft);
                        result = false;
                    }
                }

                if (Viewport.Width >= ViewportHost.Width)//已使用放大功能,向右拖动
                {
                    if ((ViewportHost.Width) < (FocusRect.Width + FocusRectLeft))
                    {
                        if (elementName == "FocusRect")
                            FocusRect.SetValue(Canvas.LeftProperty, ViewportHost.Width - FocusRect.Width - 4);
                        else
                        {
                            FocusRect.Width = ViewportHost.Width - FocusRectLeft - 4;
                        }
                        result = false;
                    }
                }
                else
                {
                    if ((Viewport.Width + ViewPortLeft) < (FocusRect.Width + FocusRectLeft))
                    {
                        if (elementName == "FocusRect")
                            FocusRect.SetValue(Canvas.LeftProperty, ViewPortLeft + Viewport.Width - FocusRect.Width);
                        else
                            FocusRect.Width = ViewPortLeft + Viewport.Width - FocusRectLeft;
                        result = false;
                    }
                }

                if (Viewport.Height > ViewportHost.Height)//已使用放大功能,向下拖动
                {
                    if ((ViewportHost.Height) < (FocusRect.Height + FocusRectTop))
                    {
                        if (elementName == "FocusRect")
                            FocusRect.SetValue(Canvas.TopProperty, ViewportHost.Height - FocusRect.Height - 4);
                        else
                            FocusRect.Height = ViewportHost.Height - FocusRectTop - 4;
                        result = false;
                    }
                }
                else
                {
                    if ((Viewport.Height + ViewPortTop) < (FocusRect.Height + FocusRectTop))
                    {
                        if (elementName == "FocusRect")
                            FocusRect.SetValue(Canvas.TopProperty, ViewPortTop + Viewport.Height - FocusRect.Height);
                        else
                            FocusRect.Height = ViewPortTop + Viewport.Height - FocusRectTop;
                        result = false;
                    }
                }
                
            //}
            //catch
            //{
            //    result = false;
            //}
            return result;
        }

        #endregion


        #region smallRectangle 鼠标事件

        void smallRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();
            prevLeftClick.X = prevLeftClick.Y = 0;
            element.Cursor = null;

            if (Viewport.Width < FocusRect.Width)
                FocusRect.Width = Viewport.Width;
            if (Viewport.Height < FocusRect.Height)
                FocusRect.Height = Viewport.Height;

            AssureFocusRectMoveInZone(element.Name);
            SetRectangles();
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void smallRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            prevLeftClick = e.GetPosition(element);
            trackingMouseMove = true;
            if (null != element)
            {
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// SmallRect[]鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void smallRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackingMouseMove)
            {
                FrameworkElement element = sender as FrameworkElement;
                double offSetY = e.GetPosition(element).Y - prevLeftClick.Y;
                double offSetX = e.GetPosition(element).X - prevLeftClick.X;

                if (AssureFocusRectMoveInZone(element.Name))
                {
                    switch (this.CurrHitPlace)
                    {
                        case HitDownSquare.HDS_TOP:
                            if ((FocusRect.Height - offSetY) > 4)
                            {
                                FocusRect.Height = FocusRect.Height - offSetY;
                                if (FocusRect.Height > 4)
                                    FocusRect.SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + offSetY);
                            }
                            break;
                        case HitDownSquare.HDS_TOPLEFT:
                            if ((FocusRect.Height - offSetY) > 4)
                            {
                                FocusRect.Height = FocusRect.Height - offSetY;
                                if (FocusRect.Height > 4)
                                    FocusRect.SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + offSetY);
                            }
                            if ((FocusRect.Width - offSetX) > 4)
                            {
                                FocusRect.Width = FocusRect.Width - offSetX;
                                if (FocusRect.Width > 4)
                                    FocusRect.SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + offSetX);
                            }
                            break;
                        case HitDownSquare.HDS_TOPRIGHT:
                            if ((FocusRect.Height - offSetY) >4)
                            {
                                FocusRect.Height = FocusRect.Height - offSetY;
                                if (FocusRect.Height > 4)
                                    FocusRect.SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + offSetY);
                            }
                            if ((FocusRect.Width + offSetX) > 4)
                                FocusRect.Width = FocusRect.Width + offSetX;
                            break;
                        case HitDownSquare.HDS_RIGHT:
                            if ((FocusRect.Width + offSetX) > 4)
                                FocusRect.Width = FocusRect.Width + offSetX;
                            break;
                        case HitDownSquare.HDS_BOTTOM:
                            if ((FocusRect.Height + offSetY) > 4)
                                FocusRect.Height = FocusRect.Height + offSetY;
                            break;
                        case HitDownSquare.HDS_BOTTOMLEFT:
                            if ((FocusRect.Height + offSetY) > 4)
                                FocusRect.Height = FocusRect.Height + offSetY;
                            if ((FocusRect.Width - offSetX) > 4)
                            {
                                FocusRect.Width = FocusRect.Width - offSetX;
                                if (FocusRect.Width > 4)
                                    FocusRect.SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + offSetX);
                            }
                            break;
                        case HitDownSquare.HDS_BOTTOMRIGHT:
                            if ((FocusRect.Height + offSetY) > 4)
                                FocusRect.Height = FocusRect.Height + offSetY;
                            if ((FocusRect.Width + offSetX) >4)
                                FocusRect.Width = FocusRect.Width + offSetX;
                            break;
                        case HitDownSquare.HDS_LEFT:
                            if ((FocusRect.Width - offSetX) > 4)
                            {
                                FocusRect.Width = FocusRect.Width - offSetX;
                                if (FocusRect.Width > 4)
                                    FocusRect.SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + offSetX);
                            }
                            break;
                        case HitDownSquare.HDS_NONE:
                            FocusRect.SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + offSetX);
                            FocusRect.SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + offSetY);
                            break;
                    }                   
                }
                SetRectangles();
            }
        }
        #endregion


        #region 设置8个小正方形位置
        /// <summary>
        /// 设置8个小正方形位置
        /// </summary>
        public void SetRectangles()
        {
            //msgBox.Text = "FocusRect height: " + FocusRect.Height + "  Width:" + FocusRect.Width + " Top:" + FocusRect.GetValue(Canvas.TopProperty) + " Left:" + FocusRect.GetValue(Canvas.LeftProperty);
            //左上
            SmallRect[0].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) - Square.Width / 2);
            SmallRect[0].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) - Square.Height / 2);
            //上中间           
            SmallRect[4].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + (FocusRect.Width - Square.Width)/2);
            SmallRect[4].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) - Square.Height/2);
            //右上        
            SmallRect[1].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + FocusRect.Width - Square.Width/2);
            SmallRect[1].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) - Square.Height / 2);
            //左下           
            SmallRect[2].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) - Square.Width / 2);
            SmallRect[2].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + FocusRect.Height - Square.Height / 2);
            //下中间
            SmallRect[5].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + (FocusRect.Width - Square.Width) / 2);
            SmallRect[5].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + FocusRect.Height - Square.Height / 2);
            //右下
            SmallRect[3].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + FocusRect.Width - Square.Height / 2);
            SmallRect[3].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + FocusRect.Height - Square.Height / 2);
            //左中间
            SmallRect[6].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) - Square.Width / 2);
            SmallRect[6].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + (FocusRect.Height - Square.Height) / 2);
            //右中间
            SmallRect[7].SetValue(Canvas.LeftProperty, (double)FocusRect.GetValue(Canvas.LeftProperty) + FocusRect.Width - Square.Width / 2);
            SmallRect[7].SetValue(Canvas.TopProperty, (double)FocusRect.GetValue(Canvas.TopProperty) + (FocusRect.Height - Square.Height) / 2);
        }
        #endregion


        #region 设置鼠标Cursor和相应位置CurrHitPlace
        void smallRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            Hit_Test(element, e.GetPosition(null));
        }

        /// <summary>
        /// 设置鼠标Cursor和相应位置CurrHitPlace
        /// </summary>
        /// <param name="element"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Hit_Test(FrameworkElement element, Point point)
        {
            switch (element.Name)
            {
                case "SmallRect0":
                    {
                        element.Cursor = Cursors.Hand; //.SizeNWSE;
                        CurrHitPlace = HitDownSquare.HDS_TOPLEFT;
                        break;
                    }
                case "SmallRect3":
                    {
                        element.Cursor = Cursors.Hand;//  .SizeNWSE;
                        CurrHitPlace = HitDownSquare.HDS_BOTTOMRIGHT;
                        break;
                    }
                case "SmallRect1":
                    {
                        element.Cursor = Cursors.Hand;//  .SizeNESW;
                        CurrHitPlace = HitDownSquare.HDS_TOPRIGHT;
                        break;
                    }
                case "SmallRect2":
                    {
                        element.Cursor = Cursors.Hand;//  .SizeNESW;
                        CurrHitPlace = HitDownSquare.HDS_BOTTOMLEFT;
                        break;
                    }
                case "SmallRect4":
                    {
                        element.Cursor = Cursors.SizeNS;
                        CurrHitPlace = HitDownSquare.HDS_TOP;
                        break;
                    }
                case "SmallRect5":
                    {
                        element.Cursor = Cursors.SizeNS;
                        CurrHitPlace = HitDownSquare.HDS_BOTTOM;
                        break;
                    }
                case "SmallRect6":
                    {
                        element.Cursor = Cursors.SizeWE;
                        CurrHitPlace = HitDownSquare.HDS_LEFT;
                        break;
                    }
                case "SmallRect7":
                    {
                        element.Cursor = Cursors.SizeWE;
                        CurrHitPlace = HitDownSquare.HDS_RIGHT;
                        break;
                    }
                default:
                    {
                        FocusRect.Cursor = Cursors.Arrow;
                        CurrHitPlace = HitDownSquare.HDS_NONE;
                        break;
                    }
            }
            return true;
        }
       #endregion


        #region 滑动条事件处理代码
        /// <summary>
        /// 滑动条事件处理代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ViewportSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider ZoomInOut = sender as Slider;
            if (ZoomInOut.Value >= ZoomInOut.Minimum && imageRatio * ZoomInOut.Value >= ZoomInOut.Minimum)
            {
                Viewport.Width = ZoomInOut.Value;
                Viewport.Height = imageRatio * ZoomInOut.Value;
                Viewport.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + (ViewportHost.Width - Viewport.Height) / 2);
                Viewport.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + (ViewportHost.Width - Viewport.Width) / 2);
                AssureFocusRectZoomInZone(ZoomInOut.Value, ZoomInOut.Minimum);
            }
        }
        #endregion


        #region 加载图片流信息并设置宽高比例
        /// <summary>
        /// 图片的高宽比
        /// </summary>
        private double imageRatio = 1;

        /// <summary>
        /// 加载图片文件信息
        /// </summary>
        /// <param name="fileStream"></param>
        public void LoadImageStream(FileStream fileStream, Slider zoomInOut)
        {
            double width = ViewportHost.Width, height = ViewportHost.Height;
            //hack:获取相应的图片高宽信息
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(fileStream);
            zoomInOut.Maximum = bitmapImage.PixelWidth;

            #region 用获取的图片高宽初始化Viewport,FocusRect区域和以slider
            if (bitmapImage.PixelWidth < bitmapImage.PixelHeight)//当图片宽小于高时
            {
                if (bitmapImage.PixelWidth > width) //当图片宽度超过可视区域的宽度时
                {
                    height = ((double)width / bitmapImage.PixelWidth) * bitmapImage.PixelHeight;
                    //zoomInOut.Value = (double)width / bitmapImage.PixelWidth;
                }
                else //未超过时则使用图片的高宽初始化显示区域
                {
                    width = bitmapImage.PixelWidth;
                    height = bitmapImage.PixelHeight;
                }
            }
            else//当图片高小于宽时
            {
                if (bitmapImage.PixelHeight > height)//当图片高度超过可视区域的高度时
                {
                    width = ((double)height / bitmapImage.PixelHeight) * bitmapImage.PixelWidth;
                    //zoomInOut.Value = (double)height / bitmapImage.PixelHeight;
                }
                else//未超过时则使用图片的高宽初始化显示区域
                {
                    width = bitmapImage.PixelWidth;
                    height = bitmapImage.PixelHeight;
                }
            }

            Viewport.Width = zoomInOut.Value = width;
            Viewport.Height = height;
            Viewport.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + (ViewportHost.Height - Viewport.Height) / 2);
            Viewport.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + (ViewportHost.Width - Viewport.Width) / 2);

            FocusRect.Width = width >= 100 ? 100 : width;
            FocusRect.Height = height >= 100 ? 100 : height;
            FocusRect.SetValue(Canvas.TopProperty, (double)ViewportHost.GetValue(Canvas.TopProperty) + (ViewportHost.Height - FocusRect.Height) / 2);
            FocusRect.SetValue(Canvas.LeftProperty, (double)ViewportHost.GetValue(Canvas.LeftProperty) + (ViewportHost.Width - FocusRect.Width) / 2);

            zoomInOut.Minimum = 16;
            zoomInOut.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ViewportSlider_ValueChanged);
            imageRatio = (double)bitmapImage.PixelHeight / bitmapImage.PixelWidth;

            SetRectangles();
            #endregion

            selectedImage.SetSource(fileStream);
        }
        #endregion
    }
}
