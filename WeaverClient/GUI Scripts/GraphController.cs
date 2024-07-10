using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.GUI_Scripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class GraphController : MonoBehaviour, IDragHandler, IScrollHandler, IBeginDragHandler
    {
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector3 offset;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos);
            offset = rectTransform.position - globalMousePos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (rectTransform == null || canvas == null)
                return;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
            {
                rectTransform.position = globalMousePos + offset;
            }
        }

        public void OnScroll(PointerEventData data)
        {
            float scaleFactor = Mathf.Exp(data.scrollDelta.y / 35.0f);
            Vector3 localScale = rectTransform.localScale * scaleFactor;
            if (localScale.x >= 0.5f && localScale.y >= 0.5f && localScale.x <= 2f && localScale.y <= 2f)
                rectTransform.localScale = localScale;
        }
    }

}
