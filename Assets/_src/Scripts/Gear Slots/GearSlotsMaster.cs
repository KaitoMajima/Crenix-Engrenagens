using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaitoMajima
{
    public class GearSlotsMaster : MonoBehaviour
    {
        [SerializeField] private List<GearSlot> _gearSlots;
        [SerializeField] private float _rotationInterval = 0.25f;
        private int _insertedGearCount;

        public Action AllInsertSequenceInitiated;
        public Action AllInsertSequenceFinished;

        private void Start()
        {
            foreach (var gearSlot in _gearSlots)
            {
                gearSlot.GearInserted += OnGearInserted;
            }
        }
        private void OnGearInserted()
        {
            _insertedGearCount++;
            if(_insertedGearCount >= _gearSlots.Count)
                StartCoroutine(InitiateRotatingSequence());

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
