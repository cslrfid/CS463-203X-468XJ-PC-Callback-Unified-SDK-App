using System;
using System.Collections.Generic;
using System.Text;

namespace CSLibrary.Diagnostics
{
#if DEBUG
    using CSLibrary.Diagnostics.Config;
#endif

    /// <summary>
    /// CSLibrary Debugger
    /// </summary>
#if !DEBUG
    public class CoreDebug
#else
    public class CoreDebug : Logger
#endif
    {
        private static object syncObject = new object();
#if DEBUG
        private static Logger m_logger = LogManager.CreateNullLogger();
#endif
        private static bool enable = false;
        /// <summary>
        /// Enable or Disable Debugger. You must include CSLibrary.xml in application directory.
        /// </summary>
        public static bool Enable
        {
            get { lock (syncObject) return CoreDebug.enable; }
            set
            {
                lock (syncObject)
                {
                    if (value != CoreDebug.enable)
                    {
                        try
                        {
                            if (value)
                            {
#if DEBUG
                                XmlLoggingConfiguration xmlConfig = new XmlLoggingConfiguration(System.IO.Path.Combine(HighLevelInterface.CurrentPath, "RFIDDebug.xml"));
                                LogManager.Configuration = xmlConfig;
                                m_logger = LogManager.GetLogger("RFID_DEBUGGER");
#if nouse
#if CS101
                                m_logger = LogManager.GetLogger("CS101_DEBUGGER");
#elif CS203
                                m_logger = LogManager.GetLogger("CS203_DEBUGGER");
#elif CS468
                                m_logger = LogManager.GetLogger("CS468_DEBUGGER");
#endif
#endif
                                m_logger.Trace("Debugger Started");
#endif
                            }
                            else
                            {
#if DEBUG
                                m_logger = LogManager.CreateNullLogger();
#endif
                            }
                            CoreDebug.enable = value;
                        }
                        catch
                        {
                            CoreDebug.enable = false;
                        }
                    }
                }
            }
        }

#if !DEBUG
        internal static int Logger
        {
            get { return 0; }
            set { }
        }
#else
        internal static Logger Logger
        {
            get { lock (syncObject) return CoreDebug.m_logger; }
            set { lock (syncObject) CoreDebug.m_logger = value; }
        }
#endif
    }
}
