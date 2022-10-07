using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using CSLibrary.Diagnostics;
using CSLibrary.Diagnostics.Config;

namespace CS203_CALLBACK_API_DEMO
{
    using CSLibrary;
    using CSLibrary.Constants;
    using CSLibrary.Structures;
    using CSLibrary.Tools;
    static class Program
    {
        public static int hours = 0;
        public static PowerForm Power = new PowerForm();
        public static ProfileForm Profile = new ProfileForm();

        public static string applicationSettings = "application.config";
        public static appSettings appSetting = new appSettings();
        public static string IP = String.Empty;
        public static string SerialNumber = String.Empty;
        public static string MAC = String.Empty;
        public static string BCIP = String.Empty;
        public static bool TCPNotify = false;

        public static HighLevelInterface ReaderXP = new HighLevelInterface();
        public static Logger tagLogger = null;
        //public static HighLevelInterface ReaderXP1 = new HighLevelInterface();

        public static string _PREFILTER_MASK_DATA;
        public static uint _PREFILTER_Bank = 1;
        public static uint _PREFILTER_Offset;
        public static bool _PREFILTER_Enable;

        public static string _POSTFILTER_MASK_EPC;
        public static uint _POSTFILTER_MASK_Offset;
        public static bool _POSTFILTER_MASK_MatchNot;
        public static bool _POSTFILTER_MASK_Enable;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DEVICE_STATUS s = new DEVICE_STATUS ();





