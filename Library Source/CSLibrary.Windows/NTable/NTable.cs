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

#if DEBUG
//#define DEBUG_TABLE_LEVEL_3
//#define DEBUG_TABLE_LEVEL_1
//#define DEBUG_TABLE_LEVEL_2
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace CSLibrary.Windows.UI
{


    public enum NTableSplitterMode
    {
        ResizeByRect,
        Default
    }

    public delegate void NTableRowHandler(int rowIndex);
    public delegate void NTableSelectionHandler();
    public delegate void NTableCellValueHandler(NTableCell cell);

    public class NTable : Control
    {

        private enum NTableMode
        {
            Interactive,
            ApplyEdit,
            BeginEdit
        }

        #region Events
        public event NTableRowHandler RowChanged;
        public event NTableSelectionHandler SelectionChanged;
        public event NTableCellValueHandler CellFocusChanged;
        public event NTableCellValueHandler CellValueChanged;
        public event NTableCellValueHandler CellValueStartEdit;
        public event NTableCellValueHandler CellValueEndEdit;
        public event NTableCellValueHandler MoveCell;
#if WIN32       
        public new event CellValueHandler Enter;
#else
        public NTableCellValueHandler Enter;
#endif
        #endregion

        #region Private fields
        NTableMode m_mode = NTableMode.Interactive;

        Dictionary<int, NTableColumn> m_columns = new Dictionary<int, NTableColumn>();

        int m_rowCount;
        int m_columnCount;

        INTableModel m_model;


        HScrollBarEx m_hScrollBar = new HScrollBarEx();
        VScrollBarEx m_vScrollBar = new VScrollBarEx();

        static INTableCellRenderer m_defaultHeaderRenderer;
        static INTableCellRenderer m_defaultCellRenderer;

        List<Color> m_colorList = new List<Color>();
        List<Color> m_greyscaleList = new List<Color>();

        StringFormat m_defaultStringFormat;

        int m_defaultRowHeight = 20;

        int m_canvasWidth;
        int m_canvasHeight;

        int m_virtualX;
        int m_virtualY;

        int m_visibleHeight;
        int m_visibleWidth;

        const int m_hScrollHeight = 20;
        const int m_vScrollWidth = 20;

        int m_avgColumnSize;

        int m_firstRow;
        int m_firstColumn;

        int m_currentMouseX = 0;
        int m_currentMouseY = 0;

        int m_visibleRowCount = 0;
        int m_invisibleColumnCount = 0;

        NTableCell m_currentCell;
        Control m_currentEditor;

        NTableDragAndDrop m_drag;

        private List<int> m_selectedRows = new List<int>();
        #endregion

        #region Constructors

        public NTable()
        {
            Initialize();

            BindModel(new TestModel());
        }

        public NTable(INTableModel tableModel)
        {
            Initialize();

            BindModel(tableModel);
        }
        #endregion

        #region Initialize
        void Initialize()
        {
            BackColor = System.Drawing.Color.White;

            Controls.Add(m_hScrollBar);
            Controls.Add(m_vScrollBar);

            m_defaultStringFormat = new StringFormat();
            m_defaultStringFormat.Alignment = StringAlignment.Center;
            m_defaultStringFormat.LineAlignment = StringAlignment.Center;
            m_defaultStringFormat.FormatFlags = StringFormatFlags.NoWrap;

            m_columnFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);

            m_hScrollBar.ValueChanged += new EventHandler(OnScroll);
            m_vScrollBar.ValueChanged += new EventHandler(OnScroll);

            m_bAutoMoveRow = true;

        }
        #endregion

        #region Focus

        #region OnGotFocus
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }
        #endregion

        #region OnLostFocus
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }
        #endregion

        #endregion

        #region Key, Mouse handlers

        #region IsInputKey
#if WIN32

        protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }
#endif
        #endregion

        #region OnClick
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

#if WindowsCE
            Focus();
#else
            Select();
#endif

            NTableCell cell = HitTest(m_currentMouseX, m_currentMouseY);

            if (cell.RowIndex > -1)
                FireCellClick(cell.RowIndex, cell.ColumnIndex);
        }

        #endregion

        #region OnMouseDown
        protected override void OnMouseDown(MouseEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnMouseDown");
#endif

            base.OnMouseDown(e);

            m_currentMouseX = e.X;
            m_currentMouseY = e.Y;

            NTableCell cell = HitTest(m_currentMouseX, m_currentMouseY);


            if (cell.RowIndex == -1)
            {

#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                if (AllowColumnResize)
                    m_drag = new NTableDragAndDrop(e.X, e.Y);

                Invalidate();

                return;
            }


        }

        #endregion

        #region OnMouseMove
        protected override void OnMouseMove(MouseEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnMouseMove");
#endif

            base.OnMouseMove(e);

            if (m_drag != null)
            {
                m_drag.SetEnd(e.X, e.Y);

                Invalidate();
            }

        }
        #endregion

        #region OnMouseUp
        protected override void OnMouseUp(MouseEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnMouseUp");
#endif

            base.OnMouseUp(e);

            if (m_drag != null)
            {
                m_drag.SetEnd(e.X, e.Y);

                ApplyDragDrop();

            }

        }
        #endregion

        #region OnKeyPress

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::OnKeyPress {0}",
                e.KeyChar));
#endif
            base.OnKeyPress(e);

            if (e.Handled || e.KeyChar != '\r')
                return;

            if (m_model == null)
                return;

            int rowIndex = m_currentCell != null ? m_currentCell.RowIndex : 0;
            int columnIndex = m_currentCell != null ? m_currentCell.ColumnIndex : 0;

            FireCellClick(rowIndex, columnIndex);
            FireCellEnter(rowIndex, columnIndex);

            e.Handled = true;

            Select(rowIndex, columnIndex);

        }

        #endregion

        #region OnKeyDown

        protected override void OnKeyDown(KeyEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::OnKeyDown {0}",
                e.KeyData));
