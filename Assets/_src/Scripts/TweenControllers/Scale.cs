using UnityEngine;
using DG.Tweening;

namespace KaitoMajima
{
    public class Scale : TweenController
    {
        [SerializeField] private Transform _tweeningTransform;

        private Vector3 _originalScale;
        [SerializeField] private Vector3 _scaleTarget;

        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);
            
            _originalScale = _tweeningTransform.localScale;

            Tween tween = _tweeningTransform.DOScale(_scaleTarget, tweenSettings.duration);

            if(tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOScale(_scaleTarget, tweenSettings.duration).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = _tweeningTransform.DOScale(_originalScale, tweenSettings.duration);

            if(tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _tweeningTransform.DOScale(_originalScale, tweenSettings.duration).From();

            return tween;
        }

    }
}