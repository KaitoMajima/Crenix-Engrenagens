using System;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private Inventory _inventory;

        [Header("Scene Dependencies")]
        [SerializeField] private Transform _slotsTransform;
        [SerializeField] private Transform _draggingObjectsTransform;

        [Header("Prefab Dependencies")]
        [SerializeField] private GameObject _slotPrefab;
        private List<InventorySlot> _currentInventorySlots;

        private void Awake()
        {
            InitializeSlots();
            _inventory.InventoryUpdated += OnInventoryUpdated;
        }
        private void InitializeSlots()
        {
            _currentInventorySlots = new List<InventorySlot>();

            for (int i = 0; i < _inventory.Capacity; i++)
            {
                var slotObj = Instantiate(_slotPrefab, _slotsTransform);
                var slot = slotObj.GetComponent<InventorySlot>();

                slot.SetInventory(_inventory).
                    SetIndex(i).
                    Initialize(_draggingObjectsTransform);

                _currentInventorySlots.Add(slot);
            }
        }
        private void OnInventoryUpdated(List<Item> items)
        {
            for (int i = 0; i < _currentInventorySlots.Count; i++)
            {
                var slot = _currentInventorySlots[i];
                
                slot.SetItem(items[i]);
                
            }
        }
    
    }
}
