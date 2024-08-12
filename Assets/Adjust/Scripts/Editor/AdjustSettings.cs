// Inspired by: https://github.com/facebook/facebook-sdk-for-unity/blob/master/Facebook.Unity.Settings/FacebookSettings.cs

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AdjustSdk
{
    public class AdjustSettings : ScriptableObject
    {
        private static AdjustSettings instance;
        public const string AdjustSettingsExportPath = "Adjust/Resources/AdjustSettings.asset";

        [SerializeField]
        private bool _iOSFrameworkAdSupport = true;
        [SerializeField]
        private bool _iOSFrameworkAdServices = false;
        [SerializeField]
        private bool _iOSFrameworkAppTrackingTransparency = false;
        [SerializeField]
        private bool _iOSFrameworkStoreKit = false;
        [SerializeField]
        private bool _androidPermissionInternet = true;
        [SerializeField]
        private bool _androidPermissionInstallReferrerService = true;
        [SerializeField]
        private bool _androidPermissionAdId = true;
        [SerializeField]
        private bool _androidPermissionAccessNetworkState = false;
        [SerializeField]
        private string _iOSUserTrackingUsageDescription;
        [SerializeField]
        private string _iOSUrlIdentifier;
        [SerializeField]
        private string[] _iOSUrlSchemes = new string[0];
        [SerializeField]
        private string[] _iOSUniversalLinksDomains = new string[0];
        [SerializeField]
        private string[] androidUriSchemes = new string[0];

        public static AdjustSettings Instance
        {
            get
            {
                instance = NullableInstance;

                if (instance == null)
                {
                    // Create AdjustSettings.asset inside the folder in which AdjustSettings.cs reside.
                    instance = ScriptableObject.CreateInstance<AdjustSettings>();
                    var guids = AssetDatabase.FindAssets(string.Format("{0} t:script", "AdjustSettings"));
                    if (guids == null || guids.Length <= 0)
                    {
                        return instance;
                    }

                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]).Replace("AdjustSettings.cs", "AdjustSettings.asset");
                    Debug.Log("READ PATH = " + assetPath);
                    // AdjustSettings.asset will be stored inside of the Adjust/Resources directory
                    if (assetPath.StartsWith("Assets"))
                    {
                        // plugin located in Assets directory
                        string rootDir = assetPath.Replace("/Adjust/Scripts/Editor/AdjustSettings.asset", "");
                        Debug.Log("ROOT DIR = " + rootDir);
                        string adjustResourcesPath = Path.Combine(rootDir, "Adjust/Resources");
                        Debug.Log("ADJUST RESOURCES DIR = " + adjustResourcesPath);
                        if (!Directory.Exists(adjustResourcesPath))
                        {
                            Debug.Log("CREATING ADJUST RESOURCES DIR = " + adjustResourcesPath);
                            Directory.CreateDirectory(adjustResourcesPath);
                        }
                        assetPath = Path.Combine(rootDir, AdjustSettingsExportPath);
                        Debug.Log("FINAL ASSET PATH = " + assetPath);
                    }
                    else
                    {
                        // plugin located in Packages folder
                        string adjustResourcesPath = Path.Combine("Assets", "Adjust/Resources");
                        Debug.Log("ADJUST RESOURCES DIR = " + adjustResourcesPath);
                        if (!Directory.Exists(adjustResourcesPath))
                        {
                            Debug.Log("CREATING ADJUST RESOURCES DIR = " + adjustResourcesPath);
                            Directory.CreateDirectory(adjustResourcesPath);
                        }
                        assetPath = Path.Combine("Assets", AdjustSettingsExportPath);
                        Debug.Log("FINAL ASSET PATH = " + assetPath);
                    }

                    AssetDatabase.CreateAsset(instance, assetPath);
                }

                return instance;
            }
        }

        public static AdjustSettings NullableInstance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets(string.Format("{0} t:ScriptableObject", "AdjustSettings"));
                    if (guids == null || guids.Length <= 0)
                    {
                        return instance;
                    }
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    instance = (AdjustSettings)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AdjustSettings));
                }

                return instance;
            }
        }

        public static bool iOSFrameworkAdSupport
        {
            get { return Instance._iOSFrameworkAdSupport; }
            set { Instance._iOSFrameworkAdSupport = value; }
        }

        public static bool iOSFrameworkAdServices
        {
            get { return Instance._iOSFrameworkAdServices; }
            set { Instance._iOSFrameworkAdServices = value; }
        }

        public static bool iOSFrameworkAppTrackingTransparency
        {
            get { return Instance._iOSFrameworkAppTrackingTransparency; }
            set { Instance._iOSFrameworkAppTrackingTransparency = value; }
        }

        public static bool iOSFrameworkStoreKit
        {
            get { return Instance._iOSFrameworkStoreKit; }
            set { Instance._iOSFrameworkStoreKit = value; }
        }

        public static bool androidPermissionInternet
        {
            get { return Instance._androidPermissionInternet; }
            set { Instance._androidPermissionInternet = value; }
        }

        public static bool androidPermissionInstallReferrerService
        {
            get { return Instance._androidPermissionInstallReferrerService; }
            set { Instance._androidPermissionInstallReferrerService = value; }
        }

        public static bool androidPermissionAdId
        {
            get { return Instance._androidPermissionAdId; }
            set { Instance._androidPermissionAdId = value; }
        }

        public static bool androidPermissionAccessNetworkState
        {
            get { return Instance._androidPermissionAccessNetworkState; }
            set { Instance._androidPermissionAccessNetworkState = value; }
        }

        public static string iOSUserTrackingUsageDescription
        {
            get { return Instance._iOSUserTrackingUsageDescription; }
            set { Instance._iOSUserTrackingUsageDescription = value; }
        }

        public static string iOSUrlIdentifier
        {
            get { return Instance._iOSUrlIdentifier; }
            set { Instance._iOSUrlIdentifier = value; }
        }

        public static string[] iOSUrlSchemes
        {
            get { return Instance._iOSUrlSchemes; }
            set { Instance._iOSUrlSchemes = value; }
        }

        public static string[] iOSUniversalLinksDomains
        {
            get { return Instance._iOSUniversalLinksDomains; }
            set { Instance._iOSUniversalLinksDomains = value; }
        }

        public static string[] AndroidUriSchemes
        {
            get { return Instance.androidUriSchemes; }
            set { Instance.androidUriSchemes = value; }
        }
    }
}
