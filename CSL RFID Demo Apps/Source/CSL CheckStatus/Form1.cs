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

using CSLibrary.Constants;
using CSLibrary.Structures;
using CSLibrary;

namespace CS203_CheckStatus
{
    public partial class Form1 : Form
    {
        uint _GOI0InterruptCount = 0;
        uint _GOI1InterruptCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            switch (lb.Text)
            {
                case "CONNECTED":
                case "ON":
                    lb.ForeColor = Color.Green;
                    break;
                case "OFF":
                case "LISTENING":
                    lb.ForeColor = Color.Red;
                    break;
                case "UNKNOWN":
                    lb.ForeColor = Color.Blue;
                    break;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            _GOI0InterruptCount = 0;
            _GOI1InterruptCount = 0;

            label_GPI0InterruptCount.Text = _GOI0InterruptCount.ToString();
            label_GPI1InterruptCount.Text = _GOI1InterruptCount.ToString();

            //while (true)
            {
                DEVICE_STATUS st = new DEVICE_STATUS();

                if (HighLevelInterface.CheckStatus(tbIpAddress.Text, ref st) == Result.OK)
                {
                    lbConnect.Text = st.IsConnected ? "CONNECTED" : "LISTENING";
                    lbElapsed.Text = st.GetElapsedTime().ToString();
                    lbKeepAlive.Text = st.IsKeepAlive ? "ON" : "OFF";
                    lbPower.Text = st.IsPowerOn ? "ON" : "OFF";
                    lbReset.Text = st.IsErrorReset ? "ON" : "OFF";
                    lbCrcFilter.Text = st.IsCRCFilter ? "ON" : "OFF";
                    tsStatus.Text = "CheckStatus OK!";
                }
                else
                {
                    lbConnect.Text = "UNKNOWN";
                    lbElapsed.Text = "UNKNOWN";
                    lbKeepAlive.Text = "UNKNOWN";
                    lbPower.Text = "UNKNOWN";
                    lbReset.Text = "UNKNOWN";
                    lbCrcFilter.Text = "UNKNOWN";
                    tsStatus.Text = "CheckStatus Failed!";
                }
                bool gpi0 = false, gpi1 = false;
                if (HighLevelInterface.GetGPIStatus(tbIpAddress.Text, ref gpi0, ref gpi1) == Result.OK)
                {
                    lbGPI0.Text = gpi0 ? "High" : "Low";
                    lbGPI1.Text = gpi1 ? "High" : "Low";
                    tsStatus.Text = "CheckGPI1Status OK!";
                }
                else
                {
                    lbGPI0.Text = "UNKNOWN";
                    lbGPI1.Text = "UNKNOWN";
                    tsStatus.Text = "CheckGPI1Status Failed!";
                }
                if (HighLevelInterface.GetGPOStatus(tbIpAddress.Text, ref gpi0, ref gpi1) == Result.OK)
                {
                    lbGPO0.Text = gpi0 ? "ON" : "OFF";
                    lbGPO1.Text = gpi1 ? "ON" : "OFF";
                    tsStatus.Text = "CheckGPI1Status OK!";
                }
                else
                {
                    lbGPO0.Text = "UNKNOWN";
                    lbGPO1.Text = "UNKNOWN";
                    tsStatus.Text = "CheckGPI1Status Failed!";
                }
                GPIOTrigger gpi0Trigger = GPIOTrigger.OFF, gpi1Trigger = GPIOTrigger.OFF;
                if (HighLevelInterface.GetGPIInterrupt(tbIpAddress.Text, ref gpi0Trigger, ref gpi1Trigger) == Result.OK)
                {
                    lbGPI0Interrupt.Text = gpi0Trigger.ToString();
                    lbGPI1Interrupt.Text = gpi1Trigger.ToString();
                    tsStatus.Text = "GetGPIInterrupt OK!";
                }
                else
                {
                    lbGPI0Interrupt.Text = "UNKNOWN";
                    lbGPI1Interrupt.Text = "UNKNOWN";
                    tsStatus.Text = "GetGPIInterrupt Failed!";
                }
                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }
        }

