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
    public partial class PowerForm : Form
    {
        private int power = 300;
        private bool m_close = false;
        #region Form
        public PowerForm()
        {
            InitializeComponent();
        }

        private void PowerForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(320 - Width, 240 - Height);
            //Program.ReaderXP.GetPowerLevel(ref power);
            power = (int)Program.appSetting.Power;
            lb_pwr.Text = string.Format("{0:F1} dBm", (double)power / 10);
        }
        private void PowerForm_LostFocus(object sender, EventArgs e)
        {
            CloseAndApply();
        }

        private void PowerForm_GotFocus(object sender, EventArgs e)
        {
            m_close = false;
        }
        #endregion

        private void tmr_autohide_Tick(object sender, EventArgs e)
        {
            CloseAndApply();
        }

        #region Function
        public void PowerUp()
        {
            if (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
                return;
            this.Show();
            power += 5;
            if (power > Program.ReaderXP.GetActiveMaxPowerLevel())
            {
                power = (int)Program.ReaderXP.GetActiveMaxPowerLevel();
            }
            lb_pwr.Text = string.Format("{0:F1} dBm", (double)power / 10);

            ResetTimer();
        }

        public void PowerDown()
        {
            if (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
                return;
            this.Show();
            power -= 5;
            if (power < 0)
            {
                power = 0;
            }
            lb_pwr.Text = string.Format("{0:F1} dBm", (double)power / 10);
            
            ResetTimer();
        }

        private void ResetTimer()
        {
            tmr_autohide.Enabled = false;
            tmr_autohide.Enabled = true;
        }

        
        private void PowerForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)0x1B)
            {
                CloseAndApply();
            }
        }

        private void CloseAndApply()
        {
            if (!m_close)
            {
                m_close = true;
                if (Program.ReaderXP.State == CSLibrary.Constants.RFState.IDLE)
                {
                    Program.ReaderXP.SetPowerLevel((uint)power);
                    Program.appSetting.Power = (uint)power;
                }
                else
                {

                }
                this.Hide();
                tmr_autohide.Enabled = false;
            }
        }
        #endregion

    }
}