using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;

public class AdjustIoPostprocessBuildPlayer : MonoBehaviour {
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
#if UNITY_ANDROID
		AdjustIo[] wrappers = (AdjustIo[])Resources.FindObjectsOfTypeAll(typeof(AdjustIo));
		foreach(AdjustIo io in wrappers){
			if(AssetDatabase.GetAssetPath(io.GetInstanceID()) != "Assets/AdjustIo/AdjustIo.prefab"){
				string result = io.TestAndroidManifest();
				if(result != ""){
					EditorUtility.DisplayDialog("AdjustIo","The AndroidManifest.xml was not up to date. Please build again otherwise AdjustIo will not work as expected. The manifest has been updated:\n" + result, "OK");
				}
			}
		}
#elif UNITY_IPHONE
		Process proc = new Process();
		proc.EnableRaisingEvents=false; 
		proc.StartInfo.FileName = "perl";
		proc.StartInfo.Arguments =  Application.dataPath + "/AdjustIo/Data/PostBuildScripts/PostBuildAdjustIoScript"+ " " + "'" + pathToBuiltProject + "'";
		proc.Start();
		proc.WaitForExit();
		UnityEngine.Debug.Log("AdjustIo build log file: " + System.IO.Directory.GetCurrentDirectory() + "/AdjustIoBuildLogFile.txt");
		UnityEngine.Debug.Log("AdjustIo, path to built project: " + pathToBuiltProject);
		//UnityEngine.Debug.Log(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);
#endif
    }
		
	[MenuItem ("AdjustIo/Check AndroidManifest.xml")]
	static void CheckAndroidManifest(){
		#if UNITY_ANDROID
			AdjustIo[] wrappers = (AdjustIo[])Resources.FindObjectsOfTypeAll(typeof(AdjustIo));
			bool found = false;
			foreach(AdjustIo io in wrappers){
				if(AssetDatabase.GetAssetPath(io.GetInstanceID()) != "Assets/AdjustIo/AdjustIo.prefab"){
					found = true;
					string result = io.TestAndroidManifest();
					if(result != ""){
						EditorUtility.DisplayDialog("AdjustIo","The AndroidManifest.xml was not up to date. The manifest has been updated:\n" + result, "OK");			
					}else{
						EditorUtility.DisplayDialog("AdjustIo", "The AndroidManifest.xml file is up to date", "OK");
					}
				}
			}
			if(!found){
				EditorUtility.DisplayDialog("AdjustIo", "We could not find the AdjustIo prefab in the scene.\nBe sure to open the scene who holds the AdjustIo prefab when starting this test.", "OK");
			}
		#else
			EditorUtility.DisplayDialog("AdjustIo", "Please change the platform to Android if you want to this test.", "OK");
		#endif
	}
}