#endif
            base.OnKeyDown(e);

            if (e.Handled)
                return;

            if (m_model == null)
                return;

            int rowIndex = m_currentCell != null ? m_currentCell.RowIndex : 0;
            int columnIndex = m_currentCell != null ? m_currentCell.ColumnIndex : 0;

            switch (e.KeyData)
            {
                case Keys.Up:
                    if (rowIndex > 0)
                        --rowIndex;
                    break;
                case Keys.Down:
                    if (rowIndex < Model.GetRowCount() - 1)
                        ++rowIndex;
                    break;
                case Keys.Left:
                    if (columnIndex > 0)
                        --columnIndex;
                    break;
                case Keys.Right:
                    if (columnIndex < Model.GetColumnCount() - 1)
                        ++columnIndex;
                    break;
                case Keys.PageUp:
                    rowIndex = m_firstRow - (m_visibleHeight / m_defaultRowHeight) + 1;
                    break;
                case Keys.PageDown:
                    rowIndex = m_firstRow + m_visibleRowCount - 1;
                    break;
                case Keys.Enter:
                    break;
                default:
                    return;
            }

            e.Handled = true;

            Select(rowIndex, columnIndex);
        }

        #endregion

        #region GetCurrentCell
        /// <summary>
        /// Safe current cell.
        /// </summary>
        /// <returns></returns>
        private NTableCell GetCurrentCell()
        {

            if (m_currentCell != null)
                return m_currentCell;

            return new NTableCell(-1, -1);
        }
        #endregion

        #region OnDoubleClick
        protected override void OnDoubleClick(EventArgs e)
        {
            NTableCell tableCell = GetCurrentCell();

            FireCellEnter(tableCell.RowIndex, tableCell.ColumnIndex);
        }

        #endregion

        #region FireCellEnter

        void FireCellEnter(int rowIndex, int columnIndex)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::FireCellEnter Row:{0}, Column:{1}",
                rowIndex, columnIndex));
#endif
            if (Enter != null)
                Enter.Invoke(new NTableCell(rowIndex, columnIndex));
        }

        #endregion

        #region FireCellClick
        void FireCellClick(int rowIndex, int columnIndex)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::FireCellClick Row:{0}, Column:{1}",
                rowIndex,columnIndex));
#endif
            OnCellClick(rowIndex, columnIndex);
        }
        #endregion

        #region OnCellClick
        void OnCellClick(int rowIndex, int columnIndex)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::OnCellClick Row:{0}, Column:{1}",
                rowIndex, columnIndex));
#endif
            if (m_rowCount == 0)
                return;

            NTableCell cell = new NTableCell(rowIndex, columnIndex);

            ApplyEdit();

            if (Model.IsCellEditable(cell.RowIndex,
                cell.ColumnIndex))
            {
                BeginEdit(cell);
            }

            if (m_multipleSelection)
            {
                if (m_selectedRows.Contains(cell.RowIndex))
                    m_selectedRows.Remove(cell.RowIndex);
                else
                    m_selectedRows.Add(cell.RowIndex);

                if (SelectionChanged != null)
                {
                    SelectionChanged.Invoke();
                }
            }

            NTableCell oldCell = m_currentCell;

            m_currentCell = cell;

            Invalidate();

            if (oldCell == null
                || cell.RowIndex != oldCell.RowIndex)
            {
                if (RowChanged != null)
                {
                    RowChanged.Invoke(cell.RowIndex);
                }
            }
        }
        #endregion

        #region ApplyDragDrop
        void ApplyDragDrop()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::ApplyDragDrop");
#endif
            NTableCell cell = HitTest(m_drag.Start.X, m_drag.Start.Y);

            NTableColumn column = m_columns[cell.ColumnIndex];

            int columnWidth = 0;

            if (m_splitterMode == NTableSplitterMode.ResizeByRect)
                columnWidth = m_drag.X > 0 ? m_drag.X : m_drag.X * -1;
            else
                columnWidth = column.Width - m_drag.X;


            column.Width = columnWidth;

            m_drag = null;

            RecalculateSize();

            Invalidate();
        }
        #endregion

        #endregion

        #region Edit

        #region BeginEdit
        public void BeginEdit()
        {
            BeginEdit(m_currentCell);
        }

        public void BeginEdit(int rowIndex, int columnIndex)
        {
            BeginEdit(new NTableCell(rowIndex, columnIndex));
        }

        public void BeginEdit(NTableCell cell)
        {

#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::BeginEdit {0}",cell));
#endif

            if (cell == null)
                return;

            if (Model == null
                || Model.GetRowCount() == 0
                || !Model.IsCellEditable(cell.RowIndex, cell.ColumnIndex))
                return;

            Rectangle rect = GetCellRect(cell.ColumnIndex, cell.RowIndex);

            if (m_mode != NTableMode.Interactive)
                return;

            m_mode = NTableMode.BeginEdit;

            m_currentEditor = m_columns[cell.ColumnIndex].CellEditor.getTableCellEditorComponent(
                this,
                Model.GetValueAt(cell.RowIndex, cell.ColumnIndex),
                true,
                cell.RowIndex,
                cell.ColumnIndex);

            if (m_columns[cell.ColumnIndex].CellEditor.IsTableControl)
            {
                m_currentEditor.Bounds = rect;
                Controls.Add(m_currentEditor);
            }

            if (m_currentEditor is TextBox)
                ((TextBox)m_currentEditor).SelectAll();

            if (CellValueStartEdit != null)
                CellValueStartEdit.Invoke(cell);

            m_currentEditor.Show();
#if WindowsCE
            m_currentEditor.Focus();
#else
            m_currentEditor.Select();
#endif
            m_currentEditor.LostFocus += new EventHandler(OnEditorLostFocus);
            m_currentEditor.KeyDown += new KeyEventHandler(OnEditorKeyDown);
            m_currentEditor.KeyPress += new KeyPressEventHandler(OnEditorKeyPress);

            if (m_currentCell == null
                || cell.RowIndex != m_currentCell.RowIndex)
            {

                m_currentCell = cell;

                if (RowChanged != null)
                    RowChanged.Invoke(cell.RowIndex);
            }else
                m_currentCell = cell;

            m_mode = NTableMode.Interactive;
        }
        #endregion

        #region OnEditorKeyPress
        void OnEditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;

