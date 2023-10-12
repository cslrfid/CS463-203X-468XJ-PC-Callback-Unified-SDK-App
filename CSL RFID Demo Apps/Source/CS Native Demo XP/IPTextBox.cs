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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class IPTextBox : UserControl
    {
        public String IP
        {
            get { return string.Format("{0}.{1}.{2}.{3}", numBox1.Text, numBox2.Text, numBox3.Text, numBox4.Text); }
            set
            {
                if (value != null)
                {
                    string[] ip = value.Split(new char[] { '.' });
                    if (ip.Length == 4)
                    {
                        numBox1.Text = ip[0];
                        numBox2.Text = ip[1];
                        numBox3.Text = ip[2];
                        numBox4.Text = ip[3];
                    }
                    else return;
                }
            }
        }

        public IPTextBox()
        {
            InitializeComponent();
        }

    }
}
