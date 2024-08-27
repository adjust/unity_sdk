using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdjustSdk
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
        public bool coppaCompliance = false;
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

        void Awake()
        {
            if (IsEditor())
            {
                return;
            }

            DontDestroyOnLoad(transform.gameObject);

            // TODO: double-check the state of Unity on deep linking nowadays
#if UNITY_ANDROID && UNITY_2019_2_OR_NEWER
            Application.deepLinkActivated += (deeplink) =>
            {
                Adjust.ProcessDeeplink(new AdjustDeeplink(deeplink));
            };
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // cold start and Application.absoluteURL not null so process deep link
                Adjust.ProcessDeeplink(new AdjustDeeplink(Application.absoluteURL));
            }
#endif

            if (!this.startManually)
            {
                AdjustConfig adjustConfig = new AdjustConfig(
                    this.appToken,
                    this.environment,
                    (this.logLevel == AdjustLogLevel.Suppress));
                adjustConfig.LogLevel = this.logLevel;
                adjustConfig.IsSendingInBackgroundEnabled = this.sendInBackground;
                adjustConfig.IsDeferredDeeplinkOpeningEnabled = this.launchDeferredDeeplink;
                adjustConfig.DefaultTracker = this.defaultTracker;
                // TODO: URL strategy
                adjustConfig.IsCoppaComplianceEnabled = this.coppaCompliance;
                adjustConfig.IsCostDataInAttributionEnabled = this.costDataInAttribution;
                adjustConfig.IsPreinstallTrackingEnabled = this.preinstallTracking;
                adjustConfig.PreinstallFilePath = this.preinstallFilePath;
                adjustConfig.IsAdServicesEnabled = this.adServices;
                adjustConfig.IsIdfaReadingEnabled = this.idfaReading;
                adjustConfig.IsLinkMeEnabled = this.linkMe;
                adjustConfig.IsSkanAttributionEnabled = this.skanAttribution;
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
            AdjustiOS.InitSdk(adjustConfig);
#elif UNITY_ANDROID
            AdjustAndroid.InitSdk(adjustConfig);
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

        public static void IsEnabled(Action<bool> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.IsEnabled(callback);
#elif UNITY_ANDROID
            AdjustAndroid.IsEnabled(callback);
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
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void ProcessDeeplink(AdjustDeeplink deeplink)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.ProcessDeeplink(deeplink);
#elif UNITY_ANDROID
            AdjustAndroid.ProcessDeeplink(deeplink);
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
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void RequestAppTrackingAuthorization(Action<int> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.RequestAppTrackingAuthorization(callback);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Requesting tracking authorization is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void UpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            bool lockWindow,
            Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.UpdateSkanConversionValue(conversionValue, coarseValue, lockWindow, callback);
#elif UNITY_ANDROID
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
#else
            Debug.Log(errorMsgPlatform);
            return -1;
#endif
        }

        public static void GetAdid(Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetAdid(callback);
#elif UNITY_ANDROID
            AdjustAndroid.GetAdid(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetAttribution(Action<AdjustAttribution> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetAttribution(callback);
#elif UNITY_ANDROID
            AdjustAndroid.GetAttribution(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetIdfa(Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetIdfa(callback);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! IDFA is not available on Android platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetIdfv(Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetIdfv(callback);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: Error! IDFV is not available on Android platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetGoogleAdId(Action<string> callback) 
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Error! Google Advertising ID is not available on iOS platform.");
#elif UNITY_ANDROID
            AdjustAndroid.GetGoogleAdId(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetAmazonAdId(Action<string> callback) 
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Error! Amazon Fire Advertising ID is not available on iOS platform.");
#elif UNITY_ANDROID
            AdjustAndroid.GetAmazonAdId(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetSdkVersion(Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetSdkVersion(callback);
#elif UNITY_ANDROID
            AdjustAndroid.GetSdkVersion(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void GetLastDeeplink(Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.GetLastDeeplink(callback);
#elif UNITY_ANDROID
            AdjustAndroid.GetLastDeeplink(callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void VerifyAppStorePurchase(
            AdjustAppStorePurchase purchase,
            Action<AdjustPurchaseVerificationResult> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.VerifyAppStorePurchase(purchase, callback);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: App Store purchase verification is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void VerifyPlayStorePurchase(
            AdjustPlayStorePurchase purchase,
            Action<AdjustPurchaseVerificationResult> verificationResultCallback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Play Store purchase verification is only supported for Android platform.");
#elif UNITY_ANDROID
            AdjustAndroid.VerifyPlayStorePurchase(purchase, verificationResultCallback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void ProcessAndResolveDeeplink(AdjustDeeplink deeplink, Action<string> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.ProcessAndResolveDeeplink(deeplink, callback);
#elif UNITY_ANDROID
            AdjustAndroid.ProcessAndResolveDeeplink(deeplink, callback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void VerifyAndTrackAppStorePurchase(
            AdjustEvent adjustEvent,
            Action<AdjustPurchaseVerificationResult> callback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            AdjustiOS.VerifyAndTrackAppStorePurchase(adjustEvent, callback);
#elif UNITY_ANDROID
            Debug.Log("[Adjust]: App Store purchase verification is only supported for iOS platform.");
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void VerifyAndTrackPlayStorePurchase(
            AdjustEvent adjustEvent,
            Action<AdjustPurchaseVerificationResult> verificationResultCallback)
        {
            if (IsEditor())
            {
                return;
            }

#if UNITY_IOS
            Debug.Log("[Adjust]: Play Store purchase verification is only supported for Android platform.");
#elif UNITY_ANDROID
            AdjustAndroid.VerifyAndTrackPlayStorePurchase(adjustEvent, verificationResultCallback);
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

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
#else
            Debug.Log("[Adjust]: Cannot run integration tests. None of the supported platforms selected.");
#endif
        }
    }
}
