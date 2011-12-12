using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Himeliya.Controls
{
    class ButtonColor
    {
        public static Color OutBorder { get; private set; }
        public static Color InnerBorder { get; private set; }

        public static Color BackgroundTop { get; private set; }
        public static Color BackgroundBottom { get; private set; }

        public static Color Text { get; private set; }

        public static Color HoverOutBorder { get; private set; }
        public static Color HoverInnerBorder { get; private set; }

        public static Color HoverBackgroundTop { get; private set; }
        public static Color HoverBackgroundBottom { get; private set; }

        public static Color HoverText { get; private set; }

        static ButtonColor()
        {
            OutBorder = Color.FromArgb(0xd5, 0xd5, 0xd5);
            InnerBorder = Color.FromArgb(0xf6, 0xf4, 0xf3);

            BackgroundTop = Color.FromArgb(0xEF, 0xED, 0xED);
            BackgroundBottom = Color.FromArgb(0xd7, 0xd5, 0xd5);

            Text = Color.FromArgb(0x54, 0x52, 0x52);

            HoverOutBorder = Color.FromArgb(0xe8, 0xa1, 0x27);
            HoverInnerBorder = Color.FromArgb(0xff, 0xd7, 0x56);

            HoverBackgroundTop = Color.FromArgb(0xff, 0xbf, 0x15);
            HoverBackgroundBottom = Color.FromArgb(0xff, 0xa2, 0x07);

            HoverText = Color.White;
        }
    }
}
