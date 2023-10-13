using System;
//using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSLibrary.Windows
{
    static class RectangleExtensions
    {
        public static Rectangle ToRectangle(RECT rectangle)
        {
            return Rectangle.FromLTRB(rectangle.left, rectangle.top, rectangle.right, rectangle.bottom);
        }

        public static RectangleF ToRectangleF(RECT rectangle)
        {
            return new RectangleF(rectangle.left, rectangle.top, rectangle.right, rectangle.bottom);
        }
    }
}
