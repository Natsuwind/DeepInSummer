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
using System.Text;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace MultiFileUpload
{
    public partial class App : Application
    {
        public static IDictionary<string, string> _initParams;

        public static IDictionary<string, string> GetInitParmas
        {
            get
            {
                return _initParams;
            }
        }

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _initParams = e.InitParams;

            string defaultColor = "LightBlue";

            if (_initParams.ContainsKey("DefaultColor") && !string.IsNullOrEmpty(_initParams["DefaultColor"]))
                defaultColor = _initParams["DefaultColor"];
         
            
            Setter gridBackgroundColorSetter = new Setter(Grid.BackgroundProperty, defaultColor);
            ((Style)this.Resources["GridStyle"]).Setters.Add(gridBackgroundColorSetter);

            Setter borderBackgroundColorSetter = new Setter(Border.BackgroundProperty, defaultColor);
            ((Style)this.Resources["BorderStyle"]).Setters.Add(borderBackgroundColorSetter);

            this.RootVisual = new Page();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;

                try
                {
                    string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                    errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                    System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Silverlight 2 Application Exception: " + errorMsg + "\");");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
