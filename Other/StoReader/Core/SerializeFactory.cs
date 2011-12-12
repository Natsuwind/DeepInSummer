using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Natsuhime.StoReader.Core
{
    /// <summary>
    /// 泛型　序列化+反序列化
    /// </summary>
    /// <typeparam name="SerializeType"></typeparam>
    public class SerializeFactory<SerializeType>
    {
        public static void SerializeFile(string fileName, SerializeType type)
        {
            try
            {
                using (FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory ,fileName), FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, type);
                }
            }
            catch(Exception ex)
            {
               
            }
        }
        public static SerializeType DeserializeFile(string fileName)
        {
            using (FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory , fileName), FileMode.OpenOrCreate))
            {
                if (fs.Length == 0)
                {
                    return default(SerializeType);
                }
                BinaryFormatter formatter = new BinaryFormatter();
                SerializeType t = (SerializeType)formatter.Deserialize(fs);
                return t;
            }
        }
    }
}

//public static void SaveConfig<T>(T data) where T : SerializableInterface
//        {
//            SerializeFactory<T>.SerializeFile(data.SerializeFileName, data);
//            data.Refresh();
//        }
