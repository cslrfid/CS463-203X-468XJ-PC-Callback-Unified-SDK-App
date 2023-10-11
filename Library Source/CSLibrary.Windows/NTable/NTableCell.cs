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

namespace CSLibrary.Windows.UI
{
    public class NTableCell
    {
        #region ColumnIndex
        int m_columnIndex;

        public int ColumnIndex
        {
            get { return m_columnIndex; }
            set { m_columnIndex = value; }
        }
        #endregion

        #region RowIndex
        int m_rowIndex;

        public int RowIndex
        {
            get { return m_rowIndex; }
            set { m_rowIndex = value; }
        }
        #endregion

        #region Contructors
        public NTableCell() { }

        public NTableCell(int rowIndex, int columnIndex)
        {
            m_rowIndex = rowIndex;
            m_columnIndex = columnIndex;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return String.Format("Row:{0} Column:{1}",
                m_rowIndex, m_columnIndex);
        }
        #endregion
    }
}
