using System;
using DG.Tweening;
using UnityEngine;

namespace KaitoMajima
{
    public abstract class TweenController : MonoBehaviour
    {
        public TweenSettings tweenSettings = TweenSettings.Default;

        protected Tween _mainTween;
        public Action onTweenFinished;
        
        #region Initialize Methods
        private void Awake()
        {
            if(tweenSettings.initializeMethod == TweenSettings.InitializeMethod.Awake)
                CallTween();
        }
        private void Start()
        {
            if(tweenSettings.initializeMethod == TweenSettings.InitializeMethod.Start)
                CallTween();


        }
        private void OnEnable()
        {
            if(tweenSettings.initializeMethod == TweenSettings.InitializeMethod.OnEnable)
                CallTween();


        }
        private void OnDisable()
        {
            if(tweenSettings.initializeMethod == TweenSettings.InitializeMethod.OnDisable)
                CallTween();

        }
        #endregion

        public void CallTween(bool reverse = false)
        {
            if(!reverse)
                _mainTween = ApplyTweenSettings(TriggerTween());
            else
                _mainTween = ApplyTweenSettings(RevertTween());
        }
        public void KillTween(bool complete = false)
        {
            _mainTween.Kill(complete);
        }
        protected virtual Tween TriggerTween()
        {
            return null;
        }
        protected virtual Tween RevertTween()
        {
            return null;
        }
        protected virtual Tween ApplyTweenSettings(Tween tween)
        {
            tween.
                SetEase(tweenSettings.easeType).
                SetLoops(tweenSettings.loopAmount, tweenSettings.loopType).
                SetDelay(tweenSettings.delay).
                SetUpdate(tweenSettings.updateType, tweenSettings.ignoreTimeScale).
                SetRelative(tweenSettings.isRelative).
                SetInverted(tweenSettings.isInverted).
                OnComplete(() => onTweenFinished?.Invoke());

 
            return tween;
        }

    }

    [Serializable]
    public struct TweenSettings
    {
        public enum InitializeMethod
        {
            None,
            OnEnable,
            Awake,
            Start,
            OnDisable
        }

        public enum TweenOrientation

        {
            To,
            From
        }
      
        [Header("Settings")]
        public InitializeMethod initializeMethod;
        public TweenOrientation tweenOrientation;

        [Header("Update Types")]
        public UpdateType updateType;
        public bool ignoreTimeScale;

        [Header("Loops")]
        public int loopAmount;
        public LoopType loopType;

        [Header("Misc")]
        public bool isRelative;
        public bool isInverted;

        [Header("Values")]
        public float duration;
        public float delay;
        public Ease easeType;

        public static TweenSettings Default = new TweenSettings()
        {
            duration = 1,
            easeType = Ease.InOutQuad
        };
    }
}