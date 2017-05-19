#if UNITY_METRO
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_WSA_10_0
using AdjustUnityWS10;
#elif UNITY_WINRT_8_1
using AdjustUnityWS81;
#endif
namespace com.adjust.sdk {
    public class AdjustMetro : IAdjust {
        private const string sdkPrefix = "unity4.11.3";

        public bool isEnabled() {
#if UNITY_WSA_10_0
            return AdjustWS10.IsEnabled();
#elif UNITY_WINRT_8_1
            return AdjustWS81.IsEnabled();
#endif
        }

        public string getAdid() {
            return null;
        }

        public AdjustAttribution getAttribution() {
            return null;
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
            Action<Dictionary<string, string>> attributionChangedDictionary = null;

            if (adjustConfig.logLevel.HasValue) {
                logLevelString = lowercaseToString(adjustConfig.logLevel.Value);
            }

            if (adjustConfig.attributionChangedDelegate != null) {
                attributionChangedDictionary = (attributionDictionary) => Adjust.runAttributionChangedDictionary(attributionDictionary);
            }
#if UNITY_WSA_10_0
            AdjustWS10.ApplicationLaunching(
#elif UNITY_WINRT_8_1
            AdjustWS81.ApplicationLaunching(
#endif
                appToken:adjustConfig.appToken,
                logLevelString:logLevelString,
                environment:environment,
                defaultTracker:adjustConfig.defaultTracker,
                eventBufferingEnabled:adjustConfig.eventBufferingEnabled,
                sdkPrefix:sdkPrefix,
                attributionChangedDic:attributionChangedDictionary,
                logDelegate:adjustConfig.logDelegate
            );
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

        public void trackEvent(AdjustEvent adjustEvent) {
#if UNITY_WSA_10_0
            AdjustWS10.TrackEvent(
#elif UNITY_WINRT_8_1
            AdjustWS81.TrackEvent(
#endif
                eventToken:adjustEvent.eventToken,
                revenue:adjustEvent.revenue,
                currency:adjustEvent.currency,
                callbackList:adjustEvent.callbackList,
                partnerList:adjustEvent.partnerList
            );
        }

        public void sendFirstPackages() {}

        public void setDeviceToken(string deviceToken) {}

        public static void addSessionPartnerParameter(string key, string value) {}

        public static void addSessionCallbackParameter(string key, string value) {}

        public static void removeSessionPartnerParameter(string key) {}

        public static void removeSessionCallbackParameter(string key) {}

        public static void resetSessionPartnerParameters() {}

        public static void resetSessionCallbackParameters() {}

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
