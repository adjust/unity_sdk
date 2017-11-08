#if UNITY_WSA
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_WSA_10_0
using Win10Interface;
#elif UNITY_WINRT_8_1
using Win81Interface;
#endif

namespace com.adjust.sdk {
    public class AdjustWindows : IAdjust {
        private const string sdkPrefix = "unity4.12.0";

		// TODO: make AdjustWS10 & AdjusWS81 implement an interface, which can be used as an abstraction here
		// thus leading to less compiler branching (e.g. #if, #endif, etc.)

        public bool isEnabled() {
#if UNITY_WSA_10_0
            return AdjustWS10.IsEnabled();
#elif UNITY_WINRT_8_1
            return AdjustWS81.IsEnabled();
#endif
        }

        public string getAdid() {
			return AdjustWS10.GetAdid ();
        }

        public AdjustAttribution getAttribution() {
			var attribution = AdjustWS10.GetAttribution ();
			var attributionJson = JsonUtility.ToJson (attribution);
			return new AdjustAttribution (attributionJson);
        }

        public void onPause() {
#if UNITY_WSA_10_0
            AdjustWS10.ApplicationDeactivated();
#elif UNITY_WINRT_8_1
            AdjustWS81.ApplicationDeactivated();
#endif
        }

        public void onResume() {
#if UNITY_WSA_10_0
            AdjustWS10.ApplicationActivated();
#elif UNITY_WINRT_8_1
            AdjustWS81.ApplicationActivated();
#endif
        }

        public void setEnabled(bool enabled) {
#if UNITY_WSA_10_0
            AdjustWS10.SetEnabled(enabled);
#elif UNITY_WINRT_8_1
            AdjustWS81.SetEnabled(enabled);
#endif
        }

        public void setOfflineMode(bool offlineMode) {
#if UNITY_WSA_10_0
            AdjustWS10.SetOfflineMode(offlineMode);
#elif UNITY_WINRT_8_1
            AdjustWS81.SetOfflineMode(offlineMode);
#endif
        }

