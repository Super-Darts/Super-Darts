using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EventHelpers
{
    /// <summary>
    /// Base Component to trigger events.
    /// </summary>
    public class EventHelper : MonoBehaviour
    {
        /// <summary>
        /// Base EventHandle for all event helpers.
        /// </summary>
        [field: SerializeField] public UnityEvent EventHandle { get; set; }

        /// <summary>
        /// Invokes the EventHandle.
        /// </summary>
        public virtual void Execute() => EventHandle?.Invoke();
    }
}