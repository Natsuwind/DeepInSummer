using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Himeliya.Controls
{
    public class ToolStripColorTable
    {
        public static Color ToolStripBackgroundTop { get; private set; }
        public static Color ToolStripBackgroundMiddle { get; private set; }
        public static Color ToolStripBackgroundBottom { get; private set; }

        public static Color ToolStripSplitButtonTop { get; private set; }
        public static Color ToolStripSplitButtonMiddle { get; private set; }
        public static Color ToolStripSplitButtonBottom { get; private set; }

        public static Color ToolStripButtonMoveBorder { get; private set; }
        public static Color ToolStripButtonMoveBackground { get; private set; }
        public static Color ToolStripButtonSelectedBorder { get; private set; }
        public static Color ToolStripButtonSelectedBackground { get; private set; }

        public static Color ToolStripBackgroundTopLine { get; private set; }
        public static Color ToolStripBackgroundBottomLine { get; private set; }

        static ToolStripColorTable()
        {
            ToolStripBackgroundTop = Color.FromArgb(0x10, 0x64, 0x7d);
            ToolStripBackgroundMiddle = Color.FromArgb(0x0d, 0x6d, 0x8d);
            ToolStripBackgroundBottom = Color.FromArgb(0x05,0x5b,0x80);

            ToolStripSplitButtonTop = Color.FromArgb(0x10,0x64,0x7d);
            ToolStripSplitButtonMiddle = Color.FromArgb(0x44, 0x8f, 0xa9);
            ToolStripSplitButtonBottom = Color.FromArgb(0x06, 0x5e, 0x82);

            ToolStripButtonSelectedBorder = Color.FromArgb(0x13,0x7e,0xa0);
            ToolStripButtonSelectedBackground = Color.FromArgb(0x18,0x61,0x78);

            ToolStripButtonMoveBorder = Color.FromArgb(0x66, 0xa3, 0xad);
            ToolStripButtonMoveBackground = Color.FromArgb(0x2d, 0x7a, 0x92);

            ToolStripBackgroundTopLine = Color.FromArgb(0x16, 0x67, 0x7F);
            ToolStripBackgroundBottomLine = Color.FromArgb(0x4A, 0x89, 0xA4);
        }
    }
}
