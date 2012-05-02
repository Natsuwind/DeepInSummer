using System;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Common;

namespace Discuz.Mall
{
    /// <summary>
    /// д�ļ�����(����)��
    /// </summary>
    public abstract class WriteFile
    {
        /// <summary>
        /// дjson�ļ�
        /// </summary>
        /// <returns>�Ƿ�д��ɹ�</returns>
        public abstract bool WriteJsonFile();

        /// <summary>
        /// дjson�ļ�
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="json_sb">json����</param>
        /// <returns>�Ƿ�д��ɹ�</returns>
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
