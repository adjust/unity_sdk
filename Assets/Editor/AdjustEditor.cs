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
		if (!isEnabled) {
			return;
		}

		var exitCode = RunPostBuildScript (preBuild: false, pathToBuiltProject: pathToBuiltProject);

		if (exitCode == -1) {
			return;
		}

		if (exitCode != 0) {
			var errorMessage = GenerateErrorScriptMessage (exitCode);

			UnityEngine.Debug.LogError ("adjust: " + errorMessage);
		}
	}

	[MenuItem ("Assets/Adjust/Fix AndroidManifest.xml")]
	static void FixAndroidManifest ()
	{
		#if UNITY_ANDROID
		var exitCode = RunPostBuildScript (preBuild: true);

		if (exitCode == 1) {
			EditorUtility.DisplayDialog("Adjust", 
			                            string.Format("AndroidManifest.xml changed or created at {0}/Plugins/Android/ .", Application.dataPath),
			                            "OK");
		} else if (exitCode == 0) {
			EditorUtility.DisplayDialog("Adjust", "AndroidManifest.xml did not needed to be changed.", "OK");
		} else {
			EditorUtility.DisplayDialog("Adjust", GenerateErrorScriptMessage(exitCode), "OK");
			
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
		
		if (AdjustEditor.iOSBuildPath == "") {
			UnityEngine.Debug.Log ("iOS build path reset to default path");
		} else {
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

		EditorUtility.DisplayDialog ("Adjust", "The post processing for adjust is now " +
			(isEnabled ? "enabled." : "disabled."), "OK");
	}

	static int RunPostBuildScript (bool preBuild, string pathToBuiltProject = "")
	{
		string resultContent;
		string arguments = null;
		string pathToScript = null;

		string filePath = System.IO.Path.Combine (Environment.CurrentDirectory, 
			                  @"Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py");

        // Check if Unity is running on Windows operating system.
        // If yes - fix line endings in python scripts.
        if (Application.platform == RuntimePlatform.WindowsEditor) {
			UnityEngine.Debug.Log ("Windows platform");

			using (System.IO.StreamReader streamReader = new System.IO.StreamReader (filePath)) {
				string fileContent = streamReader.ReadToEnd ();
				resultContent = Regex.Replace (fileContent, @"\r\n|\n\r|\n|\r", "\r\n");
			}

			if (File.Exists (filePath)) {
				File.WriteAllText (filePath, resultContent);
			}
		} else {
			UnityEngine.Debug.Log ("Unix platform");

			using (System.IO.StreamReader streamReader = new System.IO.StreamReader (filePath)) {
				string replaceWith = "\n";
				string fileContent = streamReader.ReadToEnd ();
				
				resultContent = fileContent.Replace ("\r\n", replaceWith);
			}

			if (File.Exists (filePath)) {
				File.WriteAllText (filePath, resultContent);
			}
		}

#if UNITY_ANDROID
		pathToScript = "/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py";
		arguments = "\"" + Application.dataPath + "\"";
		
		if (preBuild) {
			arguments = "--pre-build " + arguments;
		}
#elif UNITY_IOS
		pathToScript = "/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py";
		
		if (AdjustEditor.iOSBuildPath == "") {
			arguments = "\"" + pathToBuiltProject + "\"";
		} else {
			arguments = "\"" + AdjustEditor.iOSBuildPath + "\"";
		}
#else
		return -1;
#endif

		Process proc = new Process ();
		proc.EnableRaisingEvents = false; 
		proc.StartInfo.FileName = Application.dataPath + pathToScript;
		proc.StartInfo.Arguments = arguments;
		proc.Start ();
		proc.WaitForExit ();
		
		return proc.ExitCode;
	}

	static string GenerateErrorScriptMessage (int exitCode)
	{
#if UNITY_ANDROID
		if (exitCode == 1) {
			return "The AndroidManifest.xml file was only changed or created after building the package. " +
          		"PLease build again the Android Unity package so it can use the new file";
		}  
#endif

		if (exitCode != 0) {
			var message = "Build script exited with error." +
			              " Please check the Adjust log file for more information at {0}";
			string projectPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
			string logFile = null;
			
#if UNITY_ANDROID
			logFile = projectPath + "/AdjustPostBuildAndroidLog.txt";
#elif UNITY_IOS
			logFile = projectPath + "/AdjustPostBuildiOSLog.txt";
#else
			return null;
#endif
			return string.Format (message, logFile);
		} 

		return null;
	}
}
