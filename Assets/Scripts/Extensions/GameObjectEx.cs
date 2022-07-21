// External Namespaces
using UnityEngine;
using Assets.Scripts.Tweens;

namespace Assets.Scripts.Extensions
{
    /// <summary>
    /// All extensions to the UnityEngine.GameObject class.
    /// </summary>
    public static class GameObjectEx
    {
        /// <summary>
        /// This function will try to get a component, if it failed it will add the component to the game object.
        /// </summary>
        /// <typeparam name="T">Type of component. Must inherit from UnityEngine.Component</typeparam>
        /// <param name="gameObject">Object for inspection.</param>
        /// <returns>The valid component instance.</returns>
        /*
         * Author: James Greensill
         */

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
#if DEBUG
            Debug.Log($"Getting Component: {typeof(T).Name}");
#endif
            // try get the component, otherwise add the component.
            if (!gameObject.TryGetComponent<T>(out var component))
            {
#if DEBUG
                Debug.Log($"Could not get Component: {typeof(T).Name}. Adding to object.");
#endif
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Rewinds all attached <see cref="ITweener"/> components instantly
        /// </summary>
        public static void ResetTweeners(this GameObject gameobject, bool includeChildren = true)
        {
            ITweener[] tweeners;
            if(includeChildren)
                tweeners = gameobject.GetComponentsInChildren<ITweener>();
            else
                tweeners = gameobject.GetComponents<ITweener>();

			foreach (ITweener tweener in tweeners)
			    tweener.Reset();
		}
    }
}