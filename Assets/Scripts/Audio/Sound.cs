/*
 *  Author: James Greensill
 *  Usage:  Sound class used for playing sound effects & music.
 *          The sound class is used by the Sound Database class.
*/

// External Namespaces
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Type of sound file.
    /// </summary>
    public enum SoundType
    {
        SFX = 0,
        MUSIC
    }

    [System.Serializable]
    public struct SoundSettings
    {
        /// <summary>
        /// The sound type.
        /// </summary>
        public SoundType type;

        /// <summary>
        /// Mixer Group of this sound type.
        /// </summary>
        public AudioMixerGroup mixerGroup;
    }

    /// <summary>
    /// Sound class used for playing sound effects & music.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        /// <summary>
        /// The name of the sound.
        /// </summary>
        [Header("Required")]
        [Tooltip("If null, the name will be the clip file name.")]
        public string clipName;

        /// <summary>
        /// The sound clip reference.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// Settings of this sound instance.
        /// Default settings are used if not specified.<br/>
        /// default settings are:<br/><br/>
        /// type: SFX<br/>
        /// loop: false<br/>
        /// volume: 1.0f<br/>
        /// pitch: 1.0f
        /// </summary>
        public SoundSettings settings = new SoundSettings()
        {
            type = SoundType.SFX
        };
    };
}