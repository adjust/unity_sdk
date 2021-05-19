using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(Adjust))]
    public class CustomAdjustEditor : Editor
    {
        private List<string> deeplinkingParameters;
        private List<string> universalLinkDomains;
        void OnEnable()
        {
            deeplinkingParameters = AdjustSettings.UrlSchemes;
            universalLinkDomains = AdjustSettings.Domains;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.TextField("User Tracking Usage Description", AdjustSettings.UserTrackingUsageDescription);

            GUILayout.Space(20f);
            AdjustSettings.UrlSchemesDeepLinksEnabled = GUILayout.Toggle(AdjustSettings.UrlSchemesDeepLinksEnabled, "URL schemes deep links enabled");
            if (AdjustSettings.UrlSchemesDeepLinksEnabled)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Add URL schemes for deep linking", EditorStyles.boldLabel);
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
                    deeplinkingParameters[i] = EditorGUILayout.TextField("URL scheme " + i, newKeyProperty);
                    EditorGUILayout.Space();
                }
            }

            GUILayout.Space(20f);
            AdjustSettings.UniversalLinksEnabled = GUILayout.Toggle(AdjustSettings.UniversalLinksEnabled, "Universal links enabled");
            if (AdjustSettings.UniversalLinksEnabled)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Add domains for Universal linking", EditorStyles.boldLabel);
                if (GUILayout.Button("+"))
                {
                    universalLinkDomains.Add("");
                }
                if (GUILayout.Button("-"))
                {
                    universalLinkDomains.RemoveAt(deeplinkingParameters.Count - 1);
                }
                GUILayout.EndHorizontal();

                for (int i = 0; i < universalLinkDomains.Count; i++)
                {
                    string newKeyProperty = universalLinkDomains[i];
                    universalLinkDomains[i] = EditorGUILayout.TextField("Domain " + i, newKeyProperty);
                    EditorGUILayout.Space();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
