using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class RemovalDetection : MonoBehaviour, IPointerClickHandler
    {
        public Action RemoveCalled;
        public void OnPointerClick(PointerEventData eventData)
        {
            RemoveCalled?.Invoke();
        }
    }
}
