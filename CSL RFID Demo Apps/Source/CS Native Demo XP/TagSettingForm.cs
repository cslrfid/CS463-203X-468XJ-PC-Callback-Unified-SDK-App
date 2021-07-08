using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

//Import RFID Library
using CSLibrary.Constants;
using CSLibrary.Structures;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class TagSettingForm : Form
    {
        //private Singulation_FixedQ singulation_fixedQ;
        //private Singulation_DynamicQ singulation_dynamicQ;
        //private Singulation_DynamicQ singulation_dynamicQAdjust;
        //private Singulation_DynamicQ singulation_dynamicQThreshold;
        private bool m_stop = false;
        private double[] CurrentFreqList = null;

        private AntennaSeqTabPage tp_antenna_seq = new AntennaSeqTabPage();

        public TagSettingForm()
        {
            InitializeComponent();

            tp_antenna_seq.Text = "Antenna Sequence";
            tp_antenna_seq.BackColor = Color.FromArgb(192, 255, 192);
            tabControl1.Controls.Add(tp_antenna_seq);
            tabControl1.ResumeLayout();

            comboBox_TestAntenna.Items.Clear();

            switch (Program.ReaderXP.OEMDeviceType)
            {
                case Machine.CS468X:
                    for (int cnt = 0; cnt < 16; cnt++)
                        comboBox_TestAntenna.Items.Add("Port " + cnt.ToString());
                    break;

                default:
                    for (int cnt = 0; cnt < 4; cnt++)
                        comboBox_TestAntenna.Items.Add("Port " + cnt.ToString());
                    break;
            }
            comboBox_TestAntenna.SelectedIndex = 0;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            // remove test page
            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tp_engineering);

            //RFID Initial start here
            {

                textBox_PreFilter_MaskData.Text = Program._PREFILTER_MASK_DATA;
                textBox_PreFilter_Offset.Text = Program._PREFILTER_Offset.ToString();
                textBox_PreFilter_Bank.Text = Program._PREFILTER_Bank.ToString();
                checkBox_PreFilter_Enable.Checked = Program._PREFILTER_Enable;
                textBox_PostFilter_MaskData.Text = Program._POSTFILTER_MASK_EPC;
                textBox_PostFilter_Offset.Text = Program._POSTFILTER_MASK_Offset.ToString();
                checkBox_PostFilter_NotMatch.Checked = Program._POSTFILTER_MASK_MatchNot;
                checkBox_PostFilter_Enable.Checked = Program._POSTFILTER_MASK_Enable;

                //Load DataSource

                UInt16 ms = 0;

                Program.ReaderXP.GetTxOnTime(ref ms);

                textBox_TxOnTime.Text = ms.ToString("D");

                this.textBox_ReversePowerThreshold.Text = Program.appSetting.m_cfg_ReversePowerThreshold.ToString("D");
                
                this.cb_linkprofile.DataSource = Program.ReaderXP.GetActiveLinkProfile();

                this.cb_country.DataSource = Program.ReaderXP.GetActiveRegionCode();

                //this.cb_algorithm.DataSource = EnumGetValues(typeof(SingulationAlgorithm));

                //this.cb_selected.DataSource = EnumGetValues(typeof(Selected));

                //this.cb_session.DataSource = EnumGetValues(typeof(Session));

                //this.cb_target.DataSource = EnumGetValues(typeof(SessionTarget));

                if (Program.ReaderXP.IsFixedChannelOnly)
                {
                    cb_fixed.Enabled = false;
                    cb_fixed.Checked = true;
                }
                else if (Program.ReaderXP.IsHoppingChannelOnly)
                {
                    cb_fixed.Enabled = false;
                    cb_fixed.Checked = false;
                }
                else
                {
                    cb_fixed.Enabled = true;
                    cb_fixed.Checked = Program.appSetting.FixedChannel;
                }

                //Load Setting
                if (Program.ReaderXP.OEMChipSetID == ChipSetID.R2000 && Program.ReaderXP.SelectedRegionCode == CSLibrary.Constants.RegionCode.JP && Program.ReaderXP.GetAvailableFrequencyTable().Length == 6)
                {
                    cb_lbt.Visible = true;
                    cb_lbtScan.Visible = true;
                    cb_lbt.Checked = Program.appSetting.Lbt;
                    cb_lbtScan.Checked = Program.appSetting.LbtScan;
                }
                else
                {
                    cb_lbt.Visible = false;
                    cb_lbtScan.Visible = false;
                    cb_lbt.Checked = false;
                    cb_lbtScan.Checked = false;
                }

                if (Program.ReaderXP.SelectedRegionCode == CSLibrary.Constants.RegionCode.ETSI || 
                    Program.ReaderXP.SelectedRegionCode == CSLibrary.Constants.RegionCode.JP
                    )
                {
                    checkBoxFreqAgile.Enabled = true;
                    checkBoxFreqAgile.Checked = Program.appSetting.FreqAgile;
                }
                else
                    checkBoxFreqAgile.Enabled = false;

                if (cb_fixed.Checked && !checkBoxFreqAgile.Checked)
                    cb_freqlist.Enabled = cb_fixed.Checked;
                else
                    cb_freqlist.Enabled = false;

                //highlight the current link profile
                cb_linkprofile.SelectedItem = Program.appSetting.Link_profile;

                //current select frequency profile index
                cb_country.SelectedItem = Program.ReaderXP.SelectedRegionCode;

                checkBox_focusmode.Checked = Program.appSetting.focus;

                //this.cb_algorithm.SelectedItem = Program.appSetting.Singulation;
                switch (Program.appSetting.Singulation)
                {
                    case SingulationAlgorithm.FIXEDQ:
                        this.cb_algorithm.SelectedIndex = 0;
                        break;

                    case SingulationAlgorithm.DYNAMICQ:
                        this.cb_algorithm.SelectedIndex = 1;
                        break;
                }

                //this.cb_selected.SelectedItem = Program.appSetting.tagGroup.selected;
                switch (Program.appSetting.tagGroup.selected)
                {
                    case Selected.ASSERTED:
                        this.cb_selected.SelectedIndex = 1;
                        break;

                    case Selected.DEASSERTED:
                        this.cb_selected.SelectedIndex = 2;
                        break;

                    default:
                        this.cb_selected.SelectedIndex = 0;
                        break;
                }

                //this.cb_session.SelectedItem = Program.appSetting.tagGroup.session;
                switch (Program.appSetting.tagGroup.session)
                {
                    case Session.S1:
                        this.cb_session.SelectedIndex = 1;
                        break;

                    case Session.S2:
                        this.cb_session.SelectedIndex = 2;
                        break;

                    case Session.S3:
                        this.cb_session.SelectedIndex = 3;
                        break;

                    default:
                        this.cb_session.SelectedIndex = 0;
                        break;
                }

                //this.cb_target.SelectedItem = Program.appSetting.tagGroup.target;

                if (Program.appSetting.Singulation== SingulationAlgorithm.DYNAMICQ)
                {
                    DynamicQParms dq = (DynamicQParms)Program.appSetting.SingulationAlg;
                    if (dq.toggleTarget != 0)
                        this.cb_target.SelectedIndex = 2;
                    else
                        this.cb_target.SelectedIndex = (int)Program.appSetting.tagGroup.target;

                    nb_startqvalue.Value = dq.startQValue;
                    nb_minqvalue.Value = dq.minQValue;
                    nb_maxqvalue.Value = dq.maxQValue;
                    nb_thresholdMultiplier.Value = dq.thresholdMultiplier;
                    numericUpDown_Retry.Value = dq.retryCount;
                    checkBox_Toggle.Checked = (dq.toggleTarget != 0);
                }
                else
                {
                    FixedQParms fq = (FixedQParms)Program.appSetting.SingulationAlg;
                    if (fq.toggleTarget != 0)
                        this.cb_target.SelectedIndex = 2;
                    else
                        this.cb_target.SelectedIndex = (int)Program.appSetting.tagGroup.target;
                    nb_qvalue.Value = fq.qValue;
                    nb_retry.Value = fq.retryCount;
                    cb_toggle.Checked = (fq.toggleTarget != 0);
                    cb_repeat.Checked = (fq.repeatUntilNoTags != 0);
                }

                this.cb_custInvtryCont.Checked = Program.appSetting.Cfg_continuous_mode;

                this.cb_custInvtryBlocking.Checked = Program.appSetting.Cfg_blocking_mode;

                if (Program.appSetting.FixedChannel)
                {
                    cb_freqlist.SelectedItem = CurrentFreqList[(int)Program.appSetting.Channel_number];
                }

                this.cb_debug_log.Checked = Program.appSetting.Debug_log;

                cbRssiFilterEnable.Checked = Program.appSetting.EnableRssiFilter;

                nbRssiFilter.Value = Program.appSetting.RssiFilterThreshold;
                //Update maximum power
                UpdateMaxPower();

                //Update Frequency list
                UpdateFreqList();

#if nouse
                //get power level
                nb_power.Value = Program.appSetting.Power;
                if (Program.ReaderXP.OEMDeviceType == Machine.CS203)
                {
                    nb_power.Enabled = true;
                    lk_antenna_port_cfg.Enabled = false;
                }
                else
                {
                    nb_power.Enabled = false;
                    lk_antenna_port_cfg.Enabled = true;
                }
#endif

                nb_reconnectTimeout.Value = Program.appSetting.ReconnectTimeout;



                comboBox_MaskBank.SelectedIndex = (int)Program.appSetting.MaskBank;
                textBox_MaskOffset.Text = Program.appSetting.MaskOffset.ToString();
                textBox_MaskLength.Text = Program.appSetting.MaskBitLength.ToString();
                textBox_Mask.Text = Program.appSetting.Mask;

                comboBox_rflna_high_comp_norm.SelectedIndex = Program.appSetting.rflna_high_comp_norm;
                comboBox_rflna_gain_norm.SelectedIndex = Program.appSetting.rflna_gain_norm;
                comboBox_iflna_gain_norm.SelectedIndex = Program.appSetting.iflna_gain_norm;
                comboBox_ifagc_gain_norm.SelectedIndex = Program.appSetting.ifagc_gain_norm;
            }

            tabControl1.SelectedIndex = 0;

            AttachCallback(true);

#if ENGINEERING_MODE
            btnRegister.Visible = true;
#else
            btnRegister.Visible = false;
#endif

        }

        private void TagSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
            {
                m_stop = e.Cancel = true;
                Program.ReaderXP.TurnCarrierWaveOff();
            }
            else
            {
                AttachCallback(false);
            }
        }

        private void AttachCallback(bool attach)
        {
            if (attach)
            {
                Program.ReaderXP.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_OnStateChanged);
            }
            else
            {
                Program.ReaderXP.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_OnStateChanged);
            }
        }

        void ReaderXP_OnStateChanged(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            this.Invoke((System.Threading.ThreadStart)delegate()
            {
                switch (e.state)
                {
                    case CSLibrary.Constants.RFState.IDLE:
                        if (!m_stop)
                        {
                            UpdateCWBtn(true);
                        }
                        else
                        {
                            this.Close();
                        }
                        break;
                    case CSLibrary.Constants.RFState.BUSY:
                        UpdateCWBtn(false);
                        break;
                }
            });
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UpdateFreqList()
        {
            //Retrieve all available frequency channels
            cb_freqlist.DataSource = CurrentFreqList = Program.ReaderXP.GetAvailableFrequencyTable(Program.ReaderXP.GetActiveRegionCode()[cb_country.SelectedIndex]);
        }

        private void UpdateMaxPower()
        {
            nb_power.Maximum = Program.ReaderXP.GetActiveMaxPowerLevel((RegionCode)cb_country.SelectedItem);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }

        private void cb_algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_algorithm.SelectedIndex)
            {
                case 0:
                    groupBox_DYNAMICQ.Visible = false;
                    groupBox_FIXEDQ.Visible = true;
                    if (cb_target.SelectedIndex == 2)
                        cb_toggle.Checked = true;
                    else
                        cb_toggle.Checked = false;
                    break;


                case 1:
                    groupBox_FIXEDQ.Visible = false;
                    groupBox_DYNAMICQ.Visible = true;
                    if (cb_target.SelectedIndex == 2)
                        checkBox_Toggle.Checked = true;
                    else
                        checkBox_Toggle.Checked = false;
                    break;

                default:
                    groupBox_DYNAMICQ.Visible = false;
                    groupBox_FIXEDQ.Visible = false;
                    break;
            }

