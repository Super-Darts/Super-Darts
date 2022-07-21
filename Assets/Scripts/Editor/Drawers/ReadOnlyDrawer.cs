using Assets.Scripts.Attributes;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Assets.Scripts.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
#if UNITY_EDITOR

        public override float GetPropertyHeight(SerializedProperty property,
            GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

#endif
#if UNITY_EDITOR

        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
        }

#endif
    }
}