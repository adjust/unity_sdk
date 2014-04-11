using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using SimpleJSON;

namespace com.adjust.sdk {

	public class AdjustIOS : IAdjust {

		[DllImport ("__Internal")]
		private static extern void _AdjustLauchApp(string appToken, string environment, int logLevel, int eventBuffering);

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

		public AdjustIOS() { }

		public void appDidLaunch(string appToken, Util.Environment environment, Util.LogLevel logLevel, bool eventBuffering) {
			string sEnvironment = environment.ToString ().ToLower ();

			_AdjustLauchApp(appToken, sEnvironment, (int)logLevel, Convert.ToInt32(eventBuffering));
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
		public void setEnabled(bool enabled) {
			_AdjustSetEnabled (Convert.ToInt32 (enabled));
		}

		private string ConvertDicToJson (Dictionary<string, string> dictionary) {
			if (dictionary == null) {
				return null;
			}
			var jsonClass = new JSONClass();
			foreach (KeyValuePair<string, string> kvp in dictionary) {
				jsonClass.Add(kvp.Key, new JSONData(kvp.Value));
			}
			return jsonClass.ToString();
		}
	}
}
