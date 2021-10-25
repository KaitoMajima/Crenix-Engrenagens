using UnityEngine;
using UnityEngine.UI;

namespace KaitoMajima
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _spriteImage;
        private Item _item;
        public Item Item {get => _item;}
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

        public InventoryItem SetInfo(Item item)
        {
            _item = item;

            if(item == null)
            {
                _spriteImage.enabled = false;
                return this;
            }
            _spriteImage.enabled = true;
            _spriteImage.sprite = item.itemSprite;
            _spriteImage.color = item.spriteColor;

            return this;
        }
        public void RemoveFromInventory()
        {
            _inventory.RemoveItem(_currentSlot.index);
            _inventory.UpdateInventory();

            SetInfo(null);
        }
    }
}
