#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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

namespace MoreLinq
{
    using System;
#if !NO_EXCEPTION_SERIALIZATION
    using System.Runtime.Serialization;
#endif

    /// <summary>
    /// The exception that is thrown for a sequence that fails a condition.
    /// </summary>

#if !NO_EXCEPTION_SERIALIZATION
    [ Serializable ]
#endif
    public class SequenceException : Exception
    {
        private const string defaultMessage = "Error in sequence.";

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceException"/> class. 
        /// </summary>

        public SequenceException() :
            this(null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceException"/> class
        /// with a given error message. 
        /// </summary>
        /// <param name="message">A message that describes the error.</param>

        public SequenceException(string message) :
            this(message, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceException"/> class
        /// with a given error message and a reference to the inner exception
        /// that is the cause of the exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>

        public SequenceException(string message, Exception innerException) :
            base(string.IsNullOrEmpty(message) ? defaultMessage : message, innerException) { }

#if !NO_EXCEPTION_SERIALIZATION
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>

        protected SequenceException(SerializationInfo info, StreamingContext context) : 
            base(info, context) {}
#endif
    }
}
