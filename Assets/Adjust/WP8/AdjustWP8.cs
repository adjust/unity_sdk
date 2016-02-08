#if UNITY_WP8
using UnityEngine;
using System.Collections;
using AdjustUnityWP;
using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustWP8 : IAdjust
	{
		private const string sdkPrefix = "unity4.1.3";

		public bool isEnabled()
		{
			return AdjustWP.IsEnabled();
		}		
		public void onPause ()
		{
			AdjustWP.ApplicationDeactivated ();
		}
		public void onResume()
		{
			AdjustWP.ApplicationActivated ();
		}
		public void setEnabled(bool enabled)
		{
			AdjustWP.SetEnabled (enabled);
		}
		public void setOfflineMode(bool offlineMode)
		{
			AdjustWP.SetOfflineMode (offlineMode);
		}
		public void start(AdjustConfig adjustConfig)
		{
			string logLevelString = null;
			if (adjustConfig.logLevel != null) 
			{
				logLevelString = adjustConfig.logLevel.lowercaseToString();
			}

			Action<Dictionary<string, string>> attributionChangedDictionary = null;
			if (adjustConfig.attributionChangedDelegate != null)
			{
				attributionChangedDictionary = (attributionDictionary) => Adjust.runAttributionChangedDictionary(attributionDictionary);
			}

			string environment = adjustConfig.environment.lowercaseToString ();

			AdjustWP.ApplicationLaunching (
				appToken: adjustConfig.appToken,
				logLevelString: logLevelString,
				environment: environment,
				defaultTracker: adjustConfig.defaultTracker,
				eventBufferingEnabled: adjustConfig.eventBufferingEnabled,
				sdkPrefix: sdkPrefix,
				attributionChangedDic: attributionChangedDictionary,
                logDelegate: adjustConfig.logDelegate
			);
		}
		public void trackEvent (AdjustEvent adjustEvent)
		{
			AdjustWP.TrackEvent (
				eventToken: adjustEvent.eventToken,
				revenue: adjustEvent.revenue,
				currency: adjustEvent.currency,
				callbackList: adjustEvent.callbackList,
				partnerList: adjustEvent.partnerList
			);
		}
        // iOS specific methods
        public void setDeviceToken(string deviceToken) { }

		public string getIdfa()
		{
			return null;
		}

		// Android specific methods
		public void setReferrer (string referrer) { }

		public void getGoogleAdId (Action<string> onDeviceIdsRead) { }
	}
}
#endif
