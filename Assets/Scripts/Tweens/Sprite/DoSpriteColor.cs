using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Tweens.Sprite
{
    public class DoSpriteColor : SpriteTweener<Color>
    {
        public override void Play(Color target) => spriteRenderer.DOColor(target, Duration).SetEase(Ease);
    }
}