using Assets.Scripts.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts.LootTables
{
    [Serializable]
    public struct DatabaseReference
    {
        [ReadOnly] public LootDatabase database;
        [ReadOnly] public LootTier tier;
    }

    [CreateAssetMenu(fileName = "Loot Item", menuName = "ScriptableObjects/LootTables/LootItem")]
    public class LootItem : ScriptableObject, ISerializable
    {
        [ReadOnly]
        [SerializeField]
        public List<DatabaseReference> databaseReferences = new List<DatabaseReference>();

        private bool m_Unlocked;

        public GameObject Prefab;
        public Action<LootItem> OnUnlockStatusChanged;

        public bool Unlocked
        {
            get => m_Unlocked;
            set
            {
                if (value == m_Unlocked)
                    return; // No change

                m_Unlocked = value;
                OnUnlockStatusChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Constructor used when deserializing item data
        /// </summary>
        public LootItem(SerializationInfo info, StreamingContext context) => m_Unlocked = info.GetBoolean("Unlocked");

        /// <summary>
        /// Used when serializing item data
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Unlocked", m_Unlocked);
    }
}