#if UNITY_WSA
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_WSA_10_0
using Win10Interface;
#elif UNITY_WP_8_1
using Win81Interface;
#elif UNITY_WSA
using WinWsInterface;
#endif

namespace com.adjust.sdk {
    public class AdjustWindows : IAdjust {
        private const string sdkPrefix = "unity4.12.0";

        public bool isEnabled() {
			return AdjustWinInterface.IsEnabled();
        }

        public string getAdid() {
			return AdjustWinInterface.GetAdid ();
        }

        public AdjustAttribution getAttribution() {
			var attribution = AdjustWinInterface.GetAttribution ();
			var attributionJson = JsonUtility.ToJson (attribution);
			return new AdjustAttribution (attributionJson);
        }

        public void onPause() {
			AdjustWinInterface.ApplicationDeactivated();
        }

        public void onResume() {
			AdjustWinInterface.ApplicationActivated();
        }

        public void setEnabled(bool enabled) {
			AdjustWinInterface.SetEnabled(enabled);
        }

        public void setOfflineMode(bool offlineMode) {
			AdjustWinInterface.SetOfflineMode(offlineMode);
        }

        public void start(AdjustConfig adjustConfig) {
            string logLevelString = null;
            string environment = lowercaseToString(adjustConfig.environment);
            Action<Dictionary<string, string>> attributionChangedAction = null;
			Action<Dictionary<string, string>> sessionSuccessChangedAction = null;
			Action<Dictionary<string, string>> sessionFailureChangedAction = null;
			Action<Dictionary<string, string>> eventSuccessChangedAction = null;
			Action<Dictionary<string, string>> eventFailureChangedAction = null;
			Func<string, bool> deeplinkResponseFunc = null;

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

			if (adjustConfig.deeplinkDelegate != null) {
				deeplinkResponseFunc = uri => {
					if(adjustConfig.launchDeferredDeeplink) {
						Adjust.GetNativeDeferredDeeplink(uri);
					}

					return adjustConfig.launchDeferredDeeplink;
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
				ActionEventFailureData = eventFailureChangedAction,
				FuncDeeplinkResponseData = deeplinkResponseFunc
			};

			AdjustWinInterface.ApplicationLaunching (adjustConfigDto);
        }

        public void trackEvent(AdjustEvent adjustEvent) {
			AdjustWinInterface.TrackEvent (
				eventToken: adjustEvent.eventToken,
				revenue: adjustEvent.revenue,
				currency: adjustEvent.currency,
				purchaseId: null,
				callbackList: adjustEvent.callbackList,
				partnerList: adjustEvent.partnerList
			);
        }

        public void sendFirstPackages() {
			AdjustWinInterface.SendFirstPackages ();
		}

        public void setDeviceToken(string deviceToken) {
			AdjustWinInterface.SetDeviceToken (deviceToken);
		}

		public string getWinAdid() {
			return AdjustWinInterface.GetWindowsAdId();
		}

        public static void addSessionPartnerParameter(string key, string value) {
			AdjustWinInterface.AddSessionPartnerParameter (key, value);
		}

        public static void addSessionCallbackParameter(string key, string value) {
			AdjustWinInterface.AddSessionCallbackParameter (key, value);
		}

        public static void removeSessionPartnerParameter(string key) {
			AdjustWinInterface.RemoveSessionPartnerParameter (key);
		}

        public static void removeSessionCallbackParameter(string key) {
			AdjustWinInterface.RemoveSessionCallbackParameter (key);
		}

        public static void resetSessionPartnerParameters() {
			AdjustWinInterface.ResetSessionPartnerParameters ();
		}

        public static void resetSessionCallbackParameters() {
			AdjustWinInterface.ResetSessionCallbackParameters ();
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
