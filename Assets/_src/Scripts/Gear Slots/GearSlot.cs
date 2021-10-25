using System;
using UnityEngine;

namespace KaitoMajima
{
    public class GearSlot : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private SlotDetection _slotDetection;
        [SerializeField] private RemovalDetection _removalDetection;

        [Header("Prefab Dependencies")]
        [SerializeField] private GameObject _gearPrefab;

        [Header("Scene Dependencies")]
        [SerializeField] private Transform _gearSocketTransform;

        [Header("Settings")]
        [SerializeField] private bool _reverseRotationForGears;
        private WorldGear _currentGear;
        public Action GearInserted;
        public Action<Item> GearRemoved;
        
        private void Start()
        {
            _slotDetection.DropDetected += OnGearDetected;
            _removalDetection.RemoveCalled += OnRemoveGearCalled;
        }

        private void OnRemoveGearCalled()
        {
            if(_currentGear == null)
                return;
            var item = _currentGear.item;

            GearRemoved?.Invoke(item);
            ResetGear();
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

            _currentGear.SetItem(item).SetColor(item.spriteColor);

            GearInserted?.Invoke();
        }

        public void IntitiateGearRotation()
        {
            _currentGear.Rotate(_reverseRotationForGears);
        }

        public void StopGearRotation()
        {
            if(_currentGear == null)    
                return;
            
            _currentGear.StopRotating();
        }
    }
}