            /*for (; ; )
            {
                if (ReaderXP.Connect("192.168.25.207", 30000) != Result.OK)
                    MessageBox.Show("Error");
                ReaderXP.Disconnect();
            }*/
            /*try
            {
                System.Xml.Serialization.XmlSerializer mySerializer = new System.Xml.Serialization.XmlSerializer(typeof(AntennaList));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }*/
            CSLibrary.Constants.Result ret = CSLibrary.Constants.Result.OK;
            Application.EnableVisualStyles();
#if __DISABLE_CS468
            CSLibrary.Diagnostics.CoreDebug.Enable = true;
            XmlLoggingConfiguration xmlConfig = new XmlLoggingConfiguration(System.IO.Path.Combine(HighLevelInterface.CurrentPath, "CS468TagDebug.xml"));
            LogManager.Configuration = xmlConfig;
            tagLogger = LogManager.GetLogger("TAG_DEBUGGER");
#endif
            //First Step
            while (true)
            {
#if NET_BUILD
                using (NetFinderForm finder = new NetFinderForm())
                {
                    if (finder.ShowDialog() == DialogResult.OK)
                    {
                        IP = finder.ConnectIP;
                        BCIP = finder.BroadcastIP;
                        MAC = finder.MAC;
                        TCPNotify = finder.checkBox_TCPNotify.Checked;
                    }
                    else
                    {
                        goto DEINIT;
                    }
                }
#endif
                CSLibrary.Windows.UI.SplashScreen.Show(CSLibrary.Windows.UI.SplashScreen.CSL.CS203);
                {
#if benchmark
                    Application.DoEvents();

                    DateTime time = DateTime.Now;
                    DateTime time1;

                    //int time = Environment.TickCount;
                    //int time1 = Environment.TickCount;
                    string timediff;

                    if ((ret = ReaderXP.Connect(IP, 30000)) != CSLibrary.Constants.Result.OK)
                    //                    if ((ret = ReaderXP.Connect("USB1", 0)) != CSLibrary.Constants.Result.OK)
                    //#else
                    //                    if ((ret = ReaderXP.Connect()) != CSLibrary.Constants.Result.OK)
                    //#endif
                    {
                        ReaderXP.Disconnect();
                        MessageBox.Show(String.Format("StartupReader Failed{0}", ret));
                        goto DEINIT;
                    }
                    time1 = DateTime.Now;

                    timediff = "Connect time tickcount" + (time1 - time).TotalMilliseconds;
                    MessageBox.Show(timediff);


                    //                    System.Diagnostics.Trace.WriteLine(string.Format("Connect time = {0}", Environment.TickCount - time));
#endif

                    Application.DoEvents();

                    //ReaderXP.EngModeEnable("CSL2006");
                    //ReaderXP.EngDebugModeSetLogFile ("CSLibraryDebug.txt");
                    //ReaderXP.EngDebugModeEnable(HighLevelInterface.DEBUGLEVEL.PERFORMANCE);

                    int time = Environment.TickCount;
#if NET_BUILD
                    // Check ARP table
                    {
                        System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo()
                        {
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            FileName = "arp.exe",
                            Arguments = "-a " + IP
                        };

                        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                        {
                            proc.StartInfo = procStartInfo;
                            proc.Start();

                            string output = proc.StandardOutput.ReadToEnd();

                            if (string.IsNullOrEmpty(output))
                                output = proc.StandardError.ReadToEnd();

                            if (!string.IsNullOrEmpty(output))
                            {
                                string[] words = output.Split(' ');

                                foreach (var word in words)
                                {
                                    if (word.Split('-').Length == 6)
                                    {
                                        string mac1 = word.Replace("-", ":");

                                        if (!string.Equals(mac1, MAC, StringComparison.CurrentCultureIgnoreCase))
                                            MessageBox.Show("The reader MAC Address NOT match Windows ARP Table, please reboot PC or run the command ARP -d <ip> in administrator mode to refresh ARP table");

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    ReaderXP.SetTCPNotificationEnable(TCPNotify);
                    if ((ret = ReaderXP.Connect(IP, 20000)) != CSLibrary.Constants.Result.OK)
#else
                    if ((ret = ReaderXP.Connect()) != CSLibrary.Constants.Result.OK)
#endif
                    {
                        ReaderXP.Disconnect();
                        MessageBox.Show(String.Format("StartupReader Failed{0}", ret));
                        goto DEINIT;
                    }
                    System.Diagnostics.Trace.WriteLine(string.Format("Connect time = {0}", Environment.TickCount - time));


                }

                //Load settings
                string path = Application.StartupPath;
                if (File.Exists(path + @"\settings.txt"))
                {
                    using (FileStream file = new FileStream(path + @"\settings.txt", FileMode.Open))
                    using (StreamReader sr = new StreamReader(file))
                    {
                        LocalSettings.ServerIP = sr.ReadLine();
                        LocalSettings.ServerName = sr.ReadLine();
                        LocalSettings.DBName = sr.ReadLine();
                        LocalSettings.UserID = sr.ReadLine();
                        LocalSettings.Password = sr.ReadLine();
                    }
                }
                else
                {
                    //Set to default
                    LocalSettings.ServerName = "SQLEXPRESS";
                }

                string MyDocumentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CSLReader";

                try
                {
                    System.IO.Directory.CreateDirectory(MyDocumentFolder);
                }
                catch (Exception ex)
                {
                }

                if ((SerialNumber = ReaderXP.GetPCBAssemblyCode()) == null)
                {
                    MessageBox.Show(String.Format("GetPCBAssemblyCode Failed"));
                }

                if (!System.IO.File.Exists(MyDocumentFolder + "\\" + SerialNumber + ".cfg"))
                {
                    LoadDefaultSettings();
                }
                else
                {
                    if (testLoadLoadSettings())
                        LoadSettings();
                    else
                        LoadDefaultSettings();
                }

                //Open MainForm and EnableVisualStyles
                Application.Run(new MenuForm());

                //Save settings
                using (FileStream file = new FileStream(path + @"\settings.txt", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine(LocalSettings.ServerIP);
                    sw.WriteLine(LocalSettings.ServerName);
                    sw.WriteLine(LocalSettings.DBName);
                    sw.WriteLine(LocalSettings.UserID);
                    sw.WriteLine(LocalSettings.Password);
                }

                ReaderXP.Disconnect();
#if USB_BUILD
                break;
#endif
           }
DEINIT:
             CSLibrary.Windows.UI.SplashScreen.Stop();
        }

        public static CSLibrary.Structures.Version GetDemoVersion()
        {
            System.Version sver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            CSLibrary.Structures.Version ver = new CSLibrary.Structures.Version();
            ver.major = (uint)sver.Major;
            ver.minor = (uint)sver.Minor;
            ver.patch = (uint)sver.Build;
            return ver;
        }

        #region Setting

        private static bool LoadDefaultSettings()
        {
            uint power = 0, linkProfile = 0;
            SingulationAlgorithm sing = SingulationAlgorithm.UNKNOWN;
            appSetting.SerialNum = SerialNumber;

            if (ReaderXP.GetPowerLevel(ref power) != Result.OK)
            {
                MessageBox.Show(String.Format("SetPowerLevel rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
            appSetting.Power = power;

            if (ReaderXP.GetCurrentLinkProfile(ref linkProfile) != Result.OK)
            {
                MessageBox.Show(String.Format("SetCurrentLinkProfile rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
            appSetting.Link_profile = linkProfile;
            if (appSetting.FixedChannel = ReaderXP.IsFixedChannel)
            {
                appSetting.Region = ReaderXP.SelectedRegionCode;
                appSetting.Channel_number = ReaderXP.SelectedChannel;
                appSetting.Lbt = ReaderXP.LBT_ON == LBT.ON;
            }
            else
            {
                appSetting.Region = ReaderXP.SelectedRegionCode;
            }

            appSetting.Singulation = SingulationAlgorithm.DYNAMICQ;

            if (ReaderXP.SetSingulationAlgorithmParms(appSetting.Singulation, appSetting.SingulationAlg) != Result.OK)
            {
                MessageBox.Show(String.Format("GetCurrentSingulationAlgorithm rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
            if (ReaderXP.SetCurrentSingulationAlgorithm(appSetting.Singulation) != Result.OK)
            {
                MessageBox.Show(String.Format("SetCurrentSingulationAlgorithm rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }



/*
            if (ReaderXP.GetCurrentSingulationAlgorithm(ref sing) != Result.OK)
            {
                MessageBox.Show(String.Format("GetCurrentSingulationAlgorithm rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
            appSetting.Singulation = sing;

            if (ReaderXP.GetSingulationAlgorithmParms(appSetting.Singulation, appSetting.SingulationAlg) != Result.OK)
            {
                MessageBox.Show(String.Format("GetCurrentSingulationAlgorithm rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
*/
            appSetting.AntennaList = AntennaList.DEFAULT_ANTENNA_LIST;

            if (Program.ReaderXP.OEMDeviceType == Machine.CS203X)
            {
                appSetting.AntennaList[0].State = AntennaPortState.DISABLED;
                appSetting.AntennaList[1].State = AntennaPortState.DISABLED;
                appSetting.AntennaList[2].State = AntennaPortState.DISABLED;
                appSetting.AntennaList[3].State = AntennaPortState.ENABLED;
            
                if (appSetting.AntennaList.Store(ReaderXP) != Result.OK)
                {
                    MessageBox.Show(String.Format("SetAntennaList rc = {0}", ReaderXP.LastResultCode));
                    Application.Exit();
                    return false;
                }
            }

            
            return true;
        }

        static void SetLNADefault()
        {
            int rflna_high_comp_norm = Program.appSetting.rflna_high_comp_norm;
            int rflna_gain_norm = 0;
            int iflna_gain_norm = 0;
            int ifagc_gain_norm = 1;

            switch (Program.appSetting.rflna_gain_norm)
            {
                case 0:
                    rflna_gain_norm = 1;
                    break;
                case 1:
                    rflna_gain_norm = 7;
                    break;
                case 2:
                    rflna_gain_norm = 13;
                    break;
            }

            switch (Program.appSetting.iflna_gain_norm)
            {
                case 0:
                    iflna_gain_norm = 24;
                    break;
                case 1:
                    iflna_gain_norm = 18;
                    break;
                case 2:
                    iflna_gain_norm = 12;
                    break;
                case 3:
                    iflna_gain_norm = 6;
                    break;
            }

            switch (Program.appSetting.ifagc_gain_norm)
            {
                case 0:
                    ifagc_gain_norm = -12;
                    break;
                case 1:
                    ifagc_gain_norm = -6;
                    break;
                case 2:
                    ifagc_gain_norm = 0;
                    break;
                case 3:
                    ifagc_gain_norm = 6;
                    break;
            }

            Program.ReaderXP.SetLNA(rflna_high_comp_norm, rflna_gain_norm, iflna_gain_norm, ifagc_gain_norm);
        }

        public static bool testLoadLoadSettings()
        {
            appSettings a;
            a = appSetting.Load();

            if (a == null)
                return false;

            return true;
        }

        public static bool LoadSettings()
        {
            appSetting.AntennaList = AntennaList.DEFAULT_ANTENNA_LIST;
            appSetting.AntennaList.Clear();

            appSetting = appSetting.Load();

            if (appSetting == null)
                return false;

            //Previous save config not match
            if (appSetting.SerialNum != SerialNumber)
                return false;

            if (ReaderXP.SetReversePowerThreshold (appSetting.Cfg_ReversePowerThreshold) != Result.OK)
            {
                MessageBox.Show(String.Format("SetReversePowerThreshold rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }

            if (ReaderXP.SetCurrentLinkProfile(appSetting.Link_profile) != Result.OK)
            {
                MessageBox.Show(String.Format("SetCurrentLinkProfile rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }
            
            if (appSetting.FixedChannel)
            {
                if (appSetting.FreqAgile == false)
                {
                    if (ReaderXP.SetFixedChannel(appSetting.Region, appSetting.Channel_number, appSetting.Lbt ? LBT.ON : LBT.OFF) != Result.OK)
                    {
                        MessageBox.Show(String.Format("SetFixedChannel rc = {0}", ReaderXP.LastResultCode));
                        Application.Exit();
                        return false;
                    }
                }
                else
                {
                    if (ReaderXP.SetAgileChannels(appSetting.Region) != Result.OK)
                    {
                        MessageBox.Show(String.Format("SetAgileChannel rc = {0}", ReaderXP.LastResultCode));
                        Application.Exit();
                        return false;
                    }
                }
            }
            else
            {
                if (ReaderXP.SetHoppingChannels(appSetting.Region) != Result.OK)
                {
                    MessageBox.Show(String.Format("SetHoppingChannels rc = {0}", ReaderXP.LastResultCode));
                    Application.Exit();
                    return false;
                }
            }
            if (ReaderXP.SetSingulationAlgorithmParms(appSetting.Singulation, appSetting.SingulationAlg) != Result.OK)
            {
                MessageBox.Show(String.Format("SetSingulationAlgorithmParms rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }

            if (appSetting.AntennaList == null)
                ReaderXP.AntennaList = AntennaList.DEFAULT_ANTENNA_LIST;
            else
                ReaderXP.AntennaList.Copy(appSetting.AntennaList);

            if (appSetting.AntennaList.Store(ReaderXP) != Result.OK)
            {
                MessageBox.Show(String.Format("SetAntennaList rc = {0}", ReaderXP.LastResultCode));
                Application.Exit();
                return false;
            }

            if (appSetting.AntennaSequenceMode == AntennaSequenceMode.SEQUENCE ||
                appSetting.AntennaSequenceMode == AntennaSequenceMode.SEQUENCE_SMART_CHECK)
            {
                ReaderXP.AntennaSequenceSize = appSetting.AntennaSequenceSize;
                ReaderXP.AntennaSequenceMode = appSetting.AntennaSequenceMode;
                Array.Copy(appSetting.AntennaPortSequence, 0, ReaderXP.AntennaPortSequence, 0, appSetting.AntennaPortSequence.Length);
                if (ReaderXP.SetAntennaSequence(ReaderXP.AntennaPortSequence, ReaderXP.AntennaSequenceSize, ReaderXP.AntennaSequenceMode) != Result.OK)
                {
                    MessageBox.Show(String.Format("SetAntennaSequence rc = {0}", ReaderXP.LastResultCode));
                    Application.Exit();
                    return false;
                }
            }
            else
            {
                ReaderXP.AntennaSequenceSize = 0;
                ReaderXP.SetAntennaSequence((int)ReaderXP.AntennaSequenceSize);
            }

            SetLNADefault();

            return true;
        }

        public static bool SaveSettings()
        {
            appSetting.Channel_number = ReaderXP.SelectedChannel;
            appSetting.Lbt = !(ReaderXP.LBT_ON == LBT.OFF);
            appSetting.Link_profile = ReaderXP.SelectedLinkProfile;
            appSetting.Power = ReaderXP.SelectedPowerLevel;
            appSetting.Region = ReaderXP.SelectedRegionCode;
            //appSetting.FixedChannel = ReaderXP.IsFixedChannel;
            appSetting.SerialNum = SerialNumber;

            appSetting.AntennaList = ReaderXP.AntennaList;
            appSetting.AntennaSequenceMode = ReaderXP.AntennaSequenceMode;
            appSetting.AntennaSequenceSize = ReaderXP.AntennaSequenceSize;
            appSetting.AntennaPortSequence = ReaderXP.AntennaPortSequence;

            return appSetting.Save();
        }
        #endregion

        #region Reader Function

        static public Result TagSelectedEPC (string EPC)
        {
            Program.ReaderXP.Options.TagSelected.flags = SelectMaskFlags.ENABLE_TOGGLE;
            Program.ReaderXP.Options.TagSelected.bank = MemoryBank.EPC;
            Program.ReaderXP.Options.TagSelected.epcMask = new S_MASK(EPC);
            Program.ReaderXP.Options.TagSelected.epcMaskLength = (uint)Program.ReaderXP.Options.TagSelected.epcMask.Length * 8;
        
            return (Program.ReaderXP.StartOperation(Operation.TAG_SELECTED, true));
        }

        static public Result TagRead(MemoryBank bank, UInt16 offset, UInt16 count, UInt32 password, ref UInt16[] data)
        {
            Result ret;

            Program.ReaderXP.Options.TagWriteUser.accessPassword = password;
            Program.ReaderXP.Options.TagWriteUser.retryCount = 7;
            Program.ReaderXP.Options.TagReadUser.offset = offset;
            Program.ReaderXP.Options.TagReadUser.count = count;
            Program.ReaderXP.Options.TagReadUser.pData = new S_DATA ();

            ret = Program.ReaderXP.StartOperation(Operation.TAG_READ_USER, true);

            if (ret == Result.OK)
                data = Program.ReaderXP.Options.TagReadUser.pData.ToUshorts();

            return ret;
        }

        static public Result TagWrite(MemoryBank bank, UInt16 offset, UInt16 count, UInt32 password, UInt16[] data)
        {
            Program.ReaderXP.Options.TagWriteUser.accessPassword = password;
            Program.ReaderXP.Options.TagWriteUser.retryCount = 7;
            Program.ReaderXP.Options.TagWriteUser.offset = offset;
            Program.ReaderXP.Options.TagWriteUser.count = count;
            Program.ReaderXP.Options.TagWriteUser.pData = data;

            return Program.ReaderXP.StartOperation(Operation.TAG_WRITE_USER, true);
        }
        
        #endregion
    }
}