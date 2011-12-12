using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Yuwen.Tools.CreateUpdateTools
{
    internal class HashTools
    {
        /// <summary>
        /// 生成文件的MD5码
        /// </summary>
        /// <param name="fileName">文件名，包含全路径</param>
        /// <returns>MD5码</returns>
        public static string GenerateFileHashCode(string fileName)
        {
            if (fileName == null || fileName.Equals(""))
            {
                throw new ArgumentException("文件名不能为空", "fileName");

            }
            string keyString = string.Empty;
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    keyString = Md532(fs);
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }
            return keyString;
        }

        private static string Md532(Stream fs)
        {

            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(fs);
            string pwd = BitConverter.ToString(s);
            pwd = pwd.Replace("-", "");
            return pwd;
        }
    }
}
