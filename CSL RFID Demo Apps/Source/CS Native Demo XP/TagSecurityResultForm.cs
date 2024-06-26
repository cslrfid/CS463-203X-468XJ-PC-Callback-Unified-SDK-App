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
    public partial class ResultForm : UserControl
    {
        private Label lb_result;
        private LinkLabel lk_close;
        private bool Result = true;

        public ResultForm()
        {
            InitializeComponent();
        }
        private delegate void UpdateResultDeleg(bool success);
        public void UpdateResult(bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateResultDeleg(UpdateResult), new object[] { success });
                return;
            }
            if (Result != success)
            {
                if (success)
                {
                    lb_result.Text = "OK";
                    BackColor = Color.Green;
                }
                else
                {
                    lb_result.Text = "FAIL";
                    BackColor = Color.Red;
                }
                Result = success;
            }
            this.Visible = true;
        }

        private void InitializeComponent()
        {
            this.lb_result = new System.Windows.Forms.Label();
            this.lk_close = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lb_result
            // 
            this.lb_result.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular);
            this.lb_result.Location = new System.Drawing.Point(3, 0);
            this.lb_result.Name = "lb_result";
            this.lb_result.Size = new System.Drawing.Size(144, 68);
            this.lb_result.Text = "OK";
            this.lb_result.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lk_close
            // 
            this.lk_close.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lk_close.Location = new System.Drawing.Point(128, 0);
            this.lk_close.Name = "lk_close";
            this.lk_close.Size = new System.Drawing.Size(19, 18);
            this.lk_close.TabIndex = 1;
            this.lk_close.Text = "X";
            this.lk_close.Click += new System.EventHandler(this.lk_close_Click);
            // 
            // ResultForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.lk_close);
            this.Controls.Add(this.lb_result);
            this.Name = "ResultForm";
            this.Size = new System.Drawing.Size(150, 66);
            this.ResumeLayout(false);

        }

        private void lk_close_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.Visible = false;
        }

    }
}