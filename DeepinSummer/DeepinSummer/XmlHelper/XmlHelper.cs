using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Natsuhime
{
    public class XmlHelper
    {
        /// <summary>
        /// 在根节点下添加父节点
        /// </summary>
        public static void AddParentNode(string xmlPath, string parentNode)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            // 创建一个新的menber节点并将它添加到根节点下
            XmlElement Node = xdoc.CreateElement(parentNode);
            xdoc.DocumentElement.PrependChild(Node);
            xdoc.Save(xmlPath);
        }

        #region 插入一节点,带属性
        /// <summary>
        /// 插入一节点,带一属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribContent">属性值</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 插入一节点,带多属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="AttribList">属性列表</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, List<XmlAttribInfo> xmlAttribInfo, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);

            XmlElement objElement = objXmlDoc.CreateElement(Element);
            foreach (XmlAttribInfo objAttrib in xmlAttribInfo)
            {
                objElement.SetAttribute(objAttrib.Name, objAttrib.Value);
                //objElement.Attributes.Append(objAttrib.Name, objAttrib.Value);
            }
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);

            objXmlDoc.Save(xmlPath);
        }

        #endregion

        #region 插入一节点不带属性

        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        #endregion
        #region 向一个节点添加属性
        /// <summary>
        /// 向一个节点添加属性
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要添加的节点属性的名称</param>
        /// <param name="NodeAttributeText">要添加属性的值</param>
        public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(NodeAttribute1);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            nodePath.Attributes.Append(nodeAttribute);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }
        #endregion

        /// <summary>
        /// 删除XML节点和此节点下的子节点
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="Node">节点路径</param>
        public static void XmlNodeDelete(string xmlPath, string Node)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
            objXmlDoc.Save(xmlPath);
        }

        #region 删除一个节点的属性
        /**/
        /// <summary>
        /// 删除一个节点的属性
        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径（xpath）</param>
        /// <param name="NodeAttribute">属性名称</param>
        public static void xmlnNodeAttributeDel(string xmlPath, string NodePath, string NodeAttribute)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.RemoveAttribute(NodeAttribute);
            objXmlDoc.Save(xmlPath);

        }
        #endregion
    }

    public class XmlAttribInfo
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_Value;
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public XmlAttribInfo()
        {
        }
        public XmlAttribInfo(string attribname, string value)
        {
            this.m_Name = attribname;
            this.m_Value = value;
        }
    }
}
