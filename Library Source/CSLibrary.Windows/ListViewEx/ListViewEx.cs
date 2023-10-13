using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSLibrary.Windows
{
    public class ListViewEx : ListView
    {
        [DllImport("coredll.dll", EntryPoint = "SendMessage")]
        static extern uint SendMessageCE(IntPtr hwnd, uint msg, uint wparam, uint lparam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        static extern uint SendMessage32(IntPtr hwnd, uint msg, uint wparam, uint lparam);

        const uint LVM_FIRST = 0x1000;
        const uint LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
        const uint LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55;

        const uint LVS_EX_GRIDLINES = 0x00000001;
        const uint LVS_EX_DOUBLEBUFFER = 0x00010000;
        const uint LVS_EX_GRADIENT = 0x20000000;

        private void SetStyle(uint style, bool enable)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                uint currentStyle = SendMessageCE(Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
                uint lparam = enable ? currentStyle | style : currentStyle & ~style;
                SendMessageCE(Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, lparam);
            }
            else
            {
                uint currentStyle = SendMessage32(Handle, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);
                uint lparam = enable ? currentStyle | style : currentStyle & ~style;
                SendMessage32(Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, lparam);
            }
        }

        bool gridLines = false;
        public bool GridLines
        {
            get { return gridLines; }
            set
            {
                gridLines = value;
                SetStyle(LVS_EX_GRIDLINES, gridLines);
            }
        }


        bool doubleBuffering = false;
        public bool DoubleBuffering
        {
            get { return doubleBuffering; }
            set
            {
                doubleBuffering = value;
                SetStyle(LVS_EX_DOUBLEBUFFER, doubleBuffering);
            }
        }

        bool gradient = false;
        public bool Gradient
        {
            get { return gradient; }
            set
            {
                gradient = value;
                SetStyle(LVS_EX_GRADIENT, gradient);
            }
        }
    }
}
