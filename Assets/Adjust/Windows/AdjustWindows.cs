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
    public class AdjustWindows {
        private const string sdkPrefix = "unity4.12.0";
        private static bool appLaunched = false;

        public static bool isEnabled() {
			return AdjustWinInterface.IsEnabled();
        }

        public static string getAdid() {
			return AdjustWinInterface.GetAdid ();
        }

        public static AdjustAttribution getAttribution() {
			var attributionMap = AdjustWinInterface.GetAttribution ();
			if (attributionMap == null)
				return new AdjustAttribution ();

            return new AdjustAttribution(attributionMap);
        }

        public static void onPause() {
			AdjustWinInterface.ApplicationDeactivated();
        }

        public static void onResume() {
            if (!appLaunched) { return; }
            AdjustWinInterface.ApplicationActivated();
        }

        public static void setEnabled(bool enabled) {
			AdjustWinInterface.SetEnabled(enabled);
        }

        public static void setOfflineMode(bool offlineMode) {
			AdjustWinInterface.SetOfflineMode(offlineMode);
        }

        public static void start(AdjustConfig adjustConfig) {
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
				    var attribution = new AdjustAttribution(attributionMap);
				    adjustConfig.attributionChangedDelegate(attribution);
                };
            }

			if (adjustConfig.sessionSuccessDelegate != null) {
				sessionSuccessChangedAction = (sessionMap) => {
                    var sessionData = new AdjustSessionSuccess(sessionMap);
				    adjustConfig.sessionSuccessDelegate(sessionData);
				};
			}

			if (adjustConfig.sessionFailureDelegate != null) {
				sessionFailureChangedAction = (sessionMap) => {
				    var sessionData = new AdjustSessionFailure(sessionMap);
				    adjustConfig.sessionFailureDelegate(sessionData);
                };
			}

			if(adjustConfig.eventSuccessDelegate != null) {
				eventSuccessChangedAction = (eventMap) => {
                    var eventData = new AdjustEventSuccess(eventMap);
				    adjustConfig.eventSuccessDelegate(eventData);
				};
			}

			if (adjustConfig.eventFailureDelegate != null) {
				eventFailureChangedAction = (eventMap) => {
				    var eventData = new AdjustEventFailure(eventMap);
				    adjustConfig.eventFailureDelegate(eventData);
                };
			}

			if (adjustConfig.deferredDeeplinkDelegate != null) {
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
				FuncDeeplinkResponseData = deeplinkResponseFunc,
				IsDeviceKnown = adjustConfig.isDeviceKnown,
				SecretId = adjustConfig.secretId,
				AppSecretInfo1 = adjustConfig.appSecretInfo1,
				AppSecretInfo2 = adjustConfig.appSecretInfo2,
				AppSecretInfo3 = adjustConfig.appSecretInfo3,
				AppSecretInfo4 = adjustConfig.appSecretInfo4
			};

			AdjustWinInterface.ApplicationLaunching (adjustConfigDto);
            AdjustWinInterface.ApplicationActivated();
            appLaunched = true;
        }

        public static void trackEvent(AdjustEvent adjustEvent) {
			AdjustWinInterface.TrackEvent (
				eventToken: adjustEvent.eventToken,
				revenue: adjustEvent.revenue,
				currency: adjustEvent.currency,
				purchaseId: adjustEvent.transactionId,
				callbackList: adjustEvent.callbackList,
				partnerList: adjustEvent.partnerList
			);
        }

        public static void sendFirstPackages() {
			AdjustWinInterface.SendFirstPackages ();
		}

        public static void setDeviceToken(string deviceToken) {
			AdjustWinInterface.SetDeviceToken (deviceToken);
		}

        public static void appWillOpenUrl(string url)
        {
            AdjustWinInterface.AppWillOpenUrl(url);
        }

		public static string getWinAdid() {
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
    }
}
#endif
