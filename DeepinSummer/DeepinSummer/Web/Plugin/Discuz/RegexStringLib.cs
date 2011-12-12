using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Web.Plugin.Discuz
{
    public class RegexStringLib
    {
        public static string GetThreadsInBoard()
        {
            return "<span id=\"thread_[0-9]+\"><a href=\"(.*?)\".*>(.*)</a>";
        }

        public static string GetBoardPageCount()
        {
            //   <a\s+href="[a-z0-9\.\-\?\;\=\&]+"\s+class="last".*?>.*?([0-9]+)</a>
            return "<a\\s+href=\"[a-z0-9\\.\\-\\?\\;\\=\\&]+\"\\s+class=\"last\".*?>.*?([0-9]+)</a>";
        }
    }
}
