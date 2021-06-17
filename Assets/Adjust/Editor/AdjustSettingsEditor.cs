using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(AdjustSettings))]
    public class AdjustSettingsEditor : Editor
    {
        SerializedProperty isPostProcessingEnabled;
        SerializedProperty isiOS14ProcessingEnabled;
        SerializedProperty iOSUserTrackingUsageDescription;
        SerializedProperty iOSUrlName;
        SerializedProperty iOSUrlSchemes;
        SerializedProperty iOSUniversalLinksDomains;
        SerializedProperty androidUriSchemes;


        void OnEnable()
        {
            isPostProcessingEnabled = serializedObject.FindProperty("isPostProcessingEnabled");
            isiOS14ProcessingEnabled = serializedObject.FindProperty("isiOS14ProcessingEnabled");
            iOSUserTrackingUsageDescription = serializedObject.FindProperty("_iOSUserTrackingUsageDescription");
            iOSUrlName = serializedObject.FindProperty("_iOSUrlName");
            iOSUrlSchemes = serializedObject.FindProperty("_iOSUrlSchemes");
            iOSUniversalLinksDomains = serializedObject.FindProperty("_iOSUniversalLinksDomains");
            androidUriSchemes = serializedObject.FindProperty("androidUriSchemes");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(isPostProcessingEnabled);
            if (isPostProcessingEnabled.boolValue)
            {
                EditorGUILayout.PropertyField(isiOS14ProcessingEnabled, new GUIContent("Is iOS 14 Processing Enabled"));
                EditorGUILayout.PropertyField(iOSUserTrackingUsageDescription, new GUIContent("iOS User Tracking Usage Description"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("DEEP LINKING:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(iOSUrlName, new GUIContent("iOS URL Name"), true);
                EditorGUILayout.PropertyField(iOSUrlSchemes, new GUIContent("iOS URL Schemes"), true);
                EditorGUILayout.PropertyField(iOSUniversalLinksDomains, new GUIContent("iOS Universal Links Domains"), true);
                EditorGUILayout.PropertyField(androidUriSchemes, new GUIContent("Android URI Schemes"), true);
                EditorGUILayout.HelpBox(
                    "Please note that Adjust SDK doesn't remove existing URI Schemes, " +
                    "so if you need to clean previously added entries, " +
                    "you need to do it manually from \"Assets/Plugins/Android/AndroidManifest.xml\"", MessageType.Info, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
