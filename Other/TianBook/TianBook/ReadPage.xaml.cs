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
    public partial class ReadPage : PhoneApplicationPage
    {
        public ReadPage()
        {
            InitializeComponent();
        }
        string filename;
        string[] txtdata;
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            filename = NavigationContext.QueryString["filename"].ToString();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var isfs = new IsolatedStorageFileStream(filename, FileMode.Open, isf);

                StreamReader sr = new StreamReader(isfs);
                txtdata = sr.ReadToEnd().Split('\n');
                sr.Close();
                isfs.Close();
            }
        }
    }
}