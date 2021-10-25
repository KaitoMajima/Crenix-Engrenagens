using UnityEngine;
using UnityEngine.UI;

namespace KaitoMajima
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GearSlotsMaster _gearSlotsMaster;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Nugget _nugget;

        private void Start()
        {
            _button.onClick.AddListener(() => ResetGame());
        }
        public void ResetGame()
        {
            _gearSlotsMaster.ResetGears();
            _inventory.ResetInventory();
            _inventory.UpdateInventory();
            _nugget.TriggerInitialState();
        }
    }
}
