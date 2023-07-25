using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class AdjustEditor : AssetPostprocessor
{
    [MenuItem("Assets/Adjust/Export Unity Package")]
    public static void ExportAdjustUnityPackage()
    {
        string exportedFileName = "Adjust.unitypackage";
        string assetsPath = "Assets/Adjust";
        List<string> assetsToExport = new List<string>();

        // Adjust Assets.
        assetsToExport.Add(assetsPath + "/3rd Party/SimpleJSON.cs");

        assetsToExport.Add(assetsPath + "/Android/adjust-android.jar");
        assetsToExport.Add(assetsPath + "/Android/AdjustAndroid.cs");
        assetsToExport.Add(assetsPath + "/Android/AdjustAndroidManifest.xml");

        assetsToExport.Add(assetsPath + "/Editor/AdjustEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustSettings.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustSettingsEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustCustomEditor.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustEditorPreprocessor.cs");

        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.cs");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.prefab");
        assetsToExport.Add(assetsPath + "/ExampleGUI/ExampleGUI.unity");

        assetsToExport.Add(assetsPath + "/iOS/ADJAttribution.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJConfig.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJEvent.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJEventFailure.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJEventSuccess.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJLogger.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJSessionFailure.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJSessionSuccess.h");
        assetsToExport.Add(assetsPath + "/iOS/ADJSubscription.h");
        assetsToExport.Add(assetsPath + "/iOS/Adjust.h");
        assetsToExport.Add(assetsPath + "/iOS/AdjustiOS.cs");
        assetsToExport.Add(assetsPath + "/iOS/AdjustSdk.a");
        assetsToExport.Add(assetsPath + "/iOS/AdjustUnity.h");
        assetsToExport.Add(assetsPath + "/iOS/AdjustUnity.mm");
        assetsToExport.Add(assetsPath + "/iOS/AdjustUnityDelegate.h");
        assetsToExport.Add(assetsPath + "/iOS/AdjustUnityDelegate.mm");

        assetsToExport.Add(assetsPath + "/Prefab/Adjust.prefab");

        assetsToExport.Add(assetsPath + "/Unity/Adjust.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustAppStoreSubscription.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustAttribution.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustConfig.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustEnvironment.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustEvent.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustEventFailure.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustEventSuccess.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustLogLevel.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustPlayStoreSubscription.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustSessionFailure.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustSessionSuccess.cs");
        assetsToExport.Add(assetsPath + "/Unity/AdjustUtils.cs");

        assetsToExport.Add(assetsPath + "/Windows/AdjustWindows.cs");
        assetsToExport.Add(assetsPath + "/Windows/WindowsPcl.dll");
        assetsToExport.Add(assetsPath + "/Windows/WindowsUap.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/Win10Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/Win81Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Stubs/WinWsInterface.dll");
        assetsToExport.Add(assetsPath + "/Windows/W81/AdjustWP81.dll");
        assetsToExport.Add(assetsPath + "/Windows/W81/Win81Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/WS/AdjustWS.dll");
        assetsToExport.Add(assetsPath + "/Windows/WS/WinWsInterface.dll");
        assetsToExport.Add(assetsPath + "/Windows/WU10/AdjustUAP10.dll");
        assetsToExport.Add(assetsPath + "/Windows/WU10/Win10Interface.dll");
        assetsToExport.Add(assetsPath + "/Windows/Newtonsoft.Json.dll");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
    
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string projectPath)
    {
        RunPostBuildScript(target: target, projectPath: projectPath);
    }

    private static void RunPostBuildScript(BuildTarget target, string projectPath = "")
    {
        if (target == BuildTarget.iOS)
        {
#if UNITY_IOS
            Debug.Log("[Adjust]: Starting to perform post build tasks for iOS platform.");

            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

#if UNITY_2019_3_OR_NEWER
            string xcodeTarget = xcodeProject.GetUnityMainTargetGuid();
#else
            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
#endif
            HandlePlistIosChanges(projectPath);

            if (AdjustSettings.iOSUniversalLinksDomains.Length > 0)
            {
                AddUniversalLinkDomains(xcodeProject, xcodeProjectPath, xcodeTarget);
            }

            // If enabled by the user, Adjust SDK will try to add following frameworks to your project:
            // - AdSupport.framework (needed for access to IDFA value)
            // - AdServices.framework (needed in case you are running ASA campaigns)
            // - StoreKit.framework (needed for communication with SKAdNetwork framework)
            // - AppTrackingTransparency.framework (needed for information about user's consent to be tracked)
            // In case you don't need any of these, feel free to remove them from your app.

            if (AdjustSettings.iOSFrameworkAdSupport)
            {
                Debug.Log("[Adjust]: Adding AdSupport.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
                Debug.Log("[Adjust]: AdSupport.framework added successfully.");
            }
            else
            {
                Debug.Log("[Adjust]: Skipping AdSupport.framework linking.");
            }
            if (AdjustSettings.iOSFrameworkAdServices)
            {
                Debug.Log("[Adjust]: Adding AdServices.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AdServices.framework", true);
                Debug.Log("[Adjust]: AdServices.framework added successfully.");
            }
            else
            {
                Debug.Log("[Adjust]: Skipping AdServices.framework linking.");
            }
            if (AdjustSettings.iOSFrameworkStoreKit)
            {
                Debug.Log("[Adjust]: Adding StoreKit.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "StoreKit.framework", true);
                Debug.Log("[Adjust]: StoreKit.framework added successfully.");
            }
            else
            {
                Debug.Log("[Adjust]: Skipping StoreKit.framework linking.");
            }
            if (AdjustSettings.iOSFrameworkAppTrackingTransparency)
            {
                Debug.Log("[Adjust]: Adding AppTrackingTransparency.framework to Xcode project.");
                xcodeProject.AddFrameworkToProject(xcodeTarget, "AppTrackingTransparency.framework", true);
                Debug.Log("[Adjust]: AppTrackingTransparency.framework added successfully.");
            }
            else
            {
                Debug.Log("[Adjust]: Skipping AppTrackingTransparency.framework linking.");
            }

            // The Adjust SDK needs to have -ObjC flag set in other linker flags section because of it's categories.
            // OTHER_LDFLAGS -ObjC
            //
            // Seems that in newer Unity IDE versions adding -ObjC flag to Unity-iPhone target doesn't do the trick.
            // Adding -ObjC to UnityFramework target however does make things work nicely again.
            // This happens because Unity is linking SDK's static library into UnityFramework target.
            // Check for presence of UnityFramework target and if there, include -ObjC flag inside of it.
            Debug.Log("[Adjust]: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS) of Unity-iPhone target.");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-ObjC");
            Debug.Log("[Adjust]: -ObjC successfully added to other linker flags.");
            string xcodeTargetUnityFramework = xcodeProject.TargetGuidByName("UnityFramework");
            if (!string.IsNullOrEmpty(xcodeTargetUnityFramework))
            {
                Debug.Log("[Adjust]: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS) of UnityFramework target.");
                xcodeProject.AddBuildProperty(xcodeTargetUnityFramework, "OTHER_LDFLAGS", "-ObjC");
                Debug.Log("[Adjust]: -ObjC successfully added to other linker flags.");
            }

            // The Adjust SDK needs to have Obj-C exceptions enabled.
            // GCC_ENABLE_OBJC_EXCEPTIONS=YES
            Debug.Log("[Adjust]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
            xcodeProject.AddBuildProperty(xcodeTarget, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            Debug.Log("[Adjust]: Obj-C exceptions enabled successfully.");
            if (!string.IsNullOrEmpty(xcodeTargetUnityFramework))
            {
                Debug.Log("[Adjust]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
                xcodeProject.AddBuildProperty(xcodeTargetUnityFramework, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
                Debug.Log("[Adjust]: Obj-C exceptions enabled successfully.");
            }

            if (xcodeProject.ContainsFileByProjectPath("Libraries/Adjust/iOS/AdjustSigSdk.a"))
            {
                if (!string.IsNullOrEmpty(xcodeTargetUnityFramework))
                {
                    xcodeProject.AddBuildProperty(xcodeTargetUnityFramework, "OTHER_LDFLAGS", "-force_load $(PROJECT_DIR)/Libraries/Adjust/iOS/AdjustSigSdk.a");
                }
                else
                {
                    xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-force_load $(PROJECT_DIR)/Libraries/Adjust/iOS/AdjustSigSdk.a");
                }
            }

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);
#endif
        }
    }

#if UNITY_IOS
    private static void HandlePlistIosChanges(string projectPath)
    {
        const string UserTrackingUsageDescriptionKey = "NSUserTrackingUsageDescription";

        // Check if needs to do any info plist change.
        bool hasUserTrackingDescription =
            !string.IsNullOrEmpty(AdjustSettings.iOSUserTrackingUsageDescription);
        bool hasUrlSchemesDeepLinksEnabled = AdjustSettings.iOSUrlSchemes.Length > 0;

        if (!hasUserTrackingDescription && !hasUrlSchemesDeepLinksEnabled)
        {
            return;
        }

        // Get and read info plist.
        var plistPath = Path.Combine(projectPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        var plistRoot = plist.root;

        // Do the info plist changes.
        if (hasUserTrackingDescription)
        {
            if (plistRoot[UserTrackingUsageDescriptionKey] != null)
            {
                Debug.Log("[Adjust]: Overwritting User Tracking Usage Description.");
            }
            plistRoot.SetString(UserTrackingUsageDescriptionKey,
                AdjustSettings.iOSUserTrackingUsageDescription);
        }

        if (hasUrlSchemesDeepLinksEnabled)
        {
            AddUrlSchemesIOS(plistRoot, AdjustSettings.iOSUrlIdentifier, AdjustSettings.iOSUrlSchemes);
        }

        // Write any info plist change.
        File.WriteAllText(plistPath, plist.WriteToString());
    }

    private static void AddUrlSchemesIOS(PlistElementDict plistRoot, string urlIdentifier, string[] urlSchemes)
    {
        // Set Array for futher deeplink values.
        var urlTypesArray = CreatePlistElementArray(plistRoot, "CFBundleURLTypes");

        // Array will contain just one deeplink dictionary
        var urlSchemesItems = CreatePlistElementDict(urlTypesArray);
        urlSchemesItems.SetString("CFBundleURLName", urlIdentifier);
        var urlSchemesArray = CreatePlistElementArray(urlSchemesItems, "CFBundleURLSchemes");

        // Delete old deferred deeplinks URIs
        Debug.Log("[Adjust]: Removing deeplinks that already exist in the array to avoid duplicates.");
        foreach (var link in urlSchemes)
        {
            urlSchemesArray.values.RemoveAll(
                element => element != null && element.AsString().Equals(link));
        }

        Debug.Log("[Adjust]: Adding new deep links.");
        foreach (var link in urlSchemes.Distinct())
        {
            urlSchemesArray.AddString(link);
        }
    }

    private static PlistElementArray CreatePlistElementArray(PlistElementDict root, string key)
    {
        if (!root.values.ContainsKey(key))
        {
            Debug.Log(string.Format("[Adjust]: {0} not found in Info.plist. Creating a new one.", key));
            return root.CreateArray(key);
        }
        var result = root.values[key].AsArray();
        return result != null ? result : root.CreateArray(key);
    }

    private static PlistElementDict CreatePlistElementDict(PlistElementArray rootArray)
    {
        if (rootArray.values.Count == 0)
        {
            Debug.Log("[Adjust]: Deeplinks array doesn't contain dictionary for deeplinks. Creating a new one.");
            return rootArray.AddDict();
        }

        var urlSchemesItems = rootArray.values[0].AsDict();
        Debug.Log("[Adjust]: Reading deeplinks array");
        if (urlSchemesItems == null)
        {
            Debug.Log("[Adjust]: Deeplinks array doesn't contain dictionary for deeplinks. Creating a new one.");
            urlSchemesItems = rootArray.AddDict();
        }

        return urlSchemesItems;
    }

    private static void AddUniversalLinkDomains(PBXProject project, string xCodeProjectPath, string xCodeTarget)
    {
        string entitlementsFileName = "Unity-iPhone.entitlements";

        Debug.Log("[Adjust]: Adding associated domains to entitlements file.");
#if UNITY_2019_3_OR_NEWER
        var projectCapabilityManager = new ProjectCapabilityManager(xCodeProjectPath, entitlementsFileName, null, project.GetUnityMainTargetGuid());
#else
        var projectCapabilityManager = new ProjectCapabilityManager(xCodeProjectPath, entitlementsFileName, PBXProject.GetUnityTargetName());
#endif
        var uniqueDomains = AdjustSettings.iOSUniversalLinksDomains.Distinct().ToArray();
        const string applinksPrefix = "applinks:";
        for (int i = 0; i < uniqueDomains.Length; i++)
        {
            if (!uniqueDomains[i].StartsWith(applinksPrefix))
            {
                uniqueDomains[i] = applinksPrefix + uniqueDomains[i];
            }
        }

        projectCapabilityManager.AddAssociatedDomains(uniqueDomains);
        projectCapabilityManager.WriteToFile();

        Debug.Log("[Adjust]: Enabling Associated Domains capability with created entitlements file.");
        project.AddCapability(xCodeTarget, PBXCapabilityType.AssociatedDomains, entitlementsFileName);
    }
#endif
}
