/*
 *  Author: Lewis Comstive
 *  Usage: Changes level and displays text depending on game win/lose state
 */
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class LevelSelectUI : MonoBehaviour
	{
		[SerializeField] private Button[] m_LevelButtons;

		private void Start()
		{
			PlayerData.Load();

			for (int i = 0; i < m_LevelButtons.Length; i++)
				m_LevelButtons[i].interactable = i < PlayerData.Data.UnlockedLevelIndex;
		}
	}
}