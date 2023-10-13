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

namespace CS203_CALLBACK_API_DEMO
{
    public partial class ReadRegisterForm : Form
    {
        public ReadRegisterForm()
        {
            InitializeComponent();
        }

        private void ReadRegisterForm_Load(object sender, EventArgs e)
        {
            cbType.SelectedIndex = 0;
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    groupBox1.Text = "MAC Register";
                    break;
                case 1:
                    groupBox1.Text = "Tilden Register";
                    break;
                case 2:
                    groupBox1.Text = "OEM Data";
                    break;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
#if ENGINEERING_MODE
            if (!IsHexValue(tbAddress.Text) || !IsHexValue(tbValue.Text))
            {
                MessageBox.Show("Invalid Hex format, please enter again.");
                return;
            }
            switch (cbType.SelectedIndex)
            {
                case 0:
                    uint macValue = 0;
                    Program.ReaderXP.MacReadRegister(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), ref macValue);
                    tbValue.Text = macValue.ToString("X");
                    break;
                case 1:
                    ushort tildenValue = 0;
                    Program.ReaderXP.MacBypassReadRegister(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), ref tildenValue);
                    tbValue.Text = tildenValue.ToString("X");
                    break;
                case 2:
                    uint[] oemValue = new uint[1];
                    Program.ReaderXP.MacReadOemData(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), 1, ref oemValue);
                    tbValue.Text = oemValue[0].ToString("X");
                    break;
            }
#endif
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
#if ENGINEERING_MODE
            if (!IsHexValue(tbAddress.Text) || !IsHexValue(tbValue.Text))
            {
                MessageBox.Show("Invalid Hex format, please enter again.");
                return;
            }
            switch (cbType.SelectedIndex)
            {
                case 0:
                    Program.ReaderXP.MacWriteRegister(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), uint.Parse(tbValue.Text));
                    break;
                case 1:
                    Program.ReaderXP.MacBypassWriteRegister(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), ushort.Parse(tbValue.Text));
                    break;
                case 2:
                    uint[] oemValue = new uint[1] { uint.Parse(tbValue.Text, System.Globalization.NumberStyles.HexNumber) };
                    Program.ReaderXP.MacWriteOemData(ushort.Parse(tbAddress.Text, System.Globalization.NumberStyles.HexNumber), 1, oemValue);
                    break;
            }
#endif
        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {
            if (tbAddress.Text == null || tbAddress.Text.Length == 0)
            {
                tbAddress.Text = "0";
            }
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            if (tbValue.Text == null || tbValue.Text.Length == 0)
            {
                tbValue.Text = "0";
            }
        }

        private bool IsHexValue(string s)
        {
            foreach (char c in s.ToCharArray())
            {
                if (!Uri.IsHexDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}