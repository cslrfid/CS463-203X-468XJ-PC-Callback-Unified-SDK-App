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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#if DOTNET_1_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET 1.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Full Framework 1.0")]
#elif DOTNET_1_1
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET 1.1")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Full Framework 1.1")]
#elif DOTNET_2_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics - Full Framework 2.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Full Framework 2.0")]
#elif DOTNET_3_5
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET 3.5")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Full Framework 3.5")]
#elif MONO_1_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics for Mono 1.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Mono 1.0")]
#elif MONO_2_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics for Mono 2.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - Mono 2.0")]
#elif NETCF_1_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET Compact Framework 1.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - .NET Compact Framework 1.0")]
#elif NETCF_2_0
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET Compact Framework 2.0")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - .NET Compact Framework 2.0")]
#elif NETCF_3_5
[assembly: AssemblyTitle("CSLibrary.Diagnostics for .NET Compact Framework 3.5")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics - .NET Compact Framework 3.5")]
#elif DOCUMENTATION
[assembly: AssemblyTitle("CSLibrary.Diagnostics Documentation")]
[assembly: AssemblyDescription("CSLibrary.Diagnostics")]
#endif

[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Convergence System Ltd(CSL)")]
[assembly: AssemblyProduct("CSLibrary.Diagnostics - .NET Logging Library")]
[assembly: AssemblyCopyright("Copyright (c) 2010 by CSL")]
[assembly: AssemblyCulture("")]

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

#if !NETCF
//[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true, TypeInformation = true)]
//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Flags = SecurityPermissionFlag.Execution)]
//[assembly: FileIOPermission(SecurityAction.RequestMinimum, Unrestricted = true)]
#endif
[assembly: AssemblyVersionAttribute("1.0.0")]
[assembly: GuidAttribute("438687F4-839E-4bfb-B0F8-24222939D56D")]
#if !NETCF
[assembly: AssemblyFileVersionAttribute("1.0.0")]
#endif
