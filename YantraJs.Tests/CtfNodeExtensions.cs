using System.Collections.Generic;
using System;

namespace YantraJs.Tests;

public static class CtfNodeExtensions
{
    public static YantraCtfTest.CtfNode YantraClone(this YantraCtfTest.CtfNode root)
    {   
        /// 1. handle null case
        if (root == null) return null;


        /// 2. create a map to track cloned nodes: original > clone
        var map = new Dictionary<YantraCtfTest.CtfNode, YantraCtfTest.CtfNode>(ReferenceEqualityComparer.Instance);

        /// 3. create a stack for iterative DFS
        var stack = new Stack<YantraCtfTest.CtfNode>();

        /// 4. start the DFS from the root node
        var rootClone = new YantraCtfTest.CtfNode{Data = new List<int>(root.Data ?? [])};
        map[root] = rootClone;
        stack.Push(root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var currentClone = map[current];

            // processing of neighbors
            void ProcessNeighbor(YantraCtfTest.CtfNode neighbor, Action<YantraCtfTest.CtfNode> linker)
            {
                if (neighbor == null) return;

                if (!map.TryGetValue(neighbor, out var clonedNeighbor))
                {
                    // create a new clone for the neighbor
                    clonedNeighbor = new YantraCtfTest.CtfNode
                    {
                        Data = new List<int>(neighbor.Data ?? [])
                    };
                    map[neighbor] = clonedNeighbor;
                    stack.Push(neighbor);
                }
                // set the cloned neighbor in the current clone
                linker(clonedNeighbor);

            }

            /// 6. recursively clone the children
            ProcessNeighbor(current.Parent, (clone) => currentClone.Parent = clone);
            ProcessNeighbor(current.Child, (clone) => currentClone.Child = clone);
            ProcessNeighbor(current.Prev, (clone) => currentClone.Prev = clone);
            ProcessNeighbor(current.Next, (clone) => currentClone.Next = clone);
        }
        return rootClone;
    }
}
