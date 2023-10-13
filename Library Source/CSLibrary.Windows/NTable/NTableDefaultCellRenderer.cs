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
using System.Drawing;

using CSLibrary.Windows;

namespace CSLibrary.Windows.UI
{
    public class NTableDefaultCellRenderer:NTableDrawControl,INTableCellRenderer
    {
        protected Object m_value;

        public NTableDefaultCellRenderer()
        {

        }

        #region ITableCellRenderer Members

        public virtual NTableDrawControl getTableCellRendererComponent(NTable table, object value, bool isSelected, bool hasFocus, int row, int column)
        {
            this.BorderColor = table.BorderColor;
            this.Font = table.Font;

            if (!hasFocus)
            {

                if (this.IsSelected = isSelected)
                {
                    ForeColor = table.SelectionForeColor;
                    BackColor = table.SelectionBackColor;
                }
                else
                {
                    BackColor = (row % 2) == 0 ? table.BackColor : table.AltBackColor;
                    ForeColor = (row % 2) == 0 ? table.ForeColor : table.AltForeColor;
                }
            }
            else
            {
                    ForeColor = table.FocusCellForeColor;
                    BackColor = table.FocusCellBackColor; 
            }

            BorderWidth = 1;


            m_value = value;

            if (this.StringFormat == null)
                this.StringFormat = table.DefaultStringFormat;


            return this;
        }

        #endregion

        public override void Draw(Graphics graphics)
        {
            //use gradient fill instead
            /*if (IsSelected)
                GdiPlus.Fill(graphics, Rect, BackColor, Color.White, GdiPlus.FillDirection.TopToBottom);
            else*/
                graphics.FillRectangle(NTable.GetBrush(BackColor), Rect); ;
            if (BorderWidth > 0)
            {
                graphics.DrawRectangle(NTable.GetPen(BorderColor, BorderWidth),
                    Rect);
            }

            Draw(graphics, m_value.ToString(), (RectangleF)Rect);
        }

        protected void Draw(Graphics graphics, String text, RectangleF rect)
        {
            if (StringFormat.Alignment == StringAlignment.Far)
            {
                rect.Width -= 5;
            }
            else if (StringFormat.Alignment == StringAlignment.Near)
            {
                rect.X += 5;
            }

            graphics.DrawString(m_value.ToString(), this.Font,
                NTable.GetBrush(ForeColor),
                rect, 
                this.StringFormat);
        }
    }
}
