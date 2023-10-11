using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CSLibrary.Windows
{
    public class GdiPlus
    {
        #region GradientFill P/Invokes
        // This method wraps the PInvoke to GradientFill.
        // Parmeters:
        //  gx - The Graphics object we are filling
        //  rc - The rectangle to fill
        //  startColor - The starting color for the fill
        //  endColor - The ending color for the fill
        //  fillDir - The direction to fill
        //
        // Returns true if the call to GradientFill succeeded; false
        // otherwise.
        public static bool Fill(
            Graphics gx,
            Rectangle rc,
            Color startColor, Color endColor,
            FillDirection fillDir)
        {

            // Initialize the data to be used in the call to GradientFill.
            TRIVERTEX[] tva = new TRIVERTEX[2];
            tva[0] = new TRIVERTEX(rc.X, rc.Y, startColor);
            tva[1] = new TRIVERTEX(rc.Right, rc.Bottom, endColor);
            GRADIENT_RECT[] gra = new GRADIENT_RECT[] { new GRADIENT_RECT(0, 1) };

            // Get the hDC from the Graphics object.
            IntPtr hdc = gx.GetHdc();

            // PInvoke to GradientFill.
            bool b;

            b = GradientFill(
                    hdc,
                    tva,
                    (uint)tva.Length,
                    gra,
                    (uint)gra.Length,
                    (uint)fillDir);
            System.Diagnostics.Debug.Assert(b, string.Format(
                "GradientFill failed: {0}",
                System.Runtime.InteropServices.Marshal.GetLastWin32Error()));

            // Release the hDC from the Graphics object.
            gx.ReleaseHdc(hdc);

            return b;
        }

        // The direction to the GradientFill will follow
        public enum FillDirection
        {
            //
            // The fill goes horizontally
            //
            LeftToRight = GRADIENT_FILL_RECT_H,
            //
            // The fill goes vertically
            //
            TopToBottom = GRADIENT_FILL_RECT_V
        }

        public struct TRIVERTEX
        {
            public int x;
            public int y;
            public ushort Red;
            public ushort Green;
            public ushort Blue;
            public ushort Alpha;
            public TRIVERTEX(int x, int y, Color color)
                : this(x, y, color.R, color.G, color.B, color.A)
            {
            }
            public TRIVERTEX(
                int x, int y,
                ushort red, ushort green, ushort blue,
                ushort alpha)
            {
                this.x = x;
                this.y = y;
                this.Red = (ushort)(red << 8);
                this.Green = (ushort)(green << 8);
                this.Blue = (ushort)(blue << 8);
                this.Alpha = (ushort)(alpha << 8);
            }
        }
        public struct GRADIENT_RECT
        {
            public uint UpperLeft;
            public uint LowerRight;
            public GRADIENT_RECT(uint ul, uint lr)
            {
                this.UpperLeft = ul;
                this.LowerRight = lr;
            }
        }

        public const int GRADIENT_FILL_RECT_H = 0x00000000;
        public const int GRADIENT_FILL_RECT_V = 0x00000001;
        #endregion

        #region WindowsCE
        //WINGDIAPI  BOOL      WINAPI TransparentImage(HDC,int,int,int,int,HANDLE,int,int,int,int,COLORREF);
        [DllImport("coredll.dll", EntryPoint = "TransparentImage")]
        private static extern bool TransparentImageCE(IntPtr hDC, int srcx, int srcy, int srcw, int srch, IntPtr hBitmap, int destx, int dsty, int dstw, int dsth, int color);

        [DllImport("coredll.dll", EntryPoint = "FillRect")]
        private static extern int FillRectCE(IntPtr hDC, [In]ref RECT lprc, IntPtr hbr);

        [DllImport("coredll.dll", EntryPoint = "BitBlt")]
        private static extern int BitBltCE(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            TernaryRasterOperations dwRop);

        [DllImport("coredll.dll", EntryPoint = "SHLoadDIBitmap")]
        private static extern IntPtr SHLoadDIBitmapCE(string szFileName);

        [DllImport("coredll.dll", EntryPoint = "CreateCompatibleBitmap")]
        private static extern IntPtr CreateCompatibleBitmapCE(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("coredll.dll", EntryPoint = "CreateCompatibleDC")]
        private static extern IntPtr CreateCompatibleDCCE(IntPtr hdc);

        [DllImport("coredll.dll", EntryPoint = "GetDC")]
        private static extern IntPtr GetDCCE(IntPtr hWnd);

        [DllImport("coredll.dll", EntryPoint = "GetCapture")]
        private static extern IntPtr GetCaptureCE();

        [DllImport("coredll.dll", EntryPoint = "SetCapture")]
        private static extern IntPtr SetCaptureCE(IntPtr hWnd);

        [DllImport("coredll.dll", EntryPoint = "ReleaseCapture")]
        private static extern bool ReleaseCaptureCE();

        [DllImport("coredll.dll", EntryPoint = "SelectObject")]
        private static extern IntPtr SelectObjectCE(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("coredll.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObjectCE(IntPtr hObject);

        [DllImport("coredll.dll", EntryPoint = "GetObject")]
        private static extern int GetObjectCE(IntPtr hgdiobj, int cbBuffer, out LOGBRUSH brush);

        [DllImport("coredll.dll", EntryPoint = "ReleaseDC")]
        private static extern bool ReleaseDCCE(IntPtr hwnd, IntPtr hdc);

        [DllImport("coredll.dll", EntryPoint = "DeleteDC")]
        private static extern bool DeleteDCCE(IntPtr hdc);

        [DllImport("coredll.dll", EntryPoint = "GetParent")]
        private static extern IntPtr GetParentCE(IntPtr hWnd);

        [DllImport("coredll.dll", EntryPoint = "StretchBlt")]
        private static extern bool StretchBltCE(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            TernaryRasterOperations dwRop);

        [DllImport("coredll.dll", EntryPoint = "CreateSolidBrush")]
        private static extern IntPtr CreateSolidBrushCE(ColorRef color);

        [DllImport("coredll.dll", EntryPoint = "GradientFill", SetLastError = true)]
        public extern static bool GradientFillCE(
            IntPtr hdc,
            TRIVERTEX[] pVertex,
            uint dwNumVertex,
            GRADIENT_RECT[] pMesh,
            uint dwNumMesh,
            uint dwMode);
        #endregion

        #region Windows32
        //WINGDIAPI  BOOL      WINAPI TransparentImage(HDC,int,int,int,int,HANDLE,int,int,int,int,COLORREF);
        [DllImport("gdi32.dll", EntryPoint = "TransparentImage")]
        private static extern bool TransparentImage32(IntPtr hDC, int srcx, int srcy, int srcw, int srch, IntPtr hBitmap, int destx, int dsty, int dstw, int dsth, int color);

        [DllImport("user32.dll", EntryPoint = "FillRect")]
        private static extern int FillRect32(IntPtr hDC, [In]ref RECT lprc, IntPtr hbr);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        private static extern int BitBlt32(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", EntryPoint = "SHLoadDIBitmap")]
        private static extern IntPtr SHLoadDIBitmap32(string szFileName);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        private static extern IntPtr CreateCompatibleBitmap32(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        private static extern IntPtr CreateCompatibleDC32(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "GetDC")]
        private static extern IntPtr GetDC32(IntPtr hWnd);

        [DllImport("gdi32.dll", EntryPoint = "GetCapture")]
        private static extern IntPtr GetCapture32();

        [DllImport("gdi32.dll", EntryPoint = "SetCapture")]
        private static extern IntPtr SetCapture32(IntPtr hWnd);

        [DllImport("gdi32.dll", EntryPoint = "ReleaseCapture")]
        private static extern bool ReleaseCapture32();

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        private static extern IntPtr SelectObject32(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject32(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "GetObject")]
        private static extern int GetObject32(IntPtr hgdiobj, int cbBuffer, out LOGBRUSH brush);

        [DllImport("gdi32.dll", EntryPoint = "ReleaseDC")]
        private static extern bool ReleaseDC32(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        private static extern bool DeleteDC32(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "GetParent")]
        private static extern IntPtr GetParent32(IntPtr hWnd);

        [DllImport("gdi32.dll", EntryPoint = "StretchBlt")]
        private static extern bool StretchBlt32(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", EntryPoint = "CreateSolidBrush")]
        private static extern IntPtr CreateSolidBrush32(ColorRef color);

        [DllImport("gdi32.dll", EntryPoint = "GdiGradientFill", SetLastError = true)]
        public extern static bool GradientFill32(
            IntPtr hdc,
            TRIVERTEX[] pVertex,
            uint dwNumVertex,
            GRADIENT_RECT[] pMesh,
            uint dwNumMesh,
            uint dwMode);
        #endregion

        #region common export function
        public static bool TransparentImage(
            IntPtr hdc,
            int nXSrc,
            int nYSrc,
            int nWSrc,
            int nHSrc,
            IntPtr hBitmap,
            int nXDest,
            int nYDest,
            int nWDest,
            int nHDest,
            int color)
        {
            bool result = false;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = TransparentImageCE(hdc, nXSrc, nYSrc, nWSrc, nHSrc, hBitmap, nXDest, nYDest, nWDest, nYDest, color);
            }
            else
            {
                result = TransparentImage32(hdc, nXSrc, nYSrc, nWSrc, nHSrc, hBitmap, nXDest, nYDest, nWDest, nYDest, color);
            }
            return result;
        }

        public static int FillRect(IntPtr hDC, [In]ref RECT lprc, IntPtr hbr)
        {
            int result = -1;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = FillRectCE(hDC, ref lprc, hbr);
            }
            else
            {
                result = FillRect32(hDC, ref lprc, hbr);
            }
            return result;
        }

        public static int BitBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            TernaryRasterOperations dwRop)
        {
            int result = -1;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = BitBltCE(hdcDest, nXDest, nYDest, nWidth, nHeight, hdcSrc, nXSrc, nYSrc, dwRop);
            }
            else
            {
                result = BitBlt32(hdcDest, nXDest, nYDest, nWidth, nHeight, hdcSrc, nXSrc, nYSrc, dwRop);
            }
            return result;
        }

        public static IntPtr SHLoadDIBitmap(string szFileName)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = SHLoadDIBitmapCE(szFileName);
            }
            else
            {
                result = SHLoadDIBitmap32(szFileName);
            }
            return result;
        }

        public static IntPtr CreateCompatibleBitmap(
            IntPtr hdc,
            int nWidth,
            int nHeight)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = CreateCompatibleBitmapCE(hdc, nWidth, nHeight);
            }
            else
            {
                result = CreateCompatibleBitmap32(hdc, nWidth, nHeight);
            }
            return result;
        }

        public static IntPtr CreateCompatibleDC(IntPtr hdc)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = CreateCompatibleDCCE(hdc);
            }
            else
            {
                result = CreateCompatibleDC32(hdc);
            }
            return result;
        }

        public static IntPtr GetDC(IntPtr hWnd)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = GetDCCE(hWnd);
            }
            else
            {
                result = GetDC32(hWnd);
            }
            return result;
        }

        public static IntPtr GetCapture()
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = GetCaptureCE();
            }
            else
            {
                result = GetCapture32();
            }
            return result;
        }

        public static IntPtr SetCapture(IntPtr hWnd)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = SetCaptureCE(hWnd);
            }
            else
            {
                result = SetCapture32(hWnd);
            }
            return result;
        }

        public static bool ReleaseCapture()
        {
            bool result = false;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = ReleaseCaptureCE();
            }
            else
            {
                result = ReleaseCapture32();
            }
            return result;
        }

        public static IntPtr SelectObject(IntPtr hdc, IntPtr hObject)
        {
            IntPtr result = IntPtr.Zero;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = SelectObjectCE(hdc, hObject);
            }
            else
            {
                result = SelectObject32(hdc, hObject);
            }
            return result;
        }

        public static bool DeleteObject(IntPtr hObject)
        {
            bool result = false;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = DeleteObjectCE(hObject);
            }
            else
            {
                result = DeleteObject32(hObject);
            }
            return result;
        }

        public static bool ReleaseDC(IntPtr hwnd, IntPtr hdc)
        {
            bool result = false;

            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = ReleaseDCCE(hwnd, hdc);
            }
            else
            {
                result = ReleaseDC32(hwnd, hdc);
            }
            return result;
        }

        public static bool DeleteDC(IntPtr hdc)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = DeleteDCCE(hdc);
            }
            else
            {
                result = DeleteDC32(hdc);
            }
            return result;
        }

        public static IntPtr GetParent(IntPtr hWnd)
        {
            IntPtr result = IntPtr.Zero;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = GetParentCE(hWnd);
            }
            else
            {
                result = GetParent32(hWnd);
            }
            return result;
        }

        public static bool StretchBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWDest,
            int nHDest,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            int nWSrc,
            int nHtSrc,
            TernaryRasterOperations dwRop)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = StretchBltCE(hdcDest, nXDest, nYDest, nWDest, nHDest, hdcSrc, nXSrc, nYSrc, nWSrc, nHtSrc, dwRop);
            }
            else
            {
                result = StretchBlt32(hdcDest, nXDest, nYDest, nWDest, nHDest, hdcSrc, nXSrc, nYSrc, nWSrc, nHtSrc, dwRop);
            }
            return result;
        }

        public static IntPtr CreateSolidBrush(ColorRef color)
        {
            IntPtr result = IntPtr.Zero;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = CreateSolidBrushCE(color);
            }
            else
            {
                result = CreateSolidBrush32(color);
            }
            return result;
        }

        public static bool GradientFill(
            IntPtr hdc,
            TRIVERTEX[] pVertex,
            uint dwNumVertex,
            GRADIENT_RECT[] pMesh,
            uint dwNumMesh,
            uint dwMode)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = GradientFillCE(hdc, pVertex, dwNumVertex, pMesh, dwNumMesh, dwMode);
            }
            else
            {
                result = GradientFill32(hdc, pVertex, dwNumVertex, pMesh, dwNumMesh, dwMode);
            }
            return result;
        }
        #endregion

        #region common
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            /*public RECT(uint left, uint top, uint right, uint bottom)
            {
                this.left = left;
                this.right = right;
                this.top = top;
                this.bottom = bottom;
            }*/
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.right = right;
                this.top = top;
                this.bottom = bottom;
            }
            public RECT(int left, int top, Size size)
            {
                this.left = left;
                this.top = top;
                this.right = size.Width + left;
                this.bottom = size.Height + top;
            }
            public int Height
            {
                get { return bottom - top; }
            }
            public int Width
            {
                get { return right - left; }
            }
            public bool PtInRect(Point pt)
            {
                return (pt.X >= left && pt.X <= right && pt.Y >= top && pt.Y <= bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LOGBRUSH
        {
            public uint lbStyle;
            public uint lbColor; // COLORREF 0x00bbggrr
            public int lbHatch; //
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ColorRef
        {
            // COLORREF is a typedef to DWORD
            uint Value; // 0x00BBGGRR

            public byte Blue { get { return (byte)((Value >> 16) & 0xFF); } set { Value = (uint)(Value & 0xFF00FFFF | (uint)(value << 16)); } }
            public byte Green { get { return (byte)((Value >> 8) & 0xFF); } set { Value = (uint)(Value & 0xFFFF00FF | (uint)(value << 8)); } }
            public byte Red { get { return (byte)((Value >> 0) & 0xFF); } set { Value = (uint)(Value & 0xFFFFFF00 | (uint)(value << 0)); } }

            public static ColorRef From(Color c)
            {
                return new ColorRef(c);
            }

            public ColorRef(Color c)
            {
                Value = (uint)(c.B << 16 | c.G << 8 | c.R << 0);
            }
            public Color ToColor() { return Color.FromArgb(Red, Green, Blue); }

            public static ColorRef Invalid;
        }

        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062
        }
        #endregion

    }
}
