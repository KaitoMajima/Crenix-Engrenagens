using System;
using UnityEngine;

namespace KaitoMajima
{
    [CreateAssetMenu(fileName = "New Item", menuName = "KaitoMajima/Item")]
    public class Item : ScriptableObject
    {
        public Sprite itemSprite;
        public Color spriteColor = Color.white;
    }
}
