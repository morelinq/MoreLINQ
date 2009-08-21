using System;
using System.Reflection;
using System.Runtime.InteropServices;

#if SILVERLIGHT
[assembly: AssemblyTitle("MoreLINQ for Silverlight")]
#else
[assembly: AssemblyTitle("MoreLINQ")]
#endif
[assembly: AssemblyDescription("Extensions to LINQ to Objects")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MoreLINQ")]
[assembly: AssemblyCopyright("Copyright (c) 2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version numbers

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Debug or release configuration?

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

// CLS compliance and COM visibility

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

// ID of the typelib if this project is exposed to COM.

[assembly: Guid("fc632c9d-390e-4902-8c1c-3e57b08c1d38")]
