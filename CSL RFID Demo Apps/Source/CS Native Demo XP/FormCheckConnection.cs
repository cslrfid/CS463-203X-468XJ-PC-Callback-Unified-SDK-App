using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CSLibrary;

namespace CS203_CALLBACK_API_DEMO
{
    public partial class FormCheckConnection : Form
    {
        HighLevelInterface reader = new HighLevelInterface();
        public string _deviceIP = "";
        public string _bcIP = "";


        public FormCheckConnection()
        {
            InitializeComponent();
        }

        void CheckStatus()
        {
            bool cbcs, cucs;
            CSLibrary.Structures.DEVICE_STATUS st = new CSLibrary.Structures.DEVICE_STATUS();
            
            WriteLog("UDP Broadcast Mode Check Reader Status");

_step2:
            cbcs = (HighLevelInterface.CheckStatus(_bcIP, _deviceIP, ref st) == CSLibrary.Constants.Result.OK);
            if (cbcs)
            {
                WriteLog("UDP Broadcast Mode Check Reader Status Successful");
            }
            else
            {
                WriteLog("UDP Broadcast Mode Check Reader Status Fails");
            }

            WriteLog("UDP Unicast Mode Check Reader Status");

            cucs = (HighLevelInterface.CheckStatus(_deviceIP, ref st) == CSLibrary.Constants.Result.OK);
            if (cucs)
            {
                WriteLog("UDP Unicast Mode Check Reader Status Successful");
            }
            else
            {
                WriteLog("UDP Unicast Mode Check Reader Status Fails");
            }

            if (!cbcs && !cucs)
            {
                WriteLog("Cannot Check Reader Status by UDP Broadcast and Unicast");
            }

            if (cbcs && !cucs)
            {
                DialogResult dialogResult = ReadLog("Do you want to UDP Broadcast Mode Reset the Reader?");
                if (dialogResult == DialogResult.Yes)
                {
                    if (HighLevelInterface.ForceReset(_deviceIP, _bcIP) == CSLibrary.Constants.Result.OK)
                    {
                        WriteLog("UDP Broadcast Reset Success");
                        goto _step2;
                    }
                    else
                    {
                        WriteLog("Reset RFID Section fails");
                    }
                }
            }

            WriteLog("TCP Connecting");

            System.Net.Sockets.Socket IntelCMD = null;
            try
            {

                IntelCMD = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);     // TCP 1515
                IntelCMD.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(_deviceIP), 1515));
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                //                Console.Out.WriteLine("Network connection fail{0}", ex.ToString());
            }

            if (IntelCMD.Connected)
            {
                WriteLog("Connection OK");
            }
            else
            {
                WriteLog("TCP Connect Fails");
            }


            /*
            WriteLog("TCP Connecting");
            if (reader.Connect(_deviceIP, 20000) == CSLibrary.Constants.Result.OK)
            {
                WriteLog("Connection OK");
                reader.Disconnect();
            }
            else
            {
                WriteLog("TCP Connect Fails");
            }
             */
        }

        void WriteLog(string format, params object[] arg)
        {
            System.IO.TextWriter tw = null;
            string Msg = string.Format(format, arg);

            //if (!_fileOpened)
            {
                tw = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CSLReader\checkconnection.log", true);
                //_fileOpened = true;
            }

            textBox_Log.Text += Msg + System.Environment.NewLine; 
            MessageBox.Show(Msg);
            tw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + " : " + Msg);

            tw.Close();
        }

        DialogResult ReadLog(string format, params object[] arg)
        {
            System.IO.TextWriter tw = null;
            string Msg = string.Format(format, arg);

            //if (!_fileOpened)
            {
                tw = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CSLReader\checkconnection.log", true);
                //_fileOpened = true;
            }

            textBox_Log.Text += Msg + System.Environment.NewLine;
            DialogResult dialogResult = MessageBox.Show(Msg, null , MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            tw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + " : " + Msg + ":" + dialogResult.ToString());

            tw.Close();

            return dialogResult;
        }

        private void FormCheckConnection_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
                CheckStatus();
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.BeginInvoke((System.Threading.ThreadStart)delegate()
            {
                CheckStatus();
            });
        }


    }
}
