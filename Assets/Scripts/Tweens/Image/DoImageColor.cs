using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Tweens.Image
{
    public class DoImageColor : ImageTweener<Color>
    {
        public override void Play(Color target) => imageRenderer.DOColor(target, Duration).SetEase(Ease);
    }
}