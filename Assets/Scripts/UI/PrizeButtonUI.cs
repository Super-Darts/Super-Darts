/*
 *  Author: Lewis Comstive
 *  Usage: UI button for spawning a player's prize
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.LootTables;
using Assets.Scripts.Tweens;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.UI
{
	public class PrizeButtonUI : MonoBehaviour
	{
		public LootItem Prize { get; private set; }

		// Inspector //
		[SerializeField] private Button m_Button;
		[SerializeField] private TMP_Text m_DisplayName;

		public void SetPrize(LootItem prize)
		{
			if(Prize != null)
				Prize.OnUnlockStatusChanged -= OnPrizeUnlockStatusChanged;

			Prize = prize;

			m_DisplayName.text = prize.name;
			m_Button.interactable = prize.Unlocked;

			Prize.OnUnlockStatusChanged += OnPrizeUnlockStatusChanged;

			m_Button.onClick.AddListener(OnButtonClick);
		}

		private void OnDestroy() => m_Button.onClick.RemoveListener(OnButtonClick);

		private void OnPrizeUnlockStatusChanged(LootItem prize)
		{
			if(m_Button != null)
				m_Button.interactable = prize.Unlocked;
		}

		private void OnButtonClick() => FindObjectOfType<PrizesMenu>().SpawnPrize(Prize);
	}
}