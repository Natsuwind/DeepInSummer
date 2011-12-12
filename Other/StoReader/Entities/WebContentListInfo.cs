using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.StoReader.Entities
{
    public class WebContentListInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<WebContentInfo> ContentList { get; set; }
    }
}
