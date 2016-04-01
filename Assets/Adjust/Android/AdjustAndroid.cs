using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace com.adjust.sdk
{
#if UNITY_ANDROID
	public class AdjustAndroid : IAdjust
	{
		#region Fields
		private const string sdkPrefix = "unity4.6.0";

		private AndroidJavaClass ajcAdjust;
		private AndroidJavaObject ajoCurrentActivity;

		private AttributionChangeListener onAttributionChangedListener;
		private EventTrackingFailedListener onEventTrackingFailedListener;
		private EventTrackingSucceededListener onEventTrackingSucceededListener;
		private SessionTrackingFailedListener onSessionTrackingFailedListener;
		private SessionTrackingSucceededListener onSessionTrackingSucceededListener;
		#endregion

		#region Proxy listener classes
		private class AttributionChangeListener : AndroidJavaProxy
		{
			private Action<AdjustAttribution> callback;

			public AttributionChangeListener (Action<AdjustAttribution> pCallback) : base ("com.adjust.sdk.OnAttributionChangedListener")
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

				if (callback != null)
				{
					callback (adjustAttribution);
				}
			}
		}

		private class EventTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustEventSuccess> callback;

			public EventTrackingSucceededListener (Action<AdjustEventSuccess> pCallback) : base ("com.adjust.sdk.OnEventTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingSucceeded (AndroidJavaObject eventSuccessData)
			{
				if (eventSuccessData == null)
				{
					return;
				}

				AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess ();

				adjustEventSuccess.Adid = eventSuccessData.Get<string> (AdjustUtils.KeyAdid);
				adjustEventSuccess.Message = eventSuccessData.Get<string> (AdjustUtils.KeyMessage);
				adjustEventSuccess.Timestamp = eventSuccessData.Get<string> (AdjustUtils.KeyTimestamp);
				adjustEventSuccess.EventToken = eventSuccessData.Get<string> (AdjustUtils.KeyEventToken);

				try 
				{
					AndroidJavaObject ajoJsonResponse = eventSuccessData.Get<AndroidJavaObject> (AdjustUtils.KeyJsonResponse);
					string jsonResponseString = ajoJsonResponse.Call<string> ("toString");

					adjustEventSuccess.BuildJsonResponseFromString (jsonResponseString);
				}
				catch (Exception)
				{
					// JSON response reading failed.
				}

				if (callback != null)
				{
					callback (adjustEventSuccess);
				}
			}
		}

		private class EventTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustEventFailure> callback;

			public EventTrackingFailedListener (Action<AdjustEventFailure> pCallback) : base ("com.adjust.sdk.OnEventTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingFailed (AndroidJavaObject eventFailureData)
			{
				if (eventFailureData == null)
				{
					return;
				}

				AdjustEventFailure adjustEventFailure = new AdjustEventFailure ();

				adjustEventFailure.Adid = eventFailureData.Get<string> (AdjustUtils.KeyAdid);
				adjustEventFailure.Message = eventFailureData.Get<string> (AdjustUtils.KeyMessage);
				adjustEventFailure.WillRetry = eventFailureData.Get<bool> (AdjustUtils.KeyWillRetry);
				adjustEventFailure.Timestamp = eventFailureData.Get<string> (AdjustUtils.KeyTimestamp);
				adjustEventFailure.EventToken = eventFailureData.Get<string> (AdjustUtils.KeyEventToken);

				try
				{
					AndroidJavaObject ajoJsonResponse = eventFailureData.Get<AndroidJavaObject> (AdjustUtils.KeyJsonResponse);
					string jsonResponseString = ajoJsonResponse.Call<string> ("toString");

					adjustEventFailure.BuildJsonResponseFromString (jsonResponseString);
				}
				catch (Exception)
				{
					// JSON response reading failed.
				}
				
				if (callback != null)
				{
					callback (adjustEventFailure);
				}
			}
		}

		private class SessionTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustSessionSuccess> callback;

			public SessionTrackingSucceededListener (Action<AdjustSessionSuccess> pCallback) : base ("com.adjust.sdk.OnSessionTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingSucceeded (AndroidJavaObject sessionSuccessData)
			{
				if (sessionSuccessData == null)
				{
					return;
				}

				AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess ();

				adjustSessionSuccess.Adid = sessionSuccessData.Get<string> (AdjustUtils.KeyAdid);
				adjustSessionSuccess.Message = sessionSuccessData.Get<string> (AdjustUtils.KeyMessage);
				adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string> (AdjustUtils.KeyTimestamp);

				try 
				{
					AndroidJavaObject ajoJsonResponse = sessionSuccessData.Get<AndroidJavaObject> (AdjustUtils.KeyJsonResponse);
					string jsonResponseString = ajoJsonResponse.Call<string> ("toString");

					adjustSessionSuccess.BuildJsonResponseFromString (jsonResponseString);
				}
				catch (Exception)
				{
					// JSON response reading failed.
				}

				if (callback != null)
				{
					callback (adjustSessionSuccess);
				}
			}
		}

		private class SessionTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustSessionFailure> callback;

			public SessionTrackingFailedListener (Action<AdjustSessionFailure> pCallback) : base ("com.adjust.sdk.OnSessionTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingFailed (AndroidJavaObject sessionFailureData)
			{
				if (sessionFailureData == null)
				{
					return;
				}

				AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure ();

				adjustSessionFailure.Adid = sessionFailureData.Get<string> (AdjustUtils.KeyAdid);
				adjustSessionFailure.Message = sessionFailureData.Get<string> (AdjustUtils.KeyMessage);
				adjustSessionFailure.WillRetry = sessionFailureData.Get<bool> (AdjustUtils.KeyWillRetry);
				adjustSessionFailure.Timestamp = sessionFailureData.Get<string> (AdjustUtils.KeyTimestamp);

				try
				{
					AndroidJavaObject ajoJsonResponse = sessionFailureData.Get<AndroidJavaObject> (AdjustUtils.KeyJsonResponse);
					string jsonResponseString = ajoJsonResponse.Call<string> ("toString");

					adjustSessionFailure.BuildJsonResponseFromString (jsonResponseString);
				}
				catch (Exception)
				{
					// JSON response reading failed.
				}
				
				if (callback != null)
				{
					callback (adjustSessionFailure);
				}
			}
		}

		private class DeviceIdsReadListener : AndroidJavaProxy
		{
			private Action<string> onPlayAdIdReadCallback;

			public DeviceIdsReadListener (Action<string> pCallback) : base ("com.adjust.sdk.OnDeviceIdsRead")
			{
				this.onPlayAdIdReadCallback = pCallback;
			}

			public void onGoogleAdIdRead (string playAdId)
			{
				if (onPlayAdIdReadCallback == null)
				{
					return;
				}

				this.onPlayAdIdReadCallback (playAdId);
			}

			// null object.
			public void onGoogleAdIdRead (AndroidJavaObject ajoAdId)
			{
				if (ajoAdId == null)
				{
					string adId = null;
					this.onGoogleAdIdRead (adId);

					return;
				}

				this.onGoogleAdIdRead (ajoAdId.Call<string> ("toString"));
			}
		}
		#endregion

		#region Constructors
		public AdjustAndroid ()
		{
			ajcAdjust = new AndroidJavaClass ("com.adjust.sdk.Adjust");
			AndroidJavaClass ajcUnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"); 
			ajoCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		}
		#endregion

		#region Public methods
		public void start (AdjustConfig adjustConfig)
		{
			// Get environment variable.
			AndroidJavaObject ajoEnvironment = adjustConfig.environment == AdjustEnvironment.Sandbox ? 
				new AndroidJavaClass ("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject> ("ENVIRONMENT_SANDBOX") :
					new AndroidJavaClass ("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject> ("ENVIRONMENT_PRODUCTION");

			// Create adjust config object.
			AndroidJavaObject ajoAdjustConfig = new AndroidJavaObject ("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, ajoEnvironment);

			// Check log level.
			if (adjustConfig.logLevel.HasValue)
			{
				AndroidJavaObject ajoLogLevel = new AndroidJavaClass ("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject> (adjustConfig.logLevel.Value.uppercaseToString());

				if (ajoLogLevel != null)
				{
					ajoAdjustConfig.Call ("setLogLevel", ajoLogLevel);
				}
			}

			// Check event buffering setting.
			if (adjustConfig.eventBufferingEnabled.HasValue)
			{
				if (adjustConfig.eventBufferingEnabled != null)
				{
					AndroidJavaObject ajoIsEnabled = new AndroidJavaObject ("java.lang.Boolean", adjustConfig.eventBufferingEnabled.Value);
					ajoAdjustConfig.Call ("setEventBufferingEnabled", ajoIsEnabled);
				}
			}

			// Check attribution changed delagate setting.
			if (adjustConfig.attributionChangedDelegate != null)
			{
				onAttributionChangedListener = new AttributionChangeListener (adjustConfig.attributionChangedDelegate);
				ajoAdjustConfig.Call ("setOnAttributionChangedListener", onAttributionChangedListener);
			}

			// Check event success delegate setting.
			if (adjustConfig.eventSuccessDelegate != null)
			{
				onEventTrackingSucceededListener = new EventTrackingSucceededListener (adjustConfig.eventSuccessDelegate);
				ajoAdjustConfig.Call ("setOnEventTrackingSucceededListener", onEventTrackingSucceededListener);
			}

			// Check event failure delagate setting.
			if (adjustConfig.eventFailureDelegate != null)
			{
				onEventTrackingFailedListener = new EventTrackingFailedListener (adjustConfig.eventFailureDelegate);
				ajoAdjustConfig.Call ("setOnEventTrackingFailedListener", onEventTrackingFailedListener);
			}

			// Check session success delegate setting.
			if (adjustConfig.sessionSuccessDelegate != null)
			{
				onSessionTrackingSucceededListener = new SessionTrackingSucceededListener (adjustConfig.sessionSuccessDelegate);
				ajoAdjustConfig.Call ("setOnSessionTrackingSucceededListener", onSessionTrackingSucceededListener);
			}

			// Check session failure delegate setting.
			if (adjustConfig.sessionFailureDelegate != null)
			{
				onSessionTrackingFailedListener = new SessionTrackingFailedListener (adjustConfig.sessionFailureDelegate);
				ajoAdjustConfig.Call ("setOnSessionTrackingFailedListener", onSessionTrackingFailedListener);
			}

			// Set unity SDK prefix.
			ajoAdjustConfig.Call ("setSdkPrefix", sdkPrefix);
			
			// Since INSTALL_REFERRER is not triggering SDK initialisation, call onResume after onCreate.
			// OnApplicationPause doesn't get called first time the scene loads, so call to onResume is needed.
			
			// Initialise and start the SDK.
			ajcAdjust.CallStatic ("onCreate", ajoAdjustConfig);
			ajcAdjust.CallStatic ("onResume");
		}

		public void trackEvent (AdjustEvent adjustEvent)
		{
			AndroidJavaObject ajoAdjustEvent = new AndroidJavaObject ("com.adjust.sdk.AdjustEvent", adjustEvent.eventToken);

			if (adjustEvent.revenue != null && adjustEvent.currency != null)
			{
				ajoAdjustEvent.Call ("setRevenue", (double) adjustEvent.revenue, adjustEvent.currency);
			}

			if (adjustEvent.callbackList != null)
			{
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
				{
					string key = adjustEvent.callbackList [i];
					string value = adjustEvent.callbackList [i + 1];

					ajoAdjustEvent.Call ("addCallbackParameter", key, value);
				}
			}

			if (adjustEvent.partnerList != null)
			{
				for (int i = 0; i < adjustEvent.partnerList.Count; i += 2)
				{
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

		// Android specific methods
		public void onPause ()
		{
			ajcAdjust.CallStatic ("onPause");
		}
		
		public void onResume ()
		{
			ajcAdjust.CallStatic ("onResume");
		}

		public void setReferrer (string referrer)
		{
			ajcAdjust.CallStatic ("setReferrer", referrer);
		}

		public void getGoogleAdId (Action<string> onDeviceIdsRead)
		{
			DeviceIdsReadListener onDeviceIdsReadProxy = new DeviceIdsReadListener (onDeviceIdsRead);

			ajcAdjust.CallStatic ("getGoogleAdId", ajoCurrentActivity, onDeviceIdsReadProxy);
		}

		// iOS specific methods
		public void setDeviceToken (string deviceToken)
		{
		}

		public string getIdfa ()
		{
			return null;
		}
		#endregion
	}
#endif
}
