using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AdjustSdk
{
#if UNITY_ANDROID
    public class AdjustAndroid
    {
        private const string sdkPrefix = "unity5.0.5";
        private static bool isDeferredDeeplinkOpeningEnabled = true;
        private static AndroidJavaClass ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        private static DeferredDeeplinkListener onDeferredDeeplinkListener;
        private static AttributionChangedListener onAttributionChangedListener;
        private static EventTrackingFailedListener onEventTrackingFailedListener;
        private static EventTrackingSucceededListener onEventTrackingSucceededListener;
        private static SessionTrackingFailedListener onSessionTrackingFailedListener;
        private static SessionTrackingSucceededListener onSessionTrackingSucceededListener;
        private static VerificationResultListener onVerificationResultListener;
        private static VerificationResultListener onVerifyAndTrackListener;
        private static DeeplinkResolutionListener onDeeplinkResolvedListener;

        public static void InitSdk(AdjustConfig adjustConfig)
        {
            // thank you, Unity 2019.2.0, for breaking this
            // AndroidJavaObject ajoEnvironment = adjustConfig.environment == AdjustEnvironment.Sandbox ? 
            //     new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX") :
            //         new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION");

            // get environment variable
            string ajoEnvironment = adjustConfig.Environment == AdjustEnvironment.Production ? "production" : "sandbox";
            
            // create config object
            AndroidJavaObject ajoAdjustConfig;

            // check if suppress log level is supported
            if (adjustConfig.AllowSuppressLogLevel != null)
            {
                ajoAdjustConfig = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.AppToken, ajoEnvironment, adjustConfig.AllowSuppressLogLevel);
            }
            else
            {
                ajoAdjustConfig = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.AppToken, ajoEnvironment);
            }

            // check if deferred deeplink should be launched by the SDK
            if (adjustConfig.IsDeferredDeeplinkOpeningEnabled != null)
            {
                isDeferredDeeplinkOpeningEnabled = (bool)adjustConfig.IsDeferredDeeplinkOpeningEnabled;
            }

            // check log level
            if (adjustConfig.LogLevel != null)
            {
                AndroidJavaObject ajoLogLevel;
                if (adjustConfig.LogLevel.Value.ToUppercaseString().Equals("SUPPRESS"))
                {
                    ajoLogLevel = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPPRESS");
                }
                else
                {
                    ajoLogLevel = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.LogLevel.Value.ToUppercaseString());
                }

                if (ajoLogLevel != null)
                {
                    ajoAdjustConfig.Call("setLogLevel", ajoLogLevel);
                }
            }

            // set Unity SDK prefix
            ajoAdjustConfig.Call("setSdkPrefix", sdkPrefix);

            // check read device IDs only once
            if (adjustConfig.IsDeviceIdsReadingOnceEnabled != null)
            {
                if (adjustConfig.IsDeviceIdsReadingOnceEnabled == true)
                {
                    ajoAdjustConfig.Call("enableDeviceIdsReadingOnce");
                }
            }

            // check if COPPA compliance is enabled
            if (adjustConfig.IsCoppaComplianceEnabled != null)
            {
                if (adjustConfig.IsCoppaComplianceEnabled == true)
                {
                    ajoAdjustConfig.Call("enableCoppaCompliance");
                }
            }

            // check if Play Store Kids compliance is enabled
            if (adjustConfig.IsPlayStoreKidsComplianceEnabled != null)
            {
                if (adjustConfig.IsPlayStoreKidsComplianceEnabled == true)
                {
                    ajoAdjustConfig.Call("enablePlayStoreKidsCompliance");
                }
            }

            // check if user enabled sening in the background
            if (adjustConfig.IsSendingInBackgroundEnabled != null)
            {
                if (adjustConfig.IsSendingInBackgroundEnabled == true)
                {
                    ajoAdjustConfig.Call("enableSendingInBackground");
                }
            }

            // check if user wants to get cost data in attribution callback
            if (adjustConfig.IsCostDataInAttributionEnabled != null)
            {
                if (adjustConfig.IsCostDataInAttributionEnabled == true)
                {
                    ajoAdjustConfig.Call("enableCostDataInAttribution");
                }
            }

            // check if user wants to run preinstall campaigns
            if (adjustConfig.IsPreinstallTrackingEnabled != null)
            {
                if (adjustConfig.IsPreinstallTrackingEnabled == true)
                {
                    ajoAdjustConfig.Call("enablePreinstallTracking");
                }
            }

            // check if user has set custom preinstall file path
            if (adjustConfig.PreinstallFilePath != null)
            {
                ajoAdjustConfig.Call("setPreinstallFilePath", adjustConfig.PreinstallFilePath);
            }

            // check if FB app ID has been set
            if (adjustConfig.FbAppId != null)
            {
                ajoAdjustConfig.Call("setFbAppId", adjustConfig.FbAppId);
            }

            // check if user has set default tracker token
            if (adjustConfig.DefaultTracker != null)
            {
                ajoAdjustConfig.Call("setDefaultTracker", adjustConfig.DefaultTracker);
            }

            // check if user has set external device identifier
            if (adjustConfig.ExternalDeviceId != null)
            {
                ajoAdjustConfig.Call("setExternalDeviceId", adjustConfig.ExternalDeviceId);
            }

            // check if user has set max number of event deduplication IDs
            if (adjustConfig.EventDeduplicationIdsMaxSize != null)
            {
                using (AndroidJavaObject ajoEventDeduplicationIdsMaxSize = new AndroidJavaObject("java.lang.Integer", adjustConfig.EventDeduplicationIdsMaxSize))
                {
                    ajoAdjustConfig.Call("setEventDeduplicationIdsMaxSize", ajoEventDeduplicationIdsMaxSize);
                }
            }

            // check if user has set custom URL strategy
            if (adjustConfig.UrlStrategyDomains != null &&
                adjustConfig.ShouldUseSubdomains != null &&
                adjustConfig.IsDataResidency != null)
            {
                using (var ajoUrlStrategyDomains = new AndroidJavaObject("java.util.ArrayList"))
                {
                    foreach (string domain in adjustConfig.UrlStrategyDomains)
                    {
                        ajoUrlStrategyDomains.Call<bool>("add", domain);
                    }
                    ajoAdjustConfig.Call("setUrlStrategy",
                        ajoUrlStrategyDomains,
                        adjustConfig.ShouldUseSubdomains,
                        adjustConfig.IsDataResidency);
                    }
            }

            // check attribution changed delagate
            if (adjustConfig.AttributionChangedDelegate != null)
            {
                onAttributionChangedListener = new AttributionChangedListener(adjustConfig.AttributionChangedDelegate);
                ajoAdjustConfig.Call("setOnAttributionChangedListener", onAttributionChangedListener);
            }

            // check event success delegate
            if (adjustConfig.EventSuccessDelegate != null)
            {
                onEventTrackingSucceededListener = new EventTrackingSucceededListener(adjustConfig.EventSuccessDelegate);
                ajoAdjustConfig.Call("setOnEventTrackingSucceededListener", onEventTrackingSucceededListener);
            }

            // check event failure delagate
            if (adjustConfig.EventFailureDelegate != null)
            {
                onEventTrackingFailedListener = new EventTrackingFailedListener(adjustConfig.EventFailureDelegate);
                ajoAdjustConfig.Call("setOnEventTrackingFailedListener", onEventTrackingFailedListener);
            }

            // check session success delegate
            if (adjustConfig.SessionSuccessDelegate != null)
            {
                onSessionTrackingSucceededListener = new SessionTrackingSucceededListener(adjustConfig.SessionSuccessDelegate);
                ajoAdjustConfig.Call("setOnSessionTrackingSucceededListener", onSessionTrackingSucceededListener);
            }

            // check session failure delegate
            if (adjustConfig.SessionFailureDelegate != null)
            {
                onSessionTrackingFailedListener = new SessionTrackingFailedListener(adjustConfig.SessionFailureDelegate);
                ajoAdjustConfig.Call("setOnSessionTrackingFailedListener", onSessionTrackingFailedListener);
            }

            // check deferred deeplink delegate
            if (adjustConfig.DeferredDeeplinkDelegate != null)
            {
                onDeferredDeeplinkListener = new DeferredDeeplinkListener(adjustConfig.DeferredDeeplinkDelegate);
                ajoAdjustConfig.Call("setOnDeferredDeeplinkResponseListener", onDeferredDeeplinkListener);
            }

            // initialise and start the SDK
            ajcAdjust.CallStatic("initSdk", ajoAdjustConfig);
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            using (AndroidJavaObject ajoAdjustEvent =
                new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.EventToken))
            {
                // check if user has set revenue for the event
                if (adjustEvent.Revenue != null)
                {
                    ajoAdjustEvent.Call("setRevenue", (double)adjustEvent.Revenue, adjustEvent.Currency);
                }

                // check if user has added any callback parameters to the event
                if (adjustEvent.CallbackParameters != null)
                {
                    for (int i = 0; i < adjustEvent.CallbackParameters.Count; i += 2)
                    {
                        string key = adjustEvent.CallbackParameters[i];
                        string value = adjustEvent.CallbackParameters[i + 1];
                        ajoAdjustEvent.Call("addCallbackParameter", key, value);
                    }
                }

                // check if user has added any partner parameters to the event
                if (adjustEvent.PartnerParameters != null)
                {
                    for (int i = 0; i < adjustEvent.PartnerParameters.Count; i += 2)
                    {
                        string key = adjustEvent.PartnerParameters[i];
                        string value = adjustEvent.PartnerParameters[i + 1];
                        ajoAdjustEvent.Call("addPartnerParameter", key, value);
                    }
                }

                // check if user has set deduplication ID for the event
                if (adjustEvent.DeduplicationId != null)
                {
                    ajoAdjustEvent.Call("setDeduplicationId", adjustEvent.DeduplicationId);
                }

                // check if user has added callback ID to the event
                if (adjustEvent.CallbackId != null)
                {
                    ajoAdjustEvent.Call("setCallbackId", adjustEvent.CallbackId);
                }

                // check if user has added product ID to the event
                if (adjustEvent.ProductId != null)
                {
                    ajoAdjustEvent.Call("setProductId", adjustEvent.ProductId);
                }

                // check if user has added purchase token to the event
                if (adjustEvent.PurchaseToken != null)
                {
                    ajoAdjustEvent.Call("setPurchaseToken", adjustEvent.PurchaseToken);
                }

                // track the event
                ajcAdjust.CallStatic("trackEvent", ajoAdjustEvent);
            }
        }

        public static void Enable()
        {
            ajcAdjust.CallStatic("enable");
        }

        public static void Disable()
        {
            ajcAdjust.CallStatic("disable");
        }

        public static void SwitchToOfflineMode()
        {
            ajcAdjust.CallStatic("switchToOfflineMode");
        }

        public static void SwitchBackToOnlineMode()
        {
            ajcAdjust.CallStatic("switchBackToOnlineMode");
        }

        public static void EnableCoppaCompliance()
        {
            ajcAdjust.CallStatic("enableCoppaCompliance", ajoCurrentActivity);
        }

        public static void DisableCoppaCompliance()
        {
            ajcAdjust.CallStatic("disableCoppaCompliance", ajoCurrentActivity);
        }

        public static void EnablePlayStoreKidsApp()
        {
            ajcAdjust.CallStatic("enablePlayStoreKidsApp", ajoCurrentActivity);
        }

        public static void DisablePlayStoreKidsApp()
        {
            ajcAdjust.CallStatic("disablePlayStoreKidsApp", ajoCurrentActivity);
        }

        public static void SetPushToken(string pushToken)
        {
            ajcAdjust.CallStatic("setPushToken", pushToken, ajoCurrentActivity);
        }

        public static void GdprForgetMe()
        {
            ajcAdjust.CallStatic("gdprForgetMe", ajoCurrentActivity);
        }

        public static void AddGlobalPartnerParameter(string key, string value)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("addGlobalPartnerParameter", key, value);
        }

        public static void AddGlobalCallbackParameter(string key, string value)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("addGlobalCallbackParameter", key, value);
        }

        public static void RemoveGlobalPartnerParameter(string key)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeGlobalPartnerParameter", key);
        }

        public static void RemoveGlobalCallbackParameter(string key)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeGlobalCallbackParameter", key);
        }

        public static void RemoveGlobalPartnerParameters()
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeGlobalPartnerParameters");
        }

        public static void RemoveGlobalCallbackParameters()
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeGlobalCallbackParameters");
        }

        public static void ProcessDeeplink(AdjustDeeplink deeplink) 
        {
            using (AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri"))
            using (AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", deeplink.Deeplink))
            using (AndroidJavaObject ajoAdjustDeeplink = new AndroidJavaObject("com.adjust.sdk.AdjustDeeplink", ajoUri))
            {
                ajcAdjust.CallStatic("processDeeplink", ajoAdjustDeeplink, ajoCurrentActivity);
            }
        }

        public static void TrackAdRevenue(AdjustAdRevenue adRevenue)
        {
            using (AndroidJavaObject ajoAdjustAdRevenue =
                new AndroidJavaObject("com.adjust.sdk.AdjustAdRevenue", adRevenue.Source))
            {
                // check if user has set revenue
                if (adRevenue.Revenue != null)
                {
                    using (AndroidJavaObject ajoRevenue = new AndroidJavaObject("java.lang.Double", adRevenue.Revenue))
                    {
                        ajoAdjustAdRevenue.Call("setRevenue", ajoRevenue, adRevenue.Currency);
                    }
                }

                // check if user has set ad impressions count
                if (adRevenue.AdImpressionsCount != null)
                {
                    using (AndroidJavaObject ajoAdImpressionsCount =
                        new AndroidJavaObject("java.lang.Integer", adRevenue.AdImpressionsCount))
                    {
                        ajoAdjustAdRevenue.Call("setAdImpressionsCount", ajoAdImpressionsCount);
                    }
                }

                // check if user has set ad revenue network
                if (adRevenue.AdRevenueNetwork != null)
                {
                    ajoAdjustAdRevenue.Call("setAdRevenueNetwork", adRevenue.AdRevenueNetwork);
                }

                // check if user has set ad revenue unit
                if (adRevenue.AdRevenueUnit != null)
                {
                    ajoAdjustAdRevenue.Call("setAdRevenueUnit", adRevenue.AdRevenueUnit);
                }

                // check if user has set ad revenue placement
                if (adRevenue.AdRevenuePlacement != null)
                {
                    ajoAdjustAdRevenue.Call("setAdRevenuePlacement", adRevenue.AdRevenuePlacement);
                }

                // check if user has added any callback parameters
                if (adRevenue.CallbackParameters != null)
                {
                    for (int i = 0; i < adRevenue.CallbackParameters.Count; i += 2)
                    {
                        string key = adRevenue.CallbackParameters[i];
                        string value = adRevenue.CallbackParameters[i + 1];
                        ajoAdjustAdRevenue.Call("addCallbackParameter", key, value);
                    }
                }

                // check if user has added any partner parameters
                if (adRevenue.PartnerParameters != null)
                {
                    for (int i = 0; i < adRevenue.PartnerParameters.Count; i += 2)
                    {
                        string key = adRevenue.PartnerParameters[i];
                        string value = adRevenue.PartnerParameters[i + 1];
                        ajoAdjustAdRevenue.Call("addPartnerParameter", key, value);
                    }
                }

                // track ad revenue
                ajcAdjust.CallStatic("trackAdRevenue", ajoAdjustAdRevenue);
            }
        }

        public static void TrackPlayStoreSubscription(AdjustPlayStoreSubscription subscription)
        {
            using (AndroidJavaObject ajoSubscription = new AndroidJavaObject(
                "com.adjust.sdk.AdjustPlayStoreSubscription",
                Convert.ToInt64(subscription.Price),
                subscription.Currency,
                subscription.ProductId,
                subscription.OrderId,
                subscription.Signature,
                subscription.PurchaseToken))
            {
                // check if user has set purchase time for subscription
                if (subscription.PurchaseTime != null)
                {
                    ajoSubscription.Call("setPurchaseTime", Convert.ToInt64(subscription.PurchaseTime));
                }

                // check if user has added any callback parameters to the subscription
                if (subscription.CallbackParameters != null)
                {
                    for (int i = 0; i < subscription.CallbackParameters.Count; i += 2)
                    {
                        string key = subscription.CallbackParameters[i];
                        string value = subscription.CallbackParameters[i + 1];
                        ajoSubscription.Call("addCallbackParameter", key, value);
                    }
                }

                // check if user has added any partner parameters to the subscription
                if (subscription.PartnerParameters != null)
                {
                    for (int i = 0; i < subscription.PartnerParameters.Count; i += 2)
                    {
                        string key = subscription.PartnerParameters[i];
                        string value = subscription.PartnerParameters[i + 1];
                        ajoSubscription.Call("addPartnerParameter", key, value);
                    }
                }

                // track the subscription
                ajcAdjust.CallStatic("trackPlayStoreSubscription", ajoSubscription);
            }
        }

        public static void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing)
        {
            AndroidJavaObject ajoIsEnabled;
            AndroidJavaObject ajoAdjustThirdPartySharing;
            if (thirdPartySharing.IsEnabled != null)
            {
                ajoIsEnabled = new AndroidJavaObject("java.lang.Boolean", thirdPartySharing.IsEnabled.Value);
                ajoAdjustThirdPartySharing = new AndroidJavaObject("com.adjust.sdk.AdjustThirdPartySharing", ajoIsEnabled);
            }
            else
            {
                string[] parameters = null;
                ajoAdjustThirdPartySharing = new AndroidJavaObject("com.adjust.sdk.AdjustThirdPartySharing", parameters);
            }

            if (thirdPartySharing.GranularOptions != null)
            {
                for (int i = 0; i < thirdPartySharing.GranularOptions.Count;)
                {
                    string partnerName = thirdPartySharing.GranularOptions[i++];
                    string key = thirdPartySharing.GranularOptions[i++];
                    string value = thirdPartySharing.GranularOptions[i++];
                    ajoAdjustThirdPartySharing.Call("addGranularOption", partnerName, key, value);
                }
            }

            if (thirdPartySharing.PartnerSharingSettings != null)
            {
                for (int i = 0; i < thirdPartySharing.PartnerSharingSettings.Count;)
                {
                    string partnerName = thirdPartySharing.PartnerSharingSettings[i++];
                    string key = thirdPartySharing.PartnerSharingSettings[i++];
                    string value = thirdPartySharing.PartnerSharingSettings[i++];
                    ajoAdjustThirdPartySharing.Call("addPartnerSharingSetting", partnerName, key, bool.Parse(value));
                }
            }

            ajcAdjust.CallStatic("trackThirdPartySharing", ajoAdjustThirdPartySharing);
        }

        public static void TrackMeasurementConsent(bool measurementConsent)
        {
            ajcAdjust.CallStatic("trackMeasurementConsent", measurementConsent);
        }

        public static void IsEnabled(Action<bool> onIsEnabled)
        {
            IsEnabledListener isEnabledProxy = new IsEnabledListener(onIsEnabled);
            ajcAdjust.CallStatic("isEnabled", ajoCurrentActivity, isEnabledProxy);
        }

        public static void GetAdid(Action<string> onAdidRead) 
        {
            AdidReadListener onAdidReadProxy = new AdidReadListener(onAdidRead);
            ajcAdjust.CallStatic("getAdid", onAdidReadProxy);
        }

        public static void GetAttribution(Action<AdjustAttribution> onAttributionRead) 
        {
            AttributionReadListener onAttributionReadProxy = new AttributionReadListener(onAttributionRead);
            ajcAdjust.CallStatic("getAttribution", onAttributionReadProxy);
        }

        // android specific methods
        public static void GetGoogleAdId(Action<string> onDeviceIdsRead) 
        {
            GoogleAdIdReadListener onDeviceIdsReadProxy = new GoogleAdIdReadListener(onDeviceIdsRead);
            ajcAdjust.CallStatic("getGoogleAdId", ajoCurrentActivity, onDeviceIdsReadProxy);
        }

        public static void GetAmazonAdId(Action<string> onAmazonAdIdRead) 
        {
            AmazonAdIdReadListener onAmazonAdIdReadProxy = new AmazonAdIdReadListener(onAmazonAdIdRead);
            ajcAdjust.CallStatic("getAmazonAdId", ajoCurrentActivity, onAmazonAdIdReadProxy);
        }

        public static void GetSdkVersion(Action<string> onSdkVersionRead) 
        {
            SdkVersionReadListener onSdkVersionReadProxy = new SdkVersionReadListener(onSdkVersionRead, sdkPrefix);
            ajcAdjust.CallStatic("getSdkVersion", onSdkVersionReadProxy);
        }

        public static void GetLastDeeplink(Action<string> onLastDeeplinkRead) 
        {
            LastDeeplinkListener onLastDeeplinkReadProxy = new LastDeeplinkListener(onLastDeeplinkRead);
            ajcAdjust.CallStatic("getLastDeeplink", ajoCurrentActivity, onLastDeeplinkReadProxy);
        }

        public static void VerifyPlayStorePurchase(
            AdjustPlayStorePurchase purchase,
            Action<AdjustPurchaseVerificationResult> verificationInfoCallback)
        {
            onVerificationResultListener = new VerificationResultListener(verificationInfoCallback);
            using (AndroidJavaObject ajoPurchase = new AndroidJavaObject("com.adjust.sdk.AdjustPlayStorePurchase",
                purchase.ProductId,
                purchase.PurchaseToken))
            {
                ajcAdjust.CallStatic("verifyPlayStorePurchase", ajoPurchase, onVerificationResultListener);
            }
        }

        public static void ProcessAndResolveDeeplink(AdjustDeeplink deeplink, Action<string> resolvedLinkCallback)
        {
            onDeeplinkResolvedListener = new DeeplinkResolutionListener(resolvedLinkCallback);
            using (AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri"))
            using (AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", deeplink.Deeplink))
            using (AndroidJavaObject ajoAdjustDeeplink = new AndroidJavaObject("com.adjust.sdk.AdjustDeeplink", ajoUri))
            {
                ajcAdjust.CallStatic(
                    "processAndResolveDeeplink",
                    ajoAdjustDeeplink,
                    ajoCurrentActivity,
                    onDeeplinkResolvedListener);
            }
        }

        public static void VerifyAndTrackPlayStorePurchase(
            AdjustEvent adjustEvent,
            Action<AdjustPurchaseVerificationResult> verificationInfoCallback)
        {
            onVerifyAndTrackListener = new VerificationResultListener(verificationInfoCallback);
            using (AndroidJavaObject ajoAdjustEvent =
                new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.EventToken))
            {
                // check if user has set revenue for the event
                if (adjustEvent.Revenue != null)
                {
                    ajoAdjustEvent.Call("setRevenue", (double)adjustEvent.Revenue, adjustEvent.Currency);
                }

                // check if user has added any callback parameters to the event
                if (adjustEvent.CallbackParameters != null)
                {
                    for (int i = 0; i < adjustEvent.CallbackParameters.Count; i += 2)
                    {
                        string key = adjustEvent.CallbackParameters[i];
                        string value = adjustEvent.CallbackParameters[i + 1];
                        ajoAdjustEvent.Call("addCallbackParameter", key, value);
                    }
                }

                // check if user has added any partner parameters to the event
                if (adjustEvent.PartnerParameters != null)
                {
                    for (int i = 0; i < adjustEvent.PartnerParameters.Count; i += 2)
                    {
                        string key = adjustEvent.PartnerParameters[i];
                        string value = adjustEvent.PartnerParameters[i + 1];
                        ajoAdjustEvent.Call("addPartnerParameter", key, value);
                    }
                }

                // check if user has set deduplication ID for the event
                if (adjustEvent.DeduplicationId != null)
                {
                    ajoAdjustEvent.Call("setDeduplicationId", adjustEvent.DeduplicationId);
                }

                // check if user has added callback ID to the event
                if (adjustEvent.CallbackId != null)
                {
                    ajoAdjustEvent.Call("setCallbackId", adjustEvent.CallbackId);
                }

                // check if user has added product ID to the event
                if (adjustEvent.ProductId != null)
                {
                    ajoAdjustEvent.Call("setProductId", adjustEvent.ProductId);
                }

                // check if user has added purchase token to the event
                if (adjustEvent.PurchaseToken != null)
                {
                    ajoAdjustEvent.Call("setPurchaseToken", adjustEvent.PurchaseToken);
                }

                ajcAdjust.CallStatic("verifyAndTrackPlayStorePurchase", ajoAdjustEvent, onVerifyAndTrackListener);
            }
        }

        // used for testing only
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            AndroidJavaObject ajoTestOptions = AdjustUtils.TestOptionsMap2AndroidJavaObject(testOptions, ajoCurrentActivity);
            ajcAdjust.CallStatic("setTestOptions", ajoTestOptions);
        }

        public static void OnResume(string testingArgument = null)
        {
            if (testingArgument == "test")
            {
                ajcAdjust.CallStatic("onResume");
            }
        }

        public static void OnPause(string testingArgument = null)
        {
            if (testingArgument == "test")
            {
                ajcAdjust.CallStatic("onPause");
            }
        }

        // private & helper classes
        private class AttributionChangedListener : AndroidJavaProxy
        {
            private Action<AdjustAttribution> callback;

            public AttributionChangedListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onAttributionChanged(AdjustAttribution attribution);
            public void onAttributionChanged(AndroidJavaObject ajoAttribution)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (ajoAttribution == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                string trackerName = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.TrackerName = trackerName == "" ? null : trackerName;
                string trackerToken = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.TrackerToken = trackerToken == "" ? null : trackerToken;
                string network = ajoAttribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.Network = network == "" ? null : network;
                string campaign = ajoAttribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.Campaign = campaign == "" ? null : campaign;
                string adgroup = ajoAttribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.Adgroup = adgroup == "" ? null : adgroup;
                string creative = ajoAttribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.Creative = creative == "" ? null : creative;
                string clickLabel = ajoAttribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.ClickLabel = clickLabel == "" ? null : clickLabel;
                string costType = ajoAttribution.Get<string>(AdjustUtils.KeyCostType);
                adjustAttribution.CostType = costType == "" ? null : costType;
                using (AndroidJavaObject ajoCostAmount = ajoAttribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount))
                {
                    if (ajoCostAmount == null)
                    {
                        adjustAttribution.CostAmount = null;
                    }
                    else
                    {
                        double costAmount = ajoCostAmount.Call<double>("doubleValue");
                        adjustAttribution.CostAmount = costAmount;
                    }
                }
                string costCurrency = ajoAttribution.Get<string>(AdjustUtils.KeyCostCurrency);
                adjustAttribution.CostCurrency = costCurrency == "" ? null : costCurrency;
                string fbInstallReferrer = ajoAttribution.Get<string>(AdjustUtils.KeyFbInstallReferrer);
                adjustAttribution.FbInstallReferrer = fbInstallReferrer == "" ? null : fbInstallReferrer;

                this.callback(adjustAttribution);
            }
        }

        private class DeferredDeeplinkListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeferredDeeplinkResponseListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // boolean launchReceivedDeeplink(Uri deeplink);
            public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
            {
                if (this.callback == null)
                {
                    return isDeferredDeeplinkOpeningEnabled;
                }

                string strDeeplink = deeplink.Call<string>("toString");
                callback(strDeeplink);
                return isDeferredDeeplinkOpeningEnabled;
            }
        }

        private class EventTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdjustEventSuccess> callback;

            public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback) : base("com.adjust.sdk.OnEventTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onEventTrackingSucceeded(AdjustEventSuccess eventSuccessResponseData);
            public void onEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (eventSuccessData == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
                string adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventSuccess.Adid = adid == "" ? null : adid;
                string message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventSuccess.Message = message == "" ? null : message;
                string timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventSuccess.Timestamp = timestamp == "" ? null : timestamp;
                string eventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
                adjustEventSuccess.EventToken = eventToken == "" ? null : eventToken;
                string callbackId = eventSuccessData.Get<string>(AdjustUtils.KeyCallbackId);
                adjustEventSuccess.CallbackId = callbackId == "" ? null : callbackId;
                try
                {
                    using (AndroidJavaObject ajoJsonResponse = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse))
                    {
                        string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                        adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
                    }
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                this.callback(adjustEventSuccess);
            }
        }

        private class EventTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdjustEventFailure> callback;

            public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback) : base("com.adjust.sdk.OnEventTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onEventTrackingFailed(AdjustEventFailure eventFailureResponseData);
            public void onEventTrackingFailed(AndroidJavaObject eventFailureData)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (eventFailureData == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
                string adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventFailure.Adid = adid == "" ? null : adid;
                string message = eventFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventFailure.Message = message == "" ? null : message;
                adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                string timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventFailure.Timestamp =  timestamp == "" ? null : timestamp;
                string eventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
                adjustEventFailure.EventToken =  eventToken == "" ? null : eventToken;
                string callbackId = eventFailureData.Get<string>(AdjustUtils.KeyCallbackId);
                adjustEventFailure.CallbackId = callbackId == "" ? null : callbackId;
                try
                {
                    using (AndroidJavaObject ajoJsonResponse = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse))
                    {
                        string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                        adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
                    }
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                this.callback(adjustEventFailure);
            }
        }

        private class SessionTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdjustSessionSuccess> callback;

            public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback) : base("com.adjust.sdk.OnSessionTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onSessionTrackingSucceeded(AdjustSessionSuccess sessionSuccessResponseData);
            public void onSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (sessionSuccessData == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
                string adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionSuccess.Adid = adid == "" ? null : adid;
                string message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionSuccess.Message = message == "" ? null : message;
                string timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustSessionSuccess.Timestamp = timestamp == "" ? null : timestamp;
                try
                {
                    using (AndroidJavaObject ajoJsonResponse = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse))
                    {
                        string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                        adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
                    }
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                this.callback(adjustSessionSuccess);
            }
        }

        private class SessionTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdjustSessionFailure> callback;

            public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback) : base("com.adjust.sdk.OnSessionTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onSessionTrackingFailed(AdjustSessionFailure failureResponseData);
            public void onSessionTrackingFailed(AndroidJavaObject sessionFailureData)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (sessionFailureData == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
                string adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionFailure.Adid = adid == "" ? null : adid;
                string message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionFailure.Message = message == "" ? null : message;
                adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                string timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustSessionFailure.Timestamp = timestamp == "" ? null : timestamp;
                try
                {
                    using (AndroidJavaObject ajoJsonResponse = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse))
                    {
                        string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                        adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
                    }
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                this.callback(adjustSessionFailure);
            }
        }

        private class GoogleAdIdReadListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public GoogleAdIdReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnGoogleAdIdReadListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onGoogleAdIdRead(String googleAdId);
            public void onGoogleAdIdRead(string adid)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(adid);
            }
        }

        private class VerificationResultListener : AndroidJavaProxy
        {
            private Action<AdjustPurchaseVerificationResult> callback;

            public VerificationResultListener(Action<AdjustPurchaseVerificationResult> pCallback) : base("com.adjust.sdk.OnPurchaseVerificationFinishedListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onVerificationFinished(AdjustPurchaseVerificationResult result);
            public void onVerificationFinished(AndroidJavaObject ajoVerificationInfo)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (ajoVerificationInfo == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustPurchaseVerificationResult purchaseVerificationResult = new AdjustPurchaseVerificationResult();
                purchaseVerificationResult.VerificationStatus = ajoVerificationInfo.Get<string>(AdjustUtils.KeyVerificationStatus);
                purchaseVerificationResult.Code = ajoVerificationInfo.Get<int>(AdjustUtils.KeyCode);
                purchaseVerificationResult.Message = ajoVerificationInfo.Get<string>(AdjustUtils.KeyMessage);

                this.callback(purchaseVerificationResult);
            }
        }

        private class DeeplinkResolutionListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeeplinkResolutionListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResolvedListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onDeeplinkResolved(String resolvedLink);
            public void onDeeplinkResolved(string resolvedLink)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(resolvedLink);
            }
        }

        private class AdidReadListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public AdidReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnAdidReadListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onAdidRead(String adid);
            public void onAdidRead(string adid)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(adid);
            }
        }

        private class AttributionReadListener : AndroidJavaProxy
        {
            private Action<AdjustAttribution> callback;

            public AttributionReadListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionReadListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onAttributionRead(AdjustAttribution attribution);
            public void onAttributionRead(AndroidJavaObject ajoAttribution)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (ajoAttribution == null)
                {
                    this.callback(null);
                    return;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                string trackerName = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.TrackerName = trackerName == "" ? null : trackerName;
                string trackerToken = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.TrackerToken = trackerToken == "" ? null : trackerToken;
                string network = ajoAttribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.Network = network == "" ? null : network;
                string campaign = ajoAttribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.Campaign = campaign == "" ? null : campaign;
                string adgroup = ajoAttribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.Adgroup = adgroup == "" ? null : adgroup;
                string creative = ajoAttribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.Creative = creative == "" ? null : creative;
                string clickLabel = ajoAttribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.ClickLabel = clickLabel == "" ? null : clickLabel;
                string costType = ajoAttribution.Get<string>(AdjustUtils.KeyCostType);
                adjustAttribution.CostType = costType == "" ? null : costType;
                using (AndroidJavaObject ajoCostAmount = ajoAttribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount))
                {
                    if (ajoCostAmount == null)
                    {
                        adjustAttribution.CostAmount = null;
                    }
                    else
                    {
                        double costAmount = ajoCostAmount.Call<double>("doubleValue");
                        adjustAttribution.CostAmount = costAmount;
                    }
                }
                string costCurrency = ajoAttribution.Get<string>(AdjustUtils.KeyCostCurrency);
                adjustAttribution.CostCurrency = costCurrency == "" ? null : costCurrency;
                string fbInstallReferrer = ajoAttribution.Get<string>(AdjustUtils.KeyFbInstallReferrer);
                adjustAttribution.FbInstallReferrer = fbInstallReferrer == "" ? null : fbInstallReferrer;

                this.callback(adjustAttribution);
            }
        }

        private class AmazonAdIdReadListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public AmazonAdIdReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnAmazonAdIdReadListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onAmazonAdIdRead(String amazonAdId);
            public void onAmazonAdIdRead(string amazonAdId)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(amazonAdId);
            }
        }

        private class SdkVersionReadListener : AndroidJavaProxy
        {
            private Action<string> callback;
            private string sdkPrefix;

            public SdkVersionReadListener(Action<string> pCallback, string sdkPrefix) : base("com.adjust.sdk.OnSdkVersionReadListener")
            {
                this.callback = pCallback;
                this.sdkPrefix = sdkPrefix;
            }

            // native method:
            // void onSdkVersionRead(String sdkVersion);
            public void onSdkVersionRead(string sdkVersion)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(this.sdkPrefix + "@" + sdkVersion);
            }
        }

        private class IsEnabledListener : AndroidJavaProxy
        {
            private Action<bool> callback;

            public IsEnabledListener(Action<bool> pCallback) : base("com.adjust.sdk.OnIsEnabledListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onIsEnabledRead(boolean isEnabled);
            public void onIsEnabledRead(bool isEnabled)
            {
                if (this.callback == null)
                {
                    return;
                }

                this.callback(isEnabled);
            }
        }

        private class LastDeeplinkListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public LastDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnLastDeeplinkReadListener")
            {
                this.callback = pCallback;
            }

            // native method:
            // void onLastDeeplinkRead(Uri deeplink);
            public void onLastDeeplinkRead(AndroidJavaObject ajoLastDeeplink)
            {
                if (this.callback == null)
                {
                    return;
                }

                if (ajoLastDeeplink == null)
                {
                    this.callback(null);
                }
                else
                {
                    this.callback(ajoLastDeeplink.Call<string>("toString"));
                }
            }
        }
    }
#endif
}
