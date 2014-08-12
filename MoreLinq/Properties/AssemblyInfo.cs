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
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("MoreLINQ")]
[assembly: AssemblyDescription("Extensions to LINQ to Objects")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MoreLINQ")]
[assembly: AssemblyCopyright("Copyright \u00a9 2008 Jonathan Skeet. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version numbers
//
// The build and revision number reflect the date and time
// of a build, using the follow scheme:
//
// bld = months_since_2000 x 100 + day_of_month
// rev = utc_hours_since_midnight + utc_minutes

[assembly: AssemblyVersion("2.0.17511.0")]
[assembly: AssemblyFileVersion("2.0.17511.620")]

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