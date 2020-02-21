using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AdjustImeiEditor
{
    private const string MenuItem0 = "Assets/AdjustImei/Is my app properly configured?";
    private const string MenuItem1 = "Assets/AdjustImei/Autorun post-build tasks";
    private const string MenuItem2 = "Assets/AdjustImei/Run post-build tasks";
    private const string MenuItem3 = "Assets/AdjustImei/Export Unity package";

    private static bool shouldAutorun = true;

    static AdjustImeiEditor()
    {
        EditorApplication.delayCall += () =>
        {
            shouldAutorun = EditorPrefs.GetBool(MenuItem1, true);
            Menu.SetChecked(MenuItem1, shouldAutorun);
        };
    }

    [MenuItem(MenuItem0)]
    static void IsAppProperlyConfigured()
    {
        bool isProperlyConfigured = DoesAppHaveAllThePermissions();
        if (isProperlyConfigured)
        {
            EditorUtility.DisplayDialog("Adjust IMEI Plugin", "Your app is properly configured for usage of the Adjust IMEI plugin.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("AdjustImei", "Your app is NOT properly configured for usage of the Adjust IMEI plugin."
                + "\n\nPlease perform one of the following tasks: "
                + "    \n\n- Enable \"Autorun post-build tasks\" option from the menu and rebuild your app."
                + "    \n- Select \"Run post-build tasks\" option from the menu."
                + "\n\nAfter that re-run this check to make sure you app is properly configured.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void AutorunPostBuildTasks()
    {
        shouldAutorun = !shouldAutorun;
        Menu.SetChecked(MenuItem1, shouldAutorun);
        EditorPrefs.SetBool(MenuItem1, shouldAutorun);

        if (shouldAutorun)
        {
            EditorUtility.DisplayDialog("Adjust IMEI Plugin", "Adjust IMEI plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Adjust IMEI Plugin", "Adjust IMEI plugin post build tasks from now on WON’T be performed each time you build your app."
                + "\n\nPlease, make sure that your app is compatible for usage of the Adjust IMEI plugin by selecting \"Is my app properly configured?\" option from menu.", "OK");
        }
    }

    [MenuItem(MenuItem2)]
    static void RunPostBuildTasksManually()
    {
        RunPostBuildScript(BuildTarget.Android, false);
        EditorUtility.DisplayDialog("Adjust IMEI Plugin", "Post-build tasks execution completed."
                + "\n\nPlease, make sure that your app is compatible for usage of the Adjust IMEI plugin by selecting \"Is my app properly configured?\" option from menu.", "OK");
    }

    [MenuItem(MenuItem3)]
    static void ExportAdjustUnityPackage()
    {
        string exportedFileName = "AdjustImei.unitypackage";
        string assetsPath = "Assets/AdjustImei";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adjust-android-imei.jar");
        assetsToExport.Add(assetsPath + "/Android/AdjustImeiAndroid.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustImeiEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdjustImei.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdjustImei.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string projectPath)
    {
        RunPostBuildScript(target:target, preBuild:false, projectPath:projectPath);
    }

    private static void RunPostBuildScript(BuildTarget target, bool preBuild, string projectPath = "")
    {
        if (target == BuildTarget.Android)
        {
            UnityEngine.Debug.Log("[AdjustImei]: Starting to perform post build tasks for Android platform.");
            RunPostProcessTasksAndroid();
        }
    }

    private static void RunPostProcessTasksAndroid()
    {
        string appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

        // Check if user has already created AndroidManifest.xml file in its location.
        // If not, do nothing until user has valid AndroidManifest.xml file in place.
        if (!File.Exists(appManifestPath))
        {
            UnityEngine.Debug.Log("[AdjustImei]: User defined AndroidManifest.xml file not found in Plugins/Android folder.");
            UnityEngine.Debug.Log("[AdjustImei]: Aborting post build tasks. Please make sure you have valid AndroidManifest.xml file in place and rebuild your app.");
            return;
        }
        else
        {
            UnityEngine.Debug.Log("[AdjustImei]: User defined AndroidManifest.xml file located in Plugins/Android folder.");
        }

        // Seems like you already have your own AndroidManifest.xml, we’ll now run
        // some checks on it and tweak it a bit if needed to add some stuff which
        // our native Android IMEI plugin needs so that it can run properly.

        // Let’s open the app’s AndroidManifest.xml file.
        XmlDocument manifestFile = new XmlDocument();
        manifestFile.Load(appManifestPath);

        bool manifestHasChanged = false;

        // Add needed permissions if they are missing.
        manifestHasChanged |= AddPermissions(manifestFile);

        if (manifestHasChanged)
        {
            // Save the changes.
            manifestFile.Save(appManifestPath);

            // Clean the manifest file.
            CleanManifestFile(appManifestPath);

            UnityEngine.Debug.Log("[AdjustImei]: App’s AndroidManifest.xml file check and potential modification completed.");
            UnityEngine.Debug.Log("[AdjustImei]: Please check if any error message was displayed during this process "
                + "and make sure to fix all issues in order to properly use the Adjust IMEI plugin in your app.");
        }
        else
        {
            UnityEngine.Debug.Log("[AdjustImei]: App’s AndroidManifest.xml file check completed.");
            UnityEngine.Debug.Log("[AdjustImei]: No modifications performed due to app’s AndroidManifest.xml file compatibility.");
        }
    }

    private static bool AddPermissions(XmlDocument manifest)
    {
        // The Adjust IMEI plugin needs following permissions to be added to you app’s manifest file:
        // <uses-permission android:name="android.permission.READ_PHONE_STATE" />

        UnityEngine.Debug.Log("[AdjustImei]: Checking if all permissions needed for the Adjust IMEI plugin are present in the app’s AndroidManifest.xml file.");

        bool hasReadPhoneStatePermission = false;
        XmlElement manifestRoot = manifest.DocumentElement;

        // Check if permissions are already there.
        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "uses-permission")
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Value.Contains("android.permission.READ_PHONE_STATE"))
                    {
                        hasReadPhoneStatePermission = true;
                    }
                }
            }
        }

        bool manifestHasChanged = false;

        // If android.permission.READ_PHONE_STATE permission is missing, add it.
        if (!hasReadPhoneStatePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.READ_PHONE_STATE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[AdjustImei]: android.permission.READ_PHONE_STATE permission successfully added to your app’s AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[AdjustImei]: Your app’s AndroidManifest.xml file already contains android.permission.READ_PHONE_STATE permission.");
            UnityEngine.Debug.Log("[AdjustImei]: All good.");
        }

        return manifestHasChanged;
    }

    private static void CleanManifestFile(String manifestPath)
    {
        // Due to XML writing issue with XmlElement methods which are unable
        // to write "android:[param]" string, we have wrote "android__[param]" string instead.
        // Now make the replacement: "android:[param]" -> "android__[param]"

        TextReader manifestReader = new StreamReader(manifestPath);
        string manifestContent = manifestReader.ReadToEnd();
        manifestReader.Close();

        Regex regex = new Regex("android__");
        manifestContent = regex.Replace(manifestContent, "android:");

        TextWriter manifestWriter = new StreamWriter(manifestPath);
        manifestWriter.Write(manifestContent);
        manifestWriter.Close();
    }

    private static bool DoesAppHaveAllThePermissions()
    {
        string appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

        // Check if user has already created AndroidManifest.xml file in its location.
        // If not, do nothing until user has valid AndroidManifest.xml file in place.
        if (!File.Exists(appManifestPath))
        {
            return false;
        }

        // Seems like you already have your own AndroidManifest.xml.
        // We’ll now check if all the permissions needed by the plugin are added.

        // Let’s open the app’s AndroidManifest.xml file.
        XmlDocument manifestFile = new XmlDocument();
        manifestFile.Load(appManifestPath);

        // Check for the android.permission.READ_PHONE_STATE permission.
        XmlElement manifestRoot = manifestFile.DocumentElement;
        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "uses-permission")
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Value.Contains("android.permission.READ_PHONE_STATE"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}