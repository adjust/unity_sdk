using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using SimpleJSON;

namespace com.adjust.sdk {

	public class AdjustIOS : IAdjust {

		[DllImport ("__Internal")]
		private static extern void _AdjustLaunchApp(string appToken, string environment, string sdkPrefix, int logLevel, int eventBuffering);

		[DllImport ("__Internal")]
		private static extern void _AdjustTrackEvent(string eventToken, string jsonParameters);

		[DllImport ("__Internal")]
		private static extern void _AdjustTrackRevenue(double cents, string eventToken, string jsonParameters);

		[DllImport ("__Internal")]
		private static extern void _AdjustOnPause();

		[DllImport ("__Internal")]
		private static extern void _AdjustOnResume();

		[DllImport ("__Internal")]
		private static extern void _AdjustSetResponseDelegate (string sceneName);

		[DllImport ("__Internal")]
		private static extern void _AdjustSetEnabled (int enabled);

		[DllImport ("__Internal")]
		private static extern int _AdjustIsEnabled ();

		public AdjustIOS() { }

		public void appDidLaunch(string appToken, AdjustUtil.AdjustEnvironment environment, string sdkPrefix, AdjustUtil.LogLevel logLevel, bool eventBuffering) {
			string sEnvironment = environment.ToString ().ToLower ();

			_AdjustLaunchApp(appToken, sEnvironment, sdkPrefix, (int)logLevel, Convert.ToInt32(eventBuffering));
		}
		public void trackEvent (string eventToken, Dictionary<string,string> parameters = null) {
			string sJsonParameters = ConvertDicToJson(parameters);

			_AdjustTrackEvent (eventToken, sJsonParameters);
		}
		public void trackRevenue (double cents, string eventToken = null, Dictionary<string,string> parameters = null) {
			string sJsonParameters = ConvertDicToJson(parameters);

			_AdjustTrackRevenue (cents, eventToken, sJsonParameters);
		}
		public void onPause () {
			_AdjustOnPause ();
		}
		public void onResume() {
			_AdjustOnResume ();
		}
		public void setResponseDelegate(string sceneName) {
			_AdjustSetResponseDelegate (sceneName);
		}
		public void setResponseDelegateString(Action<string> responseDelegate) {
		}
		public void setEnabled(bool enabled) {
			_AdjustSetEnabled (Convert.ToInt32 (enabled));
		}
		public bool isEnabled() {
			var iIsEnabled = _AdjustIsEnabled ();
			return Convert.ToBoolean (iIsEnabled);
		}

		private string ConvertDicToJson (Dictionary<string, string> dictionary) {
			if (dictionary == null) {
				return null;
			}
			var jsonClass = new JSONClass();
			foreach (KeyValuePair<string, string> kvp in dictionary) {
				if (kvp.Value != null) {
					jsonClass.Add(kvp.Key, new JSONData(kvp.Value));
				}
			}
			return jsonClass.ToString();
		}
	}
}
