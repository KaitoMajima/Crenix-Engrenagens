using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class GearSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;

            Debug.Log($"You have dropped the {dragObj.name} onto the slot!");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;
            if(dragObj == null)
                return;
            
            Debug.Log(dragObj.name + " is entering the area of the slot.");
        }
    }
}
