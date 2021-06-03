using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(Adjust))]
    public class AdjustCustomEditor : Editor
    {
        private List<string> deeplinkingParameters;
        private List<string> universalLinkDomains;
        private List<string> androidURISchemes;

        void OnEnable()
        {
            deeplinkingParameters = AdjustSettings.UrlSchemes;
            universalLinkDomains = AdjustSettings.Domains;
            androidURISchemes = AdjustSettings.AndroidUriSchemes;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
#if UNITY_IOS

            AdjustSettings.UserTrackingUsageDescription = EditorGUILayout.TextField("User Tracking Usage Description", AdjustSettings.UserTrackingUsageDescription);

            GUILayout.Space(20f);
            AdjustSettings.UrlSchemesDeepLinksEnabled = GUILayout.Toggle(AdjustSettings.UrlSchemesDeepLinksEnabled, "URL schemes deep links enabled");
            if (AdjustSettings.UrlSchemesDeepLinksEnabled)
            {
                CreateListManipulationUI("Add URL schemes for deep linking", "URL scheme ", deeplinkingParameters);
            }

            GUILayout.Space(20f);
            AdjustSettings.UniversalLinksEnabled = GUILayout.Toggle(AdjustSettings.UniversalLinksEnabled, "Universal links enabled");
            if (AdjustSettings.UniversalLinksEnabled)
            {
                CreateListManipulationUI("Add domains for universal linking", "Domain ", universalLinkDomains);
            }
#endif
#if UNITY_ANDROID
            GUILayout.Space(20f);
            AdjustSettings.AndroidUriSchemesEnabled = GUILayout.Toggle(AdjustSettings.AndroidUriSchemesEnabled, "Android URI schemes enabled");
            if (AdjustSettings.AndroidUriSchemesEnabled)
            {
                CreateListManipulationUI("Add URI schemes for Android", "URI scheme ", androidURISchemes);
            }
#endif
            serializedObject.ApplyModifiedProperties();
        }

        private void CreateListManipulationUI(string listManipulationUIHeader, string listItemName, List<string> manipulatedList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(listManipulationUIHeader, EditorStyles.boldLabel);
            if (GUILayout.Button("+"))
            {
                manipulatedList.Add("");
            }
            if (GUILayout.Button("-"))
            {
                if (manipulatedList.Count > 0)
                {
                    manipulatedList.RemoveAt(manipulatedList.Count - 1);
                }
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < manipulatedList.Count; i++)
            {
                string newKeyProperty = manipulatedList[i];
                manipulatedList[i] = EditorGUILayout.TextField(listItemName + i, newKeyProperty);
                EditorGUILayout.Space();
            }
        }
    }
}
