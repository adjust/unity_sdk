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
		private const string sdkPrefix = "unity4.0.4";

		#region External methods

		[DllImport ("__Internal")]
		private static extern void _AdjustLaunchApp (string appToken, string environment, string sdkPrefix, int logLevel, int eventBuffering, int macMd5TrackingEnabled, string sceneName);

		[DllImport ("__Internal")]
		private static extern void _AdjustTrackEvent (string eventToken, double revenue, string currency, string receipt, string transactionId, int isReceiptSet, string jsonCallbackParameters, string jsonPartnerParameters);

		[DllImport ("__Internal")]
		private static extern void _AdjustSetEnabled (int enabled);
		
		[DllImport ("__Internal")]
		private static extern int _AdjustIsEnabled ();
		
		[DllImport ("__Internal")]
		private static extern void _AdjustSetOfflineMode (int enabled);

		[DllImport ("__Internal")]
		private static extern void _AdjustSetDeviceToken (string deviceToken);

		#endregion

		public AdjustiOS ()
		{
		}

		#region Public methods

		public void start (AdjustConfig adjustConfig)
		{
			string appToken = adjustConfig .appToken;
			string sceneName = adjustConfig.sceneName;
			string environment = adjustConfig.environment.ToString ().ToLower ();

			int logLevel = convertLogLevel (adjustConfig.logLevel);
			int eventBufferingEnabled = convertBool (adjustConfig.eventBufferingEnabled);
			int macMd5TrackingEnabled = convertBool (adjustConfig.macMd5TrackingEnabled);

			_AdjustLaunchApp (appToken, environment, sdkPrefix, logLevel, eventBufferingEnabled, macMd5TrackingEnabled, sceneName);
		}

		public void trackEvent (AdjustEvent adjustEvent)
		{
			int isReceiptSet = convertBool (adjustEvent.isReceiptSet);
			double revenue = convertDouble (adjustEvent.revenue);

			string eventToken = adjustEvent.eventToken;
			string currency = adjustEvent.currency;
			string receipt = adjustEvent.receipt;
			string transactionId = adjustEvent.transactionId;
			string stringJsonCallBackParameters = ConvertListToJson (adjustEvent.callbackList);
			string stringJsonPartnerParameters = ConvertListToJson (adjustEvent.partnerList);
			
			_AdjustTrackEvent (eventToken, revenue, currency, receipt, transactionId, isReceiptSet, stringJsonCallBackParameters, stringJsonPartnerParameters);
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

		public void setDeviceToken(string deviceToken)
		{
			_AdjustSetDeviceToken (deviceToken);
		}

		public void setReferrer(string referrer)
		{

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
