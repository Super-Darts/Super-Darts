using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Tweens.Sprite
{
    public abstract class SpriteTweener<T> : Tweener<T>
    {
        [HideInInspector] public SpriteRenderer spriteRenderer;

        public override void Stop() => DOTween.Kill(spriteRenderer);

        private void OnDestroy() => Stop();

        public void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();
    }
}