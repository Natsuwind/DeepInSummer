using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace Himeliya.Controls
{
    public class GraphicsTools
    {
        public static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int left = rectangle.Left;
            int top = rectangle.Top;
            int width = rectangle.Width;
            int height = rectangle.Height;
            int d = radius << 1;

            path.AddArc(left, top, d, d, 180, 90); // 左上原角
            path.AddLine(left + radius, top, left + width - radius, top); // 上边
            path.AddArc(left + width - d, top, d, d, 270, 90); // 右上原角
            path.AddLine(left + width, top + radius, left + width, top + height - radius); // 右边
            path.AddArc(left + width - d, top + height - d, d, d, 0, 90);// 右下角
            path.AddLine(left + width - radius, top + height, left + radius, top + height); // 下边
            path.AddArc(left, top + height - d, d, d, 90, 90); // 左下角
            path.AddLine(left, top + height - radius, left, top + radius); // 左边
            path.CloseFigure();

            return path;
        }

        public static GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int left = rectangle.Left;
            int top = rectangle.Top;
            int width = rectangle.Width;
            int height = rectangle.Height;
            int d = radius << 1;

            path.AddArc(left, top, d, d, 180, 90); // 左上角
            path.AddLine(left + radius, top, left + width - radius, top); // 上边
            path.AddArc(left + width - d, top, d, d, 270, 90); // 右上角
            path.AddLine(left + width, top + radius, left + width, top + height); // 右边
            path.AddLine(left + width, top + height, left, top + width); // 下边
            path.AddLine(left, top + height, left, top + radius); // 左边
            path.CloseFigure();

            return path;
        }

        public static GraphicsPath CreateBottomRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int left = rectangle.Left;
            int top = rectangle.Top;
            int width = rectangle.Width;
            int height = rectangle.Height;
            int d = radius << 1;

            path.AddLine(left + radius, top, left + width - radius, top); // 上边
            path.AddLine(left + width, top + radius, left + width, top + height - radius); // 右边
            path.AddArc(left + width - d, top + height - d, d, d, 0, 90); // 右下角
            path.AddLine(left + width - radius, top + height, left + radius, top + height); // 下边
            path.AddArc(left, top + height - d, d, d, 90, 90); // 左下角
            path.AddLine(left, top + height - radius, left, top + radius);  // 左边
            path.CloseFigure();

            return path;
        }
    }
}
