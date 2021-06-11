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
        SerializedProperty iOSUrlSchemes;
        SerializedProperty iOSUniversalLinksDomains;
        SerializedProperty androidUriSchemes;


        void OnEnable()
        {
            isPostProcessingEnabled = serializedObject.FindProperty("isPostProcessingEnabled");
            isiOS14ProcessingEnabled = serializedObject.FindProperty("isiOS14ProcessingEnabled");
            iOSUserTrackingUsageDescription = serializedObject.FindProperty("_iOSUserTrackingUsageDescription");
            iOSUrlSchemes = serializedObject.FindProperty("_iOSUrlSchemes");
            iOSUniversalLinksDomains = serializedObject.FindProperty("_iOSUniversalLinksDomains");
            androidUriSchemes = serializedObject.FindProperty("androidUriSchemes");
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
                EditorGUILayout.Space(16);
                EditorGUILayout.LabelField("DEEP LINKING:", EditorStyles.boldLabel);
#if UNITY_IOS
                EditorGUILayout.PropertyField(iOSUrlSchemes, new GUIContent("URL Schemes"));
                EditorGUILayout.PropertyField(iOSUniversalLinksDomains, new GUIContent("Universal Links Domains"));
#elif UNITY_ANDROID
                EditorGUILayout.PropertyField(androidUriSchemes, new GUIContent("URI Schemes"));
#endif
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
