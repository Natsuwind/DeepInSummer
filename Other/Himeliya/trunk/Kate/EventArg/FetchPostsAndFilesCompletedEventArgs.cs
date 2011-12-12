using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Himeliya.Kate.Entity;

namespace Himeliya.Kate.EventArg
{
    class FetchPostsAndFilesCompletedEventArgs : AsyncCompletedEventArgs
    {
        public List<ProjectInfo> Projects
        { get; set; }

        public FetchPostsAndFilesCompletedEventArgs(
            List<ProjectInfo> projects,
            Exception e,
            bool canceled,
            object userState
            )
            : base(e, canceled, userState)
        {
            this.Projects = projects;
        }

    }
}
