using System.Collections;
using Assets.Scripts.Tweens;
using Assets.Scripts.Tweens.Sprite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneManagement
{
    public class FadeTransition : SceneTransition
    {
        public Tweener<Color> spriteColorTweener = null;
        public override IEnumerator Transition(int sceneIndex)
        {
            spriteColorTweener.Play();
            yield return new WaitForSecondsRealtime(spriteColorTweener.Duration);
            spriteColorTweener.Stop();
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
