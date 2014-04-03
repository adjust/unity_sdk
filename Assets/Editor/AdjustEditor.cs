using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;

public class AdjustEditor : MonoBehaviour {

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		UnityEngine.Debug.Log("adjust: path to build project: " + pathToBuiltProject );
		UnityEngine.Debug.Log("adjust: path to application: " + Application.dataPath );

		#if UNITY_IPHONE
		Process proc = new Process();
		proc.EnableRaisingEvents=false; 
		proc.StartInfo.FileName = Application.dataPath + "/Editor/AdjustPostBuild";
		proc.StartInfo.Arguments = "'" + pathToBuiltProject + "'";
		proc.Start();
		proc.WaitForExit();
		#endif

	}
}
