#region License and Terms
// The MIT License (MIT)
//
// Copyright (c) .NET Foundation and Contributors
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

#if !NET7_0_OR_GREATER

namespace MoreLinq
{
    using System;

    // Source: https://github.com/dotnet/runtime/blob/v7.0.2/src/libraries/System.Private.CoreLib/src/System/Diagnostics/UnreachableException.cs

    /// <summary>
    /// Exception thrown when the program executes an instruction that was thought to be unreachable.
    /// </summary>

#if !NETSTANDARD1_0
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
#pragma warning disable CA1064 // Exceptions should be public
    sealed class UnreachableException : Exception
#pragma warning restore CA1064 // Exceptions should be public
    {
        public UnreachableException() :
            this(null) { }

        public UnreachableException(string? message) :
            base(message, null) { }

        public UnreachableException(string? message, Exception? innerException) :
            base(message ?? "The program executed an instruction that was thought to be unreachable.",
                 innerException) { }
    }
}

#endif // !NET7_0_OR_GREATER
