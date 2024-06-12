using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
    public class Adjust : MonoBehaviour
    {
        private const string errorMsgEditor = "[Adjust]: SDK can not be used in Editor.";
        private const string errorMsgStart = "[Adjust]: SDK not started. Start it manually using the 'start' method.";
        private const string errorMsgPlatform = "[Adjust]: SDK can only be used in Android and iOS apps.";

        // [Header("SDK SETTINGS:")]
        // [Space(5)]
        // [Tooltip("If selected, it is expected from you to initialize Adjust SDK from your app code. " +
        //     "Any SDK configuration settings from prefab will be ignored in that case.")]
        [HideInInspector]
        public bool startManually = true;
        [HideInInspector]
        public string appToken;
        [HideInInspector]
        public AdjustEnvironment environment = AdjustEnvironment.Sandbox;
        [HideInInspector]
        public AdjustLogLevel logLevel = AdjustLogLevel.Info;
        [HideInInspector]
        public bool sendInBackground = false;
        [HideInInspector]
        public bool launchDeferredDeeplink = true;
        [HideInInspector]
        public bool costDataInAttribution = false;
        [HideInInspector]
        public bool linkMe = false;
        [HideInInspector]
        public string defaultTracker;

        // [Header("ANDROID SPECIFIC FEATURES:")]
        // [Space(5)]
        [HideInInspector]
        public bool preinstallTracking = false;
        [HideInInspector]
        public string preinstallFilePath;

        // [Header("iOS SPECIFIC FEATURES:")]
        // [Space(5)]
        [HideInInspector]
        public bool adServices = true;
        [HideInInspector]
        public bool idfaReading = true;
        [HideInInspector]
        public bool skanAttribution = true;

#if UNITY_IOS
        // subscriptions
        private static Action<AdjustAttribution> attributionChangedDelegate = null;
        private static Action<AdjustSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdjustSessionFailure> sessionFailureDelegate = null;
        private static Action<AdjustEventSuccess> eventSuccessDelegate = null;
        private static Action<AdjustEventFailure> eventFailureDelegate = null;
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<Dictionary<string, string>> skanUpdatedDelegate = null;

        // callbacks as method parameters
        private static List<Action<int>> authorizationStatusDelegates = null;
        private static Action<AdjustPurchaseVerificationInfo> verificationInfoDelegate = null;
        private static Action<string> deeplinkResolutionDelegate = null;
        private static Action<string> skanErrorDelegate = null;
        private static Action<bool> getIsEnabledDelegate = null;
        private static Action<AdjustAttribution> getAttributionDelegate = null;
        private static Action<string> getAdidDelegate = null;
        private static Action<string> getIdfaDelegate = null;
        private static Action<string> getIdfvDelegate = null;
        private static Action<string> getSdkVersionDelegate = null;
        private static Action<string> getLastDeeplinkDelegate = null;
#endif

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);

            // TODO: double-check the state of Unity on deep linking nowadays
#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
            Application.deepLinkActivated += Adjust.ProcessDeeplink;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // cold start and Application.absoluteURL not null so process deep link
                Adjust.ProcessDeeplink(Application.absoluteURL);
            }
