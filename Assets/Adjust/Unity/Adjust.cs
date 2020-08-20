using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
    public class Adjust : MonoBehaviour
    {
        private const string errorMsgEditor = "[Adjust]: SDK can not be used in Editor.";
        private const string errorMsgStart = "[Adjust]: SDK not started. Start it manually using the 'start' method.";
        private const string errorMsgPlatform = "[Adjust]: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

        public bool startManually = true;
        public bool eventBuffering = false;
        public bool sendInBackground = false;
        public bool launchDeferredDeeplink = true;

        public string appToken = "{Your App Token}";

        public AdjustLogLevel logLevel = AdjustLogLevel.Info;
        public AdjustEnvironment environment = AdjustEnvironment.Sandbox;

#if UNITY_IOS
        // Delegate references for iOS callback triggering
        private static List<Action<int>> authorizationStatusDelegates = null;
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<AdjustEventSuccess> eventSuccessDelegate = null;
        private static Action<AdjustEventFailure> eventFailureDelegate = null;
        private static Action<AdjustSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdjustSessionFailure> sessionFailureDelegate = null;
        private static Action<AdjustAttribution> attributionChangedDelegate = null;
#endif

        void Awake()
        {
            if (IsEditor()) { return; }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                AdjustConfig adjustConfig = new AdjustConfig(this.appToken, this.environment, (this.logLevel == AdjustLogLevel.Suppress));
                adjustConfig.setLogLevel(this.logLevel);
                adjustConfig.setSendInBackground(this.sendInBackground);
                adjustConfig.setEventBufferingEnabled(this.eventBuffering);
                adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
                Adjust.start(adjustConfig);
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
                // No action, iOS SDK is subscribed to iOS lifecycle notifications.
#elif UNITY_ANDROID
                if (pauseStatus)
                {
                    AdjustAndroid.OnPause();
                }
                else
                {
                    AdjustAndroid.OnResume();
                }
#elif (UNITY_WSA || UNITY_WP8)
                if (pauseStatus)
                {
                    AdjustWindows.OnPause();
                }
                else
                {
                    AdjustWindows.OnResume();
                }
#else
                Debug.Log(errorMsgPlatform);
#endif
        }

        public static void start(AdjustConfig adjustConfig)
        {
            if (IsEditor()) { return; }

            if (adjustConfig == null)
            {
                Debug.Log("[Adjust]: Missing config to start.");
                return;
            }

#if UNITY_IOS
                Adjust.eventSuccessDelegate = adjustConfig.getEventSuccessDelegate();
                Adjust.eventFailureDelegate = adjustConfig.getEventFailureDelegate();
                Adjust.sessionSuccessDelegate = adjustConfig.getSessionSuccessDelegate();
                Adjust.sessionFailureDelegate = adjustConfig.getSessionFailureDelegate();
                Adjust.deferredDeeplinkDelegate = adjustConfig.getDeferredDeeplinkDelegate();
                Adjust.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate();
                AdjustiOS.Start(adjustConfig);
#elif UNITY_ANDROID
                AdjustAndroid.Start(adjustConfig);
#elif (UNITY_WSA || UNITY_WP8)
                AdjustWindows.Start(adjustConfig);
#else
                Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackEvent(AdjustEvent adjustEvent)
        {
            if (IsEditor()) { return; }

            if (adjustEvent == null)
            {
                Debug.Log("[Adjust]: Missing event to track.");
                return;
            }
#if UNITY_IOS
            AdjustiOS.TrackEvent(adjustEvent);
#elif UNITY_ANDROID
            AdjustAndroid.TrackEvent(adjustEvent);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.TrackEvent(adjustEvent);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void setEnabled(bool enabled)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.SetEnabled(enabled);
#elif UNITY_ANDROID
            AdjustAndroid.SetEnabled(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetEnabled(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static bool isEnabled()
        {
            if (IsEditor()) { return false; }

#if UNITY_IOS
            return AdjustiOS.IsEnabled();
#elif UNITY_ANDROID
            return AdjustAndroid.IsEnabled();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.IsEnabled();
#else
            Debug.Log(errorMsgPlatform);
            return false;
#endif
        }

        public static void setOfflineMode(bool enabled)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.SetOfflineMode(enabled);
#elif UNITY_ANDROID
            AdjustAndroid.SetOfflineMode(enabled);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetOfflineMode(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void setDeviceToken(string deviceToken)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.SetDeviceToken(deviceToken);
#elif UNITY_ANDROID
            AdjustAndroid.SetDeviceToken(deviceToken);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetDeviceToken(deviceToken);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void gdprForgetMe()
        {
#if UNITY_IOS
            AdjustiOS.GdprForgetMe();
#elif UNITY_ANDROID
            AdjustAndroid.GdprForgetMe();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.GdprForgetMe();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void disableThirdPartySharing()
        {
#if UNITY_IOS
            AdjustiOS.DisableThirdPartySharing();
#elif UNITY_ANDROID
            AdjustAndroid.DisableThirdPartySharing();
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Disable third party sharing is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void appWillOpenUrl(string url)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.AppWillOpenUrl(url);
#elif UNITY_ANDROID
            AdjustAndroid.AppWillOpenUrl(url);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AppWillOpenUrl(url);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void sendFirstPackages()
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.SendFirstPackages();
#elif UNITY_ANDROID
            AdjustAndroid.SendFirstPackages();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SendFirstPackages();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void addSessionPartnerParameter(string key, string value)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.AddSessionPartnerParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.AddSessionPartnerParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AddSessionPartnerParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void addSessionCallbackParameter(string key, string value)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.AddSessionCallbackParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.AddSessionCallbackParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AddSessionCallbackParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void removeSessionPartnerParameter(string key)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.RemoveSessionPartnerParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.RemoveSessionPartnerParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.RemoveSessionPartnerParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void removeSessionCallbackParameter(string key)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.RemoveSessionCallbackParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.RemoveSessionCallbackParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.RemoveSessionCallbackParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void resetSessionPartnerParameters()
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.ResetSessionPartnerParameters();
#elif UNITY_ANDROID
            AdjustAndroid.ResetSessionPartnerParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.ResetSessionPartnerParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void resetSessionCallbackParameters()
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.ResetSessionCallbackParameters();
#elif UNITY_ANDROID
            AdjustAndroid.ResetSessionCallbackParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.ResetSessionCallbackParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackAdRevenue(string source, string payload)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.TrackAdRevenue(source, payload);
#elif UNITY_ANDROID
            AdjustAndroid.TrackAdRevenue(source, payload);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Ad revenue tracking is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackAppStoreSubscription(AdjustAppStoreSubscription subscription)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.TrackAppStoreSubscription(subscription);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: App Store subscription tracking is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: App Store subscription tracking is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void trackPlayStoreSubscription(AdjustPlayStoreSubscription subscription)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            Debug.Log("[Adjust]: Play Store subscription tracking is only supported for Android platform.");
#elif UNITY_ANDROID
            AdjustAndroid.TrackPlayStoreSubscription(subscription);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Play Store subscription tracking is only supported for Android platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void requestTrackingAuthorizationWithCompletionHandler(Action<int> statusCallback)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            if (Adjust.authorizationStatusDelegates == null)
            {
                Adjust.authorizationStatusDelegates = new List<Action<int>>();
            }
            Adjust.authorizationStatusDelegates.Add(statusCallback);
            AdjustiOS.RequestTrackingAuthorizationWithCompletionHandler();
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Requesting tracking authorization is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Requesting tracking authorization is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static string getAdid()
        {
            if (IsEditor()) { return string.Empty; }

#if UNITY_IOS
            return AdjustiOS.GetAdid();
#elif UNITY_ANDROID
            return AdjustAndroid.GetAdid();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetAdid();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static AdjustAttribution getAttribution()
        {
            if (IsEditor()) { return null; }

#if UNITY_IOS
            return AdjustiOS.GetAttribution();
#elif UNITY_ANDROID
            return AdjustAndroid.GetAttribution();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetAttribution();
#else
            Debug.Log(errorMsgPlatform);
            return null;
#endif
        }

        public static string getWinAdid()
        {
            if (IsEditor()) { return string.Empty; }

#if UNITY_IOS
            Debug.Log("[Adjust]: Error! Windows Advertising ID is not available on iOS platform.");
            return string.Empty;
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! Windows Advertising ID is not available on Android platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetWinAdId();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static string getIdfa()
        {
            if (IsEditor()) { return string.Empty; }

#if UNITY_IOS
            return AdjustiOS.GetIdfa();
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! IDFA is not available on Android platform.");
            return string.Empty;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! IDFA is not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static string getSdkVersion()
        {
            if (IsEditor()) { return string.Empty; }

#if UNITY_IOS
            return AdjustiOS.GetSdkVersion();
#elif UNITY_ANDROID
            return AdjustAndroid.GetSdkVersion();
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetSdkVersion();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        [Obsolete("This method is intended for testing purposes only. Do not use it.")]
        public static void setReferrer(string referrer)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            Debug.Log("[Adjust]: Install referrer is not available on iOS platform.");
#elif UNITY_ANDROID
            AdjustAndroid.SetReferrer(referrer);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! Install referrer is not available on Windows platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void getGoogleAdId(Action<string> onDeviceIdsRead)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            Debug.Log("[Adjust]: Google Play Advertising ID is not available on iOS platform.");
            onDeviceIdsRead(string.Empty);
#elif UNITY_ANDROID
            AdjustAndroid.GetGoogleAdId(onDeviceIdsRead);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Google Play Advertising ID is not available on Windows platform.");
            onDeviceIdsRead(string.Empty);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static string getAmazonAdId()
        {
            if (IsEditor()) { return string.Empty; }

#if UNITY_IOS
            Debug.Log("[Adjust]: Amazon Advertising ID is not available on iOS platform.");
            return string.Empty;
#elif UNITY_ANDROID
            return AdjustAndroid.GetAmazonAdId();
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Amazon Advertising ID not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

#if UNITY_IOS
        public void GetNativeAttribution(string attributionData)
        {
            if (IsEditor()) { return; }

            if (Adjust.attributionChangedDelegate == null)
            {
                Debug.Log("[Adjust]: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdjustAttribution(attributionData);
            Adjust.attributionChangedDelegate(attribution);
        }

        public void GetNativeEventSuccess(string eventSuccessData)
        {
            if (IsEditor()) { return; }

            if (Adjust.eventSuccessDelegate == null)
            {
                Debug.Log("[Adjust]: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdjustEventSuccess(eventSuccessData);
            Adjust.eventSuccessDelegate(eventSuccess);
        }

        public void GetNativeEventFailure(string eventFailureData)
        {
            if (IsEditor()) { return; }

            if (Adjust.eventFailureDelegate == null)
            {
                Debug.Log("[Adjust]: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdjustEventFailure(eventFailureData);
            Adjust.eventFailureDelegate(eventFailure);
        }

        public void GetNativeSessionSuccess(string sessionSuccessData)
        {
            if (IsEditor()) { return; }

            if (Adjust.sessionSuccessDelegate == null)
            {
                Debug.Log("[Adjust]: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdjustSessionSuccess(sessionSuccessData);
            Adjust.sessionSuccessDelegate(sessionSuccess);
        }

        public void GetNativeSessionFailure(string sessionFailureData)
        {
            if (IsEditor()) { return; }

            if (Adjust.sessionFailureDelegate == null)
            {
                Debug.Log("[Adjust]: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdjustSessionFailure(sessionFailureData);
            Adjust.sessionFailureDelegate(sessionFailure);
        }

        public void GetNativeDeferredDeeplink(string deeplinkURL)
        {
            if (IsEditor()) { return; }

            if (Adjust.deferredDeeplinkDelegate == null)
            {
                Debug.Log("[Adjust]: Deferred deeplink delegate was not set.");
                return;
            }

            Adjust.deferredDeeplinkDelegate(deeplinkURL);
        }

        public void GetAuthorizationStatus(string authorizationStatus)
        {
            if (IsEditor()) { return; }

            if (Adjust.authorizationStatusDelegates == null)
            {
                Debug.Log("[Adjust]: Authorization status delegates were not set.");
                return;
            }

            foreach (Action<int> callback in Adjust.authorizationStatusDelegates)
            {
                callback(Int16.Parse(authorizationStatus));
            }
            Adjust.authorizationStatusDelegates.Clear();
        }
#endif

        private static bool IsEditor()
        {
#if UNITY_EDITOR
            Debug.Log(errorMsgEditor);
            return true;
#else
            return false;
#endif
        }

        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            if (IsEditor()) { return; }

#if UNITY_IOS
            AdjustiOS.SetTestOptions(testOptions);
#elif UNITY_ANDROID
            AdjustAndroid.SetTestOptions(testOptions);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetTestOptions(testOptions);
#else
            Debug.Log("[Adjust]: Cannot run integration tests. None of the supported platforms selected.");
#endif
        }
    }
}
