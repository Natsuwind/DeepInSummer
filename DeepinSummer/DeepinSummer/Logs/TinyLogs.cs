using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Natsuhime.Logs
{
    public class TinyLogs
    {
        public static void InsertLog(string logfilepath, DateTime date, object logtype, object message)
        {
            try
            {
                string dir = Path.GetDirectoryName(logfilepath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (FileStream fs = File.Open(logfilepath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine("{0}|{1}|{1}", date.ToString("yyyy-MM-dd mm:ss"), logtype.ToString(), message.ToString());
                    sw.Close();
                    fs.Close();
                }
            }
            catch { }
        }
    }
}
