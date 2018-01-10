using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk {
    public class Adjust : MonoBehaviour {
        
        private const string errorMessage = "adjust: SDK not started. Start it manually using the 'start' method.";

        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<AdjustEventSuccess> eventSuccessDelegate = null;
        private static Action<AdjustEventFailure> eventFailureDelegate = null;
        private static Action<AdjustSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdjustSessionFailure> sessionFailureDelegate = null;
        private static Action<AdjustAttribution> attributionChangedDelegate = null;

        public bool startManually = true;
        public bool eventBuffering = false;
        public bool printAttribution = true;
        public bool sendInBackground = false;
        public bool launchDeferredDeeplink = true;

        public string appToken = "{Your App Token}";

        public AdjustLogLevel logLevel = AdjustLogLevel.Info;
        public AdjustEnvironment environment = AdjustEnvironment.Sandbox;

        #region Unity lifecycle methods
        void Awake() {
            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually) {
                AdjustConfig adjustConfig;

                adjustConfig = new AdjustConfig(this.appToken, this.environment, (this.logLevel == AdjustLogLevel.Suppress));

                adjustConfig.setLogLevel(this.logLevel);
                adjustConfig.setSendInBackground(this.sendInBackground);
                adjustConfig.setEventBufferingEnabled(this.eventBuffering);
                adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);

                if (printAttribution) {
                    adjustConfig.setEventSuccessDelegate(EventSuccessCallback);
                    adjustConfig.setEventFailureDelegate(EventFailureCallback);
                    adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);
                    adjustConfig.setSessionFailureDelegate(SessionFailureCallback);
                    adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
                    adjustConfig.setAttributionChangedDelegate(AttributionChangedCallback);
                }

                Adjust.start(adjustConfig);
            }
        }

        void OnApplicationPause(bool pauseStatus) {
#if UNITY_IOS
            // nop
#elif UNITY_ANDROID
            if (pauseStatus) {
                AdjustAndroid.onPause();
            }
            else {
                AdjustAndroid.onResume();
            }
#elif (UNITY_WSA || UNITY_WP8)
            if (pauseStatus) {
                AdjustWindows.onPause();
            }
            else {
                AdjustWindows.onResume();
            }
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }
        #endregion

        #region Adjust methods
        public static void start(AdjustConfig adjustConfig) {            
            if (adjustConfig == null) {
                Debug.Log("adjust: Missing config to start.");
                return;
            }

#if UNITY_IOS
            AdjustiOS.start(adjustConfig);
#elif UNITY_ANDROID
            AdjustAndroid.start(adjustConfig);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.start(adjustConfig);            
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif

            Adjust.eventSuccessDelegate = adjustConfig.getEventSuccessDelegate();
            Adjust.eventFailureDelegate = adjustConfig.getEventFailureDelegate();
            Adjust.sessionSuccessDelegate = adjustConfig.getSessionSuccessDelegate();
            Adjust.sessionFailureDelegate = adjustConfig.getSessionFailureDelegate();
            Adjust.deferredDeeplinkDelegate = adjustConfig.getDeferredDeeplinkDelegate();
            Adjust.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate();
        }

        public static void trackEvent(AdjustEvent adjustEvent) {
            if (adjustEvent == null) {
                Debug.Log("adjust: Missing event to track.");
                return;
            }

#if UNITY_IOS
            AdjustiOS.trackEvent(adjustEvent);
#elif UNITY_ANDROID
            AdjustAndroid.trackEvent(adjustEvent);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.trackEvent(adjustEvent);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }

        public static void setEnabled(bool enabled) {
#if UNITY_IOS
            AdjustiOS.setEnabled(enabled);
#elif UNITY_ANDROID
            AdjustAndroid.setEnabled(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.setEnabled(enabled);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }

        public static bool isEnabled() {
#if UNITY_IOS
            return AdjustiOS.isEnabled();
#elif UNITY_ANDROID
            return AdjustAndroid.isEnabled();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.isEnabled();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }

        public static void setOfflineMode(bool enabled) {
#if UNITY_IOS
            AdjustiOS.setOfflineMode(enabled);
#elif UNITY_ANDROID
            AdjustAndroid.setOfflineMode(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.setOfflineMode(enabled);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }

        public static void setDeviceToken(string deviceToken)
        {
#if UNITY_IOS
            AdjustiOS.setDeviceToken(deviceToken);
#elif UNITY_ANDROID
            AdjustAndroid.setDeviceToken(deviceToken);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.setDeviceToken(deviceToken);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void appWillOpenUrl(string url)
        {
#if UNITY_IOS
            AdjustiOS.appWillOpenUrl(url);
#elif UNITY_ANDROID
            AdjustAndroid.appWillOpenUrl(url);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.appWillOpenUrl(url);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void sendFirstPackages() {
#if UNITY_IOS
            AdjustiOS.sendFirstPackages();
#elif UNITY_ANDROID
            AdjustAndroid.sendFirstPackages();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.sendFirstPackages();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
#endif
        }

        public static void addSessionPartnerParameter(string key, string value) {
#if UNITY_IOS
            AdjustiOS.addSessionPartnerParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.addSessionPartnerParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.addSessionPartnerParameter(key, value);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void addSessionCallbackParameter(string key, string value) {
#if UNITY_IOS
            AdjustiOS.addSessionCallbackParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.addSessionCallbackParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.addSessionCallbackParameter(key, value);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void removeSessionPartnerParameter(string key) {
#if UNITY_IOS
            AdjustiOS.removeSessionPartnerParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.removeSessionPartnerParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.removeSessionPartnerParameter(key);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void removeSessionCallbackParameter(string key) {
#if UNITY_IOS
            AdjustiOS.removeSessionCallbackParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.removeSessionCallbackParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.removeSessionCallbackParameter(key);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void resetSessionPartnerParameters() {
#if UNITY_IOS
            AdjustiOS.resetSessionPartnerParameters();
#elif UNITY_ANDROID
            AdjustAndroid.resetSessionPartnerParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.resetSessionPartnerParameters();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void resetSessionCallbackParameters() {
#if UNITY_IOS
            AdjustiOS.resetSessionCallbackParameters();
#elif UNITY_ANDROID
            AdjustAndroid.resetSessionCallbackParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.resetSessionCallbackParameters();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static string getAdid() {
#if UNITY_IOS
            return AdjustiOS.getAdid();
#elif UNITY_ANDROID
            return AdjustAndroid.getAdid();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.getAdid();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static AdjustAttribution getAttribution()
        {
#if UNITY_IOS
            return AdjustiOS.getAttribution();
#elif UNITY_ANDROID
            return AdjustAndroid.getAttribution();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.getAttribution();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static string getWinAdid() {
#if UNITY_IOS
            Debug.Log("adjust: Error! Win ADID is not available on iOS Platform.");
            return string.Empty;
#elif UNITY_ANDROID
            Debug.Log("adjust: Error! Win ADID is not available on Android Platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.getWinAdid();
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static string getIdfa() {
#if UNITY_IOS
            return AdjustiOS.getIdfa();
#elif UNITY_ANDROID
            Debug.Log("adjust: Error! IDFA not available on Android Platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("adjust: Error! IDFA not available on Windows Platform.");
            return string.Empty;
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void setReferrer(string referrer) {
#if UNITY_IOS
            Debug.Log("adjust: Referrer not available on iOS Platform.");
#elif UNITY_ANDROID
            AdjustAndroid.setReferrer(referrer);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("adjust: Error! Referrer not available on Windows Platform.");
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static void getGoogleAdId(Action<string> onDeviceIdsRead) {
#if UNITY_IOS
            Debug.Log("adjust: Google Ad ID not available on iOS Platform.");
            onDeviceIdsRead(string.Empty);
#elif UNITY_ANDROID
            AdjustAndroid.getGoogleAdId(onDeviceIdsRead);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("adjust: Google Ad ID not available on Windows Platform.");
            onDeviceIdsRead(string.Empty);
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }

        public static string getAmazonAdId()
        {
#if UNITY_IOS
            Debug.Log("adjust: Amazon Ad ID not available on iOS Platform.");
            return string.Empty;
#elif UNITY_ANDROID
            return AdjustAndroid.GetAmazonAdId();
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("adjust: Amazon Ad ID not available on Windows Platform.");
            return string.Empty;
#else
            Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
            return;
#endif
        }
        #endregion

        #region callbacks
        public static void GetNativeAttribution(string attributionData) {
            if (Adjust.attributionChangedDelegate == null) {
                Debug.Log("adjust: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdjustAttribution(attributionData);
            Adjust.attributionChangedDelegate(attribution);
        }

        public static void GetNativeEventSuccess(string eventSuccessData) {
            if (Adjust.eventSuccessDelegate == null) {
                Debug.Log("adjust: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdjustEventSuccess(eventSuccessData);
            Adjust.eventSuccessDelegate(eventSuccess);
        }

        public static void GetNativeEventFailure(string eventFailureData) {
            if (Adjust.eventFailureDelegate == null) {
                Debug.Log("adjust: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdjustEventFailure(eventFailureData);
            Adjust.eventFailureDelegate(eventFailure);
        }

        public static void GetNativeSessionSuccess(string sessionSuccessData) {
            if (Adjust.sessionSuccessDelegate == null) {
                Debug.Log("adjust: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdjustSessionSuccess(sessionSuccessData);
            Adjust.sessionSuccessDelegate(sessionSuccess);
        }

        public static void GetNativeSessionFailure(string sessionFailureData) {
            if (Adjust.sessionFailureDelegate == null) {
                Debug.Log("adjust: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdjustSessionFailure(sessionFailureData);
            Adjust.sessionFailureDelegate(sessionFailure);
        }

        public static void GetNativeDeferredDeeplink(string deeplinkURL) {
            if (Adjust.deferredDeeplinkDelegate == null) {
                Debug.Log("adjust: Deferred deeplink delegate was not set.");
                return;
            }

            Adjust.deferredDeeplinkDelegate(deeplinkURL);
        }
        #endregion

        #region Private & helper methods
        // Our delegate for detecting attribution changes if chosen not to start manually.
        private void AttributionChangedCallback(AdjustAttribution attributionData) {
            Debug.Log("Attribution changed!");

            if (attributionData.trackerName != null) {
                Debug.Log("Tracker name: " + attributionData.trackerName);
            }

            if (attributionData.trackerToken != null) {
                Debug.Log("Tracker token: " + attributionData.trackerToken);
            }

            if (attributionData.network != null) {
                Debug.Log("Network: " + attributionData.network);
            }

            if (attributionData.campaign != null) {
                Debug.Log("Campaign: " + attributionData.campaign);
            }

            if (attributionData.adgroup != null) {
                Debug.Log("Adgroup: " + attributionData.adgroup);
            }

            if (attributionData.creative != null) {
                Debug.Log("Creative: " + attributionData.creative);
            }

            if (attributionData.clickLabel != null) {
                Debug.Log("Click label: " + attributionData.clickLabel);
            }

            if (attributionData.adid != null) {
                Debug.Log("ADID: " + attributionData.adid);
            }
        }

        // Our delegate for detecting successful event tracking if chosen not to start manually.
        private void EventSuccessCallback(AdjustEventSuccess eventSuccessData) {
            Debug.Log("Event tracked successfully!");

            if (eventSuccessData.Message != null) {
                Debug.Log("Message: " + eventSuccessData.Message);
            }

            if (eventSuccessData.Timestamp != null) {
                Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
            }

            if (eventSuccessData.Adid != null) {
                Debug.Log("Adid: " + eventSuccessData.Adid);
            }

            if (eventSuccessData.EventToken != null) {
                Debug.Log("EventToken: " + eventSuccessData.EventToken);
            }

            if (eventSuccessData.JsonResponse != null) {
                Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
            }
        }

        // Our delegate for detecting failed event tracking if chosen not to start manually.
        private void EventFailureCallback(AdjustEventFailure eventFailureData) {
            Debug.Log("Event tracking failed!");

            if (eventFailureData.Message != null) {
                Debug.Log("Message: " + eventFailureData.Message);
            }

            if (eventFailureData.Timestamp != null) {
                Debug.Log("Timestamp: " + eventFailureData.Timestamp);
            }

            if (eventFailureData.Adid != null) {
                Debug.Log("Adid: " + eventFailureData.Adid);
            }

            if (eventFailureData.EventToken != null) {
                Debug.Log("EventToken: " + eventFailureData.EventToken);
            }

            Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());

            if (eventFailureData.JsonResponse != null) {
                Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
            }
        }

        // Our delegate for detecting successful session tracking if chosen not to start manually.
        private void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData) {
            Debug.Log("Session tracked successfully!");

            if (sessionSuccessData.Message != null) {
                Debug.Log("Message: " + sessionSuccessData.Message);
            }

            if (sessionSuccessData.Timestamp != null) {
                Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
            }

            if (sessionSuccessData.Adid != null) {
                Debug.Log("Adid: " + sessionSuccessData.Adid);
            }

            if (sessionSuccessData.JsonResponse != null) {
                Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
            }
        }

        // Our delegate for detecting failed session tracking if chosen not to start manually.
        private void SessionFailureCallback(AdjustSessionFailure sessionFailureData) {
            Debug.Log("Session tracking failed!");

            if (sessionFailureData.Message != null) {
                Debug.Log("Message: " + sessionFailureData.Message);
            }

            if (sessionFailureData.Timestamp != null) {
                Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
            }

            if (sessionFailureData.Adid != null) {
                Debug.Log("Adid: " + sessionFailureData.Adid);
            }

            Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());

            if (sessionFailureData.JsonResponse != null) {
                Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
            }
        }

        // Our delegate for getting deferred deep link content if chosen not to start manually.
        private void DeferredDeeplinkCallback(string deeplinkURL) {
            Debug.Log("Deferred deeplink reported!");

            if (deeplinkURL != null) {
                Debug.Log("Deeplink URL: " + deeplinkURL);
            } else {
                Debug.Log("Deeplink URL is null!");
            }
        }
        #endregion
    }
}