        void GetGPIOInterruptStatus()
        {
            GPIOTrigger gpi0Trigger = GPIOTrigger.OFF, gpi1Trigger = GPIOTrigger.OFF;
            if (HighLevelInterface.GetGPIInterrupt(tbIpAddress.Text, ref gpi0Trigger, ref gpi1Trigger) == Result.OK)
            {
                lbGPI0Interrupt.Text = gpi0Trigger.ToString();
                lbGPI1Interrupt.Text = gpi1Trigger.ToString();
                tsStatus.Text = "GetGPIInterrupt OK!";
            }
            else
            {
                lbGPI0Interrupt.Text = "UNKNOWN";
                lbGPI1Interrupt.Text = "UNKNOWN";
                tsStatus.Text = "GetGPIInterrupt Failed!";
            }
            Application.DoEvents();
            System.Threading.Thread.Sleep(1);
        }

        private void btnForceReset_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.ForceReset(tbIpAddress.Text) == Result.OK)
            {
                tsStatus.Text = "ForceReset OK!";
            }
            else
            {
                tsStatus.Text = "ForceReset Failed!";

            }
        }

        private void btnKeepAliveOff_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.UDPKeepAliveOff(tbIpAddress.Text) == Result.OK)
            {
                tsStatus.Text = "UDPKeepAliveOff OK!";

            }
            else
            {
                tsStatus.Text = "UDPKeepAliveOff Failed!";

            }
        }

        private void btnKeepAliveOn_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.UDPKeepAliveOn(tbIpAddress.Text) == Result.OK)
            {
                tsStatus.Text = "UDPKeepAliveOn OK!";

            }
            else
            {
                tsStatus.Text = "UDPKeepAliveOn Failed!";

            }
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");
            /*            if (HighLevelInterface.RFIDPowerOff(tbIpAddress.Text) == Result.OK)
                        {
                            tsStatus.Text = "RFIDPowerOff OK!";

                        }
                        else
                        {
                            tsStatus.Text = "RFIDPowerOff Failed!";

                        }
            */
        }

        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");
            /*            if (HighLevelInterface.RFIDPowerOn(tbIpAddress.Text) == Result.OK)
                        {
                            tsStatus.Text = "RFIDPowerOn OK!";

                        }
                        else
                        {
                            tsStatus.Text = "RFIDPowerOn Failed!";

                        }
            */        }

        private void btnErrRstOn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");
            /*            if (HighLevelInterface.AutoResetOn(tbIpAddress.Text) == Result.OK)
                        {
                            tsStatus.Text = "AutoResetOn OK!";

                        }
                        else
                        {
                            tsStatus.Text = "AutoResetOn Failed!";

                        }
            */        }

        private void btnErrRstOff_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");
            /*            if (HighLevelInterface.AutoResetOff(tbIpAddress.Text) == Result.OK)
                        {
                            tsStatus.Text = "AutoResetOff OK!";

                        }
                        else
                        {
                            tsStatus.Text = "AutoResetOff Failed!";

                        }
            */        }

        private void btnCrcFilterOn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");

