using System;
using System.Collections.Generic;

using UnityEngine;

namespace com.adjust.sdk {
    public class Adjust : MonoBehaviour {
        #region Adjust fields
        private const string errorMessage = "adjust: SDK not started. Start it manually using the 'start' method.";

        private static IAdjust instance = null;

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
        #endregion

        #region Unity lifecycle methods
        void Awake() {
            if (Adjust.instance != null) {
                return;
            }
              
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
            if (Adjust.instance == null) {
                return;
            }
            
            if (pauseStatus) {
                Adjust.instance.onPause();
            } else {
                Adjust.instance.onResume();
            }
        }
        #endregion

        #region Adjust methods
        public static void start(AdjustConfig adjustConfig) {
            if (Adjust.instance != null) {
                Debug.Log("adjust: Error, SDK already started.");
                return;
            }

            if (adjustConfig == null) {
                Debug.Log("adjust: Missing config to start.");
                return;
            }

            #if UNITY_EDITOR
                Adjust.instance = null;
            #elif UNITY_IOS
                Adjust.instance = new AdjustiOS();
            #elif UNITY_ANDROID
                Adjust.instance = new AdjustAndroid();
            #elif UNITY_WP8
                Adjust.instance = new AdjustWP8();
            #elif UNITY_METRO
                Adjust.instance = new AdjustMetro();
            #else
                Adjust.instance = null;
            #endif

            if (Adjust.instance == null) {
                Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
                return;
            }

            Adjust.eventSuccessDelegate = adjustConfig.getEventSuccessDelegate();
            Adjust.eventFailureDelegate = adjustConfig.getEventFailureDelegate();
            Adjust.sessionSuccessDelegate = adjustConfig.getSessionSuccessDelegate();
            Adjust.sessionFailureDelegate = adjustConfig.getSessionFailureDelegate();
            Adjust.deferredDeeplinkDelegate = adjustConfig.getDeferredDeeplinkDelegate();
            Adjust.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate();

            Adjust.instance.start(adjustConfig);
        }

        public static void trackEvent(AdjustEvent adjustEvent) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }
            
            if (adjustEvent == null) {
                Debug.Log("adjust: Missing event to track.");
                return;
            }

            Adjust.instance.trackEvent(adjustEvent);
        }

        public static void setEnabled(bool enabled) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.setEnabled(enabled);
        }

        public static bool isEnabled() {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return false;
            }

            return Adjust.instance.isEnabled();
        }

        public static void setOfflineMode(bool enabled) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.setOfflineMode(enabled);
        }

        public static void sendFirstPackages() {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.sendFirstPackages();
        }

        public static void addSessionPartnerParameter(string key, string value) {
            #if UNITY_IOS
                AdjustiOS.addSessionPartnerParameter(key, value);
            #elif UNITY_ANDROID
                AdjustAndroid.addSessionPartnerParameter(key, value);
            #elif UNITY_WP8
                AdjustWP8.addSessionPartnerParameter(key, value);
            #elif UNITY_METRO
                AdjustMetro.addSessionPartnerParameter(key, value);
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
            #elif UNITY_WP8
                AdjustWP8.addSessionCallbackParameter(key, value);
            #elif UNITY_METRO
                AdjustMetro.addSessionCallbackParameter(key, value);
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
            #elif UNITY_WP8
                AdjustWP8.removeSessionPartnerParameter(key);
            #elif UNITY_METRO
                AdjustMetro.removeSessionPartnerParameter(key);
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
            #elif UNITY_WP8
                AdjustWP8.removeSessionCallbackParameter(key);
            #elif UNITY_METRO
                AdjustMetro.removeSessionCallbackParameter(key);
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
            #elif UNITY_WP8
                AdjustWP8.resetSessionPartnerParameters();
            #elif UNITY_METRO
                AdjustMetro.resetSessionPartnerParameters();
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
            #elif UNITY_WP8
                AdjustWP8.resetSessionCallbackParameters();
            #elif UNITY_METRO
                AdjustMetro.resetSessionCallbackParameters();
            #else
                Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
                return;
            #endif
        }

        public static string getAdid() {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return null;
            }

            return Adjust.instance.getAdid();
        }

        public static AdjustAttribution getAttribution() {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return null;
            }

            return Adjust.instance.getAttribution();
        }
        
        // iOS specific methods
        public static void setDeviceToken(string deviceToken) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.setDeviceToken(deviceToken);
        }

        public static string getIdfa() {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return null;
            }

            return Adjust.instance.getIdfa();
        }

        // Android specific methods
        public static void setReferrer(string referrer) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.setReferrer(referrer);
        }

        public static void getGoogleAdId(Action<string> onDeviceIdsRead) {
            if (Adjust.instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            Adjust.instance.getGoogleAdId(onDeviceIdsRead);
        }
        #endregion

        #region Attribution callback

        public static void runAttributionChangedDictionary(Dictionary<string, string> dicAttributionData)
        {
            if (instance == null)
            {
                Debug.Log(Adjust.errorMessage);
                return;
            }
            if (Adjust.attributionChangedDelegate == null)
            {
                Debug.Log("adjust: Attribution changed delegate was not set.");
                return;
            }
            var attribution = new AdjustAttribution(dicAttributionData);
            Adjust.attributionChangedDelegate(attribution);
        }

        public void GetNativeAttribution(string attributionData) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            if (Adjust.attributionChangedDelegate == null) {
                Debug.Log("adjust: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdjustAttribution(attributionData);
            Adjust.attributionChangedDelegate(attribution);
        }

        public void GetNativeEventSuccess(string eventSuccessData) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            if (Adjust.eventSuccessDelegate == null) {
                Debug.Log("adjust: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdjustEventSuccess(eventSuccessData);
            Adjust.eventSuccessDelegate(eventSuccess);
        }

        public void GetNativeEventFailure(string eventFailureData) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            if (Adjust.eventFailureDelegate == null) {
                Debug.Log("adjust: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdjustEventFailure(eventFailureData);
            Adjust.eventFailureDelegate(eventFailure);
        }

        public void GetNativeSessionSuccess(string sessionSuccessData) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            if (Adjust.sessionSuccessDelegate == null) {
                Debug.Log("adjust: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdjustSessionSuccess(sessionSuccessData);
            Adjust.sessionSuccessDelegate(sessionSuccess);
        }

        public void GetNativeSessionFailure(string sessionFailureData) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

            if (Adjust.sessionFailureDelegate == null) {
                Debug.Log("adjust: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdjustSessionFailure(sessionFailureData);
            Adjust.sessionFailureDelegate(sessionFailure);
        }

        public void GetNativeDeferredDeeplink(string deeplinkURL) {
            if (instance == null) {
                Debug.Log(Adjust.errorMessage);
                return;
            }

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
