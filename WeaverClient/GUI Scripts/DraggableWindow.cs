using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    Vector3 MouseDragStartPos;
    public RectTransform parentRectTransform;
    public PointerEventData.InputButton dragMouseButton;
    public Canvas canvas;

    private void Awake()
    {
        if (parentRectTransform == null)
            parentRectTransform = transform.parent.GetComponent<RectTransform>();

        if (canvas == null)
        {

            Transform testCanvasTransform = transform.parent;
            while (testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if (canvas != null)
                {
                    break;
                }
                testCanvasTransform = testCanvasTransform.parent;
            }
        }
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.button == dragMouseButton)
        {
            parentRectTransform.position = Input.mousePosition - MouseDragStartPos;
            TrapToScreen();
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentRectTransform.SetAsLastSibling(); // Move to top
        if (eventData.button == dragMouseButton)
            MouseDragStartPos = Input.mousePosition - parentRectTransform.position;
    }

    private void TrapToScreen()
    {
        // Get the corners of the RectTransform in world space
        Vector3[] corners = new Vector3[4];
        parentRectTransform.GetWorldCorners(corners);

        // Convert corners to screen space
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[i]);
        }

        // Calculate the min and max values from the corners
        float minX = corners[0].x; // Bottom left corner
        float maxX = corners[2].x; // Top right corner
        float minY = corners[0].y; // Bottom left corner
        float maxY = corners[2].y; // Top right corner

        // Get the current position in screen space
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, parentRectTransform.position);

        // Calculate the offset necessary to keep the window within the screen bounds
        if (maxX > Screen.width)
            screenPosition.x -= (maxX - Screen.width);
        if (minX < 0)
            screenPosition.x -= minX;
        if (maxY > Screen.height)
            screenPosition.y -= (maxY - Screen.height);
        if (minY < 0)
            screenPosition.y -= minY;

        // Convert the adjusted screen position back to world space and apply it
        parentRectTransform.position = RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRectTransform, screenPosition, canvas.worldCamera, out var worldPoint) ? worldPoint : parentRectTransform.position;
    }
}
