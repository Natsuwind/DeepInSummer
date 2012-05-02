using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HaoRan.WebCam
{
    public partial class ImageButton : UserControl
    {
        public ImageButton()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ImageButton_Loaded);
        }

        void ImageButton_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Right };
       
            Image image = new Image() { Width = 30 ,Margin = new Thickness(0, 0, 12, 0)};     
            image.Source = new BitmapImage(new Uri("../" + (string.IsNullOrEmpty(this.ImagePath) ? "icon.jpg" : ImagePath), UriKind.RelativeOrAbsolute));
            stackPanel.Children.Add(image);

            TextBlock tb = new TextBlock() { Text = this.Text, VerticalAlignment = System.Windows.VerticalAlignment.Center}; 
            stackPanel.Children.Add(tb);

            imageButton.Content = stackPanel;           
        }

      
        public event RoutedEventHandler Click
        {
            add
            {
                imageButton.Click += value;
            }
            remove
            {
                imageButton.Click -= value;
            }
        }

        public string Text
        {
            get { return ButtonText.Text; }
            set { ButtonText.Text = value; }
        }

        public string ImagePath
        {
            get ; set;
        }

        
        public double ButtonHeight
        {
            get { return imageButton.Height > 43 ? imageButton.Height : 43; }
            set { imageButton.Height = value > 43 ? value : 43; LayoutRoot.Height = imageButton.Height; }
        }

        public double ButtonWidth
        {
            get { return imageButton.Width > 100 ? imageButton.Width : 100; }
            set { imageButton.Width = value > 100 ? value : 100; LayoutRoot.Width = imageButton.Width; }
        }
    }
}
