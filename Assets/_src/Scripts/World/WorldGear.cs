using UnityEngine;

namespace KaitoMajima
{
    public class WorldGear : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _gearSpriteRenderer;
        [SerializeField] private Rotate _rotateTween;
        public WorldGear SetColor(Color color)
        {
            _gearSpriteRenderer.color = color;
            return this;
        }
        
        public void Rotate()
        {
            _rotateTween.CallTween();
        }
    }
}
