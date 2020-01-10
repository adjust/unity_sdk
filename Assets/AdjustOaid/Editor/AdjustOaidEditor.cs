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

public class AdjustOaidEditor
{
    private const string MenuItem0 = "Assets/AdjustOaid/Autorun post-build tasks";
    private const string MenuItem1 = "Assets/AdjustOaid/Export Unity package";

    private static bool shouldAutorun = true;

    static AdjustOaidEditor()
    {
        EditorApplication.delayCall += () =>
        {
            shouldAutorun = EditorPrefs.GetBool(MenuItem0, true);
            Menu.SetChecked(MenuItem0, shouldAutorun);
        };
    }

    [MenuItem(MenuItem0)]
    static void AutorunPostBuildTasks()
    {
        shouldAutorun = !shouldAutorun;
        Menu.SetChecked(MenuItem0, shouldAutorun);
        EditorPrefs.SetBool(MenuItem0, shouldAutorun);

        if (shouldAutorun)
        {
            EditorUtility.DisplayDialog("Adjust OAID Plugin", "Adjust OAID plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Adjust OAID Plugin", "Adjust OAID plugin post build tasks from now on WON'T be performed each time you build your app."
                + "\n\nPlease, make sure that your app is compatible for usage of the Adjust OAID plugin by selecting \"Is my app properly configured?\" option from menu.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void ExportAdjustUnityPackage()
    {
        string exportedFileName = "AdjustOaid.unitypackage";
        string assetsPath = "Assets/AdjustOaid";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adjust-android-oaid.jar");
        assetsToExport.Add(assetsPath + "/Android/AdjustOaidAndroid.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustOaidEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdjustOaid.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdjustOaid.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
}
