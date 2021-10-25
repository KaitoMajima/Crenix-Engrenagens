using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KaitoMajima
{
    public class Nugget : MonoBehaviour
    {
        [SerializeField] private GearSlotsMaster _gearSlotsMaster;
        [SerializeField] private TextMeshProUGUI _bubbleText;
        [SerializeField] private List<TweenController> _happyAnimationTweens;

        private Vector3 _initialLocalScale;
        private Vector3 _initialPosition;

        [Multiline(2)] [SerializeField] private string _initialText;
        [Multiline(2)] [SerializeField] private string _waitingText;
        [Multiline(2)] [SerializeField] private string _finishedText;

        public enum NuggetState
        {
            Initial,
            Waiting,
            Finished
        }
        private NuggetState _nuggetState = NuggetState.Initial;
        private void Start()
        {
            _initialLocalScale = transform.localScale;
            _initialPosition = transform.position;

            _bubbleText.SetText(_initialText);
            _gearSlotsMaster.AllInsertSequenceInitiated += TriggerWaitState;
            _gearSlotsMaster.AllInsertSequenceFinished += TriggerFinishedState;
        }
        public void TriggerInitialState() => SwitchState(NuggetState.Initial);
        public void TriggerWaitState() => SwitchState(NuggetState.Waiting);
        public void TriggerFinishedState() => SwitchState(NuggetState.Finished);
        private void SwitchState(NuggetState state)
        {
            _nuggetState = state;

            switch (_nuggetState)
            {
                case NuggetState.Initial:

                    foreach (var tween in _happyAnimationTweens)
                    {
                        tween.KillTween(true);
                    }
                    transform.localScale = _initialLocalScale;
                    transform.position = _initialPosition;

                    _bubbleText.SetText(_initialText);
                    break;
                case NuggetState.Waiting:

                    _bubbleText.SetText(_waitingText);
                    break;
                case NuggetState.Finished:

                    _bubbleText.SetText(_finishedText);
                    foreach (var tween in _happyAnimationTweens)
                    {
                        tween.CallTween(false);
                    }
                    break;
            }
        }
    }
}
