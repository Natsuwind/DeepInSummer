using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.StoReader.Entities
{
    [Serializable]
    public class BookmarkInfo
    {
        public string id { get; set; }
        public int ClassID { get; set; }
        public string BookName { get; set; }
        public int MarkedIndex { get; set; }
        public string FilePath { get; set; }
    }
}
