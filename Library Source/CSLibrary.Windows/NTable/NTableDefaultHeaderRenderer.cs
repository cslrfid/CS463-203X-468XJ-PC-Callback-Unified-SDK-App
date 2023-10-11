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
    public class NTableDefaultHeaderRenderer:NTableDefaultCellRenderer
    {
        public override NTableDrawControl getTableCellRendererComponent(NTable table, object value, bool isSelected, bool hasFocus, int row, int column)
        {
            if (table.LeftHeader && row == -1 && column == 0)
            {
                this.BackColor = table.BackColor;
                this.ForeColor = table.BackColor;
                this.BorderColor = table.BackColor;
                this.Font = table.ColumnFont;
                this.StringFormat = table.DefaultStringFormat;
                this.m_value = String.Empty;
            }
            else
            {
                this.BackColor = table.ColumnBackColor;
                this.ForeColor = table.ColumnForeColor;
                this.BorderColor = table.BorderColor;
                this.Font = table.ColumnFont;
                this.StringFormat = table.DefaultStringFormat;

                this.m_value = value;
            }

            return this;
        }
    }
}
