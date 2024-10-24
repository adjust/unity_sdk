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
#if UNITY_2019_3_OR_NEWER
using UnityEditor.iOS.Xcode.Extensions;
#endif
#endif

namespace AdjustSdk
{
    public class AdjustEditor : AssetPostprocessor
    {
        private const int AdjustEditorPostProcesssBuildPriority = 90;
        private const string TargetUnityIphonePodfileLine = "target 'Unity-iPhone' do";
        private const string UseFrameworksPodfileLine = "use_frameworks!";
        private const string UseFrameworksDynamicPodfileLine = "use_frameworks! :linkage => :dynamic";
        private const string UseFrameworksStaticPodfileLine = "use_frameworks! :linkage => :static";
        
        [PostProcessBuild(AdjustEditorPostProcesssBuildPriority)]
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

                // The Adjust SDK needs to have Obj-C exceptions enabled.
                // GCC_ENABLE_OBJC_EXCEPTIONS=YES
                string xcodeTargetUnityFramework = xcodeProject.TargetGuidByName("UnityFramework");
                Debug.Log("[Adjust]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
                xcodeProject.AddBuildProperty(xcodeTarget, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
                Debug.Log("[Adjust]: Obj-C exceptions enabled successfully.");
                if (!string.IsNullOrEmpty(xcodeTargetUnityFramework))
                {
                    Debug.Log("[Adjust]: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
                    xcodeProject.AddBuildProperty(xcodeTargetUnityFramework, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
                    Debug.Log("[Adjust]: Obj-C exceptions enabled successfully.");
                }

                // potential AdjustSigSdk.xcframework embedding
                Debug.Log("[Adjust]: Checking whether AdjustSigSdk.xcframework needs to be embedded or not...");
                EmbedAdjustSignatureIfNeeded(projectPath, xcodeProject, xcodeTarget);

                // Save the changes to Xcode project file.
                xcodeProject.WriteToFile(xcodeProjectPath);
    #endif
            }
        }

    #if UNITY_IOS
        // dynamic xcframework embedding logic adjusted and taken from:
        // https://github.com/AppLovin/AppLovin-MAX-Unity-Plugin/blob/master/DemoApp/Assets/MaxSdk/Scripts/IntegrationManager/Editor/AppLovinPostProcessiOS.cs
        private static void EmbedAdjustSignatureIfNeeded(string buildPath, PBXProject project, string targetGuid)
        {
            var podsDirectory = Path.Combine(buildPath, "Pods");

            if (!Directory.Exists(podsDirectory) || !ShouldEmbedDynamicLibraries(buildPath))
            {
                Debug.Log("[Adjust]: No need to embed AdjustSigSdk.xcframework.");
                return;
            }

            var dynamicLibraryPathToEmbed = GetAdjustSignaturePathToEmbed(podsDirectory, buildPath);
            if (dynamicLibraryPathToEmbed == null) {
                return;
            }

            Debug.Log("[Adjust]: It needs to be embedded. Starting the embedding process...");
#if UNITY_2019_3_OR_NEWER
            var fileGuid = project.AddFile(dynamicLibraryPathToEmbed, dynamicLibraryPathToEmbed);
            project.AddFileToEmbedFrameworks(targetGuid, fileGuid);
#else
            string runpathSearchPaths;
            runpathSearchPaths = project.GetBuildPropertyForAnyConfig(targetGuid, "LD_RUNPATH_SEARCH_PATHS");
            runpathSearchPaths += string.IsNullOrEmpty(runpathSearchPaths) ? "" : " ";

            // check if runtime search paths already contains the required search paths for dynamic libraries
            if (runpathSearchPaths.Contains("@executable_path/Frameworks")) {
                return;
            }

            runpathSearchPaths += "@executable_path/Frameworks";
            project.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", runpathSearchPaths);
#endif
            Debug.Log("[Adjust]: Embedding process completed.");
        }

        private static bool ShouldEmbedDynamicLibraries(string buildPath)
        {
            var podfilePath = Path.Combine(buildPath, "Podfile");
            if (!File.Exists(podfilePath)) {
                return false;
            }

            // if the Podfile doesn't have a `Unity-iPhone` target, we should embed the dynamic libraries
            var lines = File.ReadAllLines(podfilePath);
            var containsUnityIphoneTarget = lines.Any(line => line.Contains(TargetUnityIphonePodfileLine));
            if (!containsUnityIphoneTarget) {
                return true;
            }

            // if the Podfile does not have a `use_frameworks! :linkage => static` line, we should not embed the dynamic libraries
            var useFrameworksStaticLineIndex = Array.FindIndex(lines, line => line.Contains(UseFrameworksStaticPodfileLine));
            if (useFrameworksStaticLineIndex == -1) {
                return false;
            }

            // if more than one of the `use_frameworks!` lines are present, CocoaPods will use the last one
            var useFrameworksLineIndex = Array.FindIndex(lines, line => line.Trim() == UseFrameworksPodfileLine); // check for exact line to avoid matching `use_frameworks! :linkage => static/dynamic`
            var useFrameworksDynamicLineIndex = Array.FindIndex(lines, line => line.Contains(UseFrameworksDynamicPodfileLine));

            // check if `use_frameworks! :linkage => :static` is the last line of the three
            // if it is, we should embed the dynamic libraries
            return useFrameworksLineIndex < useFrameworksStaticLineIndex && useFrameworksDynamicLineIndex < useFrameworksStaticLineIndex;
        }

        private static string GetAdjustSignaturePathToEmbed(string podsDirectory, string buildPath)
        {
            var adjustSignatureFrameworkToEmbed = "AdjustSigSdk.xcframework";

            // both .framework and .xcframework are directories, not files
            var directories = Directory.GetDirectories(podsDirectory, adjustSignatureFrameworkToEmbed, SearchOption.AllDirectories);
            if (directories.Length <= 0) {
                return null;
            }

            var dynamicLibraryAbsolutePath = directories[0];
            var relativePath = GetDynamicLibraryRelativePath(dynamicLibraryAbsolutePath);
            return relativePath;
        }

        private static string GetDynamicLibraryRelativePath(string dynamicLibraryAbsolutePath)
        {
            var index = dynamicLibraryAbsolutePath.LastIndexOf("Pods", StringComparison.Ordinal);
            return dynamicLibraryAbsolutePath.Substring(index);
        }

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
}