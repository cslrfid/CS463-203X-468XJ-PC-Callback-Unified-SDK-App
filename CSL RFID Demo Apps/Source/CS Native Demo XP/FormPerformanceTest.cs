using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class FormPerformanceTest : Form
    {
        DateTime StartTime = DateTime.Now;
        //uint uTotalTags = 0;
        uint uTagsPreSecond = 0;
        uint uNewTags = 0;
        string minRSSIEPC = "";
        float minRSSI = 10000;

        HashSet<string> TagList = new HashSet<string>();

        public FormPerformanceTest()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "Start")
            {
                StartTime = DateTime.Now;
                //uTotalTags = 0;
                uTagsPreSecond = 0;
                uNewTags = 0;
                minRSSIEPC = "";
                minRSSI = 10000;
                TagList.Clear();

                buttonStart.Text = "Stop";
                ShowCounter();
                InventorySetting();
                AttachCallback(true);
                
                timerMonitor.Start();
                Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, false);
            }
            else
            {
                buttonStart.Text = "Start";
                timerMonitor.Stop();

                AttachCallback(false);
                Program.ReaderXP.StopOperation(true);
            }
        }


        void InventorySetting()
        {
            if (Program.ReaderXP.State == CSLibrary.Constants.RFState.IDLE)
            {
                Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.CONTINUOUS);
                Program.ReaderXP.SetSingulationAlgorithmParms(Program.appSetting.Singulation, Program.appSetting.SingulationAlg);
                //Do Setup on SettingForm

                Program.ReaderXP.Options.TagRanging.multibanks = 0;

                Program.ReaderXP.Options.TagRanging.QTMode = false; // reset to default
                Program.ReaderXP.Options.TagRanging.focus = Program.appSetting.focus;
                Program.ReaderXP.Options.TagRanging.accessPassword = 0x0; // reset to default

                Program.ReaderXP.SetTagGroup(Program.appSetting.tagGroup);
                if (Program.appSetting.tagGroup.selected == CSLibrary.Constants.Selected.ALL)
                {
                    Program.ReaderXP.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.ZERO;
                }
                else
                {
                    Program.ReaderXP.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.SELECT;

                    Program.ReaderXP.Options.TagGeneralSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
                    switch (Program.appSetting.MaskBank)
                    {
                        case 0:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMask = new CSLibrary.Structures.S_MASK(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskLength = Program.appSetting.MaskBitLength;
                            break;

                        case 1:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
                            Program.ReaderXP.Options.TagGeneralSelected.flags |= CSLibrary.Constants.SelectMaskFlags.ENABLE_PC_MASK;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMask = new CSLibrary.Structures.S_MASK(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.epcMaskLength = Program.appSetting.MaskBitLength;
                            break;

                        case 2:
                        case 3:
                            Program.ReaderXP.Options.TagGeneralSelected.bank = (CSLibrary.Constants.MemoryBank)Program.appSetting.MaskBank;
                            Program.ReaderXP.Options.TagGeneralSelected.Mask = CSLibrary.Text.HexEncoding.ToBytes(Program.appSetting.Mask);
                            Program.ReaderXP.Options.TagGeneralSelected.MaskOffset = Program.appSetting.MaskOffset;
                            Program.ReaderXP.Options.TagGeneralSelected.MaskLength = Program.appSetting.MaskBitLength;
                            break;
                    }
                    Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_GENERALSELECTED, true);
                }

                // Select Criteria filter
                if (Program._PREFILTER_Enable)
                {
                    Program.ReaderXP.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;

/*                    Program.ReaderXP.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
                    Program.ReaderXP.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(Program._PREFILTER_MASK_EPC);
                    Program.ReaderXP.Options.TagSelected.epcMaskOffset = Program._PREFILTER_Offset;
                    Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)(Program._PREFILTER_MASK_EPC.Length) * 4;
                    Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_PREFILTER, true);
*/

                    if (Program._PREFILTER_Bank == 1)
                    {
                        Program.ReaderXP.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
                        Program.ReaderXP.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(Program._PREFILTER_MASK_DATA);
                        Program.ReaderXP.Options.TagSelected.epcMaskOffset = Program._PREFILTER_Offset;
                        Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)(Program._PREFILTER_MASK_DATA.Length) * 4;
                    }
                    else
                    {
                        Program.ReaderXP.Options.TagSelected.bank = (CSLibrary.Constants.MemoryBank)(Program._PREFILTER_Bank);
                        Program.ReaderXP.Options.TagSelected.Mask = CSLibrary.Text.HexEncoding.ToBytes(Program._PREFILTER_MASK_DATA);
                        Program.ReaderXP.Options.TagSelected.MaskOffset = Program._PREFILTER_Offset;
                        Program.ReaderXP.Options.TagSelected.MaskLength = (uint)(Program._PREFILTER_MASK_DATA.Length) * 4;
                    }

                    Program.ReaderXP.Options.TagRanging.flags |= CSLibrary.Constants.SelectFlags.SELECT;
                }

                // Post Match Criteria filter
                if (Program._POSTFILTER_MASK_Enable)
                {
                    Program.ReaderXP.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(Program._POSTFILTER_MASK_EPC);

                    CSLibrary.Structures.SingulationCriterion[] sel = new CSLibrary.Structures.SingulationCriterion[1];
                    sel[0] = new CSLibrary.Structures.SingulationCriterion();
                    sel[0].match = Program._POSTFILTER_MASK_MatchNot ? 0U : 1U;
                    sel[0].mask = new CSLibrary.Structures.SingulationMask(Program._POSTFILTER_MASK_Offset, (uint)(Program._POSTFILTER_MASK_EPC.Length * 4), Program.ReaderXP.Options.TagSelected.epcMask.ToBytes());
                    Program.ReaderXP.SetPostMatchCriteria(sel);
                    Program.ReaderXP.Options.TagRanging.flags |= CSLibrary.Constants.SelectFlags.POSTMATCH;
                }

                if (checkBoxCountwithTID.Checked)
                {
                    Program.ReaderXP.Options.TagRanging.multibanks = 1;
                    Program.ReaderXP.Options.TagRanging.bank1 = CSLibrary.Constants.MemoryBank.TID;
                    Program.ReaderXP.Options.TagRanging.offset1 = 0;
                    Program.ReaderXP.Options.TagRanging.count1 = 4;
                }

                //Program.ReaderXP.Options.TagRanging.retry = uint.Parse(ControlPanelForm.ControlPanel.textBox_RetryCount.Text);
            }
        }


        void ShowCounter()
        {
            this.BeginInvoke((System.Threading.ThreadStart)delegate()
                {
                    var running = (DateTime.Now - StartTime);
                    textBoxRunningTime.Text = running.Hours.ToString() + ":" + running.Minutes.ToString() + ":" + running.Seconds.ToString();
                    textBoxTotalTags.Text = TagList.Count.ToString();
                    textBoxTagsPerSecond.Text = uTagsPreSecond.ToString();
                    textBoxNewTags.Text = uNewTags.ToString();
                    textBoxminRSSIEPC.Text = minRSSIEPC;
                    if (minRSSI == 10000)
                    {
                        textBoxminRSSIdBmv.Text = "";
                        textBoxminRSSIdBm.Text = "";
                    }
                    else
                    {
                        textBoxminRSSIdBmv.Text = minRSSI.ToString("0.##");
                        textBoxminRSSIdBm.Text = (minRSSI - 106.989).ToString("0.##");
                    }
                    uTagsPreSecond = 0;
                    uNewTags = 0;
                });
        }

        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            ShowCounter();
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

        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            switch (e.state)
            {
                case CSLibrary.Constants.RFState.RESET:
                    MessageBox.Show("NetWork Disconnect");
                    break;
            }
        }

        void ReaderXP_TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            string EPC = e.info.epc.ToString();
            //uTotalTags ++;
            uTagsPreSecond ++;
            if (TagList.Add(EPC))
                uNewTags ++;
            if (e.info.rssi < minRSSI)
            {
                minRSSI = e.info.rssi;
                minRSSIEPC = EPC;
            }
        }

#if oldode
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

                   while (Program.ReaderXP.Reconnect(1) != Result.OK)
                       if (ControlPanelForm.ControlPanel.checkBoxLog.Checked)
                       {
                           StatisticsReport.WriteLine("Reconnect Fail!!!");
                           StatisticsReport.Flush();
                       }
                   
                   EnableButton(ButtonState.ALL, true);
               }

               panelNetworkDisconnet.Visible = false;
            });
#endif
 
    
    }
}
