#if UNITY_METRO
using UnityEngine;
using System.Collections;
using AdjustUnityWS;
using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustMetro : IAdjust
	{
		private const string sdkPrefix = "unity4.1.1";

		public bool isEnabled()
		{
			return AdjustWS.IsEnabled();
		}		
		public void onPause ()
		{
			AdjustWS.ApplicationDeactivated ();
		}
		public void onResume()
		{
			AdjustWS.ApplicationActivated ();
		}
		public void setEnabled(bool enabled)
		{
			AdjustWS.SetEnabled (enabled);
		}
		public void setOfflineMode(bool offlineMode)
		{
			AdjustWS.SetOfflineMode (offlineMode);
		}
		public void start(AdjustConfig adjustConfig)
		{
			string logLevelString = null;
			if (adjustConfig.logLevel != null) 
			{
				logLevelString = adjustConfig.logLevel.ToString();
			}

			Action<Dictionary<string, string>> attributionChangedDictionary = null;
			if (adjustConfig.attributionChangedDelegate != null)
			{
				attributionChangedDictionary = (attributionDictionary) => Adjust.runAttributionChangedDictionary(attributionDictionary);
			}

			AdjustWS.ApplicationLaunching (
				appToken: adjustConfig.appToken,
				logLevelString: logLevelString,
				environment: adjustConfig.environment.ToString ().ToLower (),
				defaultTracker: adjustConfig.defaultTracker,
				eventBufferingEnabled: adjustConfig.eventBufferingEnabled,
				sdkPrefix: sdkPrefix,
				attributionChangedDic: attributionChangedDictionary,
                logDelegate: adjustConfig.logDelegate
			);
		}
		public void trackEvent (AdjustEvent adjustEvent)
		{
			AdjustWS.TrackEvent (
				eventToken: adjustEvent.eventToken,
				revenue: adjustEvent.revenue,
				currency: adjustEvent.currency,
				callbackList: adjustEvent.callbackList,
				partnerList: adjustEvent.partnerList
			);
		}
        // iOS specific methods
		public void setDeviceToken (string deviceToken) {}
		// Android specific methods
		public void setReferrer (string referrer) {}
	}
}
#endif