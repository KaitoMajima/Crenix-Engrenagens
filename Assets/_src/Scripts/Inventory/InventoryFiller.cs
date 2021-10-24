using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class InventoryFiller : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        
        [SerializeField] private List<GameObject> _objsToFill;
        private List<InventoryItem> _itemsToFill;
        private void Start()
        {
            _itemsToFill = new List<InventoryItem>();

            foreach (var obj in _objsToFill)
            {
                if(!obj.TryGetComponent(out InventoryItem item))
                    return;
                
                
                _itemsToFill.Add(item);
            }
            _inventory.AddItems(_itemsToFill);
        }
    
    }
}
