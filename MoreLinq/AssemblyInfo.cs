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

[assembly: AssemblyTitle("MoreLINQ")]
[assembly: AssemblyDescription("Extensions to LINQ to Objects")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MoreLINQ")]
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
[assembly: System.Runtime.InteropServices.ComVisible(false)]

// ID of the typelib if this project is exposed to COM.

[assembly: System.Runtime.InteropServices.Guid("fc632c9d-390e-4902-8c1c-3e57b08c1d38")]
