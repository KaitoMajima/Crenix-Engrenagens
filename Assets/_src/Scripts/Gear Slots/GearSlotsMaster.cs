using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class GearSlotsMaster : MonoBehaviour
    {
        [Header("Local Dependencies")]
        [SerializeField] private List<GearSlot> _gearSlots;

        [Header("Scene Dependencies")]
        [SerializeField] private Inventory _inventory;

        [Header("Settings")]
        [SerializeField] private float _rotationInterval = 0.25f;
        private int _insertedGearCount;
        private Coroutine _rotatingSequence;
        public Action AllInsertSequenceInitiated;
        public Action AllInsertSequenceFinished;

        private void Start()
        {
            foreach (var gearSlot in _gearSlots)
            {
                gearSlot.GearInserted += OnGearInserted;
                gearSlot.GearRemoved += OnGearRemoved;
            }
        }

        

        public void ResetGears()
        {
            if(_rotatingSequence != null)
                StopCoroutine(_rotatingSequence);

            _insertedGearCount = 0;
            foreach (var gearSlot in _gearSlots)
            {
                gearSlot.ResetGear();
            }

        }
        private void OnGearInserted()
        {
            _insertedGearCount++;
            if(_insertedGearCount >= _gearSlots.Count)
                _rotatingSequence = StartCoroutine(InitiateRotatingSequence());

        }

        private void OnGearRemoved(Item item)
        {
            _insertedGearCount--;
            
            if(_rotatingSequence != null)
                StopCoroutine(_rotatingSequence);
            
            _inventory.AddItem(item);
            _inventory.UpdateInventory();

            foreach (var slot in _gearSlots)
            {
                slot.StopGearRotation();
            }
        }
        private IEnumerator InitiateRotatingSequence()
        {
            AllInsertSequenceInitiated?.Invoke();
            foreach (var gear in _gearSlots)
            {
                gear.IntitiateGearRotation();
                yield return new WaitForSeconds(_rotationInterval);
            }
            AllInsertSequenceFinished?.Invoke();
        }
    }
}