#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                if (m_bAutoMoveRow)
                {
                    if (Select(m_currentCell.RowIndex + 1, m_currentCell.ColumnIndex))
                        BeginEdit(m_currentCell);
                }
                else if (MoveCell != null)
                    MoveCell.Invoke(m_currentCell);
            }


        }
        #endregion

        #region OnEditorKeyDown

        void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::OnEditorKeyDown {0}", e.KeyData));
#endif

            if (e.KeyData == Keys.Down)
            {
                e.Handled = true;

#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                if (Select(m_currentCell.RowIndex + 1, m_currentCell.ColumnIndex))
                    BeginEdit(m_currentCell);

            }
            else if (e.KeyData == Keys.Up)
            {
                e.Handled = true;
#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                if (Select(m_currentCell.RowIndex - 1, m_currentCell.ColumnIndex))
                    BeginEdit(m_currentCell);

            }
            else if (e.KeyData == Keys.PageUp)
            {
                e.Handled = true;
#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                MoveBackPage();

                BeginEdit();

            }
            else if (e.KeyData == Keys.PageDown)
            {
                e.Handled = true;
#if WindowsCE
                Focus();
#else
                Select();
#endif

                ApplyEdit();

                MoveNextPage();

                BeginEdit();

            }
            else if (e.KeyData == Keys.Escape)
            {
                e.Handled = true;

#if WIN32
                e.SuppressKeyPress = true;
#endif

                m_mode = NTableMode.ApplyEdit;

                INTableCellEditor editor = m_columns[m_currentCell.ColumnIndex].CellEditor;

                CloseEditor(editor);

                m_mode = NTableMode.Interactive;

                Focus();
            }
        }

        #endregion

        #region OnEditorLostFocus
        void OnEditorLostFocus(object sender, EventArgs e)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine("NTable::OnEditorLostFocus");
#endif
            ApplyEdit();
        }
        #endregion

        #region ApplyEdit
        public void ApplyEdit()
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::ApplyEdit {0}",m_currentCell));
#endif
            if (m_currentEditor == null)
                return;

            if (m_mode != NTableMode.Interactive)
                return;

            m_mode = NTableMode.ApplyEdit;

            INTableCellEditor editor = m_columns[m_currentCell.ColumnIndex].CellEditor;

            bool error = false;

            try
            {
                object value = editor.ExtractControlValue(this, m_currentCell.RowIndex, m_currentCell.ColumnIndex,
                    m_currentEditor);

                if (value != null
                    && !value.Equals(Model.GetValueAt(m_currentCell.RowIndex,
                    m_currentCell.ColumnIndex)))
                {

                    Model.SetValueAt(value,
                        m_currentCell.RowIndex,
                        m_currentCell.ColumnIndex);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                error = true;
            }

            CloseEditor(editor);

            if (!error && CellValueChanged != null)
                CellValueChanged.Invoke(m_currentCell);

            m_mode = NTableMode.Interactive;

            if (CellValueEndEdit != null)
                CellValueEndEdit.Invoke(m_currentCell);
        }
        #endregion

        #region CloseEditor
        private void CloseEditor(INTableCellEditor editor)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine("NTable::CloseEditor");
#endif
            if (editor.IsTableControl)
            {

                m_currentEditor.Visible = false;
                m_currentEditor.KeyDown -= new KeyEventHandler(OnEditorKeyDown);
                m_currentEditor.LostFocus -= new EventHandler(OnEditorLostFocus);
                m_currentEditor.KeyPress -= new KeyPressEventHandler(OnEditorKeyPress);

                Controls.Remove(m_currentEditor);

            }

            m_currentEditor = null;

        }
        #endregion

        #endregion

        #region Resize

        #region OnResize
        protected override void OnResize(EventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnResize");
#endif

            RecalculateSize();

            Invalidate();
        }
        #endregion

        #region RefreshColumnSize
        void RefreshColumnSize()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::RefreshColumnSize");
#endif

            int columnCount = m_model.GetColumnCount();

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                NTableColumn tableColumn = GetColumn(columnIndex);

                if (tableColumn.Value == null)
                {
                    tableColumn.Value = Model.GetColumnName(columnIndex);
                }

                if (tableColumn.HeaderRenderer == null)
                    tableColumn.HeaderRenderer = DefaultHeaderRenderer;

                if (tableColumn.CellRenderer == null)
                    tableColumn.CellRenderer = DefaultCellRenderer;

                if (tableColumn.CellEditor == null)
                    tableColumn.CellEditor = DefaultCellEditor;

            }

            if (m_autoColumnSize)
            {
                int width = m_canvasHeight < Height ? Width : Width - m_hScrollHeight;
                int unassignColumnCount = 0;
                int assignWidth = 0;
                //int columnCount = Model.GetColumnCount();


                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    if (m_columns[columnIndex].AllowAutoWidth)
                        ++unassignColumnCount;
                    else
                        assignWidth += m_columns[columnIndex].Width;
                }

                if (unassignColumnCount > 0)
                {
                    int columnWidth = (width - assignWidth) / unassignColumnCount;

                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        if (m_columns[columnIndex].AllowAutoWidth)
                        {
                            m_columns[columnIndex].Width = columnWidth;
                            m_columns[columnIndex].AllowAutoWidth = true;
                        }
                    }
                }

            }
        }
        #endregion

        #region RecalculateSize
        void RecalculateSize()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::RecalculateSize");
