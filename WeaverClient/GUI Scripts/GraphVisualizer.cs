using Assembly_CSharp;
using Assets.GUI_Scripts;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WeaverClient;

public class GraphVisualizer : MonoBehaviour
{
    public GameObject graphContainer;
    public Camera mainCamera;
    public Sprite nodeSprite;
    private VisualTree visualTree;

    private bool graphIsVisible = true;

    void Start()
    {
        NetworkTree tree = NetworkTree.GenerateTestTree();
        Create(tree);
    }

    public void Create(NetworkTree tree)
    {
        visualTree = new VisualTree(tree, nodeSprite, graphContainer.transform);
        ShowVisibleNodes();
    }

    public void ToggleVisibility()
    {
        if (graphIsVisible) {
            HideAllNodes();
            graphIsVisible = false;
        }
        else
        {
            ShowVisibleNodes();
            graphIsVisible = true;
        }
    }

    // Displays visible nodes
    private void ShowVisibleNodes()
    {
        if (!graphIsVisible) return;

        foreach (VisualNode visualNode in visualTree.visual_nodes)
        {
            if (visualNode.baseNode.is_visible)
            {
                visualNode.nodeObject.SetActive(true);
                foreach (VisualEdge edge in visualNode.edges.Values)
                {
                    edge.edgeObject.SetActive(true);
                }
            }
        }
    }

    // Hides all nodes
    private void HideAllNodes()
    {
        foreach (VisualNode visualNode in visualTree.visual_nodes)
        {
            visualNode.nodeObject.SetActive(false);
            foreach (VisualEdge edge in visualNode.edges.Values)
            {
                edge.edgeObject.SetActive(false);
            }
        }
    }

    // Function to be called each tick to refresh nodes
    void Update()
    {
        visualTree.UpdateNew();
    }

    public void AddOneNode()
    {
        NetworkNode newNode = visualTree.parentTree.AddOneNode();
        if (newNode != null)
        {
            visualTree.UpdateNew();
        }
    }
}
