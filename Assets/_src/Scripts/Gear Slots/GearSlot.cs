using System;
using UnityEngine;

namespace KaitoMajima
{
    public class GearSlot : MonoBehaviour
    {
        [SerializeField] private SlotDetection _slotDetection;
        [SerializeField] private GameObject _gearPrefab;
        [SerializeField] private Transform _gearSocketTransform;
        private WorldGear _currentGear;
        public Action GearInserted;
        
        private void Start()
        {
            _slotDetection.DropDetected += OnGearDetected;
        }

        public void ResetGear()
        {
            if(_currentGear != null)
                Destroy(_currentGear.gameObject);
            _currentGear = null;

            _slotDetection.Occupied = false;
        }
        private void OnGearDetected(Item item)
        {
            var gearObj = Instantiate(_gearPrefab, _gearSocketTransform);
            _currentGear = gearObj.GetComponent<WorldGear>();

            _currentGear.SetColor(item.spriteColor);

            GearInserted?.Invoke();
        }

        public void IntitiateGearRotation()
        {
            _currentGear.Rotate();
        }
    }
}
