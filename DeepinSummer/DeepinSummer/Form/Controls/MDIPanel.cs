using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class MDIPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.BackColor = Color.White;
            using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new Point(0, 10),
                Color.FromArgb(0xf0, 0xf0, 0xf0), Color.White))
            {
                e.Graphics.FillRectangle(lgBrush, 6, 0, this.Width, 10);

            }

            using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new Point(10, 0),
                Color.FromArgb(0xf0, 0xf0, 0xf0), Color.White))
            {
                e.Graphics.FillRectangle(lgBrush, 0, 6, 10, this.Height);
            }

            using (SolidBrush sb = new SolidBrush(Color.FromArgb(0xf0, 0xf0, 0xf0)))
            {
                e.Graphics.FillRectangle(sb, 0, 0, 6, 6);
            }

            this.Padding = new Padding(20);
        }
    }
}
