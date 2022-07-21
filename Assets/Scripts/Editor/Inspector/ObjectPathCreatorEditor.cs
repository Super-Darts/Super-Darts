/*
 *  Author: James Greensill
 *  Usage:  ObjectPathCreatorEditor is used to display a custom inspector for the PathCreator.
 */

using Assets.Scripts.ObjectPathing;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(ObjectPathCreator))]
    public class ObjectPathCreatorEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Instance of ObjectPathCreator.
        /// </summary>
        private ObjectPathCreator _selectedInstance = null;

        private void OnValidate()
        {
            // Cast the target to the correct type
            _selectedInstance = (ObjectPathCreator)target;
        }

        private void OnSceneGUI()
        {
            // If there is an instance, draw the Scene GUI.
            if (_selectedInstance)
            {
                _selectedInstance.OnSceneGUI();
            }
        }

        /// <summary>
        /// Inspector Gui Gets Called.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // try get the target.
            if (_selectedInstance == null)
            {
                _selectedInstance = (ObjectPathCreator)target;
            }

            // if there is no instance, return.
            if (_selectedInstance == null)
            {
                return;
            }

            // draw the instance inspector.
            _selectedInstance.DrawInspector();
        }
    }
}