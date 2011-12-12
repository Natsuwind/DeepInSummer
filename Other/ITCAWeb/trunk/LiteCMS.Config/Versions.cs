using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace LiteCMS.Config
{
    public class Versions
    {
        public const string ASSEMBLY_VERSION = "0.1.1314.1";
        public static string GetProductVersionFromStrimg()
        {
            //从这里设置生成程序集的版本号.
            return string.Format("0.1.{0}.1", DateTime.Now.ToString("MMdd"));
        }
        public static string GetProductVersionFromAssembly()
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            #region 注释取消
            /*
            string extpart;
            switch (ver.FilePrivatePart)
            {
                case 0:
                    extpart = "";
                    break;
                case 1:
                    extpart = " Alpha1";
                    break;
                case 2:
                    extpart = " Alpha2";
                    break;
                case 3:
                    extpart = " Alpha3";
                    break;
                case 4:
                    extpart = " Alpha4";
                    break;
                case 5:
                    extpart = " Alpha5";
                    break;
                case 11:
                    extpart = " Beta1";
                    break;
                case 12:
                    extpart = " Beta2";
                    break;
                case 13:
                    extpart = " Beta3";
                    break;
                case 14:
                    extpart = " Beta4";
                    break;
                case 15:
                    extpart = " Beta5";
                    break;
                case 21:
                    extpart = " RC1";
                    break;
                case 22:
                    extpart = " RC2";
                    break;
                case 23:
                    extpart = " RC3";
                    break;
                case 24:
                    extpart = " RC4";
                    break;
                case 25:
                    extpart = " RC5";
                    break;
                default:
                    extpart = "";
                    break;
            }
             */
            #endregion
            return string.Format("{0}.{1}.{2}.{3}", ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart, ver.FilePrivatePart);
        }
    }
}