#if nouse
            
            tabControl1.SuspendLayout();

            tabControl1.Controls.Remove(singulation_fixedQ);
            tabControl1.Controls.Remove(singulation_dynamicQ);
            tabControl1.Controls.Remove(singulation_dynamicQAdjust);
            tabControl1.Controls.Remove(singulation_dynamicQThreshold);

#if ENGINEERING_MODE
            tabControl1.Controls.Remove(this.tp_cwonoff);
#endif
            switch (cb_algorithm.SelectedIndex)
            {
                case 0: //Add fixed Q Form
                    //Program.appSetting.Singulation = SingulationAlgorithm.FIXEDQ;
                    singulation_fixedQ = new Singulation_FixedQ
                        (
                            Program.appSetting.SingulationAlg
                        );
                    singulation_fixedQ.Text = "FixedQ";
                    singulation_fixedQ.BackColor = Color.FromArgb(192, 255, 192);
                    tabControl1.Controls.Add(singulation_fixedQ);
                    break;
                case 1:
                    //Program.appSetting.Singulation = SingulationAlgorithm.DYNAMICQ;
                    singulation_dynamicQ = new Singulation_DynamicQ
                        (
                        Program.appSetting.SingulationAlg, SingulationAlgorithm.DYNAMICQ
                        );
                    singulation_dynamicQ.Text = "DynamicQ";
                    singulation_dynamicQ.BackColor = Color.FromArgb(192, 255, 192);
                    tabControl1.Controls.Add(singulation_dynamicQ);
                    break;
                /*case 2:
                    //Program.appSetting.Singulation = SingulationAlgorithm.DYNAMICQ_ADJUST;
                    singulation_dynamicQAdjust = new Singulation_DynamicQ
                        (
                        Program.appSetting.SingulationAlg, SingulationAlgorithm.DYNAMICQ_ADJUST
                        );
                    singulation_dynamicQAdjust.Text = "DynamicQAdj";
                    singulation_dynamicQAdjust.BackColor = Color.FromArgb(192, 255, 192);
                    tabControl1.Controls.Add(singulation_dynamicQAdjust);
                    break;
                case 3:
                    //Program.appSetting.Singulation = SingulationAlgorithm.DYNAMICQ_THRESH;
                    singulation_dynamicQThreshold = new Singulation_DynamicQ
                        (
                        Program.appSetting.SingulationAlg, SingulationAlgorithm.DYNAMICQ_THRESH
                        );
                    singulation_dynamicQThreshold.Text = "DynamicQThres";
                    singulation_dynamicQThreshold.BackColor = Color.FromArgb(192, 255, 192);
                    tabControl1.Controls.Add(singulation_dynamicQThreshold);
                    break;*/
            }

