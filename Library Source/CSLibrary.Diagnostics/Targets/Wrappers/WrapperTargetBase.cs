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
using System.Xml;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

using CSLibrary.Diagnostics;
using CSLibrary.Diagnostics.Config;

using CSLibrary.Diagnostics.Internal;

namespace CSLibrary.Diagnostics.Targets.Wrappers
{
    /// <summary>
    /// Base class for targets wrap other (single) targets.
    /// </summary>
    public abstract class WrapperTargetBase: Target
    {
        private Target _wrappedTarget = null;

        /// <summary>
        /// The target that this target wraps.
        /// </summary>
        public Target WrappedTarget
        {
            get { return _wrappedTarget; }
            set { _wrappedTarget = value; }
        }

        /// <summary>
        /// Adds all layouts used by this target to the specified collection.
        /// </summary>
        /// <param name="layouts">The collection to add layouts to.</param>
        public override void PopulateLayouts(LayoutCollection layouts)
        {
            base.PopulateLayouts (layouts);
            WrappedTarget.PopulateLayouts(layouts);
        }

        /// <summary>
        /// Closes the target by forwarding the call to the <see cref="WrapperTargetBase.WrappedTarget"/> object.
        /// </summary>
        protected internal override void Close()
        {
            base.Close();
            WrappedTarget.Close();
        }

        /// <summary>
        /// Forwards the call to <see cref="WrapperTargetBase.WrappedTarget"/>.NeedsStackTrace().
        /// </summary>
        /// <returns>The value of forwarded call</returns>
        protected internal override int NeedsStackTrace()
        {
            return WrappedTarget.NeedsStackTrace();
        }

        /// <summary>
        /// Initializes the target by forwarding the call 
        /// to <see cref="Target.Initialize"/> to the <see cref="WrapperTargetBase.WrappedTarget"/>.
        /// </summary>
        public override void Initialize()
        {
            WrappedTarget.Initialize();
        }

        /// <summary>
        /// Returns the text representation of the object. Used for diagnostics.
        /// </summary>
        /// <returns>A string that describes the target.</returns>
        public override string ToString()
        {
            return ((this.Name != null) ? this.Name : "unnamed") + ":" + this.GetType().Name + "(" + ((WrappedTarget != null) ? WrappedTarget.ToString() : "null") + ")";
        }
    }
}
