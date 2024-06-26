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
using System.Runtime.InteropServices;
using System.Text;

using CSLibrary;
using CSLibrary.Constants;

namespace CSLibrary
{

    namespace Structures
    {
        // Auto-marshal ok
        /// <summary>
        /// Version Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class Version
        {
            /// <summary>
            /// Major
            /// </summary>
            public UInt32 major = 0;
            /// <summary>
            /// Minor
            /// </summary>
            public UInt32 minor = 0;
            /// <summary>
            /// Patch
            /// </summary>
            public UInt32 patch = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public Version()
            {
                // NOP
            }
            /// <summary>
            /// Convert Version to string
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("{0}.{1}.{2:00}", major, minor, patch);
            }
        }


        // Auto-marshaling ok
        /// <summary>
        /// Library Version Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class LibraryVersion
            :
            Version
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public LibraryVersion()
            {
                // NOP
            }
        }


        // Auto-marshaling ok
        /// <summary>
        /// Driver Version Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class DriverVersion
            :
            Version
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public DriverVersion()
            {
                // NOP
            }
        }

        // Auto-marshaling ok
        /// <summary>
        /// Mac Version Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class MacVersion
            :
            Version
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public MacVersion()
            {
                // NOP
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="major"></param>
            /// <param name="minor"></param>
            /// <param name="patch"></param>
            public MacVersion(UInt32 major, UInt32 minor, UInt32 patch)
            {
                base.major = major;
                base.minor = minor;
                base.patch = patch;
            }
        }


        // No auto-marshaling due to variable size array
        /// <summary>
        /// When  detecting  the  attached  RFID  radio  modules,  the  structures  for  retrieving 
        /// the information for the attached RFID radio modules are as follows: 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class RadioInformation
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length = 0;
            /// <summary>
            /// The version information for the bus driver that is used 
            /// to communicate with the radio module. 
            /// </summary>
            public DriverVersion driverVersion = new DriverVersion();
            /// <summary>
            /// A unique cookie for the particular radio.  This cookie 
            /// will be provided to the RFID_RadioOpen function 
            /// when the application wishes to take control of the 
            /// radio.  Note that cookies are guaranteed to only be 
            /// valid for as long as (1) the corresponding RFID radio 
            /// module is attached to the host system, (2) the 
            /// corresponding RFID radio module is not power cycled, 
            /// (3) the corresponding RFID radio module's MAC 
            /// firmware is not reset, and (4) the RFID Reader Library 
            /// is not shut down. 
            /// </summary>
            public UInt32 cookie = 0;
            /// <summary>
            /// The length, in bytes, of the radio's unique ID (aka, 
            /// serial number) including the null terminator. 
            /// </summary>
            public UInt32 idLength = 0;
            /// <summary>
            /// A pointer to an array of bytes that contains the unique 
            /// ID (aka, serial number) for the radio represented as a 
            /// null terminated ASCII character string.  
            /// </summary>
            public Byte[] uniqueId = new Byte[0];
            /// <summary>
            /// Constructor
            /// </summary>
            public RadioInformation()
            {
                // NOP
            }
        }

        // Internal class - not represented in native lib other 
        // than as anon union profileConfig in RadioLinkProfile
        /// <summary>
        /// Link Profile Configuration Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class ProfileConfig
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length_ = 4;
            /// <summary>
            /// Constructor
            /// </summary>
            public ProfileConfig()
            {
                // NOP - length_ MUST be set by child classes
            }
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length
            {
                get { return this.length_; }
            }
        }


        // Auto-marshaling ok
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class RadioLinkProfileConfig
            :
            ProfileConfig
        {
            /// <summary>
            /// The modulation type that is used by the profile. 
            /// </summary>
            public ModulationType modulationType = ModulationType.UNKNOWN;
            /// <summary>
            /// The duration of the Tari in nanoseconds. 
            /// </summary>
            public UInt32 tari = 0;
            /// <summary>
            /// The difference, in Tari, between an data zero and a data one. 
            /// </summary>
            public DataDifference data01Difference = DataDifference.UNKNOWN;
            /// <summary>
            /// The duration of the low-going portion of the interrogator-to-
            /// tag PIE symbol (i.e., pulse width) in nanoseconds. 
            /// </summary>
            public UInt32 pulseWidth = 0;
            /// <summary>
            /// The width of the interrogator (i.e., radio) to tag calibration 
            /// (i.e., RTCal) in nanoseconds.  
            /// </summary>
            public UInt32 rtCalibration = 0;
            /// <summary>
            /// The width of the tag to interrogator calibration (i.e., TRCal) in 
            /// nanoseconds. 
            /// </summary>
            public UInt32 trCalibration = 0;
            /// <summary>
            /// The tag-to-interrogator divide ratio that is sent as part of the 
            /// Query command. 
            /// </summary>
            public DivideRatio divideRatio = DivideRatio.UNKNOWN;
            /// <summary>
            /// The miller number (i.e., cycles per symbol) that is sent as 
            /// part of the Query command. 
            /// </summary>
            public MillerNumber millerNumber = MillerNumber.UNKNOWN;
            /// <summary>
            /// The tag-to-interrogator link frequency in Hz.  Note that this is 
            /// not the link data-rate.  The link data rate can be computed by 
            /// dividing this value by 2millerNumber. 
            /// </summary>
            public UInt32 trLinkFrequency = 0;
            /// <summary>
            /// The delay, in microseconds, that is inserted to ensure 
            /// meeting the minimum ISO 18000-6C T2 timing. 
            /// </summary>
            public UInt32 varT2Delay = 0;
            /// <summary>
            /// The amount of time, measured in 48MHz clock 
            /// cycles, after transmitting that the radio module 
            /// will wait before attempting to receive 
            /// backscattered signals from tags. 
            /// </summary>
            public UInt32 rxDelay = 0;
            /// <summary>
            /// The minimum ISO 18000-6C T2 time, in 
            /// microseconds, after receiving tag responses 
            /// before a radio may transmit again. 
            /// </summary>
            public UInt32 minT2Delay = 0;
            /// <summary>
            /// The number of microseconds it takes for a 
            /// signal to propagate through the radio's transmit 
            /// chain. 
            /// </summary>
            public UInt32 txPropagationDelay = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public RadioLinkProfileConfig()
            {
                base.length_ = (UInt32)56;
            }
        };


        // No auto-marshaling due to base class ProfileConfig use...
        /// <summary>
        /// RadioLinkProfile Structure
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class RadioLinkProfile
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length_ = (UInt32)(48 + 56);
            /// <summary>
            /// Indicates if the profile is the active one.  A non-zero 
            /// value indicates that the profile is active.  A zero value 
            /// indicates that the profile is inactive. 
            /// </summary>
            public UInt32 enabled = 0;
            /// <summary>
            /// The identifier used for the link profile.  This field 
            /// combined with profileVersion provides a unique 
            /// identifier for the link profile. 
            /// </summary>
            public UInt64 profileId = 0;
            /// <summary>
            /// The version of the link profile.  This field combined with 
            /// profileId provides a unique identifier for the link 
            /// profile. 
            /// </summary>
            public UInt32 profileVersion = 0;
            /// <summary>
            /// The tag protocol for which this link profile has been 
            /// configured.  The value of this field determines which of 
            /// the structures within the profileConfig contains the 
            /// link profile configuration information. 
            /// </summary>
            protected RadioProtocol profileProtocol_ = RadioProtocol.ISO18K6C;
            /// <summary>
            /// Indicates if this is a dense-reader-mode (DRM) profile.  
            /// A non-zero value indicates that the profile is a DRM 
            /// profile.  A zero value indicates that the profile is not a 
            /// DRM profile. 
            /// </summary>
            public UInt32 denseReaderMode = 0;
            /// <summary>
            /// Number of samples over which the wide band Receive 
            /// Signal Strength Indication (RSSI) will be averaged. 
            /// </summary>
            public UInt32 widebandRssiSamples = 0;
            /// <summary>
            /// Number of samples over which the narrow band RSSI 
            /// will be averaged. 
            /// </summary>
            public UInt32 narrowbandRssiSamples = 0;
            /// <summary>
            /// Reserved for future use.  
            /// </summary>
            public UInt32 realtimeRssiEnabled = 0;
            /// <summary>
            /// Reserved for future use. 
            /// </summary>
            public UInt32 realtimeWidebandRssiSamples = 0;
            /// <summary>
            /// Reserved for future use. 
            /// </summary>
            public UInt32 realtimeNarrowbandRssiSamples = 0;
            /// <summary>
            /// The link profile configuration information.  This is a 
            /// discriminated union, with profileProtocol as the 
            /// discriminator (i.e., determines which structure within 
            /// the union is used).  For example, if 
            /// profileProtocol is 
            /// RFID_RADIO_PROTOCOL_ISO18K6C, the iso18K6C 
            /// field is used. 
            /// </summary>
            protected ProfileConfig profileConfig_ = new RadioLinkProfileConfig();
            /// <summary>
            /// Constructor
            /// </summary>
            public RadioLinkProfile()
            {
                // NOP
            }
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length
            {
                get { return this.length_; }
            }
            /// <summary>
            /// The tag protocol for which this link profile has been 
            /// configured.  The value of this field determines which of 
            /// the structures within the profileConfig contains the 
            /// link profile configuration information. 
            /// </summary>
            public RadioProtocol profileProtocol
            {
                get { return this.profileProtocol_; }

                set
                {
                    if (RadioProtocol.UNKNOWN == value)
                    {
                        this.profileProtocol_ = value;
                        this.profileConfig_ = new ProfileConfig();
                    }
                    else if (RadioProtocol.ISO18K6C == value)
                    {
                        this.profileProtocol_ = value;
                        this.profileConfig_ = new RadioLinkProfileConfig();
                    }
                }
            }
            /// <summary>
            /// The link profile configuration information.  This is a 
            /// discriminated union, with profileProtocol as the 
            /// discriminator (i.e., determines which structure within 
            /// the union is used).  For example, if 
            /// profileProtocol is 
            /// RFID_RADIO_PROTOCOL_ISO18K6C, the iso18K6C 
            /// field is used.            /// </summary>
            public ProfileConfig profileConfig
            {
                get { return this.profileConfig_; }

                set
                {
                    Type configType = value.GetType();

                    if (configType == typeof(CSLibrary.Structures.ProfileConfig))
                    {
                        this.profileProtocol_ = RadioProtocol.UNKNOWN;
                        this.profileConfig_ = value;
                    }
                    else if (configType == typeof(CSLibrary.Structures.RadioLinkProfileConfig))
                    {
                        this.profileProtocol_ = RadioProtocol.ISO18K6C;
                        this.profileConfig_ = value;
                    }
                }
            }
        };



        // Auto-marshaling ok
        /// <summary>
        /// The structure used to retrieve the status for a logical antenna port is defined as 
        /// follows: 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class AntennaPortStatus
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length_ = 40;
            /// <summary>
            /// Indicates the state of the logical antenna port. 
            /// </summary>
            public AntennaPortState state = AntennaPortState.UNKNOWN;
            /// <summary>
            /// EAS Alarm
            /// </summary>
            public Boolean easAlarm;
            /// <summary>
            /// Enable local Inventory parameter.
            /// </summary>