#endif
            m_canvasHeight = Model.GetRowCount() * m_defaultRowHeight + m_defaultRowHeight /* Column Height */;

            RefreshColumnSize();

            m_currentMouseX = 0;
            m_currentMouseY = 0;
            m_virtualX = 0;
            m_virtualY = 0;

            m_canvasWidth = 0;
            m_invisibleColumnCount = 0;

            for (int i = 0; i < Model.GetColumnCount(); i++)
            {
                m_canvasWidth += m_columns[i].Width;
                if (m_canvasWidth > Width)
                    m_invisibleColumnCount++;
            }
            m_avgColumnSize = m_canvasWidth / Model.GetColumnCount();

            m_visibleWidth = Width;
            m_visibleHeight = Height;

            if (m_canvasWidth > Width)
                m_visibleHeight -= m_vScrollWidth;
            else
                m_firstColumn = 0;

            if (m_canvasHeight > Height)
                m_visibleWidth -= m_hScrollHeight;
            else
                m_firstRow = 0;

            ShowHScroll(m_visibleWidth < m_canvasWidth);
            ShowVScroll(m_visibleHeight < m_canvasHeight);

            if (m_currentEditor != null)
            {
                if (m_columns[m_currentCell.ColumnIndex].CellEditor.IsTableControl)
                {
                    m_currentEditor.Bounds = GetCellRect(m_currentCell.ColumnIndex,
                        m_currentCell.RowIndex);
                }
            }

            if (m_currentCell != null)
            {
                Select(m_currentCell.RowIndex, m_currentCell.ColumnIndex);
            }
        }
        #endregion

        #endregion

        #region Properties

        #region LeftHeader
        bool m_leftHeader;

        public bool LeftHeader
        {
            get { return m_leftHeader; }
            set { m_leftHeader = value; }
        }
        #endregion

        #region DrawGridBorder
        bool m_drawGridBorder = true;

        public bool DrawGridBorder
        {
            get { return m_drawGridBorder; }
            set { m_drawGridBorder = value; }
        }
        #endregion

        #region AllowColumnResize
        bool m_allowColumnResize;

        public bool AllowColumnResize
        {
            get { return m_allowColumnResize; }
            set { m_allowColumnResize = value; }
        }
        #endregion

        #region CurrentRowIndex
        public int CurrentRowIndex
        {
            get
            {
                if (m_currentCell != null)
                    return m_currentCell.RowIndex;

                return -1;
            }
        }
        #endregion

        #region CurrentColumnIndex
        public int CurrentColumnIndex
        {
            get
            {
                if (m_currentCell != null)
                    return m_currentCell.ColumnIndex;

                return -1;
            }
        }
        #endregion

        #region AutoColumnSize
        bool m_autoColumnSize = true;
        public bool AutoColumnSize
        {
            get { return m_autoColumnSize; }
            set { m_autoColumnSize = value; }
        }
        #endregion

        #region ShowSplitterValue
        bool m_showSplitterValue = true;
        public bool ShowSplitterValue
        {
            get { return m_showSplitterValue; }
            set { m_showSplitterValue = value; }
        }
        #endregion

        #region SplitterMode
        NTableSplitterMode m_splitterMode = NTableSplitterMode.Default;
        public NTableSplitterMode SplitterMode
        {
            get { return m_splitterMode; }
            set { m_splitterMode = value; }
        }
        #endregion

        #region ShowStartSplitter
        bool m_showStartSplitter = true;

        public bool ShowStartSplitter
        {
            get { return m_showStartSplitter; }
            set { m_showStartSplitter = value; }
        }
        #endregion

        #region SplitterWidth
        int m_splitterWidth = 1;
        public int SplitterWidth
        {
            get { return m_splitterWidth; }
            set { m_splitterWidth = value; }
        }
        #endregion

        #region SplitterColor
        Color m_splitterColor = Color.Red;
        public Color SplitterColor
        {
            get { return m_splitterColor; }
            set { m_splitterColor = value; }
        }
        #endregion

        #region SplitterStartColor
        Color m_splitterStartColor = Color.Brown;
        public Color SplitterStartColor
        {
            get { return m_splitterStartColor; }
            set { m_splitterStartColor = value; }
        }
        #endregion

        #region ColumnFont
        Font m_columnFont;
        public Font ColumnFont
        {
            get { return m_columnFont; }
            set { m_columnFont = value; }
        }
        #endregion

        #region ColumnBackColor
        Color m_columnBackColor = System.Drawing.Color.Chocolate;
        public Color ColumnBackColor
        {
            get { return m_columnBackColor; }
            set { m_columnBackColor = value; }
        }
        #endregion

        #region ColumnForeColor
        Color m_columnForeColor = System.Drawing.Color.White;
        public Color ColumnForeColor
        {
            get { return m_columnForeColor; }
            set { m_columnForeColor = value; }
        }
        #endregion

        #region DefaultRowHeight
        public int DefaultRowHeight
        {
            get { return m_defaultRowHeight; }
            set { m_defaultRowHeight = value; }
        }
        #endregion

        #region DefaultTextAligment
        /// <summary>
        /// Gets or sets the text alignment information on the vertical plane.
        /// </summary>
        public StringAlignment DefaultTextAligment
        {
            get
            {
                return m_defaultStringFormat.Alignment;
            }
            set
            {
                m_defaultStringFormat.Alignment = value;
            }
        }
        #endregion

        #region DefaultLineAligment
        /// <summary>
        /// Gets or sets the line alignment on the horizontal plane.
        /// </summary>
        public StringAlignment DefaultLineAligment
        {
            get
            {
                return m_defaultStringFormat.LineAlignment;
            }
            set
            {
                m_defaultStringFormat.LineAlignment = value;
            }
        }
        #endregion

        #region DefaultStringFormat
        internal StringFormat DefaultStringFormat
        {
            get { return m_defaultStringFormat; }
            set { m_defaultStringFormat = value; }
        }
        #endregion

        #region AltBackColor
        Color m_altBackColor = System.Drawing.Color.Khaki;
        public Color AltBackColor
        {
            get { return m_altBackColor; }
            set { m_altBackColor = value; }
        }
        #endregion

        #region AltForeColor
        Color m_altForeColor = Color.Black;
        public Color AltForeColor
        {
            get { return m_altForeColor; }
            set { m_altForeColor = value; }
        }
        #endregion

        #region BorderColor
        Color m_borderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        public Color BorderColor
        {
            get { return m_borderColor; }
            set { m_borderColor = value; }
        }
        #endregion

        #region SelectionForeColor
        Color m_selectionForeColor = System.Drawing.Color.Black;
        public Color SelectionForeColor
        {
            get { return m_selectionForeColor; }
            set { m_selectionForeColor = value; }
        }
        #endregion

        #region SelectionBackColor
        Color m_selectionBackColor = System.Drawing.Color.DarkOrange;
        public Color SelectionBackColor
        {
            get { return m_selectionBackColor; }
            set { m_selectionBackColor = value; }
        }
        #endregion

        #region FocusCellForeColor
        Color m_focusCellForeColor = Color.White;
        public Color FocusCellForeColor
        {
            get
            {
                return m_focusCellForeColor;
            }
            set
            {
                m_focusCellForeColor = value;
            }
        }
        #endregion

        #region FocusCellBackColor
        Color m_focusCellBackColor = Color.Black;
        public Color FocusCellBackColor
        {
            get
            {
                return m_focusCellBackColor;
            }
            set
            {
                m_focusCellBackColor = value;
            }
        }
        #endregion

        #region MultipleSelection
        private bool m_multipleSelection;

        public bool MultipleSelection
        {
            get { return m_multipleSelection; }
            set { m_multipleSelection = value; }
        }
        #endregion

        #region Model
        public INTableModel Model
        {
            get
            {
                return m_model;
            }
        }
        #endregion

        #region AutoMoveRow
        bool m_bAutoMoveRow;
        /// <summary>
        /// Automatically moved on the next row when enter pressed on the edit control.
        /// </summary>
        public bool AutoMoveRow
        {
            get { return m_bAutoMoveRow; }
            set { m_bAutoMoveRow = value; }
        }

        #endregion

        #endregion

        #region Scroll bar

        #region ShowHScroll
        void ShowHScroll(bool bShow)
        {
            m_hScrollBar.Visible = bShow;

            if (bShow)
            {
                m_hScrollBar.Top = m_visibleHeight;
                m_hScrollBar.Left = 0;
                m_hScrollBar.Width = m_visibleWidth;
                m_hScrollBar.Height = m_vScrollWidth;

                m_hScrollBar.Minimum = 0;
                m_hScrollBar.Maximum = m_canvasWidth;
                m_hScrollBar.LargeChange = m_avgColumnSize * (m_invisibleColumnCount + 1);
                m_hScrollBar.SmallChange = m_hScrollBar.LargeChange;
            }
        }
        #endregion

        #region ShowVScroll
        void ShowVScroll(bool bShow)
        {
            m_vScrollBar.Visible = bShow;

            if (bShow)
            {
                m_vScrollBar.Top = 0;
                m_vScrollBar.Left = m_visibleWidth;
                m_vScrollBar.Height = m_visibleHeight;
                m_vScrollBar.Width = 20;

                m_vScrollBar.Minimum = 0;
                m_vScrollBar.Maximum = m_canvasHeight;

                m_vScrollBar.LargeChange = m_visibleHeight - m_defaultRowHeight > 0 ?
                    m_visibleHeight - m_defaultRowHeight : m_defaultRowHeight;
                m_vScrollBar.SmallChange = m_defaultRowHeight;
            }
        }
        #endregion

        #region OnScroll
        void OnScroll(object sender, EventArgs e)
        {
            if (sender == m_vScrollBar)
            {
                int scrollRow = m_vScrollBar.Value / DefaultRowHeight;

                if (scrollRow > Model.GetRowCount())
                    m_firstRow = Model.GetRowCount() - 1;
                else
                    m_firstRow = scrollRow;


                m_virtualY = m_firstRow * DefaultRowHeight;
            }
            else
            {
                Trace.Assert(m_avgColumnSize != 0);

                int scrollColumn = m_hScrollBar.Value / m_avgColumnSize / (m_invisibleColumnCount + 1);

                if (scrollColumn > m_columns.Count)
                    m_firstColumn = m_columns.Count - 1;
                else
                    m_firstColumn = scrollColumn;

                m_virtualX = m_firstColumn * m_avgColumnSize;

            }


            if (m_currentEditor != null)
            {
                if (m_columns[m_currentCell.ColumnIndex].CellEditor.IsTableControl)
                {
                    /*
                    m_currentEditor.Bounds = GetCellRect(m_currentCell.ColumnIndex,
                        m_currentCell.RowIndex);*/

                    ApplyEdit();
                }
            }

            Invalidate();
        }
        #endregion

        #region IsPairScrollShow
        bool IsPairScrollShow
        {
            get
            {
                return m_hScrollBar.Visible && m_vScrollBar.Visible;
            }
        }
        #endregion

        #endregion

        #region Model

        #region BindModel
        public void BindModel(INTableModel model)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::BindModel");
