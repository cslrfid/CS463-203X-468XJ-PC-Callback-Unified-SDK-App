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
using System.Text;
using System.Runtime.InteropServices;

namespace CSLibrary.Structures
{
    using Constants;

    /// <summary>
    /// Write structures, configure this before write
    /// </summary>
    public class TagWriteParms
    {
        public MemoryBank bank;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure (Process Retry / Library Retry)
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// Number of retrial will retry if write failure (Write Retry / Firmware Retry)
        /// </summary>
        public UInt32 writeRetryCount = 32;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to write.
        /// </summary>
        public UInt16 offset;
        /// <summary>
        /// The number of 16-bit words that will be written.  
        /// </summary>                                       
        public UInt16 count;
        /// <summary>
        /// A array to the 16-bit values to write to the tag's memory bank.
        /// </summary>
        public UInt16[] pData = new UInt16[0];
        //public S_DATA pData = new S_DATA();
        /// <summary>
        /// Flag - Normal or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.SELECT;
    }

    /// <summary>
    /// Write PC structures, configure this before write new PC value
    /// </summary>
    public class TagWritePcParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure (Process Retry / Library Retry)
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// Number of retrial will retry if write failure (Write Retry / Firmware Retry)
        /// </summary>
        public UInt32 writeRetryCount = 32;
        /// <summary>
        /// A new pc to the 16-bit values to write to the tag's memory bank.
        /// </summary>
//        public S_PC pc = new S_PC();
        public UInt16 pc;
        /// <summary>
        /// Flag - Normal or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.SELECT;
    }
/*
    /// <summary>
    /// Write PC structures, configure this before write new PC value
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TagWritePcParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// A new pc to the 16-bit values to write to the tag's memory bank.
        /// </summary>
        public UInt16 pc;
        /// <summary>
        /// 
        /// </summary>
        public TagWritePcParms()
        {
            // NOP
        }
    }
 */
    /// <summary>
    /// Write EPC structures, configure this before write new EPC value
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TagWriteEpcParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure (Process Retry / Library Retry)
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// Number of retrial will retry if write failure (Write Retry / Firmware Retry)
        /// </summary>
        public UInt32 writeRetryCount = 32;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to write.
        /// </summary>
        public UInt16 offset;
        /// <summary>
        /// The number of 16-bit words that will be written.  This field must be
        /// between 1 and 31, inclusive.  
        /// </summary>
        public UInt16 count;
        /// <summary>
        /// A new epc to the 16-bit values to write to the tag's memory bank.
        /// </summary>
        public S_EPC epc;
        /// <summary>
        /// 
        /// </summary>
        public TagWriteEpcParms()
        {
            // NOP
        }
    }
    /// <summary>
    /// Write password structures, configure this before write new password value
    /// </summary>
    public class TagWritePwdParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure (Process Retry / Library Retry)
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// Number of retrial will retry if write failure (Write Retry / Firmware Retry)
        /// </summary>
        public UInt32 writeRetryCount = 32;
        /// <summary>
        /// A new password to the 32-bit values to write to the tag's memory bank.
        /// </summary>
        //public S_PWD password = new S_PWD();
        public UInt32 password;
        /// <summary>
        /// Flag - Normal or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.SELECT;
    }

/*    
    /// <summary>
    /// Write password structures, configure this before write new password value
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TagWritePwdParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// A new password to the 32-bit values to write to the tag's memory bank.
        /// </summary>
        public uint password;
        /// <summary>
        /// 
        /// </summary>
        public TagWritePwdParms()
        {
            // NOP
        }
    }
*/

    /// <summary>
    /// Write User structures, configure this before write new user data
    /// </summary>
    public class TagWriteUserParms
    {
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Number of retrial will retry if write failure (Process Retry / Library Retry)
        /// </summary>
        public UInt32 retryCount;
        /// <summary>
        /// Number of retrial will retry if write failure (Write Retry / Firmware Retry)
        /// </summary>
        public UInt32 writeRetryCount = 32;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to write.
        /// </summary>
        public UInt16 offset;
        /// <summary>
        /// The number of 16-bit words that will be written.  
        /// </summary>                                       
        public UInt16 count;
        /// <summary>
        /// A array to the 16-bit values to write to the tag's memory bank.
        /// </summary>
        public UInt16[] pData = new UInt16[0];
        //public S_DATA pData = new S_DATA();
        /// <summary>
        /// Flag - Normal or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.SELECT;




    }

    /// <summary>
    /// The ISO 18000-6C tag-block write operation parameters
    /// </summary>
    public class TAG_BLOCK_WRITE_PARMS
    {
        /// <summary>
        /// The maximum number of times the write should be retried in the event 
        /// of write-verify failure(s).  In the event of write-verify failure(s), the write 
        /// will be retried either for the specified number of retries or until the data 
        /// written is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the written data, the write 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 31, inclusive. 
        /// If verify is non-zero, this parameter is ignored.
        /// </summary>
        public uint retryCount = 31;
        /// <summary>
        /// Starting offset
        /// </summary>
        public ushort offset;
        /// <summary>
        /// Total number of words written to user memory
        /// </summary>
        public ushort count;
        /// <summary>
        /// Write Buffer data to target tag
        /// </summary>
        public UInt16[] pData = new UInt16[0];
        /// <summary>
        /// Target Memory Bank
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// Target Access Password
        /// </summary>
        public uint accessPassword;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.SELECT;
    }
    
    /*
     * /// <summary>
        /// Write User structures, configure this before write new user data
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class TagWriteUserParms
        {
            /// <summary>
            /// The access password for the tags.  A value of zero indicates no 
            /// access password. 
            /// </summary>
            public UInt32 accessPassword;
            /// <summary>
            /// Number of retrial will retry if write failure
            /// </summary>
            public UInt32 retryCount;
            /// <summary>
            /// The offset, in the memory bank, of the first 16-bit word to write.
            /// </summary>
            public UInt16 offset;
            /// <summary>
            /// The number of 16-bit words that will be written.  
            /// </summary>                                       
            public UInt16 count;
            /// <summary>
            /// A array to the 16-bit values to write to the tag's memory bank.
            /// </summary>
            public UInt16[] pData = new UInt16[0];
            /// <summary>
            /// Constructor
            /// </summary>
            public TagWriteUserParms()
            {
                // NOP
            }
        }
    */
}
