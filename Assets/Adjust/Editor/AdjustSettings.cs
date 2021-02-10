// Inspired by: https://github.com/facebook/facebook-sdk-for-unity/blob/master/Facebook.Unity.Settings/FacebookSettings.cs

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AdjustSettings : ScriptableObject
{
    private static AdjustSettings instance;

    [SerializeField]
    private bool isPostProcessingEnabled = true;
    [SerializeField]
    private bool isiOS14ProcessingEnabled = false;

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
        get
        {
            return Instance.isPostProcessingEnabled;
        }

        set
        {
            if (Instance.isPostProcessingEnabled != value)
            {
                Instance.isPostProcessingEnabled = value;
            }
        }
    }

    public static bool IsiOS14ProcessingEnabled
    {
        get
        {
            return Instance.isiOS14ProcessingEnabled;
        }

        set
        {
            if (Instance.isiOS14ProcessingEnabled != value)
            {
                Instance.isiOS14ProcessingEnabled = value;
            }
        }
    }
}
