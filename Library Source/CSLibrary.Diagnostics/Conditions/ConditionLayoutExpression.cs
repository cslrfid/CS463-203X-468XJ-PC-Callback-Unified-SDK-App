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

using System.Xml.Serialization;

namespace CSLibrary.Diagnostics.Conditions
{
    /// <summary>
    /// Condition layout expression (represented by a string literal
    /// with embedded ${})
    /// </summary>
    internal sealed class ConditionLayoutExpression : ConditionExpression
    {
        private Layout _layout;

        /// <summary>
        /// Creates a new instance of <see cref="ConditionLayoutExpression"/>
        /// and initializes the layout.
        /// </summary>
        /// <param name="layout"></param>
        public ConditionLayoutExpression(string layout) 
        {
            _layout = new Layout(layout);
        }

        /// <summary>
        /// Evaluates the expression by calculating the value
        /// of the layout in the specified evaluation context.
        /// </summary>
        /// <param name="context">Evaluation context</param>
        /// <returns>The value of the layout.</returns>
        public override object Evaluate(LogEventInfo context)
        {
            return _layout.GetFormattedMessage(context);
        }

        /// <summary>
        /// Returns a string representation of this expression.
        /// </summary>
        /// <returns>String literal in single quotes.</returns>
        public override string ToString()
        {
            return "'" + _layout.Text + "'";
        }

        /// <summary>
        /// Adds all layouts used by this expression to the specified collection.
        /// </summary>
        /// <param name="layouts">The collection to add layouts to.</param>
        public override void PopulateLayouts(LayoutCollection layouts)
        {
            _layout.PopulateLayouts(layouts);
        }
    }
}
