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

namespace CS203_CALLBACK_API_DEMO
{
    using CSLibrary.Constants;
    using CSLibrary.Structures;
    using CSLibrary.Text;

    public partial class GeigerSearchForm : Form
    {
        #region Private Member
#if CS101
        private RingTone[] RingMelody = new RingTone[4]
            {
                RingTone.T1,
                RingTone.T1,
                RingTone.T3,
                RingTone.T5,
            };
#endif
        private int[] ThresholdArr = new int[] { 0, 50, 65, 70};
        private float rssi = 0;
        private float rssimin = 999;
        private float rssimax = -999;
        private bool bTone = true;
        private string TargetEPC = "";
        private bool m_stop = false;

        #endregion

        #region Form

        CS203_CALLBACK_API_DEMO.RSSI90 rssi90 = new RSSI90();

        public GeigerSearchForm()
        {
            InitializeComponent();
            radioButton_dBm.Checked = true;
        }
        private void GeigerSearchForm_Load(object sender, EventArgs e)
        {
            lb_threshold.Text = tk_threshold.Value.ToString();
            tb_epc.Text = TargetEPC;
            AttachCallback(true);
        }

        private void GeigerSearchForm_Closing(object sender, CancelEventArgs e)
        {
            tmr_tone.Enabled = tmr_ZeroDetector.Enabled = false;
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                e.Cancel = m_stop = true;
                Program.ReaderXP.StopOperation(true);
            }
            else
            {
                AttachCallback(false);
            }
        }
        #endregion

