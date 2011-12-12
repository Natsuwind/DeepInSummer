using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class Button : System.Windows.Forms.Button
    {

        public Button()
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(this.Parent.BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(Point.Empty, e.ClipRectangle.Size);
            rect.Width -= 1;
            rect.Height -= 1;

            Color coutBorder;
            Color cinnerBorder;
            Color cbackgroundTop;
            Color cbackgroundBottom;
            Color ctext;

            if (mouseover)
            {
                coutBorder = ButtonColor.HoverOutBorder;
                cinnerBorder = ButtonColor.HoverInnerBorder;
                cbackgroundTop = ButtonColor.HoverBackgroundTop;
                cbackgroundBottom = ButtonColor.HoverBackgroundBottom;
                ctext = mousedown ? Color.Black : ButtonColor.HoverText;
            }
            else
            {
                coutBorder = ButtonColor.OutBorder;
                cinnerBorder = ButtonColor.InnerBorder;
                cbackgroundTop = ButtonColor.BackgroundTop;
                cbackgroundBottom = ButtonColor.BackgroundBottom;
                ctext = ButtonColor.Text;
            }


            using (GraphicsPath path = GraphicsTools.CreateRoundRectangle(rect, 2))
            {
                using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new Point(rect.Width, rect.Height),
                    cbackgroundTop, cbackgroundBottom))
                {
                    g.FillPath(lgBrush, path);
                }

                g.DrawPath(new Pen(coutBorder), path);
                rect.Inflate(-1, -1);
                using (GraphicsPath path2 = GraphicsTools.CreateRoundRectangle(rect, 2))
                {
                    g.DrawPath(new Pen(cinnerBorder), path2);
                }
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(this.Text, this.Font, new SolidBrush(ctext), e.ClipRectangle, sf);

            UpdateBounds(this.Location.X, this.Location.Y, this.Width, this.Height, e.ClipRectangle.Width, e.ClipRectangle.Height);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mouseover = true;
            this.Invalidate(false);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseover = false;
            this.Invalidate(false);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            mousedown = true;
            this.Invalidate(false);
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            mousedown = false;
            this.Invalidate(false);
            base.OnMouseUp(mevent);
        }

        private bool mouseover = false;
        private bool mousedown = false;
    }
}