#if ENGINEERING_MODE
            tabControl1.Controls.Add(this.tp_cwonoff);
#endif
            tabControl1.ResumeLayout();

//            tabControl1.SelectedIndex = 3;
#endif
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            Text = "Applying configuration : Please wait...";


            // for Test setup
/*
            {
                for (int cnt = 0; cnt < Program.appSetting.AntennaList.Count; cnt++)
                {
                    if (Program.appSetting.AntennaList[cnt].State == AntennaPortState.ENABLED) 
                    if (Program.appSetting.AntennaList[cnt].PowerLevel > Program.appSetting.TestSetupMaxPower[cb_freqlist.SelectedIndex])
                        Program.ReaderXP.SetPowerLevel((uint)cnt, (uint)Program.appSetting.TestSetupMaxPower[cb_freqlist.SelectedIndex]);
                }
            }
*/

            SetLNAIndex();

            this.Enabled = false;
            //Start to apply all settings

            Program._PREFILTER_MASK_DATA = textBox_PreFilter_MaskData.Text;
            try
            {
                Program._PREFILTER_Offset = uint.Parse(textBox_PreFilter_Offset.Text);
            }
            catch (Exception ex) { }
            try
            {
                Program._PREFILTER_Bank = uint.Parse(textBox_PreFilter_Bank.Text);
            }
            catch (Exception ex) { }
            Program._PREFILTER_Enable = checkBox_PreFilter_Enable.Checked;
            Program._POSTFILTER_MASK_EPC = textBox_PostFilter_MaskData.Text;
            try
            {
                Program._POSTFILTER_MASK_Offset = uint.Parse(textBox_PostFilter_Offset.Text);
            }
            catch (Exception ex) { }
            Program._POSTFILTER_MASK_MatchNot = checkBox_PostFilter_NotMatch.Checked;
            Program._POSTFILTER_MASK_Enable = checkBox_PostFilter_Enable.Checked;

            try
            {
                Program.appSetting.m_cfg_ReversePowerThreshold = UInt16.Parse(this.textBox_ReversePowerThreshold.Text);
                Program.ReaderXP.SetReversePowerThreshold(Program.appSetting.m_cfg_ReversePowerThreshold);
            }
            catch (Exception ex) { }


            /*
            try
            {
                UInt16 ms = UInt16.Parse(textBox1.Text);

                Program.ReaderXP.SetTxOnTime(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not set Tx On Time");
            }
            */

            //Set PowerLevel
            Result res = Result.OK;

            if ((cb_custInvtryBlocking.Checked && cb_custInvtryCont.Checked))
            {
                if (MessageBox.Show("Please don't run in blocking and continuous mode in the same time, otherwise you can't abort the operation", "warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                }
                else
                {
                    goto ERROR;
                }
            }

#if nouse
            if (nb_power.Enabled == true)
            if ((res = Program.ReaderXP.SetPowerLevel((uint)nb_power.Value)) != Result.OK)
            {
                ts_status.Text = string.Format("SetPowerLevel fail with err {0}", res);
                goto ERROR;
            }
#endif

            //Set LinkProfile
            if ((res = Program.ReaderXP.SetCurrentLinkProfile(uint.Parse(cb_linkprofile.Text))) != Result.OK)
            {
                ts_status.Text = string.Format("SetCurrentLinkProfile fail with err {0}", res);
                goto ERROR;
            }

            //Set Region and Frequency
            if (cb_fixed.Checked)
            {
                if (!checkBoxFreqAgile.Checked)
                {
                    //if (freq.ListItems.SelectedIndices.Count == 0 || freq.ListItems.SelectedIndices[0] < 0 || freq.ListItems.SelectedIndices[0] >= freq.ListItems.Items.Count)
                    if (cb_freqlist.SelectedIndex < 0 || cb_freqlist.SelectedIndex >= cb_freqlist.Items.Count)
                    {
                        ts_status.Text = "Please select a channel first";
                        goto ERROR;
                    }
                    //if ((res = Program.ReaderXP.SetFixedChannel(Program.ReaderXP.GetActiveRegionCode()[cb_country.SelectedIndex], (uint)freq.ListItems.SelectedIndices[0], cb_lbt.Checked ? LBT.ON : LBT.OFF)) != CSLibrary.Constants.Result.OK)

                    LBT lbt_setting = LBT.OFF;
                    if (cb_lbt.Checked)
                    {
                        if (cb_lbtScan.Checked)
                            lbt_setting = LBT.SCAN;
                        else
                            lbt_setting = LBT.ON;
                    }
                    if ((res = Program.ReaderXP.SetFixedChannel(Program.ReaderXP.GetActiveRegionCode()[cb_country.SelectedIndex], (uint)cb_freqlist.SelectedIndex, lbt_setting)) != CSLibrary.Constants.Result.OK)
                    {
                        ts_status.Text = string.Format("SetFixedChannel fail with err {0}", res);
                        goto ERROR;
                    }
                }
                else
                {
                    res = Program.ReaderXP.SetAgileChannels(Program.ReaderXP.GetActiveRegionCode()[cb_country.SelectedIndex]);
                    if (res != CSLibrary.Constants.Result.OK)
                    {
                        ts_status.Text = string.Format("SetAgileChannel fail with err {0}", res);
                        goto ERROR;
                    }
                }
            }
            else
            {
                if ((res = Program.ReaderXP.SetHoppingChannels(Program.ReaderXP.GetActiveRegionCode()[cb_country.SelectedIndex])) != CSLibrary.Constants.Result.OK)
                {
                    ts_status.Text = string.Format("SetHoppingChannels fail with err {0}", res);
                    goto ERROR;
                }
            }

            {

                Program.appSetting.Cfg_blocking_mode = cb_custInvtryBlocking.Checked;

                //Program.appSetting.Singulation = (SingulationAlgorithm)cb_algorithm.
                switch (cb_algorithm.SelectedIndex)
                {
                    case 0:
                        Program.appSetting.Singulation = SingulationAlgorithm.FIXEDQ;
                        break;

                    default:
                        Program.appSetting.Singulation = SingulationAlgorithm.DYNAMICQ;
                        break;
                }

                Program.appSetting.Cfg_continuous_mode = cb_custInvtryCont.Checked;

                Program.appSetting.FixedChannel = cb_fixed.Checked;

#if nouse
                Program.appSetting.tagGroup = new TagGroup
                                            (
                                                (Selected)cb_selected.SelectedItem,
                                                (Session)cb_session.SelectedItem,
                                                (SessionTarget)cb_target.SelectedItem
                                            );
#endif
                Program.appSetting.focus = checkBox_focusmode.Checked;
                //Program.ReaderXP.TCP_Connect = checkBox_focusmode.Checked;

                Program.appSetting.tagGroup = new TagGroup
                                            (
                                                ((cb_selected.SelectedIndex == 1) ? Selected.ASSERTED : (cb_selected.SelectedIndex == 2) ? Selected.DEASSERTED : Selected.ALL),
                                                (Session)cb_session.SelectedIndex,
                                                (cb_target.SelectedIndex == 2) ? SessionTarget.A : (SessionTarget)cb_target.SelectedIndex
                                            );

                switch (Program.appSetting.Singulation)
                {
                    case SingulationAlgorithm.DYNAMICQ:
                        DynamicQParms dynQ = new DynamicQParms();
                        dynQ.startQValue = (uint)nb_startqvalue.Value;
                        dynQ.minQValue = (uint)nb_minqvalue.Value;
                        dynQ.maxQValue = (uint)nb_maxqvalue.Value;
                        dynQ.retryCount = (uint)numericUpDown_Retry.Value;
                        dynQ.toggleTarget = (checkBox_Toggle.Checked) ? 1U : 0U;
                        dynQ.thresholdMultiplier = (uint)nb_thresholdMultiplier.Value;
                        Program.appSetting.SingulationAlg = dynQ;
                        break;
                    /*case SingulationAlgorithm.DYNAMICQ_ADJUST:
                        Program.appSetting.SingulationAlg = singulation_dynamicQAdjust.DynamicQAdjust;
                        break;
                    case SingulationAlgorithm.DYNAMICQ_THRESH:
                        Program.appSetting.SingulationAlg = singulation_dynamicQThreshold.DynamicQThreshold;
                        break;*/
                    case SingulationAlgorithm.FIXEDQ:
                        FixedQParms m_fixedQ = new FixedQParms();
                        m_fixedQ.qValue = (uint)nb_qvalue.Value;
                        m_fixedQ.retryCount = (uint)nb_retry.Value;
                        m_fixedQ.toggleTarget = cb_toggle.Checked ? 1U : 0U;
                        m_fixedQ.repeatUntilNoTags = cb_repeat.Checked ? 1U : 0U;
                        Program.appSetting.SingulationAlg = m_fixedQ;
                        break;
                }




#if nouse
                switch (Program.appSetting.Singulation)
                {
                    case SingulationAlgorithm.DYNAMICQ:
                        Program.appSetting.SingulationAlg = singulation_dynamicQ.DynamicQ;
                        break;
                    /*case SingulationAlgorithm.DYNAMICQ_ADJUST:
                        Program.appSetting.SingulationAlg = singulation_dynamicQAdjust.DynamicQAdjust;
                        break;
                    case SingulationAlgorithm.DYNAMICQ_THRESH:
                        Program.appSetting.SingulationAlg = singulation_dynamicQThreshold.DynamicQThreshold;
                        break;*/
                    case SingulationAlgorithm.FIXEDQ:
                        Program.appSetting.SingulationAlg = singulation_fixedQ.FixedQ;
                        break;
                }

#endif
            }
            Program.appSetting.Lbt = cb_lbt.Checked;
            Program.appSetting.FreqAgile = checkBoxFreqAgile.Checked;
            //CSLibrary.Diagnostics.CoreDebug.Enable = Program.appSetting.Debug_log = this.cb_debug_log.Checked;
#if NET_BUILD
            Program.ReaderXP.ReconnectTimeout = Program.appSetting.ReconnectTimeout = (uint)this.nb_reconnectTimeout.Value;
#endif
            Program.appSetting.EnableRssiFilter = cbRssiFilterEnable.Checked;
            Program.appSetting.RssiFilterThreshold = (uint)nbRssiFilter.Value;

            Program.appSetting.MaskBank = (uint)comboBox_MaskBank.SelectedIndex;
            Program.appSetting.MaskOffset = (uint.Parse)(textBox_MaskOffset.Text);
            Program.appSetting.MaskBitLength = (uint.Parse)(textBox_MaskLength.Text);
            Program.appSetting.Mask = textBox_Mask.Text;

            //#region === Antenna Port setting ===
            //
            //Program.ReaderXP.AntennaList.Store(Program.ReaderXP);
            //
            //#endregion === Antenna Port setting ===


            #region === Antenna Sequence setting ===

            Program.appSetting.AntennaSequenceMode = Program.ReaderXP.AntennaSequenceMode;
            Program.appSetting.AntennaSequenceSize = Program.ReaderXP.AntennaSequenceSize;
            Program.appSetting.AntennaPortSequence = Program.ReaderXP.AntennaPortSequence;

            if (Program.ReaderXP.SetOperationMode(
                Program.appSetting.Cfg_continuous_mode ? RadioOperationMode.CONTINUOUS : RadioOperationMode.NONCONTINUOUS,
                Program.appSetting.AntennaSequenceMode,
                (int)Program.appSetting.AntennaSequenceSize
                ) != CSLibrary.Constants.Result.OK)
            {
                MessageBox.Show("SetOperationMode failed");
            }

            if ((Program.ReaderXP.AntennaSequenceMode & AntennaSequenceMode.SEQUENCE) != 0)
            {
                byte[] seq = new byte[Program.appSetting.AntennaPortSequence.Length];
                for (int i = 0; i < Program.appSetting.AntennaSequenceSize; i++)
                {
                    seq[i] = (byte)Program.appSetting.AntennaPortSequence[i];
                    //MessageBox.Show(Program.appSetting.AntennaPortSequence[i].ToString());
                }

                if (Program.ReaderXP.SetAntennaSequence(seq, (uint)Program.appSetting.AntennaSequenceSize, Program.appSetting.AntennaSequenceMode) != CSLibrary.Constants.Result.OK)
                {
                    MessageBox.Show("SetAntennaSequence failed");
                }
            }

            #endregion === Antenna Sequence setting ===








            //Save all settings to config file
            if (cb_save.Checked)
            {
                Program.SaveSettings();
            }

            ts_status.Text = string.Format("Apply Configuration Successful");

            this.Enabled = true;

            this.DialogResult = DialogResult.OK;

            Text = "Apply Configuration Successful";

            return;

        ERROR:
            //ts_status.Text = string.Format("Apply Configuration failed");

            this.Enabled = true;

            //this.DialogResult = DialogResult.Cancel;

            Text = "Apply Configuration failed";
        }

        private void cb_country_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFreqList();
            UpdateMaxPower();
        }

        private void cb_fixed_CheckedChanged(object sender, EventArgs e)
        {
            cb_freqlist.Enabled = cb_fixed.Checked;
        }
        /// <summary>
        /// Same as full framework Enum.GetValues
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private object[] EnumGetValues(Type enumType)
        {
            if (enumType.BaseType ==
                typeof(Enum))
            {
                //get the public static fields (members of the enum)  
                FieldInfo[] fi = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
                //create a new enum array  
                object[] values = new object[fi.Length];
                //populate with the values  
                for (int iEnum = 0; iEnum < fi.Length; iEnum++)
                {
                    values[iEnum] = fi[iEnum].GetValue(null);
                }
                //return the array  
                return values;
            }

            //the type supplied does not derive from enum  
            throw new ArgumentException("enumType parameter is not a System.Enum");

        }

        private void btn_cwon_Click(object sender, EventArgs e)
        {
#if !old
            //SetCarrierTestAntenna
            if (Program.ReaderXP.OEMChipSetID == ChipSetID.R2000)
            {
                Program.ReaderXP.EngModeEnable("CSL2006");
                Program.ReaderXP.EngWriteRegister(0x113, (uint)comboBox_TestAntenna.SelectedIndex);

                uint pllvalue = Program.ReaderXP.GetPllValue((uint)comboBox_TestFrequency.SelectedIndex);

                Program.ReaderXP.EngWriteRegister(0x010D, pllvalue);

                Program.ReaderXP.EngWriteRegister(0x010E, Convert.ToUInt32(textBox_TestPLLDACCTL.Text, 16));

                Program.ReaderXP.EngWriteRegister(0x0114, uint.Parse(textBox_TestPower.Text));
            }
            Program.ReaderXP.TurnCarrierWaveOn(cb_withData.Checked);
#else
            Program.ReaderXP.EngTest_TransmitRandomData(true);
#endif
        }

        private void btn_cwoff_Click(object sender, EventArgs e)
        {
#if !old
            Program.ReaderXP.TurnCarrierWaveOff();
#else
            Program.ReaderXP.EngTest_TransmitRandomData(false);
#endif
        }

        private void UpdateCWBtn(bool en)
        {
            cb_withData.Enabled = btn_cwon.Enabled = en;
            btn_cwoff.Enabled = !en;
        }

        private void lk_antenna_port_cfg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //using (AntennaPortCfgForm ant = new AntennaPortCfgForm())
            using (ConfigureAntenna ant = new ConfigureAntenna())
            {
                ant.ShowDialog();
            }
        }

        private void lb_detail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (LinkProfileInformation info = new LinkProfileInformation(uint.Parse(cb_linkprofile.Text)))
            {
                info.ShowDialog();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
#if ENGINEERING_MODE
            new ReadRegisterForm().ShowDialog();
#endif
        }

        private void lbOperationConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
