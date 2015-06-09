using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SimpleJSON;
using UnityEngine;

namespace com.adjust.sdk
{
#if UNITY_IOS
	public class AdjustiOS : IAdjust
	{
		#region External methods

		[DllImport ("__Internal")]
		private static extern void _AdjustLaunchApp (string appToken, string environment, string sdkPrefix, int logLevel, int eventBuffering, string sceneName);

		[DllImport ("__Internal")]
		private static extern void _AdjustTrackEvent (string eventToken, double revenue, string currency, string jsonCallbackParameters, string jsonPartnerParameters);

		[DllImport ("__Internal")]
		private static extern void _AdjustSetEnabled (int enabled);
		
		[DllImport ("__Internal")]
		private static extern int _AdjustIsEnabled ();
		
		[DllImport ("__Internal")]
		private static extern void _AdjustSetOfflineMode (int enabled);

		public AdjustiOS ()
		{
		}

		#endregion

		#region Public methods

		public void start (AdjustConfig adjustConfig)
		{
			string appToken = adjustConfig .appToken;
			string sdkPrefix = adjustConfig.sdkPrefix;
			string sceneName = adjustConfig.sceneName;
			string environment = adjustConfig.environment.ToString ().ToLower ();

			int logLevel = convertLogLevel (adjustConfig.logLevel);
			int eventBufferingEnabled = convertBool (adjustConfig.eventBufferingEnabled);

			_AdjustLaunchApp (appToken, environment, sdkPrefix, logLevel, eventBufferingEnabled, sceneName);
		}

		public void trackEvent (AdjustEvent adjustEvent)
		{
			double revenue = convertDouble (adjustEvent.revenue);

			string eventToken = adjustEvent.eventToken;
			string currency = adjustEvent.currency;
			string stringJsonCallBackParameters = ConvertListToJson (adjustEvent.callbackList);
			string stringJsonPartnerParameters = ConvertListToJson (adjustEvent.partnerList);
			
			_AdjustTrackEvent (eventToken, revenue, currency, stringJsonCallBackParameters, stringJsonPartnerParameters);
		}

		public void onPause ()
		{

		}

		public void onResume ()
		{

		}

		public void setEnabled (bool enabled)
		{
			_AdjustSetEnabled (convertBool (enabled));
		}

		public bool isEnabled ()
		{
			var iIsEnabled = _AdjustIsEnabled ();

			return Convert.ToBoolean (iIsEnabled);
		}

		public void setOfflineMode (bool enabled)
		{
			_AdjustSetOfflineMode (convertBool (enabled));
		}

		#endregion

		#region Private and helper methods

		private int convertLogLevel (AdjustLogLevel? logLevel)
		{
			if (logLevel == null) {
				return -1;
			}

			return (int)logLevel;
		}

		private int convertBool (bool? value)
		{
			if (value == null) {
				return -1;
			}

			if (value.Value) {
				return 1;
			} else {
				return 0;
			}
		}

		private double convertDouble (double? value)
		{
			if (value == null) {
				return -1;
			}

			return (double)value;
		}

		private string ConvertListToJson (List<String> list)
		{
			if (list == null) {
				return null;
			}

			var jsonArray = new JSONArray ();
			
			foreach (var listItem in list) {
				jsonArray.Add (new JSONData (listItem));
			}

			return jsonArray.ToString ();
		}

		#endregion
	}
#endif
}
