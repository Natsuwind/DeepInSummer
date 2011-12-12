using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Himeliya.Controls
{
    public class ToolStripRender :ToolStripRenderer
    {
        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);
            ToolStripRadius = 3;
            toolStrip.Cursor = Cursors.Hand;

            toolStrip.AutoSize = false;
            toolStrip.ImageScalingSize = new Size(50, 50);
            

            foreach (ToolStripItem tsi in toolStrip.Items)
            {
                if (tsi is ToolStripButton)
                {
                    tsi.Padding = new Padding(10, 5, 10, 5);
                    tsi.Margin = new Padding(5, 1, 5, 2);
                    tsi.DisplayStyle = ToolStripItemDisplayStyle.Image;
                }
            }
        }


        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            if (e.ToolStrip is ToolStripDropDownMenu) return;
            using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new PointF(0, e.ToolStrip.Height),
                    ToolStripColorTable.ToolStripBackgroundTop, ToolStripColorTable.ToolStripBackgroundBottom))
            {
                ColorBlend blend = new ColorBlend(3);
                blend.Colors = new Color[] { ToolStripColorTable.ToolStripBackgroundTop, 
                    ToolStripColorTable.ToolStripBackgroundMiddle,ToolStripColorTable.ToolStripBackgroundMiddle,
                    ToolStripColorTable.ToolStripBackgroundBottom};
                blend.Positions = new float[] { 0.0f, 0.33f,0.58f, 1.0f };

                lgBrush.InterpolationColors = blend;            

                using (GraphicsPath border = GetToolStripRectangle(e.ToolStrip))
                {
                    e.Graphics.FillPath(lgBrush, border);
                    e.Graphics.DrawLine(new Pen( ToolStripColorTable.ToolStripBackgroundTopLine), 0, 0, e.ToolStrip.Width, 0);
                    e.Graphics.DrawLine(new Pen( ToolStripColorTable.ToolStripBackgroundBottomLine), 0, e.ToolStrip.Height - 2,
                        e.ToolStrip.Width, e.ToolStrip.Height - 2);
                }
            }

        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            // base.OnRenderSplitButtonBackground(e);
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;
            if (null == item) return;
            e.Graphics.DrawLine(new Pen(Color.Red), item.Bounds.Location, new Point(item.Bounds.Location.X, item.Height));

        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            // base.OnRenderSeparator(e);
            if (!e.Item.IsOnDropDown)
            {
                int top = 9;
                int left = e.Item.Width / 2; left--;
                int height = e.Item.Height - top  * 2;
                RectangleF separator = new RectangleF(left, top, 0.5f, height);

                using (LinearGradientBrush b = new LinearGradientBrush(
                    separator.Location,
                    new Point(Convert.ToInt32(separator.Left), Convert.ToInt32(separator.Bottom)),
                    Color.Red, Color.Black))
                {
                    ColorBlend blend = new ColorBlend();
                    blend.Colors = new Color[] { ToolStripColorTable.ToolStripSplitButtonTop, ToolStripColorTable.ToolStripSplitButtonMiddle, ToolStripColorTable.ToolStripSplitButtonMiddle, ToolStripColorTable.ToolStripSplitButtonBottom };
                    blend.Positions = new float[] { 0.0f, 0.22f, 0.78f, 1.0f };

                    b.InterpolationColors = blend;
                    
                    

                    e.Graphics.FillRectangle(b, separator);
                }
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            DrawButtonBackground(e);
        }

        private GraphicsPath GetToolStripRectangle(ToolStrip toolStrip)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(0, 0, toolStrip.Width, 0);
            path.AddLine(toolStrip.Width, 0, toolStrip.Width, toolStrip.Height);
            path.AddLine(toolStrip.Width, toolStrip.Height, 0, toolStrip.Height);
            path.AddLine(0, toolStrip.Height, 0, 0);

            return path;
        }



        protected new void DrawButtonBackground(ToolStripItemRenderEventArgs e)
        {
            bool chk = false;
            if (e.Item is ToolStripButton)
                chk = (e.Item as ToolStripButton).Checked;

            DrawButtonBackground(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), e.Item.Selected, e.Item.Pressed, chk);
        }

        private void DrawButtonBackground(Graphics g, Rectangle r, bool selected, bool pressed, bool checkd)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle border = new Rectangle(r.Left, r.Top, r.Width, r.Height);
            border.Inflate(-5, -5);

            if (selected || pressed || checkd)
            {                
                using (GraphicsPath path = GraphicsTools.CreateRoundRectangle(border, ToolStripRadius))
                {
                   
                    if (checkd)
                    {
                        using (SolidBrush sb = new SolidBrush(ToolStripColorTable.ToolStripButtonSelectedBackground))
                        {
                            g.FillPath(sb, path);
                        }

                        using (Pen p = new Pen(ToolStripColorTable.ToolStripButtonSelectedBorder))
                        {
                            g.DrawPath(p, path);
                        }

                    }
                    else
                    {
                        using (SolidBrush sb = new SolidBrush(ToolStripColorTable.ToolStripButtonMoveBackground))
                        {
                            g.FillPath(sb, path);
                        }
                        using (Pen p = new Pen(ToolStripColorTable.ToolStripButtonMoveBorder))
                        {
                            g.DrawPath(p, path);
                        }
                    }

                    
                }               
            }
        }

        private int ToolStripRadius;
        
    }
}
