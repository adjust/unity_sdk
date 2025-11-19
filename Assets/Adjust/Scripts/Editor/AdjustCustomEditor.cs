using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace AdjustSdk
{
    [CustomEditor(typeof(Adjust))]
    public class AdjustCustomEditor : Editor
    {
        private Editor settingsEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var adjust = target as Adjust;
            GUIStyle darkerCyanTextFieldStyles = new GUIStyle(EditorStyles.boldLabel);
            darkerCyanTextFieldStyles.normal.textColor = new Color(0f/255f, 190f/255f, 190f/255f);

            // Not gonna ask: http://answers.unity.com/answers/1244650/view.html
            EditorGUILayout.Space();
            var origFontStyle = EditorStyles.label.fontStyle;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            adjust.startManually = EditorGUILayout.Toggle("START SDK MANUALLY", adjust.startManually, EditorStyles.toggle);
            EditorStyles.label.fontStyle = origFontStyle;
 
            using (new EditorGUI.DisabledScope(adjust.startManually))
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("MULTIPLATFORM SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                adjust.appToken = EditorGUILayout.TextField("App Token", adjust.appToken);
                adjust.environment = (AdjustEnvironment)EditorGUILayout.EnumPopup("Environment", adjust.environment);
                adjust.logLevel = (AdjustLogLevel)EditorGUILayout.EnumPopup("Log Level", adjust.logLevel);
                adjust.firstSessionDelay = EditorGUILayout.Toggle("First Session Delay", adjust.firstSessionDelay);
                adjust.sendInBackground = EditorGUILayout.Toggle("Send In Background", adjust.sendInBackground);
                adjust.launchDeferredDeeplink = EditorGUILayout.Toggle("Launch Deferred Deep Link", adjust.launchDeferredDeeplink);
                adjust.costDataInAttribution = EditorGUILayout.Toggle("Cost Data In Attribution Callback", adjust.costDataInAttribution);
                adjust.deviceIdsReadingOnce = EditorGUILayout.Toggle("Device IDs Reading Once", adjust.deviceIdsReadingOnce);
                adjust.eventDeduplicationIdsMaxSize = EditorGUILayout.IntField("Event Deduplication IDs Count", adjust.eventDeduplicationIdsMaxSize);
                adjust.defaultTracker = EditorGUILayout.TextField("Default Tracker", adjust.defaultTracker);
                
                // Store Info section - visually grouped
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Store Info:", EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                adjust.storeName = EditorGUILayout.TextField("Store Name", adjust.storeName);
                adjust.storeAppId = EditorGUILayout.TextField("Store App ID", adjust.storeAppId);
                EditorGUI.indentLevel -= 1;
                
                // URL Strategy and Data Residency section - visually grouped
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("URL Strategy And Data Residency:", EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                
                // URL Strategy Domains list
                if (adjust.urlStrategyDomains == null)
                {
                    adjust.urlStrategyDomains = new System.Collections.Generic.List<string>();
                }
                
                EditorGUILayout.LabelField("URL Strategy Domains", EditorStyles.label);
                EditorGUI.indentLevel += 1;
                int domainCount = adjust.urlStrategyDomains.Count;
                int newDomainCount = EditorGUILayout.IntField("Size", domainCount);
                if (newDomainCount != domainCount)
                {
                    while (adjust.urlStrategyDomains.Count < newDomainCount)
                    {
                        adjust.urlStrategyDomains.Add("");
                    }
                    while (adjust.urlStrategyDomains.Count > newDomainCount)
                    {
                        adjust.urlStrategyDomains.RemoveAt(adjust.urlStrategyDomains.Count - 1);
                    }
                }
                
                for (int i = 0; i < adjust.urlStrategyDomains.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    adjust.urlStrategyDomains[i] = EditorGUILayout.TextField("Element " + i, adjust.urlStrategyDomains[i]);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        adjust.urlStrategyDomains.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;
                
                adjust.shouldUseSubdomains = EditorGUILayout.Toggle("Should Use Subdomains", adjust.shouldUseSubdomains);
                adjust.isDataResidency = EditorGUILayout.Toggle("Is Data Residency", adjust.isDataResidency);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }

            if (settingsEditor == null)
            {
                settingsEditor = CreateEditor(AdjustSettings.Instance);
            }

            settingsEditor.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(adjust);
                EditorSceneManager.MarkSceneDirty(adjust.gameObject.scene);
            }
        }
    }
}
