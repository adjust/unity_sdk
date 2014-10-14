using UnityEngine;
using System.Collections.Generic;
using com.adjust.sdk;
using System;

public class Adjust : MonoBehaviour {

	private static IAdjust instance = null;
	private static string errorMessage = "adjust: SDK not started. Start it manually using the 'appDidLaunch' method";
	private static Action<ResponseData> responseDelegate = null;

	public string appToken = "{Your App Token}";
	public AdjustUtil.LogLevel logLevel = AdjustUtil.LogLevel.Info;
	public AdjustUtil.AdjustEnvironment environment = AdjustUtil.AdjustEnvironment.Sandbox;
	public bool eventBuffering = false;
	public bool startManually = false;
	public const string sdkPrefix = "unity3.4.2";

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

	public static void appDidLaunch(string appToken, AdjustUtil.AdjustEnvironment environment, AdjustUtil.LogLevel logLevel, bool eventBuffering) {
		if (Adjust.instance != null) {
			Debug.Log("adjust: warning, SDK already started. Restarting");
		}

#if UNITY_ANDROID
		Adjust.instance = new AdjustAndroid();
#elif UNITY_IOS
		Adjust.instance = new AdjustIOS();
#elif UNITY_WP8
		Adjust.instance = new AdjustWP8();
#elif UNITY_METRO
		Adjust.instance = new AdjustMetro();
#endif

		if (Adjust.instance == null) {
			Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps");
			return;
		}

		Adjust.instance.appDidLaunch (appToken, environment, sdkPrefix , logLevel, eventBuffering);
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
		Adjust.instance.setResponseDelegateString (runResponseDelegate);
	}

	public static void setEnabled(bool enabled) {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}
		Adjust.instance.setEnabled(enabled);
	}

	public static bool isEnabled() {
		if (Adjust.instance == null) {
			Debug.Log(Adjust.errorMessage);
			return false;
		}
		return Adjust.instance.isEnabled ();
	}

	public void getNativeMessage (string sResponseData) {
		Adjust.runResponseDelegate (sResponseData);
	}

	public static void runResponseDelegate(string sResponseData) {
		if (instance == null) {
			Debug.Log(Adjust.errorMessage);
			return;
		}
		if (responseDelegate == null) {
			Debug.Log("adjust: response delegate not set to receive callbacks");
			return;
		}

		var responseData = new ResponseData (sResponseData);
		responseDelegate (responseData);
	}
}