#endif

            if (!this.startManually)
            {
                AdjustConfig adjustConfig = new AdjustConfig(
                    this.appToken,
                    this.environment,
                    (this.logLevel == AdjustLogLevel.Suppress));
                adjustConfig.SetLogLevel(this.logLevel);
                if (this.sendInBackground == true)
                {
                    adjustConfig.EnableSendingInBackground();
                }
                if (this.launchDeferredDeeplink == false)
                {
                    adjustConfig.DisableDeferredDeeplinkOpening();
                }
                adjustConfig.SetDefaultTracker(this.defaultTracker);
                // TODO: URL strategy
                if (this.costDataInAttribution == true)
                {
                    adjustConfig.EnableCostDataInAttribution();
                }
                if (this.preinstallTracking == true)
                {
                    adjustConfig.EnablePreinstallTracking();
                }
                adjustConfig.SetPreinstallFilePath(this.preinstallFilePath);
                if (this.adServices == false)
                {
                    adjustConfig.DisableAdServices();
                }
                if (this.idfaReading == false)
                {
                    adjustConfig.DisableIdfaReading();
                }
                if (this.linkMe == true)
                {
                    adjustConfig.EnableLinkMe();
                }
                if (this.skanAttribution == false)
                {
                    adjustConfig.DisableSkanAttribution();
                }
                Adjust.InitSdk(adjustConfig);
            }
        }

        public static void InitSdk(AdjustConfig adjustConfig)
        {
            if (IsEditor())
            {
                return;
            }

            if (adjustConfig == null)
            {
                Debug.Log("[Adjust]: Missing config to start.");
                return;
            }

#if UNITY_IOS
            Adjust.eventSuccessDelegate = adjustConfig.GetEventSuccessDelegate();
            Adjust.eventFailureDelegate = adjustConfig.GetEventFailureDelegate();
            Adjust.sessionSuccessDelegate = adjustConfig.GetSessionSuccessDelegate();
            Adjust.sessionFailureDelegate = adjustConfig.GetSessionFailureDelegate();
            Adjust.deferredDeeplinkDelegate = adjustConfig.GetDeferredDeeplinkDelegate();
            Adjust.attributionChangedDelegate = adjustConfig.GetAttributionChangedDelegate();
            Adjust.skanUpdatedDelegate = adjustConfig.GetSkanUpdatedDelegate();
            AdjustiOS.InitSdk(adjustConfig);
#elif UNITY_ANDROID
            AdjustAndroid.InitSdk(adjustConfig);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.Start(adjustConfig);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            if (IsEditor())
            {
                return;
            }

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

        public static void Enable()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.Enable();
#elif UNITY_ANDROID
            AdjustAndroid.Enable();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetEnabled(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void Disable()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.Disable();
#elif UNITY_ANDROID
            AdjustAndroid.Disable();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetEnabled(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void EnableCoppaCompliance()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.EnableCoppaCompliance();
#elif UNITY_ANDROID
            AdjustAndroid.EnableCoppaCompliance();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DisableCoppaCompliance()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.DisableCoppaCompliance();
#elif UNITY_ANDROID
            AdjustAndroid.DisableCoppaCompliance();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void EnablePlayStoreKidsApp()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Marking apps as Play Store kids app is only supported for Android platform.");
#elif UNITY_ANDROID
            AdjustAndroid.EnablePlayStoreKidsApp();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DisablePlayStoreKidsApp()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Marking apps as Play Store kids app is only supported for Android platform.");
#elif UNITY_ANDROID
            AdjustAndroid.DisablePlayStoreKidsApp();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void IsEnabled(Action<bool> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getIsEnabledDelegate = callback;
            AdjustiOS.IsEnabled(gameObjectName);
#elif UNITY_ANDROID
            AdjustAndroid.IsEnabled(callback);
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.IsEnabled();
#else
            Debug.Log(errorMsgPlatform);
            return;
#endif
        }

        public static void SwitchToOfflineMode()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.SwitchToOfflineMode();
#elif UNITY_ANDROID
            AdjustAndroid.SwitchToOfflineMode();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetOfflineMode(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void SwitchBackToOnlineMode()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.SwitchBackToOnlineMode();
#elif UNITY_ANDROID
            AdjustAndroid.SwitchBackToOnlineMode();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetOfflineMode(enabled);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void SetPushToken(string pushToken)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.SetPushToken(pushToken);
#elif UNITY_ANDROID
            AdjustAndroid.SetPushToken(pushToken);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.SetDeviceToken(deviceToken);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GdprForgetMe()
        {
            if (IsEditor())
            {
                return;
            }

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

        public static void ProcessDeeplink(string deeplink)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.ProcessDeeplink(deeplink);
#elif UNITY_ANDROID
            AdjustAndroid.ProcessDeeplink(deeplink);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AppWillOpenUrl(url);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void AddGlobalPartnerParameter(string key, string value)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.AddGlobalPartnerParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.AddGlobalPartnerParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AddSessionPartnerParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void AddGlobalCallbackParameter(string key, string value)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.AddGlobalCallbackParameter(key, value);
#elif UNITY_ANDROID
            AdjustAndroid.AddGlobalCallbackParameter(key, value);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.AddSessionCallbackParameter(key, value);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RemoveGlobalPartnerParameter(string key)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.RemoveGlobalPartnerParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.RemoveGlobalPartnerParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.RemoveSessionPartnerParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RemoveGlobalCallbackParameter(string key)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.RemoveGlobalCallbackParameter(key);
#elif UNITY_ANDROID
            AdjustAndroid.RemoveGlobalCallbackParameter(key);
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.RemoveSessionCallbackParameter(key);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RemoveGlobalPartnerParameters()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.RemoveGlobalPartnerParameters();
#elif UNITY_ANDROID
            AdjustAndroid.RemoveGlobalPartnerParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.ResetSessionPartnerParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RemoveGlobalCallbackParameters()
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.RemoveGlobalCallbackParameters();
#elif UNITY_ANDROID
            AdjustAndroid.RemoveGlobalCallbackParameters();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.ResetSessionCallbackParameters();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void TrackAdRevenue(AdjustAdRevenue adRevenue)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.TrackAdRevenue(adRevenue);
#elif UNITY_ANDROID
            AdjustAndroid.TrackAdRevenue(adRevenue);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Ad revenue tracking is only supported for Android and iOS platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void TrackAppStoreSubscription(AdjustAppStoreSubscription subscription)
        {
            if (IsEditor())
            {
                return;
            }

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

        public static void TrackPlayStoreSubscription(AdjustPlayStoreSubscription subscription)
        {
            if (IsEditor())
            {
                return;
            }

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

        public static void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.TrackThirdPartySharing(thirdPartySharing);
#elif UNITY_ANDROID
            AdjustAndroid.TrackThirdPartySharing(thirdPartySharing);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Third party sharing tracking is only supported for iOS and Android platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void TrackMeasurementConsent(bool measurementConsent)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.TrackMeasurementConsent(measurementConsent);
#elif UNITY_ANDROID
            AdjustAndroid.TrackMeasurementConsent(measurementConsent);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Measurement consent tracking is only supported for iOS and Android platforms.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RequestAppTrackingAuthorizationWithCompletionHandler(
            Action<int> callback,
            string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            if (Adjust.authorizationStatusDelegates == null)
            {
                Adjust.authorizationStatusDelegates = new List<Action<int>>();
            }
            Adjust.authorizationStatusDelegates.Add(callback);
            AdjustiOS.RequestAppTrackingAuthorizationWithCompletionHandler(gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Requesting tracking authorization is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Requesting tracking authorization is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void UpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            bool lockWindow,
            Action<string> callback,
            string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.skanErrorDelegate = callback;
            AdjustiOS.UpdateSkanConversionValue(conversionValue, coarseValue, lockWindow, gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Updating SKAdNetwork conversion value is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Updating SKAdNetwork conversion value is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static int GetAppTrackingAuthorizationStatus()
        {
            if (IsEditor())
            {
                return -1;
            }

#if UNITY_IOS
            return AdjustiOS.GetAppTrackingAuthorizationStatus();
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! App tracking authorization status is only supported for iOS platform.");
            return -1;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! App tracking authorization status is only supported for iOS platform.");
            return -1;
#else
            Debug.Log(errorMsgPlatform);
            return -1;
#endif
        }

        public static void GetAdid(Action<string> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getIdfaDelegate = callback;
            AdjustiOS.GetAdid(gameObjectName);
#elif UNITY_ANDROID
            AdjustAndroid.GetAdid(callback);
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetAdid();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static void GetAttribution(
            Action<AdjustAttribution> callback,
            string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getAttributionDelegate = callback;
            AdjustiOS.GetAttribution(gameObjectName);
#elif UNITY_ANDROID
            AdjustAndroid.GetAttribution(callback);
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetAttribution();
#else
            Debug.Log(errorMsgPlatform);
            return null;
#endif
        }

//         public static string getWinAdid()
//         {
//             if (IsEditor())
//             {
//                 return string.Empty;
//             }

// #if UNITY_IOS
//             Debug.Log("[Adjust]: Error! Windows Advertising ID is not available on iOS platform.");
//             return string.Empty;
// #elif UNITY_ANDROID
//             Debug.Log("[Adjust]: Error! Windows Advertising ID is not available on Android platform.");
//             return string.Empty;
// #elif (UNITY_WSA || UNITY_WP8)
//             return AdjustWindows.GetWinAdId();
// #else
//             Debug.Log(errorMsgPlatform);
//             return string.Empty;
// #endif
//         }

        public static void GetIdfa(Action<string> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getIdfaDelegate = callback;
            AdjustiOS.GetIdfa(gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! IDFA is not available on Android platform.");
            return;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! IDFA is not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static void GetIdfv(Action<string> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getIdfvDelegate = callback;
            AdjustiOS.GetIdfv(gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! IDFV is not available on Android platform.");
            return;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! IDFV is not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static void GetSdkVersion(Action<string> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getSdkVersionDelegate = callback;
            AdjustiOS.GetSdkVersion(gameObjectName);
#elif UNITY_ANDROID
            AdjustAndroid.GetSdkVersion(callback);
#elif (UNITY_WSA || UNITY_WP8)
            return AdjustWindows.GetSdkVersion();
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static void GetLastDeeplink(Action<string> callback, string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.getLastDeeplinkDelegate = callback;
            AdjustiOS.GetLastDeeplink(gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! Last deeplink getter is not available on Android platform.");
            return;
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Error! Last deeplink getter is not available on Windows platform.");
            return string.Empty;
#else
            Debug.Log(errorMsgPlatform);
            return string.Empty;
#endif
        }

        public static void VerifyAppStorePurchase(
            AdjustAppStorePurchase purchase,
            Action<AdjustPurchaseVerificationInfo> verificationInfoDelegate,
            string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            if (purchase == null ||
                purchase.transactionId == null ||
                purchase.productId == null ||
                purchase.receipt == null)
            {
                Debug.Log("[Adjust]: Invalid App Store purchase parameters.");
                return;
            }

            Adjust.verificationInfoDelegate = verificationInfoDelegate;
            AdjustiOS.VerifyAppStorePurchase(purchase, gameObjectName);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: App Store purchase verification is only supported for iOS platform.");
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: App Store purchase verification is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void VerifyPlayStorePurchase(
            AdjustPlayStorePurchase purchase,
            Action<AdjustPurchaseVerificationInfo> verificationResultCallback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Play Store purchase verification is only supported for Android platform.");
#elif UNITY_ANDROID
            if (purchase == null ||
                purchase.productId == null ||
                purchase.purchaseToken == null)
            {
                Debug.Log("[Adjust]: Invalid Play Store purchase parameters.");
                return;
            }

            AdjustAndroid.VerifyPlayStorePurchase(purchase, verificationResultCallback);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Play Store purchase verification is only supported for Android platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void ProcessAndResolveDeeplink(
            string deeplink,
            Action<string> resolvedDeeplinkCallback,
            string gameObjectName = "Adjust")
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Adjust.deeplinkResolutionDelegate = resolvedDeeplinkCallback;
            AdjustiOS.ProcessAndResolveDeeplink(deeplink, gameObjectName);
#elif UNITY_ANDROID
            AdjustAndroid.ProcessAndResolveDeeplink(deeplink, resolvedDeeplinkCallback);
#elif (UNITY_WSA || UNITY_WP8)
            Debug.Log("[Adjust]: Deep link processing is only supported for Android and iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

#if UNITY_IOS
        public void UnityAdjustAttributionCallback(string attributionData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.attributionChangedDelegate == null)
            {
                Debug.Log("[Adjust]: Attribution changed delegate was not set.");
                return;
            }

            var attribution = new AdjustAttribution(attributionData);
            Adjust.attributionChangedDelegate(attribution);
        }

        public void UnityAdjustEventSuccessCallback(string eventSuccessData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.eventSuccessDelegate == null)
            {
                Debug.Log("[Adjust]: Event success delegate was not set.");
                return;
            }

            var eventSuccess = new AdjustEventSuccess(eventSuccessData);
            Adjust.eventSuccessDelegate(eventSuccess);
        }

        public void UnityAdjustEventFailureCallback(string eventFailureData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.eventFailureDelegate == null)
            {
                Debug.Log("[Adjust]: Event failure delegate was not set.");
                return;
            }

            var eventFailure = new AdjustEventFailure(eventFailureData);
            Adjust.eventFailureDelegate(eventFailure);
        }

        public void UnityAdjustSessionSuccessCallback(string sessionSuccessData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.sessionSuccessDelegate == null)
            {
                Debug.Log("[Adjust]: Session success delegate was not set.");
                return;
            }

            var sessionSuccess = new AdjustSessionSuccess(sessionSuccessData);
            Adjust.sessionSuccessDelegate(sessionSuccess);
        }

        public void UnityAdjustSessionFailureCallback(string sessionFailureData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.sessionFailureDelegate == null)
            {
                Debug.Log("[Adjust]: Session failure delegate was not set.");
                return;
            }

            var sessionFailure = new AdjustSessionFailure(sessionFailureData);
            Adjust.sessionFailureDelegate(sessionFailure);
        }

        public void UnityAdjustDeferredDeeplinkCallback(string deeplinkURL)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.deferredDeeplinkDelegate == null)
            {
                Debug.Log("[Adjust]: Deferred deeplink delegate was not set.");
                return;
            }

            Adjust.deferredDeeplinkDelegate(deeplinkURL);
        }

        public void UnityAdjustSkanUpdatedCallback(string skanUpdateData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.skanUpdatedDelegate == null)
            {
                Debug.Log("[Adjust]: SKAN update delegate was not set.");
                return;
            }

            Adjust.skanUpdatedDelegate(AdjustUtils.GetSkanUpdateDataDictionary(skanUpdateData));
        }

        public void UnityAdjustAttDialogCallback(string authorizationStatus)
        {
            if (IsEditor())
            {
                return;
            }

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

        public void UnityAdjustPurchaseVerificationCallback(string verificationInfoData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.verificationInfoDelegate == null)
            {
                Debug.Log("[Adjust]: Purchase verification info delegate was not set.");
                return;
            }

            var verificationInfo = new AdjustPurchaseVerificationInfo(verificationInfoData);
            Adjust.verificationInfoDelegate(verificationInfo);
        }

        public void UnityAdjustResolvedDeeplinkCallback(string resolvedLink)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.deeplinkResolutionDelegate == null)
            {
                Debug.Log("[Adjust]: Deep link reoslution delegate was not set.");
                return;
            }

            Adjust.deeplinkResolutionDelegate(resolvedLink);
        }

        public void UnityAdjustSkanErrorCallback(string skanError)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.skanErrorDelegate == null)
            {
                Debug.Log("[Adjust]: SKAN error delegate was not set.");
                return;
            }

            Adjust.skanErrorDelegate(skanError);
        }

        public void UnityAdjustIsEnabledGetter(string isEnabled)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getIsEnabledDelegate == null)
            {
                Debug.Log("[Adjust]: Is enabled delegate was not set.");
                return;
            }

            Adjust.getIsEnabledDelegate(bool.Parse(isEnabled));
        }

        public void UnityAdjustAttributionGetter(string attributionData)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getAttributionDelegate == null)
            {
                Debug.Log("[Adjust]: Attribution delegate was not set.");
                return;
            }

            var attribution = new AdjustAttribution(attributionData);
            Adjust.getAttributionDelegate(attribution);
        }

        public void UnityAdjustAdidGetter(string adid)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getAdidDelegate == null)
            {
                Debug.Log("[Adjust]: Adid delegate was not set.");
                return;
            }

            Adjust.getAdidDelegate(adid);
        }

        public void UnityAdjustIdfaGetter(string idfa)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getIdfaDelegate == null)
            {
                Debug.Log("[Adjust]: IDFA delegate was not set.");
                return;
            }

            Adjust.getIdfaDelegate(idfa);
        }

        public void UnityAdjustIdfvGetter(string idfv)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getIdfvDelegate == null)
            {
                Debug.Log("[Adjust]: IDFV delegate was not set.");
                return;
            }

            Adjust.getIdfvDelegate(idfv);
        }

        public void UnityAdjustSdkVersionGetter(string sdkVersion)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getSdkVersionDelegate == null)
            {
                Debug.Log("[Adjust]: SDK version delegate was not set.");
                return;
            }

            Adjust.getSdkVersionDelegate(sdkVersion);
        }

        public void UnityAdjustLastDeeplinkGetter(string lastDeeplink)
        {
            if (IsEditor())
            {
                return;
            }

            if (Adjust.getLastDeeplinkDelegate == null)
            {
                Debug.Log("[Adjust]: Last deep link delegate was not set.");
                return;
            }

            Adjust.getLastDeeplinkDelegate(lastDeeplink);
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
            if (IsEditor())
            {
                return;
            }

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
