using System;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> _items;
        [SerializeField] private int _inventoryCapacity = 5;
        public int Capacity {get => _inventoryCapacity; set => _inventoryCapacity = value;}
        public Action<List<InventoryItem>> InventoryUpdated;
        private void Awake()
        {
            InitiateItemList();
        }

        private void InitiateItemList()
        {
            while (_items.Count < _inventoryCapacity)
            {
                _items.Add(null);
            }
            
        }
        public bool AddItem(InventoryItem itemObj, bool updateInventory = true)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];

                if(item != null)
                    continue;
                
                _items[i] = itemObj;

                if(updateInventory)
                    InventoryUpdated?.Invoke(_items);
                
                return true;
            }
            Debug.LogWarning("O inventário está cheio!");
            return false;
        }
        
        public void AddItems(List<InventoryItem> itemObjs, bool updateInventory = true)
        {
            foreach (var item in itemObjs)
            {
                var hasLimitReached = !(AddItem(item, false));

                if(hasLimitReached)
                    return;
            }

            if(updateInventory)
                InventoryUpdated?.Invoke(_items);
        }
        public bool AddItem(InventoryItem item, int index, bool updateInventory = true)
        {
            if(_items[index] != null)
            {
                Debug.LogWarning("Já existe um item nesse slot!");
                return false;
            }
            
            _items[index] = item;

            if(updateInventory)
                InventoryUpdated?.Invoke(_items);
            
            return true;
        }

        public bool RemoveItem(int index, bool updateInventory = true)
        {
            _items[index] = null;

            if(updateInventory)
                InventoryUpdated?.Invoke(_items);
            
            return true;
        }

        public bool SwapItem(InventoryItem swappingItem, int originalIndex, int newIndex, bool updateInventory = true)
        {
            var swappedItem = _items[newIndex];

            if(swappingItem == swappedItem)
                return false;

            Debug.Log(swappingItem);

            RemoveItem(newIndex, false);
            RemoveItem(originalIndex, false);

            AddItem(swappingItem, newIndex, false);
            AddItem(swappedItem, originalIndex, false);

            if(updateInventory)
                InventoryUpdated?.Invoke(_items);

            return true;
        }
    }
}