#endif

            if (model == null)
                throw new Exception("Model can not be null");

            ApplyEdit();

            m_model = model;

            m_model.Change += new TableModelChangeHandler(OnModelChange);

            OnTableSettingsChanged();

        }
        #endregion

        #region OnModelChange
        private void OnModelChange()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnModelChange");
#endif
            if (m_model.GetRowCount() != m_rowCount
                || m_model.GetColumnCount() != m_columnCount)
            {
                m_rowCount = m_model.GetRowCount();
                m_columnCount = m_model.GetColumnCount();

                RecalculateSize();
            }

            m_selectedRows.Clear();

            Invalidate();
        }
        #endregion

        #region OnTableSettingsChanged
        void OnTableSettingsChanged()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnTableSettingsChanged");
#endif

            m_currentCell = null;
            m_avgColumnSize = 0;
            m_firstColumn = 0;
            m_firstRow = 0;
            m_virtualX = 0;
            m_virtualY = 0;

            m_hScrollBar.Value = 0;
            m_vScrollBar.Value = 0;

            m_invisibleColumnCount = 0;

            m_rowCount = m_model.GetRowCount();
            m_selectedRows.Clear();

            RecalculateSize();

            Invalidate();
        }
        #endregion

        #endregion

        #region Default Renderers and Editors

        public static INTableCellRenderer DefaultCellRenderer
        {
            get
            {
                if (m_defaultCellRenderer == null)
                {
                    m_defaultCellRenderer = new NTableDefaultCellRenderer();
                }

                return m_defaultCellRenderer;
            }
        }

        public static INTableCellRenderer DefaultHeaderRenderer
        {
            get
            {
                if (m_defaultHeaderRenderer == null)
                {
                    m_defaultHeaderRenderer = new NTableDefaultHeaderRenderer();
                }

                return m_defaultHeaderRenderer;
            }
        }

        public static INTableCellEditor DefaultCellEditor
        {
            get
            {
                return new NTableDefaultCellEditor();
            }
        }
        #endregion

        #region Brushes,Pens,Canvas, ect.
        static SolidBrush m_brush;
        internal static Brush GetBrush(Color color)
        {
            if (NTable.m_brush == null)
            {
                NTable.m_brush = new SolidBrush(color);
            }
            else
                NTable.m_brush.Color = color;


            return NTable.m_brush;
        }

        static Pen m_pen;
        internal static Pen GetPen()
        {
            return GetPen(Color.Black, 1);
        }

        internal static Pen GetPen(Color color)
        {
            return GetPen(color, 1);
        }

        internal static Pen GetPen(Color color, float width)
        {
            if (NTable.m_pen == null)
            {
                NTable.m_pen = new Pen(color, width);
            }
            else
            {
                NTable.m_pen.Color = color;
                NTable.m_pen.Width = width;
            }

            return NTable.m_pen;
        }
        #endregion

        #region API

        #region MoveNextPage
        public void MoveNextPage()
        {
            Move(m_visibleRowCount);
        }
        #endregion

        #region MoveBackPage
        public void MoveBackPage()
        {
            Move(-m_visibleRowCount);
        }
        #endregion

        #region MoveNext
        public void MoveNext()
        {
            Move(1);
        }
        #endregion

        #region MoveBack
        public void MoveBack()
        {
            Move(-1);
        }
        #endregion

        #region Move
