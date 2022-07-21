using UnityEngine;
using System.Collections;
using Assets.Scripts.LootTables;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
	public class UnlockPrizeUI : MonoBehaviour
	{
		[SerializeField] private Transform m_SpawnPosition;

		public void OnPrizeUnlocked(LootItem prize) => Instantiate(prize.Prefab, m_SpawnPosition.position, m_SpawnPosition.transform.rotation * prize.Prefab.transform.rotation);
	}
}