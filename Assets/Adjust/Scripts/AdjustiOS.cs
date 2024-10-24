using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AdjustSdk
{
#if UNITY_IOS
    public class AdjustiOS
    {
        private const string sdkPrefix = "unity5.0.5";

        // app callbacks as method parameters
        private static List<Action<bool>> appIsEnabledGetterCallbacks;
        private static List<Action<AdjustAttribution>> appAttributionGetterCallbacks;
        private static List<Action<string>> appAdidGetterCallbacks;
        private static List<Action<string>> appIdfaGetterCallbacks;
        private static List<Action<string>> appIdfvGetterCallbacks;
        private static List<Action<string>> appLastDeeplinkGetterCallbacks;
        private static List<Action<string>> appSdkVersionGetterCallbacks;
        private static List<Action<int>> appAttCallbacks;
        private static Action<AdjustPurchaseVerificationResult> appPurchaseVerificationCallback;
        private static Action<AdjustPurchaseVerificationResult> appVerifyAndTrackCallback;
        private static Action<string> appResolvedDeeplinkCallback;
        private static Action<string> appSkanErrorCallback;

        // app callbacks as subscriptions
        private static Action<AdjustAttribution> appAttributionCallback;
        private static Action<AdjustSessionSuccess> appSessionSuccessCallback;
        private static Action<AdjustSessionFailure> appSessionFailureCallback;
        private static Action<AdjustEventSuccess> appEventSuccessCallback;
        private static Action<AdjustEventFailure> appEventFailureCallback;
        private static Action<string> appDeferredDeeplinkCallback;
        private static Action<Dictionary<string, string>> appSkanUpdatedCallback;

        // extenral C methods
        private delegate void AdjustDelegateAttributionCallback(string attribution);
        private delegate void AdjustDelegateSessionSuccessCallback(string sessionSuccess);
        private delegate void AdjustDelegateSessionFailureCallback(string sessionFailure);
        private delegate void AdjustDelegateEventSuccessCallback(string eventSuccess);
        private delegate void AdjustDelegateEventFailureCallback(string eventFailure);
        private delegate void AdjustDelegateDeferredDeeplinkCallback(string callback);
        private delegate void AdjustDelegateSkanUpdatedCallback(string callback);
        [DllImport("__Internal")]
        private static extern void _AdjustInitSdk(
            string appToken,
            string environment,
            string sdkPrefix,
            string defaultTracker,
            string externalDeviceId,
            string jsonUrlStrategyDomains,
            int allowSuppressLogLevel,
            int logLevel,
            int attConsentWaitingInterval,
            int eventDeduplicationIdsMaxSize,
            int shouldUseSubdomains,
            int isCoppaComplianceEnabled,
            int isDataResidency,
            int isSendingInBackgroundEnabled,
            int isAdServicesEnabled,
            int isIdfaReadingEnabled,
            int isSkanAttributionEnabled,
            int isLinkMeEnabled,
            int isCostDataInAttributionEnabled,
            int isDeviceIdsReadingOnceEnabled,
            int isDeferredDeeplinkOpeningEnabled,
            AdjustDelegateAttributionCallback attributionCallback,
            AdjustDelegateEventSuccessCallback eventSuccessCallback,
            AdjustDelegateEventFailureCallback eventFailureCallback,
            AdjustDelegateSessionSuccessCallback sessionSuccessCallback,
            AdjustDelegateSessionFailureCallback sessionFailureCallback,
            AdjustDelegateDeferredDeeplinkCallback deferredDeeplinkCallback,
            AdjustDelegateSkanUpdatedCallback skanUpdatedCallback);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackEvent(
            string eventToken,
            double revenue,
            string currency,
            string productId,
            string transactionId,
            string callbackId,
            string deduplicationId,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustEnable();

        [DllImport("__Internal")]
        private static extern void _AdjustDisable();

        [DllImport("__Internal")]
        private static extern void _AdjustSwitchToOfflineMode();

        [DllImport("__Internal")]
        private static extern void _AdjustSwitchBackToOnlineMode();

        [DllImport("__Internal")]
        private static extern void _AdjustSetPushToken(string pushToken);

        [DllImport("__Internal")]
        private static extern void _AdjustProcessDeeplink(string deeplink);

        private delegate void AdjustDelegateResolvedDeeplinkCallback(string deeplink);
        [DllImport("__Internal")]
        private static extern void _AdjustProcessAndResolveDeeplink(
            string deeplink,
            AdjustDelegateResolvedDeeplinkCallback callback);

        private delegate void AdjustDelegateIsEnabledGetter(bool isEnabed);
        [DllImport("__Internal")]
        private static extern void _AdjustIsEnabled(AdjustDelegateIsEnabledGetter callback);

        private delegate void AdjustDelegateAttributionGetter(string attribution);
        [DllImport("__Internal")]
        private static extern void _AdjustGetAttribution(AdjustDelegateAttributionGetter callback);

        private delegate void AdjustDelegateAdidGetter(string adid);
        [DllImport("__Internal")]
        private static extern void _AdjustGetAdid(AdjustDelegateAdidGetter callback);

        private delegate void AdjustDelegateIdfaGetter(string idfa);
        [DllImport("__Internal")]
        private static extern void _AdjustGetIdfa(AdjustDelegateIdfaGetter callback);

        private delegate void AdjustDelegateIdfvGetter(string idfv);
        [DllImport("__Internal")]
        private static extern void _AdjustGetIdfv(AdjustDelegateIdfvGetter callback);

        private delegate void AdjustDelegateLastDeeplinkGetter(string lastDeeplink);
        [DllImport("__Internal")]
        private static extern void _AdjustGetLastDeeplink(AdjustDelegateLastDeeplinkGetter callback);

        private delegate void AdjustDelegateSdkVersionGetter(string sdkVersion);
        [DllImport("__Internal")]
        private static extern void _AdjustGetSdkVersion(AdjustDelegateSdkVersionGetter callback);

        [DllImport("__Internal")]
        private static extern void _AdjustGdprForgetMe();

        [DllImport("__Internal")]
        private static extern void _AdjustAddGlobalPartnerParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdjustAddGlobalCallbackParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveGlobalPartnerParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveGlobalCallbackParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveGlobalPartnerParameters();

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveGlobalCallbackParameters();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackAdRevenue(
            string source,
            double revenue,
            string currency,
            int adImpressionsCount,
            string adRevenueNetwork,
            string adRevenueUnit,
            string adRevenuePlacement,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackAppStoreSubscription(
            string price,
            string currency,
            string transactionId,
            string transactionDate,
            string salesRegion,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackThirdPartySharing(
            int enabled,
            string jsonGranularOptions,
            string jsonPartnerSharingSettings);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackMeasurementConsent(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdjustSetTestOptions(
            string overwriteUrl,
            string extraPath,
            long timerIntervalInMilliseconds,
            long timerStartInMilliseconds,
            long sessionIntervalInMilliseconds,
            long subsessionIntervalInMilliseconds,
            int teardown,
            int deleteState,
            int noBackoffWait,
            int adServicesFrameworkEnabled,
            int attStatus,
            string idfa);

        private delegate void AdjustDelegateAttCallback(int status);
        [DllImport("__Internal")]
        private static extern void _AdjustRequestAppTrackingAuthorization(AdjustDelegateAttCallback callback);

        private delegate void AdjustDelegateSkanErrorCallback(string error);
        [DllImport("__Internal")]
        private static extern void _AdjustUpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            int lockWindow,
            AdjustDelegateSkanErrorCallback callback);

        [DllImport("__Internal")]
        private static extern int _AdjustGetAppTrackingAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionStart();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionEnd();

        private delegate void AdjustDelegatePurchaseVerificationCallback(string verificationResult);
        [DllImport("__Internal")]
        private static extern void _AdjustVerifyAppStorePurchase(
            string transactionId,
            string productId,
            AdjustDelegatePurchaseVerificationCallback callback);

        private delegate void AdjustDelegateVerifyAndTrackCallback(string verificationResult);
        [DllImport("__Internal")]
        private static extern void _AdjustVerifyAndTrackAppStorePurchase(
            string eventToken,
            double revenue,
            string currency,
            string productId,
            string transactionId,
            string callbackId,
            string deduplicationId,
            string jsonCallbackParameters,
            string jsonPartnerParameters,
            AdjustDelegatePurchaseVerificationCallback callback);

        // public API
        public AdjustiOS() {}

        public static void InitSdk(AdjustConfig adjustConfig)
        {
            string appToken = adjustConfig.AppToken != null ? adjustConfig.AppToken : "ADJ_INVALID";
            string environment = adjustConfig.Environment.ToLowercaseString();
            string defaultTracker = adjustConfig.DefaultTracker != null ? adjustConfig.DefaultTracker : "ADJ_INVALID";
            string externalDeviceId = adjustConfig.ExternalDeviceId != null ? adjustConfig.ExternalDeviceId : "ADJ_INVALID";
            string stringJsonUrlStrategyDomains = AdjustUtils.ConvertReadOnlyCollectionToJson(
                adjustConfig.UrlStrategyDomains != null ? adjustConfig.UrlStrategyDomains.AsReadOnly() : null);
            int attConsentWaitingInterval = AdjustUtils.ConvertInt(adjustConfig.AttConsentWaitingInterval);
            int eventDeduplicationIdsMaxSize = AdjustUtils.ConvertInt(adjustConfig.EventDeduplicationIdsMaxSize);
            int logLevel = AdjustUtils.ConvertLogLevel(adjustConfig.LogLevel);
            int isCoppaComplianceEnabled = AdjustUtils.ConvertBool(adjustConfig.IsCoppaComplianceEnabled);
            int isSendingInBackgroundEnabled = AdjustUtils.ConvertBool(adjustConfig.IsSendingInBackgroundEnabled);
            int isAdServicesEnabled = AdjustUtils.ConvertBool(adjustConfig.IsAdServicesEnabled);
            int isIdfaReadingEnabled = AdjustUtils.ConvertBool(adjustConfig.IsIdfaReadingEnabled);
            int allowSuppressLogLevel = AdjustUtils.ConvertBool(adjustConfig.AllowSuppressLogLevel);
            int isDeferredDeeplinkOpeningEnabled = AdjustUtils.ConvertBool(adjustConfig.IsDeferredDeeplinkOpeningEnabled);
            int isSkanAttributionEnabled = AdjustUtils.ConvertBool(adjustConfig.IsSkanAttributionEnabled);
            int isLinkMeEnabled = AdjustUtils.ConvertBool(adjustConfig.IsLinkMeEnabled);
            int isCostDataInAttributionEnabled = AdjustUtils.ConvertBool(adjustConfig.IsCostDataInAttributionEnabled);
            int isDeviceIdsReadingOnceEnabled = AdjustUtils.ConvertBool(adjustConfig.IsDeviceIdsReadingOnceEnabled);
            int shouldUseSubdomains = AdjustUtils.ConvertBool(adjustConfig.ShouldUseSubdomains);
            int isDataResidency = AdjustUtils.ConvertBool(adjustConfig.IsDataResidency);
            appAttributionCallback = adjustConfig.AttributionChangedDelegate;
            appEventSuccessCallback = adjustConfig.EventSuccessDelegate;
            appEventFailureCallback = adjustConfig.EventFailureDelegate;
            appSessionSuccessCallback = adjustConfig.SessionSuccessDelegate;
            appSessionFailureCallback = adjustConfig.SessionFailureDelegate;
            appDeferredDeeplinkCallback = adjustConfig.DeferredDeeplinkDelegate;
            appSkanUpdatedCallback = adjustConfig.SkanUpdatedDelegate;

            _AdjustInitSdk(
                appToken,
                environment,
                sdkPrefix,
                defaultTracker,
                externalDeviceId,
                stringJsonUrlStrategyDomains,
                allowSuppressLogLevel,
                logLevel,
                attConsentWaitingInterval,
                eventDeduplicationIdsMaxSize,
                shouldUseSubdomains,
                isCoppaComplianceEnabled,
                isDataResidency,
                isSendingInBackgroundEnabled,
                isAdServicesEnabled,
                isIdfaReadingEnabled,
                isSkanAttributionEnabled,
                isLinkMeEnabled,
                isCostDataInAttributionEnabled,
                isDeviceIdsReadingOnceEnabled,
                isDeferredDeeplinkOpeningEnabled,
                AttributionCallbackMonoPInvoke,
                EventSuccessCallbackMonoPInvoke,
                EventFailureCallbackMonoPInvoke,
                SessionSuccessCallbackMonoPInvoke,
                SessionFailureCallbackMonoPInvoke,
                DeferredDeeplinkCallbackMonoPInvoke,
                SkanUpdatedCallbackMonoPInvoke);
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            double revenue = AdjustUtils.ConvertDouble(adjustEvent.Revenue);
            string eventToken = adjustEvent.EventToken;
            string currency = adjustEvent.Currency;
            string productId = adjustEvent.ProductId;
            string transactionId = adjustEvent.TransactionId;
            string callbackId = adjustEvent.CallbackId;
            string deduplicationId = adjustEvent.DeduplicationId;
            string stringJsonCallbackParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adjustEvent.CallbackParameters);
            string stringJsonPartnerParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adjustEvent.PartnerParameters);

            _AdjustTrackEvent(
                eventToken,
                revenue,
                currency,
                productId,
                transactionId,
                callbackId,
                deduplicationId,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void Enable()
        {
            _AdjustEnable();
        }

        public static void Disable()
        {
            _AdjustDisable();
        }

        public static void SwitchToOfflineMode()
        {
            _AdjustSwitchToOfflineMode();
        }

        public static void SwitchBackToOnlineMode()
        {
            _AdjustSwitchBackToOnlineMode();
        }

        public static void ProcessDeeplink(AdjustDeeplink adjustDeeplink)
        {
            _AdjustProcessDeeplink(adjustDeeplink.Deeplink);
        }

        public static void AddGlobalPartnerParameter(string key, string value)
        {
            _AdjustAddGlobalPartnerParameter(key, value);
        }

        public static void AddGlobalCallbackParameter(string key, string value)
        {
            _AdjustAddGlobalCallbackParameter(key, value);
        }

        public static void RemoveGlobalPartnerParameter(string key)
        {
            _AdjustRemoveGlobalPartnerParameter(key);
        }

        public static void RemoveGlobalCallbackParameter(string key)
        {
            _AdjustRemoveGlobalCallbackParameter(key);
        }

        public static void RemoveGlobalPartnerParameters()
        {
            _AdjustRemoveGlobalPartnerParameters();
        }

        public static void RemoveGlobalCallbackParameters()
        {
            _AdjustRemoveGlobalCallbackParameters();
        }

        public static void TrackAdRevenue(AdjustAdRevenue adRevenue)
        {
            string source = adRevenue.Source;
            double revenue = AdjustUtils.ConvertDouble(adRevenue.Revenue);
            string currency = adRevenue.Currency;
            int adImpressionsCount = AdjustUtils.ConvertInt(adRevenue.AdImpressionsCount);
            string adRevenueNetwork = adRevenue.AdRevenueNetwork;
            string adRevenueUnit = adRevenue.AdRevenueUnit;
            string adRevenuePlacement = adRevenue.AdRevenuePlacement;
            string stringJsonCallbackParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adRevenue.CallbackParameters);
            string stringJsonPartnerParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adRevenue.PartnerParameters);

            _AdjustTrackAdRevenue(
                source,
                revenue,
                currency,
                adImpressionsCount,
                adRevenueNetwork,
                adRevenueUnit,
                adRevenuePlacement,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void TrackAppStoreSubscription(AdjustAppStoreSubscription subscription)
        {
            string price = subscription.Price;
            string currency = subscription.Currency;
            string transactionId = subscription.TransactionId;
            string transactionDate = subscription.TransactionDate;
            string salesRegion = subscription.SalesRegion;
            string stringJsonCallbackParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(subscription.CallbackParameters);
            string stringJsonPartnerParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(subscription.PartnerParameters);
            
            _AdjustTrackAppStoreSubscription(
                price,
                currency,
                transactionId,
                transactionDate,
                salesRegion,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing)
        {
            int enabled = AdjustUtils.ConvertBool(thirdPartySharing.IsEnabled);
            string stringJsonGranularOptions = AdjustUtils.ConvertReadOnlyCollectionOfTripletsToJson(thirdPartySharing.GranularOptions);
            string stringJsonPartnerSharingSettings = AdjustUtils.ConvertReadOnlyCollectionOfTripletsToJson(thirdPartySharing.PartnerSharingSettings);

            _AdjustTrackThirdPartySharing(
                enabled,
                stringJsonGranularOptions,
                stringJsonPartnerSharingSettings);
        }

        public static void TrackMeasurementConsent(bool enabled)
        {
            _AdjustTrackMeasurementConsent(AdjustUtils.ConvertBool(enabled));
        }

        public static void RequestAppTrackingAuthorization(Action<int> callback)
        {
            if (appAttCallbacks == null)
            {
                appAttCallbacks = new List<Action<int>>();
            }
            appAttCallbacks.Add(callback);
            _AdjustRequestAppTrackingAuthorization(AttCallbackMonoPInvoke);
        }

        public static void UpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            bool lockedWindow,
            Action<string> callback)
        {
            appSkanErrorCallback = callback;
            _AdjustUpdateSkanConversionValue(
                conversionValue,
                coarseValue,
                AdjustUtils.ConvertBool(lockedWindow),
                SkanErrorCallbackMonoPInvoke);
        }

        // TODO: consider making async
        public static int GetAppTrackingAuthorizationStatus()
        {
            return _AdjustGetAppTrackingAuthorizationStatus();
        }

        public static void SetPushToken(string pushToken)
        {
            _AdjustSetPushToken(pushToken);
        }

        public static void IsEnabled(Action<bool> callback)
        {
            if (appIsEnabledGetterCallbacks == null)
            {
                appIsEnabledGetterCallbacks = new List<Action<bool>>();
            }
            appIsEnabledGetterCallbacks.Add(callback);
            _AdjustIsEnabled(IsEnabledGetterMonoPInvoke);
        }

        public static void GetAttribution(Action<AdjustAttribution> callback)
        {
            if (appAttributionGetterCallbacks == null)
            {
                appAttributionGetterCallbacks = new List<Action<AdjustAttribution>>();
            }
            appAttributionGetterCallbacks.Add(callback);
            _AdjustGetAttribution(AttributionGetterMonoPInvoke);
        }

        public static void GetAdid(Action<string> callback)
        {
            if (appAdidGetterCallbacks == null)
            {
                appAdidGetterCallbacks = new List<Action<string>>();
            }
            appAdidGetterCallbacks.Add(callback);
            _AdjustGetAdid(AdidGetterMonoPInvoke);
        }

        public static void GetIdfa(Action<string> callback)
        {
            if (appIdfaGetterCallbacks == null)
            {
                appIdfaGetterCallbacks = new List<Action<string>>();
            }
            appIdfaGetterCallbacks.Add(callback);
            _AdjustGetIdfa(IdfaGetterMonoPInvoke);
        }

        public static void GetIdfv(Action<string> callback)
        {
            if (appIdfvGetterCallbacks == null)
            {
                appIdfvGetterCallbacks = new List<Action<string>>();
            }
            appIdfvGetterCallbacks.Add(callback);
            _AdjustGetIdfv(IdfvGetterMonoPInvoke);
        }

        public static void GetLastDeeplink(Action<string> callback)
        {
            if (appLastDeeplinkGetterCallbacks == null)
            {
                appLastDeeplinkGetterCallbacks = new List<Action<string>>();
            }
            appLastDeeplinkGetterCallbacks.Add(callback);
            _AdjustGetLastDeeplink(LastDeeplinkGetterMonoPInvoke);
        }

        public static void GetSdkVersion(Action<string> callback)
        {
            if (appSdkVersionGetterCallbacks == null)
            {
                appSdkVersionGetterCallbacks = new List<Action<string>>();
            }
            appSdkVersionGetterCallbacks.Add(callback);
            _AdjustGetSdkVersion(SdkVersionGetterMonoPInvoke);
        }

        public static void GdprForgetMe()
        {
            _AdjustGdprForgetMe();
        }

        public static void VerifyAppStorePurchase(
            AdjustAppStorePurchase purchase,
            Action<AdjustPurchaseVerificationResult> callback)
        {
            string transactionId = purchase.TransactionId;
            string productId = purchase.ProductId;
            appPurchaseVerificationCallback = callback;
            
            _AdjustVerifyAppStorePurchase(
                transactionId,
                productId,
                PurchaseVerificationCallbackMonoPInvoke);
        }

        public static void ProcessAndResolveDeeplink(AdjustDeeplink adjustDeeplink, Action<string> callback)
        {
            appResolvedDeeplinkCallback = callback;
            _AdjustProcessAndResolveDeeplink(adjustDeeplink.Deeplink, ResolvedDeeplinkCallbackMonoPInvoke);
        }

        public static void VerifyAndTrackAppStorePurchase(
            AdjustEvent adjustEvent,
            Action<AdjustPurchaseVerificationResult> callback)
        {
            double revenue = AdjustUtils.ConvertDouble(adjustEvent.Revenue);
            string eventToken = adjustEvent.EventToken;
            string currency = adjustEvent.Currency;
            string productId = adjustEvent.ProductId;
            string transactionId = adjustEvent.TransactionId;
            string callbackId = adjustEvent.CallbackId;
            string deduplicationId = adjustEvent.DeduplicationId;
            string stringJsonCallbackParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adjustEvent.CallbackParameters);
            string stringJsonPartnerParameters = AdjustUtils.ConvertReadOnlyCollectionOfPairsToJson(adjustEvent.PartnerParameters);
            appVerifyAndTrackCallback = callback;

            _AdjustVerifyAndTrackAppStorePurchase(
                eventToken,
                revenue,
                currency,
                productId,
                transactionId,
                callbackId,
                deduplicationId,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters,
                VerifyAndTrackCallbackMonoPInvoke);
        }

        // used for testing only (don't use this in your app)
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string overwriteUrl = testOptions[AdjustUtils.KeyTestOptionsOverwriteUrl];
            string extraPath = testOptions.ContainsKey(AdjustUtils.KeyTestOptionsExtraPath) ? testOptions[AdjustUtils.KeyTestOptionsExtraPath] : null;
            string idfa = testOptions.ContainsKey(AdjustUtils.KeyTestOptionsIdfa) ? testOptions[AdjustUtils.KeyTestOptionsIdfa] : null;
            long timerIntervalMilis = -1;
            long timerStartMilis = -1;
            long sessionIntMilis = -1;
            long subsessionIntMilis = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;
            bool adServicesFrameworkEnabled = false;
            int attStatus = -1;

            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds)) 
            {
                timerIntervalMilis = long.Parse(testOptions[AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsTimerStartInMilliseconds)) 
            {
                timerStartMilis = long.Parse(testOptions[AdjustUtils.KeyTestOptionsTimerStartInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds))
            {
                sessionIntMilis = long.Parse(testOptions[AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds))
            {
                subsessionIntMilis = long.Parse(testOptions[AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsTeardown))
            {
                teardown = testOptions[AdjustUtils.KeyTestOptionsTeardown].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsDeleteState))
            {
                deleteState = testOptions[AdjustUtils.KeyTestOptionsDeleteState].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsNoBackoffWait))
            {
                noBackoffWait = testOptions[AdjustUtils.KeyTestOptionsNoBackoffWait].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled))
            {
                adServicesFrameworkEnabled = testOptions[AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled].ToLower() == "true";
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsAttStatus)) 
            {
                attStatus = int.Parse(testOptions[AdjustUtils.KeyTestOptionsAttStatus]);
            }

            _AdjustSetTestOptions(
                overwriteUrl,
                extraPath,
                timerIntervalMilis,
                timerStartMilis,
                sessionIntMilis,
                subsessionIntMilis,
                AdjustUtils.ConvertBool(teardown),
                AdjustUtils.ConvertBool(deleteState),
                AdjustUtils.ConvertBool(noBackoffWait),
                AdjustUtils.ConvertBool(adServicesFrameworkEnabled),
                attStatus,
                idfa);
        }

        public static void TrackSubsessionStart(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdjustTrackSubsessionStart();
            }
        }

        public static void TrackSubsessionEnd(string testingArgument = null)
        {
            if (testingArgument == "test") 
            {
                _AdjustTrackSubsessionEnd();
            }
        }

        // MonoPInvokeCallback methods as method parameters
        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateIsEnabledGetter))]
        private static void IsEnabledGetterMonoPInvoke(bool isEnabled) {
            if (appIsEnabledGetterCallbacks != null)
            {
                foreach (Action<bool> callback in appIsEnabledGetterCallbacks)
                {
                    callback(isEnabled);
                }
                appIsEnabledGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateAttributionGetter))]
        private static void AttributionGetterMonoPInvoke(string attribution) {
            if (appAttributionGetterCallbacks != null)
            {
                foreach (Action<AdjustAttribution> callback in appAttributionGetterCallbacks)
                {
                    callback(new AdjustAttribution(attribution));    
                }
                appAttributionGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateAdidGetter))]
        private static void AdidGetterMonoPInvoke(string adid) {
            if (appAdidGetterCallbacks != null)
            {
                foreach (Action<string> callback in appAdidGetterCallbacks)
                {
                    callback(adid);
                }
                appAdidGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateIdfaGetter))]
        private static void IdfaGetterMonoPInvoke(string idfa) {
            if (appIdfaGetterCallbacks != null)
            {
                foreach (Action<string> callback in appIdfaGetterCallbacks)
                {
                    callback(idfa);
                }
                appIdfaGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateIdfvGetter))]
        private static void IdfvGetterMonoPInvoke(string idfv) {
            if (appIdfvGetterCallbacks != null)
            {
                foreach (Action<string> callback in appIdfaGetterCallbacks)
                {
                    callback(idfv);
                }
                appIdfvGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateLastDeeplinkGetter))]
        private static void LastDeeplinkGetterMonoPInvoke(string lastDeeplink) {
            if (appLastDeeplinkGetterCallbacks != null)
            {
                foreach (Action<string> callback in appLastDeeplinkGetterCallbacks)
                {
                    callback(lastDeeplink);
                }
                appLastDeeplinkGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateSdkVersionGetter))]
        private static void SdkVersionGetterMonoPInvoke(string sdkVersion) {
            if (appSdkVersionGetterCallbacks != null)
            {
                foreach (Action<string> callback in appSdkVersionGetterCallbacks)
                {
                    callback(sdkPrefix + "@" + sdkVersion);
                }
                appSdkVersionGetterCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateAttCallback))]
        private static void AttCallbackMonoPInvoke(int status) {
            if (appAttCallbacks != null)
            {
                foreach (Action<int> callback in appAttCallbacks)
                {
                    callback(status);
                }
                appAttCallbacks.Clear();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegatePurchaseVerificationCallback))]
        private static void PurchaseVerificationCallbackMonoPInvoke(string verificationResult) {
            if (appPurchaseVerificationCallback != null)
            {
                appPurchaseVerificationCallback(new AdjustPurchaseVerificationResult(verificationResult));
                appPurchaseVerificationCallback = null;
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateVerifyAndTrackCallback))]
        private static void VerifyAndTrackCallbackMonoPInvoke(string verificationResult) {
            if (appVerifyAndTrackCallback != null)
            {
                appVerifyAndTrackCallback(new AdjustPurchaseVerificationResult(verificationResult));
                appVerifyAndTrackCallback = null;
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateResolvedDeeplinkCallback))]
        private static void ResolvedDeeplinkCallbackMonoPInvoke(string deeplink) {
            if (appResolvedDeeplinkCallback != null)
            {
                appResolvedDeeplinkCallback(deeplink);
                appResolvedDeeplinkCallback = null;
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateSkanErrorCallback))]
        private static void SkanErrorCallbackMonoPInvoke(string error) {
            if (appSkanErrorCallback != null)
            {
                appSkanErrorCallback(error);
                appSkanErrorCallback = null;
            }
        }

        // MonoPInvokeCallback methods as subscriptions
        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateAttributionCallback))]
        private static void AttributionCallbackMonoPInvoke(string attribution) {
            if (appAttributionCallback != null)
            {
                appAttributionCallback(new AdjustAttribution(attribution));
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateSessionSuccessCallback))]
        private static void SessionSuccessCallbackMonoPInvoke(string sessionSuccess) {
            if (appSessionSuccessCallback != null)
            {
                appSessionSuccessCallback(new AdjustSessionSuccess(sessionSuccess));
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateSessionFailureCallback))]
        private static void SessionFailureCallbackMonoPInvoke(string sessionFailure) {
            if (appSessionFailureCallback != null)
            {
                appSessionFailureCallback(new AdjustSessionFailure(sessionFailure));
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateEventSuccessCallback))]
        private static void EventSuccessCallbackMonoPInvoke(string eventSuccess) {
            if (appEventSuccessCallback != null)
            {
                appEventSuccessCallback(new AdjustEventSuccess(eventSuccess));
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateEventFailureCallback))]
        private static void EventFailureCallbackMonoPInvoke(string eventFailure) {
            if (appEventFailureCallback != null)
            {
                appEventFailureCallback(new AdjustEventFailure(eventFailure));
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateDeferredDeeplinkCallback))]
        private static void DeferredDeeplinkCallbackMonoPInvoke(string deeplink) {
            if (appDeferredDeeplinkCallback != null)
            {
                appDeferredDeeplinkCallback(deeplink);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(AdjustDelegateSkanUpdatedCallback))]
        private static void SkanUpdatedCallbackMonoPInvoke(string skanData) {
            if (appSkanUpdatedCallback != null)
            {
                appSkanUpdatedCallback(AdjustUtils.GetSkanUpdateDataDictionary(skanData));
            }
        }
    }
#endif
}
