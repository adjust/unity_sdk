using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;

public class AdjustEditor : MonoBehaviour {

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		var exitCode = RunPostBuildScript (preBuild: false, pathToBuiltProject: pathToBuiltProject);

		if (exitCode == -1) {
			return;
		}
		if (exitCode != 0) {
			var errorMessage = GenerateErrorScriptMessage(exitCode);

			UnityEngine.Debug.LogError ("adjust: " + errorMessage);
		}
	}

	[MenuItem("Adjust/Fix AndroidManifest.xml")]
	static void FixAndroidManifest() {
		#if UNITY_ANDROID
		var exitCode = RunPostBuildScript (preBuild: true);
		if (exitCode == 1) {
			EditorUtility.DisplayDialog("Adjust", 
			                            string.Format("AndroidManifest.xml changed or created at {0}/Plugins/Android/ .",Application.dataPath),
			                            "OK");
		} else if (exitCode == 0) {
			EditorUtility.DisplayDialog("Adjust", 
			                            "AndroidManifest.xml did not needed to be changed.",
			                            "OK");
		} else {
			EditorUtility.DisplayDialog("Adjust", 
			                            GenerateErrorScriptMessage(exitCode),
			                            "OK");
			
		}
		#else
		EditorUtility.DisplayDialog("Adjust", "Option only valid for the Android platform.", "OK");
		#endif
	}

	static int RunPostBuildScript (bool preBuild, string pathToBuiltProject = "") {
		string pathToScript = null;
		string arguments = null;

		#if UNITY_ANDROID
		pathToScript = "/Editor/AdjustPostBuildAndroid";
		arguments = Application.dataPath;
		if (preBuild)
			arguments = "--pre-build " + arguments;
		#elif UNITY_IOS
		pathToScript = "/Editor/AdjustPostBuildiOS";
		arguments = pathToBuiltProject;
		#else
		return -1;
		#endif

		Process proc = new Process();
		proc.EnableRaisingEvents=false; 
		proc.StartInfo.FileName = Application.dataPath + pathToScript;
		proc.StartInfo.Arguments = arguments;
		proc.Start();
		proc.WaitForExit();
		return proc.ExitCode;
	}

	static string GenerateErrorScriptMessage(int exitCode) {
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
			logFile = projectPath + "/AdjustPostBuiliOSLog.txt";
			#else
			return null;
			#endif
			return string.Format(message, logFile);
		} 

		return null;
	}
}
