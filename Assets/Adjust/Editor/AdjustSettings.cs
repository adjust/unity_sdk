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
    private string _iOSUserTrackingUsageDescription = "";
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
                AssetDatabase.CreateAsset(instance, assetPath);

                // Before switching to AssetsDatabase, EditorPrefs were used to write 'adjustiOS14Support' key.
                // Check if this key still exists in EditorPrefs.
                // If yes, migrate the value to AdjustSettings.asset and remove the key from EditorPrefs.
                if (EditorPrefs.HasKey("adjustiOS14Support"))
                {
                    Debug.Log("[Adjust]: Found 'adjustiOS14Support' key in EditorPrefs.");
                    Debug.Log("[Adjust]: Migrating that value to AdjustSettings.asset.");
                    IsiOS14ProcessingEnabled = EditorPrefs.GetBool("adjustiOS14Support", false);
                    EditorPrefs.DeleteKey("adjustiOS14Support");
                    Debug.Log("[Adjust]: Key 'adjustiOS14Support' removed from EditorPrefs.");
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

    public static string iOSUserTrackingUsageDescription
    {
        get => Instance._iOSUserTrackingUsageDescription;
        set => Instance._iOSUserTrackingUsageDescription = value;
    }

    public static string[] iOSUrlSchemes
    {
        get => Instance._iOSUrlSchemes;
        set => Instance._iOSUrlSchemes = value;
    }

    public static string[] iOSUniversalLinksDomains
    {
        get => Instance._iOSUniversalLinksDomains;
        set => Instance._iOSUniversalLinksDomains = value;
    }

    public static string[] AndroidUriSchemes
    {
        get => Instance.androidUriSchemes;
        set => Instance.androidUriSchemes = value;
    }
}
