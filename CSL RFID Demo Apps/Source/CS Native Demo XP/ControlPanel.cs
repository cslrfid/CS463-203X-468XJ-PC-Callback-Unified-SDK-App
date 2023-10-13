/*
Copyright (c) 2023 Convergence Systems Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.IO;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class ControlPanelForm : Form
    {
        public static ControlPanelForm  _controlPanel = null;
        private static Thread           _controlPanelThread = null;
        private static TagInventoryForm _mainForm = null;

        //public event EventHandler OnSQLCheck;

        public bool EnableStart
        {
            get { return btn_once.Enabled; }
            set { btn_once.Enabled = btn_run.Enabled = value; }
        }

        public bool EnableStop
        {
            get { return btn_stop.Enabled; }
            set { btn_stop.Enabled = value; }
        }

        public bool EnableClear
        {
            get { return btn_clear.Enabled; }
            set { btn_clear.Enabled = value; }
        }

        public bool EnableSelect
        {
            get { return btn_select.Enabled; }
            set { btn_select.Enabled = value; }
        }

        public bool EnableSave
        {
            get { return btn_save.Enabled; }
            set { btn_save.Enabled = value; }
        }

        public static TagInventoryForm MainForm
        {
            get { return ControlPanelForm._mainForm; }
            set { ControlPanelForm._mainForm = value; }
        }

        public ControlPanelForm()
        {
            InitializeComponent();
        }

        public int TagLogFileSizeLimit = 0;
        //public int TagLogFileLimit = 0;
        //public int TagLogFileCurrentNumber = 1;

        private void btn_run_Click(object sender, EventArgs e)
        {
            int cnt;

            TagLogFileSizeLimit = int.Parse(textBox1.Text) * 1024 * 1024;
            
            if (_mainForm != null)
                _mainForm.Start();
        }

        private void btn_once_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.StartOnce();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.Stop();

        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.SelectTag();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.Save();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.CloseForm();
        }

        public static ControlPanelForm ControlPanel
        {
            get { return ControlPanelForm._controlPanel; }
        }

        public static Thread ControlPanelThread
        {
            get { return ControlPanelForm._controlPanelThread; }
        }

        public static Thread LaunchControlPanel(TagInventoryForm form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            if (ControlPanelThread != null) return ControlPanelThread;

            _mainForm = form;

            ControlPanelForm._controlPanelThread = new Thread(ControlPanelThreadProc);
            ControlPanelThread.Name = "ControlPanel";
            ControlPanelThread.Priority = ThreadPriority.Highest;
            ControlPanelThread.IsBackground = false;
            ControlPanelThread.Start();
            return ControlPanelThread;
        }

        public static void EnablePannel(bool enable)
        {
            try
            {
                if (_controlPanelThread != null && _controlPanelThread.IsAlive)
                {
                    _controlPanel.Enabled = enable;
                }
            }
            catch (Exception) { }
        }

        public static void CloseControlPanel()
        {
            try
            {
                if (_controlPanelThread != null && _controlPanelThread.IsAlive)
                {
                    _controlPanel.Invoke(new MethodInvoker(_controlPanel.Close));
                    _controlPanelThread.Join();
                    _controlPanelThread = null;
                }
            }
            catch (Exception) { }
        }

        public static void SetResize(Point location, int height)
        {
            if (_controlPanelThread != null && _controlPanelThread.IsAlive && ControlPanel != null && ControlPanel.Created)
            {
                _controlPanel.Invoke(new ResizeFormDeleg(_controlPanel.SetResizeForm), new object[] { location, height });
            }
        }

        public static void SetTopMost(bool topMost)
        {
            if (_controlPanelThread != null && _controlPanelThread.IsAlive && ControlPanel != null && ControlPanel.Created)
            {
                if (topMost)
                    _controlPanel.Invoke(new MethodInvoker(_controlPanel.SetAsTopMost));
                else
                    _controlPanel.Invoke(new MethodInvoker(_controlPanel.ResetAsTopMost));
            }
        }

        public static void ShowControlPanel(bool show)
        {
            if (_controlPanelThread != null && _controlPanelThread.IsAlive && ControlPanel != null && ControlPanel.Created)
            {
                if (show)
                    _controlPanel.Invoke(new MethodInvoker(_controlPanel.ShowControlPanel));
                else
                    _controlPanel.Invoke(new MethodInvoker(_controlPanel.HideControlPanel));
            }
        }

        private delegate void ResizeFormDeleg(Point location, int height);
        private void SetResizeForm(Point location, int height)
        {
            if (this.InvokeRequired)
            {
                Invoke(new ResizeFormDeleg(SetResizeForm), new object[] { location, height });
                return;
            }
            this.Height = height;
            this.Location = location;
        }

        private void SetAsTopMost()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(SetAsTopMost));
                return;
            }
            this.TopMost = true;
        }

        private void ResetAsTopMost()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(SetAsTopMost));
                return;
            }
            this.TopMost = false;
        }

        private void ShowControlPanel()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowControlPanel));
                return;
            }
            this.Visible = true;
        }

        private void HideControlPanel()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(HideControlPanel));
                return;
            }
            this.Visible = false;
        }

        [STAThread]
        static void ControlPanelThreadProc()
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0} is threadID {1}", System.Threading.Thread.CurrentThread.Name, System.Threading.Thread.CurrentThread.ManagedThreadId));
            _controlPanel = new ControlPanelForm();

            _controlPanel.StartPosition = FormStartPosition.Manual;
            _controlPanel.ShowInTaskbar = false;
            Application.Run(_controlPanel);
            System.Diagnostics.Debug.WriteLine("ControlPanel Thread is exiting");
        }

        private void ControlPanelForm_Load(object sender, EventArgs e)
        {
#if PRODUCTIONTOOLS
            textBox_LogPath.Text = ".\\";
#else
            textBox_LogPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSLReader\\";
#endif
            System.Diagnostics.Debug.Assert(MainForm != null);

            Point pt = MainForm.Location;
            pt.Offset(MainForm.Width, 0);
            Rectangle r = Screen.GetBounds(this.DesktopBounds);
            if (pt.X + Width > r.Width)
                pt.X = r.Right - Width;

            this.Location = pt;

            this.Height = MainForm.Height;

            MainForm.OnButtonEnable += new EventHandler<TagInventoryForm.OnButtonClickEventArgs>(MainForm_OnButtonEnable);

            EnableStop = false;
            EnableSelect = _mainForm.SelectMode;
        }
        void MainForm_OnButtonEnable(object sender, TagInventoryForm.OnButtonClickEventArgs e)
        {
            ButtonHandle(e.State, e.Enable);
        }

        private delegate void OnButtonHandleDeleg(TagInventoryForm.ButtonState btnState, bool en);
        private void ButtonHandle(TagInventoryForm.ButtonState btnState, bool en)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new OnButtonHandleDeleg(ButtonHandle), new object[] { btnState , en});
                return;
            }
            switch (btnState)
            {
                case TagInventoryForm.ButtonState.Clear:
                    btn_clear.Enabled = en;
                    break;
                case TagInventoryForm.ButtonState.Save:
                    btn_save.Enabled = en;
                    break;
                case TagInventoryForm.ButtonState.Select:
                    btn_select.Enabled = en;
                    break;
                case TagInventoryForm.ButtonState.Start:
                    btn_once.Enabled = btn_run.Enabled = en;
                    break;
                case TagInventoryForm.ButtonState.Stop:
                    btn_stop.Enabled = en;
                    break;
                case TagInventoryForm.ButtonState.ALL:
                    this.Enabled = en;
                    break;
                default: break;
            }
        }

        private void ControlPanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.OnButtonEnable -= new EventHandler<TagInventoryForm.OnButtonClickEventArgs>(MainForm_OnButtonEnable);
        }

        private void cb_sql_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_sql.Checked)
            {
                _mainForm.Save2SQL();
            }
        }

        private void btn_clearcount_Click(object sender, EventArgs e)
        {
            if (_mainForm != null)
                _mainForm.ClearCount();
        }

        private void btn_dumpregs_Click(object sender, EventArgs e)
        {
            CSLibrary.HighLevelInterface.DEBUGMACREGISTER currentRegs;
            currentRegs = Program.ReaderXP.DebugMacReadRegisters();

            string CycleLogFileName = "RegistersLog-" + Program.SerialNumber.Replace(':', '.') + ".TXT";

            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

            tw.WriteLine("Mac Registers value : {0}", DateTime.Now);

            for (int cnt = 0x0000; cnt < 3; cnt++)
            {
                tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt, currentRegs._0000[cnt]);
            }

            for (int cnt = 0x0000; cnt < 2; cnt++)
            {
                tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0x500, currentRegs._0500[cnt]);
                //debugRegs._0500[cnt] = value;
            }

            for (int cnt = 0x0000; cnt < 4; cnt++)
            {
                tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0x600, currentRegs._0600[cnt]);
                //MacReadRegister((MacRegister)(cnt + 0x0600), ref value);
                //debugRegs._0600[cnt] = value;
            }

            tw.WriteLine("0x{0:x4} : 0x{1:x8}", 0x700, currentRegs._0700[0]);
            //MacReadRegister((MacRegister)0x0700, ref value);
            //debugRegs._0700[0] = value;

            for (uint cnt = 0x0000; cnt < 16; cnt++)
            {
                for (int cnt1 = 0x0000; cnt1 < 6; cnt1++)
                {
                    tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8}", cnt, cnt1 + 0x702, currentRegs._0702_707[cnt, cnt1]);
                    //MacReadRegister((MacRegister)(cnt + 0x0702), ref value);
                    //debugRegs._0702_707[cnt, cnt1] = value;
                }
            }

            for (uint cnt = 0x0000; cnt < 8; cnt++)
            {
                for (int cnt1 = 0x0000; cnt1 < 12; cnt1++)
                {
                    tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8}", cnt, cnt1 + 0x0801, currentRegs._0801_80c[cnt, cnt1]);
                    //MacReadRegister((MacRegister)(cnt + 0x0801), ref value);
                    //debugRegs._0801_80c[cnt, cnt1] = value;
                }
            }

            for (int cnt = 0x0000; cnt < 2; cnt++)
            {
                tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0x0900, currentRegs._0900[cnt]);
                //MacReadRegister((MacRegister)(cnt + 0x0900), ref value);
                //debugRegs._0900[cnt] = value;
            }

            for (uint cnt = 0x0000; cnt < 4; cnt++)
            {
                for (int cnt1 = 0x0000; cnt1 < 4; cnt1++)
                {
                    tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8}", cnt, cnt1 + 0x0903, currentRegs._0903_906[cnt, cnt1]);
                    //MacReadRegister((MacRegister)(cnt + 0x0903), ref value);
                    //debugRegs._0903_906[cnt, cnt1] = value;
                }
            }

            for (int cnt = 0x0000; cnt < 12; cnt++)
            {
                if (cnt == 0)
                    tw.WriteLine("0x{0:x4} : 0x{1:x8} (not for R2000)", cnt + 0x0910, currentRegs._0910_921[cnt]);
                else
                    tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0x0910, currentRegs._0910_921[cnt]);
                //MacReadRegister((MacRegister)(cnt + 0x0910), ref value);
                //debugRegs._0910_921[cnt] = value;
            }

            for (int cnt = 0x0000; cnt < 8; cnt++)
            {
                if (cnt == 0x00 || cnt >= 0x06)
                    tw.WriteLine("0x{0:x4} : 0x{1:x8} (not for R2000)", cnt + 0x0a00, currentRegs._0a00_a07[cnt]);
                else
                    tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0x0a00, currentRegs._0a00_a07[cnt]);
                //MacReadRegister((MacRegister)(cnt + 0x0a00), ref value);
                //debugRegs._0a00_a07[cnt] = value;
            }

            for (uint cnt = 0x0000; cnt < 8; cnt++)
            {
                for (int cnt1 = 0x0000; cnt1 < 16; cnt1++)
                {
                    tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8}", cnt, cnt1 + 0x0a09, currentRegs._0a09_a18[cnt, cnt1]);
                    //MacReadRegister((MacRegister)(cnt + 0x0a09), ref value);
                    //debugRegs._0a09_a18[cnt, cnt1] = value;
                }
            }

            for (int cnt = 0x0000; cnt < 0x85; cnt++)
            {
                if (cnt == 0x03 || cnt == 0x0c || cnt == 0x17 || cnt == 0x18 || cnt == 0x19 || cnt == 0x1f || cnt == 0x28 || cnt == 0x2d || cnt == 0x2e || cnt == 0x2f || (cnt >= 0x33 && cnt <= 0x3f) || (cnt >= 0x4b && cnt <= 0x4d) || cnt == 0x4f || (cnt >= 0x58 && cnt <= 0x5e) || (cnt >= 0x78 && cnt <= 0x7f))
                    tw.WriteLine("0x{0:x4} : 0x{1:x8} (not for R2000)", cnt + 0xb00, currentRegs._0b00[cnt]);
                else
                    tw.WriteLine("0x{0:x4} : 0x{1:x8}", cnt + 0xb00, currentRegs._0b00[cnt]);
                //MacReadRegister((MacRegister)(cnt + 0x0b00), ref value);
                //debugRegs._0b00[cnt] = value;
            }

            for (uint cnt = 0x0000; cnt < 50; cnt++)
            {
                for (int cnt1 = 0x0000; cnt1 < 6; cnt1++)
                {
                    if (cnt1 == 5)
                        tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8} (not for R2000)", cnt, cnt1 + 0xc02, currentRegs._0c02_c07[cnt, cnt1]);
                    else
                        tw.WriteLine("0x{0:x2} : 0x{1:x4} : 0x{2:x8}", cnt, cnt1 + 0xc02, currentRegs._0c02_c07[cnt, cnt1]);
                    //MacReadRegister((MacRegister)(cnt + 0x0c02), ref value);
                    //debugRegs._0c02_c07[cnt, cnt1] = value;
                }
            }

            tw.WriteLine("0x{0:x4} : 0x{1:x8}", 0x0c08, currentRegs._0c08[0]);
            //MacReadRegister((MacRegister)0x0c08, ref value);
            //debugRegs._0c08[0] = value;

            tw.WriteLine("0x{0:x4} : 0x{1:x8}", 0x0f0f, currentRegs._0f0f[0]);
            //MacReadRegister((MacRegister)0x0f0f, ref value);
            //debugRegs._0f0f[0] = value;
            
            //tw.WriteLine("{0} : System Reset", DateTime.Now.ToString());

            tw.Close();

            MessageBox.Show("Registers Dump Finish!!!", "Debug");

            //if (_mainForm != null)
            //    _mainForm.ClearCount();

        }
    }
}