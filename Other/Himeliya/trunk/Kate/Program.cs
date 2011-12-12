using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Natsuhime.Data;
using Natsuhime.Common;

namespace Himeliya.Kate
{
    static class Program
    {
        internal static string configPath;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            KateInit();
            Application.Run(new MainForm());
        }

        static void KateInit()
        {
            configPath = Utils.GetMapPath("~/config/kate.config");
            DbHelper.ConnectionString = @"Data Source=" + configPath;
            DbHelper.Dbtype = "Sqlite";
        }
    }
}
