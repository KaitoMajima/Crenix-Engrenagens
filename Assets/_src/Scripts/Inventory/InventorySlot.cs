using UnityEngine;
using UnityEngine.EventSystems;

namespace KaitoMajima
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [Header("Prefab Dependencies")]
        [SerializeField] private GameObject _defaultItemPrefab;
        private Inventory _inventory;
        private InventoryItem _defaultItemInstance;
        [HideInInspector] public int index;

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
        public InventorySlot Initialize(Transform draggingObjectsTransform)
        {
            var itemInstanceObj = Instantiate(_defaultItemPrefab, transform);

            if(itemInstanceObj.TryGetComponent(out ItemMovement itemMovement))
                itemMovement.draggingObjectsTransform = draggingObjectsTransform;

            _defaultItemInstance = itemInstanceObj.GetComponent<InventoryItem>();

            return this;
        }
        public InventorySlot SetItem(Item item)
        {
            ChangeItem(item);

            return this;
        }
        public void OnDrop(PointerEventData eventData)
        {
            var dragObj = eventData.pointerDrag;

            if(dragObj == null)
                return;
            
            if(!dragObj.TryGetComponent(out InventoryItem itemInstance))
                return;

            var item = itemInstance.Item;
            var instanceIndex = itemInstance.CurrentSlot.index;

            _inventory.SwapItem(item, instanceIndex, index);
            _inventory.UpdateInventory();
        }
        private void ChangeItem(Item item)
        {
            _defaultItemInstance.SetInventory(_inventory)
                    .SetSlot(this)
                    .SetInfo(item);
        }
    }
}
