using DG.Tweening;
using UnityEngine;

namespace KaitoMajima
{
    public class SpriteFade : TweenController
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _originalFadeValue;
        [SerializeField] [Range(0,1)] private float _fadeEndValue;
        protected override Tween TriggerTween()
        {
            _mainTween.Kill(true);
            
            _originalFadeValue = _spriteRenderer.color.a;

            Tween tween = _spriteRenderer.DOFade(_fadeEndValue, tweenSettings.duration);

            if(tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _spriteRenderer.DOFade(_fadeEndValue, tweenSettings.duration).From();

            return tween;
        }

        protected override Tween RevertTween()
        {
            _mainTween.Kill(true);

            Tween tween = _spriteRenderer.DOFade(_originalFadeValue, tweenSettings.duration);

            if(tweenSettings.tweenOrientation == TweenSettings.TweenOrientation.From)
                tween = _spriteRenderer.DOFade(_originalFadeValue, tweenSettings.duration).From();

            return tween;
        }
    }
}