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
using System.Threading;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class MessageWithGpioForm : Form
    {
        public static MessageWithGpioForm msgform = null;
        protected static TagInventoryWithGpioForm inv = null;
        private static Thread msgThread = null;

        public MessageWithGpioForm()
        {
            InitializeComponent();
        }

        public static Thread MsgThread
        {
            get { return MessageWithGpioForm.msgThread; }
        }
        private delegate void CloseFormDeleg();
        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CloseFormDeleg(CloseForm), new object[] { });
                return;
            }
            if (MessageWithGpioForm.msgform != null)
            {
                MessageWithGpioForm.msgform.Close();
            }
        }

        public static Thread LaunchForm(TagInventoryWithGpioForm form)
        {
            if (form == null) throw new ArgumentNullException("form");

            inv = form;

            //if (MsgThread != null) return MsgThread;

            MessageWithGpioForm.msgThread = new Thread(MsgThreadProc);
            MsgThread.Name = "MessageForm";
            MsgThread.Priority = ThreadPriority.Highest;
            MsgThread.IsBackground = false;
            MsgThread.Start();
            return MsgThread;
        }

        [STAThread]
        static void MsgThreadProc()
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0} is threadID {1}", System.Threading.Thread.CurrentThread.Name, System.Threading.Thread.CurrentThread.ManagedThreadId));
            msgform = new MessageWithGpioForm();

            msgform.StartPosition = FormStartPosition.CenterScreen;
            msgform.ShowInTaskbar = false;
            Application.Run(msgform);
            System.Diagnostics.Debug.WriteLine("ControlPanel Thread is exiting");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            inv.AbortReset();
        }
    }
}