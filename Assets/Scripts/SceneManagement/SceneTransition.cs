using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneManagement
{
    public abstract class SceneTransition : MonoBehaviour
    {
        public void Run(Scene scene) => StartCoroutine(Transition(scene.buildIndex));
        public void Run(int sceneIndex) => StartCoroutine(Transition(sceneIndex));
        public abstract IEnumerator Transition(int sceneIndex);
    }
}
