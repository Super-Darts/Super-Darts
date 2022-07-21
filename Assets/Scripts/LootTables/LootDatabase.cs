using UnityEditor;
using UnityEngine;
using Assets.Scripts.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Scripts.DataStructures;

namespace Assets.Scripts.LootTables
{
    [System.Serializable]
    public class LootTier
    {
        public string name;
        public Range<int> tierRange;

        [ReadOnly]
        public List<LootItem> referencedItems = new List<LootItem>();

        public bool Equals(LootTier other) => tierRange.Equals(other.tierRange);
    }

    [CreateAssetMenu(fileName = "Loot Database", menuName = "ScriptableObjects/LootTables/Database")]
    public class LootDatabase : ScriptableObject, ISerializable
    {
        [field: SerializeField] public List<LootTier> tiers;
        [field: SerializeField] public List<LootItem> lootItems = new List<LootItem>();

        private string _path = "";

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var item in lootItems)
            {
                item.GetObjectData(info, context);
            }
        }

#if UNITY_EDITOR
		private void Awake() { _SubmitToAssetProcessor(); }
#endif

        private void RemoveFromItemReferences()
        {
            for (var i = 0; i < lootItems.Count; i++)
            {
                int indexToRemove = -1;
                var item = lootItems[i];
                for (int j = 0; j < item.databaseReferences.Count; j++)
                {
                    var dbRef = item.databaseReferences[j];
                    if (this == dbRef.database)
                    {
                        indexToRemove = j;
                    }
                }

                if (indexToRemove >= 0)
                {
                    item.databaseReferences.RemoveAt(indexToRemove);
                }
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
			if (string.IsNullOrEmpty(_path)) { _SubmitToAssetProcessor(); }
#endif

            for (var i = 0; i < lootItems.Count; i++)
            {
                LootItem item = lootItems[i];
                if (item == null)
                {
                    lootItems.RemoveAt(i);
                    continue;
                }

                if (item.databaseReferences == null)
                {
                    continue;
                }
                List<int> indicesToRemove = new List<int>(item.databaseReferences.Count);

                bool hasReference = false;
                foreach (var dbRef in item.databaseReferences)
                {
                    if (dbRef.database == this)
                    {
                        hasReference = true;
                        // found reference.
                        break;
                    }

                    if (dbRef.database == null)
                    {
                        indicesToRemove.Add(item.databaseReferences.IndexOf(dbRef));
                    }
                }

                foreach (var index in indicesToRemove)
                {
                    item.databaseReferences.RemoveAt(index);
                }

                if (hasReference)
                {
                    // already been added as a reference.
                    continue;
                }

                // add if it has not been added as a reference.
                item.databaseReferences.Add(new DatabaseReference()
                {
                    database = this
                }
                );
            }
        }

        #if UNITY_EDITOR
        private void _SubmitToAssetProcessor()
        {
            _path = AssetDatabase.GetAssetPath(GetInstanceID());
            LootDatabaseProcessor.flagFileAddresses.Add(new AssetWatcher()
            {
                path = _path,
                function = RemoveFromItemReferences
            });
        }
        #endif
    }
}