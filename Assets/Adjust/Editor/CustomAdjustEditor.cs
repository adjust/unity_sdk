using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(Adjust))]
    public class CustomAdjustEditor : Editor
    {
        List<string> deeplinkingParameters;
        void OnEnable()
        {
            deeplinkingParameters = AdjustSettings.UrlSchemes;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.TextField("User Tracking Usage Description", AdjustSettings.UserTrackingUsageDescription);

            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Add URL schemes for deeplinking", EditorStyles.boldLabel);
            if (GUILayout.Button("+"))
            {
                deeplinkingParameters.Add("");
            }
            if (GUILayout.Button("-"))
            {
                deeplinkingParameters.RemoveAt(deeplinkingParameters.Count - 1);
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < deeplinkingParameters.Count; i++)
            {
                string newKeyProperty = deeplinkingParameters[i];
                deeplinkingParameters[i] = EditorGUILayout.TextField("New Deeplink " + i, newKeyProperty);
                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
