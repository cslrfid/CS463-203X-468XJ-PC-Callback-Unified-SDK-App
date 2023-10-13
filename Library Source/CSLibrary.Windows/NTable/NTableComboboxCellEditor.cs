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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace CSLibrary.Windows.UI
{
    public interface INTableComboboxCellEditorModel
    {
        int GetRowCount();

        Object GetValueAt(int rowIndex);

        void SetValueAt(int rowIndex,object oValue);

        bool IsEditable();
    }

    [DesignTimeVisible(false)]
    public class NTableComboboxCellEditor : ComboBox, INTableCellEditor
    {
        INTableComboboxCellEditorModel m_model;

        #region Functions
#if WindowsCE
        [DllImport("coredll.dll")]
        public static extern Boolean SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("coredll.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
#else
        [DllImport("user32.dll")]
        public static extern Boolean SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
#endif

        private void DropDown(bool value)
        {
            SendMessage(this.Handle, 0x14f, value ? -1 : 0, 0);
        }

        private bool IsDropedDown()
        {
            return SendMessage(this.Handle, 0x157, 0, 0);
        }
        #endregion

        public NTableComboboxCellEditor(INTableComboboxCellEditorModel model)
        {
            m_model = model;

            for(int i = 0; i < model.GetRowCount(); i++)
            {
                Items.Add(model.GetValueAt(i));
            }

            if (m_model.IsEditable())
                this.DropDownStyle = ComboBoxStyle.DropDown;
            else
                this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #region ITableCellEditor Members

        public Control getTableCellEditorComponent(NTable table, object value, bool isSelected, int row, int column)
        {
            foreach (Object itemObject in Items)
            {
                if (value == itemObject)
                {
                    this.SelectedItem = itemObject;
                    break;
                }
            }

            return this;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && IsDroped)
            {
                if (this.Items.Count - 1 > this.SelectedIndex)
                    this.SelectedIndex += 1;

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && IsDroped)
            {        
                if (this.SelectedIndex > 0)                
                    this.SelectedIndex -= 1;

                e.Handled = true;
            }
#if WindowsCE
            else if (e.KeyCode == Keys.Enter && !IsDroped)
            {        
                DropDown(true);

                e.Handled = true;
            }
#endif
            else
                base.OnKeyDown(e);
        }
#if WindowsCE
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' && IsDroped)
            {
                e.Handled = true;
            }
            else
                base.OnKeyPress(e); 
        }

        protected override void OnLostFocus(EventArgs e)
        {
            DropDown(false);
        }
#endif
        public object ExtractControlValue(NTable table, int row, int column, Control editorControl)
        {
            return SelectedItem;
        }

        public bool IsTableControl
        {
            get { return true; }
        }

        public bool IsDroped
        {
            get {
#if WindowsCE
                return IsDropedDown();
#else
                return this.DroppedDown;
#endif
            }
        }

        #endregion
    }
}
