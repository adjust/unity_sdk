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
                adjust.appToken = EditorGUILayout.TextField("App Token", adjust.appToken);
                adjust.environment = (AdjustEnvironment)EditorGUILayout.EnumPopup("Environment", adjust.environment);
                adjust.logLevel = (AdjustLogLevel)EditorGUILayout.EnumPopup("Log Level", adjust.logLevel);
                // TODO: URL strategy missing
                adjust.sendInBackground = EditorGUILayout.Toggle("Send In Background", adjust.sendInBackground);
                adjust.launchDeferredDeeplink = EditorGUILayout.Toggle("Launch Deferred Deep Link", adjust.launchDeferredDeeplink);
                adjust.costDataInAttribution = EditorGUILayout.Toggle("Cost Data In Attribution Callback", adjust.costDataInAttribution);
                adjust.linkMe = EditorGUILayout.Toggle("LinkMe", adjust.linkMe);
                adjust.defaultTracker = EditorGUILayout.TextField("Default Tracker", adjust.defaultTracker);
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ANDROID SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adjust.preinstallTracking = EditorGUILayout.Toggle("Preinstall Tracking", adjust.preinstallTracking);
                adjust.preinstallFilePath = EditorGUILayout.TextField("Preinstall File Path", adjust.preinstallFilePath);
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("IOS SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adjust.adServices = EditorGUILayout.Toggle("AdServices Info Reading", adjust.adServices);
                adjust.idfaReading = EditorGUILayout.Toggle("IDFA Info Reading", adjust.idfaReading);
                adjust.skanAttribution = EditorGUILayout.Toggle("SKAdNetwork Handling", adjust.skanAttribution);
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
