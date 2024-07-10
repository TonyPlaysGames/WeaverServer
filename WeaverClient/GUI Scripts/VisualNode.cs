using Assets.GUI_Scripts;
using WeaverClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WeaverClient;

namespace Assembly_CSharp
{
    public class VisualNode
    {
        public NetworkNode baseNode;
        public GameObject nodeObject;
        public Text nodeText;
        public Dictionary<NetworkNode, VisualEdge> edges = new Dictionary<NetworkNode, VisualEdge>();

        public VisualNode(NetworkNode baseNode, Sprite circleSprite, Transform parent)
        {
            this.baseNode = baseNode;

            nodeObject = new GameObject(baseNode.name);
            nodeObject.transform.SetParent(parent, false);
            nodeObject.transform.localPosition = new Vector3(baseNode.position.x, baseNode.position.y, 0);

            var image = nodeObject.AddComponent<Image>();
            image.sprite = circleSprite;
            image.color = baseNode.color;

            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(nodeObject.transform, false);
            RectTransform rect = textObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 50); // Example size, adjust as needed
            rect.localPosition = Vector3.zero;

            nodeText = textObject.AddComponent<Text>();
            nodeText.text = baseNode.name;
            nodeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nodeText.alignment = TextAnchor.MiddleCenter;
        }

        public void SetNodeColor(Color color)
        {
            nodeObject.GetComponent<Image>().color = color;
        }

        public void SetNodeText(string text)
        {
            nodeText.text = text;
        }
    }
}