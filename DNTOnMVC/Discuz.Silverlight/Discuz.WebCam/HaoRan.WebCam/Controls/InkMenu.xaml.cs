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
using System.Windows.Ink;

namespace HaoRan.WebCam
{
    public partial class InkMenu : UserControl
    {
        public InkMenu()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(InkMenu_Loaded);
        }

        void InkMenu_Loaded(object sender, RoutedEventArgs e)
        {
            inkAttributes = inkPreview.Strokes[0].DrawingAttributes;
            inkAttributes.Color = GetColor(inkFillSlider.Value);
            inkAttributes.OutlineColor = GetColor(inkStrokeSlider.Value);
            inkAttributes.Width = inkAttributes.Height = inkThicknessSlider.Value * 10d;            
        }


        #region 橡皮擦点击事件
        /// <summary>
        /// 橡皮擦点击事件
        /// </summary>
        public event RoutedEventHandler Click
        {
            add
            {
                Erase.Click += value;
            }
            remove
            {
                Erase.Click -= value;
            }
        }
        #endregion

        #region 获取颜色值

        public DrawingAttributes inkAttributes;

        void inkStrokeSlider_ValueChanged(object sender, EventArgs e)
        {
            byte alpha = inkAttributes.OutlineColor.A;
            inkAttributes.Color = GetColor(inkStrokeSlider.Value, alpha);
        }

        void inkFillSlider_ValueChanged(object sender, EventArgs e)
        {
            byte alpha = inkAttributes.Color.A;
            inkAttributes.Color = GetColor(inkFillSlider.Value, alpha);
        }

        void inkThicknessSlider_ValueChanged(object sender, EventArgs e)
        {
            inkAttributes.Width = inkThicknessSlider.Value * 10d;
            inkAttributes.Height = inkThicknessSlider.Value * 10d;
        }

        void inkTransparencySlider_ValueChanged(object sender, EventArgs e)
        {
            Color color = inkAttributes.Color;
            Color outlineColor = inkAttributes.OutlineColor;
            inkAttributes.Color = Color.FromArgb((byte)Math.Floor(256d * (1d - inkTransparencySlider.Value)), color.R, color.G, color.B);
            inkAttributes.OutlineColor = Color.FromArgb((byte)Math.Floor(256d * (1d - inkTransparencySlider.Value)), outlineColor.R, outlineColor.G, outlineColor.B);
        }

     
        /// <summary>
        /// 获取颜色值
        /// </summary>
        /// <returns></returns>
        public Color GetColor(double value)
        {
            return this.GetColor(value, 255);
        }

        /// <summary>
        /// 将滑动条值(Value)转换为ARGB 颜色值并返回
        /// </summary>
        /// <param name="alpha">alpha通道，该值介于0到255</param>
        /// <returns>ARGB 颜色值</returns>
        private Color GetColor(double value, byte alpha)
        {
            Color color;
           
            // 将滑动条的值转换为 ARGB 颜色值
            if (value < 0.143d)
            {
                color = Color.FromArgb(alpha, (byte)Math.Floor((value * 256d) / 0.143d), 0, 0);
            }
            else if (value < 0.286d)
            {
                color = Color.FromArgb(alpha, (byte)Math.Floor(256d * (0.286d - value) / 0.143d), (byte)Math.Floor(256d * (value - 0.143d) / 0.143d), 0);
            }
            else if (value < 0.429)
            {
                color = Color.FromArgb(alpha, 0, (byte)Math.Floor(256d * (0.429d - value) / 0.143d), (byte)Math.Floor(256d * (value - 0.286d) / 0.143d));
            }
            else if (value < 0.571)
            {
                color = Color.FromArgb(alpha, 0, (byte)Math.Floor(256d * (value - 0.429d) / 0.143d), 255);
            }
            else if (value < 0.714)
            {
                color = Color.FromArgb(alpha, (byte)Math.Floor(256d * (value - 0.571d) / 0.143d), (byte)Math.Floor(256d * (0.714d - value) / 0.143d), 255);
            }
            else if (value < 0.857)
            {
                color = Color.FromArgb(alpha, 255, (byte)Math.Floor(256d * (value - 0.714d) / 0.143d), (byte)Math.Floor(256d * (0.857d - value) / 0.143d));
            }
            else
            {
                color = Color.FromArgb(alpha, 255, 255, (byte)Math.Floor(256d * (value - 0.857d) / 0.143d));
            }
            return color;
        }
        #endregion
    }
}
