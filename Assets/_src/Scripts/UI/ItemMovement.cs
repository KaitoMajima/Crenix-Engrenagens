using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class ItemMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _movingTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Vector2 _defaultPosition;
        private Transform _originalTransform;
        public Transform draggingObjectsTransform;

        private void Awake()
        {
            _originalTransform = _movingTransform.parent;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;

            _movingTransform.SetParent(draggingObjectsTransform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _movingTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            _movingTransform.SetParent(_originalTransform);
            _movingTransform.anchoredPosition = _defaultPosition;

        }
    }
}
