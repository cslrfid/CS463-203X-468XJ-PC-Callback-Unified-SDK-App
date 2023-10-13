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
using System.Windows.Forms;
using System.ComponentModel;

namespace CSLibrary.Windows.UI
{
    [DesignTimeVisible(false)]
    public class NTableDefaultCellEditor:TextBox,INTableCellEditor
    {

        bool m_tableControl;

        public NTableDefaultCellEditor() { m_tableControl = true; }
        public NTableDefaultCellEditor(bool tableControl) 
        {
            m_tableControl = tableControl;
        }



        #region ITableCellEditor Members

        public System.Windows.Forms.Control getTableCellEditorComponent(NTable table, 
            object value, 
            bool isSelected, 
            int row, 
            int column)
        {
            Text = value.ToString();
            Multiline = true;

            return this;
        }


        public Object ExtractControlValue(NTable table, int row, int column, Control editorControl)
        {
            Object rv = null;

            Type type = table.Model.GetColumnClass(column);

            if (type.Equals(int.MaxValue.GetType()))
            {
                int iValue = 0;

                if(editorControl.Text != String.Empty)
                iValue = int.Parse(editorControl.Text);

                rv = iValue;
            }
            else if (type.Equals(decimal.MaxValue.GetType()))
            {
                decimal dValue = 0;

                if (editorControl.Text != String.Empty)
                dValue = decimal.Parse(editorControl.Text);

                rv = dValue;
            }
            else
                rv = editorControl.Text;


            return rv;
        }

        public bool IsTableControl
        {
            get { return m_tableControl; }
        }
        #endregion
    }
}
