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
    public partial class FormReflectedPowerTest16Ports : Form
    {
        uint _tagNumber = 0;
        uint _lastFreqChannel = 0;

        DateTime StartTime = DateTime.Now;
        //uint uTotalTags = 0;
        uint uTagsPreSecond = 0;
        uint uNewTags = 0;
        string minRSSIEPC = "";
        float minRSSI = 10000;

        HashSet<string> TagList = new HashSet<string>();

        public FormReflectedPowerTest16Ports()
        {
            InitializeComponent();

            if (Program.ReaderXP.OEMDeviceType == CSLibrary.Constants.Machine.CS203X)
            {
                button0.Enabled = false;
                button1.Enabled = false;
                button6.Enabled = false;
            }

            InventorySetting();
            AttachCallback(true);
        }

        void InventorySetting()
        {
            if (Program.ReaderXP.State == CSLibrary.Constants.RFState.IDLE)
            {
                Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
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

                //Program.ReaderXP.Options.TagRanging.retry = uint.Parse(ControlPanelForm.ControlPanel.textBox_RetryCount.Text);
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
            _tagNumber++;
            _lastFreqChannel = e.info.freqChannel;
        }

        private void button0_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP0.Text = "";
            textBox_TN0.Text = "";
            textBox_LE0.Text = "";
            textBox_LTF0.Text = "";
            button0.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(0, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP0.Text = (rp / 10f).ToString("F1");
            textBox_TN0.Text = _tagNumber.ToString();
            textBox_LE0.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF0.Text = _lastFreqChannel.ToString();
            button0.Text = "Test";
        }

        void DisableAllAntennaPort()
        {
            for (uint cnt = 0; cnt < 16; cnt++)
                Program.ReaderXP.AntennaPortSetState(cnt, CSLibrary.Constants.AntennaPortState.DISABLED);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP1.Text = "";
            textBox_TN1.Text = "";
            textBox_LE1.Text = "";
            textBox_LTF1.Text = "";
            button1.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(1, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP1.Text = (rp / 10f).ToString("F1");
            textBox_TN1.Text = _tagNumber.ToString();
            textBox_LE1.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF1.Text = _lastFreqChannel.ToString();
            button1.Text = "Test";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP2.Text = "";
            textBox_TN2.Text = "";
            textBox_LE2.Text = "";
            textBox_LTF2.Text = "";
            button2.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(2, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP2.Text = (rp / 10f).ToString("F1");
            textBox_TN2.Text = _tagNumber.ToString();
            textBox_LE2.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF2.Text = _lastFreqChannel.ToString();
            button2.Text = "Test";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP3.Text = "";
            textBox_TN3.Text = "";
            textBox_LE3.Text = "";
            textBox_LTF3.Text = "";
            button3.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(3, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP3.Text = (rp / 10f).ToString("F1");
            textBox_TN3.Text = _tagNumber.ToString();
            textBox_LE3.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF3.Text = _lastFreqChannel.ToString();
            button3.Text = "Test";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP4.Text = "";
            textBox_TN4.Text = "";
            textBox_LE4.Text = "";
            textBox_LTF4.Text = "";
            button4.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(4, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP4.Text = (rp / 10f).ToString("F1");
            textBox_TN4.Text = _tagNumber.ToString();
            textBox_LE4.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF4.Text = _lastFreqChannel.ToString();
            button4.Text = "Test";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP5.Text = "";
            textBox_TN5.Text = "";
            textBox_LE5.Text = "";
            textBox_LTF5.Text = "";
            button5.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(5, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP5.Text = (rp / 10f).ToString("F1");
            textBox_TN5.Text = _tagNumber.ToString();
            textBox_LE5.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF5.Text = _lastFreqChannel.ToString();
            button5.Text = "Test";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP6.Text = "";
            textBox_TN6.Text = "";
            textBox_LE6.Text = "";
            textBox_LTF6.Text = "";
            button6.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(6, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP6.Text = (rp / 10f).ToString("F1");
            textBox_TN6.Text = _tagNumber.ToString();
            textBox_LE6.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF6.Text = _lastFreqChannel.ToString();
            button6.Text = "Test";

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP7.Text = "";
            textBox_TN7.Text = "";
            textBox_LE7.Text = "";
            textBox_LTF7.Text = "";
            button7.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(7, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP7.Text = (rp / 10f).ToString("F1");
            textBox_TN7.Text = _tagNumber.ToString();
            textBox_LE7.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF7.Text = _lastFreqChannel.ToString();
            button7.Text = "Test";

        }

        private void button8_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP8.Text = "";
            textBox_TN8.Text = "";
            textBox_LE8.Text = "";
            textBox_LTF8.Text = "";
            button8.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(8, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP8.Text = (rp / 10f).ToString("F1");
            textBox_TN8.Text = _tagNumber.ToString();
            textBox_LE8.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF8.Text = _lastFreqChannel.ToString();
            button8.Text = "Test";

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP9.Text = "";
            textBox_TN9.Text = "";
            textBox_LE9.Text = "";
            textBox_LTF9.Text = "";
            button9.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(9, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP9.Text = (rp / 10f).ToString("F1");
            textBox_TN9.Text = _tagNumber.ToString();
            textBox_LE9.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF9.Text = _lastFreqChannel.ToString();
            button9.Text = "Test";

        }

        private void button10_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP10.Text = "";
            textBox_TN10.Text = "";
            textBox_LE10.Text = "";
            textBox_LTF10.Text = "";
            button10.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(10, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP10.Text = (rp / 10f).ToString("F1");
            textBox_TN10.Text = _tagNumber.ToString();
            textBox_LE10.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF10.Text = _lastFreqChannel.ToString();
            button10.Text = "Test";

        }

        private void button11_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP11.Text = "";
            textBox_TN11.Text = "";
            textBox_LE11.Text = "";
            textBox_LTF11.Text = "";
            button11.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(11, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP11.Text = (rp / 10f).ToString("F1");
            textBox_TN11.Text = _tagNumber.ToString();
            textBox_LE11.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF11.Text = _lastFreqChannel.ToString();
            button11.Text = "Test";

        }

        private void button12_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP12.Text = "";
            textBox_TN12.Text = "";
            textBox_LE12.Text = "";
            textBox_LTF12.Text = "";
            button12.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(12, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP12.Text = (rp / 10f).ToString("F1");
            textBox_TN12.Text = _tagNumber.ToString();
            textBox_LE12.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF12.Text = _lastFreqChannel.ToString();
            button12.Text = "Test";

        }

        private void button13_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP13.Text = "";
            textBox_TN13.Text = "";
            textBox_LE13.Text = "";
            textBox_LTF13.Text = "";
            button13.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(13, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP13.Text = (rp / 10f).ToString("F1");
            textBox_TN13.Text = _tagNumber.ToString();
            textBox_LE13.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF13.Text = _lastFreqChannel.ToString();
            button13.Text = "Test";

        }

        private void button14_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP14.Text = "";
            textBox_TN14.Text = "";
            textBox_LE14.Text = "";
            textBox_LTF14.Text = "";
            button14.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(14, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP14.Text = (rp / 10f).ToString("F1");
            textBox_TN14.Text = _tagNumber.ToString();
            textBox_LE14.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF14.Text = _lastFreqChannel.ToString();
            button14.Text = "Test";

        }

        private void button15_Click(object sender, EventArgs e)
        {
            int rp = 0;
            _tagNumber = 0;
            _lastFreqChannel = 99;

            textBox_RP15.Text = "";
            textBox_TN15.Text = "";
            textBox_LE15.Text = "";
            textBox_LTF15.Text = "";
            button15.Text = "Testing...";
            Application.DoEvents();

            DisableAllAntennaPort();
            Program.ReaderXP.AntennaPortSetState(15, CSLibrary.Constants.AntennaPortState.ENABLED);
            Program.ReaderXP.SetOperationMode(CSLibrary.Constants.RadioOperationMode.NONCONTINUOUS);
            Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING, true);
            Program.ReaderXP.GetReversedPowerLevel(ref rp);
            textBox_RP15.Text = (rp / 10f).ToString("F1");
            textBox_TN15.Text = _tagNumber.ToString();
            textBox_LE15.Text = Program.ReaderXP.LastMacErrorCode.ToString("X4");
            textBox_LTF15.Text = _lastFreqChannel.ToString();
            button15.Text = "Test";

        }
    }
}
