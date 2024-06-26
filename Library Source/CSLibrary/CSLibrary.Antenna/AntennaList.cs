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

//#if CS468
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CSLibrary
{
    using CSLibrary.Constants;
    using CSLibrary.Structures;
#if NETCFDESIGNTIME
    [System.ComponentModel.TypeConverter(typeof(AntennaListTypeConverter))]
#endif
    public class AntennaList
        :
        List<Antenna>
    {

#if NETCFDESIGNTIME && NO_NEED

        public static AntennaList DEFAULT_ANTENNA_LIST
        {
            get
            {
                Object obj = TypeDescriptor.GetConverter(typeof(AntennaList)).ConvertFromString
                (
                    CSLibrary.Properties.Settings.Default.DefaultAntennaSettings
                );

                if (null == obj)
                {
                    // SHOULD NEVER OCCUR
                }

                return obj as AntennaList;
            }
        }
#endif
        /*0,ENABLED,300,2000,0,0,0,1048575;
        1,DISABLED,300,2000,0,4,4,1048575;
        2,DISABLED,300,2000,0,8,8,1048575;
        3,DISABLED,300,2000,0,12,12,1048575;
        4,DISABLED,300,2000,0,1,1,1048575;
        5,DISABLED,300,2000,0,5,5,1048575;
        6,DISABLED,300,2000,0,9,9,1048575;
        7,DISABLED,300,2000,0,13,13,1048575;
        8,DISABLED,300,2000,0,2,2,1048575;
        9,DISABLED,300,2000,0,6,6,1048575;
        10,DISABLED,300,2000,0,10,10,1048575;
        11,DISABLED,300,2000,0,14,14,1048575;
        12,DISABLED,300,2000,0,3,3,1048575;
        13,DISABLED,300,2000,0,7,7,1048575;
        14,DISABLED,300,2000,0,11,11,1048575;
        15,DISABLED,300,2000,0,15,15,1048575;*/
        /// <summary>
        /// Default antenna list
        /// </summary>
        public static readonly AntennaList DEFAULT_ANTENNA_LIST;

        static AntennaList()
        {
            Machine DeviceType = Machine.CS468;

            DEFAULT_ANTENNA_LIST = new AntennaList();

            switch (DeviceType)
            {
                case Machine.CS101:
                case Machine.CS203:
                case Machine.CS208:
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(0, AntennaPortState.ENABLED, 300, 2000, 0, 0, 0, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    break;

                case Machine.CS469:
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(0, AntennaPortState.ENABLED, 300, 2000, 0, 0, 0, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(1, AntennaPortState.DISABLED, 300, 2000, 0, 4, 4, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(2, AntennaPortState.DISABLED, 300, 2000, 0, 8, 8, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(3, AntennaPortState.DISABLED, 300, 2000, 0, 12, 12, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    break;

                case Machine.CS203X:
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(0, AntennaPortState.DISABLED, 300, 2000, 0, 0, 0, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(1, AntennaPortState.DISABLED, 300, 2000, 0, 4, 4, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(2, AntennaPortState.DISABLED, 300, 2000, 0, 8, 8, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(3, AntennaPortState.ENABLED, 300, 2000, 0, 12, 12, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    break;

                case Machine.CS468:
                case Machine.CS468INT:
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(0, AntennaPortState.ENABLED, 300, 2000, 0, 0, 0, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(1, AntennaPortState.DISABLED, 300, 2000, 0, 4, 4, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(2, AntennaPortState.DISABLED, 300, 2000, 0, 8, 8, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(3, AntennaPortState.DISABLED, 300, 2000, 0, 12, 12, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(4, AntennaPortState.DISABLED, 300, 2000, 0, 1, 1, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(5, AntennaPortState.DISABLED, 300, 2000, 0, 5, 5, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(6, AntennaPortState.DISABLED, 300, 2000, 0, 9, 9, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(7, AntennaPortState.DISABLED, 300, 2000, 0, 13, 13, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(8, AntennaPortState.DISABLED, 300, 2000, 0, 2, 2, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(9, AntennaPortState.DISABLED, 300, 2000, 0, 6, 6, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(10, AntennaPortState.DISABLED, 300, 2000, 0, 10, 10, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(11, AntennaPortState.DISABLED, 300, 2000, 0, 14, 14, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(12, AntennaPortState.DISABLED, 300, 2000, 0, 3, 3, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(13, AntennaPortState.DISABLED, 300, 2000, 0, 7, 7, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(14, AntennaPortState.DISABLED, 300, 2000, 0, 11, 11, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    DEFAULT_ANTENNA_LIST.Add(new Antenna(15, AntennaPortState.DISABLED, 300, 2000, 0, 15, 15, false, false, SingulationAlgorithm.DYNAMICQ, 0, false, 0, false, 0, 1048575));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Create an empty antenna list
        /// </summary>
        public AntennaList()
            :
            base()
        {
            // NOP
        }


        /// <summary>
        /// Create an empty antenna list with initial capacity
        /// </summary>
        /// <param name="capacity"></param>
        public AntennaList(Int32 capacity)
            :
            base(capacity)
        {
            // NOP
        }


        /// <summary>
        /// Copy an antenna list ~ no checks for dup ports
        /// </summary>
        /// <param name="enumerable"></param>
        public AntennaList(IEnumerable<Antenna> enumerable)
            :
            base(enumerable)
        {
            // NOP
        }


        /// <summary>
        /// Copy an antenna list ~ performing a DEEP copy of
        /// antennas if indicated
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="deepCopy"></param>

        public AntennaList(IEnumerable<Antenna> enumerable, Boolean deepCopy)
            :
            base()
        {
            if (!deepCopy)
            {
                this.AddRange(enumerable);
            }
            else
            {
                this.Copy(enumerable);
            }
        }



        public void Copy(IEnumerable<Antenna> from)
        {
            this.Clear();

            foreach (Antenna antenna in from)
            {
                this.Add(new Antenna(antenna));
            }
        }



        // Attempt to load info for all known antennas

        public Result Load (HighLevelInterface transport)
        {
            this.Clear();

            for (UInt32 port = 0; ; ++port)
            {
                Antenna antenna = new Antenna(port);

                Result status = antenna.Load(transport);

                if (Result.OK == status)
                {
                    this.Add(antenna);
                }
                else if (Result.INVALID_PARAMETER == status)
                {
                    break; // this rcv when portIndex > logical antenna count on radio
                }
                else if (Result.EMULATION_MODE == status)
                {
                    break; // this rcv when library ( transport ) is in emulation mode
                }
                else
                {
                    Console.WriteLine("Error while reading antenna information");

                    return status; // this rcv all other errors
                }
            }

            return Result.OK;
        }


        // Attempt to save all link profiles currently on the radio
        // and mark the active one.

        public Result Store (HighLevelInterface transport)
        {
            foreach (Antenna antenna in this)
            {
                Result status = antenna.Store(transport);

                if (Result.OK != status)
                {
                    return status;
                }
            }

            return Result.OK;
        }


        // Attempt to locate antenna with matching ( logical ) port

        public Antenna FindByPort(UInt32 port)
        {
            Antenna Result = this.Find
                (
                    delegate(Antenna antenna)
                    {
                        return antenna.Port == port;
                    }
                );

            return Result;
        }

        // Attempt to locate antenna with matching physical rx port

        public Antenna FindByRxPort(UInt32 port)
        {
            Antenna Result = this.Find
                (
                    delegate(Antenna antenna)
                    {
                        return antenna.PhysicalRxPort == port;
                    }
                );

            return Result;
        }

        // Attempt to locate antenna with matching physical rx port

        public Antenna FindByTxPort(UInt32 port)
        {
            Antenna Result = this.Find
                (
                    delegate(Antenna antenna)
                    {
                        return antenna.PhysicalTxPort == port;
                    }
                );

            return Result;
        }


    } // END class Source_AntennaList


} // END namespace RFID.RFIDInterface
//#endif