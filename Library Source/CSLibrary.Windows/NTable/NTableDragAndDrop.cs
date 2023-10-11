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
    public class NTableDragAndDrop
    {
        Point m_start, m_end;

        public NTableDragAndDrop(int x, int y)
        {
            m_start.X = x;
            m_start.Y = y;
            m_end.X = x;
            m_end.Y = y;
        }

        public Point End
        {
            get { return m_end; }
        }

        public Point Start
        {
            get { return m_start; }
        }

        public int X
        {
            get
            {
                return m_start.X - End.X;
            }
        }

        public int Y
        {
            get
            {
                return m_start.Y - End.Y;
            }
        }

        public void SetEnd(int x, int y)
        {
            m_end.X = x;
            m_end.Y = y;

        }
    }
}