#if WIN32
        public new void Move(int index)
#else
        public void Move(int index)
#endif
        {
            int currentRowIndex = m_currentCell == null ? 0 : m_currentCell.RowIndex;
            int currentColumnIndex = m_currentCell == null ? 0 : m_currentCell.ColumnIndex;

            Select(currentRowIndex + index, currentColumnIndex);
        }
        #endregion

        #region Select
        public bool Select(int rowIndex)
        {
            return Select(rowIndex, 0);
        }

        public bool Select(int rowIndex, int columnIndex)
        {
#if DEBUG_TABLE_LEVEL_3
            Debug.WriteLine(String.Format("NTable::Select Row:{0} Column:{1}",rowIndex,columnIndex));
#endif

            Debug.Assert(100000 != Model.GetRowCount(), "Please init your model");

            if (m_model == null)
                return false;

            if (Model.GetRowCount() == 0)
                return false;

            if (Model.GetColumnCount() <= columnIndex)
                return false;

            int currentRowIndex = m_currentCell != null ?
                m_currentCell.RowIndex : 0;

            if (Model.GetRowCount() <= rowIndex)
                rowIndex = Model.GetRowCount() - 1;

            if (rowIndex < 0)
                rowIndex = 0;

            m_currentCell = new NTableCell(rowIndex, columnIndex);

            if (m_vScrollBar.Visible)
            {
                if (rowIndex + 2 > (m_firstRow + m_visibleRowCount)
                    || rowIndex < m_firstRow)
                {
                    if (currentRowIndex == rowIndex - 1)
                        m_vScrollBar.Value += m_defaultRowHeight;
                    else
                        m_vScrollBar.Value = rowIndex * m_defaultRowHeight;
                }
            }

            if (RowChanged != null)
                RowChanged.Invoke(rowIndex);

            if (CellFocusChanged != null)
                CellFocusChanged.Invoke(m_currentCell);

            Invalidate();


            return true;

        }

        public void Select(List<int> rowIndex)
        {
            foreach (int i in rowIndex)
                if (!m_selectedRows.Contains(i))
                    m_selectedRows.Add(i);

            Invalidate();
        }
        #endregion

        #region Deselect
        public void Deselect(int rowIndex)
        {
            if (m_multipleSelection)
                m_selectedRows.Remove(rowIndex);
            else
                m_currentCell = null;

            if (SelectionChanged != null)
                SelectionChanged.Invoke();

            Invalidate();
        }

        public void Deselect(List<int> rowIndex)
        {
            foreach (int i in rowIndex)
                m_selectedRows.Remove(i);

            if (SelectionChanged != null)
                SelectionChanged.Invoke();

            Invalidate();
        }
        #endregion

        #region ClearSelectedRows
        public bool ClearSelectedRows()
        {
            bool rv = false;

            if (m_multipleSelection)
            {
                m_selectedRows.Clear();

                if (SelectionChanged != null)
                    SelectionChanged.Invoke();

                rv = true;
            }

            Invalidate();
            return rv;
        }
        #endregion

        #region SelectedRows
        public List<int> SelectedRows()
        {
            return m_selectedRows;
        }
        #endregion

        #region SetColumnWidth
        public void SetColumnWidth(int columnIndex, int width)
        {

#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::SetColumnWidth");
#endif

            if (columnIndex >= m_columns.Count)
                throw new Exception("Invalid column index");

            NTableColumn column = m_columns[columnIndex];


            column.Width = width;

            RecalculateSize();

        }
        #endregion

        #region SetColumnName
        public void SetColumnName(int columnIndex, String name)
        {

#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::SetColumnName");
#endif

            if (columnIndex >= m_columns.Count)
                throw new Exception("Invalid column index");

            NTableColumn column = m_columns[columnIndex];


            column.Name = name;

        }
        #endregion

        #region HitTest
        public NTableCell HitTest(int x, int y)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::HitTest");
