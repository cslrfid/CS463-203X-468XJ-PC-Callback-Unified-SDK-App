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

namespace CSLibrary.Windows.UI
{
    public interface INTableCellEditor
    {
        /// <summary>
        /// Returns control
        /// </summary>
        /// <param name="table"></param>
        /// <param name="value"></param>
        /// <param name="isSelected"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        Control getTableCellEditorComponent(NTable table, Object value,
                          bool isSelected,
                          int row, int column);




        /// <summary>
        /// Returns value from control
        /// with type equal with type of model for this column and cell
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="editorControl"></param>
        /// <returns></returns>
        Object ExtractControlValue(NTable table, int row, int column, Control editorControl);

        /// <summary>
        /// Its means what control will be registered
        /// as part of the table and will be used for 
        /// cell replacement when edit start
        /// </summary>
        bool IsTableControl
        {
            get;
        }
    }
}
