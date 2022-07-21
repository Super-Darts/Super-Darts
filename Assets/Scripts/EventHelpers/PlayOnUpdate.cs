/*
 *  Author: James Greensill
 *  Usage:  For use in the inspector, to execute code on Update.
 */

namespace Assets.Scripts.EventHelpers
{
    /// <summary>
    /// This helper will execute the event on Update.
    /// </summary>
    public class PlayOnUpdate : EventHelper
    {
        public void Update() => Execute();
    }
}