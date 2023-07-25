using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
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
                adjust.urlStrategy = (AdjustUrlStrategy)EditorGUILayout.EnumPopup("URL Strategy", adjust.urlStrategy);
                adjust.eventBuffering = EditorGUILayout.Toggle("Event Buffering", adjust.eventBuffering);
                adjust.sendInBackground = EditorGUILayout.Toggle("Send In Background", adjust.sendInBackground);
                adjust.launchDeferredDeeplink = EditorGUILayout.Toggle("Launch Deferred Deep Link", adjust.launchDeferredDeeplink);
                adjust.needsCost = EditorGUILayout.Toggle("Cost Data In Attribution Callback", adjust.needsCost);
                adjust.coppaCompliant = EditorGUILayout.Toggle("COPPA Compliant", adjust.coppaCompliant);
                adjust.linkMe = EditorGUILayout.Toggle("LinkMe", adjust.linkMe);
                adjust.defaultTracker = EditorGUILayout.TextField("Default Tracker", adjust.defaultTracker);
                adjust.startDelay = EditorGUILayout.DoubleField("Start Delay", adjust.startDelay);
                EditorGUILayout.LabelField("App Secret:", EditorStyles.label);
                EditorGUI.indentLevel += 1;
                adjust.secretId = EditorGUILayout.LongField("Secret ID", adjust.secretId);
                adjust.info1 = EditorGUILayout.LongField("Info 1", adjust.info1);
                adjust.info2 = EditorGUILayout.LongField("Info 2", adjust.info2);
                adjust.info3 = EditorGUILayout.LongField("Info 3", adjust.info3);
                adjust.info4 = EditorGUILayout.LongField("Info 4", adjust.info4);
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ANDROID SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adjust.preinstallTracking = EditorGUILayout.Toggle("Preinstall Tracking", adjust.preinstallTracking);
                adjust.preinstallFilePath = EditorGUILayout.TextField("Preinstall File Path", adjust.preinstallFilePath);
                adjust.playStoreKidsApp = EditorGUILayout.Toggle("Play Store Kids App", adjust.playStoreKidsApp);
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("IOS SETTINGS:", darkerCyanTextFieldStyles);
                EditorGUI.indentLevel += 1;
                adjust.adServicesInfoReading = EditorGUILayout.Toggle("AdServices Info Reading", adjust.adServicesInfoReading);
                adjust.idfaInfoReading = EditorGUILayout.Toggle("IDFA Info Reading", adjust.idfaInfoReading);
                adjust.skAdNetworkHandling = EditorGUILayout.Toggle("SKAdNetwork Handling", adjust.skAdNetworkHandling);
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