#if NCS468
            using (ConfigureOperation op = new ConfigureOperation())
            {
                op.ShowDialog();
            }
#endif
        }

        private void checkBoxFreqAgile_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFreqAgile.Checked)
            {
                cb_fixed.Checked = true;
                cb_freqlist.Enabled = false;
            }
            else if (cb_fixed.Checked == true)
                cb_freqlist.Enabled = true;
            else
                cb_freqlist.Enabled = false;
        }

        private void cb_selected_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_selected.SelectedIndex)
            {
                case 0:
                    groupBox_Mask.Visible = false;
                    break;

                case 1:
                    groupBox_Mask.Text = "Asserted Mask";
                    groupBox_Mask.Visible = true;
                    break;

                case 2:
                    groupBox_Mask.Text = "Deasserted Mask";
                    groupBox_Mask.Visible = true;
                    break;
            }
        }

        private void cb_target_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_target.SelectedIndex == 2) // Toggle A/B
            {
                if (cb_algorithm.SelectedIndex == 0) // Fixed Q
                {
                    cb_toggle.Checked = true;
                }
                else
                {
                    checkBox_Toggle.Checked = true;
                }
            }
            else
            {
                if (cb_algorithm.SelectedIndex == 0) // Fixed Q
                {
                    cb_toggle.Checked = false;
                }
                else
                {
                    checkBox_Toggle.Checked = false;
                }
            }
        }

        private void button_Write_Click(object sender, EventArgs e)
        {
            try
            {
                Program.ReaderXP.EngModeEnable("CSL2006");
                Program.ReaderXP.EngWriteRegister(0x0306, uint.Parse(textBox_TxOnTime.Text));
                Program.ReaderXP.EngWriteRegister(0x0307, uint.Parse(textBox_TxOffTime.Text));
                Program.ReaderXP.EngWriteRegister(0x030E, uint.Parse(textBoxLBTListeningThreshold.Text));
                Program.ReaderXP.EngWriteRegister(0x0303, uint.Parse(textBoxLBTListeningTime.Text));

                Program.appSetting.TestSetupMaxPower[0] = uint.Parse(textBox_MaxPowerCh1.Text);
                Program.appSetting.TestSetupMaxPower[1] = uint.Parse(textBox_MaxPowerCh2.Text);
                Program.appSetting.TestSetupMaxPower[2] = uint.Parse(textBox_MaxPowerCh3.Text);
                Program.appSetting.TestSetupMaxPower[3] = uint.Parse(textBox_MaxPowerCh4.Text);
                Program.appSetting.TestSetupMaxPower[4] = uint.Parse(textBox_MaxPowerCh5.Text);
                Program.appSetting.TestSetupMaxPower[5] = uint.Parse(textBox_MaxPowerCh6.Text);
                
                ts_status.Text = "Write MAC Registers Sucess.";
            }
            catch (Exception ex)
            {
                ts_status.Text = "Write MAC Registers ERROR!!!";
            }
        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            btn_apply.Visible = true;

            if (tabControl1.SelectedTab.Text == "Test Setup")
            {
                uint value = 0;

                Program.ReaderXP.EngModeEnable("CSL2006");
                Program.ReaderXP.EngReadRegister(0x0306, ref value);
                textBox_TxOnTime.Text = value.ToString();
                Program.ReaderXP.EngReadRegister(0x0307, ref value);
                textBox_TxOffTime.Text = value.ToString();
                Program.ReaderXP.EngReadRegister(0x030E, ref value);
                textBoxLBTListeningThreshold.Text = value.ToString();
                Program.ReaderXP.EngReadRegister(0x0303, ref value);
                textBoxLBTListeningTime.Text = value.ToString();

                textBox_MaxPowerCh1.Text = Program.appSetting.TestSetupMaxPower[0].ToString();
                textBox_MaxPowerCh2.Text = Program.appSetting.TestSetupMaxPower[1].ToString();
                textBox_MaxPowerCh3.Text = Program.appSetting.TestSetupMaxPower[2].ToString();
                textBox_MaxPowerCh4.Text = Program.appSetting.TestSetupMaxPower[3].ToString();
                textBox_MaxPowerCh5.Text = Program.appSetting.TestSetupMaxPower[4].ToString();
                textBox_MaxPowerCh6.Text = Program.appSetting.TestSetupMaxPower[5].ToString();
            }
            else if (tabControl1.SelectedTab.Text == "Carrier Wave")
            {
                comboBox_TestFrequency.DataSource = cb_freqlist.DataSource;
                comboBox_TestFrequency.SelectedIndex = 0;
            }
        }


        void ReaderXP_StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            // dBv to dBm
            // return (float)Math.Round(this._RSSI - 106.98);

            {
                switch (e.state)
                {
                    case RFState.ENGTESTRESULT:
                        this.BeginInvoke((System.Threading.ThreadStart)delegate()
                           {
                               label_RSSI.Text = (Program.ReaderXP._Debug_EngineeringTest_RSSI - 106.98).ToString("0.00");
                               label_RSSI1.Text = Program.ReaderXP._Debug_EngineeringTest_RSSI1.ToString("X2");
                               label_RSSI2.Text = (Program.ReaderXP._Debug_EngineeringTest_RSSI2 - 106.98).ToString("0.00");
                               label_RSSI3.Text = Program.ReaderXP._Debug_EngineeringTest_RSSI3.ToString("X2");
                           });
                        break;
                }
            }
        }

        private void button_ReadRSSI_Click(object sender, EventArgs e)
        {

            if (button_ReadRSSI.Text.Length >= 5 && button_ReadRSSI.Text.Substring(0, 5) == "Start")
            {
                button_ReadRSSI.Text = "Stop";
                //Program.ReaderXP.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
                Program.ReaderXP.EngTest_RSSIStreamingTest();
            }
            else
            {
                //Program.ReaderXP.OnStateChanged -= new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(ReaderXP_StateChangedEvent);
                Program.ReaderXP.StopOperation(true);
                button_ReadRSSI.Text = "Start Read RSSI";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UInt16 value = 0;

            Program.ReaderXP.EngModeEnable("CSL2006");
            if (Program.ReaderXP.EngBypassReadRegister(0x450, ref value) == Result.OK)
            {
                textBox_rawValue.Text = value.ToString("x4");

                var ifagc_gain_norm = (value & 0x07);
                var iflna_gain_norm = ((value >> 3) & 0x07);
                var rflna_gain_norm = ((value >> 6) & 0x03);
                var rflna_high_comp_norm = ((value >> 8) & 0x01);

                comboBox_rflna_high_comp_norm.SelectedIndex = rflna_high_comp_norm;

                switch (rflna_gain_norm)
                {
                    case 0:
                        comboBox_rflna_gain_norm.SelectedIndex = 0;
                        break;
                    case 2:
                        comboBox_rflna_gain_norm.SelectedIndex = 1;
                        break;
                    case 3:
                        comboBox_rflna_gain_norm.SelectedIndex = 2;
                        break;
                    default:
                        textBox_rawValue.Text += " : Error rflna_gain_norm";
                        break;
                }

                switch (iflna_gain_norm)
                {
                    case 0:
                        comboBox_iflna_gain_norm.SelectedIndex = 0;
                        break;
                    case 1:
                        comboBox_iflna_gain_norm.SelectedIndex = 1;
                        break;
                    case 3:
                        comboBox_iflna_gain_norm.SelectedIndex = 2;
                        break;
                    case 7:
                        comboBox_iflna_gain_norm.SelectedIndex = 3;
                        break;
                    default:
                        textBox_rawValue.Text += " : Error iflna_gain_norm";
                        break;
                }

                switch (ifagc_gain_norm)
                {
                    case 0:
                        comboBox_ifagc_gain_norm.SelectedIndex = 0;
                        break;
                    case 4:
                        comboBox_ifagc_gain_norm.SelectedIndex = 1;
                        break;
                    case 6:
                        comboBox_ifagc_gain_norm.SelectedIndex = 2;
                        break;
                    case 7:
                        comboBox_ifagc_gain_norm.SelectedIndex = 3;
                        break;
                    default:
                        textBox_rawValue.Text += " : Error ifagc_gain_norm";
                        break;
                }
            }
            else
            {
                textBox_rawValue.Text = "Read error";
            }
        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rflna_high_comp_norm = comboBox_rflna_high_comp_norm.SelectedIndex;
            int rflna_gain_norm = 0;
            int iflna_gain_norm = 0;
            int ifagc_gain_norm = 0;

            switch (comboBox_rflna_gain_norm.SelectedIndex)
            {
                case 0:
                    rflna_gain_norm = 0;
                    break;
                case 1:
                    rflna_gain_norm = 2;
                    break;
                case 2:
                    rflna_gain_norm = 3;
                    break;
            }

            switch (comboBox_iflna_gain_norm.SelectedIndex)
            {
                case 0:
                    iflna_gain_norm = 0;
                    break;
                case 1:
                    iflna_gain_norm = 1;
                    break;
                case 2:
                    iflna_gain_norm = 3;
                    break;
                case 3:
                    iflna_gain_norm = 7;
                    break;
            }

            switch (comboBox_ifagc_gain_norm.SelectedIndex)
            {
                case 0:
                    ifagc_gain_norm = 0;
                    break;
                case 1:
                    ifagc_gain_norm = 4;
                    break;
                case 2:
                    ifagc_gain_norm = 6;
                    break;
                case 3:
                    ifagc_gain_norm = 7;
                    break;
            }

            int value = rflna_high_comp_norm << 8 | 
                rflna_gain_norm << 6 | 
                iflna_gain_norm << 3 |
                ifagc_gain_norm;

            Program.ReaderXP.EngModeEnable("CSL2006");
            Program.ReaderXP.EngBypassWriteRegister(0x450, (UInt16)(value));
        }

        void SetLNAIndex()
        {
            int rflna_high_comp_norm = comboBox_rflna_high_comp_norm.SelectedIndex;
            int rflna_gain_norm = 0;
            int iflna_gain_norm = 0;
            int ifagc_gain_norm = 0;

            Program.appSetting.rflna_high_comp_norm = comboBox_rflna_high_comp_norm.SelectedIndex;
            Program.appSetting.rflna_gain_norm = comboBox_rflna_gain_norm.SelectedIndex;
            Program.appSetting.iflna_gain_norm = comboBox_iflna_gain_norm.SelectedIndex;
            Program.appSetting.ifagc_gain_norm = comboBox_ifagc_gain_norm.SelectedIndex;

            switch (comboBox_rflna_gain_norm.SelectedIndex)
            {
                case 0:
                    rflna_gain_norm = 1;
                    break;
                case 1:
                    rflna_gain_norm = 7;
                    break;
                case 2:
                    rflna_gain_norm = 13;
                    break;
            }

            switch (comboBox_iflna_gain_norm.SelectedIndex)
            {
                case 0:
                    iflna_gain_norm = 24;
                    break;
                case 1:
                    iflna_gain_norm = 18;
                    break;
                case 2:
                    iflna_gain_norm = 12;
                    break;
                case 3:
                    iflna_gain_norm = 6;
                    break;
            }

            switch (comboBox_ifagc_gain_norm.SelectedIndex)
            {
                case 0:
                    ifagc_gain_norm = -12;
                    break;
                case 1:
                    ifagc_gain_norm = -6;
                    break;
                case 2:
                    ifagc_gain_norm = 0;
                    break;
                case 3:
                    ifagc_gain_norm = 6;
                    break;
            }

            Program.ReaderXP.SetLNA(rflna_high_comp_norm, rflna_gain_norm, iflna_gain_norm, ifagc_gain_norm);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox_focusmode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_focusmode.Checked)
            {
                cb_session.SelectedIndex = 1;
                cb_session.Enabled = false;
                cb_target.SelectedIndex = 0;
                cb_target.Enabled = false;
            }
            else
            {
                cb_session.Enabled = true;
                cb_target.Enabled = true;
            }
        }

        private void comboBox_rflna_gain_norm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_rflna_gain_norm.SelectedIndex == 2)
            {
                comboBox_rflna_high_comp_norm.SelectedIndex = 0;
                comboBox_rflna_high_comp_norm.Enabled = false;
            }
            else
            {
                comboBox_rflna_high_comp_norm.Enabled = true;
            }

        }
    }
}