using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdjustSdk
{
    [CustomEditor(typeof(AdjustSettings))]
    public class AdjustSettingsEditor : Editor
    {
        SerializedProperty iOSFrameworkAdSupport;
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
        SerializedProperty androidAppLinksDomains;
        SerializedProperty androidCustomActivityName;
        SerializedProperty androidUseAdjustBroadcastReceiver;

        void OnEnable()
        {
            iOSFrameworkAdSupport = serializedObject.FindProperty("_iOSFrameworkAdSupport");
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
            androidAppLinksDomains = serializedObject.FindProperty("_androidAppLinksDomains");
            androidCustomActivityName = serializedObject.FindProperty("_androidCustomActivityName");
            androidUseAdjustBroadcastReceiver = serializedObject.FindProperty("_androidUseAdjustBroadcastReceiver");
        }
        public override void OnInspectorGUI()
        {
            GUIStyle darkerCyanTextFieldStyles = new GUIStyle(EditorStyles.boldLabel);
            darkerCyanTextFieldStyles.normal.textColor = new Color(0f/255f, 190f/255f, 190f/255f);

            var adjust = FindObjectOfType<Adjust>();
            bool isStartManually = adjust != null && adjust.startManually;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("IOS SETTINGS:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            
            // Fields that should be disabled when startManually is checked
            using (new EditorGUI.DisabledScope(isStartManually))
            {
                if (adjust != null)
                {
                    EditorGUI.BeginChangeCheck();
                    adjust.linkMe = EditorGUILayout.Toggle("LinkMe", adjust.linkMe);
                    if (EditorGUI.EndChangeCheck())
                    {
                        EditorUtility.SetDirty(adjust);
                    }
                }
                
                if (adjust != null)
                {
                    EditorGUI.BeginChangeCheck();
                    adjust.idfaReading = EditorGUILayout.Toggle("IDFA Info Reading", adjust.idfaReading);
                    adjust.idfvReading = EditorGUILayout.Toggle("IDFV Info Reading", adjust.idfvReading);
                    adjust.adServices = EditorGUILayout.Toggle("AdServices Info Reading", adjust.adServices);
                    adjust.skanAttribution = EditorGUILayout.Toggle("SKAdNetwork Handling", adjust.skanAttribution);
                    adjust.appTrackingTransparencyUsage = EditorGUILayout.Toggle("App Tracking Transparency Usage", adjust.appTrackingTransparencyUsage);
                    adjust.attConsentWaitingInterval = EditorGUILayout.IntField("ATT Consent Waiting Interval", adjust.attConsentWaitingInterval);
                    if (EditorGUI.EndChangeCheck())
                    {
                        EditorUtility.SetDirty(adjust);
                    }
                }
                
                EditorGUILayout.PropertyField(iOSUserTrackingUsageDescription,
                    new GUIContent("User Tracking Description",
                        "String you would like to display to your users describing the reason " +
                        "behind asking for tracking permission."),
                    true);
            }
            
            // Link iOS Frameworks - NOT disabled
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Link iOS Frameworks", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(iOSFrameworkAdSupport,
                new GUIContent("AdSupport.framework",
                    "iOS framework needed to access IDFA value"),
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
            
            // Deep linking - NOT disabled, moved to end
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Deep linking", EditorStyles.boldLabel);
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
            EditorGUI.indentLevel -= 1;
            
            EditorGUI.indentLevel -= 1;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("ANDROID SETTINGS:", darkerCyanTextFieldStyles);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.Space();
            
            // Preinstall Tracking, Preinstall File Path, Facebook App ID - at top, disabled when startManually
            using (new EditorGUI.DisabledScope(isStartManually))
            {
                if (adjust != null)
                {
                    EditorGUI.BeginChangeCheck();
                    adjust.preinstallTracking = EditorGUILayout.Toggle("Preinstall Tracking", adjust.preinstallTracking);
                    adjust.preinstallFilePath = EditorGUILayout.TextField("Preinstall File Path", adjust.preinstallFilePath);
                    adjust.fbAppId = EditorGUILayout.TextField("Facebook App ID", adjust.fbAppId);
                    if (EditorGUI.EndChangeCheck())
                    {
                        EditorUtility.SetDirty(adjust);
                    }
                }
            }
            
            // Custom Android Activity Name - NOT disabled
            EditorGUILayout.PropertyField(androidCustomActivityName,
                new GUIContent("Custom Android Activity Name",
                    "In case you are using custom activity instead of the default Unity activity " +
                    "(com.unity3d.player.UnityPlayerActivity), please specify it's full name."),
                true);
            
            // Use Adjust Broadcast Receiver - NOT disabled
            EditorGUILayout.PropertyField(androidUseAdjustBroadcastReceiver,
                new GUIContent("Use Adjust Broadcast Receiver",
                    "When enabled, AdjustBroadcastReceiver will be added as a listener to INSTALL_REFERRER intent. " +
                    "If you have no use case for AdjustBroadcastReceiver in your app, you can disable this option. " +
                    "Note: SDK 5.x uses the modern Install Referrer Library, so this receiver may not be needed."),
                true);
            
            // Add Android Permissions - NOT disabled
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add Android Permissions", EditorStyles.boldLabel);
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
            
            // Deep linking - NOT disabled
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Deep linking", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(androidUriSchemes,
                new GUIContent("Android URI Schemes",
                    "URI schemes handled by your app. " +
                    "Make sure to enter just the scheme name with :// part at the end."),
                true);
            EditorGUILayout.PropertyField(androidAppLinksDomains,
                new GUIContent("Android App Links Domains",
                    "App Links domains handled by your app. " +
                    "Enter the domain (e.g., example.com or example.go.link). " +
                    "If you need to specify a path, use format: domain/path (e.g., adj.st/blah). " +
                    "The SDK will automatically add android:autoVerify=\"true\" and parse host/pathPrefix correctly."),
                true);
            EditorGUILayout.HelpBox(
                "Please note that Adjust SDK doesn't remove existing URI Schemes or App Links, " +
                "so if you need to clean previously added entries, " +
                "you need to do it manually from \"Assets/Plugins/Android/AndroidManifest.xml\"",
                MessageType.Info,
                true);
            EditorGUI.indentLevel -= 1;
            
            EditorGUI.indentLevel -= 1;
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
