/*
 *  Author: James Greensill
 *  Usage:  Helper class for scriptable objects.
 */

using Assets.Scripts.Extensions;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Helper class for scriptable object I/O
    /// </summary>
    public static class ScriptableObjectHelper
    {
        /// <summary>
        /// Save scriptable object data to a .asset file.
        /// </summary>
        /// <typeparam name="T">ScriptableObject type</typeparam>
        /// <param name="objectRef">data</param>
        public static void Save<T>(T objectRef) where T : ScriptableObject
        {
#if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel("Save Object Path", Application.dataPath, "ObjectPath", "asset");
            Debug.Log(path);
            if (path.Length != 0)
            {
                path = path.Replace(Application.dataPath, "Assets");
                Debug.Log(path);
                AssetDatabase.CreateAsset(objectRef.Clone(), path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
#else
            Debug.LogWarning("NOT IN EDITOR");
#endif
        }

        /// <summary>
        /// Load ScriptableObject data from a file into a data container.
        /// </summary>
        /// <typeparam name="T">ScriptableObject type</typeparam>
        /// <param name="data">data container</param>
        public static void Load<T>(ref T data) where T : ScriptableObject
        {
#if UNITY_EDITOR
            string path = EditorUtility.OpenFilePanel("Load Object Path", Application.dataPath, "asset");
            if (path.Length != 0)
            {
                path = path.Replace(Application.dataPath, "Assets");
                data = AssetDatabase.LoadAssetAtPath<T>(path);
            }
#else
            Debug.LogWarning("NOT IN EDITOR");
#endif
        }

        /// <summary>
        /// Create a scriptable object instance.
        /// </summary>
        /// <typeparam name="T">ScriptableObject type</typeparam>
        /// <returns></returns>
        public static T Create<T>() where T : ScriptableObject => ScriptableObject.CreateInstance<T>();

        /// <summary>
        /// Clone a scriptable object
        /// </summary>
        /// <typeparam name="T">ScriptableObject type</typeparam>
        /// <param name="scriptableObject">ScriptableObject to clone</param>
        /// <returns>cloned ScriptableObject</returns>
        public static T Clone<T>(T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }
            T instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name; // remove (Clone) from name
            return instance;
        }
    }
}