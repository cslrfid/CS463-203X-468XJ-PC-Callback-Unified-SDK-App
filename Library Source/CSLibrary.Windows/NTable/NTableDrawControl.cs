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
    public abstract class NTableDrawControl
    {
        private Rectangle m_rect;
        private Font m_font;
        private StringFormat m_stringFormat;

        private Color m_foreColor;
        private Color m_backColor;

        private float m_borderWidth = 1;

        private Color m_borderColor = Color.Empty;
        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public StringFormat StringFormat
        {
            get { return m_stringFormat; }
            set { m_stringFormat = value; }
        }

        public Font Font
        {
            get { return m_font; }
            set { m_font = value; }
        }


        public Color BackColor
        {
            get { return m_backColor; }
            set { m_backColor = value; }
        }


        public Color ForeColor
        {
            get { return m_foreColor; }
            set { m_foreColor = value; }
        }




        public float BorderWidth
        {
            get { return m_borderWidth; }
            set { m_borderWidth = value; }
        }




        public Rectangle Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        public Color BorderColor
        {
            get { return m_borderColor; }
            set { m_borderColor = value; }
        }

        public abstract void Draw(Graphics graphics);
    }
}
