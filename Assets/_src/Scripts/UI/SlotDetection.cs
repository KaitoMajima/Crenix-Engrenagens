using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class SlotDetection : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        private bool _occupied;
        public bool Occupied {get => _occupied; set => _occupied = value;}
        public Action<Item> DropDetected;
        public void OnDrop(PointerEventData eventData)
        {
            if(_occupied)
                return;
            
            var dragObj = eventData.pointerDrag;
            if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
                return;
            
            var item = itemInstance.Item;
            DropDetected?.Invoke(item);

            itemInstance.RemoveFromInventory();
            _occupied = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;
            if(dragObj == null)
                return;   
        }
    }
}
