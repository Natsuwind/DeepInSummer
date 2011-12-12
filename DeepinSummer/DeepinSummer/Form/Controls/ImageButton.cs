using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using System.Resources;
using System.Reflection;


namespace Himeliya.Controls
{
    public class ImageButton : Button
    {
        [ToolboxItem(true)]
        [Browsable(true)]
        public Image DefaultImage
        {
            get
            {
                return defaultImg;
            }
            set
            {
                defaultImg = value;
            }
        }
        [ToolboxItem(true)]
        [Browsable(true)]
        public Image HoverImage
        {
            get
            {
                return hoverImg;
            }
            set
            {
                hoverImg = value;
            }
        }

        public ImageButton()
        {
            this.Cursor = Cursors.Hand;
            Assembly foms = Assembly.GetAssembly(typeof(Control));
            Image img = Bitmap.FromStream(foms.GetManifestResourceStream("System.Windows.Forms.blank.bmp"));
            defaultImg = img;
            hoverImg = img;

            this.Width = 102;
            this.Height = 78;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(this.Parent.BackColor);

            Image currentImg;
            int x = Location.X;
            int y = Location.Y;

            if (mousemove)
            {
                currentImg = hoverImg;
            }
            else if (mousedown)
            {
                currentImg = defaultImg;
                x -= 2;
                y -= 2;
            }
            else
            {
                currentImg = defaultImg;
            }


            g.DrawImage(currentImg,0,0,e.ClipRectangle.Width,e.ClipRectangle.Height);


            UpdateBounds(x,y, this.Width, this.Height, e.ClipRectangle.Width, e.ClipRectangle.Height);
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            mousemove = true;
            this.Invalidate(false);
            base.OnMouseEnter(eventargs);
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            mousemove = false;
            this.Invalidate(false);
            base.OnMouseLeave(eventargs);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            mousedown = true;            
            Invalidate(false);
            OnPaint(new PaintEventArgs(this.CreateGraphics(), this.Bounds));
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            mousedown = false;
            Invalidate(false);
            base.OnMouseUp(mevent);
        }

        private bool mousemove = false;
        private bool mousedown = false;
        private Image defaultImg;
        private Image hoverImg;

    }
}
