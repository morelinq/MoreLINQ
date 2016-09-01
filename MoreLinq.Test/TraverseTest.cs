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

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TraverseTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraverseBreadthFirstNullGenerator()
        {
            MoreEnumerable.TraverseBreadthFirst(new object(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraverseDepthFirstNullGenerator()
        {
            MoreEnumerable.TraverseDepthFirst(new object(), null);
        }

        [Test]
        public void TraverseDepthFirstFNullGenerator()
        {
            MoreEnumerable.TraverseDepthFirst(new object(), o => new BreakingSequence<object>());
        }

        [Test]
        public void TraverseBreadthFirstIsStreaming()
        {
            MoreEnumerable.TraverseBreadthFirst(new object(), o => new BreakingSequence<object>());
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

        private class Tree<T>
        {
            public T Value { get; set; }
            public ICollection<Tree<T>> Children { get; private set; }

            public Tree()
            {
                Children = new List<Tree<T>>();
            }
        }

        [Test]
        public void TraverseBreadthFirstTraversesBreadthFirst()
        {
            var tree = new Tree<int> {
                Value = 1,
                Children =  {
                    new Tree<int> {
                        Value = 2,
                        Children = {
                            new Tree<int> { Value = 3 },
                        }
                    },
                    new Tree<int> {
                        Value = 5,
                        Children = {
                            new Tree<int> { Value = 6 },
                        }
                    }
                }
            };
            var res = MoreEnumerable.TraverseBreadthFirst(tree, t => t.Children).Select(t => t.Value);
            res.AssertSequenceEqual(1, 2, 5, 3, 6);
        }

        [Test]
        public void TraverseDepthFirstTraversesDepthFirst()
        {
            var tree = new Tree<int> {
                Value = 1,
                Children =  {
                    new Tree<int> {
                        Value = 2,
                        Children = {
                            new Tree<int> { Value = 3 },
                        }
                    },
                    new Tree<int> {
                        Value = 5,
                        Children = {
                            new Tree<int> { Value = 6 },
                        }
                    }
                }
            };
            var res = MoreEnumerable.TraverseDepthFirst(tree, t => t.Children).Select(t => t.Value);
            res.AssertSequenceEqual(1, 2, 3, 5, 6);
        }
    }
}
