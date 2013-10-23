using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;


//This class is placed inside the Plugins folder so the Class AdjustIO is accessible
//from js scripts.

public class AdjustIo : MonoBehaviour {
	
	private static bool initialized = false;
	
#if UNITY_ANDROID
	private static AndroidJavaClass jniAdjustIo;
	private static AndroidJavaObject jniCurrentActivity;
	private static AndroidJavaClass jniAdjustIoUnity3DHelper;
	
#elif UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void AdjustIoInit(string appId);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoSetLogLevel(int logLevel);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoSetEnvironment(int environment);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoTrackEvent(string eventToken);

	[DllImport ("__Internal")]
	private static extern void AdjustIoTrackEventWithParameters(string eventToken);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoClearParameters();
		
	[DllImport ("__Internal")]
	private static extern void AdjustIoAddParameter(string key, string value);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoTrackRevenue(double cents);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoTrackRevenueForEvent(double cents, string eventToken);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoTrackRevenueForEventWithParameters(double cents, string eventToken);
	
	[DllImport ("__Internal")]
	private static extern void AdjustIoSetEventBufferingEnabled(bool enabled);
#endif
	
	private static void SetParameters(Dictionary<string,string> parameters){
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIoUnity3DHelper.CallStatic("clearParameters");
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoClearParameters();		
		#endif
		#if !UNITY_EDITOR
			foreach(KeyValuePair<string,string> kvp in parameters){
				#if UNITY_ANDROID
					jniAdjustIoUnity3DHelper.CallStatic("addParameter",kvp.Key,kvp.Value);
				#elif UNITY_IPHONE
					AdjustIoAddParameter(kvp.Key,kvp.Value);
				#endif
			}
		#endif
	}
	
	public enum LogLevel{
		AILogLevelVerbose = 0,
		AILogLevelDebug,
		AILogLevelInfo,
		AILogLevelWarn,
		AILogLevelError,
		AILogLevelAssert
	}
	
	public enum Environment{
		AIEnvironmentSandbox = 0,
		AIEnvironmentProduction
	}
	
	public LogLevel logLevel = LogLevel.AILogLevelInfo;
	public Environment environment;
	public string appToken = "YourAppToken";
	public bool eventBufferingEnabled = false;
	
	
	private static AdjustIo instance = null;
	
	private static void Initialize(){
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIo.CallStatic("onResume",jniCurrentActivity);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoInit(instance.appToken);
			SetLogLevel(instance.logLevel);
			SetEnvironment(instance.environment);
			if(instance.eventBufferingEnabled){
				SetEventBufferingEnabled();
			}
		#endif
	}
	
	private static string GetLogLevelName(LogLevel logLevel){
		return logLevel.ToString().Substring(10).ToLower();
	}
				
	private static string GetEnvironmentName(Environment environment){
		return environment.ToString().Substring(13).ToLower();
	}
	
	private static string GetEventBufferingEnabledName(bool enabled){
		if(enabled){
			return "true";
		}else{
			return "false";
		}
	}
	
