using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class TitlePanel : Panel
    {
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);

            using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new Point(0, this.Height),
                TitlePanelColor.BackgroundTop, TitlePanelColor.BackgroundBottom))
            {
                e.Graphics.FillRectangle(lgBrush, e.ClipRectangle);
                //e.Graphics.FillRectangle(new SolidBrush(TitlePanelColor.Background), e.ClipRectangle);
                //Rectangle rect = new Rectangle(0, 9, e.ClipRectangle.Width, e.ClipRectangle.Height - 9);
                //e.Graphics.FillRectangle(lgBrush, rect);
                e.Graphics.DrawLine(Pens.White, 0, this.Height - 2, this.Width, this.Height - 2);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(0xe6, 0xe6, 0xe6)), 0, this.Height - 1, this.Width, this.Height - 1);

            }            
        }
    }

    internal class TitlePanelColor
    {
        
        public static Color Background { get; private set; }

        public static Color BackgroundTop { get; private set; }
        public static Color BackgroundBottom { get; private set; }

        static TitlePanelColor()
        {
            Background = Color.FromArgb(0xf3, 0xf3, 0xf3);

            BackgroundTop = Color.FromArgb(0xfc, 0xfc, 0xfc);
            BackgroundBottom = Color.FromArgb(0xee, 0xee, 0xee);
        }
    }
}
