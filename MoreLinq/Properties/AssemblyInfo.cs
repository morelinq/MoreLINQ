#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("MoreLINQ")]
[assembly: AssemblyDescription("Extensions to LINQ to Objects")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MoreLINQ")]
[assembly: AssemblyCopyright("Copyright \u00a9 2008 Jonathan Skeet. All rights reserved. Portions Copyright (c) Microsoft. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Debug or release configuration?

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

// CLS compliance and COM visibility

[assembly: CLSCompliant(true)]
#if !NO_COM
[assembly: ComVisible(false)]

// ID of the typelib if this project is exposed to COM.

[assembly: Guid("fc632c9d-390e-4902-8c1c-3e57b08c1d38")]
#endif

[assembly: InternalsVisibleTo("MoreLinq.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100290a359c9159ca9f82c3d03a0d0f3e3475193a03396eef81aa8704db25c9e06507f28326ddf2f74671ca6a906ab2fc560dbac02e0ddabff53872ba3d6b609735f4c9ba4cba88c6bbca1ede2f78d4b473be3fac627b1faa700656d13aaf946eb6738a299c0001d5fe2ae0c0ef79843fc84460bb2de8855938d622dcd48bbcdbd5")]
[assembly: InternalsVisibleTo("MoreLinq.Portable.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100290a359c9159ca9f82c3d03a0d0f3e3475193a03396eef81aa8704db25c9e06507f28326ddf2f74671ca6a906ab2fc560dbac02e0ddabff53872ba3d6b609735f4c9ba4cba88c6bbca1ede2f78d4b473be3fac627b1faa700656d13aaf946eb6738a299c0001d5fe2ae0c0ef79843fc84460bb2de8855938d622dcd48bbcdbd5")]
