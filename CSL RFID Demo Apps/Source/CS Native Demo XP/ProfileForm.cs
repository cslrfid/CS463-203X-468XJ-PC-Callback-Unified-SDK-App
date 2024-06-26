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
    public partial class ProfileForm : Form
    {
        #region Private Member
        //private uint profile = 2;
        private bool m_close = false;
        private uint MaxProfile = 0;
        private int SelectProfile = 0;
        private int select_profile_index = 0;
        private uint[] Profile;
        #endregion

        #region Form
        public ProfileForm()
        {
            InitializeComponent();
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 240 - Height);
            Profile = Program.ReaderXP.GetActiveLinkProfile();
            MaxProfile = Profile[Profile.Length - 1];
            SelectProfile = (int)Program.appSetting.Link_profile;
            for (int index = 0; index < Profile.Length; index++)
            {
                if (Profile[index] == Program.appSetting.Link_profile)
                {
                    select_profile_index = index;
                    break;
                }
            }
            lb_profile.Text = SelectProfile.ToString();
        }

        private void ProfileForm_Closing(object sender, CancelEventArgs e)
        {

        }
        private void ProfileForm_GotFocus(object sender, EventArgs e)
        {
            m_close = false;
        }

        private void ProfileForm_LostFocus(object sender, EventArgs e)
        {
            CloseAndApply();
        }
        private void ProfileForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)0x1B)
            {
                CloseAndApply();
            }
        }
        #endregion

        #region Timer
        private void tmr_autoclose_Tick(object sender, EventArgs e)
        {
            CloseAndApply();
        }
        #endregion

        #region Function
        public void ProfileUp()
        {
            if (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
                return;

            this.Show();

            select_profile_index++;
            if (select_profile_index >= Profile.Length)
            {
                select_profile_index = Profile.Length - 1;
            }

            lb_profile.Text = Profile[select_profile_index].ToString();
            ResetTimer();
        }

        public void ProfileDown()
        {
            if (Program.ReaderXP.State != CSLibrary.Constants.RFState.IDLE)
                return;
            this.Show();
            select_profile_index--;
            if (select_profile_index < 0)
            {
                select_profile_index = 0;
            }

            lb_profile.Text = Profile[select_profile_index].ToString();
            ResetTimer();
        }

        private void ResetTimer()
        {
            tmr_autohide.Enabled = false;
            tmr_autohide.Enabled = true;
        }
        private void CloseAndApply()
        {
            if (!m_close)
            {
                m_close = true;
                if (Program.ReaderXP.State == CSLibrary.Constants.RFState.IDLE)
                {
                    Program.ReaderXP.SetCurrentLinkProfile(Profile[select_profile_index]);
                    Program.appSetting.Link_profile = Profile[select_profile_index];
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