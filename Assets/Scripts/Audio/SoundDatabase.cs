/*
 *  Author: James Greensill
 *  Usage:  Sound Database is used to hold all sounds references and their associated sounds.
 *          The Sound Database is used by Sound Proxies to play sounds.
 */

// External Namespaces

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "ScriptableObjects/Audio/SoundDatabase")]
    public class SoundDatabase : ScriptableObject
    {
        /// <summary>
        /// Array of sounds within the database.
        /// (note: the size should not change @ runtime.)
        /// </summary>
        [SerializeField] public Sound[] sounds;

        /// <summary>
        /// Internal dictionary. The "sounds" array is converted into this dictionary.
        /// </summary>
        private Dictionary<string, Sound> _internalSoundDictionary = new Dictionary<string, Sound>();

        public Sound Get(string clipName) => _internalSoundDictionary[clipName];

        public bool TryGet(string clipName, out Sound sound) => _internalSoundDictionary.TryGetValue(clipName, out sound);

        public bool Initialized => _internalSoundDictionary.Count > 0;

        internal void InitializeDatabase()
        {
            // Only Initialize the database once.
            if (!Initialized)
            {
                foreach (var sound in sounds)
                {
                    _internalSoundDictionary.Add(sound.clipName, sound);
                }
            }
        }
    }
}