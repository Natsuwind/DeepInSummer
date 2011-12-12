using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;
using System.ComponentModel;

namespace Himeliya.Controls
{
    public class ProgressBar:System.Windows.Forms.Control
    {
        public ProgressBar()
        {
            timeControl = new Timer();
            timeControl.Interval = 1;
            timeControl.Tick += new EventHandler(timeControl_Tick);

            Width = 120;
            Maximum = 100;
            Minimum = 0;
            _Value = 0;
            currentWidth = 0;
            targetWidth = 0;
        }

        void timeControl_Tick(object sender, EventArgs e)
        {
            if (0==targetWidth)
            {
                currentWidth = 0;
                updatePos();
                timeControl.Enabled = false;
            }
            else if (Width - 4 == targetWidth)
            {
                currentWidth = targetWidth;
                updatePos();
                timeControl.Enabled = false;
            }
            else if (currentWidth < targetWidth)
            {
                currentWidth = currentWidth + stepWidth > targetWidth ? targetWidth : currentWidth + stepWidth;
                updatePos();
            }
            else if (currentWidth > targetWidth)
            {
                currentWidth = currentWidth - stepWidth < targetWidth ? targetWidth : currentWidth - stepWidth;
                updatePos();
            }
            else timeControl.Enabled = false;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GraphicsTools.CreateRoundRectangle(this.ClientRectangle,4))
            {
                g.FillPath(new SolidBrush(Color.FromArgb(0xc8, 0xc8, 0xc8)), path );
            }

            updatePos();
        }

        [ToolboxItem(true)]
        [Browsable(true)]
        public int Maximum
        {
            get;
            set;
        }
        [ToolboxItem(true),Browsable(true)]
        public int Minimum { get; set; }
        [ToolboxItem(true), Browsable(true)]
        public int Value
        {
            get { return _Value; }
            set
            {
                if (value > Maximum)
                    throw new ArgumentException(string.Format("Value[{0}]不能大于Maximum[{1}]。", value, Maximum), "Value");
                if (value < Minimum)
                    throw new ArgumentException(string.Format("value[{1}]不能小于Minimum[{1]]。", value, Minimum), "Value");

                _Value = value;
                targetWidth = (int)Math.Floor((Convert.ToDecimal(this.Width) / Maximum) * value);
                targetWidth = targetWidth > this.Width - 4 ? this.Width - 4 : targetWidth;
                timeControl.Enabled = true;
            }
        }
        [Browsable(false)]
        public new int Height { get { return 18; } }

        

        private void updatePos()
        {
            Graphics g = this.CreateGraphics();
            g.Clear(this.BackColor);
            Rectangle rect = this.ClientRectangle;
            rect.Inflate(-2, -2);
            rect.Width = currentWidth;
            using (LinearGradientBrush lgBrush = new LinearGradientBrush(rect.Location, new Point(rect.X, rect.Height + 1),
                Color.FromArgb(0xff, 0x91, 0x04), Color.FromArgb(0xfe, 0x5d, 0x03)))
            {
                g.FillRectangle(lgBrush, rect);
            }
            // 
            g.DrawLine(new Pen(Color.FromArgb(0xfd, 0xa8, 0x21)), rect.Location.X, rect.Location.Y + 1, rect.Width - 4, rect.Location.Y + 1);

            g.Dispose();
        }

        private Timer timeControl;
        private int currentWidth;
        private int targetWidth;
        private const int stepWidth = 5;


        private int _Value;
    }
}
