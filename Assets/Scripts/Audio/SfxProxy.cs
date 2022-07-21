/*
 *  Author: James Greensill
 *  Usage:  SfxProxy is used to play Set & Forget audio clips.
 */

namespace Assets.Scripts.Audio
{
    public class SfxProxy : SoundProxy
    {
        /// <summary>
        /// Sound Effects are typically "Play & Forget". Once started, this sound effect cannot be stopped.
        /// </summary>
        /// <param name="sound">Sound Effect</param>
        protected override void _PlaySound(Sound sound)
        {
            // If the sound is not null & type is sfx.
            if (sound is { settings: { type: SoundType.SFX } })
            {
                source.PlayOneShot(sound.clip);
            }
        }
    }
}