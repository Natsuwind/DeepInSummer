using System;

namespace LiteCMS.Entity
{
    public class AttachmentInfo
    {
        public int Attachmentid { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public int Filetype { get; set; }
        public int Posterid { get; set; }
        public string Description { get; set; }
    }
}
