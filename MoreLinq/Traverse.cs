#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Felipe Sateler. All rights reserved.
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

    public partial class MoreEnumerable
    {
        /// <summary>
        /// Traverses a tree starting at the <paramref name="root"/> according to the <paramref name="childrenSelector"/> traversal function.
        /// The returned elements are the result of breadth-first traversal.
        /// </summary>
        /// <typeparam name="T">The tree node type</typeparam>
        /// <param name="root">The root of the tree to traverse</param>
        /// <param name="childrenSelector">The function that produces the children of each element</param>
        /// <returns>A sequence containing the traversed values</returns>
        /// <remarks>
        /// This function defers traversal until needed and streams the results.
        /// The progression function is not checked for loops. If you need the resulting sequence to be finite
        /// you must ensure that loops are not produced by <paramref name="childrenSelector"/>
        /// </remarks>
        public static IEnumerable<T> TraverseBreadthFirst<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (childrenSelector == null) throw new ArgumentNullException("childrenSelector");
            return TraverseBreadthFirstImpl(root, childrenSelector);
        }

        private static IEnumerable<T> TraverseBreadthFirstImpl<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count != 0) {
                var current = queue.Dequeue();
                yield return current;
                foreach (var child in childrenSelector(current))
                    queue.Enqueue(child);
            }
        }

        /// <summary>
        /// Traverses a tree starting at the <paramref name="root"/> according to the <paramref name="childrenSelector"/> traversal function.
        /// The returned elements are the result of depth-first traversal.
        /// </summary>
        /// <typeparam name="T">The tree node type</typeparam>
        /// <param name="root">The root of the tree to traverse</param>
        /// <param name="childrenSelector">The function that produces the children of each element</param>
        /// <returns>A sequence containing the traversed values</returns>
        /// <remarks>
        /// This function defers traversal until needed and streams the results.
        /// The progression function is not checked for loops. If you need the resulting sequence to be finite
        /// you must ensure that loops are not produced by <paramref name="childrenSelector"/>
        /// </remarks>
        public static IEnumerable<T> TraverseDepthFirst<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (childrenSelector == null) throw new ArgumentNullException("childrenSelector");
            return TraverseDepthFirstImpl(root, childrenSelector);
        }

        private static IEnumerable<T> TraverseDepthFirstImpl<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                var current = stack.Pop();
                yield return current;
                // because a stack pops the elements out in LIFO order, we need to push them in reverse
                // if we want to traverse the returned list in the same order as was returned to us
                foreach (var child in childrenSelector(current).Reverse())
                    stack.Push(child);
            }
        }
    }
}
