/*
 *  Author: James Greensill
 *  Usage:  For use in the inspector, to execute code on Awake.
 */

namespace Assets.Scripts.EventHelpers
{
	/// <summary>
	/// This helper will execute the event on Awake.
	/// </summary>
	public class PlayOnEnable: EventHelper
	{
		public void OnEnable() => Execute();
	}
}