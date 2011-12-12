using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class TabToolStripRender : ToolStripRenderer
    {

        protected override void Initialize(ToolStrip toolStrip)
        {
            // base.Initialize(toolStrip);
            toolStrip.Cursor = Cursors.Hand;

            toolStrip.Padding = new Padding(0,0,0,0);
            toolStrip.Margin = new Padding(0, 0, 0, 0);

            toolStrip.GripMargin = new Padding(0);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            //toolStrip.PreferredSize = new Size(toolStrip.Width, toolStrip.Height);

            foreach (ToolStripItem tsi in toolStrip.Items)
            {
                if (tsi is ToolStripButton)
                {
                    tsi.AutoSize = false;
                    tsi.Margin = new Padding(0);
                    tsi.Padding = new Padding(0);
                    tsi.Height = 33;
                    tsi.Width = toolStrip.Width;
                }
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(TabToolStripColor.Background), e.ToolStrip.Bounds);

            int left = e.ToolStrip.Left;
            int t = e.ToolStrip.Top;
            int w = e.ToolStrip.Width;
            int h = e.ToolStrip.Height;

            g.DrawLine(Pens.White, w - 2 + left, t, w - 2 + left, t + h);
            g.DrawLine(new Pen(TabToolStripColor.BackgroundBorder), w - 1 + left, t, w - 1 + left, t + h);

        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripButton)
            {
                
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (!(e.Item as ToolStripButton).Checked || e.Item.Pressed || e.Item.Selected)
                {
                    if (e.Item.Pressed || e.Item.Selected)
                    {
                        int top = 0;

                        if (e.Item != e.ToolStrip.Items[0])
                        {
                            top++;
                            g.DrawLine(new Pen(TabToolStripColor.BackgroundBorder), 0, 0, e.Item.Width, 0);
                        }

                        Rectangle rectOutBorder = new Rectangle(0, top, e.Item.Width - 1, e.Item.Height - top - 1);
                        Rectangle rectInnerBorder = new Rectangle(0, top, e.Item.Width - 1, e.Item.Height - top - 1);
                        rectInnerBorder.Inflate(-1, -1);
                        Rectangle rectButtonBackground = new Rectangle(rectOutBorder.Location, rectOutBorder.Size);
                        rectButtonBackground.Inflate(-1, -1);

                        g.DrawRectangle(new Pen(TabToolStripColor.ButtonActiveOutBorder), rectOutBorder);
                        g.DrawRectangle(new Pen(TabToolStripColor.ButtonActiveInnerBorder), rectInnerBorder);

                        using (LinearGradientBrush lgBrush = new LinearGradientBrush(rectButtonBackground, TabToolStripColor.ButtonActiveBackgroundTop, TabToolStripColor.ButtonActiveBackgroundBottom, 90))
                        {
                            g.FillRectangle(lgBrush, rectButtonBackground);
                        }
                    }
                    else
                    {
                        using (LinearGradientBrush lgBrush = new LinearGradientBrush(Point.Empty, new Point(0, e.Item.Height),
                            TabToolStripColor.ButtonBackgroundTop, TabToolStripColor.ButtonBackgroundBottom))
                        {
                            g.FillRectangle(lgBrush, new Rectangle(Point.Empty, new Size(e.Item.Width - 1, e.Item == e.ToolStrip.Items[e.ToolStrip.Items.Count - 1] ? e.Item.Height - 1 : e.Item.Height)));
                        }

                        g.DrawLine(Pens.White, 1, 2, e.Item.Width - 2, 2);
                        g.DrawLine(Pens.White, 1, 1, 1, e.Item.Height - 1);
                        g.DrawLine(Pens.White, e.Item.Width - 2, 1, e.Item.Width - 2, e.Item.Height - 1);

                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddRectangle(new Rectangle(Point.Empty, new Size(e.Item.Width - 1, e.Item == e.ToolStrip.Items[e.ToolStrip.Items.Count - 1] ? e.Item.Height - 1 : e.Item.Height)));
                            g.DrawPath(new Pen(TabToolStripColor.BackgroundBorder), path);
                        }
                    }
                }
                else
                {
                    int top = 0;

                    if (e.Item != e.ToolStrip.Items[0])
                    {
                        top++;
                        g.DrawLine(new Pen(TabToolStripColor.BackgroundBorder), 0, 0, e.Item.Width, 0);
                    }

                    Rectangle rectOutBorder = new Rectangle(0, top, e.Item.Width - 1, e.Item.Height - top - 1);
                    Rectangle rectInnerBorder = new Rectangle(0, top, e.Item.Width - 1, e.Item.Height - top - 1);
                    rectInnerBorder.Inflate(-1, -1);
                    Rectangle rectButtonBackground = new Rectangle(rectOutBorder.Location, rectOutBorder.Size);
                    rectButtonBackground.Inflate(-1, -1);

                    g.DrawRectangle(new Pen(TabToolStripColor.ButtonActiveOutBorder), rectOutBorder);
                    g.DrawRectangle(new Pen(TabToolStripColor.ButtonActiveInnerBorder), rectInnerBorder);

                    using (LinearGradientBrush lgBrush = new LinearGradientBrush(rectButtonBackground, TabToolStripColor.ButtonActiveBackgroundTop, TabToolStripColor.ButtonActiveBackgroundBottom, 90))
                    {
                        g.FillRectangle(lgBrush, rectButtonBackground);
                    }
                }

            }







            // 绘制第一个按钮
            
            // 绘制其他按钮

            // 绘制选中按钮



            //if (e.Item is ToolStripButton)
            //{
            //    Graphics g = e.Graphics;
            //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //    ToolStripButton item = e.Item as ToolStripButton;

            //    Rectangle rect = new Rectangle(Point.Empty, item.Size);
            //    rect.Inflate(-1, -1);

            //    //if (e.ToolStrip.Items[0] == e.Item)
            //        g.DrawRectangle(Pens.Coral, rect);

            //}
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if ("ControlText" == e.TextColor.Name)
            {
                if (e.Item.Selected || ((ToolStripButton)e.Item).Checked)
                {
                    e.TextColor = Color.White;
                }
                else
                {
                    e.TextColor = Color.Black;
                }
            }
            base.OnRenderItemText(e);
        }

        private GraphicsPath GetToolStripRectangle(ToolStrip toolStrip)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(0, -1, toolStrip.Width, -1);
            path.AddLine(toolStrip.Width, -1, toolStrip.Width, toolStrip.Height-1);
            path.AddLine(toolStrip.Width, toolStrip.Height, 0, toolStrip.Height);
            path.AddLine(0, toolStrip.Height, 0, 0);

            return path;
        }
    }
}
