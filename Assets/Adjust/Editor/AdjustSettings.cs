// Inspired by: https://github.com/facebook/facebook-sdk-for-unity/blob/master/Facebook.Unity.Settings/FacebookSettings.cs

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdjustSettings : ScriptableObject
{
    private static AdjustSettings instance;

    [SerializeField]
    private bool isPostProcessingEnabled = true;
    [SerializeField]
    private bool isiOS14ProcessingEnabled = false;
    [SerializeField]
    private string userTrackingUsageDescription = "";

    [Header("DEEP LINKING:")]
    [Space(5)]
    [SerializeField]
    private bool urlSchemesDeepLinksEnabled = false;
    [SerializeField]
    private string[] urlSchemes = new string[0];
    [SerializeField]
    private bool universalLinksEnabled = false;
    [SerializeField]
    private string[] domains = new string[0];
    [SerializeField]
    private bool androidUriSchemesEnabled = false;
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
                AssetDatabase.CreateAsset(instance, assetPath);

                // Before switching to AssetsDatabase, EditorPrefs were used to write 'adjustiOS14Support' key.
                // Check if this key still exists in EditorPrefs.
                // If yes, migrate the value to AdjustSettings.asset and remove the key from EditorPrefs.
                if (EditorPrefs.HasKey("adjustiOS14Support"))
                {
                    UnityEngine.Debug.Log("[Adjust]: Found 'adjustiOS14Support' key in EditorPrefs.");
                    UnityEngine.Debug.Log("[Adjust]: Migrating that value to AdjustSettings.asset.");
                    IsiOS14ProcessingEnabled = EditorPrefs.GetBool("adjustiOS14Support", false);
                    EditorPrefs.DeleteKey("adjustiOS14Support");
                    UnityEngine.Debug.Log("[Adjust]: Key 'adjustiOS14Support' removed from EditorPrefs.");
                }
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

    public static bool IsPostProcessingEnabled
    {
        get => Instance.isPostProcessingEnabled;
        set => Instance.isPostProcessingEnabled = value;
    }

    public static bool IsiOS14ProcessingEnabled
    {
        get => Instance.isiOS14ProcessingEnabled;
        set => Instance.isiOS14ProcessingEnabled = value;
    }

    public static bool UrlSchemesDeepLinksEnabled
    {
        get => Instance.urlSchemesDeepLinksEnabled;
        set => Instance.urlSchemesDeepLinksEnabled = value;
    }

    public static string[] UrlSchemes
    {
        get => Instance.urlSchemes;
    }

    public static string UserTrackingUsageDescription
    {
        get => Instance.userTrackingUsageDescription;
        set => Instance.userTrackingUsageDescription = value;
    }

    public static bool UniversalLinksEnabled
    {
        get => Instance.universalLinksEnabled;
        set => Instance.universalLinksEnabled = value;
    }

    public static string[] Domains
    {
        get => Instance.domains;
        set => Instance.domains = value;
    }

    public static bool AndroidUriSchemesEnabled
    {
        get => Instance.androidUriSchemesEnabled;
        set => Instance.androidUriSchemesEnabled = value;
    }

    public static string[] AndroidUriSchemes
    {
        get => Instance.androidUriSchemes;
        set => Instance.androidUriSchemes = value;
    }
}
