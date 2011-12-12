using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Comsenz.Ywen.SWMUpdateCreator
{
    [Serializable, XmlRoot("AutoUpdater")]
    public class UpdateListXmlInfo
    {
        public string Description { get; set; }
        public UpdaterInfo Updater { get; set; }
        public ApplicationInfo Application { get; set; }
        [XmlArrayItem("File")]
        public List<SWMFileInfo> Files { get; set; }
        public string ChangeLog { get; set; }
    }

    public class UpdaterInfo
    {
        public string Url { get; set; }
        public string LastUpdateTime { get; set; }
    }
    public class ApplicationInfo
    {
        [XmlAttribute("applicationId")]
        public string ApplicationId { get; set; }
        public string EntryPoint { get; set; }
        public string Location { get; set; }
        public string Version { get; set; }
    }
    public class SWMFileInfo
    {
        [XmlAttribute("Ver")]
        public string Ver { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("MD5")]
        public string MD5 { get; set; }
        [XmlIgnore()]
        public string FullFileName { get; set; }
        [XmlIgnore()]
        public string RelativeCreateName { get; set; }
    }
}
