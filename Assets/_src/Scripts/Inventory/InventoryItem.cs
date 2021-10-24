using UnityEngine;

namespace KaitoMajima
{
    public class InventoryItem : MonoBehaviour
    {
        private InventorySlot _currentSlot;
        public InventorySlot CurrentSlot {get => _currentSlot;}
        private Inventory _inventory;
        public InventoryItem SetInventory(Inventory inventory)
        {
            _inventory = inventory;
            return this;
        }

        public InventoryItem SetSlot(InventorySlot slot)
        {
            _currentSlot = slot;
            return this;
        }

        public void RemoveFromInventory()
        {
            _inventory.RemoveItem(_currentSlot.index);
            Destroy(gameObject);
        }
    }
}
