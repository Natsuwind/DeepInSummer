using System;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Common;

namespace Discuz.Mall
{
    /// <summary>
    /// 写文件操作(抽象)类
    /// </summary>
    public abstract class WriteFile
    {
        /// <summary>
        /// 写json文件
        /// </summary>
        /// <returns>是否写入成功</returns>
        public abstract bool WriteJsonFile();

        /// <summary>
        /// 写json文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="json_sb">json数据</param>
        /// <returns>是否写入成功</returns>
        public bool WriteJsonFile(string path, StringBuilder json_sb)
        {
            try
            {
                if (!Directory.Exists(path))
                    Utils.CreateDir(path);

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(json_sb.ToString());
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
