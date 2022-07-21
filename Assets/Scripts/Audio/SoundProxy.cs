/*
 *  Author: James Greensill
 *  Usage:  Sound Proxy Component is used to access sound databases & play sounds.
 */

// Internal Namespaces.
using Assets.Scripts.Extensions;

// External Namespaces.
using UnityEngine;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Component to act as a database accessor.
    /// </summary>
    public abstract class SoundProxy : MonoBehaviour
    {
        /// <summary>
        /// Sound database for this proxy to access.
        /// (note: This should set via the inspector.)
        /// </summary>
        [SerializeField] public SoundDatabase soundDatabase;

        /// <summary>
        /// Audio source this proxy will use to play sounds.
        /// (note: If this component is not on the object, it will automatically be created
        /// @ runtime with Assets::Scripts::Extensions::GameObject::GetOrAddComponent(T).)
        /// </summary>
        protected AudioSource source;

        private void Awake()
        {
            // Get the audio source.
            source = gameObject.GetOrAddComponent<AudioSource>();

            soundDatabase.InitializeDatabase();
        }

        /// <summary>
        /// This will play a sound with a given name.
        /// </summary>
        /// <param name="soundName">Name of the sound clip.</param>
        /// <param name="settings">Optional parameters to be passed via code.</param>
        public void Play(string soundName, SoundSettings? settings)
        {
            // Get the sound from the database.
            if (!_TryGetSound(soundName, out Sound sound))
            {
                return;
            }

            if (settings != null)
            {
                sound.settings = settings.Value;
            }

            // Play sound.
            _PlaySound(sound);
        }

        /// <summary>
        /// Inspector friendly Play method.
        /// </summary>
        /// <param name="soundName"></param>
        public void Play(string soundName) => Play(soundName, null);

        public void Stop() => source?.Stop();

        /// <summary>
        /// This will mute the audio source.
        /// </summary>
        /// <param name="flag">Mute flag.</param>
        public void Mute(bool flag) => source.mute = flag;
        
        // TODO: Use player settings when they have been implemented?
        /// <summary>
        /// This will mute all audio sources in the scene.
        /// </summary>
        /// <param name="flag">Mute flag.</param>
        public void MuteAll(bool flag) => AudioListener.volume = flag ? 0 : 1;

        /// <summary>
        /// This will play the sound clip stored in the Sound class.
        /// </summary>
        /// <param name="sound">Sound value. Must be a valid reference of an object.</param>
        protected abstract void _PlaySound(Sound sound);

        /// <summary>
        /// This will try to get the sound from the database.
        /// </summary>
        /// <param name="soundName">Name of the sound object in the database.</param>
        /// <param name="sound">Retrieved sound instance.</param>
        /// <returns></returns>
        private bool _TryGetSound(string soundName, out Sound sound)
        {
            if (!soundDatabase.TryGet(soundName, out sound))
            {
#if DEBUG
                Debug.LogError($"SoundProxy: Sound '{soundName}' not found in database.");
#endif
                return false;
            }
            return true;
        }
    }
}