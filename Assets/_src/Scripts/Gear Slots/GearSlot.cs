using System;
using UnityEngine;

namespace KaitoMajima
{
    public class GearSlot : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private SlotDetection _slotDetection;

        [Header("Prefab Dependencies")]
        [SerializeField] private GameObject _gearPrefab;

        [Header("Scene Dependencies")]
        [SerializeField] private Transform _gearSocketTransform;

        [Header("Settings")]
        [SerializeField] private bool _reverseRotationForGears;
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
            _currentGear.Rotate(_reverseRotationForGears);
        }
    }
}
