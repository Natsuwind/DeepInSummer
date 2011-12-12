using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Himeliya.Controls
{
    public class TabToolStripColor
    {
        public static Color Background { get; private set; }
        public static Color BackgroundBorder { get; private set; }

        public static Color ButtonBackgroundTop { get; private set; }
        public static Color ButtonBackgroundBottom { get; private set; }

        public static Color ButtonActiveOutBorder { get; private set; }
        public static Color ButtonActiveInnerBorder { get; private set; }

        public static Color ButtonActiveBackgroundTop { get; private set; }
        public static Color ButtonActiveBackgroundBottom { get; private set; }

        static TabToolStripColor()
        {
            Background = Color.FromArgb(0xF3, 0xF3, 0xF3);
            BackgroundBorder = Color.FromArgb(0xE6, 0xE6, 0xE6);
            ButtonBackgroundTop = Color.FromArgb(0xF8, 0xF8, 0xF8);
            ButtonBackgroundBottom = Color.FromArgb(0xEF, 0xEF, 0xEF);

            ButtonActiveOutBorder = Color.FromArgb(0xe8, 0xa1, 0x27);
            ButtonActiveInnerBorder = Color.FromArgb(0xff, 0xbe, 0x30);

            ButtonActiveBackgroundTop = Color.FromArgb(0xff, 0xc0, 0x15);
            ButtonActiveBackgroundBottom = Color.FromArgb(0xff, 0xa0, 0x06);
        }
    }
}
