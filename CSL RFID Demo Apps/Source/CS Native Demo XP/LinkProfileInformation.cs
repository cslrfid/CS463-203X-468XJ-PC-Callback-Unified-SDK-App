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
    public partial class LinkProfileInformation : Form
    {
        public LinkProfileInformation()
        {
            InitializeComponent();
        }

        public LinkProfileInformation(uint link)
            : this()
        {
            CSLibrary.LinkProfileInfo info = Program.ReaderXP.GetActiveLinkProfileInfo(link);

            lb_dataDifference.Text = info.LinkProfileConfig.data01Difference.ToString();
            lb_denseReaderMode.Text = info.DenseReaderMode ? "Enable" : "Disable";
            lb_divideRatio.Text = info.LinkProfileConfig.divideRatio.ToString();
            lb_millerNumber.Text = info.LinkProfileConfig.millerNumber.ToString();
            lb_modulationType.Text = info.LinkProfileConfig.modulationType.ToString();
            lb_nbRssiSamples.Text = info.NarrowbandRssiSamples.ToString();
            lb_profileID.Text = info.ProfileId.ToString();
            lb_profileState.Text = info.Enabled ? "Enable" : "Disable";
            lb_profileUID.Text = info.ProfileUniqueId;
            lb_profileVersion.Text = info.ProfileVersion.ToString();
            lb_radioProtocol.Text = info.ProfileProtocol.ToString();
            lb_realtimeNBRssiSamples.Text = info.RealtimeNarrowbandRssiSamples.ToString();
            lb_realtimeRssiEnable.Text = info.RealtimeRssiEnabled ? "Enable" : "Disable";
            lb_realTimeWBRssiSamples.Text = info.RealtimeWidebandRssiSamples.ToString();
            lb_wbRssiSamples.Text = info.WidebandRssiSamples.ToString();

        }
    }
}