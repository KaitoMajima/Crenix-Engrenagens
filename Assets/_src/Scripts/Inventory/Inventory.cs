using System;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Item> _items;
        [SerializeField] private int _inventoryCapacity = 5;
        public int Capacity {get => _inventoryCapacity; set => _inventoryCapacity = value;}
        public Action<List<Item>> InventoryUpdated;
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
        public void UpdateInventory()
        {
            InventoryUpdated?.Invoke(_items);
        }
        public bool AddItem(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var searchedItem = _items[i];

                if(searchedItem != null)
                    continue;
                
                _items[i] = item;
                return true;
            }
            Debug.LogWarning("O inventário está cheio!");
            return false;
        }
        public bool AddItem(Item item, int index)
        {
            if(_items[index] != null)
            {
                Debug.LogWarning("Já existe um item nesse slot!");
                return false;
            }
            _items[index] = item;
            
            return true;
        }
        
        public void AddItems(List<Item> items)
        {
            foreach (var item in items)
            {
                var hasLimitReached = !(AddItem(item));

                if(hasLimitReached)
                    return;
            }
        }

        public bool RemoveItem(int index)
        {
            _items[index] = null;
            return true;
        }

        public bool SwapItem(Item swappingItem, int originalIndex, int newIndex)
        {
            var swappedItem = _items[newIndex];

            if(swappingItem == swappedItem)
                return false;

            RemoveItem(newIndex);
            RemoveItem(originalIndex);

            AddItem(swappingItem, newIndex);
            AddItem(swappedItem, originalIndex);

            return true;
        }
    }
}
