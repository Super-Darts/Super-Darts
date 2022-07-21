/*
 *  Author: James Greensill
 *  Usage:  MusicProxy is used to play longer lasting clips.
 */

namespace Assets.Scripts.Audio
{
    public class MusicProxy : SoundProxy
    {
        /// <summary>
        /// Music Clips are typically longer lasting than SFX clips, therefore they can be stopped.
        /// (note: only one music clip PER PROXY can be playing at a time)
        /// </summary>
        /// <param name="sound"></param>
        protected override void _PlaySound(Sound sound)
        {
            // If the sound is not null & type is sfx.
            if (sound is { settings: { type: SoundType.MUSIC } })
            {
                source.clip = sound.clip;
                source.Play();
            }
        }
    }
}