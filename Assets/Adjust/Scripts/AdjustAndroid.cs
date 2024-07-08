using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AdjustSdk
{
#if UNITY_ANDROID
    public class AdjustAndroid
    {
        private const string sdkPrefix = "unity5.0.0";
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
                    ajoLogLevel = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
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
                AndroidJavaObject ajoEventDeduplicationIdsMaxSize = new AndroidJavaObject("java.lang.Integer", adjustConfig.EventDeduplicationIdsMaxSize);
                ajoAdjustConfig.Call("setEventDeduplicationIdsMaxSize", ajoEventDeduplicationIdsMaxSize);
            }

            // check if user has set custom URL strategy
            if (adjustConfig.UrlStrategyDomains != null &&
                adjustConfig.ShouldUseSubdomains != null &&
                adjustConfig.IsDataResidency != null)
            {
                var ajoUrlStrategyDomains = new AndroidJavaObject("java.util.ArrayList");
                foreach (string domain in adjustConfig.UrlStrategyDomains)
                {
                    ajoUrlStrategyDomains.Call("add", domain);
                }
                ajoAdjustConfig.Call("setUrlStrategy",
                    ajoUrlStrategyDomains,
                    adjustConfig.ShouldUseSubdomains,
                    adjustConfig.IsDataResidency);
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
            AndroidJavaObject ajoAdjustEvent = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.EventToken);

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

            // Track the event.
            ajcAdjust.CallStatic("trackEvent", ajoAdjustEvent);
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

        public static void ProcessDeeplink(string url) 
        {
            AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", url);
            ajcAdjust.CallStatic("processDeeplink", ajoUri, ajoCurrentActivity);
        }

        public static void TrackAdRevenue(AdjustAdRevenue adRevenue)
        {
            AndroidJavaObject ajoAdjustAdRevenue = new AndroidJavaObject("com.adjust.sdk.AdjustAdRevenue", adRevenue.Source);

            // check if user has set revenue
            if (adRevenue.Revenue != null)
            {
                AndroidJavaObject ajoRevenue = new AndroidJavaObject("java.lang.Double", adRevenue.Revenue);
                ajoAdjustAdRevenue.Call("setRevenue", ajoRevenue, adRevenue.Currency);
            }

            // check if user has set ad impressions count
            if (adRevenue.AdImpressionsCount != null)
            {
                AndroidJavaObject ajoAdImpressionsCount = new AndroidJavaObject("java.lang.Integer", adRevenue.AdImpressionsCount);
                ajoAdjustAdRevenue.Call("setAdImpressionsCount", ajoAdImpressionsCount);
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

            // track ad revenue.
            ajcAdjust.CallStatic("trackAdRevenue", ajoAdjustAdRevenue);
        }

        public static void TrackPlayStoreSubscription(AdjustPlayStoreSubscription subscription)
        {
            AndroidJavaObject ajoSubscription = new AndroidJavaObject("com.adjust.sdk.AdjustPlayStoreSubscription",
                Convert.ToInt64(subscription.Price),
                subscription.Currency,
                subscription.ProductId,
                subscription.OrderId,
                subscription.Signature,
                subscription.PurchaseToken);

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
            DeviceIdsReadListener onDeviceIdsReadProxy = new DeviceIdsReadListener(onDeviceIdsRead);
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

        public static void VerifyPlayStorePurchase(
            AdjustPlayStorePurchase purchase,
            Action<AdjustPurchaseVerificationResult> verificationInfoCallback)
        {
            AndroidJavaObject ajoPurchase = new AndroidJavaObject("com.adjust.sdk.AdjustPlayStorePurchase",
                purchase.ProductId,
                purchase.PurchaseToken);
            onVerificationResultListener = new VerificationResultListener(verificationInfoCallback);

            ajcAdjust.CallStatic("verifyPlayStorePurchase", ajoPurchase, onVerificationResultListener);
        }

        public static void ProcessAndResolveDeeplink(string url, Action<string> resolvedLinkCallback)
        {
            onDeeplinkResolvedListener = new DeeplinkResolutionListener(resolvedLinkCallback);
            AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", url);
            ajcAdjust.CallStatic("processAndResolveDeeplink", ajoUri, ajoCurrentActivity, onDeeplinkResolvedListener);
        }

        public static void VerifyAndTrackPlayStorePurchase(
            AdjustEvent adjustEvent,
            Action<AdjustPurchaseVerificationResult> verificationInfoCallback)
        {
            AndroidJavaObject ajoAdjustEvent = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.EventToken);

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

            onVerifyAndTrackListener = new VerificationResultListener(verificationInfoCallback);

            ajcAdjust.CallStatic("verifyAndTrackPlayStorePurchase", ajoAdjustEvent, onVerifyAndTrackListener);
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
        // TODO: add AndroidJavaObject null instance handling to each proxy
        private class AttributionChangedListener : AndroidJavaProxy
        {
            private Action<AdjustAttribution> callback;

            public AttributionChangedListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
            {
                this.callback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onAttributionChanged(AndroidJavaObject attribution)
            {
                if (callback == null)
                {
                    return;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                adjustAttribution.TrackerName = attribution.Get<string>(AdjustUtils.KeyTrackerName) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.TrackerToken = attribution.Get<string>(AdjustUtils.KeyTrackerToken) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.Network = attribution.Get<string>(AdjustUtils.KeyNetwork) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.Campaign = attribution.Get<string>(AdjustUtils.KeyCampaign) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.Adgroup = attribution.Get<string>(AdjustUtils.KeyAdgroup) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.Creative = attribution.Get<string>(AdjustUtils.KeyCreative) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.ClickLabel = attribution.Get<string>(AdjustUtils.KeyClickLabel) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.CostType = attribution.Get<string>(AdjustUtils.KeyCostType) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyCostType);
                AndroidJavaObject ajoCostAmount = attribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount) == null ?
                    null : attribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount);
                if (ajoCostAmount == null)
                {
                    adjustAttribution.CostAmount = null;
                }
                else
                {
                    double costAmount = ajoCostAmount.Call<double>("doubleValue");
                    adjustAttribution.CostAmount = costAmount;
                }
                adjustAttribution.CostCurrency = attribution.Get<string>(AdjustUtils.KeyCostCurrency) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyCostCurrency);
                adjustAttribution.FbInstallReferrer = attribution.Get<string>(AdjustUtils.KeyFbInstallReferrer) == "" ?
                    null : attribution.Get<string>(AdjustUtils.KeyFbInstallReferrer);
                callback(adjustAttribution);
            }
        }

        private class DeferredDeeplinkListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeferredDeeplinkResponseListener")
            {
                this.callback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
            {
                if (callback == null)
                {
                    return isDeferredDeeplinkOpeningEnabled;
                }

                string deeplinkURL = deeplink.Call<string>("toString");
                callback(deeplinkURL);
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

            // method must be lowercase to match Android method signature
            public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
            {
                if (callback == null)
                {
                    return;
                }
                if (eventSuccessData == null)
                {
                    return;
                }

                AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
                adjustEventSuccess.Adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid) == "" ?
                    null : eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventSuccess.Message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage) == "" ?
                    null : eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventSuccess.Timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp) == "" ?
                    null : eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventSuccess.EventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken) == "" ?
                    null : eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
                adjustEventSuccess.CallbackId = eventSuccessData.Get<string>(AdjustUtils.KeyCallbackId) == "" ?
                    null : eventSuccessData.Get<string>(AdjustUtils.KeyCallbackId);

                try
                {
                    AndroidJavaObject ajoJsonResponse = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adjustEventSuccess);
            }
        }

        private class EventTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdjustEventFailure> callback;

            public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback) : base("com.adjust.sdk.OnEventTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
            {
                if (callback == null)
                {
                    return;
                }
                if (eventFailureData == null)
                {
                    return;
                }

                AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
                adjustEventFailure.Adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid) == "" ?
                    null : eventFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventFailure.Message = eventFailureData.Get<string>(AdjustUtils.KeyMessage) == "" ?
                    null : eventFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                adjustEventFailure.Timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp) == "" ?
                    null : eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventFailure.EventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken) == "" ?
                    null : eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
                adjustEventFailure.CallbackId = eventFailureData.Get<string>(AdjustUtils.KeyCallbackId) == "" ?
                    null : eventFailureData.Get<string>(AdjustUtils.KeyCallbackId);

                try
                {
                    AndroidJavaObject ajoJsonResponse = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }
                
                callback(adjustEventFailure);
            }
        }

        private class SessionTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdjustSessionSuccess> callback;

            public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback) : base("com.adjust.sdk.OnSessionTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
            {
                if (callback == null)
                {
                    return;
                }
                if (sessionSuccessData == null)
                {
                    return;
                }

                AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
                adjustSessionSuccess.Adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid) == "" ?
                    null : sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionSuccess.Message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage) == "" ?
                    null : sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp) == "" ?
                    null : sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);

                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adjustSessionSuccess);
            }
        }

        private class SessionTrackingFailedListener : AndroidJavaProxy
        {
            private Action<AdjustSessionFailure> callback;

            public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback) : base("com.adjust.sdk.OnSessionTrackingFailedListener")
            {
                this.callback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
            {
                if (callback == null)
                {
                    return;
                }
                if (sessionFailureData == null)
                {
                    return;
                }

                AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
                adjustSessionFailure.Adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid) == "" ?
                    null : sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionFailure.Message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage) == "" ?
                    null : sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                adjustSessionFailure.Timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp) == "" ?
                    null : sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);

                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Native Android SDK should send empty JSON object if none available as of v4.12.5.
                    // Native Android SDK added special logic to send Unity friendly values as of v4.15.0.
                }

                callback(adjustSessionFailure);
            }
        }

        private class DeviceIdsReadListener : AndroidJavaProxy
        {
            private Action<string> onPlayAdIdReadCallback;

            public DeviceIdsReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeviceIdsRead")
            {
                this.onPlayAdIdReadCallback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onGoogleAdIdRead(string playAdId)
            {
                if (onPlayAdIdReadCallback == null)
                {
                    return;
                }

                this.onPlayAdIdReadCallback(playAdId);
            }

            // handling of null object
            public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
            {
                if (ajoAdId == null)
                {
                    string adId = null;
                    this.onGoogleAdIdRead(adId);
                    return;
                }

                this.onGoogleAdIdRead(ajoAdId.Call<string>("toString"));
            }
        }

        private class VerificationResultListener : AndroidJavaProxy
        {
            private Action<AdjustPurchaseVerificationResult> callback;

            public VerificationResultListener(Action<AdjustPurchaseVerificationResult> pCallback) : base("com.adjust.sdk.OnPurchaseVerificationFinishedListener")
            {
                this.callback = pCallback;
            }

            public void onVerificationFinished(AndroidJavaObject verificationInfo)
            {
                AdjustPurchaseVerificationResult purchaseVerificationResult = new AdjustPurchaseVerificationResult();
                // verification status
                purchaseVerificationResult.VerificationStatus = verificationInfo.Get<string>(AdjustUtils.KeyVerificationStatus);
                // status code
                purchaseVerificationResult.Code = verificationInfo.Get<int>(AdjustUtils.KeyCode);
                // message
                purchaseVerificationResult.Message = verificationInfo.Get<string>(AdjustUtils.KeyMessage);

                if (callback != null)
                {
                    callback(purchaseVerificationResult);
                }
            }
        }

        private class DeeplinkResolutionListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeeplinkResolutionListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResolvedListener")
            {
                this.callback = pCallback;
            }

            public void onDeeplinkResolved(string resolvedLink)
            {
                if (callback != null)
                {
                    callback(resolvedLink);
                }
            }
        }

        private class AdidReadListener : AndroidJavaProxy
        {
            private Action<string> onAdidReadCallback;

            public AdidReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnAdidReadListener")
            {
                this.onAdidReadCallback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onAdidRead(string adid)
            {
                if (onAdidReadCallback == null)
                {
                    return;
                }

                this.onAdidReadCallback(adid);
            }

            // handling of null object
            public void onAdidRead(AndroidJavaObject ajoAdid)
            {
                if (ajoAdid == null)
                {
                    string adid = null;
                    this.onAdidRead(adid);
                    return;
                }

                this.onAdidRead(ajoAdid.Call<string>("toString"));
            }
        }

        private class AttributionReadListener : AndroidJavaProxy
        {
            private Action<AdjustAttribution> onAttributionReadCallback;

            public AttributionReadListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionReadListener")
            {
                this.onAttributionReadCallback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onAttributionRead(AndroidJavaObject ajoAttribution)
            {
                if (this.onAttributionReadCallback == null)
                {
                    return;
                }

                if (ajoAttribution == null)
                {
                    this.onAttributionReadCallback(null);
                    return;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                adjustAttribution.TrackerName = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerName) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.TrackerToken = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerToken) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.Network = ajoAttribution.Get<string>(AdjustUtils.KeyNetwork) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.Campaign = ajoAttribution.Get<string>(AdjustUtils.KeyCampaign) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.Adgroup = ajoAttribution.Get<string>(AdjustUtils.KeyAdgroup) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.Creative = ajoAttribution.Get<string>(AdjustUtils.KeyCreative) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.ClickLabel = ajoAttribution.Get<string>(AdjustUtils.KeyClickLabel) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.CostType = ajoAttribution.Get<string>(AdjustUtils.KeyCostType) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyCostType);
                AndroidJavaObject ajoCostAmount = ajoAttribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount) == null ?
                    null : ajoAttribution.Get<AndroidJavaObject>(AdjustUtils.KeyCostAmount);
                if (ajoCostAmount == null)
                {
                    adjustAttribution.CostAmount = null;
                }
                else
                {
                    double costAmount = ajoCostAmount.Call<double>("doubleValue");
                    adjustAttribution.CostAmount = costAmount;
                }
                adjustAttribution.CostCurrency = ajoAttribution.Get<string>(AdjustUtils.KeyCostCurrency) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyCostCurrency);
                adjustAttribution.FbInstallReferrer = ajoAttribution.Get<string>(AdjustUtils.KeyFbInstallReferrer) == "" ?
                    null : ajoAttribution.Get<string>(AdjustUtils.KeyFbInstallReferrer);

                this.onAttributionReadCallback(adjustAttribution);
            }
        }

        private class AmazonAdIdReadListener : AndroidJavaProxy
        {
            private Action<string> onAmazonAdIdReadCallback;

            public AmazonAdIdReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnAmazonAdIdReadListener")
            {
                this.onAmazonAdIdReadCallback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onAmazonAdIdRead(string amazonAdId)
            {
                if (this.onAmazonAdIdReadCallback == null)
                {
                    return;
                }

                this.onAmazonAdIdReadCallback(amazonAdId);
            }

            // handling of null object
            public void onAmazonAdIdRead(AndroidJavaObject ajoAmazonAdId)
            {
                if (ajoAmazonAdId == null)
                {
                    string amazonAdId = null;
                    this.onAmazonAdIdRead(amazonAdId);
                    return;
                }

                this.onAmazonAdIdRead(ajoAmazonAdId.Call<string>("toString"));
            }
        }

        private class SdkVersionReadListener : AndroidJavaProxy
        {
            private Action<string> onSdkVersionReadCallback;
            private string sdkPrefix;

            public SdkVersionReadListener(Action<string> pCallback, string sdkPrefix) : base("com.adjust.sdk.OnSdkVersionReadListener")
            {
                this.onSdkVersionReadCallback = pCallback;
                this.sdkPrefix = sdkPrefix;
            }

            // method must be lowercase to match Android method signature
            public void onSdkVersionRead(string sdkVersion)
            {
                if (this.onSdkVersionReadCallback == null)
                {
                    return;
                }

                this.onSdkVersionReadCallback(this.sdkPrefix + "@" + sdkVersion);
            }

            // handling of null object
            public void onSdkVersionRead(AndroidJavaObject ajoSdkVersion)
            {
                if (ajoSdkVersion == null)
                {
                    string sdkVersion = null;
                    this.onSdkVersionRead(sdkVersion);
                    return;
                }

                this.onSdkVersionRead(ajoSdkVersion.Call<string>("toString"));
            }
        }

        private class IsEnabledListener : AndroidJavaProxy
        {
            private Action<bool> onIsEnabledCallback;

            public IsEnabledListener(Action<bool> pCallback) : base("com.adjust.sdk.OnIsEnabledListener")
            {
                this.onIsEnabledCallback = pCallback;
            }

            // method must be lowercase to match Android method signature
            public void onIsEnabledRead(bool isEnabled)
            {
                if (this.onIsEnabledCallback == null)
                {
                    return;
                }

                this.onIsEnabledCallback(isEnabled);
            }

            // not handling null for primitive data type
        }
    }
#endif
}