/*            if (HighLevelInterface.CrcFilterOn(tbIpAddress.Text) == Result.OK)
            {
                tsStatus.Text = "CrcFilterOn OK!";

            }
            else
            {
                tsStatus.Text = "CrcFilterOn Failed!";

            }
*/        }

        private void btnCrcFilterOff_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Function NOT Continuous Support");
            /*            if (HighLevelInterface.CrcFilterOff(tbIpAddress.Text) == Result.OK)
                        {
                            tsStatus.Text = "CrcFilterOff OK!";

                        }
                        else
                        {
                            tsStatus.Text = "CrcFilterOff Failed!";
                        }
            */        }

        private void btnGPO0On_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPO0Status(tbIpAddress.Text, true) == Result.OK)
            {
                tsStatus.Text = "SetGPO0Status OK!";

            }
            else
            {
                tsStatus.Text = "SetGPO0Status Failed!";

            }
        }

        private void btnGPO0Off_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPO0Status(tbIpAddress.Text, false) == Result.OK)
            {
                tsStatus.Text = "SetGPO0Status OK!";

            }
            else
            {
                tsStatus.Text = "SetGPO0Status Failed!";

            }
        }

        private void btnGPO1On_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPO1Status(tbIpAddress.Text, true) == Result.OK)
            {
                tsStatus.Text = "SetGPO1Status OK!";

            }
            else
            {
                tsStatus.Text = "SetGPO1Status Failed!";

            }
        }

        private void btnGPO1Off_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPO1Status(tbIpAddress.Text, false) == Result.OK)
            {
                tsStatus.Text = "SetGPO1Status OK!";

            }
            else
            {
                tsStatus.Text = "SetGPO1Status Failed!";

            }
        }

        private void btnGPI1Interrupt_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPI1Interrupt(tbIpAddress.Text, (GPIOTrigger)cbGPI1Interrupt.SelectedIndex) == Result.OK)
            {
                tsStatus.Text = "SetGPI1Interrupt OK!";

            }
            else
            {
                tsStatus.Text = "SetGPI1Interrupt Failed!";

            }

            GetGPIOInterruptStatus();
        }

        private void btnGPI0Interrupt_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.SetGPI0Interrupt(tbIpAddress.Text, (GPIOTrigger)cbGPI0Interrupt.SelectedIndex) == Result.OK)
            {
                tsStatus.Text = "SetGPI0Interrupt OK!";

            }
            else
            {
                tsStatus.Text = "SetGPI0Interrupt Failed!";

            }

            GetGPIOInterruptStatus();
        }

        private void btnStartPoll_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.StartPollGPIStatus(GPIStatusCallback) == Result.OK)
            {
                tsStatus.Text = "StartPollGPIStatus OK!";

            }
            else
            {
                tsStatus.Text = "StartPollGPIStatus Failed!";

            }
        }

        private void btnStopPoll_Click(object sender, EventArgs e)
        {
            if (HighLevelInterface.StopPollGPIStatus() == Result.OK)
            {
                tsStatus.Text = "StopPollGPIStatus OK!";

            }
            else
            {
                tsStatus.Text = "StopPollGPIStatus Failed!";

            }
        }

        public bool GPIStatusCallback
        (
           string ip,
           int GPI0,
           int GPI1
        )
        {
            Invoke((System.Threading.ThreadStart)delegate()
            {
                if (ip == tbIpAddress.Text)
                {
                    if (GPI0 == 1)
                    {
                        lbGPI0.Text = "High";
                        _GOI0InterruptCount++;
                    }
                    else if (GPI0 == -1)
                    {
                        lbGPI0.Text = "Low";
                        _GOI0InterruptCount++;
                    }

                    if (GPI1 == 1)
                    {
                        lbGPI1.Text = "High";
                        _GOI1InterruptCount++;
                    }
                    else if (GPI1 == -1)
                    {
                        lbGPI1.Text = "Low";
                        _GOI1InterruptCount++;
                    }

                    label_GPI0InterruptCount.Text = _GOI0InterruptCount.ToString();
                    label_GPI1InterruptCount.Text = _GOI1InterruptCount.ToString();
                }
            });
            return true;
        }

        private void button_GetGPI0_Click(object sender, EventArgs e)
        {
            bool ret = false;

            if (HighLevelInterface.GetGPI0Status(tbIpAddress.Text, ref ret) == Result.OK)
            {
                if (ret)
                    lbGPI0.Text = "High";
                else
                    lbGPI0.Text = "Low";
            }
            else
                lbGPI1.Text = "UNKNOWN";

        }

        private void button_GetGPI1_Click(object sender, EventArgs e)
        {
            bool ret = false;

            if (HighLevelInterface.GetGPI1Status(tbIpAddress.Text, ref ret) == Result.OK)
            {
                if (ret)
                    lbGPI1.Text = "High";
                else
                    lbGPI1.Text = "Low";
            }
            else
                lbGPI1.Text = "UNKNOWN";
        }

        private void button_Set5VOn_Click(object sender, EventArgs e)
        {
            HighLevelInterface.Set5VPowerOut(tbIpAddress.Text, true);
        }

        private void button_Set5VOff_Click(object sender, EventArgs e)
        {
            HighLevelInterface.Set5VPowerOut(tbIpAddress.Text, false);
        }

        private void button_Get5VStatus_Click(object sender, EventArgs e)
        {
            bool ret = false;

            if (HighLevelInterface.Get5VPowerOut(tbIpAddress.Text, ref ret) == Result.OK)
            {
                if (ret)
                    label_5VOutStatus.Text = "ON";
                else
                    label_5VOutStatus.Text = "OFF";
            }
            else
                label_5VOutStatus.Text = "UNKNOWN";

        }
    }
}