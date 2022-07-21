using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.ObjectPathing
{
    [CreateAssetMenu(fileName = "ObjectPathDatabase", menuName = "ScriptableObjects/ObjectPathing/PathDatabase")]
    public class ObjectPathDatabase : ScriptableObject
    {
        /// <summary>
        /// The range of that the speed can be. This is used to make the speed random.
        /// </summary>
        [field: SerializeField] public Range<float> SpeedRange { get; set; }

        /// <summary>
        /// The rate at which the object switches path's at. This is used to make the switching random.
        /// </summary>
        [field: SerializeField] public Range<float> SwitchRateRange { get; set; }
        /// <summary>
        /// List of Paths that the object can follow. Chosen at random per SwitchRate Interval.
        /// </summary>
        [field: SerializeField] public ObjectPath[] Paths { get; set; }
    }
}