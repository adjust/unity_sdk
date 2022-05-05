using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace com.adjust.sdk
{
    [CustomEditor(typeof(AdjustSettings))]
    public class AdjustSettingsEditor : Editor
    {
        SerializedProperty iOSFrameworkAdSupport;
        SerializedProperty iOSFrameworkiAd;
        SerializedProperty iOSFrameworkAdServices;
        SerializedProperty iOSFrameworkAppTrackingTransparency;
        SerializedProperty iOSFrameworkStoreKit;
        SerializedProperty androidPermissionInternet;
        SerializedProperty androidPermissionInstallReferrerService;
        SerializedProperty androidPermissionAdId;
        SerializedProperty androidPermissionAccessNetworkState;
        SerializedProperty iOSUserTrackingUsageDescription;
        SerializedProperty iOSUrlIdentifier;
        SerializedProperty iOSUrlSchemes;
        SerializedProperty iOSUniversalLinksDomains;
        SerializedProperty androidUriSchemes;

        void OnEnable()
        {
            iOSFrameworkAdSupport = serializedObject.FindProperty("_iOSFrameworkAdSupport");
            iOSFrameworkiAd = serializedObject.FindProperty("_iOSFrameworkiAd");
            iOSFrameworkAdServices = serializedObject.FindProperty("_iOSFrameworkAdServices");
            iOSFrameworkAppTrackingTransparency = serializedObject.FindProperty("_iOSFrameworkAppTrackingTransparency");
            iOSFrameworkStoreKit = serializedObject.FindProperty("_iOSFrameworkStoreKit");
            androidPermissionInternet = serializedObject.FindProperty("_androidPermissionInternet");
            androidPermissionInstallReferrerService = serializedObject.FindProperty("_androidPermissionInstallReferrerService");
            androidPermissionAdId = serializedObject.FindProperty("_androidPermissionAdId");
            androidPermissionAccessNetworkState = serializedObject.FindProperty("_androidPermissionAccessNetworkState");
            iOSUserTrackingUsageDescription = serializedObject.FindProperty("_iOSUserTrackingUsageDescription");
            iOSUrlIdentifier = serializedObject.FindProperty("_iOSUrlIdentifier");
            iOSUrlSchemes = serializedObject.FindProperty("_iOSUrlSchemes");
            iOSUniversalLinksDomains = serializedObject.FindProperty("_iOSUniversalLinksDomains");
            androidUriSchemes = serializedObject.FindProperty("androidUriSchemes");
        }
        public override void OnInspectorGUI()
        {
            GUIStyle darkerCyanTextFieldStyles = new GUIStyle(EditorStyles.boldLabel);
            darkerCyanTextFieldStyles.normal.textColor = new Color(0f/255f, 190f/255f, 190f/255f);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("LINK IOS FRAMEWORKS:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(iOSFrameworkAdSupport,
                new GUIContent("AdSupport.framework",
                    "iOS framework needed to access IDFA value"),
                true);
            EditorGUILayout.PropertyField(iOSFrameworkiAd,
                new GUIContent("iAd.framework",
                    "iOS framework needed to support iAd based Apple Search Ads attribution"),
                true);
            EditorGUILayout.PropertyField(iOSFrameworkAdServices,
                new GUIContent("AdServices.framework",
                    "iOS framework needed to support AdServices based Apple Search Ads attribution"),
                true);
            EditorGUILayout.PropertyField(iOSFrameworkAppTrackingTransparency,
                new GUIContent("AppTrackingTransparency.framework",
                    "iOS framework needed to display tracking consent dialog"),
                true);
            EditorGUILayout.PropertyField(iOSFrameworkStoreKit,
                new GUIContent("StoreKit.framework",
                    "iOS framework needed to use SKAdNetwork capabilities"),
                true);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ADD ANDROID PERMISSIONS:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(androidPermissionInternet,
                new GUIContent("android.permission.INTERNET",
                    "Android permission needed to send data to Adjust backend"),
                true);
            EditorGUILayout.PropertyField(androidPermissionInstallReferrerService,
                new GUIContent("com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE",
                    "Android permission needed to read install referrer"),
                true);
            EditorGUILayout.PropertyField(androidPermissionAdId,
                new GUIContent("com.google.android.gms.permission.AD_ID",
                    "Android permission needed to read Google Advertising ID if you target API 33 or later"),
                true);
            EditorGUILayout.PropertyField(androidPermissionAccessNetworkState,
                new GUIContent("android.permission.ACCESS_NETWORK_STATE",
                    "Android permission needed to determine type of network device is connected to"),
                true);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("IOS PRIVACY:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(iOSUserTrackingUsageDescription,
                new GUIContent("User Tracking Description",
                    "String you would like to display to your users describing the reason " +
                    "behind asking for tracking permission."),
                true);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("DEEP LINKING:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(iOSUrlIdentifier,
                new GUIContent("iOS URL Identifier",
                    "Value of CFBundleURLName property of the root CFBundleURLTypes element. " +
                    "If not needed otherwise, value should be your bundle ID."),
                true);
            EditorGUILayout.PropertyField(iOSUrlSchemes,
                new GUIContent("iOS URL Schemes",
                    "URL schemes handled by your app. " +
                    "Make sure to enter just the scheme name without :// part at the end."),
                true);
            EditorGUILayout.PropertyField(iOSUniversalLinksDomains,
                new GUIContent("iOS Universal Links Domains",
                    "Associated domains handled by your app. State just the domain part without applinks: part in front."),
                true);
            EditorGUILayout.PropertyField(androidUriSchemes,
                new GUIContent("Android URI Schemes",
                    "URI schemes handled by your app. " +
                    "Make sure to enter just the scheme name with :// part at the end."),
                true);
            EditorGUILayout.HelpBox(
                "Please note that Adjust SDK doesn't remove existing URI Schemes, " +
                "so if you need to clean previously added entries, " +
                "you need to do it manually from \"Assets/Plugins/Android/AndroidManifest.xml\"",
                MessageType.Info,
                true);
            EditorGUI.indentLevel -= 1;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
