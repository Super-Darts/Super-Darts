/*
 *  Author: Lewis Comstive
 *  Usage: Updates text of UI with values of the level currently active 
 */
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DartGame;

namespace Assets.Scripts.UserInterface
{
	/// <summary>
	/// Updates text of UI with values of the level currently active
	/// </summary>
	public class LevelInfoUI : MonoBehaviour
	{
		// Inspector Variables //

		[SerializeField] private ScoreUI m_Lives;
		[SerializeField] private ScoreUI m_TimeCountdown;
		[SerializeField] private ScoreUI m_PointsRequired;

		/// <summary>
		/// Updates text of UI using values from `level`
		/// </summary>
		public void SetLevel(DartGameManager.Level level)
		{
			m_Lives.SetScore(level.lives);
			m_PointsRequired.SetScore(level.pointsNeeded);
			m_TimeCountdown.SetScore(Mathf.FloorToInt(level.time));

			m_TimeRemaining = level.time;
		}

		/// <summary>
		/// Used to set the countdown timer text value.
		/// Intended to be called from an event or UnityEvent
		/// </summary>
		public void TimeRemainingChanged(int timeRemaining) => m_TimeCountdown.SetScore(timeRemaining);

		// Temporary, ideally a separate script handles this and
		// 	a UnityEvent is used to call TimeRemainingChanged (?)
		private float m_TimeRemaining = 0;

		// Reduces m_TimeRemaining by Time.fixedDeltaTime every physics update.
		// Used instead of Update() purely as it's (usually) called less often,
		//	and m_TimeRemaining is floored so only really needed to be updated once per second
		private void FixedUpdate()
		{
			if(m_TimeRemaining > 0)
			{
				m_TimeRemaining = Mathf.Clamp(m_TimeRemaining - Time.fixedDeltaTime, 0, float.MaxValue);
				TimeRemainingChanged(Mathf.FloorToInt(m_TimeRemaining));
			}
		}
	}
}