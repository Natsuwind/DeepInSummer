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
using System.Collections.ObjectModel;
using System.Windows.Controls.DataVisualization.Charting;
using System.Runtime.Serialization.Json;
using System.Windows.Browser;
using System.IO;
using System.Xml;

namespace PollChart
{
    public partial class MainPage : UserControl
    {
        public HtmlDocument _htmlDocument = HtmlPage.Document;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            GetJSONData();
        }


        public static string ServiceUrl
        {
            get
            {
                string path = Application.Current.Host.Source.AbsoluteUri;
                path = path.Substring(0, path.IndexOf("/silverlight/piechart"));
                // Application.Current.Host.Source.AbsoluteUri.Replace(path, "");
                //System.Windows.Browser.HtmlPage.Window.Alert(path + "/services/MixObjects.asmx");
                //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsolutePath);
                //System.Windows.Browser.HtmlPage.Window.Alert(Application.Current.Host.Source.AbsoluteUri);
                //System.Windows.Browser.HtmlPage.Window.Alert(path + "/services/Album.asmx");
                //return path;
                return path + "/services/MixObjects.asmx/PollPieData?topicid=";
            }
        }


         public void GetJSONData()
        {
            string serverUri = _htmlDocument.DocumentUri.ToString();
            
            //int thisApp = serverUri.IndexOf("/silverlight/piechart");

            serverUri = ServiceUrl;// serverUri = serverUri.Substring(0, thisApp) + "/services/MixObjects.asmx/PollPieData?topicid="; //"http://localhost:8080/services/MixObjects.asmx/PollPieData?topicid=";
            string value = "";
            HtmlDocument doc = HtmlPage.Document;
            doc.QueryString.TryGetValue("topicid", out value);
            System.Uri webServiceUri = new System.Uri(serverUri + value);
   
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
            wc.OpenReadAsync(webServiceUri);
        }


         void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
         {
             try
             {
                 StreamReader responseReader = new StreamReader(e.Result);

                 string jsonData = responseReader.ReadToEnd();
                 responseReader.Close();

                 XmlReader xr = XmlReader.Create(new StringReader(jsonData));
                 xr.ReadToFollowing("string");
                 xr.Read();


                 DataContractJsonSerializer jss = new DataContractJsonSerializer(typeof(ChartClass));
                 ChartClass cc = (ChartClass)jss.ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xr.Value)));

                 Collection<SectorData> inputData = new Collection<SectorData>();
                 foreach (SectorData sector in cc.Sectors)
                 {
                     inputData.Add(sector);
                 }

                 PieSeries series = new PieSeries();
                 series.ItemsSource = inputData;
                 series.IndependentValueBinding = new System.Windows.Data.Binding("Title");
                 series.DependentValueBinding = new System.Windows.Data.Binding("Value");
                 series.AnimationSequence = AnimationSequence.LastToFirst;
                 PollPieChart.Series.Add(series);
             }

             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             {}
         }
    }

    public class ChartClass
    {
        public string CreateDate;
        public SectorData[] Sectors;
    }
}
