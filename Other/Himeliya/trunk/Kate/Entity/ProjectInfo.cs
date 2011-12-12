using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Himeliya.Kate.Entity
{
    [Serializable]
    class ProjectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Charset { get; set; }
        public IWebProxy Proxy { get; set; }
        public int TotalPageCount { get; set; }
        public int CurrentPageId { get; set; }
        public int CurrentPostId { get; set; }
        public int TotalFileCount { get; set; }
        public int IsActivate { get; set; }

        public List<PostInfo> PostList { get; set; }
        public List<PostInfo> FailedList { get; set; }

        public List<string> ExitsFileUrl { get; set; }
    }
}
