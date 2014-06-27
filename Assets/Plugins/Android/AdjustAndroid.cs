using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace com.adjust.sdk {
#if UNITY_ANDROID
	public class AdjustAndroid : IAdjust {

		private AndroidJavaClass ajcAdjust;
		private AndroidJavaClass ajcAdjustUnity;
		private AndroidJavaObject ajoCurrentActivity;

		public AdjustAndroid() {
			ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			ajcAdjustUnity = new AndroidJavaClass("com.adjust.sdk.AdjustUnity");
			AndroidJavaClass ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			ajoCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		}

		public void appDidLaunch(string appToken, AdjustUtil.AdjustEnvironment environment, string sdkPrefix, AdjustUtil.LogLevel logLevel, bool eventBuffering) {

			string sEnvironment = environment.ToString ().ToLower ();
			string sLogLevel = logLevel.ToString ().ToLower ();

			ajcAdjust.CallStatic("appDidLaunch",
	     		ajoCurrentActivity, 
	          	appToken,
	            sEnvironment,
                sLogLevel,
	     		eventBuffering);
			ajcAdjust.CallStatic("setSdkPrefix",sdkPrefix);

			onResume ();
		}

		public void trackEvent(string eventToken, Dictionary<string,string> parameters = null) {
			var javaParameters = ConvertDicToJava (parameters);
			ajcAdjust.CallStatic("trackEvent", eventToken, javaParameters);
		}

		public void trackRevenue(double cents, string eventToken = null, Dictionary<string,string> parameters = null) {
			var javaParameters = ConvertDicToJava (parameters);
			ajcAdjust.CallStatic("trackRevenue", cents, eventToken, javaParameters);
		}

		public void onPause() {
			ajcAdjust.CallStatic ("onPause");
		}

		public void onResume() {
			ajcAdjust.CallStatic("onResume", ajoCurrentActivity);
		}

		public void setResponseDelegate(string sceneName) {
			ajcAdjustUnity.CallStatic ("setResponseDelegate", sceneName);
		}

		public void setResponseDelegateString(Action<string> responseDelegate) { }

		public void setEnabled(bool enabled) {
			ajcAdjust.CallStatic ("setEnabled", ConvertBoolToJava(enabled));
		}

		public bool isEnabled() {
			var ajo = ajcAdjust.CallStatic<AndroidJavaObject> ("isEnabled");
			return ConvertBoolFromJava (ajo) ?? false;
		}

		private AndroidJavaObject ConvertBoolToJava(bool value) {
			AndroidJavaObject javaBool = new AndroidJavaObject ("java.lang.Boolean", value.ToString ().ToLower ());
			return javaBool;
		}

		private bool? ConvertBoolFromJava(AndroidJavaObject ajo) {
			if (ajo == null) {
				return null;
			}
			var sBool = ajo.Call<string>("toString");
			try {
				return Convert.ToBoolean (sBool);
			} catch (FormatException) {
				return null;
			}
		}

		private AndroidJavaObject ConvertDicToJava(Dictionary<string, string> dictonary)
		{
			if (dictonary == null) {
				return null;
			}

			AndroidJavaObject javaDic = new AndroidJavaObject("java.util.HashMap", dictonary.Count);

			foreach (var pair in dictonary) {
				if (pair.Value != null) {
					javaDic.Call<string>("put", pair.Key, pair.Value);
				}
			}

			return javaDic;
		}
	}
#endif
}
