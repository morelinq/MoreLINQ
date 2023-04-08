#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2023 Jamin King. All rights reserved.
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
	using System.Collections.Generic;
	using System.Linq;

	static partial class MoreEnumerable
	{
		/// <summary>
		/// Trim from the start and end of an enumeration any items
		/// matching a given predicate.
		/// Example #1: [1,4,8,1,5,4,7,9], IsOdd -> [4,8,1,5,4]
		/// Example #2: " foo  ", IsWhiteSapce -> ['f','o','o']
		/// </summary>
		/// <param name="source">The enumeration</param>
		/// <param name="predicate">The trim condition</param>
		/// <typeparam name="T">The item of items in the unmeration</typeparam>
		/// <exeception cref="ArgumentNullException"/>
		/// <returns>
		/// The trimmed enumeration
		/// </returns>
		public static IEnumerable<T> Trim<T>(this IEnumerable<T> source, Predicate<T> predicate)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			
			/*
				TODO(?)  Special cases for random-access enumerations such
						T[] or ILst<T>. These can be optimized by not having
						to enumerate over the "middle."
			*/

			return source.TrimStart(predicate).TrimEnd(predicate);
		}

		private static IEnumerable<T> TrimStart<T>(this IEnumerable<T> source, Predicate<T> predicate)
		{
			return source.SkipWhile(t => predicate(t));
		}

		private static IEnumerable<T> TrimEnd<T>(this IEnumerable<T> source, Predicate<T> predicate)
		{
			/*
				The goal of any optimized enumeration algorithm should be to
					> Minimize the amount of foreach (translates to new'ing an enumerator)
					> Minimize the amount of intermediary memory such as temporary lists holding values from the enumeration
					> Move to each item at most once (no multiple enumeration)
				with of course some tradoff involved.
			*/
			ICollection<T> section = new List<T>();
			using IEnumerator<T> enumerator = source.GetEnumerator();
			for (bool enumerating = enumerator.MoveNext(); enumerating; )
			{
				// (1) Return items not matching predicate
				do
				{
					T cur = enumerator.Current;
					if (predicate(cur))
						break;
					else
						yield return cur;
				} while (enumerating = enumerator.MoveNext());
				/*
					(2) Return items matching the predicate, so long as they
						don't form the ending sequence of the enumeration.
				*/
				if (enumerating)
				{
					/*
						If here, we're currently on an item matching the predicate.
						Place this an any subsequent items matching the predicate
						into a collection. We may return the items in the collection,
						depending on whether
					*/
					section.Clear(); 
					T cur = enumerator.Current;
					do section.Add(cur);
					while ((enumerating = enumerator.MoveNext()) && predicate(cur = enumerator.Current));

					if (enumerating)
					{
						/*
							If here, we're currently on an item not matching the predicate;
							i.e., the section of items matching the predicate was not the
							end which would be trimmed.
						*/
						foreach (T t in section)
							yield return t;
					}
					/*
						Else, the section of items matching the predicate will be thrown
						away because we'll terminate the loop.
					*/
				}
			}
		}
	}
}
