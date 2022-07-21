/*
 *  Author: Lewis Comstive
 *  Usage: Changes level and displays text depending on game win/lose state
 */

using Assets.Scripts.DartGame;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Extensions;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Helpers
{
	/// <summary>
	/// Changes level and displays text depending on game win/lose state
	/// </summary>
	public class GameStateManager : MonoBehaviour
	{
		[SerializeField, Tooltip("When game has been won")]
		private UnityEvent m_OnWinState;
		
		[SerializeField, Tooltip("When game has been lost")]
		private UnityEvent m_OnLoseState;

		[SerializeField, Tooltip("When the game has been won OR lost")]
		private UnityEvent m_OnGameDecided;

		[SerializeField, Tooltip("When a new highscore has been reached")]
		private UnityEvent m_OnNewHighscore;

		/// <summary>
		/// Maximum level index that can be unlocked.
		/// This number is based off build scene count, expecting a main menu and one tutorial scene.
		/// </summary>
		private int MaxLevelIndex => SceneManager.sceneCountInBuildSettings - 1; // Subtract 1 for MainMenu scene. 0 = tutorial

		/// <summary>
		/// Set the win condition for this game.
		/// Expected to be done via script or UnityEvent that keeps track of scoring
		/// </summary>
		/// <param name="win">Whether player has won all rounds in this scene</param>
		public void SetGameWinState(bool win)
		{
			PlayerData.Load();
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex - 1; // Subtract 1 for MainMenu scene
			bool canProgressToNextLevel = win &&
				currentSceneIndex >= PlayerData.Data.UnlockedLevelIndex && // Current level is latest unlocked
				currentSceneIndex < MaxLevelIndex; // There are more scenes to unlock

			if (canProgressToNextLevel)
			{
				PlayerData.Data.UnlockedLevelIndex = (byte)(currentSceneIndex + 1);
				PlayerData.Save();
			}

			DartGameManager manager = FindObjectOfType<DartGameManager>();
			if (win && PlayerData.Data.Highscore < manager.totalScore)
			{
				PlayerData.Data.Highscore = manager.totalScore;
				m_OnNewHighscore.Invoke();
				PlayerData.Save();
			}

			// Invoike event based on win state
			if(win)
				m_OnWinState.Invoke();
			else
				m_OnLoseState.Invoke();

			m_OnGameDecided.Invoke();
		}
	}
}