/*
 *  Author: Lewis Comstive
 *  Usage: Creates a UI menu for prizes
 */
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.LootTables;
using Assets.Scripts.Extensions;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.UI
{
	public class PrizesMenu : MonoBehaviour
	{
		[System.Serializable]
		public struct PrizeMenuUIElement
		{
			public Button Button;

			[Tooltip("Case-sensitive")]
			public string PrizeName;
		}

		[SerializeField, Tooltip("The transform to spawn prizes, will also be the parent for spawned objects")]
		private Transform m_SpawnPosition;

		[field: SerializeField] public LootDatabase Database { get; private set; }

		[Header("UI")]
		[SerializeField] PrizeMenuUIElement[] m_PrizeUI;

		private Dictionary<LootItem, GameObject> m_SpawnedPrizes = new Dictionary<LootItem, GameObject>();

		private void OnEnable()
		{
			SetButtonStates();

			foreach (LootItem prize in Database.lootItems)
			{
				// Yucky way of doing things but it just needs to work
				foreach (PrizeMenuUIElement ui in m_PrizeUI)
				{
					if (ui.PrizeName.Equals(prize.name))
					{
						ui.Button.onClick.AddListener(() => SpawnPrize(prize));
						break;
					}
				}
			}
		}

		/// <summary>
		/// Updates all <see cref="m_PrizeUI"/> interactable states to match unlocked prizes in <see cref="PlayerData.Data"/>
		/// </summary>
		public void SetButtonStates()
		{
			PlayerData.Load();

			foreach (PrizeMenuUIElement ui in m_PrizeUI)
				ui.Button.interactable = false;

			// Alter UI buttons for each available prize
			foreach (LootItem prize in Database.lootItems)
			{
				// Yucky way of doing things but it just needs to work
				foreach (PrizeMenuUIElement ui in m_PrizeUI)
				{
					if (ui.PrizeName.Equals(prize.name))
					{
						ui.Button.interactable = PlayerData.Data.UnlockedPrizes.Find(x => x.Equals(prize.name)) != null;
						break;
					}
				}
			}
		}

		/// <returns><see cref="LootItem"/> matching <paramref name="name"/>, or `null` if not found</returns>
		public LootItem FindPrize(string name) => Database.lootItems.Find(x => x.name.Equals(name));

		/// <summary>
		/// Spawns a prize at <see cref="m_SpawnPosition"/>
		/// </summary>
		/// <param name="prize"></param>
		public void SpawnPrize(LootItem prize)
		{
			if(m_SpawnedPrizes.ContainsKey(prize))
			{
				// Just delete and respawn later. Should be changed to increase performance when more time available
				Destroy(m_SpawnedPrizes[prize]);
				m_SpawnedPrizes.Remove(prize);
			}

			GameObject spawned = Instantiate(
				prize.Prefab,
				m_SpawnPosition.position,
				prize.Prefab.transform.rotation
				);
			spawned.ResetTweeners();

			m_SpawnedPrizes.Add(prize, spawned);
		}
	}


#if UNITY_EDITOR
	[CustomEditor(typeof(PrizesMenu))]
	public class PrizesMenuEditor : Editor
	{
		private string m_PrizeName = "";
		private PrizesMenu m_Target;

		private void OnEnable() => m_Target = (PrizesMenu)target;

		private void SetUnlockStatus(LootItem item, bool unlocked)
		{
			if (!item)
				return;

			item.Unlocked = unlocked;

			if(unlocked) // Add to save data
				PlayerData.Data.UnlockedPrizes.Add(item.name);
			else // Remove from save data
				PlayerData.Data.UnlockedPrizes.RemoveAll(x => x.Equals(item.name));
			PlayerData.Save();

			m_Target.SetButtonStates();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space(20);

			m_PrizeName = EditorGUILayout.TextField("Prize Name", m_PrizeName);
			if (GUILayout.Button("Lock"))
			{
				PlayerData.Load();
				SetUnlockStatus(m_Target.FindPrize(m_PrizeName), false);
				PlayerData.Save();
			}
			if (GUILayout.Button("Unlock"))
			{
				PlayerData.Load();
				SetUnlockStatus(m_Target.FindPrize(m_PrizeName), true);
				PlayerData.Save();
			}

			EditorGUILayout.Space(20);
			EditorGUILayout.LabelField("Hax");
			if(GUILayout.Button("Unlock All"))
			{
				PlayerData.Load();
				PlayerData.Data.UnlockedPrizes = new List<string>();
				foreach(LootItem item in m_Target.Database.lootItems)
					SetUnlockStatus(item, true);
				PlayerData.Save();
			}
			if(GUILayout.Button("Reset Unlock Data"))
			{
				foreach (LootItem item in m_Target.Database.lootItems)
					SetUnlockStatus(item, false);

				PlayerData.Load();
				PlayerData.Data.UnlockedPrizes = new List<string>();
				PlayerData.Save();
			}
		}
	}
#endif
}