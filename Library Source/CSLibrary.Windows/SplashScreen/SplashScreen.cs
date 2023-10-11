using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace CSLibrary.Windows.UI
{
    /// <summary>
    /// SplashScreen
    /// </summary>
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// Device Type
        /// </summary>
        public enum CSL
        {
            /// <summary>
            /// Handheld Reader CS101
            /// </summary>
            CS101 = 0,
            /// <summary>
            /// Integrated RFID Reader CS203
            /// </summary>
            CS203 = 1,
        }

        private int dot = 0;
        // Threading
        static SplashScreen ms_frmSplash = null;
        static Thread ms_oThread = null;
        static CSL m_type = CSL.CS101;
        static int m_width = 320;
        static int m_height = 240;
        static object myLock = new object();
        static int m_formStarted = 0;


        /// <summary>
        /// IsDisposed
        /// </summary>
        public new bool IsDisposed = false;
        /// <summary>
        /// Constructor
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }
        private delegate void UpdateInformationDel(string info);
        /// <summary>
        /// A static method to create the thread and 
        /// launch the SplashScreen.
        /// </summary>
        /// <param name="info">Device type</param>
        static public void UpdateInformation(string info)
        {
            // Make sure it's only launched once.
            if (ms_frmSplash == null)
                return;
            if (ms_frmSplash.InvokeRequired)
            {
                ms_frmSplash.BeginInvoke(new UpdateInformationDel(UpdateInformation), new object[] { info });
                return;
            }
            lock (myLock)
            {
                ms_frmSplash.Show();
                ms_frmSplash.lbStatus.Text = info;
                ms_frmSplash.lbStatus.Invalidate();
            }
        }
        /// <summary>
        /// A static method to create the thread and 
        /// launch the SplashScreen.
        /// </summary>
        /// <param name="info">Device type</param>
        static public void UpdateAnimationInformation(string info)
        {
            // Make sure it's only launched once.
            if (ms_frmSplash == null)
                return;
            if (ms_frmSplash.InvokeRequired)
            {
                ms_frmSplash.BeginInvoke(new UpdateInformationDel(UpdateAnimationInformation), new object[] { info });
                return;
            }
            lock (myLock)
            {
                ms_frmSplash.Show();
                ms_frmSplash.lb_text.Text = info;
                ms_frmSplash.lb_text.Invalidate();
            }
        }
        /// <summary>
        /// A static method to create the thread and 
        /// launch the SplashScreen.
        /// </summary>
        /// <param name="type">Device type</param>
        static public void Show(CSL type)
        {
            m_type = type;

            // Make sure it's only launched once.
            if (ms_frmSplash != null)
                return;
            ms_oThread = new Thread(new ThreadStart(SplashScreen.ShowForm));
            ms_oThread.IsBackground = true;
            ms_oThread.Start();
            IsReady();
        }
        delegate void HideDelegate();
        /// <summary>
        /// A static method to create the thread and 
        /// launch the SplashScreen.
        /// </summary>
        /// <param name="type">Device type</param>
        static public void Hided()
        {
            if (ms_frmSplash == null)
                return;
            if (ms_frmSplash.InvokeRequired)
            {
                ms_frmSplash.BeginInvoke(new HideDelegate(Hided), new object[] { });
                return;
            }
            lock (myLock)
            {
                ms_frmSplash.Hide();
            }
        }

        // A private entry point for the thread.
        static private void ShowForm()
        {
            ms_frmSplash = new SplashScreen();
            Application.Run(ms_frmSplash);
        }
        /// <summary>
        /// Wait SplashScreen to ready
        /// </summary>
        private static bool IsReady()
        {
            while (true)
            {
                if (Interlocked.Equals(m_formStarted, 1))
                    break;
                Thread.Sleep(1);
                Application.DoEvents();
            }
            return true;
        }

        private delegate void CloseSplashScreenDel();
        /// <summary>
        /// A static method to close the SplashScreen
        /// </summary>
        static public void Stop()
        {
            if (ms_frmSplash == null || ms_frmSplash.IsDisposed)
            {
                return;
            }

            if (ms_frmSplash.InvokeRequired)
            {
                ms_frmSplash.Invoke(new CloseSplashScreenDel(Stop), new object[] { });
                return;
            }

            ms_frmSplash.Close();
            ms_oThread = null;	// we don't need these any more.
            ms_frmSplash = null;
            Interlocked.Decrement(ref  m_formStarted);
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
#if schmidt
            pic_schmidt.Visible = true;
#endif
#if Savi
            picSavi.Visible = true;
#endif
#if CSL
            picCSL.Visible = true;
#endif
            switch (m_type)
            {
                case CSL.CS101:
                    pic_cs101.Visible = true;
                    pic_cs203.Visible = false;
                    pnl_text.Location = new Point(103, 72);
                    break;
                case CSL.CS203:
                    pic_cs101.Visible = false;
                    pic_cs203.Visible = true;
                    pnl_text.Location = new Point(62, 72);
                    break;
            }

            RECT rtDesktop = new RECT();
            if (SystemParametersInfo(SPI_GETWORKAREA, 0, ref rtDesktop, 0))
            {
                m_width = rtDesktop.right - rtDesktop.left;
                m_height = rtDesktop.bottom - rtDesktop.top;
            }
            this.Location = new Point((m_width - Width) / 2, (m_height - Height) / 2);
            updateTextTimer.Enabled = true;
            Interlocked.Increment(ref  m_formStarted);
        }

        private void SplashScreen_Closing(object sender, CancelEventArgs e)
        {
            updateTextTimer.Enabled = false;
        }

        private void updateTextTimer_Tick(object sender, EventArgs e)
        {
            if (dot == 3)
            {
                dot = 0;
                lb_dot.Text = "";
            }
            else
            {
                lb_dot.Text += ".";
                dot++;
            }
        }

        private const int SPI_GETWORKAREA = 48;
#if WindowsCE
        [DllImport("coredll.dll")]
        [return : MarshalAs( UnmanagedType.Bool)]
#else
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
#endif
        private extern static bool SystemParametersInfo(int uiAction, int uiParam, ref RECT pvParam, int fWinIni);
        /// <summary>
        /// RECT
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            /// <summary>
            /// Left position
            /// </summary>
            public int left;
            /// <summary>
            /// Top position
            /// </summary>
            public int top;
            /// <summary>
            /// Right position
            /// </summary>
            public int right;
            /// <summary>
            /// Bottom position
            /// </summary>
            public int bottom;
        }

    }
}