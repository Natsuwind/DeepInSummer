using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Wysky.Tools;
using System.IO;

namespace TemplateCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1 && args.Length < 4)
            {
                #region 获取参数
                string templatefolder = args[0];
                string aspxpagefilefolder = args[1];
                string version = args.Length == 3 ? args[2] : "2.6."+DateTime.Now.ToString("MMdd");

                if (!Directory.Exists(templatefolder))
                {
                    Console.WriteLine("SourceTemplateFolder DOES NOT exists!");
                    return;
                }
                if (!Directory.Exists(aspxpagefilefolder))
                {
                    Console.WriteLine("AspxCreatedFolder DOES NOT exists!");
                    return;
                }
                #endregion

                string namespaces = "System.Data,Discuz.Common,Discuz.Forum,Discuz.Entity";
                NewTemplate templatecreator = new NewTemplate("Discuz.Web", namespaces);
                templatecreator.Productversion = version;
                templatecreator.Productname = string.Format("Discuz!NT {0} (.net Framework 2.x/3.x)", version);
                Console.WriteLine(@"Creating...");
                try
                {
                    templatecreator.CreateFromFolder(templatefolder, aspxpagefilefolder);
                    Console.WriteLine(@"Completed...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"Error:" + ex.Message);
                }
            }
            else
            {
                Console.WriteLine(@"Discuz!NT Template Creator for Command-Line v1.0");
                Console.WriteLine(@"Power by wysky 2008-12");
                Console.WriteLine(@"Useage:");
                Console.WriteLine(@"TemplateCreator.exe SourceTemplateFolder AspxCreatedFolder [ForumVersion]");
                Console.WriteLine(@"TemplateCreator.exe 模板目录 生成目录 论坛版本(可选参数,建议填写.)");
                Console.WriteLine(@"Likes:");
                Console.WriteLine(@"TemplateCreator.exe c:\dnt\template c:\dnt\aspx\1 2.6.1216");
            }

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