        public void start(AdjustConfig adjustConfig) {
            string logLevelString = null;
            string environment = lowercaseToString(adjustConfig.environment);
            Action<Dictionary<string, string>> attributionChangedAction = null;
			Action<Dictionary<string, string>> sessionSuccessChangedAction = null;
			Action<Dictionary<string, string>> sessionFailureChangedAction = null;
			Action<Dictionary<string, string>> eventSuccessChangedAction = null;
			Action<Dictionary<string, string>> eventFailureChangedAction = null;

            if (adjustConfig.logLevel.HasValue) {
                logLevelString = lowercaseToString(adjustConfig.logLevel.Value);
            }

            if (adjustConfig.attributionChangedDelegate != null) {
				attributionChangedAction = (attributionMap) => {
					var attributionMapJson = JsonUtility.ToJson(attributionMap);
					Adjust.GetNativeAttribution(attributionMapJson);
				};
            }

			if (adjustConfig.sessionSuccessDelegate != null) {
				sessionSuccessChangedAction = (sessionMap) => {
					var sessionMapJson = JsonUtility.ToJson (sessionMap);
					Adjust.GetNativeSessionSuccess(sessionMapJson);
				};
			}

			if (adjustConfig.sessionFailureDelegate != null) {
				sessionFailureChangedAction = (sessionMap) => {
					var sessionMapJson = JsonUtility.ToJson (sessionMap);
					Adjust.GetNativeSessionFailure(sessionMapJson);
				};
			}

			if(adjustConfig.eventSuccessDelegate != null) {
				eventSuccessChangedAction = (eventMap) => {
					var eventMapJson = JsonUtility.ToJson(eventMap);
					Adjust.GetNativeEventSuccess(eventMapJson);
				};
			}

			if (adjustConfig.eventFailureDelegate != null) {
				eventFailureChangedAction = (eventMap) => {
					var eventMapJson = JsonUtility.ToJson(eventMap);
					Adjust.GetNativeEventFailure(eventMapJson);
				};
			}

			bool sendInBackground = false;
			if (adjustConfig.sendInBackground.HasValue) {
				sendInBackground = adjustConfig.sendInBackground.Value;
			}

			double delayStartSeconds = 0;
			if (adjustConfig.delayStart.HasValue) {
				delayStartSeconds = adjustConfig.delayStart.Value;
			}

			AdjustConfigDto adjustConfigDto = new AdjustConfigDto {
				AppToken = adjustConfig.appToken,
				Environment = environment,
				SdkPrefix = sdkPrefix,
				SendInBackground = sendInBackground,
				DelayStart = delayStartSeconds,
				UserAgent = adjustConfig.userAgent,
				DefaultTracker = adjustConfig.defaultTracker,
				EventBufferingEnabled = adjustConfig.eventBufferingEnabled,
				LaunchDeferredDeeplink = adjustConfig.launchDeferredDeeplink,
				LogLevelString = logLevelString,
				LogDelegate = adjustConfig.logDelegate,
				ActionAttributionChangedData = attributionChangedAction,
				ActionSessionSuccessData = sessionSuccessChangedAction,
				ActionSessionFailureData = sessionFailureChangedAction,
				ActionEventSuccessData = eventSuccessChangedAction,
				ActionEventFailureData = eventFailureChangedAction
			};

#if UNITY_WSA_10_0
			AdjustWS10.ApplicationLaunching (
#elif UNITY_WINRT_8_1
            AdjustWS81.ApplicationLaunching(
#endif
				adjustConfigDto
			);
        }

        public void trackEvent(AdjustEvent adjustEvent) {
#if UNITY_WSA_10_0
			AdjustWS10.TrackEvent (
#elif UNITY_WINRT_8_1
            AdjustWS81.TrackEvent(
#endif
				eventToken: adjustEvent.eventToken,
				revenue: adjustEvent.revenue,
				currency: adjustEvent.currency,
				purchaseId: null,
				callbackList: adjustEvent.callbackList,
				partnerList: adjustEvent.partnerList
			);
        }

        public void sendFirstPackages() {
			AdjustWS10.SendFirstPackages ();
		}

        public void setDeviceToken(string deviceToken) {
			AdjustWS10.SetDeviceToken (deviceToken);
		}

		public string getWinAdid() {
			return AdjustWS10.GetWindowsAdId();
		}

        public static void addSessionPartnerParameter(string key, string value) {
			AdjustWS10.AddSessionPartnerParameter (key, value);
		}

        public static void addSessionCallbackParameter(string key, string value) {
			AdjustWS10.AddSessionCallbackParameter (key, value);
		}

        public static void removeSessionPartnerParameter(string key) {
			AdjustWS10.RemoveSessionPartnerParameter (key);
		}

        public static void removeSessionCallbackParameter(string key) {
			AdjustWS10.RemoveSessionCallbackParameter (key);
		}

        public static void resetSessionPartnerParameters() {
			AdjustWS10.ResetSessionPartnerParameters ();
		}

        public static void resetSessionCallbackParameters() {
			AdjustWS10.ResetSessionCallbackParameters ();
		}

		public static string lowercaseToString(AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "verbose";
			case AdjustLogLevel.Debug:
				return "debug";
			case AdjustLogLevel.Info:
				return "info";
			case AdjustLogLevel.Warn:
				return "warn";
			case AdjustLogLevel.Error:
				return "error";
			case AdjustLogLevel.Assert:
				return "assert";
			case AdjustLogLevel.Suppress:
				return "suppress";
			default:
				return "unknown";
			}
		}

		public static string lowercaseToString(AdjustEnvironment adjustEnvironment)
		{
			switch (adjustEnvironment)
			{
			case AdjustEnvironment.Sandbox:
				return "sandbox";
			case AdjustEnvironment.Production:
				return "production";
			default:
				return "unknown";
			}
		}

		// iOS specific methods
		public string getIdfa() {
			return null;
		}

		// Android specific methods
		public void setReferrer(string referrer) {}

		public void getGoogleAdId(Action<string> onDeviceIdsRead) {}
    }
}
#endif
