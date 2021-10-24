using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        private Inventory _inventory;

        private InventoryItem _currentItem;
        public InventoryItem CurrentItem {get => _currentItem; set => _currentItem = value;}
        private GameObject _currentItemObj;
        public int index;

        public InventorySlot SetInventory(Inventory inventory)
        {
            _inventory = inventory;
            return this;
        }
        public InventorySlot SetIndex(int index)
        {
            this.index = index;
            return this;
        }

        public InventorySlot SetItem(InventoryItem itemObj)
        {
            DestroyCurrentItem();
            InstantiateNewItem(itemObj);

            return this;
        }
        public void OnDrop(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;

            if(dragObj == null)
                return;
            
            if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
                return;

            var item = itemInstance.currentSlot.CurrentItem;
            var instanceIndex = itemInstance.currentSlot.index;

            _inventory.SwapItem(item, instanceIndex, index);
        }

        private void InstantiateNewItem(InventoryItem item)
        {
            if (item != null)
            {
                var itemInstance = Instantiate(item, transform, false);
                itemInstance.currentSlot = this;

                _currentItem = item;
                _currentItemObj = itemInstance.gameObject;
            }
        }

        private void DestroyCurrentItem()
        {
            _currentItem = null;
            
            if (_currentItemObj != null)
                Destroy(_currentItemObj);
        }

    }
}
