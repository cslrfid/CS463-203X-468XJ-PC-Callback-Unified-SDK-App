using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CSLibrary.Text;
using CSLibrary.Constants;
using CSLibrary.Structures;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class TagRFMicroForm : Form
    {
        enum Opts
        {
            UNKNOWN = -1,
            TAG_SEARCH,
            TAG_READ,
            TAG_EXIT
        }

        #region Private Member
        private Opts m_opt = Opts.UNKNOWN;
        private uint m_retry_cnt = 50;
        private bool m_cancel_read_all_bank = false;
        private bool m_read_done = false;
        private bool m_cancel_write_all_bank = false;
        private bool m_stop = false;
        private object m_synclock = new object();
        private TagCallbackInfo m_record = new TagCallbackInfo();
        private TagDataModel m_tagTable = new TagDataModel(SlowFlags.EPC | SlowFlags.RSSI);

        private UInt64 _calibrationCode;
        
        #endregion

        #region Form
        public TagRFMicroForm()
        {
            InitializeComponent();
        }


        private void NewReadWriteForm_Load(object sender, EventArgs e)
        {
            //Setting Table
            nTable1.SetColumnWidth(0, 250);
            nTable1.BindModel(m_tagTable);

            AttachCallback(true);
        }

        private void NewReadWriteForm_Closing(object sender, CancelEventArgs e)
        {
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

        #region Event Callback
        private void AttachCallback(bool en)
        {
            if (en)
            {
                Program.ReaderXP.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
                Program.ReaderXP.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagSearchAllEvent);
                Program.ReaderXP.OnAccessCompleted += new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(ReaderXP_TagCompletedEvent);
            }
            else
            {
                Program.ReaderXP.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
                Program.ReaderXP.OnAsyncCallback -= new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(ReaderXP_TagSearchAllEvent);
                Program.ReaderXP.OnAccessCompleted -= new EventHandler<CSLibrary.Events.OnAccessCompletedEventArgs>(ReaderXP_TagCompletedEvent);
            }
        }

        void ReaderXP_TagCompletedEvent(object sender, CSLibrary.Events.OnAccessCompletedEventArgs e)
        {
        }

        void ReaderXP_TagSearchAllEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            this.Invoke((System.Threading.ThreadStart)delegate()
            {
                if (e.type == CallbackType.TAG_RANGING)
                {
                    string epcStr = e.info.epc.ToString();
                    UInt16 []epcUint16 = e.info.epc.ToUshorts();
                    UInt16 ocrssi = epcUint16[epcUint16.Length - 1];
                    //UInt16 ocrssi = 0;

                    // Change RSSI to OC RSSI
                    e.info.epc = new S_EPC(epcStr.Substring(0, epcStr.Length - 4));
                    //e.info.epc = new S_EPC(epcStr.Substring(0, epcStr.Length));
                    e.info.rssi = ocrssi;
                    AddItem(e.info);
                }
            });
        }

        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            if (this.IsDisposed)
                return;
            this.Invoke((System.Threading.ThreadStart)delegate()
            {
                switch (e.state)
                {
                    case RFState.IDLE:
                        EnableForm(true);
                        if (!m_stop)
                        {
                            switch (m_opt)
                            {
                                case Opts.TAG_SEARCH:
                                    ChangeBtnState(false);
                                    break;
                            }
                        }
                        else
                        {
                            CloseForm();
                        }
                        break;
                    case RFState.BUSY:
                        switch (m_opt)
                        {
                            case Opts.TAG_SEARCH:
                                ChangeBtnState(true);
                                break;
                        }
                        break;
                    case RFState.ABORT:
                        EnableForm(false);
                        break;
                }
            });
        }
        #endregion

        #region UI Update
        private void ChangeBtnState(bool searching)
        {
            if (searching)
            {
                btn_search.Text = "Stop";
                btn_search.BackColor = Color.Red;
            }
            else
            {
                btn_search.Text = "Search";
                btn_search.BackColor = Color.FromArgb(0, 192, 0);
            }
        }

        #endregion

        #region Other Handle

        private void Start()
        {
            if (Program.ReaderXP.State == RFState.IDLE)
            {
                Clear();
                m_opt = Opts.TAG_SEARCH;

                Program.ReaderXP.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.ZERO;

                Program.ReaderXP.SetOperationMode(Program.appSetting.Cfg_continuous_mode ? RadioOperationMode.CONTINUOUS : RadioOperationMode.NONCONTINUOUS);
                //Program.ReaderXP.SetTagGroup(Program.appSetting.tagGroup);
                Program.ReaderXP.SetTagGroup(CSLibrary.Constants.Selected.ASSERTED, CSLibrary.Constants.Session.S1, CSLibrary.Constants.SessionTarget.A);

                {
                    uint retry = 0;
                    switch (Program.appSetting.Singulation)
                    {
                        case SingulationAlgorithm.FIXEDQ:
                            {
                                FixedQParms p = (FixedQParms)Program.appSetting.SingulationAlg;
                                retry = p.retryCount;
                                p.retryCount = 5;
                                Program.ReaderXP.SetSingulationAlgorithmParms(Program.appSetting.Singulation, Program.appSetting.SingulationAlg);
                                p.retryCount = retry;
                            }
                            break;

                        case SingulationAlgorithm.DYNAMICQ:
                            {
                                DynamicQParms p = (DynamicQParms)Program.appSetting.SingulationAlg;
                                retry = p.retryCount;
                                p.retryCount = 5;
                                Program.ReaderXP.SetSingulationAlgorithmParms(Program.appSetting.Singulation, Program.appSetting.SingulationAlg);
                                p.retryCount = retry;
                            }
                            break;
                    }
                }

                // Select RFMicro filter
                {
                    CSLibrary.Structures.SelectCriterion extraSlecetion = new CSLibrary.Structures.SelectCriterion();

                    // for ok config
                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.TID, 0, 28, new byte[] { 0xe2, 0x82, 0x40, 0x30 });
                    Program.ReaderXP.SetSelectCriteria(1, extraSlecetion);

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.NOTHING_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.BANK3, 0xd0, 8, new byte[] { 0x20 });
                    Program.ReaderXP.SetSelectCriteria(0, extraSlecetion);

                    Program.ReaderXP.Options.TagRanging.flags |= CSLibrary.Constants.SelectFlags.SELECT;
                }

                // Multi bank inventory
                Program.ReaderXP.Options.TagRanging.multibanks = 1;
                Program.ReaderXP.Options.TagRanging.bank1 = CSLibrary.Constants.MemoryBank.BANK0;
                Program.ReaderXP.Options.TagRanging.offset1 = 13;
                Program.ReaderXP.Options.TagRanging.count1 = 1;
                //Program.ReaderXP.Options.TagRanging.bank2 = CSLibrary.Constants.MemoryBank.USER;
                //Program.ReaderXP.Options.TagRanging.offset2 = 8;
                //Program.ReaderXP.Options.TagRanging.count2 = 4;

                Program.ReaderXP.StartOperation(Operation.TAG_RANGING, false);
            }
        }
        private void Stop()
        {
            if (Program.ReaderXP.State == RFState.BUSY)
            {
                Program.ReaderXP.StopOperation(true);
            }
        }

        private void Clear()
        {
            m_tagTable.Clear();
            tb_entertag.Text = "";
        }

        private void tb_entertag_TextChanged(object sender, EventArgs e)
        {
            m_record.epc = new S_EPC(tb_entertag.Text);
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (Program.ReaderXP.State == RFState.IDLE)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        private void lb_clear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void nTable1_RowChanged(int rowIndex)
        {
            m_read_done = false;
            m_record.epc = m_tagTable.Items[rowIndex].epc;
            m_record.pc = m_tagTable.Items[rowIndex].pc;
            tb_entertag.Text = m_record.epc.ToString();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                Program.ReaderXP.StopOperation(true);
            }

            m_opt = (Opts)tc_readAndWrite.SelectedIndex;

            switch ((Opts)tc_readAndWrite.SelectedIndex)
            {
                case Opts.TAG_SEARCH:
                    ChangeBtnState(false);
                    break;
                case Opts.TAG_READ:

                    if (nTable1.CurrentRowIndex == -1)
                    {
                        //MessageBox.Show("Please choose a tag first or input a tag ID");
                        if (MessageBox.Show("Return to select tag page press'Yes', Continue to read any tag press 'No'", "No Tag ID", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            tc_readAndWrite.SelectedIndex = 0;
                        else
                        {
                            textBox_selectedEPC.Text = tb_entertag.Text;
                        }
                    }
                    else
                    {
                        textBox_selectedEPC.Text = m_record.epc.ToString();
                    }
                    break;
                case Opts.TAG_EXIT:
                    Program.ReaderXP.CancelAllSelectCriteria();
                    this.Close();
                    break;
            }
        }
        #endregion

        #region Read
        private void btn_readallbank_Click(object sender, EventArgs e)
        {
            if (Program.ReaderXP.State != RFState.IDLE)
            {
                MessageBox.Show("Reader is busy now, please try later.");
                return;
            }

            lb_ReadInfo.Text = "Reading...";
            btn_readallbank.Text = "Stop";
            btn_readallbank.BackColor = Color.Red;

            Application.DoEvents();

            Program.ReaderXP.Options.TagSelected.epcMask = new S_MASK(/*m_record.pc.ToString() + */m_record.epc.ToString());

            Program.ReaderXP.Options.TagSelected.flags = SelectMaskFlags.ENABLE_TOGGLE;
            //Comment:If enable PC lock, please make sure you are not using Higgs3 Tag. Otherwise, read will fail
            Program.ReaderXP.Options.TagSelected.epcMaskOffset = 0;
            Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)Program.ReaderXP.Options.TagSelected.epcMask.Length * 8;

            if (checkBox_tagID.Checked)
            {
                Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true);
                Program.ReaderXP.SetSelectCriteria(1, null);

                Program.ReaderXP.Options.TagRead.bank = CSLibrary.Constants.MemoryBank.BANK2;
                Program.ReaderXP.Options.TagRead.offset = 0;
                Program.ReaderXP.Options.TagRead.count = 2;

                if (Program.ReaderXP.StartOperation(Operation.TAG_READ, true) == Result.OK)
                {
                    textBox_tagID.Text = Program.ReaderXP.Options.TagRead.pData.ToString();
                }
                else
                {
                    textBox_tagID.Text = "READ ERROR";
                }
            }

            if (checkBox_calData.Checked)
            {
                Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true);
                Program.ReaderXP.SetSelectCriteria(1, null);

                Program.ReaderXP.Options.TagRead.bank = CSLibrary.Constants.MemoryBank.BANK3;
                Program.ReaderXP.Options.TagRead.offset = 8;
                Program.ReaderXP.Options.TagRead.count = 4;

                if (Program.ReaderXP.StartOperation(Operation.TAG_READ, true) == Result.OK)
                {
                    textBox_calData.Text = Program.ReaderXP.Options.TagRead.pData.ToString();

                    {
                        UInt16 [] cal = Program.ReaderXP.Options.TagRead.pData.ToUshorts();
                        _calibrationCode = (UInt64)(((UInt64)cal[0] << 48) | ((UInt64)cal[1] << 32) | ((UInt64)cal[2] << 16) | ((UInt64)cal[3]));
                    }
                }
                else
                {
                    textBox_calData.Text = "READ ERROR";
                    _calibrationCode = 0;
                }
            }

            if (checkBox_sensorCode.Checked)
            {
                {
                    Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true);
                    Program.ReaderXP.SetTagGroup(CSLibrary.Constants.Selected.ASSERTED, CSLibrary.Constants.Session.S1, CSLibrary.Constants.SessionTarget.A); // better for read rangage

                    CSLibrary.Structures.SelectCriterion extraSlecetion = new CSLibrary.Structures.SelectCriterion();

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.BANK3, 0xe0, 0, new byte[1]);
                    Program.ReaderXP.SetSelectCriteria(0, extraSlecetion);

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.EPC, 0x20, 0x60, Hex.ToBytes(textBox_selectedEPC.Text));
                    Program.ReaderXP.SetSelectCriteria(1, extraSlecetion);
                }

                Program.ReaderXP.Options.TagRead.bank = CSLibrary.Constants.MemoryBank.BANK0;
                Program.ReaderXP.Options.TagRead.offset = 12;
                Program.ReaderXP.Options.TagRead.count = 1;

                if (Program.ReaderXP.StartOperation(Operation.TAG_READ, true) == Result.OK)
                {
                    textBox_sensorCode.Text = Program.ReaderXP.Options.TagRead.pData.ToString();
                }
                else
                {
                    textBox_sensorCode.Text = "READ ERROR";
                }
            }

            if (checkBox_OCRSSI.Checked)
            {
                {
                    Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true);
                    Program.ReaderXP.SetTagGroup(CSLibrary.Constants.Selected.ASSERTED, CSLibrary.Constants.Session.S1, CSLibrary.Constants.SessionTarget.A); // better for read rangage

                    CSLibrary.Structures.SelectCriterion extraSlecetion = new CSLibrary.Structures.SelectCriterion();

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.BANK3, 0xd0, 8, new byte[] { 0x20 });
                    Program.ReaderXP.SetSelectCriteria(0, extraSlecetion);

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.EPC, 0x20, 0x60, Hex.ToBytes(textBox_selectedEPC.Text));

                    Program.ReaderXP.SetSelectCriteria(1, extraSlecetion);
                }

                Program.ReaderXP.Options.TagRead.bank = CSLibrary.Constants.MemoryBank.BANK0;
                Program.ReaderXP.Options.TagRead.offset = 13;
                Program.ReaderXP.Options.TagRead.count = 1;

                if (Program.ReaderXP.StartOperation(Operation.TAG_READ, true) == Result.OK)
                {
                    textBox_OCRSSI.Text = Program.ReaderXP.Options.TagRead.pData.ToString();
                    label_ocrssi.Text = "OC RSSI : " + Program.ReaderXP.Options.TagRead.pData.ToUshorts()[0].ToString();
                }
                else
                {
                    textBox_OCRSSI.Text = "READ ERROR";
                    label_ocrssi.Text = "OC RSSI : ";
                }
            }

            if (checkBox_temp.Checked)
            {
                {
                    Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true);
                    Program.ReaderXP.SetTagGroup(CSLibrary.Constants.Selected.ASSERTED, CSLibrary.Constants.Session.S1, CSLibrary.Constants.SessionTarget.A);// better for read rangage

                    CSLibrary.Structures.SelectCriterion extraSlecetion = new CSLibrary.Structures.SelectCriterion();

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.BANK3, 0xe0, 0, new byte[] { 0x00 });
                    Program.ReaderXP.SetSelectCriteria(0, extraSlecetion);

                    extraSlecetion.action = new CSLibrary.Structures.SelectAction(CSLibrary.Constants.Target.SELECTED, CSLibrary.Constants.Action.ASLINVA_DSLINVB, 0);
                    extraSlecetion.mask = new CSLibrary.Structures.SelectMask(CSLibrary.Constants.MemoryBank.EPC, 0x20, 0x60, Hex.ToBytes(textBox_selectedEPC.Text));
                    Program.ReaderXP.SetSelectCriteria(1, extraSlecetion);
                }

                Program.ReaderXP.Options.TagRead.bank = CSLibrary.Constants.MemoryBank.BANK0;
                Program.ReaderXP.Options.TagRead.offset = 14;
                Program.ReaderXP.Options.TagRead.count = 1;

                if (Program.ReaderXP.StartOperation(Operation.TAG_READ, true) == Result.OK)
                {
                    textBox_temp.Text = Program.ReaderXP.Options.TagRead.pData.ToString();
                    label_Temp.Text = "Temp : " + Math.Round(getTemperatue(Program.ReaderXP.Options.TagRead.pData.ToUshorts()[0], _calibrationCode), 2).ToString();
                }
                else
                {
                    textBox_temp.Text = "READ ERROR";
                    label_Temp.Text = "Temp : ";
                }
            }

            lb_ReadInfo.Text = "Read done!!!";
            btn_readallbank.Text = "Read";
            btn_readallbank.BackColor = Color.FromArgb(0, 192, 0);
            Application.DoEvents();
            m_read_done = true;

        }
        
        double getTemperatue(UInt16 temp, UInt64 CalCode)
        {
            int crc = (int)(CalCode >> 48) & 0xffff;
            int calCode1 = (int)(CalCode >> 36) & 0x0fff;
            int calTemp1 = (int)(CalCode >> 25) & 0x07ff;
            int calCode2 = (int)(CalCode >> 13) & 0x0fff;
            int calTemp2 = (int)(CalCode >> 2) & 0x7FF;
            int calVer = (int)(CalCode & 0x03);

            double fTemperature = temp;
            fTemperature = ((double)calTemp2 - (double)calTemp1) * (fTemperature - (double)calCode1);
            fTemperature /= ((double)(calCode2) - (double)calCode1);
            fTemperature += (double)calTemp1;
            fTemperature -= 800;
            fTemperature /= 10;

            return fTemperature;
        }

        #endregion

        #region Delegate
        private delegate void AddItemDeleg(TagCallbackInfo info);
        private void AddItem(TagCallbackInfo info)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AddItemDeleg(AddItem), new object[] { info });
                return;
            }

            lock (m_tagTable)
            {
                for (int cnt = 0; cnt < m_tagTable.Items.Count; cnt++)
                {
                    if (m_tagTable.Items[cnt].epc.ToString() == info.epc.ToString())
                    {
                        //m_tagTable.Items[cnt].rssi = info.rssi;
                        m_tagTable.UpdateItem(info, cnt);
                        return;
                    }
                }

                info.index = m_tagTable.Items.Count;
                m_tagTable.AddItem(info);
            }
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

        private void lb_clear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        
        }

        private void lb_ReadInfo_Click(object sender, EventArgs e)
        {

        }

    }
}