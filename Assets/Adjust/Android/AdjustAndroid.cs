using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SimpleJSON;
using UnityEngine;

namespace com.adjust.sdk
{
#if UNITY_ANDROID
	public class AdjustAndroid : IAdjust
	{
		private const string sdkPrefix = "unity4.0.4";
		private AndroidJavaClass ajcAdjust;
		private AndroidJavaObject ajoCurrentActivity;
		private AttributionChangeListener onAttributionChangedListener;

		private class AttributionChangeListener : AndroidJavaProxy
		{
			private Action<AdjustAttribution> callback;

			public AttributionChangeListener (Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
			{
				this.callback = pCallback;
			}

			public void onAttributionChanged (AndroidJavaObject attribution)
			{
				AdjustAttribution adjustAttribution = new AdjustAttribution ();

				adjustAttribution.trackerName = attribution.Get<string> ("trackerName");
				adjustAttribution.trackerToken = attribution.Get<string> ("trackerToken");
				adjustAttribution.network = attribution.Get<string> ("network");
				adjustAttribution.campaign = attribution.Get<string> ("campaign");
				adjustAttribution.adgroup = attribution.Get<string> ("adgroup");
				adjustAttribution.creative = attribution.Get<string> ("creative");
				adjustAttribution.clickLabel = attribution.Get<string> ("clickLabel");

				if (callback != null) {
					callback (adjustAttribution);
				}
			}
		}

		public AdjustAndroid ()
		{
			ajcAdjust = new AndroidJavaClass ("com.adjust.sdk.Adjust");
			AndroidJavaClass ajcUnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
			ajoCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		}

		#region Public methods

		public void onPause ()
		{
			ajcAdjust.CallStatic ("onPause");
		}
		
		public void onResume ()
		{
			ajcAdjust.CallStatic ("onResume");
		}

		public void start (AdjustConfig adjustConfig)
		{
			AndroidJavaObject ajoEnvironment = adjustConfig.environment == AdjustEnvironment.Sandbox ? 
				new AndroidJavaClass ("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject> ("ENVIRONMENT_SANDBOX") :
					new AndroidJavaClass ("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject> ("ENVIRONMENT_PRODUCTION");

			AndroidJavaObject ajoAdjustConfig = new AndroidJavaObject ("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, ajoEnvironment);

			if (adjustConfig.logLevel != null) {
				AndroidJavaObject ajoLogLevel = new AndroidJavaClass ("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject> (adjustConfig.logLevel.ToString().ToUpper());

				if (ajoLogLevel != null) {
					ajoAdjustConfig.Call ("setLogLevel", ajoLogLevel);
				}
			}

			if (adjustConfig.attributionChangedDelegate != null) {
				onAttributionChangedListener = new AttributionChangeListener (adjustConfig.attributionChangedDelegate);
				ajoAdjustConfig.Call ("setOnAttributionChangedListener", onAttributionChangedListener);
			}

			ajoAdjustConfig.Call ("setSdkPrefix", sdkPrefix);
			
			// Since INSTALL_REFERRER is not triggering SDK initialisation, call onResume after onCreate.
			// OnApplicationPause doesn't get called first time the scene loads, so call to onResume is needed.
			
			ajcAdjust.CallStatic ("onCreate", ajoAdjustConfig);
			ajcAdjust.CallStatic ("onResume");
		}

		public void trackEvent (AdjustEvent adjustEvent)
		{
			AndroidJavaObject ajoAdjustEvent = new AndroidJavaObject ("com.adjust.sdk.AdjustEvent", adjustEvent.eventToken);

			if (adjustEvent.revenue != null && adjustEvent.currency != null) {
				ajoAdjustEvent.Call ("setRevenue", (double)adjustEvent.revenue, adjustEvent.currency);
			}

			if (adjustEvent.callbackList != null) {
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2) {
					string key = adjustEvent.callbackList [i];
					string value = adjustEvent.callbackList [i + 1];

					ajoAdjustEvent.Call ("addCallbackParameter", key, value);
				}
			}

			if (adjustEvent.partnerList != null) {
				for (int i = 0; i < adjustEvent.partnerList.Count; i += 2) {
					string key = adjustEvent.partnerList [i];
					string value = adjustEvent.partnerList [i + 1];
				
					ajoAdjustEvent.Call ("addPartnerParameter", key, value);
				}
			}

			ajcAdjust.CallStatic ("trackEvent", ajoAdjustEvent);
		}

		public bool isEnabled ()
		{
			return ajcAdjust.CallStatic<bool> ("isEnabled");
		}

		public void setEnabled (bool enabled) 
		{
			ajcAdjust.CallStatic ("setEnabled", enabled);
		}

		public void setOfflineMode (bool enabled)
		{
			ajcAdjust.CallStatic ("setOfflineMode", enabled);
		}

		public void setReferrer(string referrer)
		{
			ajcAdjust.CallStatic ("setReferrer", referrer);
		}

		public void setDeviceToken(string deviceToken)
		{

		}

		#endregion
	}
#endif
}
