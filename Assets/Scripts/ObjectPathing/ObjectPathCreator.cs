/*
 *  Author: James Greensill.
 *  Usage:  ObjectPathCreator is used to create T:ObjectPaths in the editor.
 */

using Assets.Scripts.Helpers;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace Assets.Scripts.ObjectPathing
{
    public class ObjectPathCreator : MonoBehaviour
    {
        /// <summary>
        /// Current ObjectPath instance.
        /// </summary>
        private ObjectPath _pathInstance;

        /// <summary>
        /// Draw Custom Inspector (To be called from an editor script).
        /// </summary>
        public void DrawInspector()
        {
#if UNITY_EDITOR
            // If path is not null
            if (_pathInstance != null)
            {
                // Add Button (Add Point)
                EditorGuiHelper.DrawButton("Add Point", () => _pathInstance.AddDirectionalPoint());

                // Add Button (Clear Path)
                EditorGuiHelper.DrawButton("Clear Path", () => _pathInstance.Path.Clear());

                // Add Button (Delete Path Instance)
                EditorGuiHelper.DrawButton("Delete", () => _pathInstance = null);

                // Draw the ObjectPath in the inspector.
                DrawInspector(ref _pathInstance);
            }

            // Draw a dropdown menu.
            if (EditorGUILayout.DropdownButton(new GUIContent("File I/O"), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                // Add menu item (New)
                menu.AddItem(new GUIContent("New"), false, () => _pathInstance = ScriptableObjectHelper.Create<ObjectPath>());
                if (_pathInstance != null)
                {
                    // (note: this will open a popup)
                    // Add menu item (Save)
                    menu.AddItem(new GUIContent("Save"), false, () => ScriptableObjectHelper.Save(_pathInstance));
                }
                // (note: this will open a popup)
                // Add menu item (Load)
                menu.AddItem(new GUIContent("Load"), false, () => ScriptableObjectHelper.Load<ObjectPath>(ref _pathInstance));
                // Show the menu.
                menu.ShowAsContext();
            }
#endif
        }

        internal void DrawInspector(ref ObjectPath path)
        {
#if UNITY_EDITOR
            // Copy the object path.
            ObjectPath pathCopy = path;

            if (!pathCopy)
            {
                return;
            }

            pathCopy.Loop = EditorGUILayout.Toggle(new GUIContent("Looping?"), pathCopy.Loop);

            // Loop through all the points in the path.
            for (var index = 0; index < pathCopy.Path.Count; index++)
            {
                Vector3 vec3 = pathCopy.Path[index];

                GUILayout.BeginHorizontal();

                // Draw the positional data.
                EditorGuiHelper.Draw($"Position Node: {index}", ref vec3);
                pathCopy.Path[index] = vec3;

                int tempIndex = index;
                // Draw Button (Remove Point)
                EditorGuiHelper.DrawButton("-", () => pathCopy.Path.RemoveAt(tempIndex));

                GUILayout.EndHorizontal();
            }
            // Set path to modified path copy.
            path = pathCopy;
#endif
        }

        public void OnSceneGUI()
        {
#if UNITY_EDITOR
            // Draw Path
            if (_pathInstance != null)
            {
                for (var index = 0; index < _pathInstance.Path.Count; index++)
                {
                    Vector3 vec3 = _pathInstance.Path[index];
                    // First Line is red.
                    if (index == 0)
                    {
                        Handles.color = Color.red;
                    }
                    else
                    {
                        Handles.color = Color.green;
                    }

                    // Draw handle lines.
                    if (index < _pathInstance.Path.Count - 1)
                    {
                        Vector3 next = _pathInstance.Path[index + 1];
                        Vector3 location = Vector3.Lerp(vec3, next, 0.5f);
                        Vector3 direction = (next - vec3).normalized;
                        Handles.DrawLine(vec3, next);
                        Handles.color = Color.magenta;
                        Handles.ArrowHandleCap(0, location, Quaternion.LookRotation(direction, Vector3.up), 1, EventType.Repaint);

                        if (index == _pathInstance.Path.Count - 2)
                        {
                            // Handles.color = Color.green;
                            // Handles.DrawLine(next, _pathInstance.Path[0]);
                            // Handles.color = Color.magenta;
                            // Handles.ArrowHandleCap(0, location, Quaternion.Euler(direction), 1, EventType.Repaint);
                        }
                    }
                    Handles.Label(_pathInstance.Path[index] + new Vector3(0, 1, 0), $"Position: {index}");

                    // Draw translation handles to be able to modify the values in the scene.

                    _pathInstance.Path[index] = Handles.PositionHandle(_pathInstance.Path[index], Quaternion.identity);
                }
            }
#endif
        }
    }
}