using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public partial class ToolStripPanel :Panel
    {
        public ToolStripPanel()
        {
            InitializeComponent();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, e.ClipRectangle.Height);

            using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new PointF(0, Height),
                    ToolStripColorTable.ToolStripBackgroundTop, ToolStripColorTable.ToolStripBackgroundBottom))
            {
                ColorBlend blend = new ColorBlend(3);
                blend.Colors = new Color[] { ToolStripColorTable.ToolStripBackgroundTop, 
                    ToolStripColorTable.ToolStripBackgroundMiddle,ToolStripColorTable.ToolStripBackgroundMiddle,
                    ToolStripColorTable.ToolStripBackgroundBottom};
                blend.Positions = new float[] { 0.0f, 0.33f, 0.58f, 1.0f };

                lgBrush.InterpolationColors = blend;

                g.FillRectangle(lgBrush, rect);

                g.DrawLine(new Pen(ToolStripColorTable.ToolStripBackgroundBottomLine), 0, rect.Height - 2, rect.Width, rect.Height - 2);
                g.DrawLine(Pens.White, 0, Height, Width, Height);
            }

        }

    }
}
