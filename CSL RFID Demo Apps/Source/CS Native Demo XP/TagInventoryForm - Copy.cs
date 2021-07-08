using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace CS203_CALLBACK_API_DEMO
{
    using CSLibrary;
    using CSLibrary.Constants;
    using CSLibrary.Structures;
    using CSLibrary.Tools;

    public partial class TagInventoryForm : Form
    {
        #region Private Member
        private SQLMethod sqlMethod = null;

        private Thread reset;
        private long totaltags = 0;
        private long totaltagsthiscycle = 0;
        private long [] totaltagsthiscycleperantenna = new long[16];
        private long totaltagscycle = 0;
        private long totalcrc = 0;
        private Stopwatch watchrate = new Stopwatch();
        private object MyLock = new object();
        private object MyLock1 = new object();
        private bool selectMode = false;
        public string EPC = "";
        private bool mStop = false;
        private int lastRingTick = 0;

        private uint AntCycleCount = 0;
        private int[] AntCycleTime = new int[5];
        private int AntCycleTimeCount = 0;

        public string TagLogFileName;
        
        private int numChannelBusy = 0;

        private bool SaveSQL = false;

        public enum ButtonState : int
        {
            Start = 0,
            Stop,
            Select,
            Save,
            Clear,
            Exit,
            ALL,
            Unknow
        }

        private Custom.Control.SortColumnHeader m_colhdrIndex;
        private Custom.Control.SortColumnHeader m_colhdrPc;
        private Custom.Control.SortColumnHeader m_colhdrXPc_W1;
        private Custom.Control.SortColumnHeader m_colhdrXPc_W2;
        private Custom.Control.SortColumnHeader m_colhdrEpc;
        private Custom.Control.SortColumnHeader m_colhdrTid;
        private Custom.Control.SortColumnHeader m_colhdrUser;
        private Custom.Control.SortColumnHeader m_colhdrRssi;
        private Custom.Control.SortColumnHeader m_colhdrCount;
        private Custom.Control.SortColumnHeader m_colhdrAntennaPort;
        private Custom.Control.SortColumnHeader m_colhdrFreqChannel;
        private Custom.Control.SortColumnHeader m_colhdrCrc16;

        private Custom.Control.SortListView m_sortListView;

        private List<CSLibrary.Structures.TagCallbackInfo> InventoryListItems = new List<CSLibrary.Structures.TagCallbackInfo>();

        private List<CSLibrary.Structures.TagCallbackInfo> lock_InvItems
        {
            get { lock (MyLock1) { return InventoryListItems; } }
            set { lock (MyLock1) { InventoryListItems = value; } }
        }

        private bool _InventoryRunning = false;

        #endregion

        #region Public Member
        public bool SelectMode
        {
            get { return selectMode; }
        }

        public class OnButtonClickEventArgs : EventArgs
        {
            private ButtonState state = ButtonState.Unknow;
            private bool enable = false;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Stat">Input TagAccess Data</param>
            public OnButtonClickEventArgs(ButtonState Stat, bool Enable)
            {
                state = Stat;
                enable = Enable;
            }
            /// <summary>
            /// State
            /// </summary>
            public ButtonState State
            {
                get { return state; }
                set { state = value; }
            }
            public bool Enable
            {
                get { return enable; }
            }
        }
        public event EventHandler<OnButtonClickEventArgs> OnButtonEnable;

        string CycleLogFileName = "CycleLog-" + Program.SerialNumber.Replace(':', '.') + ".TXT";

        #endregion

        #region Form
        public TagInventoryForm(bool SelectMode)
        {
            InitializeComponent();
            InitListView();
            selectMode = SelectMode;
            try
            {
                sqlMethod = new SQLMethod();
            }
            catch
            {
                MessageBox.Show("SQL start error");
            }
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            //Show MAC and IP
            Text = String.Format("IP = {0}, Serial = {1}", Program.IP, Program.SerialNumber);

            //Third Step (Attach to Form)
            AttachCallback(true);

            ControlPanelForm.LaunchControlPanel(this);

        }

        private void InventoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Fourth Step (Dettach from Form and Stop)
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                mStop = e.Cancel = true;
                Program.ReaderXP.StopOperation(true);
            }
            else
            {
                AttachCallback(false);
            }
        }

 
        #endregion

        #region Event Callback
        private void Reset()
        {
            Result rc = Result.OK;

            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
               panelNetworkDisconnet.Location = new Point((this.Size.Width - panelNetworkDisconnet.Size.Width) / 2, (this.Size.Height - panelNetworkDisconnet.Size.Height) / 2);
               panelNetworkDisconnet.Visible = true;
               //MessageForm.LaunchForm(this);
               {
                   EnableButton(ButtonState.ALL, false);
                   Application.DoEvents();
               RETRY:
                   //Reset Reader first, it will shutdown current reader and restart reader
                   //It will also reconfig back previous operation

                   while (Program.ReaderXP.Reconnect(1) != Result.OK) ;
                   EnableButton(ButtonState.ALL, true);
               }

               panelNetworkDisconnet.Visible = false;
            });

            //MessageForm.msgform.CloseForm();
        }

        private void ResetInventory()
        {
            Result rc = Result.OK;

            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
                panelNetworkDisconnet.Location = new Point((this.Size.Width - panelNetworkDisconnet.Size.Width) / 2, (this.Size.Height - panelNetworkDisconnet.Size.Height) / 2);
                panelNetworkDisconnet.Visible = true;
                //MessageForm.LaunchForm(this);
                {
                    EnableButton(ButtonState.ALL, false);
                    Application.DoEvents();
                RETRY:
                    //Reset Reader first, it will shutdown current reader and restart reader
                    //It will also reconfig back previous operation

                    while (Program.ReaderXP.Reconnect(1) != Result.OK) ;

                    timer_ElapsedTime.Enabled = true;
                    Program.ReaderXP.StartOperation(Operation.TAG_RANGING, false);

                    EnableButton(ButtonState.ALL, true);
                }

                panelNetworkDisconnet.Visible = false;
            });

            //MessageForm.msgform.CloseForm();
        }

        private delegate DialogResult ShowMsgDeleg(string msg);
        private DialogResult ShowMsg(string msg)
        {
            if (this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(new ShowMsgDeleg(ShowMsg), new object[] { msg });
                //return DialogResult.None;
            }
            return MessageBox.Show(msg, "Retry", MessageBoxButtons.YesNo);
        }

        public void AbortReset()
        {
            if (reset != null && reset.IsAlive)
            {
                reset.Abort();
            }
        }
        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            // Diagnostics
            {
                switch (e.state)
                {
                    case RFState.INVENTORY_MAC_ERROR:
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            tw.WriteLine("{0} : Inventory MAC Error {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), Program.ReaderXP.LastMacErrorCode.ToString("X4") + Environment.NewLine);

                            tw.Close();
                        }
                        break;


                    case RFState.INVENTORY_CYCLE_BEGIN:
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            tw.WriteLine("{0} : Inventory Cycle Begin : # Tags {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), _tagPerInvCyc);

                            tw.Close();

                            _tagPerInvCyc = 0;
                        }
                        break;

                    case RFState.INVENTORY_ROUND_BEGIN:
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            tw.WriteLine("{0} : Inventory Round Begin", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));

                            tw.Close();
                        }
                        break;

                    case RFState.INVENTORY_ROUND_END:
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            tw.WriteLine("{0} : Inventory Round End : # Tags {1} {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), _tagInvRndCnt, (_tagInvRndCnt == 0 ? "!!!!!!!!!!" : ""));

                            tw.Close();

                            _tagInvRndCnt = 0;
                        }
                        break;

                    case RFState.INVENTORY_ROUND_BEGIN_DIAGNOSTICS: //  Inventory Round Begin   Atmel Time = YYYY ms
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            tw.WriteLine("{0} : Inventory Round Begin Diagnostics   Atmel Time = {1} ms", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), Program.ReaderXP._Debug_Inventory_Round_Begin_Diagnostics_Atmel_Time);

                            tw.Close();
                        }
                        break;

                    case RFState.INVENTORY_ROUND_END_DIAGNOSTICS: // Time of Logging :  Inventory Round End      Atmel Time = ZZZZ ms; Round Time = AAAA ms; EPC Tag = BB; RN16 = CC; RN16 Tout = DD; �Empty Round� (if Round Time > 30 ms and  EPC Tag = 0) 
                        if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                        {
                            TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                            string RoundTime;
                            UInt32 a = 0;

                            if (Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_Atmel_Time < Program.ReaderXP._Debug_Inventory_Round_Begin_Diagnostics_Atmel_Time)
                            {
                                RoundTime = "Native value";
                            }
                            else
                            {
                                a = (Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_Atmel_Time - Program.ReaderXP._Debug_Inventory_Round_Begin_Diagnostics_Atmel_Time);
                                RoundTime = a.ToString();
                            }

                            tw.WriteLine("{0} : Inventory Round End Diagnostics   Atmel Time = {1} ms; Round Time = {2} ms; EPC Tag = {3}; RN16 = {4}; RN16 Tout = {5}; {6}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_Atmel_Time, RoundTime, Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_EPC_successfully_read, Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_RN16, Program.ReaderXP._Debug_Inventory_Round_End_Diagnostics_RN16_timeout, ((a > 250 && _tagInvRndCnt == 0) ? "Empty Round" : ""));

                            tw.Close();

                            _tagInvRndCnt = 0;
                        }
                        break;
                }
            }

            this.BeginInvoke((System.Threading.ThreadStart)delegate()
               {
                   switch (e.state)
                   {
                       case RFState.IDLE:
                           timer_ElapsedTime.Enabled = false;
                           watchrate.Stop();

                           if (Program.ReaderXP.LastMacErrorCode != 0)
                           {
                               string errMsg = "Mac Error Code 0x" + Program.ReaderXP.LastMacErrorCode.ToString("X4");

                               TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                               tw.WriteLine("{0} : " + errMsg, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));

                               tw.Close();

                               MessageBox.Show(errMsg, "Inventory Error");
                           }

                           //ControlPanelForm.EnablePannel(true);
                           EnableButton(ButtonState.Start, true);
                           EnableButton(ButtonState.Stop, false);
                           EnableTimer(false);
                           if (mStop)
                               this.Close();
                           break;
                       case RFState.BUSY:
                           watchrate.Reset();
                           watchrate.Start();
                           totaltags = 0;
                           totalcrc = 0;
                           EnableButton(ButtonState.Start, false);
                           EnableButton(ButtonState.Stop, true);
                           EnableTimer(true);
                           break;
                       case RFState.RESET:
                           timer_ElapsedTime.Enabled = false;
                           if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                           {
                               totaltagscycle = totaltagsthiscycle;
                               totaltagsthiscycle = 0;

                               //TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "\\CycleLog.Txt", true);
                               TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                               tw.WriteLine("{0} : System Reset, Last Tag #{1}, {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), totaltagscycle, logTagCntPerCycle() + Environment.NewLine);

                               tw.Close();
                           }

                           Program.ReaderXP.EngWriteRegister(0x201, 0x0b);

                           //Use other thread to create progress
                           ResetInventory();
                           //reset = new Thread(new ThreadStart(Reset));
                           //reset.Start();

                           break;

                       case RFState.DISCONNECTED:
                           if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                           {
                               //TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "\\CycleLog.Txt", true);
                               TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                               tw.WriteLine("{0} : System Reset", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));

                               tw.Close();
                           }

                           Reset();
                           break;

                       case RFState.ABORT:
                           //ControlPanelForm.EnablePannel(false);
                           break;

                       case RFState.ANT_CYCLE_END:

                           //invalidFilterEPC.Clear ();
                           //invalidFilterCount.Clear();

                           AntCycleCount++;
                           totaltagscycle = totaltagsthiscycle;
                           totaltagsthiscycle = 0;

                           if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                           //                               ControlPanelForm.ControlPanel.checkBoxSaveSummary.Checked)
                           {
                               int portbusy = 0;
                               int common = 0;

                               //TextWriter tw = new StreamWriter(ControlPanelForm.SummaryFile, true);
                               //TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "\\CycleLog.Txt", true);
                               TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                               tw.Write("{0} : Antenna Cycle {1}, {2} Tag/Cycle, {3}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), AntCycleCount, totaltagscycle, logTagCntPerCycle() + Environment.NewLine);

                               /*
                               for (int cnt = 0; cnt < 16; cnt++)
                               {
                                   if (Program.ReaderXP.ChannelStatus[cnt] == RFState.CH_BUSY)
                                   {
                                       portbusy = 1;
                                       if (common == 0)
                                           tw.Write("Port Busy ");
                                       else
                                           tw.Write(", ");
                                       tw.Write("{0}", cnt);
                                       common = 1;
                                   }

                               }

                               if (portbusy == 0)
                                   tw.WriteLine("CLEAR");
                               else
                                   tw.WriteLine("");
                               */

                               tw.Close();
                           }
                           numChannelBusy = 0;

                           if (AntCycleTimeCount <= 4)
                           {
                               AntCycleTime[AntCycleTimeCount++] = Environment.TickCount;
                           }
                           else
                           {
                               AntCycleTime[0] = AntCycleTime[1];
                               AntCycleTime[1] = AntCycleTime[2];
                               AntCycleTime[2] = AntCycleTime[3];
                               AntCycleTime[3] = AntCycleTime[4];
                               AntCycleTime[4] = Environment.TickCount;
                           }

                           break;

                   }
               });
        }

        uint _tagPerInvCyc = 0;
        uint _tagInvRndCnt = 0;

        string logTagCntPerCycle ()
        {
            string taglog = "";
            //long totalTags = totaltagsthiscycle;
            //totaltagsthiscycle = 0;
            long [] totalTagPerAntenna = new long[16]; 

            int numofantenna;


            switch (Program.ReaderXP.OEMDeviceType)
            {
                case Machine.CS468:
                case Machine.CS468INT:
                    numofantenna = 16;
                    break;

                case Machine.CS463:
                case Machine.CS469:
                    numofantenna = 4;
                    break;

                default :
                    numofantenna = 1;
                    break;
            }

            for (int cnt = 0; cnt < numofantenna; cnt ++)
            {
                totalTagPerAntenna [cnt] = totaltagsthiscycleperantenna[cnt];
                totaltagsthiscycleperantenna[cnt] = 0;
            }

            //taglog = totalTags.ToString() + ", ";



            for (int cnt = 0; cnt < numofantenna; cnt++)
            {
                if (Program.appSetting.AntennaList[cnt].AntennaStatus.state == AntennaPortState.ENABLED)
                {
                    taglog += "P" + cnt.ToString() + ":" + totalTagPerAntenna[cnt] + " ";
                }
            }

            return taglog;
        }

        const string DATETIMEFORMAT = "yyyy/MM/dd HH:mm:ss.fff UTC zzz";

        void TagLogFile_SetName ()
        {
            //TagLogFileName = CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "TagLog" + CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.TagLogFileCurrentNumber + (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.radioButton_CSV.Checked ? ".csv" : ".txt");
            TagLogFileName = CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "TagLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.radioButton_CSV.Checked ? ".csv" : ".txt");
        }


        //List <string> invalidFilterEPC = new List<string> ();
        //List <int> invalidFilterCount = new List<int> ();

        void ReaderXP_TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            /*if (!e.info.crcInvalid)
            {
                int thisTick = Environment.TickCount;
                if ((thisTick - lastRingTick) > 250)
                {
                    lastRingTick = thisTick;
                    System.Media.SystemSounds.Beep.Play();
                }
            }*/
            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
                // Do your work here
                // UI refresh and data processing on other Thread
                // Notes :  blocking here will cause problem
                //          Please use asyn call or separate thread to refresh UI

