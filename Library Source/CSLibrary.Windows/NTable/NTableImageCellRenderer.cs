/*
Copyright (C) 2006  Denis Oleynik // denis.oleynik@gmail.com

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

http://sourceforge.net/projects/ntable
http://groups.google.com/group/ntable
http://ntable.blogspot.com

*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSLibrary.Windows.UI
{
    public class NTableImageCellRenderer:NTableDefaultCellRenderer,INTableCellRenderer
    {
        Image m_image;

        bool m_bDrawText = false;

        public bool DrawText
        {
            get { return m_bDrawText; }
            set { m_bDrawText = value; }
        }

        public Image Picture
        {
            get { return m_image; }
            set { m_image = value; }
        }

        public override void Draw(Graphics graphics)
        {

            if(m_image != null)
            {
                graphics.FillRectangle(NTable.GetBrush(BackColor), Rect);

                if (BorderWidth > 0)
                {
                    graphics.DrawRectangle(NTable.GetPen(BorderColor, BorderWidth),
                        Rect);
                }

                /*graphics.DrawImage(m_image,
                     Rect.X + (int)BorderWidth,
                     Rect.Y + (int)BorderWidth);*/

                graphics.DrawImage(m_image,
                    new Rectangle(Rect.X + (int)BorderWidth, 
                    Rect.Y + (int)BorderWidth,
                    m_image.Width,
                    m_image.Height),
                    new Rectangle(0,0,m_image.Width,m_image.Height),
                    GraphicsUnit.Pixel);				



                if(m_bDrawText)
                {
                    int width = Rect.Width - m_image.Width;
                    int x = Rect.X + m_image.Width + (int)BorderWidth;

                    if (Rect.X + Rect.Width > x)
                    {
                        Draw(graphics,
                            m_value.ToString(),
                            new RectangleF(x, Rect.Y, width, Rect.Height));
                    }
                }


            }else
                base.Draw(graphics);
        }
    }
}
