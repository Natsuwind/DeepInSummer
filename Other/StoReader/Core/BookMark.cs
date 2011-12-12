using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Natsuhime.StoReader.Entities;

namespace Natsuhime.StoReader.Core
{
    public class Bookmark
    {
        static XmlDocument ConfigDoc;
        static string ConfigfilePath;
        static Bookmark()
        {
            ConfigfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TXTReader.config");
            if (!File.Exists(ConfigfilePath))
                File.Create(ConfigfilePath);
            ConfigDoc = new XmlDocument();
            ConfigDoc.Load(ConfigfilePath);
        }
        public static void Add(BookmarkInfo info)
        {
            XmlNode objNode = ConfigDoc.SelectSingleNode("/TXTReaderConfig/bookmark");
            XmlElement objElement = ConfigDoc.CreateElement("item");
            objElement.SetAttribute("id", Guid.NewGuid().ToString());
            objElement.SetAttribute("classid", info.ClassID.ToString());
            objElement.SetAttribute("bookname", info.BookName);
            objElement.SetAttribute("markedindex", info.MarkedIndex.ToString());
            objElement.SetAttribute("filepath", info.FilePath);
            //objElement.InnerText = info.BookName;
            objNode.AppendChild(objElement);
            ConfigDoc.Save(ConfigfilePath);

            //List<Natsuhime.XmlAttribInfo> arribs = new List<Natsuhime.XmlAttribInfo>();
            //arribs.Add(new XmlAttribInfo("classid     ", info.ClassID.ToString()             ));
            //arribs.Add(new XmlAttribInfo("markedindex ", info.MarkedIndex.ToString()             ));
            //arribs.Add(new XmlAttribInfo("filepath    ", info.FilePath                  ));
            //XmlHelper.XmlInsertElement(ConfigfilePath, "/TXTReaderConfig/bookmark", "item", arribs, info.BookName);
        }
        public static void Update(BookmarkInfo info)
        {
            XmlNodeList nodelist = ConfigDoc.SelectSingleNode("/TXTReaderConfig/bookmark").ChildNodes;
            foreach (XmlNode xn in nodelist)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Attributes["id"].Value == info.id.ToString())
                {
                    xe.SetAttribute("markedindex", info.MarkedIndex.ToString());
                    break;
                }
            }
            ConfigDoc.Save(ConfigfilePath);
        }

        public static List<BookmarkInfo> GetBookmarkList()
        {
            List<BookmarkInfo> bookmarklist = new List<BookmarkInfo>();
            XmlNode objNode = ConfigDoc.SelectSingleNode("/TXTReaderConfig/bookmark");
            foreach (XmlNode n in objNode.ChildNodes)
            {
                BookmarkInfo info = new BookmarkInfo();
                info.id = n.Attributes["id"].Value;
                info.ClassID = Convert.ToInt32(n.Attributes["classid"].Value);
                info.BookName = n.Attributes["bookname"].Value;
                info.MarkedIndex = Convert.ToInt32(n.Attributes["markedindex"].Value);
                info.FilePath = n.Attributes["filepath"].Value;
                bookmarklist.Add(info);
                //foreach (XmlAttribute a in n.Attributes)
                //{
                //    //a.
                //}
            }
            return bookmarklist;
        }

    }
}
