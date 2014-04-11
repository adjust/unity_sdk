using UnityEngine;
using System.Collections.Generic;
using com.adjust.sdk;
using System;

public class Adjust : MonoBehaviour {

	private static IAdjust instance = null;
	private static string errorMessage = "adjust: SDK not started. Start it manually using the 'appDidLaunch' method";
	private static Action<ResponseData> responseDelegate = null;

	public string appToken = "{Your App Token}";
	public Util.LogLevel logLevel = Util.LogLevel.Info;
	public Util.Environment environment = Util.Environment.Sandbox;
	public bool eventBuffering = false;
	public bool startManually = false;

	void Awake() {
		if (!this.startManually) {
			Adjust.appDidLaunch(this.appToken, this.environment, this.logLevel, this.eventBuffering);
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		if (Adjust.instance == null) {
			return;
		}

		if (pauseStatus) {
			Adjust.instance.onPause();
		} else {
			Adjust.instance.onResume();
		}
	}

	public static void appDidLaunch(string appToken, Util.Environment environment, Util.LogLevel logLevel, bool eventBuffering) {
		if (Adjust.instance != null) {
			Debug.Log("adjust: warning, SDK already started. Restarting");
		}

#if UNITY_ANDROID
		Adjust.instance = new AdjustAndroid();
#elif UNITY_IOS
		Adjust.instance = new AdjustIOS();
#endif

		if (Adjust.instance == null) {
			Debug.Log("adjust: SDK can only be used in Android or iOS");
			return;
		}

		Adjust.instance.appDidLaunch (appToken, environment, logLevel, eventBuffering);
	}

	public static void trackEvent(string eventToken, Dictionary<string,string> parameters = null) {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}

		Adjust.instance.trackEvent (eventToken, parameters);
	}

	public static void trackRevenue(double cents, string eventToken = null, Dictionary<string,string> parameters = null) {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}
		
		Adjust.instance.trackRevenue(cents ,eventToken, parameters);
	}

	public static void setResponseDelegate(Action<ResponseData> responseDelegate, string sceneName = "Adjust") {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}

		Adjust.responseDelegate = responseDelegate;
		Adjust.instance.setResponseDelegate (sceneName);
	}

	public static void setEnabled(bool enabled) {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.instance.setEnabled(enabled);
	}

	public void getNativeMessage (string sResponseData) {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}
		if (Adjust.responseDelegate == null) {
			Debug.Log("adjust: response delegate not set to receive callbacks");
			return;
		}

		var responseData = new ResponseData (sResponseData);
		Adjust.responseDelegate (responseData);
	}

}