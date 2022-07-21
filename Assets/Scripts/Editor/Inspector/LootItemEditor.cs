using Assets.Scripts.LootTables;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Assets.Scripts.Editor.Inspector
{
    [CustomEditor(typeof(LootItem))]
    public class LootItemEditor : UnityEditor.Editor
    {
        private LootItem _ref = null;

        public override void OnInspectorGUI()
        {
#if UNITY_EDITOR
            _ref = (LootItem)target;
            base.OnInspectorGUI();

            if (!_ref)
            {
                return;
            }

            if (_ref.databaseReferences != null)
            {
                for (var index = 0; index < _ref.databaseReferences.Count; index++)
                {
                    EditorGUILayout.LabelField($"Database: {_ref.databaseReferences[index].database.name}.");
                    if (EditorGUILayout.DropdownButton(new GUIContent($"{_ref.databaseReferences[index].tier.name} - {_ref.databaseReferences[index].tier.tierRange.ToString()}"), FocusType.Keyboard))
                    {
                        GenericMenu menu = new GenericMenu();

                        // select tier
                        // remove item ref from current tier
                        // add item ref to selected tier.

                        foreach (var tier in _ref.databaseReferences[index].database.tiers)
                        {
                            //if (_ref.databaseReferences[index].tier == tier)
                            //{
                            //    continue;
                            //}
                            //
                            //if (tier.referencedItems.TryGetValue(_ref.GetInstanceID(), out var lootItem))
                            //{
                            //    if (lootItem.databaseReferences[index].tier == tier)
                            //    {
                            //        Debug.Log($"Removing Instance: {lootItem.name} from tier: {tier.name}");
                            //        tier.referencedItems.Remove(_ref.GetInstanceID());
                            //    }
                            //}
                            //
                            //foreach (var refItem in tier.referencedItems)
                            //{
                            //    Debug.Log(refItem.Value.name);
                            //}

                            var delegateIndex = index;
                            menu.AddItem(new GUIContent($"{tier.name} - {tier.tierRange.ToString()}"), false, () =>
                            {
                                // _ref.databaseReferences[delegateIndex].tier = tier;
                                _ref.databaseReferences[delegateIndex].tier.referencedItems.Remove(_ref);
                                tier.referencedItems.Add(_ref);

                                var refDatabaseReference = _ref.databaseReferences[delegateIndex];
                                refDatabaseReference.tier = tier;
                                _ref.databaseReferences[delegateIndex] = refDatabaseReference;
                            });
                        }

                        menu.ShowAsContext();
                    }
                }
            }
#endif
        }
    }
}