        #region Button Handle

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                Program.ReaderXP.StopOperation(true);
                return;
            }

            rssimin = 999;
            rssimax = -999;
            rssi90.ClearData();
            Start();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Stop()
        {
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                Program.ReaderXP.StopOperation(true);
            }
        }

        private void Start()
        {
            if (Program.ReaderXP.State == RFState.IDLE)
            {
                Program.ReaderXP.Options.TagSelected.flags = SelectMaskFlags.ENABLE_TOGGLE;
                Program.ReaderXP.Options.TagSelected.epcMask = new S_MASK(tb_epc.Text);
                Program.ReaderXP.Options.TagSelected.epcMaskLength = Hex.GetBitCount(tb_epc.Text);

                Program.ReaderXP.StartOperation(Operation.TAG_SELECTED, true);

                Program.ReaderXP.SetOperationMode(Program.appSetting.Cfg_continuous_mode ? RadioOperationMode.CONTINUOUS : RadioOperationMode.NONCONTINUOUS);
                Program.ReaderXP.Options.TagSearchOne.avgRssi = cb_averaging.Checked;
                Program.ReaderXP.StartOperation(Operation.TAG_SEARCHING, false);
            }
        }

        private void btn_scan_Click(object sender, EventArgs e)
        {
            //Stop current operation
            if (Program.ReaderXP.State == RFState.BUSY)
            {
                Program.ReaderXP.StopOperation(true);
            }
            while (Program.ReaderXP.State != RFState.IDLE)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }

            AttachCallback(false);

            using (TagSearchForm inv = new TagSearchForm(radioButton_dBm.Checked))
            {
                if (inv.ShowDialog() == DialogResult.OK)
                {
                    TargetEPC = tb_epc.Text = inv.epc;
                }
            }
            AttachCallback(true);
        }

        #endregion

        #region Timer Handle

        void StopOORDetectTmr()
        {
            tmr_ZeroDetector.Enabled = false;
        }

        private void tmr_ZeroDetector_Tick(object sender, EventArgs e)
        {
            StopOORDetectTmr();
            UpdateRssiLb(String.Empty, String.Empty, String.Empty);
            UpdateProgressValue(0);
            tmr_tone.Enabled = false;
        }
        private void tmr_tone_Tick(object sender, EventArgs e)
        {
            if (bTone)
            {
                if (rssi > ThresholdArr[3])
                {
#if CS101
                    Device.MelodyPlay(RingMelody[3], BUZZER_SOUND.HIGH);
#else
                    Console.Beep((int)523.2, 300);
#endif
                }
                else if (rssi <= ThresholdArr[3] && rssi > ThresholdArr[2])
                {
#if CS101
                    Device.MelodyPlay(RingMelody[2], BUZZER_SOUND.HIGH);
#else
                    Console.Beep((int)523.2, 300);
#endif
                }
                else if (rssi <= ThresholdArr[2] && rssi > ThresholdArr[1])
                {
#if CS101
                    Device.MelodyPlay(RingMelody[1], BUZZER_SOUND.HIGH);
#else
                    Console.Beep((int)523.2, 300);
#endif
                }
                else if (rssi <= ThresholdArr[1])
                {
#if CS101
                    Device.MelodyPlay(RingMelody[0], BUZZER_SOUND.HIGH);
#else
                    Console.Beep((int)523.2, 300);
#endif
                }
            }


            label_rssi90.Text = rssi90.GetRssi90();
        }

        #endregion

        #region Event Callback
        private void AttachCallback(bool en)
        {
            if (en)
            {
                Program.ReaderXP.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagSearchOneEvent);
                Program.ReaderXP.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
            }
            else
            {
                Program.ReaderXP.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagSearchOneEvent);
                Program.ReaderXP.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
            }
        }

        void ReaderXP_TagSearchOneEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            this.Invoke((System.Threading.ThreadStart)delegate()
            {
                switch (e.type)
                {
                    case CallbackType.TAG_SEARCHING:
                        rssi = ((TagCallbackInfo)e.info).rssi;

                        if (radioButton_dBm.Checked)
                            rssi -= 106.989F;

                        if (rssi < rssimin)
                            rssimin = rssi;

                        if (rssi > rssimax)
                            rssimax = rssi;

                        rssi90.Add(rssi);
                        
                        UpdateRssiLb(((int)rssi).ToString(), ((int)rssimin).ToString(), ((int)rssimax).ToString());
                        //lb_rssi.Text = rssi.ToString();

                        UpdateProgressValue((int)rssi);

                        //Tone
                        EnToneTmr(true);

                        if ((radioButton1.Checked && rssi < 0) || (radioButton_dBm.Checked && rssi < -107))
                            UpdateRssiLb(String.Empty, String.Empty, String.Empty);
                        else
                            RestartOORDetectTmr();
                        break;
                }
            });
        }

        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            this.Invoke((System.Threading.ThreadStart)delegate()
            {
                switch (e.state)
                {
                    case RFState.IDLE:
                        if (!m_stop)
                        {
                            //Device.MelodyPlay(RingTone.T1, BUZZER_SOUND.HIGH);
                            UpdateStartButton(true);
                            EnableForm(true);
                        }
                        else
                        {
                            CloseForm();
                        }
                        break;
                    case RFState.BUSY:
                        //Device.MelodyPlay(RingTone.T2, BUZZER_SOUND.HIGH);
                        UpdateStartButton(false);
                        break;
                    case RFState.ABORT:
                        EnableForm(false);
                        break;
                }
            });
        }

        #endregion

        #region Delegate
        private delegate void UpdateStartButtonDeleg(bool start);
        private void UpdateStartButton(bool start)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateStartButtonDeleg(UpdateStartButton), new object[] { start });
                return;
            }
            if (start)
            {
                btn_start.Text = "Start";
                btn_start.BackColor = Color.FromArgb(0, 192, 0);
                btn_start.ForeColor = Color.Black;
                cb_averaging.Enabled = true;
                tmr_tone.Enabled = false;
                tmr_ZeroDetector.Enabled = false;
            }
            else
            {
                btn_start.Text = "Stop";
                btn_start.BackColor = Color.Red;
                btn_start.ForeColor = Color.White;
                cb_averaging.Enabled = false;
                tmr_tone.Enabled = true;
                tmr_ZeroDetector.Enabled = true;
            }
        }
        private delegate void RestartOORDetectTmrDeleg();
        void RestartOORDetectTmr()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RestartOORDetectTmrDeleg(RestartOORDetectTmr), new object[] { });
                return;
            }
            tmr_ZeroDetector.Enabled = false;
            tmr_ZeroDetector.Enabled = true; // OK?
        }
        private delegate void ShowMsgDeleg(string msg);
        private void ShowMsg(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowMsgDeleg(ShowMsg), new object[] { msg });
                return;
            }
            MessageBox.Show(msg);
        }
        private delegate void EnToneTmrDeleg(bool en);
        private void EnToneTmr(bool en)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnToneTmrDeleg(EnToneTmr), new object[] { en });
                return;
            }
            if (tmr_tone.Enabled != en)
                tmr_tone.Enabled = en;
        }

        private delegate void UpdateRssiLbDeleg(string msg, string msg1, string msg2);
        private void UpdateRssiLb(string msg, string msg1, string msg2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateRssiLbDeleg(UpdateRssiLb), new object[] { msg, msg1, msg2});
                return;
            }
            lb_rssi.Text = msg;
            label_minRSSI.Text = msg1;
            label_maxRSSI.Text = msg2;
        }

        private delegate void UpdateProgressValueDeleg(int value);
        private void UpdateProgressValue(int value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateProgressValueDeleg(UpdateProgressValue), new object[] { value });
                return;
            }
            if (value > pg_rssi.Maximum)
                pg_rssi.Value = pg_rssi.Maximum;
            else if (value < pg_rssi.Minimum)
                pg_rssi.Value = pg_rssi.Minimum;
            else
                pg_rssi.Value = value;
        }
        private delegate void CloseFormDeleg();
        private void CloseForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CloseFormDeleg(CloseForm), new object[] { });
                return;
            }
            this.Close();
        }
        private delegate void EnableFormDeleg(bool en);
        private void EnableForm(bool en)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableFormDeleg(EnableForm), new object[] { en });
                return;
            }
            this.Enabled = en;
        }
        #endregion

        #region Other Handle
        private void tk_threshold_ValueChanged(object sender, EventArgs e)
        {
            ThresholdArr[3] = tk_threshold.Value;
            lb_threshold.Text = tk_threshold.Value.ToString();
        }
        private void cb_sound_CheckStateChanged(object sender, EventArgs e)
        {
            bTone = cb_sound.Checked;
        }

        private void tb_epc_TextChanged(object sender, EventArgs e)
        {
            TargetEPC = tb_epc.Text;
        }
        #endregion

        private void radioButton_dBm_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_dBm.Checked)
            {
                tk_threshold.Minimum = -100;
                tk_threshold.Maximum = 0;
                tk_threshold.Value = 75 - 107;
                pg_rssi.Minimum = -100;
                pg_rssi.Maximum = 0;
                label1.Text = "-100";
                label3.Text = "-50";
                label2.Text = "0";
                rssimin = 999;
                rssimax = -999;
                rssi90.ClearData();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                tk_threshold.Minimum = 0;
                tk_threshold.Maximum = 100;
                tk_threshold.Value = 75;
                pg_rssi.Minimum = 0;
                pg_rssi.Maximum = 100;
                label1.Text = "0";
                label3.Text = "50";
                label2.Text = "100";
                rssimin = 999;
                rssimax = -999;
                rssi90.ClearData();
            }
        }

        private void label_minRSSI_Click(object sender, EventArgs e)
        {

        }

        private void label_rssi90_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            rssimin = 999;
            rssimax = -999;
            rssi90.ClearData();
        }
    }

    public class RSSI90
    {
        private List <float> rssiArray = new List <float>();

        public RSSI90()
        {
            ClearData();
        }

        public void ClearData ()
        {
            lock (rssiArray)
                rssiArray.Clear();
        }

        public void Add (float value)
        {
            lock (rssiArray)
            {
                if (rssiArray.Count >= 100)
                    rssiArray.RemoveAt (0);
                rssiArray.Add(value);
            }
        }

        public string GetRssi90()
        {
            List<float> tmp;

            lock (rssiArray)
            {
                if (rssiArray.Count < 100)
                    return "not ready";
                
                tmp = (new List<float>(rssiArray));
            }

            tmp.Sort();

#if debugrssi90
            foreach (float data in tmp)
                Console.WriteLine(data);
            Console.WriteLine("----------------------");
#endif

            return tmp[90].ToString("0.00");
        }
    }
}