#if nouse
                { // special filter for Macao customer
                    TagCallbackInfo data = e.info;
                    int cnt = 0;

                    if (data.epcstrlen != 6)
                        return;

                    for (cnt = 0; cnt < invalidFilterEPC.Count; cnt++)
                        if (invalidFilterEPC[cnt] == data.epc.ToString ())
                        {
                            if (invalidFilterCount[cnt] < 5)
                            {
                                invalidFilterCount[cnt]++;
                                return;
                            }

                            break;
                        }

                    if (cnt == invalidFilterEPC.Count)
                    {
                        invalidFilterEPC.Add(data.epc.ToString ());
                        invalidFilterCount.Add(0);
                        return;
                    }
                }
#endif
                if (!e.info.crcInvalid)
                {
                    if (e.type == CallbackType.TAG_EASALARM)
                    {
                        string EPCONLY = "EAS Alarm Tag Discovered";

                        Interlocked.Increment(ref totaltagsthiscycle);
                        Interlocked.Increment(ref totaltags);

                        int foundIndex = lock_InvItems.FindIndex(delegate(CSLibrary.Structures.TagCallbackInfo iepc) { return (iepc.epc.ToString() == e.info.epc.ToString() && (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBox_GroupTags.Checked ? true : iepc.antennaPort == e.info.antennaPort)); });
                        {
                            if (foundIndex >= 0)
                            {
                                //found a record
                                lock_InvItems[foundIndex].count++;
                                lock_InvItems[foundIndex].antennaPort = e.info.antennaPort;

                                //UI update in separate thread
                                UpdateListView(lock_InvItems[foundIndex]);
                                WriteTagLog(e.info.receiveTime, EPCONLY, "", "", e.info.rssi, e.info.antennaPort, e.info.freqChannel, lock_InvItems[foundIndex].count);
                            }
                            else
                            {
                                //record no exist
                                //add a record to item list
                                e.info.index = lock_InvItems.Count;
                                lock_InvItems.Add(e.info);
                                string TagDataStr = e.info.epc.ToString();
                                string TIDONLY = "";
                                string USERONLY = "";

                                //UI update in separate thread
                                LVAddItem(
                                    e.info.index.ToString(),
                                    "",
                                    "",
                                    "",
                                    EPCONLY,
                                    "",
                                    "",
                                    "",
                                    e.info.count.ToString(),
                                    e.info.antennaPort.ToString(),
                                    "",
                                    e.info.crc16.ToString("X4"));
                                if (SaveSQL)
                                {
                                    sqlMethod.AddData(EPCONLY);
                                }
                                //WriteTagLog(e.info.receiveTime, EPCONLY, e.info.rssi, e.info.antennaPort, e.info.freqChannel, 0);
                            }
                        }
                    }
                    else
                    {
                        if ((!Program.appSetting.EnableRssiFilter) ||
                            (Program.appSetting.EnableRssiFilter && Program.appSetting.RssiFilterThreshold < e.info.rssi))
                        {
                            totaltagsthiscycleperantenna[e.info.antennaPort]++;
                            _tagPerInvCyc++;
                            _tagInvRndCnt++;
                            
                            Interlocked.Increment(ref totaltagsthiscycle);
                            Interlocked.Increment(ref totaltags);
                            TagCallbackInfo data = e.info;
                            
                            UpdateInvUI(data);
                        }
#if nouse
                    if (Program.appSetting.EnableRssiFilter)
                    {
                        if (Program.appSetting.RssiFilterThreshold < e.info.rssi)
                        {
                            Interlocked.Increment(ref totaltags);
                            TagCallbackInfo data = e.info;
                            UpdateInvUI(data);
                        }
                    }
                    else
                    {
                        Interlocked.Increment(ref totaltags);
                        TagCallbackInfo data = e.info;
                        UpdateInvUI(data);
                    }
#endif

                    }
                }
                else
                {
                    totalcrc++;
                }
