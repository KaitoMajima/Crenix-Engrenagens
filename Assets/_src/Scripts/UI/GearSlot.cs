using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class GearSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;

            if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
                return;
            
            itemInstance.RemoveFromInventory();

            Debug.Log($"Você dropou o {dragObj.name} no encaixe!");
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;
            if(dragObj == null)
                return;
            
            Debug.Log(dragObj.name + " está entrando na área do encaixe.");
        }
    }
}
