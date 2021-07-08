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
    public partial class FormLargeReadWrite : Form
    {
        public FormLargeReadWrite()
        {
            InitializeComponent();
        }

        private void button_SelectTag_Click(object sender, EventArgs e)
        {
            //textBox_EPC.Text = "E2C06F920000003A007CE904";

            //Stop current operation
            if (Program.ReaderXP.State ==  CSLibrary.Constants.RFState.BUSY)
            {
                Program.ReaderXP.StopOperation(true);
            }

            while (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
            {
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }

            using (TagSearchForm inv = new TagSearchForm())
            {
                if (inv.ShowDialog() == DialogResult.OK)
                {
                    textBox_EPC.Text = inv.epc;
                }
            }
        }

        private void button_GeneralWrite_Click(object sender, EventArgs e)
        {
            int count = int.Parse(textBox_Length.Text);
            UInt16[] data = new UInt16[count];
            DateTime start, end;
            UInt16 dataPattern = Convert.ToUInt16(comboBox_DataPattern.Text, 16);

            for (int cnt = 0; cnt < count; cnt++)
                data[cnt] = dataPattern;

            Program.ReaderXP.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
            Program.ReaderXP.Options.TagSelected.epcMask = new  CSLibrary.Structures.S_MASK(textBox_EPC.Text);
            Program.ReaderXP.Options.TagSelected.epcMaskOffset = 0;
            Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)Program.ReaderXP.Options.TagSelected.epcMask.Length * 8;
            Program.ReaderXP.Options.TagSelected.flags =  CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true) != CSLibrary.Constants.Result.OK)
            {
                MessageBox.Show("Selected tag failed");
                return;
            }

            Program.ReaderXP.Options.TagWriteUser.retryCount = 31;
            Program.ReaderXP.Options.TagWriteUser.accessPassword = 0x00000000;
            Program.ReaderXP.Options.TagWriteUser.offset = UInt16.Parse(textBox_Offset.Text);
            Program.ReaderXP.Options.TagWriteUser.count = UInt16.Parse(textBox_Length.Text);
            Program.ReaderXP.Options.TagWriteUser.pData = data;

            start = DateTime.Now;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER, true) == CSLibrary.Constants.Result.OK)
            {
                double TotalSeconds = (DateTime.Now - start).TotalSeconds;
                MessageBox.Show("General Write Success : " + TotalSeconds.ToString() + "s");
            }
            else
            {
                MessageBox.Show("General Write Fail!!!");
            }
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            int count = int.Parse(textBox_Length.Text);
            DateTime start, end;
            UInt16 dataPattern = Convert.ToUInt16(comboBox_DataPattern.Text, 16);

            Program.ReaderXP.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
            Program.ReaderXP.Options.TagSelected.epcMask = new  CSLibrary.Structures.S_MASK(textBox_EPC.Text);
            Program.ReaderXP.Options.TagSelected.epcMaskOffset = 0;
            Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)Program.ReaderXP.Options.TagSelected.epcMask.Length * 8;
            Program.ReaderXP.Options.TagSelected.flags =  CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true) != CSLibrary.Constants.Result.OK)
            {
                MessageBox.Show("Selected tag failed");
                return;
            }

            Program.ReaderXP.Options.TagReadUser.retryCount = 31;
            Program.ReaderXP.Options.TagReadUser.accessPassword = 0x00000000;
            Program.ReaderXP.Options.TagReadUser.offset = UInt16.Parse(textBox_Offset.Text);
            Program.ReaderXP.Options.TagReadUser.count = UInt16.Parse(textBox_Length.Text);

            start = DateTime.Now;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_READ_USER, true) == CSLibrary.Constants.Result.OK)
            {
                double TotalSeconds = (DateTime.Now - start).TotalSeconds;
                UInt16 [] data = Program.ReaderXP.Options.TagReadUser.pData.ToUshorts();
                bool match = true;
                
                textBox_Data.Text = "";
                for (int cnt = 0; cnt < data.Length; cnt++)
                {
                    textBox_Data.Text += data[cnt].ToString("X4") + " ";
                    if (data[cnt] != dataPattern)
                        match = false;
                }

                MessageBox.Show("Read Success : " + TotalSeconds.ToString() + "s" + ", Verify : " + (match ? "OK" : "Fail"));
            }
            else
            {
                MessageBox.Show("General Write Fail!!!");
            }
        }

        private void button_BlockWrite_Click(object sender, EventArgs e)
        {
            int count = int.Parse(textBox_Length.Text);
            UInt16[] data = new UInt16[count];
            DateTime start, end;
            UInt16 dataPattern = Convert.ToUInt16(comboBox_DataPattern.Text, 16);

            for (int cnt = 0; cnt < count; cnt++)
                data[cnt] = dataPattern;

            Program.ReaderXP.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
            Program.ReaderXP.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(textBox_EPC.Text);
            Program.ReaderXP.Options.TagSelected.epcMaskOffset = 0;
            Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)Program.ReaderXP.Options.TagSelected.epcMask.Length * 8;
            Program.ReaderXP.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED, true) != CSLibrary.Constants.Result.OK)
            {
                MessageBox.Show("Selected tag failed");
                return;
            }

            Program.ReaderXP.Options.TagBlockWrite.flags = CSLibrary.Constants.SelectFlags.SELECT;
            Program.ReaderXP.Options.TagBlockWrite.retryCount = 31;
            Program.ReaderXP.Options.TagBlockWrite.accessPassword = 0;
            Program.ReaderXP.Options.TagBlockWrite.bank = CSLibrary.Constants.MemoryBank.USER;
            Program.ReaderXP.Options.TagBlockWrite.offset = UInt16.Parse(textBox_Offset.Text);
            Program.ReaderXP.Options.TagBlockWrite.count = UInt16.Parse(textBox_Length.Text);
            Program.ReaderXP.Options.TagBlockWrite.pData = data;

            start = DateTime.Now;

            //Program.ReaderXP.EngModeEnable("CSL2006");
            //Program.ReaderXP.EngDebugModeSetLogFile("C:\\TEMP\\D.TXT");
            //Program.ReaderXP.EngDebugModeEnable(CSLibrary.HighLevelInterface.DEBUGLEVEL.REGISTER);

            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_BLOCK_WRITE, true) == CSLibrary.Constants.Result.OK)
            {
                double TotalSeconds = (DateTime.Now - start).TotalSeconds;
                MessageBox.Show("Block Write Success : " + TotalSeconds.ToString() + "s");
            }
            else
            {
                MessageBox.Show("Block Write Fail!!!");
            }

            

#if nouse            
            
            Program.ReaderXP.Options.TagWriteUser.retryCount = 31;
            Program.ReaderXP.Options.TagWriteUser.accessPassword = 0x00000000;
            Program.ReaderXP.Options.TagWriteUser.offset = UInt16.Parse(textBox_Offset.Text);
            Program.ReaderXP.Options.TagWriteUser.count = UInt16.Parse(textBox_Length.Text);
            Program.ReaderXP.Options.TagWriteUser.pData = data;

            start = DateTime.Now;
            if (Program.ReaderXP.StartOperation(CSLibrary.Constants.Operation.TAG_WRITE_USER, true) == CSLibrary.Constants.Result.OK)
            {
                double TotalSeconds = (DateTime.Now - start).TotalSeconds;
                MessageBox.Show("General Write Success : " + TotalSeconds.ToString() + "s");
            }
            else
            {
                MessageBox.Show("General Write Fail!!!");
            }

#endif
        }
    }
}