#if nouse

                if (Program.tagLogger != null)
                {
                    Program.tagLogger.Log(CSLibrary.Diagnostics.LogLevel.Info, String.Format("CRC[{0}]:PC[{1}]:EPC[{2}]", e.info.crcInvalid, e.info.pc, e.info.epc));
                }
#endif
            });
        }

        void WriteTagLog(DateTime TagReceiveTime, string epc, string bank1, string bank2, float rssi, uint antennaPort, uint freqChannel, uint count)
        {
            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxTagLog.Checked)
            {
                FileInfo f = new FileInfo(TagLogFileName);

                if (f.Exists)
                {
                    long s1 = f.Length;

                    if (f.Length > CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.TagLogFileSizeLimit)
                    {
                        TagLogFile_SetName();
                    }
                }

                {
                    TextWriter tw = new StreamWriter(TagLogFileName, true);
                    tw.WriteLine(epc + "," + bank1 + "," + bank2 + "," + TagReceiveTime.ToString(DATETIMEFORMAT) + "," + rssi.ToString("F1") + "," + count + "," + antennaPort + "," + freqChannel.ToString() + "," + Program.SerialNumber);
                    tw.Close();
                }
                //Program.tagLogger.Log(CSLibrary.Diagnostics.LogLevel.Info, String.Format("CRC[{0}]:PC[{1}]:EPC[{2}]", e.info.crcInvalid, e.info.pc, e.info.epc));                    
            }
        }

        
        
        private void UpdateInvUI(TagCallbackInfo InventoryInformation)
        {
            if (InventoryInformation.crcInvalid == true)
                return;
            //lock (MyLock)
            {
                int foundIndex = lock_InvItems.FindIndex(delegate(CSLibrary.Structures.TagCallbackInfo iepc) { return (iepc.epc.ToString() == InventoryInformation.epc.ToString() && (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBox_GroupTags.Checked ? true : iepc.antennaPort == InventoryInformation.antennaPort)); });
                {
                    if (foundIndex >= 0)
                    {
                        //found a record
                        lock_InvItems[foundIndex].count++;
                        lock_InvItems[foundIndex].rssi = InventoryInformation.rssi;
                        lock_InvItems[foundIndex].index = foundIndex;
                        lock_InvItems[foundIndex].antennaPort = InventoryInformation.antennaPort;
                        lock_InvItems[foundIndex].freqChannel = InventoryInformation.freqChannel;

                        //UI update in separate thread
                        UpdateListView(lock_InvItems[foundIndex]);

                        string TagDataStr = InventoryInformation.epc.ToString();
                        string EPCONLY;
                        string TIDONLY = "";
                        string USERONLY = "";

                        if (TagDataStr.Length > InventoryInformation.pc.EPCLength * 4)
                        {
                            EPCONLY = TagDataStr.Substring(0, (int)InventoryInformation.pc.EPCLength * 4);

                            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxReadTID.Checked)
                                TIDONLY = TagDataStr.Substring(EPCONLY.ToString().Length, System.Convert.ToInt16(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBoxTIDcount.Text) * 4);

                            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxReadUser.Checked)
                                USERONLY = TagDataStr.Substring(EPCONLY.ToString().Length + TIDONLY.Length, System.Convert.ToInt16(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBoxUsercount.Text) * 4);
                        }
                        else
                            EPCONLY = TagDataStr;

                        WriteTagLog(InventoryInformation.receiveTime, EPCONLY, TIDONLY, USERONLY, InventoryInformation.rssi, InventoryInformation.antennaPort, InventoryInformation.freqChannel, lock_InvItems[foundIndex].count);
                    }
                    else
                    {
                        //record no exist
                        //add a record to item list
                        InventoryInformation.index = lock_InvItems.Count;
                        lock_InvItems.Add(InventoryInformation);
                        string TagDataStr = InventoryInformation.epc.ToString();
                        string EPCONLY;
                        string TIDONLY = "";
                        string USERONLY = "";

                        if (TagDataStr.Length > InventoryInformation.pc.EPCLength * 4)
                        {
                            EPCONLY = TagDataStr.Substring(0, (int)InventoryInformation.pc.EPCLength * 4);

                            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxReadTID.Checked)
                                TIDONLY = TagDataStr.Substring(EPCONLY.ToString().Length, System.Convert.ToInt16(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBoxTIDcount.Text) * 4);

                            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxReadUser.Checked)
                                USERONLY = TagDataStr.Substring(EPCONLY.ToString().Length + TIDONLY.Length, System.Convert.ToInt16(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBoxUsercount.Text) * 4);
                        }
                        else
                            EPCONLY = TagDataStr;

                        //UI update in separate thread
                        LVAddItem(
                            InventoryInformation.index.ToString(),
                            InventoryInformation.pc.ToString(),
                            InventoryInformation.xpc_w1.ToString(),
                            InventoryInformation.xpc_w2.ToString(),
                            EPCONLY,
                            TIDONLY,
                            USERONLY,
                            InventoryInformation.rssi.ToString(),
                            InventoryInformation.count.ToString(),
                            InventoryInformation.antennaPort.ToString(),
                            InventoryInformation.freqChannel.ToString(),
                            InventoryInformation.crc16.ToString("X4"));
                        if (SaveSQL)
                        {
                            sqlMethod.AddData(InventoryInformation.epc.ToString());
                        }

                        WriteTagLog(InventoryInformation.receiveTime, EPCONLY, TIDONLY, USERONLY, InventoryInformation.rssi, InventoryInformation.antennaPort, InventoryInformation.freqChannel, 1);
                    }
                }
            }
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
       

        #endregion

        #region Delegate

        private delegate void LVAddItemDeleg(string index, string pc, string epc, string rssi, string count, string antennaPort);
        private void LVAddItem(string index, string pc, string epc, string rssi, string count, string antennaPort)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LVAddItemDeleg(LVAddItem), new object[] { index, pc, epc, rssi, count, antennaPort });
                return;
            }
            lock (MyLock)
            {
                ListViewItem item = new ListViewItem(index);
                item.Font = new Font("Courier New", 12, FontStyle.Regular);
                item.SubItems.Add(pc);
                item.SubItems.Add(epc);
                item.SubItems.Add(rssi);
                item.SubItems.Add(count);
                item.SubItems.Add(antennaPort);
                item.Tag = Environment.TickCount;
                item.ForeColor = Color.Green;
                item.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold);
                //item.ForeColor = Color.White;
                m_sortListView.Items.Add(item);
            }
        }

        private void LVAddItem(string index, string pc, string xpc_w1, string xpc_w2, string epc, string tid, string user, string rssi, string count, string antennaPort, string freqChannel, string crc16)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LVAddItemDeleg(LVAddItem), new object[] { index, pc, epc, rssi, count, antennaPort });
                return;
            }
            lock (MyLock)
            {
                ListViewItem item = new ListViewItem(index);
                item.Font = new Font("Courier New", 12, FontStyle.Regular);
                //item.Font = new Font("Arial", 8.5f, FontStyle.Bold);
                item.SubItems.Add(pc);
                item.SubItems.Add(xpc_w1);
                item.SubItems.Add(xpc_w2);
                item.SubItems.Add(epc);
                item.SubItems.Add(tid);
                item.SubItems.Add(user);
                item.SubItems.Add(rssi);
                item.SubItems.Add(count);
                item.SubItems.Add(antennaPort);
                item.SubItems.Add(freqChannel);
                item.SubItems.Add(crc16);
                item.Tag = Environment.TickCount;
                item.ForeColor = Color.Green;
                item.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold);
                //item.Font = new Font("Arial", 8.5f, FontStyle.Bold);
                //
                //item.ForeColor = Color.White;
                m_sortListView.Items.Add(item);
            }
        }

        private delegate void UpdateListViewDeleg(TagCallbackInfo item);
        private void UpdateListView(TagCallbackInfo item)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateListViewDeleg(UpdateListView), new object[] { item });
                return;
            }
            lock (MyLock)
            {
                for (int index = 0; index < m_sortListView.Items.Count; index++)
                {
                    try
                    {
                        if (item.pc != null)
                        {
                            if (m_sortListView.Items[index].SubItems[4].Text == item.epc.ToString().Substring(0, (int)item.pc.EPCLength * 4) && (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBox_GroupTags.Checked ? true : uint.Parse(m_sortListView.Items[index].SubItems[9].Text) == item.antennaPort))
                            {
                                m_sortListView.Items[index].SubItems[7].Text = item.rssi.ToString();
                                m_sortListView.Items[index].SubItems[8].Text = item.count.ToString();
                                m_sortListView.Items[index].SubItems[9].Text = item.antennaPort.ToString();
                                m_sortListView.Items[index].SubItems[10].Text = item.freqChannel.ToString();
                                m_sortListView.Items[index].Tag = Environment.TickCount;
                                m_sortListView.Items[index].ForeColor = Color.Green;
                                break;
                            }
                        }
                        else
                        {
                            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBox_GroupTags.Checked ? true : uint.Parse(m_sortListView.Items[index].SubItems[9].Text) == item.antennaPort)
                            {
                                m_sortListView.Items[index].SubItems[8].Text = item.count.ToString();
                                m_sortListView.Items[index].SubItems[9].Text = item.antennaPort.ToString();
                                m_sortListView.Items[index].Tag = Environment.TickCount;
                                m_sortListView.Items[index].ForeColor = Color.Green;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private delegate void EnableTimerDeleg(bool en);
        private void EnableTimer(bool en)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableTimerDeleg(EnableTimer), new object[] { en });
                return;
            }
            tmrRowColor.Enabled = tmr_updatelist.Enabled = en;
        }

        #endregion

        #region ListView Handle
        private void InitListView()
        {
            this.m_sortListView = new Custom.Control.SortListView();
            // 
            // m_sortListView
            // 
            this.m_sortListView.FullRowSelect = true;
            this.m_sortListView.GridLines = true;
            this.m_sortListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.m_sortListView.Location = new System.Drawing.Point(3, 3);
            this.m_sortListView.Name = "m_sortListView";
            this.m_sortListView.Size = new System.Drawing.Size(380, 400);
            this.m_sortListView.TabIndex = 0;
            this.m_sortListView.UseCompatibleStateImageBehavior = false;
            this.m_sortListView.View = System.Windows.Forms.View.Details;
            this.m_sortListView.Font = new Font(FontFamily.GenericSerif, 6);
            this.m_sortListView.SelectedIndexChanged += new System.EventHandler(this.m_sortListView_SelectedIndexChanged);
            m_colhdrIndex = new Custom.Control.SortColumnHeader();
            m_colhdrPc = new Custom.Control.SortColumnHeader();
            m_colhdrXPc_W1 = new Custom.Control.SortColumnHeader();
            m_colhdrXPc_W2 = new Custom.Control.SortColumnHeader();
            m_colhdrEpc = new Custom.Control.SortColumnHeader();
            m_colhdrTid = new Custom.Control.SortColumnHeader();
            m_colhdrUser = new Custom.Control.SortColumnHeader();
            m_colhdrRssi = new Custom.Control.SortColumnHeader();
            m_colhdrCount = new Custom.Control.SortColumnHeader();
            m_colhdrAntennaPort = new Custom.Control.SortColumnHeader();
            m_colhdrFreqChannel = new Custom.Control.SortColumnHeader();
            m_colhdrCrc16 = new Custom.Control.SortColumnHeader();
            m_sortListView.Columns.AddRange(new ColumnHeader[] { 
                m_colhdrIndex,
                m_colhdrPc,
                m_colhdrXPc_W1,
                m_colhdrXPc_W2,
                m_colhdrEpc,
                m_colhdrTid,
                m_colhdrUser,
                m_colhdrRssi, 
                m_colhdrCount, 
                m_colhdrAntennaPort,
                m_colhdrFreqChannel,
                m_colhdrCrc16
            });
            m_colhdrIndex.Text = "Index";
            m_colhdrIndex.Width = 80;
            m_colhdrPc.Text = "PC";
            m_colhdrPc.Width = 80;
            m_colhdrXPc_W1.Text = "XPC_W1";
            m_colhdrXPc_W1.Width = 80;
            m_colhdrXPc_W2.Text = "XPC_W2";
            m_colhdrXPc_W2.Width = 80;
            m_colhdrEpc.Text = "EPC";
            m_colhdrEpc.Width = 260;
            m_colhdrTid.Text = "TID";
            m_colhdrTid.Width = 80;
            m_colhdrUser.Text = "USER";
            m_colhdrUser.Width = 80;
            m_colhdrRssi.Text = "RSSI";
            m_colhdrRssi.Width = 80;
            m_colhdrCount.Text = "Count";
            m_colhdrCount.Width = 100;
            m_colhdrAntennaPort.Text = "Antenna Port";
            m_colhdrAntennaPort.Width = 100;
            m_colhdrFreqChannel.Text = "Frequency Channel";
            m_colhdrFreqChannel.Width = 100;
            m_colhdrCrc16.Text = "CRC16";
            m_colhdrCrc16.Width = 80;
            m_sortListView.SortColumn = 0;
            //Controls.Add(m_sortListView);
            m_sortListView.Dock = DockStyle.Fill;
            toolStripContainer1.ContentPanel.Controls.Add(m_sortListView);
            // Assign specific comparers to each column header.
            m_colhdrIndex.ColumnHeaderSorter = new Custom.Control.ComparerStringAsInt();
            m_colhdrPc.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrXPc_W1.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrXPc_W2.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrEpc.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrTid.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrUser.ColumnHeaderSorter = new Custom.Control.ComparerString();
            m_colhdrRssi.ColumnHeaderSorter = new Custom.Control.ComparerStringAsDouble();
            m_colhdrCount.ColumnHeaderSorter = new Custom.Control.ComparerStringAsInt();
            m_colhdrAntennaPort.ColumnHeaderSorter = new Custom.Control.ComparerStringAsInt();
            m_colhdrFreqChannel.ColumnHeaderSorter = new Custom.Control.ComparerStringAsInt();
            m_colhdrCrc16.ColumnHeaderSorter = new Custom.Control.ComparerStringAsInt();
            this.m_sortListView.Sorting = Custom.Control.SortOrder.Ascending;
        }

        private void m_sortListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!selectMode)
                return;
            if (m_sortListView.SelectedIndices != null && m_sortListView.SelectedIndices.Count > 0 && m_sortListView.SelectedIndices[0] >= 0)
            {
                EnableButton(ButtonState.Select, true);
            }
            else
            {
                EnableButton(ButtonState.Select, false);
                if (Program.ReaderXP.State == RFState.BUSY)
                {
                    EnableButton(ButtonState.Start, false);
                    EnableButton(ButtonState.Stop, true);
                }
                else if (Program.ReaderXP.State == RFState.IDLE)
                {
                    EnableButton(ButtonState.Start, true);
                    EnableButton(ButtonState.Stop, false);
                }
            }
        }
        #endregion

        #region Button Handle
        
        UInt32 elapsedTime = 0; 
        public void Start()
        {
            elapsedTime = 0;

            TagLogFile_SetName();

            if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
            {
                //TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "\\CycleLog.Txt", true);
                TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                tw.WriteLine("{0} {1} : Start Inventory ", Program.ReaderXP.DeviceNameOrIP ,Program.ReaderXP.MacAddress);
                tw.WriteLine("Addional information : {0}:{1} {2} {3} {4} {5}",
                    Program.appSetting.Singulation,
                    (Program.appSetting.Singulation == SingulationAlgorithm.FIXEDQ ?
                    ((FixedQParms)Program.appSetting.SingulationAlg).qValue :
                    ((DynamicQParms)Program.appSetting.SingulationAlg).startQValue),
                    Program.appSetting.Power,
                    (Program.appSetting.FixedChannel ? "Fixed:" +  Program.appSetting.Channel_number : "Hopping"),
                    Program.appSetting.tagGroup.session,
                    Program.appSetting.tagGroup.target
                    );

                tw.Close();
            }

            if (CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.checkBoxTagLog.Checked)
            {
                FileInfo f = new FileInfo(TagLogFileName);

                if (f.Exists)
                {
                    long s1 = f.Length;

                    if (f.Length > CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.TagLogFileSizeLimit)
                    {
                        TagLogFile_SetName();
                    }
                }

                {
                    TextWriter tw = new StreamWriter(TagLogFileName, true);
                    tw.WriteLine("{0} {1} : Start Inventory ", Program.ReaderXP.DeviceNameOrIP, Program.ReaderXP.MacAddress);
                    tw.Close();
                }
            }

            if (Program.ReaderXP.State == RFState.IDLE)
            {
                Program.ReaderXP.SetOperationMode(RadioOperationMode.CONTINUOUS);
                Program.ReaderXP.SetSingulationAlgorithmParms(Program.appSetting.Singulation, Program.appSetting.SingulationAlg);
                //Do Setup on SettingForm

                Program.ReaderXP.Options.TagRanging.multibanks = 0;
                if (ControlPanelForm.ControlPanel.checkBoxReadTID.Checked == true)
                {
                    Program.ReaderXP.Options.TagRanging.multibanks++;
                    Program.ReaderXP.Options.TagRanging.bank1 = MemoryBank.TID;
                    Program.ReaderXP.Options.TagRanging.offset1 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxTIDoffset.Text);
                    Program.ReaderXP.Options.TagRanging.count1 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxTIDcount.Text);
                }
                if (ControlPanelForm.ControlPanel.checkBoxReadUser.Checked == true)
                {
                    Program.ReaderXP.Options.TagRanging.multibanks++;
                    if (Program.ReaderXP.Options.TagRanging.multibanks == 1)
                    {
                        Program.ReaderXP.Options.TagRanging.bank1 = MemoryBank.USER;
                        Program.ReaderXP.Options.TagRanging.offset1 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxUseroffset.Text);
                        Program.ReaderXP.Options.TagRanging.count1 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxUsercount.Text);
                    }
                    else
                    {
                        Program.ReaderXP.Options.TagRanging.bank2 = MemoryBank.USER;
                        Program.ReaderXP.Options.TagRanging.offset2 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxUseroffset.Text);
                        Program.ReaderXP.Options.TagRanging.count2 = Convert.ToUInt16(ControlPanelForm.ControlPanel.textBoxUsercount.Text);
                    }
                }

                Program.ReaderXP.Options.TagRanging.QTMode = false; // reset to default
                Program.ReaderXP.Options.TagRanging.accessPassword = 0x0; // reset to default

                Program.ReaderXP.SetTagGroup(Program.appSetting.tagGroup);
                if (Program.appSetting.tagGroup.selected == Selected.ALL)
                {
                    Program.ReaderXP.Options.TagRanging.flags = SelectFlags.ZERO;
                }
                else
                {
                    Program.ReaderXP.Options.TagRanging.flags = SelectFlags.SELECT;

                    Program.ReaderXP.Options.TagGeneralSelected.flags = SelectMaskFlags.ENABLE_TOGGLE;
                    switch (Program.appSetting.MaskBank)
                    {
                        case 0:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = MemoryBank.EPC;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMask = new S_MASK(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskLength = Program.appSetting.MaskBitLength;
                            break;

                        case 1:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = MemoryBank.EPC;
                            Program.ReaderXP.Options.TagGeneralSelected.flags |= SelectMaskFlags.ENABLE_PC_MASK;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMask = new S_MASK(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskLength = Program.appSetting.MaskBitLength;
                            break;

                        case 2:
                        case 3:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = (MemoryBank)Program.appSetting.MaskBank;
                            Program.ReaderXP.Options.TagGeneralSelected.Mask = CSLibrary.Text.HexEncoding.ToBytes(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.MaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.MaskLength = Program.appSetting.MaskBitLength;
                            break;
                    }
                    Program.ReaderXP.StartOperation(Operation.TAG_GENERALSELECTED, true);
                }

                timer_ElapsedTime.Enabled = true;
                Program.ReaderXP.StartOperation(Operation.TAG_RANGING, false);
            }
        }
        public void StartOnce()
        {
            if (Program.ReaderXP.State == RFState.IDLE)
            {
                elapsedTime = 0;
                timer_ElapsedTime.Enabled = true;

                Program.ReaderXP.SetOperationMode(RadioOperationMode.NONCONTINUOUS);
                Program.ReaderXP.SetTagGroup(Program.appSetting.tagGroup);
                Program.ReaderXP.SetSingulationAlgorithmParms(Program.appSetting.Singulation, Program.appSetting.SingulationAlg);
                //Do Setup on SettingForm
                Program.ReaderXP.Options.TagRanging.flags = SelectFlags.ZERO;
                Program.ReaderXP.Options.TagRanging.QTMode = false; // reset to default
                Program.ReaderXP.Options.TagRanging.accessPassword = 0x0; // reset to default
                Program.ReaderXP.StartOperation(Operation.TAG_RANGING, false);
            }
        }

        public void Stop()
        {
            if (Program.ReaderXP.State == RFState.BUSY)
                Program.ReaderXP.StopOperation(true);

            timer_ElapsedTime.Enabled = false;

            while (Program.ReaderXP.State != RFState.IDLE)
                Thread.Sleep(100);

            // Get current profile for debugging read tag = 0
            {
                uint profile = 99;

                Program.ReaderXP.GetCurrentLinkProfile(ref profile);
                if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                {
                    //TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + "\\CycleLog.Txt", true);
                    TextWriter tw = new StreamWriter(CS203_CALLBACK_API_DEMO.ControlPanelForm.ControlPanel.textBox_LogPath.Text + CycleLogFileName, true);

                    tw.WriteLine("Stop Inventory : Current profile {0}", profile);

                    tw.Close();
                }
            }
        }

        public void Clear()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(Clear));
                return;
            }
            InventoryListItems.Clear();
            m_sortListView.Items.Clear();
        }

        public void ClearCount()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(ClearCount));
                return;
            }
            for (int cnt = 0; cnt < lock_InvItems.Count; cnt++)
                lock_InvItems[cnt].count = 0;
            for (int cnt = 0; cnt < m_sortListView.Items.Count; cnt++)
                m_sortListView.Items[cnt].SubItems[8].Text = "0";
            
        }

        public void SelectTag()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(SelectTag));
                return;
            }
            if (selectMode && m_sortListView.SelectedIndices.Count > 0)
            {
                EPC = m_sortListView.Items[m_sortListView.SelectedIndices[0]].SubItems[2].Text;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(CloseForm));
                return;
            }
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void Save()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(Save));
                return;
            }
            if (InventoryListItems.Count == 0)
                return;
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (TagCallbackInfo data in InventoryListItems)
                    {
                        sw.WriteLine(string.Format("{0:X4}-{1},{2:F1}", data.pc, data.epc, data.rssi, data.antennaPort));
                    }
                }
            }
        }

        public void Save2SQL()
        {
            if (!sqlMethod.Prepare())
            {
                MessageBox.Show("Connect to Server error! Please check connection.");
            }
            else
            {
                SaveSQL = true;
            }
        }

        private delegate void EnableButtonDeleg(ButtonState state, bool en);
        private void EnableButton(ButtonState state, bool en)
        {
            RaiseEvent<OnButtonClickEventArgs>(OnButtonEnable, this, new OnButtonClickEventArgs(state, en));
        }


        #endregion

        #region Timer
        private void tmr_updatelist_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
            tsl_uid.Text = "Tag read = " + lock_InvItems.Count;
            tsl_rate.Text = string.Format("Rate = {0:F1} Tag/s", totaltags / watchrate.Elapsed.TotalSeconds);
            tsl_crc.Text = string.Format("CRC = {0:F1} Tag/s", totalcrc / watchrate.Elapsed.TotalSeconds);
            tsl_ElapsedTime.Text = elapsedTime.ToString() + "s";
            if (watchrate.Elapsed.TotalSeconds > 10)
            {
                watchrate.Reset();
                watchrate.Start();
                Interlocked.Exchange(ref totaltags, 0);
                Interlocked.Exchange(ref totalcrc, 0);
            }
            });
        }
        #endregion

        #region Protected
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void RaiseEvent<T>(EventHandler<T> eventHandler, object sender, T e)
            where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            ControlPanelForm.CloseControlPanel();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ControlPanelForm.SetTopMost(true);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            ControlPanelForm.SetTopMost(false);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            ControlPanelForm.SetResize(new Point(this.Location.X+ this.Width, this.Location.Y), this.Height);
        }

        public override Size MaximumSize
        {
            get
            {
                if (ControlPanelForm._controlPanel != null)
                {
                    Rectangle rc = Screen.GetWorkingArea(this.DesktopBounds);
                    return new Size(rc.Width - ControlPanelForm._controlPanel.Width, rc.Height);
                }
                else
                    return base.MaximumSize;
            }
            set
            {
                base.MaximumSize = value;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            ControlPanelForm.SetResize(new Point(this.Location.X + this.Width, this.Location.Y), this.Height);
        }
        #endregion

        private void tmrRowColor_Tick(object sender, EventArgs e)
        {
#if !DETECT_TAG_BY_TIME
            for (int i = 0; i < m_sortListView.Items.Count; i++)
            {
                // Change tag color (OrangeRed < AntCycleTime[0] > OrangeRed < AntCycleTime[1] > YellowGreen < AntCycleTime[2] > LimeGreen < AntCycleTime[3] > Green < AntCycleTime[4] > Green)

                if (AntCycleTimeCount <= 1)
                {
                    m_sortListView.Items[i].ForeColor = Color.Green;
                }
                else
                {
                    int j;

                    for (j = AntCycleTimeCount - 2; j >= 0; j--)
                    {
                        if ((int)m_sortListView.Items[i].Tag > AntCycleTime[j])
                        {
                            switch (AntCycleTimeCount - j)
                            {
                                case 2:
                                    m_sortListView.Items[i].ForeColor = Color.Green;
                                    break;

                                case 3:
                                    m_sortListView.Items[i].ForeColor = Color.LimeGreen;
                                    break;

                                case 4:
                                    m_sortListView.Items[i].ForeColor = Color.YellowGreen;
                                    break;

                                default:
                                    m_sortListView.Items[i].ForeColor = Color.OrangeRed;
                                    break;
                            }
                            break;
                        }
                    }

                    if (j < 0)
                        m_sortListView.Items[i].ForeColor = Color.OrangeRed;

                }
            }
#else

            int curTime = Environment.TickCount;
            for (int i = 0; i < m_sortListView.Items.Count; i++)
            {
                int timeDiff = curTime - (int)m_sortListView.Items[i].Tag;
                if (timeDiff > 5000)
                {
                    m_sortListView.Items[i].ForeColor = Color.OrangeRed;
                }
                else if (timeDiff <= 5000 && timeDiff > 4000)
                {
                    m_sortListView.Items[i].ForeColor = Color.OrangeRed;
                }
                else if (timeDiff <= 4000 && timeDiff > 3000)
                {
                    m_sortListView.Items[i].ForeColor = Color.YellowGreen;
                }
                else if (timeDiff <= 3000 && timeDiff > 2000)
                {
                    m_sortListView.Items[i].ForeColor = Color.LimeGreen;
                }
                else if (timeDiff <= 1000 && timeDiff >= 0)
                {
                    m_sortListView.Items[i].ForeColor = Color.Green;
                }
            }
            //Application.DoEvents();
#endif
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void timer_ElapsedTime_Tick(object sender, EventArgs e)
        {
            elapsedTime++;
        }

        private void tsl_ElapsedTime_Click(object sender, EventArgs e)
        {

        }
    }

    #region Log records

#if temp
    public class InventoryDelayLog
    {
        string _fullFileName;



        public void SetLog(string fullFileName, uint maxFiles, uint maxSize)
        {
            _fullFileName = fullFileName;
        }

        public void DeleteOldFile()
        {
        }

        public void WriteToFile()
        {
        }
    }

    public class InventorySummaryLog
    {
        string _fullFileName;

        uint[] _tagsOfAntenna = new uint[16];
        uint _totalTagsPerCycle;


        public InventorySummaryLog(HighLevelInterface readerHandler)
        {
            ClearData();
        }

        ~InventorySummaryLog()
        {
        }

        public void ClearData ()
        {
            _totalTagPerCycle = 0;
            for (int cnt = 0; cnt < _tagsOfAntenna.Length; cnt++)
                _tagsOfAntenna[cnt] = 0;
        }

        public void SetLogFile (string fullFileName)
        {
            _FullFileName = fullFileName;
        }

        public void AddRecord()
        {
            _totalTagsPerCycle++;
        }

        public void WriteReset()
        {
        }

        public void WriteCycleSummary()
        {
            TextWriter tw = new StreamWriter(_fullFileName, true);

            tw.Write("{0} : Cycle {1}, {2} Tag/Cycle, Channel State : ", DateTime.Now.ToString(), AntCycleCount, totaltagscycle);

            for (int cnt = 0; cnt < 16; cnt++)
            {
                if (Program.ReaderXP.ChannelStatus[cnt] == RFState.CH_BUSY)
                {
                    portbusy = 1;
                    if (common == 0)
                        tw.Write("Port Busy ");
                    else
                        tw.Write(", ");
                    tw.Write("{0}", cnt);
                    common = 1;
                }
            }

            if (portbusy == 0)
                tw.WriteLine("CLEAR");
            else
                tw.WriteLine("");

            tw.Close();

            numChannelBusy = 0;
        }

    }
#endif

    #endregion

}
