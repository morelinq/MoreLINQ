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

namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TraverseTest
    {
        [Test]
        public void TraverseDepthFirstFNullGenerator()
        {
            _ = MoreEnumerable.TraverseDepthFirst(new object(), _ => new BreakingSequence<object>());
        }

        [Test]
        public void TraverseBreadthFirstIsStreaming()
        {
            _ = MoreEnumerable.TraverseBreadthFirst(new object(), _ => new BreakingSequence<object>());
        }

        [Test]
        public void TraverseDepthFirstPreservesChildrenOrder()
        {
            var res = MoreEnumerable.TraverseDepthFirst(0, i => i == 0 ? Enumerable.Range(1, 10) : Enumerable.Empty<int>());
            res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }


        [Test]
        public void TraverseBreadthFirstPreservesChildrenOrder()
        {
            var res = MoreEnumerable.TraverseBreadthFirst(0, i => i == 0 ? Enumerable.Range(1, 10) : Enumerable.Empty<int>());
            res.AssertSequenceEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        sealed class Tree<T>
        {
            public T Value { get; }
            public IEnumerable<Tree<T>> Children { get; }

            public Tree(T value, IEnumerable<Tree<T>> children)
            {
                Value = value;
                Children = children;
            }
        }

        static class Tree
        {
            public static Tree<T> New<T>(T value, params Tree<T>[] children) => new(value, children);
        }

        [Test]
        public void TraverseBreadthFirstTraversesBreadthFirst()
        {
            var tree = Tree.New(1,
                Tree.New(2,
                    Tree.New(3)),
                Tree.New(5,
                    Tree.New(6))
            );
            var res = MoreEnumerable.TraverseBreadthFirst(tree, t => t.Children).Select(t => t.Value);
            res.AssertSequenceEqual(1, 2, 5, 3, 6);
        }

        [Test]
        public void TraverseDepthFirstTraversesDepthFirst()
        {
            var tree = Tree.New(1,
                Tree.New(2,
                    Tree.New(3)),
                Tree.New(5,
                    Tree.New(6))
            );
            var res = MoreEnumerable.TraverseDepthFirst(tree, t => t.Children).Select(t => t.Value);
            res.AssertSequenceEqual(1, 2, 3, 5, 6);
        }
    }
}
