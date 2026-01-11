using System.Collections.Generic;
using System;
// Alias for shorter name
using CtfNode = YantraJs.Tests.YantraCtfTest.CtfNode;

namespace YantraJs.Tests;

public static class CtfNodeExtensions
{    
    public static CtfNode YantraClone(this CtfNode root)
    {
        // 1. Handle edge case for null root
        if (root == null) return null;

        // 2. Create a map to track cloned nodes: original > clone
        // Using ReferenceEqualityComparer to ensure we match object instance (not value)
        var map = new Dictionary<CtfNode, CtfNode>(ReferenceEqualityComparer.Instance);

        // 3. Create a stack for iterative Depth-First Search (DFS) to avoid StackOverflowException
        var stack = new Stack<CtfNode>();

        // 4. Start cloning from the root node
        var rootClone = new CtfNode { Data = new List<int>(root.Data ?? []) };
        map[root] = rootClone;
        stack.Push(root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var currentClone = map[current];

            // Function to process neighbors
            void ProcessNeighbor(CtfNode neighbor, Action<CtfNode> linker)
            {
                if (neighbor == null) return;

                if (!map.TryGetValue(neighbor, out var clonedNeighbor))
                {
                    // Create a new clone for the neighbor if not created yet
                    clonedNeighbor = new CtfNode
                    {
                        Data = new List<int>(neighbor.Data ?? [])
                    };
                    map[neighbor] = clonedNeighbor;
                    stack.Push(neighbor);
                }
                // Link the cloned neighbor to the current clone using the provided linker action
                linker(clonedNeighbor);
            }

            // 5. Iteratively process all directions (Parent, Child, Prev, Next)
            ProcessNeighbor(current.Parent, (clone) => currentClone.Parent = clone);
            ProcessNeighbor(current.Child, (clone) => currentClone.Child = clone);
            ProcessNeighbor(current.Prev, (clone) => currentClone.Prev = clone);
            ProcessNeighbor(current.Next, (clone) => currentClone.Next = clone);
        }
        return rootClone;
    }
}
