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
#if TEST_MODEL
using System;
using System.Collections.Generic;
using System.Text;


namespace CSLibrary.Windows.UI
{
	public abstract class NTableListModel<T>:INTableModel
	{
		protected List<T> m_list;

		public NTableListModel()
		{
			m_list = new List<T>();
		}

		public NTableListModel(List<T> list)
		{
			m_list = list;
		}


		protected String GetColumnNameError(int columnIndex)
		{
			throw new Exception("Invalid column number " + columnIndex);
		}


		protected Type GetColumnClassError(int columnIndex)
		{
			throw new Exception("Invalid column number " + columnIndex);
		}

		public Object GetValueAtError(int rowIndex, int columnIndex)
		{
 			throw new Exception("Invalid column or row number ");
		}

		public T GetObject(int rowIndex)
		{
			return m_list[rowIndex];
		}

		#region INTableModel Members

        public virtual int GetRowCount()
        {
            return m_list.Count;
        }

        public abstract int GetColumnCount();

        public abstract string GetColumnName(int columnIndex);

        public abstract Type GetColumnClass(int columnIndex);

        public abstract Object GetValueAt(int rowIndex, int columnIndex);

		public virtual bool IsCellEditable(int rowIndex, int columnIndex)
		{
			return false;
		}

		public virtual void SetValueAt(object aValue, int rowIndex, int columnIndex)
		{
			throw new Exception("The method or operation is not implemented.");
		}

        public virtual Object GetObjectAt(int rowIndex, int columnIndex)
        {
            return GetObject(rowIndex);
        }

        public event TableModelChangeHandler Change;

        protected void FireChangeEvent()
        {
            if (Change != null)
                Change.Invoke();
        }

        #endregion

        public int Add(T item)
        {
            m_list.Insert(0, item);

            if (Change != null)
                Change.Invoke();

            return 0;
        }

        public int Add(List<T> itemsList)
        {
            m_list.AddRange(itemsList);

            if (Change != null)
                Change.Invoke();

            return 0;
        }
}
}
#endif