	private static void SetLogLevel(LogLevel logLevel){
		if(!initialized){
			return;
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIoUnity3DHelper.CallStatic("setLogLevel",jniCurrentActivity,GetLogLevelName(logLevel));
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoSetLogLevel((int)logLevel);
		#endif
	}
	
	private static void SetEnvironment(Environment environment){
		if(!initialized){
			return;
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIoUnity3DHelper.CallStatic("setEnvironment",jniCurrentActivity,GetEnvironmentName(environment));
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoSetEnvironment((int)environment);
		#endif
	}
	
	private static void SetEventBufferingEnabled(bool bufferingEnabled){
		#if UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoSetEventBufferingEnabled(bufferingEnabled);
		#endif	
	}
	
	public static AdjustIo GetInstance(){
		return instance;
	}
	
	public static void TrackEvent(string eventToken){
		if(!initialized){
			return;
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIo.CallStatic("trackEvent",eventToken);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoTrackEvent(eventToken);
		#endif
	}
	
	public static void TrackEvent(string eventToken, Dictionary<string,string> parameters){
		if(!initialized){
			return;
		}
		SetParameters(parameters);
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIoUnity3DHelper.CallStatic("trackEventWithParameters",eventToken);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoTrackEventWithParameters(eventToken);
		#endif
	}
	
	public static void TrackRevenue(double cents){
		if(!initialized){
			return;
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIo.CallStatic("trackRevenue",cents);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoTrackRevenue(cents);
		#endif
	}
	
	public static void TrackRevenue(double cents, string eventToken){
		if(!initialized){
			return;
		}
		#if UNITY_ANDROID && !UNITY_EDITOR
			jniAdjustIo.CallStatic("trackRevenue",cents,eventToken);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoTrackRevenueForEvent(cents,eventToken);
		#endif
	}
	
	public static void TrackRevenue(double cents, string eventToken, Dictionary<string,string> parameters){
		if(!initialized){
			return;
		}
		SetParameters(parameters);		
		#if UNITY_ANDROID && !UNITY_EDITOR		 
			jniAdjustIoUnity3DHelper.CallStatic("trackRevenueForEventWithParameters",cents,eventToken);
		#elif UNITY_IPHONE && !UNITY_EDITOR
			AdjustIoTrackRevenueForEventWithParameters(cents,eventToken);
		#endif
	}
	
	void Awake(){
		if(initialized){
			Destroy(gameObject);
		}else{	
			instance = this;
			DontDestroyOnLoad(gameObject);
			initialized = true;			
			#if UNITY_ANDROID && !UNITY_EDITOR
				if(!Application.isEditor){
					jniAdjustIo = new AndroidJavaClass("com.adeven.adjustio.AdjustIo");
					AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
					jniCurrentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
					jniAdjustIoUnity3DHelper = new AndroidJavaClass("com.adeven.adjustio.unity3d.AdjustIoUnity3DHelper");
				}
			#endif	
			Initialize();
			#if UNITY_ANDROID && UNITY_EDITOR
				string manifestChanges = TestAndroidManifest();
				if(manifestChanges != ""){
					Debug.Log("The Android manifest has been updated: " + manifestChanges);
				}
			#endif
		}
	}
	
	void OnApplicationPause(bool paused){
#if UNITY_ANDROID && !UNITY_EDITOR
		if(paused){
			jniAdjustIo.CallStatic("onPause");
		}else{
			jniAdjustIo.CallStatic("onResume",jniCurrentActivity);
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#if UNITY_ANDROID
	public string TestAndroidManifest(){
		string changes = "";
		string[] lines = System.IO.File.ReadAllLines(Application.dataPath + "/Plugins/Android/AndroidManifest.xml");
		for(int i = 0; i < lines.Length; i++){
			if(lines[i].Contains("<meta-data android:name=\"AdjustIoAppToken\"")){
				string[] parts = lines[i].Split(new string[]{"android:value"},System.StringSplitOptions.None);
				if(parts.Length != 2){
					return "Could not test if androidManifest was correct, please put each meta-data element on a seperate single line.";
				}else{
					string appToken_ = parts[1].Split('"')[1];
					if(appToken_ != appToken){
						lines[i] = lines[i].Replace("\"" + appToken_ + "\"","\"" + appToken + "\"");
						changes += "changed app token from " + appToken_ + " to " + appToken + "\n";
					}
				}
			}else if(lines[i].Contains("<meta-data android:name=\"AdjustIoLogLevel\"")){
				string[] parts = lines[i].Split(new string[]{"android:value"},System.StringSplitOptions.None);
				if(parts.Length != 2){
					return "Could not test if androidManifest was correct, please put each meta-data element on a seperate single line.";
				}else{
					string logLevel_ = parts[1].Split('"')[1];
					string logLevelName = GetLogLevelName(logLevel);
					if(!logLevel_.Equals(logLevelName,System.StringComparison.InvariantCultureIgnoreCase)){
						lines[i] = lines[i].Replace("\"" + logLevel_ + "\"","\"" + logLevelName + "\"");
						changes += "changed log level from " + logLevel_ + " to " + logLevelName + "\n";
					}
				}
			}else if(lines[i].Contains("<meta-data android:name=\"AdjustIoEnvironment\"")){
				string[] parts = lines[i].Split(new string[]{"android:value"},System.StringSplitOptions.None);
				if(parts.Length != 2){
					return "Could not test if androidManifest was correct, please put each meta-data element on a seperate single line.";
				}else{
					string environment_ = parts[1].Split('"')[1];
					string environmentName = GetEnvironmentName(environment);
					if(!environment_.Equals(environmentName,System.StringComparison.InvariantCultureIgnoreCase)){
						lines[i] = lines[i].Replace("\"" + environment_ + "\"","\"" + environmentName + "\"");
						changes += "changed environment from " + environment_ + " to " + environmentName + "\n";
					}
				}
			}else if(lines[i].Contains("<meta-data android:name=\"AdjustIoEventBuffering\"")){
				string[] parts = lines[i].Split(new string[]{"android:value"},System.StringSplitOptions.None);
				if(parts.Length != 2){
					return "Could not test if androidManifest was correct, please put each meta-data element on a seperate, single line.";
				}else{
					string eventBufferingEnabled_ = parts[1].Split('"')[1];
					string eventBufferingEnabledName = GetEventBufferingEnabledName(eventBufferingEnabled);
					if(!eventBufferingEnabled_.Equals(eventBufferingEnabledName,System.StringComparison.InvariantCultureIgnoreCase)){
						lines[i] = lines[i].Replace("\"" + eventBufferingEnabled_ + "\"","\"" + eventBufferingEnabledName + "\"");
						changes += "changed eventBufferingEnbaled from " + eventBufferingEnabled_ + " to " + eventBufferingEnabledName + "\n";
					}
				}
			}
		}
		if(changes != ""){
			System.IO.File.WriteAllLines(Application.dataPath + "/Plugins/Android/AndroidManifest.xml",lines);
		}
		return changes;
	}	
#endif
}