#endif

            NTableCell tableCell = new NTableCell();

            int columnIndex = 0;
            int totalColumnWidth = 0;

            while (columnIndex < m_columns.Count)
            {
                if (x > totalColumnWidth && x < totalColumnWidth + m_columns[columnIndex].Width)
                    break;

                totalColumnWidth += m_columns[columnIndex].Width;
                ++columnIndex;
            }


            tableCell.ColumnIndex = columnIndex;// (m_iVirtualX + x) / m_iAvgColumnSize;

            if (tableCell.ColumnIndex >= m_columns.Count)
                tableCell.ColumnIndex = m_columns.Count - 1;

            if (y >= DefaultRowHeight)
            {
                tableCell.RowIndex = (m_virtualY + y) / DefaultRowHeight;

                if (tableCell.RowIndex > Model.GetRowCount())
                    tableCell.RowIndex = Model.GetRowCount();

                --tableCell.RowIndex;

            }
            else
                tableCell.RowIndex = -1;


            return tableCell;
        }
        #endregion

        #region AddColumn
        public void AddColumn(NTableColumn column)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::AddColumn");
#endif
            if (m_columns.ContainsKey(column.Index))
                m_columns.Remove(column.Index);

            m_columns.Add(column.Index, column);

            OnTableSettingsChanged();
        }
        #endregion

        #region GetColumn
        public NTableColumn GetColumn(int index)
        {

            Debug.Assert(index > -1, "Column index must be great then -1");

#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::GetColumn");
#endif
            if (!m_columns.ContainsKey(index))
                m_columns.Add(index, new NTableColumn(index));


            return m_columns[index];
        }
        #endregion

        #region GetSelectedItems
        public List<T> GetSelectedItems<T>()
            where T : class
        {
            List<T> list = new List<T>();

            foreach (int i in m_selectedRows)
                list.Add(Model.GetObjectAt(i, 0) as T);

            return list;
        }
        #endregion

        #region IsRowSelected
        public bool IsRowSelected(int rowIndex)
        {
            if (m_multipleSelection)
                return m_selectedRows.Contains(rowIndex);
            else if (m_currentCell != null)
                return m_currentCell.RowIndex == rowIndex;

            return false;
        }
        #endregion

        #endregion

        #region Paint

        #region GetCellRect
        Bitmap m_backBuffer;

        Rectangle GetCellRect(int x, int y, int width, int height)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::GetCellRect");
