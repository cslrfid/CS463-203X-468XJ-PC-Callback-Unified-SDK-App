using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace CSLibrary.Windows
{   
    public class ListViewCustomDraw : ListViewEx
    {
        [DllImport("coredll.dll", EntryPoint = "CallWindowProc")]
        static extern IntPtr CallWindowProcCE(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "CallWindowProc")]
        static extern IntPtr CallWindowProc32(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("coredll", EntryPoint = "SendMessage")]
        static extern int SendMessageCE(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern int SendMessage32(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);


        [DllImport("coredll.dll", EntryPoint = "SendMessage")]
        static extern uint SendMessageCE(IntPtr hwnd, uint msg, uint wparam, uint lparam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        static extern uint SendMessage32(IntPtr hwnd, uint msg, uint wparam, uint lparam);

        [DllImport("coredll.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        static extern int SetWindowLongCE(IntPtr hWnd, int nIndex, WndProcDelegate newProc);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        static extern int SetWindowLong32(IntPtr hWnd, int nIndex, WndProcDelegate newProc);

        [DllImport("coredll.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        static extern IntPtr GetWindowLongCE(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        const int GWL_WNDPROC = -4;

        const int WM_NOTIFY = 0x4E;
        const int NM_CUSTOMDRAW = (-12);

        const int CDRF_NOTIFYITEMDRAW = 0x00000020;
        const int CDRF_NOTIFYSUBITEMDRAW = CDRF_NOTIFYITEMDRAW;
        const int CDRF_NOTIFYPOSTPAINT = 0x00000010;
        const int CDRF_SKIPDEFAULT = 0x00000004;
        const int CDRF_DODEFAULT = 0x00000000;
        const int CDDS_PREPAINT = 0x00000001;
        const int CDDS_POSTPAINT = 0x00000002;
        const int CDDS_ITEM = 0x00010000;
        const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
        const int CDDS_SUBITEM = 0x00020000;
        const int CDIS_SELECTED = 0x0001;
        const int LVM_GETSUBITEMRECT = (0x1000 + 56);

        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        IntPtr lpPrevWndFunc;

        public ListViewCustomDraw()
        {
            View = View.Details;
            DoubleBuffering = true;
            GridLines = true;
            Gradient = true;

            ParentChanged += delegate
            {
                if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    lpPrevWndFunc = GetWindowLongCE(Parent.Handle, GWL_WNDPROC);
                    SetWindowLongCE(Parent.Handle, GWL_WNDPROC, WndProc);
                }
                else
                {
                    lpPrevWndFunc = GetWindowLong32(Parent.Handle, GWL_WNDPROC);
                    SetWindowLong32(Parent.Handle, GWL_WNDPROC, WndProc);
                }
            };
        }

        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_NOTIFY)
            {
                var nmhdr = (NMHDR)Marshal.PtrToStructure(lParam, typeof(NMHDR));
                if (nmhdr.hwndFrom == Handle && nmhdr.code == NM_CUSTOMDRAW)
                    return CustomDraw(hWnd, msg, wParam, lParam);

            }
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                return CallWindowProcCE(lpPrevWndFunc, hWnd, msg, wParam, lParam);
            }
            else
            {
                return CallWindowProc32(lpPrevWndFunc, hWnd, msg, wParam, lParam);
            }
        }

        private IntPtr CustomDraw(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            int result;
            var nmlvcd = (NMLVCUSTOMDRAW)Marshal.PtrToStructure(lParam, typeof(NMLVCUSTOMDRAW));
            switch (nmlvcd.nmcd.dwDrawStage)
            {
                case CDDS_PREPAINT:
                    result = CDRF_NOTIFYITEMDRAW;
                    break;

                case CDDS_ITEMPREPAINT:
                    var itemBounds = RectangleExtensions.ToRectangle(nmlvcd.nmcd.rc);
                    if ((nmlvcd.nmcd.uItemState & CDIS_SELECTED) != 0)
                    {
                        using (var brush = new SolidBrush(SystemColors.Highlight))
                        using (var graphics = Graphics.FromHdc(nmlvcd.nmcd.hdc))
                            graphics.FillRectangle(brush, itemBounds);
                    }

                    result = CDRF_NOTIFYSUBITEMDRAW;
                    break;

                case CDDS_SUBITEM | CDDS_ITEMPREPAINT:
                    var index = nmlvcd.nmcd.dwItemSpec;
                    var rect = new RECT();
                    rect.top = nmlvcd.iSubItem;
                    if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
                    {
                        SendMessageCE(Handle, LVM_GETSUBITEMRECT, index, ref rect);
                    }
                    else
                    {
                        SendMessage32(Handle, LVM_GETSUBITEMRECT, index, ref rect);
                    }
                    rect.left += 2;

                    Color textColor;
                    if ((nmlvcd.nmcd.uItemState & CDIS_SELECTED) != 0)
                        textColor = SystemColors.HighlightText;
                    else
                        textColor = SystemColors.ControlText;

                    using (var brush = new SolidBrush(textColor))
                    using (var graphics = Graphics.FromHdc(nmlvcd.nmcd.hdc))
                        graphics.DrawString(Items[index].SubItems[nmlvcd.iSubItem].Text,
                                            Font,
                                            brush,
                                            RectangleExtensions.ToRectangleF(rect));

                    result = CDRF_SKIPDEFAULT | CDRF_NOTIFYSUBITEMDRAW;
                    break;

                default:
                    result = CDRF_DODEFAULT;
                    break;
            }

            return (IntPtr)result;
        }
    }
}
