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
using System.Windows.Navigation;
using System.Windows.Browser;

namespace HaoRan.WebCam
{
    public partial class MainPage : Page
    {
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            webCam.IsEnabled = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice() != null;
         
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {}

        private void selectImage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ImageBrowser", UriKind.Relative)); 
        }

        private void webCam_Click(object sender, RoutedEventArgs e)
        {         
            this.NavigationService.Navigate(new Uri("/WebCam", UriKind.Relative)); 
        }
       
      
       
    }
}
