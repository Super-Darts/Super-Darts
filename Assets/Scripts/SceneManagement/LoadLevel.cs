/*
 *  Author: Lewis Comstive
 *  Usage: Used to load and reload scenes
 */

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.SceneManagement
{
    /// <summary>
    /// Used to load and reload scenes
    /// </summary>
    public class LoadLevel : MonoBehaviour
    {
        public UnityEvent<int> OnSceneTransition;

        /// <summary>
        /// Loads a scene at <see cref="index"/>
        /// </summary>
        public void LoadIndex(int index)
        {
            if (OnSceneTransition.GetPersistentEventCount() > 0)
            {
                OnSceneTransition.Invoke(index);
            }
            else
            {
                SceneManager.LoadScene(index);
            }
        }

        /// <summary>
        /// Loads a scene with the exact name <see cref="levelName"/>.
        /// Case sensitive.
        /// </summary>
        public void Load(string levelName)
        {
            if (OnSceneTransition.GetPersistentEventCount() > 0)
            {
                OnSceneTransition.Invoke(SceneManager.GetSceneByName(levelName).buildIndex);
            }
            else
            {
                SceneManager.LoadScene(levelName);
            }
        }

        /// <summary>
        /// Reloads the current scene
        /// </summary>
        public void Reload() => LoadIndex(SceneManager.GetActiveScene().buildIndex);

        /// <summary>
        /// Loads the next scene in build settings
        /// </summary>
        public void NextScene() => LoadIndex(SceneManager.GetActiveScene().buildIndex + 1);

        /// <summary>
        /// Exits the application
        /// </summary>
        public void Exit() => Application.Quit();
    }
}