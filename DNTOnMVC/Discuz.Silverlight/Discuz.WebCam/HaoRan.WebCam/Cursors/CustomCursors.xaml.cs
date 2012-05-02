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

namespace HaoRan.WebCam
{
    public partial class CustomCursors : UserControl
    {
        public CustomCursors()
        {
            InitializeComponent();
        }

        public void SetPostion(Point position)
        {
            double leftOffset = this.ActualWidth / 2;
            double topOffset = this.ActualHeight / 2;
            double newLeft = position.X - leftOffset;
            double newTop = position.Y - topOffset;
            Canvas.SetLeft(this, newLeft);
            Canvas.SetTop(this, newTop);
        }
    }
}
