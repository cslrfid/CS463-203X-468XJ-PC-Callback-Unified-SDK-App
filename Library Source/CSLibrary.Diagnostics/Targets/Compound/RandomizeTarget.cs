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
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;

using CSLibrary.Diagnostics.Internal;
using System.Net;
using System.Net.Sockets;

using CSLibrary.Diagnostics.Config;

namespace CSLibrary.Diagnostics.Targets.Compound
{
    /// <summary>
    /// A compound target writes to a randomly-chosen target among the sub-targets.
    /// </summary>
    /// <example>
    /// <p>This example causes the messages to be written to either file1.txt or file2.txt 
    /// chosen randomly on a per-message basis.
    /// </p>
    /// <p>
    /// To set up the target in the <a href="config.html">configuration file</a>, 
    /// use the following syntax:
    /// </p>
    /// <code lang="XML" src="examples/targets/Configuration File/RandomizeGroup/NLog.config" />
    /// <p>
    /// The above examples assume just one target and a single rule. See below for
    /// a programmatic configuration that's equivalent to the above config file:
    /// </p>
    /// <code lang="C#" src="examples/targets/Configuration API/RandomizeGroup/Simple/Example.cs" />
    /// </example>
    [Target("RandomizeGroup", IgnoresLayout = true, IsCompound = true)]
    public class RandomizeTarget: CompoundTargetBase
    {
        private static Random _random = new Random();

        /// <summary>
        /// Creates an instance of <see cref="RandomizeTarget"/>.
        /// </summary>
        public RandomizeTarget()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="RandomizeTarget"/> and
        /// initializes the <see cref="Targets"/> collection with the
        /// specified list of <see cref="Target"/> objects.
        /// </summary>
        public RandomizeTarget(params Target[] targets) : base(targets)
        {
        }

        /// <summary>
        /// Forwards the log event to one of the sub-targets. 
        /// The sub-target is randomly chosen.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        protected internal override void Write(LogEventInfo logEvent)
        {
            int pos = _random.Next(Targets.Count);
            Targets[pos].Write(logEvent);
        }
   }
}
