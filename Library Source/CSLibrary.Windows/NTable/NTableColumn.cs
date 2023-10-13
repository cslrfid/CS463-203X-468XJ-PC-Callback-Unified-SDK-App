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
    public class NTableColumn
    {
        #region Constructors
        public NTableColumn() { }

        public NTableColumn(int index)
        {
            Constructor(index, 0, null, null, null);
        }

        public NTableColumn(int index,
            int width)
        {
            Constructor(index, width, null, null, null);
        }


        public NTableColumn(int index, 
            int width,
            INTableCellRenderer cellRenderer)
        {
            Constructor(index, width, cellRenderer, null, null);
        }

        public NTableColumn(int index,
            int width,
            INTableCellRenderer cellRenderer,
            INTableCellEditor cellEditor)
        {
            Constructor(index, width, cellRenderer, cellEditor, null);
        }

        public NTableColumn(int index,
            int width,
            INTableCellRenderer cellRenderer, 
            INTableCellEditor cellEditor,
            INTableCellRenderer headerRenderer) 
        {
            Constructor(index, width, cellRenderer, cellEditor, headerRenderer);
        }

        private void Constructor(int index,
            int width,
            INTableCellRenderer cellRenderer,
            INTableCellEditor cellEditor,
            INTableCellRenderer headerRenderer)
        {
            m_index = index;
            m_width = width;

            m_cellRenderer = cellRenderer;
            m_cellEditor = cellEditor;
            m_headerRenderer = headerRenderer;

            m_minWidth = 15;

            if (m_width < m_minWidth)
            {
                m_allowAutoWidth = true;
                m_width = m_minWidth;
            }
        }
        #endregion

        #region AllowAutoWidth
        private bool m_allowAutoWidth;

        public bool AllowAutoWidth
        {
            get { return m_allowAutoWidth; }
            set { m_allowAutoWidth = value; }
        }
        #endregion

        #region Index
        private int m_index;
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }
        #endregion

        #region Width
        private int m_width;
        public int Width
        {
            get { return m_width; }
            set 
            {
                if (value >= m_minWidth)
                    m_width = value;
                else
                    m_width = m_minWidth;

                m_allowAutoWidth = false;
            }
        }
        #endregion

        #region MinWidth
        private int m_minWidth;
        public int MinWidth
        {
            get { return m_minWidth; }
            set { m_minWidth = value; }
        }
        #endregion

        #region PreferredWidth
        private int m_preferredWidth;
        public int PreferredWidth
        {
            get { return m_preferredWidth; }
            set { m_preferredWidth = value; }
        }
        #endregion

        #region HeaderRenderer
        private INTableCellRenderer m_headerRenderer;

        public INTableCellRenderer HeaderRenderer
        {
            get { return m_headerRenderer; }
            set { m_headerRenderer = value; }
        }
        #endregion

        #region CellRenderer
        private INTableCellRenderer m_cellRenderer;

        public INTableCellRenderer CellRenderer
        {
            get { return m_cellRenderer; }
            set { m_cellRenderer = value; }
        }
        #endregion

        #region CellEditor
        private INTableCellEditor m_cellEditor;

        public INTableCellEditor CellEditor
        {
            get { return m_cellEditor; }
            set { m_cellEditor = value; }
        }
        #endregion

        #region Value
        private Object m_value;

        public Object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
        #endregion

        #region Name
        private String m_name;
        public String Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        #endregion

    }
}
