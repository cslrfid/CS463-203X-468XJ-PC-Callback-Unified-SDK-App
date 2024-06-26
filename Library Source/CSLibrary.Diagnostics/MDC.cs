// 
// Copyright (c) 2004-2006 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Collections;

namespace CSLibrary.Diagnostics
{
    /// <summary>
    /// Mapped Diagnostics Context - a thread-local structure that keeps a dictionary
    /// of strings and provides methods to output them in layouts. 
    /// Mostly for compatibility with log4net.
    /// </summary>
    public sealed class MDC
    {
        private MDC(){}

        /// <summary>
        /// Sets the current thread MDC item to the specified value.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <param name="value">Item value.</param>
        public static void Set(string item, string value)
        {
            IDictionary dict = GetThreadDictionary();

            dict[item] = value;
        }

        /// <summary>
        /// Gets the current thread MDC named item.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>The item value of String.Empty if the value is not present.</returns>
        public static string Get(string item)
        {
            IDictionary dict = GetThreadDictionary();

            string s = (string)dict[item];
            if (s == null)
                return String.Empty;
            else
                return s;
        }

        /// <summary>
        /// Checks whether the specified item exists in current thread MDC.
        /// </summary>
        /// <param name="item">Item name.</param>
        /// <returns>A boolean indicating whether the specified item exists in current thread MDC.</returns>
        public static bool Contains(string item)
        {
            IDictionary dict = GetThreadDictionary();

            return dict.Contains(item);
        }

        /// <summary>
        /// Removes the specified item from current thread MDC.
        /// </summary>
        /// <param name="item">Item name.</param>
        public static void Remove(string item)
        {
            IDictionary dict = GetThreadDictionary();

            dict.Remove(item);
        }

        /// <summary>
        /// Clears the content of current thread MDC.
        /// </summary>
        public static void Clear()
        {
            IDictionary dict = GetThreadDictionary();

            dict.Clear();
        }

        internal static IDictionary GetThreadDictionary()
        {
            IDictionary threadDictionary = (IDictionary)System.Threading.Thread.GetData(_dataSlot);

            if (threadDictionary == null)
            {
                threadDictionary = new Hashtable();
                System.Threading.Thread.SetData(_dataSlot, threadDictionary);
            }

            return threadDictionary;
        }

        private static LocalDataStoreSlot _dataSlot = System.Threading.Thread.AllocateDataSlot();
    }
}
