using Assembly_CSharp;
using Assets.GUI_Scripts;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace WeaverClient
{
    public class NetworkNode
    {
        public bool is_visible;
        public string name;
        public int spawn_radius = 10;
        public Vector2 position;
        public Color color;
        public List<NetworkNode> neighbors;
    }

    public class NetworkTree
    {
        public List<NetworkNode> nodes;
        public Rect bounds;

        public static NetworkTree GenerateTestTree()
        {
            NetworkTree testTree = new NetworkTree
            {
                nodes = new List<NetworkNode>(),
                bounds = new Rect(300, 300, 200, 200)  // Example bounds
            };

            // Create 5 test nodes
            for (int i = 0; i < 5; i++)
            {
                bool nodeAdded = false;
                while (!nodeAdded)
                {
                    NetworkNode newNode = new NetworkNode
                    {
                        position = new Vector2(Random.Range(-200f, 200f), Random.Range(-200f, 200f)),
                        neighbors = new List<NetworkNode>(),
                        is_visible = true,
                        name = $"Node {i}",
                        color = new Color(Random.value, Random.value, Random.value, 1.0f) // Random default color
                    };
                    nodeAdded = testTree.TryAddNode(newNode);
                }
            }

            // Establish connections between nodes (simple chain for demonstration)
            for (int i = 0; i < testTree.nodes.Count - 1; i++)
            {
                testTree.nodes[i].neighbors.Add(testTree.nodes[i + 1]);
                testTree.nodes[i + 1].neighbors.Add(testTree.nodes[i]);  // For bidirectional edge
            }

            return testTree;
        }

        public bool TryAddNode(NetworkNode newNode)
        {
            foreach (NetworkNode node in nodes)
            {
                if (Vector2.Distance(node.position, newNode.position) < Mathf.Max(node.spawn_radius, newNode.spawn_radius))
                {
                    return false;  // Overlapping detected
                }
            }

            nodes.Add(newNode);
            return true;
        }

        public NetworkNode AddOneNode()
        {
            int tries = 0;
            int nodenumber = nodes.Count;
            // Try a maximum of 100 times
            while (tries < 100)
            {
                NetworkNode test = new NetworkNode
                {
                    position = new Vector2(Random.Range(-200f, 200f), Random.Range(-200f, 200f)),
                    neighbors = new List<NetworkNode>(),
                    is_visible = true,
                    name = $"Node {nodenumber}",
                    color = new Color(Random.value, Random.value, Random.value, 1.0f) // Random default color
                };
                test.neighbors.Add(nodes[nodenumber - 1]);
                nodes[nodenumber - 1].neighbors.Add(test); // Add edges to one other

                if (TryAddNode(test)) return test;
                tries++;
            }

            return null;
        }


    }
}