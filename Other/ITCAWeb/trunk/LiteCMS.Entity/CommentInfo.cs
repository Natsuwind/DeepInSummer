using System;

namespace LiteCMS.Entity
{
    public class CommentInfo
    {
        public int Commentid { get; set; }
        public int Articleid { get; set; }
        public string Articletitle { get; set; }
        public int Uid { get; set; }
        public string Username { get; set; }
        public string Postdate { get; set; }
        public int Del { get; set; }
        public string Content { get; set; }
        public int Goodcount { get; set; }
        public int Badcount { get; set; }
    }
}