using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ScriptableObjectEx
    {
        /// <summary>
        /// Creates a clone of a scriptable object.
        /// </summary>
        /// <typeparam name="T">Type of scriptable object</typeparam>
        /// <param name="scriptableObject">Instance of scriptable object</param>
        /// <returns>Cloned Scriptable Object</returns>
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject =>
            ScriptableObjectHelper.Clone(scriptableObject);
    }
}