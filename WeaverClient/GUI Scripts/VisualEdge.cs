using Assembly_CSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI_Scripts
{
    public enum EdgeType { OneWay, Bidirectional, Reverse }

    public class VisualEdge
    {
        public GameObject edgeObject;
        public EdgeType edgeType;
        private VisualNode node1;
        private VisualNode node2;

        public VisualEdge(VisualNode node1, VisualNode node2, EdgeType type)
        {
            this.node1 = node1;
            this.node2 = node2; // Set start/end nodes
            edgeType = type;

            edgeObject = new GameObject($"Edge_{node1.baseNode.name}<->{node2.baseNode.name}");
            edgeObject.transform.SetParent(node1.nodeObject.transform.parent, false); // Auto get parent transform
            
            LineRenderer lineRenderer = edgeObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false; // prevents it from using the world space positioning
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] { node1.baseNode.position, node2.baseNode.position });

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(node1.baseNode.color, 0.0f), new GradientColorKey(node2.baseNode.color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;

            lineRenderer.startWidth = lineRenderer.endWidth = 5f;  // Adjust width as needed
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // Using an unlit color shader

            lineRenderer.sortingLayerName = "Foreground";  // Ensure this layer exists and is in front of other elements
            lineRenderer.sortingOrder = 5;  // Adjust sorting order as needed
        }
    }
}
