using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(Adjust))]
    public class AdjustCustomEditor : Editor
    {
        private Editor settingsEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space(16);
            EditorGUILayout.LabelField("POST PROCESSING SETTINGS:", EditorStyles.boldLabel);

            if (settingsEditor == null)
            {
                settingsEditor = CreateEditor(AdjustSettings.Instance);
            }
            settingsEditor.OnInspectorGUI();
        }
    }
}
