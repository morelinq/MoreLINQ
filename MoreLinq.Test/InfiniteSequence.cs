using System;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// Test support class used to rturn an infinite sequence.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InfiniteSequence<T> : IEnumerable<T>
    {
        private readonly T m_Value; // value that will always be returned

        #region Constructors
        /// <summary>
        /// Creates an infinite sequence that returns the specified value.
        /// </summary>
        /// <param name="value">The type of the elements in the sequence</param>
        public InfiniteSequence(T value)
        {
            m_Value = value;
        }
        #endregion

        #region IEnumerable<T> Members
        /// <summary>
        /// Returns an iterator that will forever return the specified value
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            while (true)
                yield return m_Value;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}