//            [MarshalAs(UnmanagedType.Bool)]
            public Boolean enableLocalInv;
            /// <summary>
            /// Inventory local parameter 
            /// </summary>
            public SingulationAlgorithm inv_algo;
            /// <summary>
            /// Inventory local parameter
            /// </summary>
            public UInt32 startQ;
            /// <summary>
            /// Enable local profile
            /// </summary>
//            [MarshalAs(UnmanagedType.Bool)]
            public Boolean enableLocalProfile;
            /// <summary>
            /// Link Profile
            /// </summary>
            public UInt32 profile;
            /// <summary>
            /// Enable Local frequency
            /// </summary>
//            [MarshalAs(UnmanagedType.Bool)]
            public Boolean enableLocalFreq;
            /// <summary>
            /// frequency channel to use.
            /// </summary>
            public UInt32 freqChn;
            /// <summary>
            /// The stored value from the last measurement of the 
            /// antenna-sense resistor for the logical antenna port's 
            /// physical transmit antenna port.  The last measurement 
            /// taken occurred the last time that the carrier wave was 
            /// turned on for this antenna port – note that this means that 
            /// when retrieving the logical antenna port's status, this does 
            /// not Result in an active measurement of the antenna-sense 
            /// resistor.  This value is specified in ohms. 
            /// </summary>
            public UInt32 antennaSenseValue = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public AntennaPortStatus()
            {
                // NOP
            }
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length
            {
                get { return this.length_; }
            }
        };


        // Auto-marshaling ok
        /// <summary>
        /// When configuring or retrieving the configuration for logical antenna ports, an 
        /// application has several parameters that it can set/retrieve.  
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class AntennaPortConfig
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length_ = 28;
            /// <summary>
            /// The power level for the logical antenna port's physical 
            /// transmit antenna.  This value is specified in 0.1 (i.e., 
            /// 1/10th) dBm. Note that all radio modules may not support 
            /// setting an antenna port's power level at 1/10th dBm 
            /// resolutions.  The dBm rounding/truncation policy is left to 
            /// the radio module and is outside the scope of the RFID 
            /// Reader Library. 
            /// </summary>
            public UInt32 powerLevel = 0;
            /// <summary>
            /// Specifies the maximum amount of time, in milliseconds, 
            /// that may be spent on the logical antenna port during a 
            /// tag-protocol-operation cycle before switching to the next 
            /// enabled antenna port.  A value of zero indicates that there 
            /// is no maximum dwell time for this antenna port.  If this 
            /// parameter is zero, then numberInventoryCycles may 
            /// not be zero. 
            /// See  for the effect of antenna-port dwell time and number 
            /// of inventory cycles on the amount of time spent on an 
            /// antenna port during a single tag-protocol-operation cycle. 
            /// NOTE:  when performing any non-inventory ISO 18000-6C tag
            /// access operation (i.e., read, write, kill, or lock), the
            /// radio module ignores the dwell time for the antenna port 
            /// which is used for the tag-protocol operation. 
            /// </summary>
            public UInt32 dwellTime = 0;
            /// <summary>
            /// Specifies the maximum number of inventory cycles to 
            /// attempt on the antenna port during a tag-protocol-
            /// operation cycle before switching to the next enabled 
            /// antenna port.  An inventory cycle consists of one or more 
            /// executions of the singulation algorithm for a particular 
            /// inventory-session target (i.e., A or B).  If the singulation 
            /// algorithm [SING-ALG] is configured to toggle the 
            /// inventory-session, executing the singulation algorithm for 
            /// inventory session A and inventory session B counts as 
            /// two inventory cycles.  A value of zero indicates that there 
            /// is no maximum number of inventory cycles for this 
            /// antenna port.  If this parameter is zero, then dwellTime 
            /// may not be zero. 
            /// See  for the effect of antenna-port dwell time and number 
            /// of inventory cycles on the amount of time spent on an 
            /// antenna port during a single tag-protocol-operation cycle. 
            /// NOTE:  when performing any non-inventory ISO 18000-
            /// 6C tag access operation (i.e., read, write, kill, or lock), the 
            /// radio module ignores the number of inventory cycles for 
            /// the antenna port which is used for the tag-protocol 
            /// operation. 
            /// </summary>
            public UInt32 numberInventoryCycles = 0;
            /// <summary>
            /// The physical receive port that this logical antenna port is 
            /// mapped to.  Consult [MAC-EDS] for the valid physical 
            /// receive antenna ports.  In version 1.1, when calling 
            /// RFID_AntennaPortSetConfiguration this value 
            /// must be the same as the value in physicalTxPort. 
            /// </summary>
            public UInt32 physicalRxPort = 0;
            /// <summary>
            /// The physical transmit port that this logical antenna port is 
            /// mapped to.  Consult [MAC-EDS] for the valid physical 
            /// receive antenna ports.  In version 1.1, when calling 
            /// RFID_AntennaPortSetConfiguration this value 
            /// must be the same as the value in physcialRxPort. 
            /// </summary>
            public UInt32 physicalTxPort = 0;
            /// <summary>
            /// The measured resistance, specified in ohms, above which 
            /// the antenna-sense resistance should be considered to be 
            /// an open circuit (i.e., a disconnected antenna).  If it is 
            /// detected that the antenna-sense resistance is above the 
            /// threshold, the carrier wave will not be turned on in order 
            /// to protect the circuit. 
            /// NOTE:  This value, while appearing in the per-antenna 
            /// configuration is actually a system-wide setting in the 
            /// current release.  Changing it will Result in the value being 
            /// changed for all antennas.  To prevent unintentionally 
            /// changing this value for all antennas, it is best to first 
            /// retrieve the antenna configuration for the antenna for 
            /// which configuration will be changed, update the fields that 
            /// should be changed, and then set the configuration. 
            /// </summary>
            public UInt32 antennaSenseThreshold = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public AntennaPortConfig()
            {
                // NOP
            }
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length
            {
                get { return this.length_; }
            }
        };


        // Auto-marshaling ok since fixed size array
        /// <summary>
        /// The tag mask is used to specify a bit pattern that is used to match against one of 
        /// a tag's memory banks to determine if it is matching or non-matching.  A mask is 
        /// a combination of a memory bank and a sequence of bits that will be matched at 
        /// the specified offset within the chosen memory bank.  
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SelectMask
        {
            /// <summary>
            /// The memory bank that contains the bits that will be compared 
            /// against the bit pattern specified in mask.  For a tag mask, 
            /// <see cref="MemoryBank.RESERVED"/> is not a valid value. 
            /// </summary>
            public MemoryBank bank = MemoryBank.UNKNOWN;
            /// <summary>
            /// The offset, in bits, from the start of the memory bank, of the 
            /// first bit that will be matched against the mask.  If offset falls 
            /// beyond the end of the memory bank, the tag is considered 
            /// non-matching. 
            /// </summary>
            public UInt32 offset = 0;
            /// <summary>
            /// The number of bits in the mask.  A length of zero will cause all 
            /// tags to match.  If (offset+count) falls beyond the end of 
            /// the memory bank, the tag is considered non-matching.  Valid 
            /// values are 0 to 255, inclusive. 
            /// </summary>
            public UInt32 count = 0;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match 
            /// </summary>
//            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            protected Byte[] m_mask = new Byte[32];
            /// <summary>
            /// Constructor
            /// </summary>
            public SelectMask()
            {
                // NOP
            }
            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="bank">Memory bank</param>
            /// <param name="offset">offset in bit</param>
            /// <param name="count">count in bit</param>
            /// <param name="epc">epc mask</param>
            public SelectMask(MemoryBank bank, UInt32 offset, UInt32 count, Byte[] epc)
            {
                this.bank = bank;
                this.count = count;
                this.mask = epc == null ? null : (byte[])epc.Clone();
                this.offset = offset;
            }
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match – i.e., the most significant bit of the bit 
            /// array appears in the most-significant bit (i.e., bit 7) of the first 
            /// byte of the buffer (i.e., mask[0]).  All bits beyond count are 
            /// ignored.  For example, if the application wished to find tags 
            /// with the following 12 bits 1000.1100.1101, starting at offset 
            /// 16 in the EPC memory bank, then the fields would be set as 
            /// follows: 
            /// bank    = RFID_18K6C_MEMORY_BANK_EPC 
            /// offset  = 16 
            /// count   = 12 
            /// mask[0] = 0x8C (1000.1100) 
            /// mask[1] = 0xD? (1101.????) 
            /// </summary>
            public Byte[] mask
            {
                get { return m_mask; }
                set
                {
                    if (value != null)
                    {
                        if (value.Length > 0 && value.Length <= 32)
                        {
                            Array.Copy(value, m_mask, value.Length);
                        }
                        else if (value.Length > 32)
                        {
                            Array.Copy(value, m_mask, 32);
                        }
                    }
                }
            }
        };


        // Auto-marshaling ok
        /// <summary>
        /// The partitioning of tags into disjoint groups is accomplished by applying actions 
        /// to the tags that match and/or do not match the specified mask.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SelectAction
        {
            /// <summary>
            /// Specifies what flag, selected (i.e., SL) or one of the four inventory 
            /// flags (i.e., S0, S1, S2, or S3), will be modified by the action. 
            /// </summary>
            public Target target = Target.UNKNOWN;
            /// <summary>
            /// Specifies the action that will be applied to the tag populations (i.e, the 
            /// matching and non-matching tags). 
            /// </summary>
            public CSLibrary.Constants.Action action = CSLibrary.Constants.Action.UNKNOWN;
            /// <summary>
            /// Specifies if, during singulation, a tag will respond to a subsequent 
            /// inventory operation with its entire Electronic Product Code (EPC) or 
            /// will only respond with the portion of the EPC that immediately follows 
            /// the bit pattern (as long as the bit pattern falls within the EPC – if the 
            /// bit pattern does not fall within the tag’s EPC, the tag ignores the tag 
            /// partitioning operation2).  If this parameter is non-zero: 
            /// <para>      bank must be <see cref="MemoryBank.EPC"/>. </para>
            /// <para>      target must be <see cref="Target.SELECTED"/>. </para>
            /// This action must correspond to the last tag select operation issued 
            /// before the inventory operation or access command. 
            /// </summary>
            public Int32 enableTruncate = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public SelectAction()
            {
                // NOP
            }
            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="target"><see cref="Target"/></param>
            /// <param name="action"><see cref="CSLibrary.Constants.Action"/></param>
            /// <param name="enableTruncate"></param>
            public SelectAction(Target target, CSLibrary.Constants.Action action, Int32 enableTruncate)
            {
                this.action = action;
                this.enableTruncate = enableTruncate;
                this.target = target;
            }
        };


        // Auto-marshaling ok
        /// <summary>
        /// Together, the selection mask and selection action form a single selection 
        /// criterion.  
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SelectCriterion
        {
            /// <summary>
            /// The mask that will be applied to a tag to determine if it is matching or non-
            /// matching. 
            /// </summary>
            public SelectMask mask = new SelectMask();
            /// <summary>
            /// The action that is to be applied to matching and/or non-matching tags (as 
            /// defined by the mask field). 
            /// </summary>
            public SelectAction action = new SelectAction();
            /// <summary>
            /// Constructor
            /// </summary>
            public SelectCriterion()
            {
                // NOP
            }
        };


        // No auto-marshaling due to variable size array - we could
        // constrain to size 8 (?) since max MAC allows...
        /// <summary>
        /// Tag-selection criteria 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SelectCriteria
        {
            /// <summary>
            /// The number of selection criteria in the array pointed to by the pCriteria 
            /// field.  This field must be greater than or equal to zero and less than or 
            /// equal to the maximum number of selection criteria as specified in [MAC-
            /// EDS].  Calling RFID_18K6CSetSelectCriteria with this field set to 
            /// zero Results in disabling all selection criteria (i.e., even if the 
            /// <see cref="SelectFlags.SELECT"/> flag is provided to the appropriate 
            /// RFID_18K6CTag* function, no selects will be issued).  If this field is zero, 
            /// pCriteria may be NULL. 
            /// </summary>
            public UInt32 countCriteria = 0;
            /// <summary>
            /// A pointer to an array, containing countCriteria entries, of selection 
            /// criterion structures that are to be applied sequentially, beginning with 
            /// pCriteria[0], to the tag population.  If this field is NULL, 
            /// countCriteria must be zero. 
            /// </summary>
            public SelectCriterion[] pCriteria = new SelectCriterion[0];
            /// <summary>
            /// Constructor
            /// </summary>
            public SelectCriteria()
            {
                // The count criteria field must be set manually since the user
                // is allowed to have a pCriteria array of len N and count less
                // than N...
            }
        };



        // Auto-marshaling ok since fixed size array
        /// <summary>
        /// The post-singulation match mask is used to specify a bit pattern of up to 496 bits 
        /// that is used to match against the EPC backscattered by a tag during singulation 
        /// to determine if a tag is matching or non-matching.  
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SingulationMask
        {
            /// <summary>
            /// The offset in bits, from the start of the Electronic Product 
            /// Code (EPC), of the first bit that will be matched against the 
            /// mask.  If offset falls beyond the end of EPC, the tag is 
            /// considered non-matching. 
            /// </summary>
            public UInt32 offset = 0;
            /// <summary>
            /// The number of bits in the mask.  A length of zero will cause 
            /// all tags to match.  If (offset+count) falls beyond the end 
            /// of the EPC, the tag is considered non-matching.  Valid 
            /// values are 0 to 496, inclusive. 
            /// </summary>
            public UInt32 count = 0;
            /// <summary>
            /// 
            /// </summary>
//            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 62)]
            protected Byte[] m_mask = new Byte[62];
            /// <summary>
            /// Constructor
            /// </summary>
            public SingulationMask()
            {
                // NOP
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="offset"></param>
            /// <param name="count"></param>
            /// <param name="mask"></param>
            public SingulationMask(UInt32 offset, UInt32 count, Byte[] mask)
            {
                this.offset = offset;
                this.count = count;
                this.mask = (byte[])mask.Clone();
            }
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match – i.e., the most significant bit of the 
            /// bit array appears in the most-significant bit (i.e., bit 7) of the 
            /// first byte of the buffer (i.e., mask[0]).  All bits beyond count 
            /// are ignored.  For example, if the application wished to find 
            /// tags with the following 16 bits 1011.1111.1010.0101, 
            /// starting at offset 20 in the Electronic Product Code, then the 
            /// fields would be set as follows: 
            /// offset  = 20 
            /// count   = 16 
            /// mask[0] = 0xBF (1011.1111) 
            /// mask[1] = 0xA5 (1010.0101) 
            /// </summary>
            public Byte[] mask
            {
                get { return m_mask; }
                set 
                {
                    if (value != null)
                    {
                        if (value.Length > 0 && value.Length <= 62)
                        {
                            Array.Copy(value, m_mask, value.Length);
                        }
                        else if (value.Length > 62)
                        {
                            Array.Copy(value, m_mask, 62);
                        }
                    }
                }
            }
        };



        // Auto-marshaling ok
        /// <summary>
        /// Together, the selection mask and an indication if the application is interested in 
        /// matching or non-matching tags form a single post-singulation match criterion.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SingulationCriterion
        {
            /// <summary>
            /// Determines if the associated tag-protocol operation will be 
            /// applied to tags that match the mask or not.  A non-zero 
            /// value indicates that the tag-protocol operation should be 
            /// applied to tags that match the mask.  A value of zero 
            /// indicates that the tag-protocol operation should be applied 
            /// to tags that do not match the mask. 
            /// </summary>
            public UInt32 match = 0;
            /// <summary>
            /// The mask that will be applied to the tag’s Electronic Product 
            /// Code to determine if it is matching or non-matching. 
            /// </summary>
            public SingulationMask mask = new SingulationMask();
            /// <summary>
            /// Constructor
            /// </summary>
            public SingulationCriterion()
            {
                // NOP
            }
        };



        // No auto-marshaling due to variable size array - we could
        // constrain to size 8 (?) since max MAC allows...
        /// <summary>
        /// Post-singulation match criteria can be grouped together using the following: 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SingulationCriteria
        {
            /// <summary>
            /// The number of singulation criteria in the array pointed to by the 
            /// pCriteria field.  This field must be greater than or equal to zero and 
            /// less than or equal to the maximum number of post-singulation criteria as 
            /// specified in [MAC-EDS].  Calling 
            /// <see cref="CSLibrary.HighLevelInterface.SetPostMatchCriteria(CSLibrary.Structures.SingulationCriterion[])"/> with this field set to zero Results 
            /// in disabling all post-singulation criteria (i.e., even if the 
            /// <see cref="SelectFlags.POST_MATCH"/> flag is provided to the appropriate 
            /// RFID_18K6CTag* function, no post-singulation matching will be 
            /// performed – the Result is that all tags are considered matching).  If this 
            /// field is zero, pCriteria may be NULL. 
            /// </summary>
            public UInt32 countCriteria = 0;
            /// <summary>
            ///  A pointer to an array, containing countCriteria entries, of post-
            ///  singulation criterion structures that are to be applied sequentially, 
            ///  beginning with pCriteria[0], to singulated tags.  This must not be 
            ///  NULL.  If this field is NULL, countCriteria must be zero. 
            /// </summary>
            public SingulationCriterion[] pCriteria = new SingulationCriterion[0];
            /// <summary>
            /// Constructor
            /// </summary>
            public SingulationCriteria()
            {
                // NOP
            }
        };



        // Auto-marshaling ok
        /// <summary>
        ///  A class that specifies the tag group that will 
        /// have a subsequent tag-protocol operation applied 
        /// to it.  This parameter must not be NULL. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class TagGroup
        {
            /// <summary>
            /// Specifies the state of the selected (SL) flag for tags that will have 
            /// the operation applied to them. 
            /// </summary>
            public Selected selected = Selected.UNKNOWN;
            /// <summary>
            /// Specifies which inventory session flag (i.e., S0, S1, S2, or S3) 
            /// will be matched against the inventory state specified by target. 
            /// </summary>
            public Session session = Session.UNKNOWN;
            /// <summary>
            /// Specifies the state of the inventory session flag (i.e., A or B), 
            /// specified by session, for tags that will have the operation 
            /// applied to them. 
            /// </summary>
            public SessionTarget target = SessionTarget.UNKNOWN;
            /// <summary>
            /// Constructor
            /// </summary>
            public TagGroup()
            {
                // NOP
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="selected"></param>
            /// <param name="session"></param>
            /// <param name="target"></param>
            public TagGroup(Selected selected, Session session, SessionTarget target)
            {
                this.selected = selected;
                this.session = session;
                this.target = target;
            }
        };


        /// <summary>
        /// SingulationAlgorithmParms
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class SingulationAlgorithmParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length_ = 4;
            /// <summary>
            /// Constructor
            /// </summary>
            public SingulationAlgorithmParms()
            {
                // NOP - child classes MUST set length_ to 
                //       an appropriate value...
            }
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length
            {
                get { return this.length_; }
            }
        }



        // Auto-marshaling ok 
        /// <summary>
        /// The  parameters  for  the  fixed-Q  algorithm,  MAC  singulation  algorithm  0,  (i.e., 
        /// RFID_18K6C_SINGULATION_ALGORITHM_FIXEDQ)
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class FixedQParms
            :
            SingulationAlgorithmParms
        {
            /// <summary>
            /// The Q value to use.  Valid values are 0-15, inclusive. 
            /// </summary>
            public UInt32 qValue = 0;
            /// <summary>
            /// Specifies the number of times to try another execution 
            /// of the singulation algorithm for the specified 
            /// session/target before either toggling the target (if 
            /// toggleTarget is non-zero) or terminating the 
            /// inventory/tag access operation.  Valid values are 0-
            /// 255, inclusive. 
            /// </summary>
            public UInt32 retryCount = 0;
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for 
            /// the specified target (i.e., A or B), if the target should be toggled 
            /// (i.e., A to B or B to A) and another inventory cycle run.  A non-
            /// zero value indicates that the target should be toggled.  A zero 
            /// value indicates that the target should not be toggled.  Note that 
            /// if the target is toggled, retryCount and 
            /// repeatUntilNoTags will also apply to the new target. 
            /// </summary>
            public UInt32 toggleTarget = 0;
            /// <summary>
            /// A flag that indicates whether or not the singulation 
            /// algorithm should continue performing inventory rounds 
            /// until no tags are singulated.  A non-zero value indicates 
            /// that, for each execution of the singulation algorithm, 
            /// inventory rounds should be performed until no tags are 
            /// singulated.  A zero value indicates that a single 
            /// inventory round should be performed for each 
            /// execution of the singulation algorithm. 
            /// </summary>
            public UInt32 repeatUntilNoTags = 0;
            /// <summary>
            /// Constructor
            /// </summary>
            public FixedQParms()
            {
                base.length_ = (base.length_ + 16);
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="qValue"></param>
            /// <param name="retryCount"></param>
            /// <param name="toggleTarget"></param>
            /// <param name="repeatUntilNoTags"></param>
            public FixedQParms(uint qValue, uint retryCount, uint toggleTarget, uint repeatUntilNoTags)
            {
                this.qValue = qValue;
                this.retryCount = retryCount;
                this.toggleTarget = toggleTarget;
                this.repeatUntilNoTags = repeatUntilNoTags;
                base.length_ = (base.length_ + 16);
            }
        }
#if DynamicQ3
        // Auto-marshaling ok 
        /// <summary>
        /// The parameters for the dynamic-Q algorithm, MAC singulation algorithm 1, (i.e., 
        /// RFID_18K6C_SINGULATION_ALGORITHM_DYNAMICQ)
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class DynamicQ_1Parms
            :
            SingulationAlgorithmParms
        {
            /// <summary>
            /// The starting Q value to use.  Valid values are 0-15, inclusive.  
            /// startQValue must be greater than or equal to minQValue and 
            /// less than or equal to maxQValue. 
            /// </summary>
            public UInt32 startQValue = 0;
            /// <summary>
            /// The minimum Q value to use.  Valid values are 0-15, inclusive.  
            /// minQValue must be less than or equal to startQValue and 
            /// maxQValue. 
            /// </summary>
            public UInt32 minQValue = 0;
            /// <summary>
            /// The maximum Q value to use.  Valid values are 0-15, inclusive.  
            /// maxQValue must be greater than or equal to startQValue and 
            /// minQValue. 
            /// </summary>
            public UInt32 maxQValue = 0;
            /// <summary>
            /// Specifies the number of times to try another execution of 
            /// the singulation algorithm for the specified session/target 
            /// before either toggling the target (if toggleTarget is non-
            /// zero) or terminating the inventory/tag access operation.  
            /// Valid values are 0-255, inclusive. 
            /// </summary>
            public UInt32 retryCount = 0;
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for the 
            /// specified target (i.e., A or B), if the target should be toggled (i.e., A 
            /// to B or B to A) and another inventory cycle run.  A non-zero value 
            /// indicates that the target should be toggled.  A zero value indicates 
            /// that the target should not be toggled.  Note that if the target is 
            /// toggled, retryCount and maxQueryRepCount will also apply to the 
            /// new target. 
            /// </summary>
            public UInt32 toggleTarget = 0;

            /// <summary>
            /// Maximum number of query reps that will be issued in a single round.
            /// </summary>
            public UInt32 maxReps = 0;

            /// <summary>
            /// Q adjustment high threshold
            /// </summary>
            public UInt32 HighThres = 0;

            /// <summary>
            /// Q adjustment low threshold
            /// </summary>
            public UInt32 LowThres = 0;
        }

        // Auto-marshaling ok 
        /// <summary>
        /// The parameters for the dynamic-Q algorithm, MAC singulation algorithm 2, (i.e., 
        /// RFID_18K6C_SINGULATION_ALGORITHM_DYNAMICQ)
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class DynamicQ_2Parms
            :
            SingulationAlgorithmParms
        {
            /// <summary>
            /// The starting Q value to use.  Valid values are 0-15, inclusive.  
            /// startQValue must be greater than or equal to minQValue and 
            /// less than or equal to maxQValue. 
            /// </summary>
            public UInt32 startQValue = 0;
            /// <summary>
            /// The minimum Q value to use.  Valid values are 0-15, inclusive.  
            /// minQValue must be less than or equal to startQValue and 
            /// maxQValue. 
            /// </summary>
            public UInt32 minQValue = 0;
            /// <summary>
            /// The maximum Q value to use.  Valid values are 0-15, inclusive.  
            /// maxQValue must be greater than or equal to startQValue and 
            /// minQValue. 
            /// </summary>
            public UInt32 maxQValue = 0;
            /// <summary>
            /// Specifies the number of times to try another execution of 
            /// the singulation algorithm for the specified session/target 
            /// before either toggling the target (if toggleTarget is non-
            /// zero) or terminating the inventory/tag access operation.  
            /// Valid values are 0-255, inclusive. 
            /// </summary>
            public UInt32 retryCount = 0;
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for the 
            /// specified target (i.e., A or B), if the target should be toggled (i.e., A 
            /// to B or B to A) and another inventory cycle run.  A non-zero value 
            /// indicates that the target should be toggled.  A zero value indicates 
            /// that the target should not be toggled.  Note that if the target is 
            /// toggled, retryCount and maxQueryRepCount will also apply to the 
            /// new target. 
            /// </summary>
            public UInt32 toggleTarget = 0;

            /// <summary>
            /// Maximum number of query reps that will be issued in a single round.
            /// </summary>
            public UInt32 maxReps = 0;

            /// <summary>
            /// Q adjustment high threshold
            /// </summary>
            public UInt32 HighThres = 0;

            /// <summary>
            /// Q adjustment low threshold
            /// </summary>
            public UInt32 LowThres = 0;
        }
#endif

        // Auto-marshaling ok 
        /// <summary>
        /// The parameters for the dynamic-Q algorithm, MAC singulation algorithm 3, (i.e., 
        /// RFID_18K6C_SINGULATION_ALGORITHM_DYNAMICQ)
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class DynamicQParms
            :
            SingulationAlgorithmParms
        {
            /// <summary>
            /// The starting Q value to use.  Valid values are 0-15, inclusive.  
            /// startQValue must be greater than or equal to minQValue and 
            /// less than or equal to maxQValue. 
            /// </summary>
            public UInt32 startQValue = 0;
            /// <summary>
            /// The minimum Q value to use.  Valid values are 0-15, inclusive.  
            /// minQValue must be less than or equal to startQValue and 
            /// maxQValue. 
            /// </summary>
            public UInt32 minQValue = 0;
            /// <summary>
            /// The maximum Q value to use.  Valid values are 0-15, inclusive.  
            /// maxQValue must be greater than or equal to startQValue and 
            /// minQValue. 
            /// </summary>
            public UInt32 maxQValue = 0;
            /// <summary>
            /// Specifies the number of times to try another execution of 
            /// the singulation algorithm for the specified session/target 
            /// before either toggling the target (if toggleTarget is non-
            /// zero) or terminating the inventory/tag access operation.  
            /// Valid values are 0-255, inclusive. 
            /// </summary>
            public UInt32 retryCount = 0;
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for the 
            /// specified target (i.e., A or B), if the target should be toggled (i.e., A 
            /// to B or B to A) and another inventory cycle run.  A non-zero value 
            /// indicates that the target should be toggled.  A zero value indicates 
            /// that the target should not be toggled.  Note that if the target is 
            /// toggled, retryCount and maxQueryRepCount will also apply to the 
            /// new target. 
            /// </summary>
            public UInt32 toggleTarget = 0;
            /// <summary>
            /// The multiplier, specified in units of fourths (i.e., 0.25), that will be 
            /// applied to the Q-adjustment threshold as part of the dynamic-Q 
            /// algorithm.  For example, a value of 7 represents a multiplier of 
            /// 1.75.  See [MAC-EDS] for specifics on how the Q-adjustment 
            /// threshold is used in the dynamic Q algorithm.  Valid values are 0-
            /// 255, inclusive. 
            /// </summary>
            public UInt32 thresholdMultiplier = 0;
        }

        // NO Auto-marshaling
        /// <summary>
        /// Once the tag population has been partitioned into disjoint groups, a subsequent 
        /// tag-protocol operation (i.e., an inventory operation or access command) is then 
        /// applied to one of the tag groups.  A tag group is specified using the following: 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class QueryParms
        {
            /// <summary>
            /// Once the tag population has been partitioned into disjoint groups, a subsequent 
            /// tag-protocol operation (i.e., an inventory operation or access command) is then 
            /// applied to one of the tag groups.  A tag group is specified using the following: 
            /// </summary>
            public TagGroup tagGroup = new TagGroup();
            /// <summary>
            /// Based upon usage scenarios, different singulation algorithms (i.e., Q-adjustment, 
            /// etc.) may be desired.  This document simply documents the mechanisms by 
            /// which an application can choose and configure singulation algorithms.  
            /// </summary>
            public SingulationAlgorithmParms singulationParms = new SingulationAlgorithmParms();
            /// <summary>
            /// Constructor
            /// </summary>
            public QueryParms()
            {
                // NOP
            }
        }

        // Auto-marshaling ok
        /// <summary>
        /// All tag-protocol operation functions share a common set of parameters.  These 
        /// parameters are gathered in one place for convenience. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct CommonParms
        {
            /// <summary>
            /// The maximum number of tags to which the tag-protocol operation 
            /// will be applied.  If this number is zero, then the operation is applied 
            /// to all tags that match the selection, and optionally post-singulation, 
            /// match criteria (within the constraints of the antenna-port dwell time 
            /// and inventory cycle count – see ).  If this number is non-zero, the 
            /// antenna-port dwell-time and inventory-cycle-count constraints still 
            /// apply, however the operation will be prematurely terminated if the 
            /// maximum number of tags have the tag-protocol operation applied to 
            /// them.  For version 1.0, this field may have a maximum value 1. 
            /// </summary>
            public UInt32 tagStopCount;
            /// <summary>
            /// a callback function that the library will invoke with the 
            /// Resulting operation-response packets.  This field must not be NULL. 
            /// </summary>
            public IntPtr callback;
            /// <summary>
            /// An application-defined value that is passed through unmodified to 
            /// the application-specified callback function. 
            /// </summary>
            public IntPtr context;
            /// <summary>
            /// A pointer to a 32-bit integer that upon return will contain the return 
            /// code from the last call to the application-supplied callback function.  
            /// If the application does not desire this information, this field may be 
            /// NULL. 
            /// </summary>
            public IntPtr callbackCode;
        }


        // Auto-marshaling not ok ( see common parms ) 
        /// <summary>
        /// An inventory operation allows the application to gather the EPCs (or part of the 
        /// EPCs if the tag-select action specified to truncate the response) for all tags of 
        /// interest.  The data returned from the inventory operation will adhere to the format 
        /// specified in [REC]. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct InventoryParms
        {
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public InventoryParms(bool initial)
            {
                this.length = (UInt32)Marshal.SizeOf(typeof(InventoryParms));
                this.common = new CommonParms();
            }
        }

        // Auto-marshaling ok 
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct ReadCmdParms
        {
            /// <summary>
            /// The length of this structure.
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to read.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank;
            /// <summary>
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to read from the specified memory 
            /// bank. 
            /// </summary>
            public UInt16 offset;
            /// <summary>
            /// The number of 16-bit words to read.  If this value is zero and 
            /// bank is MemoryBank.EPC, the read will return the 
            /// contents of the tag's EPC memory starting at the 16-bit word 
            /// specified by offset through the end of the EPC.  If this value is 
            /// zero and bank is not MemoryBank.EPC, the read 
            /// will return, for the tag's chosen memory bank, data starting from 
            /// the 16-bit word specified by offset to the end of the memory 
            /// bank.  If this value is non-zero, it must be in the range 1 to 255, 
            /// inclusive. 
            /// </summary>
            public UInt16 count;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public ReadCmdParms(bool initial)
            {
                this.length = (UInt32)Marshal.SizeOf(typeof(ReadCmdParms));
                this.bank = MemoryBank.UNKNOWN;
                this.offset = 0;
                this.count = 0;
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct Read2BandsCmdParms
        {
            /// <summary>
            /// The length of this structure.
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to read.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank1;
            /// <summary>
            /// The memory bank from which to read.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank2;
            /// <summary>
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to read from the specified memory 
            /// bank. 
            /// </summary>
            public UInt16 offset1;
            /// <summary>
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to read from the specified memory 
            /// bank. 
            /// </summary>
            public UInt16 offset2;
            /// <summary>
            /// 
            /// The number of 16-bit words to read.  If this value is zero and 
            /// bank is MemoryBank.EPC, the read will return the 
            /// contents of the tag's EPC memory starting at the 16-bit word 
            /// specified by offset through the end of the EPC.  If this value is 
            /// zero and bank is not MemoryBank.EPC, the read 
            /// will return, for the tag's chosen memory bank, data starting from 
            /// the 16-bit word specified by offset to the end of the memory 
            /// bank.  If this value is non-zero, it must be in the range 1 to 255, 
            /// inclusive. 
            /// </summary>
            public UInt16 count1;
            /// <summary>
            /// 
            /// The number of 16-bit words to read.  If this value is zero and 
            /// bank is MemoryBank.EPC, the read will return the 
            /// contents of the tag's EPC memory starting at the 16-bit word 
            /// specified by offset through the end of the EPC.  If this value is 
            /// zero and bank is not MemoryBank.EPC, the read 
            /// will return, for the tag's chosen memory bank, data starting from 
            /// the 16-bit word specified by offset to the end of the memory 
            /// bank.  If this value is non-zero, it must be in the range 1 to 255, 
            /// inclusive. 
            /// </summary>
            public UInt16 count2;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public Read2BandsCmdParms(bool initial)
            {
                this.length = (UInt32)Marshal.SizeOf(typeof(Read2BandsCmdParms));
                this.bank1 = MemoryBank.UNKNOWN;
                this.offset1 = 0;
                this.count1 = 0;
                this.bank2 = MemoryBank.UNKNOWN;
                this.offset2 = 0;
                this.count2 = 0;
            }
        }
        
        // Auto-marshaling not ok ( see common parms ) 
        /// <summary>
        /// Reading tag data should not be confused with performing an inventory.  
        /// Whereas an inventory is restricted to returning all or part of the tag's EPC data, a 
        /// read operation can be used to read one or more 16-bit words from any of a tag's 
        /// memory banks.  While a read may be used to retrieve a set of tag EPC data, if 
        /// the EPC is the only desired data, then at the low-level tag access, performing an 
        /// inventory operation is more efficient.  The data returned from the tag read will 
        /// adhere to the format specified in [REC]. 
        /// </summary>
        [StructLayout( LayoutKind.Sequential )]
        struct ReadParms
        {
            /// <summary>
            /// ReadParms length
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// 
            /// </summary>
            public ReadCmdParms readCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no 
            /// access password. 
            /// </summary>
            public UInt32 accessPassword;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public ReadParms(bool initial)
            {
                this.accessPassword = 0x0;
                this.common = new CommonParms();
                this.length = (UInt32)Marshal.SizeOf(typeof(ReadParms));
                this.readCmdParms = new ReadCmdParms(initial);
            }
        }




        // No auto-marshaling ~ contains variable sized array
        /// <summary>
        /// Write, beginning at the specified 16-bit offset, a contiguous set of one or 
        /// more 16-bit words to one of the tag's memory banks. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct WriteSequentialParms
            : IDisposable
        {
            /// <summary>
            /// The length of this structure.
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank;
            /// <summary>
            /// The number of 16-bit words to be written to the tag's specified 
            /// memory bank.  For version 1.1 of the RFID Reader Library, this 
            /// parameter must contain a value between 1 and 8, inclusive. 
            /// </summary>
            public UInt16 count;
            /// <summary>
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to write in the specified memory bank. 
            /// </summary>
            public UInt16 offset;
             /// <summary>
            /// Reserved for future use.  Set to zero. 
            /// </summary>
            public UInt32 reserved;
            /// <summary>
            /// A buffer of count 16-bit values to be written 
            /// sequentially to the tag's specified memory bank.  The high-order 
            /// byte of pData[n] will be written to the tag's memory-bank byte at 
            /// 16-bit offset (offset+n).  The low-order byte will be written to the 
            /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
            /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
            /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
            /// written to it.  This field must not be NULL. 
            /// </summary>
            public IntPtr pData;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="bnk">
            ///  The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </param>
            /// <param name="count">
            /// The number of 16-bit words to be written to the tag's specified 
            /// memory bank.  For version 1.1 of the RFID Reader Library, this 
            /// parameter must contain a value between 1 and 8, inclusive. 
            /// </param>
            /// <param name="offset">
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to write in the specified memory bank. 
            /// </param>
            /// <param name="data">
            /// A buffer of count 16-bit values to be written 
            /// sequentially to the tag's specified memory bank.  The high-order 
            /// byte of pData[n] will be written to the tag's memory-bank byte at 
            /// 16-bit offset (offset+n).  The low-order byte will be written to the 
            /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
            /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
            /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
            /// written to it.  This field must not be NULL. 
            /// </param>
            public WriteSequentialParms(MemoryBank bnk, UInt16 count, UInt16 offset, UInt16[] data)
            {
                if (data == null)
                    throw new ReaderException(Result.INVALID_PARAMETER);
                this.length = (UInt32)(Marshal.SizeOf(typeof(WriteSequentialParms)));
                this.bank = bnk;
                this.count = count;
                this.offset = offset;
                this.reserved = 0;
                try
                {
                    this.pData = Marshal.AllocHGlobal(sizeof(UInt16) * data.Length);
                    Marshal.Copy((Int16[])data.Clone(), 0, this.pData, data.Length);
                }
                catch (OutOfMemoryException)
                {
                    throw new ReaderException(Result.OUT_OF_MEMORY);
                }
            }
            /// <summary>
            /// Release all resources
            /// </summary>
            public void Dispose()
            {
                if (this.pData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.pData);
                    this.pData = IntPtr.Zero;
                }
            }
        }



        // No auto-marshaling ~ contains variable sized array(s)
        /// <summary>
        /// Write  one  or  more  16-bit  words  to  potentially  non-contiguous  16-bit 
        /// offsets to one of the tag's memory banks.  This can be thought of as a 
        /// random-access write to a single tag memory bank. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct WriteRandomParms
            : IDisposable
        {
            /// <summary>
            /// The length of this structure.
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank;
            /// <summary>
            /// The number of 16-bit words to be written to the tag's specified 
            /// memory bank.  For version 1.1 of the RFID Reader Library, this 
            /// parameter must contain a value between 1 and 8, inclusive. 
            /// </summary>
            public UInt16 count;
            /// <summary>
            /// Reserved for future use.  Set to zero. 
            /// </summary>
            public UInt16 reserved;
            /// <summary>
            /// An array of count 16-bit values that specify 16-bit tag-
            /// memory-bank offsets, with zero being the first 16-bit word in the 
            /// memory bank, where the corresponding 16-bit words in the pData 
            /// array will be written.  i.e., the 16-bit word in pData[n] will be written to 
            /// the 16-bit tag-memory-bank offset contained in pOffset[n].  This 
            /// field must not be NULL. 
            /// </summary>
            public IntPtr pOffset;
            /// <summary>
            /// A buffer of count 16-bit values to be written to the tag's 
            /// specified memory bank.  The high-order byte of pData[n] will be 
            /// written to the tag's memory-bank byte at pOffset[n].  The low-order 
            /// byte will be written to the next byte.  For example, if pOffset[n] is 2 
            /// and pData[n] is 0x1122, then the tag-memory byte at 16-bit offset 2 
            /// (byte offset 4) will have 0x11 written to it and the next byte (byte offset 
            /// 5) will have 0x22 written to it.  This field must not be NULL. 
            /// </summary>
            public IntPtr pData;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="bnk">
            ///  The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </param>
            /// <param name="count">
            /// The number of 16-bit words to be written to the tag's specified 
            /// memory bank.  For version 1.1 of the RFID Reader Library, this 
            /// parameter must contain a value between 1 and 8, inclusive. 
            /// </param>
            /// <param name="offset">
            /// The offset of the first 16-bit word, where zero is the first 16-bit 
            /// word in the memory bank, to write in the specified memory bank. 
            /// </param>
            /// <param name="data">
            /// A buffer of count 16-bit values to be written 
            /// sequentially to the tag's specified memory bank.  The high-order 
            /// byte of pData[n] will be written to the tag's memory-bank byte at 
            /// 16-bit offset (offset+n).  The low-order byte will be written to the 
            /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
            /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
            /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
            /// written to it.  This field must not be NULL. 
            /// </param>
            public WriteRandomParms(MemoryBank bnk, UInt16 count, UInt16[] offset, UInt16[] data)
            {
                if (data == null || offset == null)
                    throw new ReaderException(Result.INVALID_PARAMETER);
                this.length = (UInt32)(Marshal.SizeOf(typeof(WriteRandomParms)));
                this.bank = bnk;
                this.count = count;
                this.reserved = 0;
                try
                {
                    this.pOffset = Marshal.AllocHGlobal(sizeof(UInt16) * offset.Length);
                    Marshal.Copy((Int16[])offset.Clone(), 0, this.pOffset, offset.Length);
                    this.pData = Marshal.AllocHGlobal(sizeof(UInt16) * data.Length);
                    Marshal.Copy((Int16[])data.Clone(), 0, this.pData, data.Length);
                }
                catch (OutOfMemoryException)
                {
                    throw new ReaderException(Result.OUT_OF_MEMORY);
                }
            }
            /// <summary>
            /// Release all resources
            /// </summary>
            public void Dispose()
            {
                if (this.pData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.pData);
                    this.pData = IntPtr.Zero;
                }
                if (this.pOffset != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.pOffset);
                    this.pOffset = IntPtr.Zero;
                }
            }
        }



        // No auto-marshaling ~ contains class w/ base representing
        // a union ( in the native lib )
        /// <summary>
        /// A tag-write command allows an application to write one or more 16-bit words to 
        /// the specified memory bank of the ISO 18000-6C tags of interest.  The data 
        /// returned from the tag write will adhere to the format specified in [REC].  An 
        /// application is given some flexibility in the execution of a write to an ISO 18000-
        /// 6C tag.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Explicit)]
        struct WriteParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            [FieldOffset(0)]
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            [FieldOffset(4)]
            public CommonParms common;
            /// <summary>
            /// The type of write that will be performed – i.e., sequential or random.  
            /// The value of this field determines which of the structures within 
            /// parameters contains the write parameters.  
            /// </summary>
            [FieldOffset(20)]
            public WriteType writeType;
            /// <summary>
            /// The write parameters.  This is a discriminated union, with writeType 
            /// as the discriminator (i.e., determines which structure within the union is 
            /// used). .  The following table lists the valid values for the writeType 
            /// field and their corresponding structures within this union. 
            /// </summary>
            [FieldOffset(24)]
            public WriteSequentialParms writeSequentialParms;
            /// <summary>
            /// The write parameters.  This is a discriminated union, with writeType 
            /// as the discriminator (i.e., determines which structure within the union is 
            /// used). .  The following table lists the valid values for the writeType 
            /// field and their corresponding structures within this union. 
            /// </summary>
            [FieldOffset(24)]
            public WriteRandomParms writeRandomParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            [FieldOffset(44)]
            public UInt32 accessPassword;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public WriteParms(bool initial)
            {
                this.accessPassword = 0x0;
                this.common = new CommonParms();
                this.length = (UInt32)Marshal.SizeOf(typeof(WriteParms));
                this.writeRandomParms = new WriteRandomParms();
                this.writeSequentialParms = new WriteSequentialParms();
                this.writeType = WriteType.UNKNOWN;
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct PermalockCmdParms : IDisposable
        {
            public UInt32 length;
            /// <summary>
            /// Permalock access flags
            /// </summary>
            public PermalockFlags flags;
            /// <summary>
            /// The number of 16-bit words to be written to the tag's specified memory 
            /// bank.  For version 1.2 of the RFID Reader Library, this parameter must 
            /// contain a value between 1 and 255, inclusive. 
            /// </summary>
            public UInt16 count;                //4
            /// <summary>
            /// The number of 16-bit words that will be written.  This field must be 
            /// between 1 and 32, inclusive.
            /// </summary>
            public UInt16 offset;
            /// <summary>
            /// A buffer of count 16-bit values to be written 
            /// sequentially to the tag's specified memory bank.  The high-order 
            /// byte of pData[n] will be written to the tag's memory-bank byte at 
            /// 16-bit offset (offset+n).  The low-order byte will be written to the 
            /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
            /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
            /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
            /// written to it.  This field must not be NULL. 
            /// </summary>
            public IntPtr pData;              

            /// <summary>
            /// Constructor
            /// </summary>
            public PermalockCmdParms(PermalockFlags flags, UInt16 count, UInt16 offset, UInt16[] data)
            {
                if (data == null)
                    throw new ReaderException(Result.INVALID_PARAMETER);
                this.length = (UInt32)(Marshal.SizeOf(typeof(PermalockCmdParms)));
                this.flags = flags;
                this.count = count;
                this.offset = offset;
                try
                {
                    this.pData = Marshal.AllocHGlobal(sizeof(UInt16) * data.Length);
                    Marshal.Copy((Int16[])data.Clone(), 0, this.pData, data.Length);
                }
                catch (OutOfMemoryException)
                {
                    throw new ReaderException(Result.OUT_OF_MEMORY);
                }
            }
            /// <summary>
            /// Release all resources
            /// </summary>
            public void Dispose()
            {
                if (this.pData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.pData);
                    this.pData = IntPtr.Zero;
                }
            }
        }

        // No auto-marshaling ~ contains class w/ base representing
        // a union ( in the native lib )
        /// <summary>
        /// A tag block-write command allows an application to write one or more 16-bit 
        /// words to the specified memory bank of the ISO 18000-6C tags of interest.  The 
        /// data returned from the block write will adhere to the format specified in [REC].  A 
        /// Write can be performed on a contiguous set of one or more 16-bit words to one 
        /// of the tag's memory banks. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct PermalockParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters. 
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// Permalock command parms
            /// </summary>
            public PermalockCmdParms permalockCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;
            public PermalockParms(bool initial)
            {
                this.accessPassword = 0;
                this.common = new CommonParms();
                this.length = (UInt32)Marshal.SizeOf(typeof(PermalockParms));
                this.permalockCmdParms = new PermalockCmdParms();
            }
        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct BlockWriteCmdParms : IDisposable
        {
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank;
            /// <summary>
            /// The number of 16-bit words that will be written.  This field must be 
            /// between 1 and 32, inclusive.
            /// </summary>
            public UInt16 offset;
            /// <summary>
            /// The number of 16-bit words to be written to the tag's specified memory 
            /// bank.  For version 1.2 of the RFID Reader Library, this parameter must 
            /// contain a value between 1 and 255, inclusive. 
            /// </summary>
            public UInt16 count;
            /// <summary>
            /// A buffer of count 16-bit values to be written 
            /// sequentially to the tag's specified memory bank.  The high-order 
            /// byte of pData[n] will be written to the tag's memory-bank byte at 
            /// 16-bit offset (offset+n).  The low-order byte will be written to the 
            /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
            /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
            /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
            /// written to it.  This field must not be NULL. 
            /// </summary>
            public IntPtr pData;
            /// <summary>
            /// Constructor
            /// </summary>
            public BlockWriteCmdParms(MemoryBank bnk, UInt16 count, UInt16 offset, UInt16[] data)
            {
                if (data == null)
                    throw new ReaderException(Result.INVALID_PARAMETER);
                this.length = (UInt32)(Marshal.SizeOf(typeof(BlockWriteCmdParms)));
                this.bank = bnk;
                this.count = count;
                this.offset = offset;
                try
                {
                    this.pData = Marshal.AllocHGlobal(sizeof(UInt16) * data.Length);
                    Marshal.Copy((Int16[])data.Clone(), 0, this.pData, data.Length);
                }
                catch (OutOfMemoryException)
                {
                    throw new ReaderException(Result.OUT_OF_MEMORY);
                }
            }
            /// <summary>
            /// Release all resources
            /// </summary>
            public void Dispose()
            {
                if (this.pData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.pData);
                    this.pData = IntPtr.Zero;
                }
            }
        }
        // No auto-marshaling ~ contains class w/ base representing
        // a union ( in the native lib )
        /// <summary>
        /// A tag block-write command allows an application to write one or more 16-bit 
        /// words to the specified memory bank of the ISO 18000-6C tags of interest.  The 
        /// data returned from the block write will adhere to the format specified in [REC].  A 
        /// Write can be performed on a contiguous set of one or more 16-bit words to one 
        /// of the tag's memory banks. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct BlockWriteParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters. 
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// The ISO 18000-6C block write specific command paramters
            /// </summary>
            public BlockWriteCmdParms blockWriteCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;
            public BlockWriteParms(bool initial)
            {
                this.accessPassword = 0x0;
                this.blockWriteCmdParms = new BlockWriteCmdParms();
                this.common = new CommonParms();
                this.length = (UInt32)Marshal.SizeOf(typeof(BlockWriteParms));
            }

        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct BlockEraseCmdParms
        {
            public UInt32 length;
            /// <summary>
            /// The memory bank from which to write.  Valid values are: 
            /// MemoryBank.RESERVED 
            /// MemoryBank.EPC  
            /// MemoryBank.TID 
            /// MemoryBank.USER 
            /// </summary>
            public MemoryBank bank;  //4
            /// <summary>
            /// The number of 16-bit words that will be written.  This field must be 
            /// between 1 and 32, inclusive.
            /// </summary>
            public UInt16 offset;
            /// <summary>
            /// The number of 16-bit words to be written to the tag's specified memory 
            /// bank.  For version 1.2 of the RFID Reader Library, this parameter must 
            /// contain a value between 1 and 255, inclusive. 
            /// </summary>
            public UInt16 count;                   //4

            /// <summary>
            /// Constructor
            /// </summary>
            public BlockEraseCmdParms(MemoryBank bnk, UInt16 count, UInt16 offset)
            {
                this.length = (UInt32)(Marshal.SizeOf(typeof(BlockEraseCmdParms)));
                this.bank = bnk;
                this.count = count;
                this.offset = offset;
            }
        }
        /// <summary>
        /// Tag Erase
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct BlockEraseParms
        {
            public UInt32 length;
            public CommonParms common;
            public BlockEraseCmdParms blockEraseCmdParms;
            public UInt32 accessPassword;
            public BlockEraseParms(bool initial)
            {
                this.accessPassword = 0;
                this.blockEraseCmdParms = new BlockEraseCmdParms();
                this.common = new CommonParms();
                this.length = (UInt32)(Marshal.SizeOf(typeof(BlockEraseParms)));
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct KillCmdParms
        {
            public UInt32 length;
            /// <summary>
            /// The kill password for the tags. 
            /// </summary>
            public UInt32 killPassword;
            /* Extended command	for Higs 3*/
            public ExtendedKillCommand exCommand;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public KillCmdParms(bool initial)
            {
                this.length = (UInt32)(Marshal.SizeOf(typeof(KillCmdParms)));
                this.killPassword = 0x0;
                this.exCommand = ExtendedKillCommand.UNKNOWN;
            }
        }
        /// <summary>
        /// A tag-kill command allows an application to kill (i.e., render inoperable) a set of 
        /// tags of interest.  The data returned from the tag kill will adhere to the format 
        /// specified in [REC]. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct KillParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// The ISO 18000-6C tag-kill specific command parameters
            /// </summary>
            public KillCmdParms killCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;
            /// <summary>
            /// Initial Constructor
            /// </summary>
            /// <param name="initial"></param>
            public KillParms(bool initial)
            {
                this.length = (UInt32)Marshal.SizeOf(typeof(KillParms));
                this.common = new CommonParms();
                this.killCmdParms = new KillCmdParms(initial);
                this.accessPassword = 0x0;
            }
        }


        // Auto-marshaling ok
        /// <summary>
        /// A structure that contains the access permissions to be set for the tag. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct TagPerm
        {
            /// <summary>
            /// The access permissions for the tag's kill password.  
            /// </summary>
            public Permission killPasswordPermissions;
            /// <summary>
            /// The access permissions for the tag's access password. 
            /// </summary>
            public Permission accessPasswordPermissions;
            /// <summary>
            /// The access permissions for the tag's EPC memory bank.  
            /// </summary>
            public Permission epcMemoryBankPermissions;
            /// <summary>
            /// The access permissions for the tag's TID memory bank.
            /// </summary>
            public Permission tidMemoryBankPermissions;
            /// <summary>
            /// The access permissions for the tag's user memory bank.
            /// </summary>
            public Permission userMemoryBankPermissions;
        }
        /// <summary>
        /// The ISO 18000-6C tag-lock operation lock specific command parameters.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct LockCmdParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            ///  A structure that contains the access permissions to be set for the tag. 
            /// </summary>
            public TagPerm permissions;

            public LockCmdParms(bool initial)
            {
                this.length = (UInt32)Marshal.SizeOf(typeof(LockCmdParms));
                this.permissions = new TagPerm();
            }
        }

        /// <summary>
        /// A  tag-permission  command  (aka,  tag  lock)  allows  the  application  to  set  the 
        /// access permissions of a tag. 
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct LockParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// The ISO 18000-6C tag-lock operation lock specific command parameters.
            /// </summary>
            public LockCmdParms lockCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;

            public LockParms(bool initial)
            {
                this.accessPassword = 0;
                this.common = new CommonParms();
                this.length = (UInt32)Marshal.SizeOf(typeof(LockParms));
                this.lockCmdParms = new LockCmdParms(initial);
            }
        }
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        struct EASCmdParms 
        {
            public UInt32 length;
            public bool enable;
            public EASCmdParms(bool initial)
            {
                this.length = (UInt32)(Marshal.SizeOf(typeof(EASCmdParms)));
                this.enable = false;
            }
        }

        /// <summary>
        /// G2X EAS
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct EASParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// Enable EAS 
            /// </summary>
            public EASCmdParms easCmdParms;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;
            public EASParms(bool initial)
            {
                this.length = (UInt32)(Marshal.SizeOf(typeof(EASParms)));
                this.common = new CommonParms();
                this.accessPassword = 0x0;
                this.easCmdParms = new EASCmdParms(initial);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct ReadProtectParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length;
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword;

            public ReadProtectParms(bool initial)
            {
                this.accessPassword = 0;
                this.common = new CommonParms();
                this.length = (UInt32)(Marshal.SizeOf(typeof(ReadProtectParms)));
            }

        }


        /// <summary>
        /// NonVolatileMemoryBlock - for future use
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public class NonVolatileMemoryBlock
        {
            /// <summary>
            /// 
            /// </summary>
            public UInt32 address = 0;
            /// <summary>
            /// 
            /// </summary>
            public UInt32 length = 0;
            /// <summary>
            /// 
            /// </summary>
            public IntPtr pData = IntPtr.Zero;
            /// <summary>
            /// 
            /// </summary>
            public UInt32 flags = 0;
            /// <summary>
            /// 
            /// </summary>
            public NonVolatileMemoryBlock()
            {
                // NOP
            }
        }
        #region WaterYu @ 16-Sept-2009

        [StructLayout(LayoutKind.Sequential)]
        class InternalTagInventoryParms
        {
            public readonly UInt32 length = (UInt32)(10 + IntPtr.Size);
            /// <summary>
            /// Flag - Zero or combination of  Select or Post-Match
            /// </summary>
            public SelectFlags flags = SelectFlags.UNKNOWN;
            /// <summary>
            /// <para>The maximum number of tags to which the tag-protocol operation will be </para>
            /// <para>applied.  If this number is zero, then the operation is applied to all </para>
            /// <para>tags that match the selection, and optionally post-singulation, match  </para>
            /// <para>criteria.  If this number is non-zero, the antenna-port dwell-time and </para>
            /// <para>inventory-round-count constraints still apply, however the operation   </para>
            /// <para>will be prematurely terminated if the maximum number of tags have the  </para>
            /// <para>tag-protocol operation applied to them.                                </para>
            /// </summary>
            public ushort tagStopCount = 0;
            /// <summary>
            /// callback pointer
            /// </summary>
//            public Native.TagSearchOneCallbackDelegate pCallback;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalTagInventoryParms()
            {
                // NOP
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        class InternalTagSearchOneParms
        {
            /// <summary>
            /// Structure size
            /// </summary>   
            public readonly UInt32 length = (UInt32)(8 + IntPtr.Size);
            /// <summary>
            /// 
            /// </summary>
            public Boolean avgRssi;
            /// <summary>
            /// 
            /// </summary>
//            public Native.TagSearchOneCallbackDelegate pCallback;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalTagSearchOneParms()
            {
                // NOP
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        class InternalTagRangingParms
        {
            /// <summary>
            /// Structure size
            /// </summary>   
            public readonly UInt32 length = (UInt32)(10 + IntPtr.Size);
            /// <summary>
            /// Flag - Zero or combination of  Select or Post-Match
            /// </summary>
            public SelectFlags flags = SelectFlags.UNKNOWN;
            /// <summary>
            /// <para>The maximum number of tags to which the tag-protocol operation will be </para>
            /// <para>applied.  If this number is zero, then the operation is applied to all </para>
            /// <para>tags that match the selection, and optionally post-singulation, match  </para>
            /// <para>criteria.  If this number is non-zero, the antenna-port dwell-time and </para>
            /// <para>inventory-round-count constraints still apply, however the operation   </para>
            /// <para>will be prematurely terminated if the maximum number of tags have the  </para>
            /// <para>tag-protocol operation applied to them.                                </para>
            /// </summary>
            public ushort tagStopCount = 0;
            /// <summary>
            /// callback pointer
            /// </summary>
//            public Native.TagRangingCallbackDelegate pCallback;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalTagRangingParms()
            {
                // NOP
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        class InternalTagReadProtectParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 4);
            /// <summary>
            /// The common tag-protocol operation parameters.
            /// </summary>
            public CommonParms common;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword = 0;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalTagReadProtectParms()
            {
                // NOP
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        class InternalCustCmdTagReadProtectParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            public UInt32 length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 4);
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword = 0;
            /// <summary>
            /// Retry count
            /// </summary>
            public UInt32 retry = 0;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalCustCmdTagReadProtectParms()
            {
                // NOP
            }
        }

        /// <summary>
        /// Selected tag parameters, configure this before any specific tag operation
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class TagSelectedParms
        {
            /// <summary>
            /// A mask that indicates current parameters enalbe or not.
            /// </summary>
            public SelectMaskFlags flags = SelectMaskFlags.DISABLE_ALL;
            /// <summary>
            /// memory bank
            /// </summary>
            public MemoryBank bank = MemoryBank.EPC;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match – i.e., the most significant bit of the 
            /// bit array appears in the most-significant bit (i.e., bit 7) of the 
            /// first byte of the buffer (i.e., mask[0]).  All bits beyond count 
            /// are ignored.  For example, if the application wished to find 
            /// tags with the following 16 bits 1011.1111.1010.0101, 
            /// starting at offset 20 in the Electronic Product Code, then the 
            /// fields would be set as follows: 
            /// offset  = 20 
            /// count   = 16 
            /// mask[0] = 0xBF (1011.1111) 
            /// mask[1] = 0xA5 (1010.0101) 
            /// </summary>
            public S_MASK epcMask = new S_MASK();
            /// <summary>
            /// epc mask length in bit, e.g. epc = 0x3000, length = 4 * 8 = 32 bits, epc = 0x300, length = 3 * 8 = 24 bits
            /// </summary>
            public UInt32 epcMaskLength = 0;
            /// <summary>
            /// epc mask offset in bit, note : if enable PC mask, this parameter is ignored.
            /// </summary>
            public UInt32 epcMaskOffset = 0;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match (use epcMask, if you select EPC)
            /// </summary>
            public byte [] Mask;
            /// <summary>
            /// epc mask length in bit (use epcMaskLength, if you select EPC)
            /// </summary>
            public UInt32 MaskLength = 0;
            /// <summary>
            /// epc mask offset in bit (use epcMaskOffset, if you select EPC)
            /// </summary>
            public UInt32 MaskOffset = 0;
            /// <summary>
            /// Qvalue
            /// </summary>
            public UInt32 Qvalue = 0;
            /// <summary>
            /// UCODE 7 Parallel Encoding
            /// </summary>
            public bool ParallelEncoding = false;
            /// <summary>
            /// constructor
            /// </summary>
            public TagSelectedParms()
            {
                // NOP
            }
        }
        /// <summary>
        /// Selected tag parameters, configure this before any specific tag operation
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class TagGeneralSelectedParms
        {
            /// <summary>
            /// A mask that indicates current parameters enalbe or not.
            /// </summary>
            public SelectMaskFlags flags = SelectMaskFlags.DISABLE_ALL;
            /// <summary>
            /// memory bank
            /// </summary>
            public MemoryBank bank = MemoryBank.EPC;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match – i.e., the most significant bit of the 
            /// bit array appears in the most-significant bit (i.e., bit 7) of the 
            /// first byte of the buffer (i.e., mask[0]).  All bits beyond count 
            /// are ignored.  For example, if the application wished to find 
            /// tags with the following 16 bits 1011.1111.1010.0101, 
            /// starting at offset 20 in the Electronic Product Code, then the 
            /// fields would be set as follows: 
            /// offset  = 20 
            /// count   = 16 
            /// mask[0] = 0xBF (1011.1111) 
            /// mask[1] = 0xA5 (1010.0101) 
            /// </summary>
            public S_MASK epcMask = new S_MASK();
            /// <summary>
            /// epc mask length in bit, e.g. epc = 0x3000, length = 4 * 8 = 32 bits, epc = 0x300, length = 3 * 8 = 24 bits
            /// </summary>
            public UInt32 epcMaskLength = 0;
            /// <summary>
            /// epc mask offset in bit, note : if enable PC mask, this parameter is ignored.
            /// </summary>
            public UInt32 epcMaskOffset = 0;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match (use epcMask, if you select EPC)
            /// </summary>
            public byte[] Mask;
            /// <summary>
            /// epc mask length in bit (use epcMaskLength, if you select EPC)
            /// </summary>
            public UInt32 MaskLength = 0;
            /// <summary>
            /// epc mask offset in bit (use epcMaskOffset, if you select EPC)
            /// </summary>
            public UInt32 MaskOffset = 0;
            /// <summary>
            /// UCODE 7 Parallel Encoding
            /// </summary>
            public bool ParallelEncoding = false;
            /// <summary>
            /// constructor
            /// </summary>
            public TagGeneralSelectedParms()
            {
                // NOP
            }
        }
        /// <summary>
        /// Post match tag parameters, configure this before any specific tag operation
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class TagPostMachParms
        {
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for 
            /// the specified target (i.e., A or B), if the target should be toggled 
            /// (i.e., A to B or B to A) and another inventory cycle run.  A non-
            /// zero value indicates that the target should be toggled.  A zero 
            /// value indicates that the target should not be toggled.  
            /// </summary>
            public Boolean toggleTarget = false;
            /// <summary>
            /// The offset in bits, from the start of the Electronic Product 
            /// Code (EPC), of the first bit that will be matched against the 
            /// mask.  If offset falls beyond the end of EPC, the tag is 
            /// considered non-matching. 
            /// </summary>
            public uint offset = 0;
            /// <summary>
            /// The number of bits in the mask.  A length of zero will cause 
            /// all tags to match.  If (offset+count) falls beyond the end 
            /// of the EPC, the tag is considered non-matching.  Valid 
            /// values are 0 to 496, inclusive. 
            /// </summary>
            public uint count = 0;
            /// <summary>
            /// A buffer that contains a left-justified bit array that represents 
            /// that bit pattern to match – i.e., the most significant bit of the 
            /// bit array appears in the most-significant bit (i.e., bit 7) of the 
            /// first byte of the buffer (i.e., mask[0]).  All bits beyond count 
            /// are ignored.  For example, if the application wished to find 
            /// tags with the following 16 bits 1011.1111.1010.0101, 
            /// starting at offset 20 in the Electronic Product Code, then the 
            /// fields would be set as follows: 
            /// offset  = 20 
            /// count   = 16 
            /// mask[0] = 0xBF (1011.1111) 
            /// mask[1] = 0xA5 (1010.0101)
            /// </summary>
            public Byte[] mask = new byte[62];
            /// <summary>
            /// constructor
            /// </summary>
            public TagPostMachParms()
            {
                // NOP
            }
        }
        /// <summary>
        /// TagReadProtectParms
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class TagReadProtectParms
        {
            /// <summary>
            /// Flag - Zero or combination of  Select or Post-Match
            /// </summary>
            public SelectFlags flags = SelectFlags.UNKNOWN;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no 
            /// access password. 
            /// </summary>
            public UInt32 accessPassword;
            /// <summary>
            /// Max retry count can't excess 15
            /// </summary>
            public UInt32 retryCount;

        }
        [StructLayout(LayoutKind.Sequential)]
        class InternalCustCmdEASParms
        {
            /// <summary>
            /// Structure size
            /// </summary>
            protected UInt32 length = 16;
            /// <summary>
            /// Enable EAS 
            /// </summary>
            public bool enable = false;
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no access 
            /// password. 
            /// </summary>
            public UInt32 accessPassword = 0;
            /// <summary>
            /// Retry count
            /// </summary>
            public UInt32 retry = 0;
            /// <summary>
            /// constructor
            /// </summary>
            public InternalCustCmdEASParms()
            {
                // NOP
            }
        }
        #endregion //WaterYu @ 16 - Sept-2009 END


#if NOUSE
        [StructLayout(LayoutKind.Sequential)]
        public class cmnparm
        {
            protected UInt32 length_ = 4;

            public cmnparm()
            {
                // NOP - child classes MUST set length_ to 
                //       an appropriate value...
            }

            public UInt32 length
            {
                get { return this.length_; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_INV_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public UInt32 tagStopCount = 0;
            public InventoryCallbackDelegate callback;
            public CB_INV_PARMS()
            {
                base.length_ = 20;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_READ_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public ReadParms parms = new ReadParms();
            public TagAccessCallbackDelegate callback;
            public CB_READ_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 12 + 16);//2c
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_WRITE_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public WriteParms parms = new WriteParms();
            public TagAccessCallbackDelegate callback;
            public CB_WRITE_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 4 + (12 + IntPtr.Size * 2) + 12 + 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_LOCK_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public LockParms parms = new LockParms();
            public TagAccessCallbackDelegate callback;
            public CB_LOCK_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + (20) + 4 + 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_KILL_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public KillParms parms = new KillParms();
            public TagAccessCallbackDelegate callback;
            public CB_KILL_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 8 + 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_BLOCK_WRITE_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public BlockWriteParms parms = new BlockWriteParms();
            public TagAccessCallbackDelegate callback;
            public CB_BLOCK_WRITE_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 20 + (IntPtr.Size) + 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CB_BLOCK_ERASE_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public UInt32 flags = 0;
            public BlockEraseParms parms = new BlockEraseParms();
            public TagAccessCallbackDelegate callback;
            public CB_BLOCK_ERASE_PARMS()
            {
                base.length_ = (UInt32)(4 + (4 + IntPtr.Size * 3) + 20 + 16);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public class CB_TAG_SEARCH_PARMS : cmnparm
        {
            public Int32 handle = 0;
            public bool average = false;
            public bool bUsePc = false;
            public UInt32 retryCount = 0;
            public UInt32 masklen = 0;
            public Byte[] pc_epc_mask = new byte[32];

            public CB_TAG_SEARCH_PARMS()
            {
                base.length_ = 56;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TAG_INV_RECORD_T
        {
            public UInt32 ms_ctr;		//0-3
            [MarshalAs(UnmanagedType.R8)]
            public Double rssi;		//4-5
            public UInt16 ana_ctrl1;  //6-7
            public UInt16 pc;			//8-9
            public UInt16 epclength;  //10-11
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public Byte[] epc;	//12 - 76
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct TAG_ACCESS_RECORD_T
        {
            public UInt32 ms_ctr;		//0-3
            public byte cmd;		//4-5
            public byte error_code;  //6-7
            public UInt16 datalength;  //10-11
            public IntPtr pdata;	//12 - 76
        }; 
#endif
    } // Structures end


} // rfid_csharp end

