using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class InventoryFiller : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private List<Item> _objsToFill;

        private void Start()
        {
            Fill();
            _inventory.InventoryResetted += Fill;
        }
        private void Fill()
        {
            _inventory.AddItems(_objsToFill);
            _inventory.UpdateInventory();
        }
    }
}
