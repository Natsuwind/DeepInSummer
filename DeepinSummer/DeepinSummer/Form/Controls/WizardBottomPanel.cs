using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class WizardBottomPanel :System.Windows.Forms.Control
    {
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            using (LinearGradientBrush lgBrush = new LinearGradientBrush(pevent.ClipRectangle.Location, new Point(pevent.ClipRectangle.X, pevent.ClipRectangle.Y + Height),
                Color.White, Color.FromArgb(0xe6, 0xe3, 0xdd)))
            {
                pevent.Graphics.FillRectangle(lgBrush, pevent.ClipRectangle);
            }
        }
    }
}
