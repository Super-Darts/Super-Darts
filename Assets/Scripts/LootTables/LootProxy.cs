using Assets.Scripts.DataStructures;
using System.Linq;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.LootTables
{
    public enum LootRequestResult
    {
        SUCCESS,
        OUT_OF_BOUNDS
    }

    public class LootProxy : MonoBehaviour
    {
        [field: SerializeField] public LootDatabase database;

        public LootRequestResult TryRequest(int queryScore, out LootItem lootItem)
        {
            foreach (var tier in database.tiers)
            {
                Debug.Log($"{tier.tierRange.ToString()}");
                Debug.Log($"{queryScore}");
                
                if (tier.tierRange.Bounds(queryScore))
                {
                    lootItem = tier.referencedItems.Random();
                    return LootRequestResult.SUCCESS;
                }
            }

            lootItem = null;
            return LootRequestResult.OUT_OF_BOUNDS;
        }

        public void TestRequest(int queryScore)
        {
            if (TryRequest(queryScore, out var item) != LootRequestResult.SUCCESS)
            {
                Debug.Log("Failed to get item.");
                return;
            }

            Debug.Log($"{item.name}, ");
        }
    }
}