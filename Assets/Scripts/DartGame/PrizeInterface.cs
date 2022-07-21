/*
 *  AUTHOR: James Greensill
 */

using Assets.Scripts.LootTables;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DartGame
{
    public class PrizeInterface : MonoBehaviour
    {
        // usage.
        // submit a request to fetch a prize with a score.
        // unlock this prize in the player data.
        // reflect unlocked item in the UI.

        [SerializeField] public UnityEvent<LootItem> onPrizeUnlocked;

        [field: SerializeField] private LootProxy _proxy { get; set; }

        public void UnlockRandomPrize()
        {
            if (!_proxy)
            {
#if DEBUG
                Debug.LogError($"Loot proxy null reference on the PrizeInterface script.");
#endif
                return;
            }

            PlayerData.Load();
            if (_proxy.TryRequest(PlayerData.Data.Score, out LootItem item) == LootRequestResult.SUCCESS)
            {
#if DEBUG
                Debug.Log($"Score: {PlayerData.Data.Score}");
                Debug.Log($"Fetched item with the score: {PlayerData.Data.Score}. Item name: {item.name}.");
#endif
                item.Unlocked = true;
                onPrizeUnlocked?.Invoke(item);
                PlayerData.Data.UnlockedPrizes.Add(item.name);
                PlayerData.Save();
            }
        }

        private void Awake() => _proxy ??= GetComponent<LootProxy>();

        private void Start() => _proxy ??= GetComponent<LootProxy>();
    }
}