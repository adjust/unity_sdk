using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(AdjustSettings))]
    public class AdjustSettingsEditor : Editor
    {
        SerializedProperty isPostProcessingEnabled;
#if UNITY_IOS
        SerializedProperty isiOS14ProcessingEnabled;
        SerializedProperty iOSUserTrackingUsageDescription;
        SerializedProperty iOSUrlSchemes;
        SerializedProperty iOSUniversalLinksDomains;
#elif UNITY_ANDROID
        SerializedProperty androidUriSchemes;
#endif


        void OnEnable()
        {
            isPostProcessingEnabled = serializedObject.FindProperty("isPostProcessingEnabled");
#if UNITY_IOS
            isiOS14ProcessingEnabled = serializedObject.FindProperty("isiOS14ProcessingEnabled");
            iOSUserTrackingUsageDescription = serializedObject.FindProperty("_iOSUserTrackingUsageDescription");
            iOSUrlSchemes = serializedObject.FindProperty("_iOSUrlSchemes");
            iOSUniversalLinksDomains = serializedObject.FindProperty("_iOSUniversalLinksDomains");
#elif UNITY_ANDROID
            androidUriSchemes = serializedObject.FindProperty("androidUriSchemes");
#endif
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(isPostProcessingEnabled);
            if (isPostProcessingEnabled.boolValue)
            {
#if UNITY_IOS
                EditorGUILayout.PropertyField(isiOS14ProcessingEnabled, new GUIContent("Is iOS 14 Processing Enabled"));
                EditorGUILayout.PropertyField(iOSUserTrackingUsageDescription, new GUIContent("User Tracking Usage Description"));
#endif
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("DEEP LINKING:", EditorStyles.boldLabel);
#if UNITY_IOS
                EditorGUILayout.PropertyField(iOSUrlSchemes, new GUIContent("URL Schemes"), true);
                EditorGUILayout.PropertyField(iOSUniversalLinksDomains, new GUIContent("Universal Links Domains"), true);
#elif UNITY_ANDROID
                EditorGUILayout.PropertyField(androidUriSchemes, new GUIContent("URI Schemes"), true);
                EditorGUILayout.HelpBox(
                    "Please note that Adjust SDK doesn't remove existing URI Schemes, " +
                    "so if you need to clean previously added entries, " +
                    "you need to do it manually from \"Assets/Plugins/Android/AndroidManifest.xml\"", MessageType.Info, true);
#endif
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
