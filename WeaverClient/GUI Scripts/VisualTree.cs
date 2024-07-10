using Assembly_CSharp;
using System.Collections.Generic;
using UnityEngine;
using WeaverClient;

namespace Assets.GUI_Scripts
{
    public class VisualTree
    {
        public List<VisualNode> visual_nodes = new List<VisualNode>();
        public List<VisualEdge> visual_edges = new List<VisualEdge>();
        public NetworkTree parentTree;

        private Transform parent;
        private Sprite sprite;

        public VisualTree(NetworkTree parentTree, Sprite nodeSprite, Transform parent)
        {
            this.parentTree = parentTree;
            this.parent = parent;
            this.sprite = nodeSprite;

            Dictionary<NetworkNode, VisualNode> createdNodes = new Dictionary<NetworkNode, VisualNode>();

            foreach (NetworkNode baseNode in parentTree.nodes)
            {
                VisualNode visualNode = new VisualNode(baseNode, nodeSprite, parent);
                createdNodes[baseNode] = visualNode;
                visual_nodes.Add(visualNode);
            }

            foreach (NetworkNode baseNode in parentTree.nodes)
            {
                VisualNode visualNode = createdNodes[baseNode];
                foreach (NetworkNode neighbor in baseNode.neighbors)
                {
                    if (!visualNode.edges.ContainsKey(neighbor))
                    {
                        VisualEdge edge = new VisualEdge(visualNode, createdNodes[neighbor], EdgeType.Bidirectional);
                        visualNode.edges.Add(neighbor, edge);
                        visual_edges.Add(edge);
                    }
                }
            }
        }

        public void UpdateNew()
        {
            Dictionary<NetworkNode, VisualNode> existingNodes = new Dictionary<NetworkNode, VisualNode>();
            foreach (VisualNode vn in visual_nodes)
            {
                existingNodes[vn.baseNode] = vn;
            }

            foreach (NetworkNode baseNode in parentTree.nodes)
            {
                if (!existingNodes.ContainsKey(baseNode))
                {
                    VisualNode visualNode = new VisualNode(baseNode, sprite, parent);
                    visual_nodes.Add(visualNode);
                    existingNodes[baseNode] = visualNode;
                }
            }

            foreach (VisualNode visualNode in visual_nodes)
            {
                foreach (NetworkNode neighbor in visualNode.baseNode.neighbors)
                {
                    if (!visualNode.edges.ContainsKey(neighbor))
                    {
                        VisualEdge edge = new VisualEdge(visualNode, existingNodes[neighbor], EdgeType.Bidirectional);
                        visualNode.edges.Add(neighbor, edge);
                        visual_edges.Add(edge);
                    }
                }
            }
        }



    }
}
