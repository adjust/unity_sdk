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

public class AdjustSamsungReferrerEditor
{
    private const string MenuItem0 = "Assets/AdjustSamsungReferrer/Autorun post-build tasks";
    private const string MenuItem1 = "Assets/AdjustSamsungReferrer/Export Unity package";

    private static bool shouldAutorun = true;

    static AdjustSamsungReferrerEditor()
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
            EditorUtility.DisplayDialog("Adjust Samsung Referrer Plugin", "Adjust Samsung Referrer plugin post build tasks will from now on be performed each time you build your app.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Adjust Samsung Referrer Plugin", "Adjust Samsung Referrer plugin post build tasks from now on WON'T be performed each time you build your app."
                + "\n\nPlease, make sure that your app is compatible for usage of the Adjust Samsung Referrer plugin by selecting \"Is my app properly configured?\" option from menu.", "OK");
        }
    }

    [MenuItem(MenuItem1)]
    static void ExportAdjustUnityPackage()
    {
        string exportedFileName = "AdjustSamsungReferrer.unitypackage";
        string assetsPath = "Assets/AdjustSamsungReferrer";
        List<string> assetsToExport = new List<string>();

        assetsToExport.Add(assetsPath + "/Android/adjust-android-samsung-referrer.jar");
        assetsToExport.Add(assetsPath + "/Android/AdjustSamsungReferrerAndroid.cs");
        assetsToExport.Add(assetsPath + "/Editor/AdjustSamsungReferrerEditor.cs");
        assetsToExport.Add(assetsPath + "/Prefab/AdjustSamsungReferrer.prefab");
        assetsToExport.Add(assetsPath + "/Unity/AdjustSamsungReferrer.cs");

        AssetDatabase.ExportPackage(
            assetsToExport.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }
}