#endif
            int allowWidth, allowHeight;

            if (x + width > m_visibleWidth)
                allowWidth = m_visibleWidth - x;
            else
                allowWidth = width;


            if (y + height > m_visibleHeight)
                allowHeight = m_visibleHeight - y;
            else
                allowHeight = height;

            return new Rectangle(x, y, allowWidth, allowHeight);
        }

        Rectangle GetCellRect(int columnIndex, int rowIndex)
        {
            int i = 0;
            int x = 0;
            int y = 0;

            while (i < columnIndex)
                x += m_columns[i++].Width;

            y = (rowIndex + 1) * DefaultRowHeight;

            x -= m_virtualX;

            y -= m_virtualY;

            return GetCellRect(
                x,
                y,
                m_columns[columnIndex].Width,
                DefaultRowHeight);

        }
        #endregion

        #region RedrawBackBuffer
        void RedrawBackBuffer()
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::RedrawBackBuffer");
#endif
            m_backBuffer = new Bitmap(m_visibleWidth, m_visibleHeight);

            using (Graphics graphics = Graphics.FromImage(m_backBuffer))
            {
                graphics.FillRectangle(GetBrush(BackColor), 0, 0, m_visibleWidth, m_visibleHeight);

                int columnCount = m_model.GetColumnCount();

                int x = 0, y = 0;

                int selectedColumnIndex = m_currentCell != null ? m_currentCell.ColumnIndex : -1;
                int selectedRowIndex = m_currentCell != null ? m_currentCell.RowIndex : -1;

                // Rendering columns

                for (int columnIndex = m_firstColumn; columnIndex < columnCount; columnIndex++)
                {
                    NTableColumn tableColumn = m_columns[columnIndex];

                    INTableCellRenderer renderer = tableColumn.HeaderRenderer;

                    NTableDrawControl control = renderer.getTableCellRendererComponent(this,
                            tableColumn.Name == null ?
                            Model.GetColumnName(columnIndex) :
                            tableColumn.Name,
                            false,
                            false,
                            -1,
                            columnIndex);


                    control.Rect = GetCellRect(x, y, tableColumn.Width, m_defaultRowHeight);
                    control.Draw(graphics);

                    x += tableColumn.Width;

                    if (x > m_visibleWidth)
                        break;
                }

                // Assing column width
                y += m_defaultRowHeight;

                // Rendering rows

                m_visibleRowCount = 0;

                for (int rowIndex = m_firstRow; rowIndex < m_model.GetRowCount(); rowIndex++)
                {
                    if (y + m_defaultRowHeight
                        > m_visibleHeight)
                        break;

                    x = 0;

                    ++m_visibleRowCount;

                    for (int columnIndex = m_firstColumn; columnIndex < columnCount; columnIndex++)
                    {
                        NTableColumn tableColumn = m_columns[columnIndex];

                        INTableCellRenderer renderer = null;

                        if (m_leftHeader && columnIndex == 0)
                            renderer = tableColumn.HeaderRenderer;
                        else
                            renderer = tableColumn.CellRenderer;

                        NTableDrawControl control = renderer.getTableCellRendererComponent(this,
                            Model.GetValueAt(rowIndex, columnIndex),
                            m_multipleSelection ?
                            m_selectedRows.Contains(rowIndex) :
                            rowIndex == selectedRowIndex,
                            rowIndex == selectedRowIndex && columnIndex == selectedColumnIndex,
                            rowIndex,
                            columnIndex);

                        control.Rect = GetCellRect(x, y, tableColumn.Width, m_defaultRowHeight);

                        control.Draw(graphics);

                        x += tableColumn.Width;


                        if (x > m_visibleWidth)
                            break;
                    }

                    y += m_defaultRowHeight;

                    // if (y > m_iVisibleHeight)
                    //     break;
                }


                if (m_drag != null)
                {

                    if (m_showStartSplitter)
                    {

                        graphics.DrawLine(GetPen(m_splitterStartColor, m_splitterWidth),
                            m_drag.Start.X,
                            0,
                            m_drag.Start.X,
                            m_visibleHeight);

                        if (m_splitterMode == NTableSplitterMode.ResizeByRect)
                        {
                            graphics.DrawLine(GetPen(m_splitterColor, m_splitterWidth),
                                m_drag.Start.X,
                                0,
                                m_drag.End.X,
                                0);
                        }
                    }

                    if (m_showSplitterValue)
                    {
                        int px = 0;

                        if (m_splitterMode == NTableSplitterMode.Default)
                            px = m_drag.X * -1;
                        else
                            px = m_drag.X > 0 ? m_drag.X : m_drag.X * -1;

                        graphics.DrawString(String.Format(" {0}px", px),
                            Font,
                            GetBrush(m_splitterColor),
                            m_drag.End.X, m_visibleHeight / 2);
                    }

                    graphics.DrawLine(GetPen(m_splitterColor, m_splitterWidth),
                        m_drag.End.X,
                        0,
                        m_drag.End.X,
                        m_visibleHeight);


                }

                if (Focused && m_drawGridBorder)
                {
                    graphics.DrawRectangle(GetPen(m_borderColor, 3), this.ClientRectangle);
                }
            }
        }
        #endregion

        #region OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnPaint");
#endif
            if (m_model == null)
                return;

            RedrawBackBuffer();

            e.Graphics.DrawImage(m_backBuffer, 0, 0);
            m_backBuffer.Dispose();
            m_backBuffer = null;
            Thread.Sleep(1); //Let it drawing, it will enhance performance
        }
        #endregion

        #region OnPaintBackground
        protected override void OnPaintBackground(PaintEventArgs e)
        {
#if DEBUG_TABLE_LEVEL_1
            Debug.WriteLine("NTable::OnPaintBackground");
#endif
            if (IsPairScrollShow)
            {
                e.Graphics.FillRectangle(GetBrush(BackColor),
                    m_visibleWidth,
                    m_visibleHeight,
                    m_hScrollHeight,
                    m_visibleHeight);
                Thread.Sleep(0);
            }
        }
        #endregion

        #endregion
        #region TestModel
        public class TestModel : INTableModel
        {

            int m_rowCount;

            public TestModel()
            {
                m_rowCount = 100000;
            }

            public TestModel(int rowCount)
            {
                m_rowCount = rowCount;
            }

            #region ITableModel Members

            private static string[] columnNames = { "Col0", "Col1" };

            public int GetRowCount()
            {
                return m_rowCount;
            }

            public int GetColumnCount()
            {
                return columnNames.Length;
            }

            public string GetColumnName(int columnIndex)
            {
                return columnNames[columnIndex];
            }

            public Type GetColumnClass(int columnIndex)
            {
                return String.Empty.GetType();
            }

            public bool IsCellEditable(int rowIndex, int columnIndex)
            {
                return true;
            }

            public object GetValueAt(int rowIndex, int columnIndex)
            {
                return String.Format("{0}:{1}", rowIndex, columnIndex);
            }

            public void SetValueAt(object aValue, int rowIndex, int columnIndex)
            {
                if (Change != null)
                    Change.Invoke();
            }

            public object GetObjectAt(int rowIndex, int columnIndex)
            {
                return String.Empty;
            }

            #endregion

            #region ITableModel Members

            public event TableModelChangeHandler Change;

            #endregion
        }
        #endregion
    }
}
