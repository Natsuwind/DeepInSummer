using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace Natsuhime
{
    public class XmlUtility
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();

        public XmlUtility(string XmlFile)
        {
            try
            {
                if (!File.Exists(XmlFile))
                {
                    //CreatXmlFile(XmlFile);
                    throw new Exception(string.Format("没有找到{0}文件", XmlFile));
                }
                objXmlDoc.Load(XmlFile);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }
        protected void CreatXmlFile(string file)
        {
            XmlTextWriter writer = new
            XmlTextWriter(file, Encoding.UTF8);

            // start writing!
            writer.WriteStartDocument();
            writer.WriteStartElement("Root");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        /// <summary>
        /// 查找数据。返回一个DataSet 多条 
        /// </summary>
        /// <param name="XmlPathNode">结点路径</param>
        /// <returns></returns>
        public DataSet GetData(string XmlPathNode)
        {
            //查找数据。返回一个DataSet 
            DataSet ds = new DataSet();
            //=========多个=================
            foreach (XmlNode xmlnode in objXmlDoc.SelectNodes(XmlPathNode))
            {
                StringReader read = new StringReader(xmlnode.OuterXml);
                ds.ReadXml(read);
            }
            //==============================
            return ds;
        }
        /// <summary>
        /// 查找数据。返回一个DataSet 单条
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <returns></returns>
        public DataSet GetDataSingle(string XmlPathNode)
        {
            //查找数据。返回一个DataSet 
            DataSet ds = new DataSet();
            //==========单个================
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            //==============================
            return ds;
        }
        /// <summary>
        /// 更新节点内容
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <param name="Content"></param>
        public void Replace(string XmlPathNode, string Content)
        {
            //更新节点内容。 
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }
        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="Node"></param>
        public void Delete(string Node)
        {
            //删除一个节点。 
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        }
        /// <summary>
        /// 插入一个节点和此节点的一子节点
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="ChildNode"></param>
        /// <param name="Element"></param>
        /// <param name="Content"></param>
        public void InsertNode(string MainNode, string ChildNode, string Element, string Content)
        {
            //插入一个节点和此节点的一子节点。 
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
        }
        /// <summary>
        /// 插入一个节点,带一属性
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="Element"></param>
        /// <param name="Attrib"></param>
        /// <param name="AttribContent"></param>
        /// <param name="Content"></param>
        public void InsertElement(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            //插入一个节点,带一属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// 插入一个节点,不带属性
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="Element"></param>
        /// <param name="Content"></param>
        public void InsertElement(string MainNode, string Element, string Content)
        {
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        public void Save()
        {
            //保存文件。 
            try
            {
                objXmlDoc.Save(strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }
    }
}
