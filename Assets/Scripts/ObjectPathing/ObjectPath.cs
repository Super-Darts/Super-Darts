/*
 *  Author: James Greensill
 *  Usage:  Data Structure for object path data.
 */

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ObjectPathing
{
    public class ObjectPath : ScriptableObject
    {
        /// <summary>
        /// Name of the path.
        /// </summary>
        [field: SerializeField]
        public string pathName = "Enter name here...";

        /// <summary>
        /// List of path points in global space.
        /// </summary>
        [field: SerializeField]
        public List<Vector3> Path { get; set; } = new List<Vector3>();

        /// <summary>
        /// Flag to determine whether the path should loop.
        /// </summary>
        [field: SerializeField]
        public bool Loop { get; set; } = false;

        /// <summary>
        /// Adds a point to the path, based on where previous points are.
        /// </summary>
        /// <param name="length"></param>
        public void AddDirectionalPoint(float length = 1.0f)
        {
            Vector3 pointToAdd = new Vector3();

            if (Path.Count > 1)
            {
                pointToAdd = Path[Path.Count - 1];
            }
            if (Path.Count >= 2)
            {
                Vector3 direction = pointToAdd - Path[Path.Count - 2];
                pointToAdd = direction.normalized * length;
            }

            Path.Add(pointToAdd);
        }
    }
}