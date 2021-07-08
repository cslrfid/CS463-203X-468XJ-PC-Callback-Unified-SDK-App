using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSLibrary;
using CSLibrary.Constants;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class FormOnOffTest : Form
    {
        DateTime RunningTime;
        uint CycleCnt;
        uint tps;
        uint nonZeroCount;
        uint zeroCount;
        uint reconnectCount;
        string logFile;
        bool saveToFile;

        public FormOnOffTest()
        {
            InitializeComponent();
            AttachCallback (true);
        }

        private void FormOnOffTest_Load(object sender, EventArgs e)
        {
            textBoxLogFile.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TagLogFile");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            issueTime = DateTime.Now;

            if (buttonStart.Text == "Start")
            {
                buttonStart.Text = "Stop";
                reconnectCount = 0;
                tps = 0;
                zeroCount = 0;
                nonZeroCount = 0;
                CycleCnt = 1;
                //logFile = textBoxLogFile.Text + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                logFile = textBoxLogFile.Text + issueTime.ToString("yyyyMMddHHmmss") + ".txt";
                saveToFile = checkBoxSavetoLog.Checked;
                LogWrite("Start Key pressed");
                RunningTime = DateTime.MinValue;
                timerReaderOn.Interval = int.Parse(textBox1.Text) * 60 * 1000;
                timerReaderOff.Interval = int.Parse(textBox2.Text) * 1000;
                timerReaderOn_Tick(null, null);
                timerTime.Start();
                labelStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                timerReaderOn.Start();
                //timerTime.Enabled = true;
                //timerReaderOn.Enabled = true;
            }
            else
            {
                LogWrite("Stop Key pressed");
                buttonStart.Text = "Start";
                timerTime.Stop();
                timerReaderOff.Stop();
                //timerTime.Enabled = false;
                //timerReaderOn.Enabled = false;
                Program.ReaderXP.StopOperation(true);
                labelReaderStatus.Text = "";
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            RunningTime = RunningTime.AddSeconds(1);
            var a = RunningTime.Hour;
            labelTime.Text = a.ToString() + RunningTime.ToString(":mm:ss");
            if (tps == 0)
                zeroCount++;
            else
                nonZeroCount++;
            labelNonZeroCount.Text = nonZeroCount.ToString();
            labelZeroCount.Text = zeroCount.ToString();
            labelTagsperSecond.Text = tps.ToString();
            labelDisconnectCount.Text = reconnectCount.ToString();
            tps = 0;
            labelReaderStatus.Text = Program.ReaderXP.State.ToString();
        }

        private void timerReaderOn_Tick(object sender, EventArgs e)
        {
            timerReaderOff.Start();
            labelCycle.Text = (CycleCnt++).ToString();
            Program.ReaderXP.Options.TagRanging.flags = SelectFlags.ZERO;

            if (Program.ReaderXP.State == RFState.DISCONNECTED || Program.ReaderXP.State == RFState.ABORT)
            {
                issueTime = DateTime.Now;
                reconnectCount++;
                LogWrite("Network disconnected");
                LogWrite("Network reconnect");
                Program.ReaderXP.Reconnect(5);
            }

            if (Program.ReaderXP.State == RFState.IDLE)
            {
                Program.ReaderXP.SetOperationMode(RadioOperationMode.CONTINUOUS);
                Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, false);
            }
        }

        private void timerReaderOff_Tick(object sender, EventArgs e)
        {
            timerReaderOff.Stop();
            Program.ReaderXP.StopOperation(true);
        }

        private void FormOnOffTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Fourth Step (Dettach from Form and Stop)
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                //mStop = e.Cancel = true;
                Program.ReaderXP.StopOperation(true);
            }

            AttachCallback(false);
        }

        private void AttachCallback(bool en)
        {
            if (en)
            {
                Program.ReaderXP.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
                Program.ReaderXP.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagInventoryEvent);
            }
            else
            {
                Program.ReaderXP.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagInventoryEvent);
                Program.ReaderXP.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
            }
        }

        void ReaderXP_TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            if (e.type == CallbackType.TAG_RANGING)
            {
                tps++;
                LogWrite("RSSI = " + e.info.rssi.ToString("0.0") + " PC = " + e.info.pc.ToString() + " EPC = " + e.info.epc.ToString());
            }
        }

        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
           switch (e.state)
           {
               case RFState.RESET:
                   reconnectCount++;
                   LogWrite("Inventory disconnected");
                   LogWrite("Network reconnect");
                   Program.ReaderXP.Reconnect(5);
                   break;
           }
        }

        DateTime issueTime;

        private delegate void LogWriteDeleg(string msg);
        void LogWrite(string msg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LogWriteDeleg(LogWrite), new object[] { msg });
                return;
            }
            
            issueTime = DateTime.Now;
            if (saveToFile)
            {
                //this.BeginInvoke((System.Threading.ThreadStart)delegate()
                {
                    if (System.IO.File.Exists(logFile))
                    {
                        if (new System.IO.FileInfo(logFile).Length > 5000000)
                        {
                            //logFile = textBoxLogFile.Text + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                            logFile = textBoxLogFile.Text + issueTime.ToString("yyyyMMddHHmmss") + ".txt";
                        }
                    }

                    //System.IO.File.AppendAllText(logFile, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss : ") + msg + Environment.NewLine);
                    System.IO.File.AppendAllText(logFile, issueTime.ToString("yyyy/MM/dd HH:mm:ss : ") + msg + Environment.NewLine);
                }//);
            }
        }
    }
}
