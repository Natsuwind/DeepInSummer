using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Natsuhime.Common
{
    public class Config
    {
        public static Dictionary<string, object> LoadConfig(string configFilePath)
        {
            if (configFilePath == null || configFilePath == string.Empty)
            {
                throw new ArgumentNullException("configFilePath");
            }
            return (Dictionary<string, object>)SerializationHelper.LoadBinary(configFilePath);
        }

        public static void SaveConfig(string configFilePath, Dictionary<string, object> list)
        {
            if (configFilePath == null || configFilePath == string.Empty)
            {
                throw new ArgumentNullException("configFilePath");
            }
            if (list == null || list.Count == 0)
            {
                throw new ArgumentNullException("list");
            }
            SerializationHelper.SaveBinary(list, configFilePath);
        }
    }
}
