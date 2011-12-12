using System;
using System.Collections.Generic;
using System.Text;

namespace Himeliya.Kate.Entity
{
    class PostInfo
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<string> FileList { get; set; }
        public List<string> SuccessFileList { get; set; }
        public List<string> FailedFileList { get; set; }
    }
}
