using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Himeliya.Kate.Entity;

namespace Himeliya.Kate.EventArg
{
    class FetchTitleCompletedEventArgs : AsyncCompletedEventArgs
    {
        public List<PostInfo> Posts
        { get; set; }

        public int TotalPageCount
        { get; set; }

        public FetchTitleCompletedEventArgs(
            List<PostInfo> posts,
            int totalPageCount,
            Exception e,
            bool canceled,
            object userState
            )
            : base(e, canceled, userState)
        {
            this.Posts = posts;
            this.TotalPageCount = totalPageCount;
        }

    }
}
