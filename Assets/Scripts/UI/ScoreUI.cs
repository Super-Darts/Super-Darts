/*
 *  Author: Lewis Comstive
 *  Usage: Updates text UI with player score, intended for use with UnityEvents
 */
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UserInterface
{
	/// <summary>
	/// Updates text UI with player score, intended for use with UnityEvents.
	/// Must be put on GameObject with TMP_Text component to be modified.
	/// </summary>
	[RequireComponent(typeof(TMP_Text))]
	public class ScoreUI : MonoBehaviour
	{
		/// <summary>
		/// Current score value
		/// </summary>
		[SerializeField] private int m_Score;

		/// <summary>
		/// Format to use when setting text. '$score' is replaced with integer value of score (e.g. "Score: $score")
		/// </summary>
		[SerializeField, Tooltip("Format to use when setting text. '$score' is replaced with integer value of score (e.g. \"Score: $score\")")]
		private string m_Format = "Score: $score";

		/// <summary>
		/// Cache of local text component
		/// </summary>
		private TMP_Text m_Text;

		/// <summary>
		/// Current score value
		/// </summary>
		public int Score
		{
			get => m_Score;
			set => SetScore(value);
		}

		/// <summary>
		/// Sets <see cref="Score"/> and updates the attached text component
		/// </summary>
		public void SetScore(int score)
		{
			m_Score = score;
			m_Text.text = m_Format.Replace("$score", score.ToString());
		}

		public void IncrementScore() => SetScore(m_Score + 1);
		public void DecrementScore() => SetScore(m_Score - 1);

		public void IncrementScore(int value) => SetScore(m_Score + value);
		public void DecrementScore(int value) => SetScore(m_Score - value);

		// Get text component and set initial value
		private void Awake()
		{
			m_Text = GetComponent<TMP_Text>();
			SetScore(m_Score);
		}
	}
}