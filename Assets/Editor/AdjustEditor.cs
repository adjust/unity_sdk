//#define ADJUST_NO_PYTHON

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AdjustEditor : MonoBehaviour
{
    static bool isEnabled = true;
    static string iOSBuildPath = "";

    [PostProcessBuild]
    public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
    {
        if (!isEnabled)
        {
            return;
        }

        var exitCode = RunPostBuildScript (target: target, preBuild: false, pathToBuiltProject: pathToBuiltProject);

        if (exitCode == -1)
        {
            return;
        }

        if (exitCode != 0)
        {
            var errorMessage = GenerateErrorScriptMessage (target, exitCode);
            UnityEngine.Debug.LogError ("adjust: " + errorMessage);
        }
    }

    [MenuItem ("Assets/Adjust/Fix AndroidManifest.xml")]
    static void FixAndroidManifest ()
    {
#if UNITY_ANDROID
        var exitCode = RunPostBuildScript (target: BuildTarget.Android, preBuild: true);

        if (exitCode == 1)
        {
            EditorUtility.DisplayDialog ("Adjust", 
                                         string.Format("AndroidManifest.xml changed or created at {0}/Plugins/Android/ .", Application.dataPath),
                                         "OK");
        }
        else if (exitCode == 0)
        {
            EditorUtility.DisplayDialog ("Adjust", "AndroidManifest.xml did not needed to be changed.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog ("Adjust", GenerateErrorScriptMessage (BuildTarget.Android, exitCode), "OK");
        }
#else
        EditorUtility.DisplayDialog ("Adjust", "Option only valid for the Android platform.", "OK");
#endif
    }

    [MenuItem ("Assets/Adjust/Set iOS build path")]
    static void SetiOSBuildPath ()
    {
#if UNITY_IOS
        AdjustEditor.iOSBuildPath = EditorUtility.OpenFolderPanel (
            title: "iOs build path",
            folder: EditorUserBuildSettings.GetBuildLocation (BuildTarget.iOS),
            defaultName: "");
        
        if (AdjustEditor.iOSBuildPath == "")
        {
            UnityEngine.Debug.Log ("iOS build path reset to default path");
        }
        else
        {
            UnityEngine.Debug.Log (string.Format ("iOS build path: {0}", AdjustEditor.iOSBuildPath));
        }
#else
        EditorUtility.DisplayDialog ("Adjust", "Option only valid for the Android platform.", "OK");
#endif
    }

    [MenuItem ("Assets/Adjust/Change post processing status")]
    static void ChangePostProcessingStatus ()
    {
        isEnabled = !isEnabled;
        EditorUtility.DisplayDialog ("Adjust", "The post processing for adjust is now " + (isEnabled ? "enabled." : "disabled."), "OK");
    }

    static int RunPostBuildScript (BuildTarget target, bool preBuild, string pathToBuiltProject = "")
    {
		if (target == BuildTarget.Android)
		{
#if ADJUST_NO_PYTHON
			AdjustAndroidPostProcess proc = new AdjustAndroidPostProcess();
			return proc.Start(Application.dataPath, preBuild);
#else
			string pathToScript = "/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py";
			ReadyPythonScript(pathToScript);

			string arguments = "\"" + Application.dataPath + "\"";
			if (preBuild)
			{
				arguments = "--pre-build " + arguments;
			}

			return ExecutePython (pathToScript, arguments);
#endif
		}
		else if (target == BuildTarget.iOS)
		{
			string pathToScript = "/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py";
			ReadyPythonScript (pathToScript);

			string arguments = null;
			if (AdjustEditor.iOSBuildPath == "")
			{
				arguments = "\"" + pathToBuiltProject + "\"";
			}
			else
			{
				arguments = "\"" + AdjustEditor.iOSBuildPath + "\"";
			}

			return ExecutePython (pathToScript, arguments);
		}
		else
		{
			return -1;
		}
	}

	/// <summary>
	/// Update the line endings on the Python script.
	/// </summary>
	private static void ReadyPythonScript (string pathToScript)
	{
		string resultContent;
		// Used to only ever change "Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py" which seemed off (didn't fix Cloud Build issue)
		string filePath = Path.Combine (Environment.CurrentDirectory, "Assets" + pathToScript);

		// Check if Unity is running on Windows operating system.
		// If yes - fix line endings in python scripts.
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			UnityEngine.Debug.Log ("Windows platform");

			using (StreamReader streamReader = new StreamReader (filePath))
			{
				string fileContent = streamReader.ReadToEnd ();
				resultContent = Regex.Replace (fileContent, @"\r\n|\n\r|\n|\r", "\r\n");
			}
		}
		else
		{
			UnityEngine.Debug.Log ("Unix platform");

			using (StreamReader streamReader = new StreamReader (filePath))
			{
				string fileContent = streamReader.ReadToEnd ();
				resultContent = fileContent.Replace ("\r\n", "\n");
			}
		}

		if (File.Exists (filePath))
		{
			File.WriteAllText (filePath, resultContent);
		}
	}

	private static int ExecutePython (string pathToScript, string arguments)
	{
		Process proc = new Process ();
		proc.EnableRaisingEvents = false;
		proc.StartInfo.FileName = Application.dataPath + pathToScript;
		proc.StartInfo.Arguments = arguments;
		proc.Start ();
		proc.WaitForExit ();

		return proc.ExitCode;
	}

	static string GenerateErrorScriptMessage (BuildTarget target, int exitCode)
    {
        if (target == BuildTarget.Android)
        {
            if (exitCode == 1)
            {
                return "The AndroidManifest.xml file was only changed or created after building the package. " +
                        "Please build again the Android Unity package so it can use the new file";
            }
        }

        if (exitCode != 0)
        {
            var message = "Build script exited with error. " +
                          "Please check the Adjust log file for more information at {0}";
            string projectPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
            string logFile = null;
            
            if (target == BuildTarget.Android)
            {
                logFile = projectPath + "/AdjustPostBuildAndroidLog.txt";
            }
            else if (target == BuildTarget.iOS)
            {
                logFile = projectPath + "/AdjustPostBuildiOSLog.txt";
            }
            else
            {
                return null;
            }

            return string.Format (message, logFile);
        } 

        return null;
    }
}
