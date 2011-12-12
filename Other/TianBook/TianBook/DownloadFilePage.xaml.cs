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
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.IO;

namespace TianBook
{
    public partial class DownloadFilePage : PhoneApplicationPage
    {
        public DownloadFilePage()
        {
            InitializeComponent();
            wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            wc.OpenReadAsync(
                new Uri(tbxUrl.Text.Trim()),
                ""
                );
        }


        WebClient wc;
        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                string iconPath = "1.txt";

                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var isfs = new IsolatedStorageFileStream(iconPath, FileMode.Create, isf);
                    int bytesRead;
                    byte[] bytes = new byte[e.Result.Length];
                    while ((bytesRead = e.Result.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        isfs.Write(bytes, 0, bytesRead);
                    }
                    isfs.Flush();
                    isfs.Close();
                }


                this.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
            }
        }
    }
}