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
using System.Windows.Ink;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.IO;

namespace HaoRan.WebCam
{
    public partial class AdvanceMode : Page
    {
        /// <summary>
        /// Focus区域宽度
        /// </summary>
        double FocusWidth = 0;
        /// <summary>
        /// Focus区域高度
        /// </summary>
        double FocusHeight = 0;
        /// <summary>
        /// 文件名称
        /// </summary>
        string fileName;

        public AdvanceMode()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AdvanceMode_Loaded);
        }

        void AdvanceMode_Loaded(object sender, RoutedEventArgs e)
        {      
            if (this.NavigationContext.QueryString.ContainsKey("focusWidth"))
                double.TryParse(NavigationContext.QueryString["focusWidth"], out FocusWidth);
            if (this.NavigationContext.QueryString.ContainsKey("focusHeight"))
                double.TryParse(NavigationContext.QueryString["focusHeight"], out FocusHeight);
            if (this.NavigationContext.QueryString.ContainsKey("fileName"))
                fileName = NavigationContext.QueryString["fileName"];

            Viewport.Width = FocusWidth;
            Viewport.Height = FocusHeight;
            
            Viewport.SetValue(Canvas.TopProperty, (double)InkCanvas.GetValue(Canvas.TopProperty) + (InkCanvas.Height - Viewport.Height) / 2);
            Viewport.SetValue(Canvas.LeftProperty, (double)InkCanvas.GetValue(Canvas.LeftProperty) + (InkCanvas.Width - Viewport.Width) / 2);
           
            VisualEffect.SelectedIndex = 0;
            ObservableCollection<BitmapImage> previewImage = Utils.GetPreviewImage(Utils.ServiceUrl, Utils.GetUserId());
            Viewport.Source = previewImage[0];   
            Viewport.MouseRightButtonDown += new MouseButtonEventHandler(Viewport_MouseRightButtonDown);
            Viewport.MouseMove += new MouseEventHandler(Viewport_MouseMove);
            Viewport.MouseLeftButtonDown += new MouseButtonEventHandler(Viewport_MouseLeftButtonDown);
            Viewport.MouseRightButtonDown += new MouseButtonEventHandler(Viewport_MouseRightButtonDown);

            ViewportSliderX.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ViewportSlider_ValueChanged);
            ViewportSliderY.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ViewportSlider_ValueChanged);
            ViewportSliderZ.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ViewportSlider_ValueChanged);
        }

        void ViewportSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ImageProjection.RotationX = ViewportSliderX.Value;
            ImageProjection.RotationY = ViewportSliderY.Value;
            ImageProjection.RotationZ = ViewportSliderZ.Value;
        }
     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {}

        #region VisualEffect视觉效果

        bool applyRippleEffect;

        RippleEffect rippleEffect = new RippleEffect();
        /// <summary>
        /// 视觉效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Viewport.Effect = null;
            switch (((ListBoxItem)VisualEffect.SelectedValue).Tag.ToString())
            {
                case "InvertColor": Viewport.Effect = new InvertColorEffect(); break;
                case "Ripple":
                    {
                        Viewport.Effect = rippleEffect;                      
                        applyRippleEffect = true;
                        break;
                    }
                case "ParametricEdge": Viewport.Effect = new ParametricEdgeDetectionShader(); break;
                default: Viewport.Effect = null; break;
            }
        }
        #endregion

        #region Ink 事件代码
        public enum InkEditingMode
        {
            None,
            Ink,
            Erase
        }
        private InkEditingMode editingMode = InkEditingMode.Ink;

        private Stroke inkStroke = null;

        private StylusPointCollection erasePoints = null;

        void onInkPresenterDown(object sender, MouseButtonEventArgs e)
        {
            if (editingMode == InkEditingMode.None)
                return;

            (sender as FrameworkElement).CaptureMouse();
            StylusPointCollection stylusPoints = e.StylusDevice.GetStylusPoints(InkCanvas);

            if (editingMode == InkEditingMode.Erase)
            {
                erasePoints = new StylusPointCollection();
                erasePoints.Add(stylusPoints);
            }
            else if (editingMode == InkEditingMode.Ink)
            {
                inkStroke = new Stroke();
                inkStroke.StylusPoints.Add(stylusPoints);
                inkStroke.DrawingAttributes = new DrawingAttributes();
                inkStroke.DrawingAttributes.Color = inkMenu.inkAttributes.Color;
                inkStroke.DrawingAttributes.OutlineColor = inkMenu.inkAttributes.OutlineColor;
                inkStroke.DrawingAttributes.Width = inkMenu.inkAttributes.Width;
                inkStroke.DrawingAttributes.Height = inkMenu.inkAttributes.Height;
                InkCanvas.Strokes.Add(inkStroke);
            }
        }

        void SetEditingMode()
        {
            if (EditInkMode.IsChecked == true)
            {
                if (inkMenu.Erase.IsChecked == false)
                    editingMode = InkEditingMode.Ink;
                else
                    editingMode = InkEditingMode.Erase;
            }
            else
                editingMode = InkEditingMode.None;
        }

        void onInkPresenterMove(object sender, MouseEventArgs e)
        {
            SetEditingMode();
            if (editingMode == InkEditingMode.None) return;
            StylusPointCollection stylusPoints = e.StylusDevice.GetStylusPoints(InkCanvas);

            if (editingMode == InkEditingMode.Erase)
            {
                if (erasePoints != null)
                {
                    // hittest and erase
                    erasePoints.Add(stylusPoints);
                    StrokeCollection hitStrokes = InkCanvas.Strokes.HitTest(erasePoints);
                    for (int i = 0; i < hitStrokes.Count; i++)
                    {
                        InkCanvas.Strokes.Remove(hitStrokes[i]);
                    }
                }
            }
            else if (editingMode == InkEditingMode.Ink)
            {
                if (inkStroke != null)
                {
                    inkStroke.StylusPoints.Add(stylusPoints);
                }
            }
        }
    
        void onInkPresenterEnter(object sender, MouseButtonEventArgs e)
        {
            if (EditInkMode.IsChecked == true)
            {
                 if (inkMenu.Erase.IsChecked == false)
                    editingMode = InkEditingMode.Ink;
                 else
                    editingMode = InkEditingMode.Erase;
            }
            else
                editingMode = InkEditingMode.None;

            erasePoints = null;
            inkStroke = null;
        }

        void onInkPresenterUp(object sender, MouseButtonEventArgs e)
        {
            if (editingMode == InkEditingMode.None)
                return;

            if (inkStroke != null)
            {
                inkStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(InkCanvas));
            }
            (sender as FrameworkElement).ReleaseMouseCapture();
            erasePoints = null;
            inkStroke = null;
        }

        private void EditInkMode_Click(object sender, RoutedEventArgs e)
        {
            if (EditInkMode.IsChecked == true)
            {
                inkMenu.Visibility = System.Windows.Visibility.Visible;
                if (inkMenu.Erase.IsChecked == false)
                    editingMode = InkEditingMode.Ink;
                else
                    editingMode = InkEditingMode.Erase;
            }
            else
            {
                inkMenu.Visibility = System.Windows.Visibility.Collapsed;
                editingMode = InkEditingMode.None;
            }
        }

        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            inkMenu.Visibility = System.Windows.Visibility.Visible;
            if (inkMenu.Erase.IsChecked == false)
                editingMode = InkEditingMode.Ink;
            else
                editingMode = InkEditingMode.Erase;
        }

        #endregion

        #region 上传头像事件代码
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            Utils.UploadUserFile(fileName, InkCanvas, Viewport,
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
                cw.LargeImageWidth.Text = Viewport.Width.ToString();
                cw.LargeImageHeight.Text = Viewport.Height.ToString();
                cw.Show();
            }
        }
        #endregion

        #region Viewport鼠标事件代码(目前用于VisualEffect)

        void Viewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (applyRippleEffect && EditInkMode.IsChecked == false)
            {
                Point point = e.GetPosition(sender as FrameworkElement);
                rippleEffect.Center = new Point(point.X / Viewport.ActualWidth, point.Y / Viewport.ActualHeight);
            }
        }

        void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {         
            if (EditInkMode.IsChecked == false)
               applyRippleEffect = false;
        }

        void Viewport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            applyRippleEffect = true;

            Point point = e.GetPosition(sender as FrameworkElement);
            rippleEffect.Center = new Point(point.X / Viewport.ActualWidth, point.Y / Viewport.ActualHeight);
        }
        
        #endregion
                
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void InkCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            editingMode = InkEditingMode.None;
            erasePoints = null;
            inkStroke = null;
        }
    }
}
