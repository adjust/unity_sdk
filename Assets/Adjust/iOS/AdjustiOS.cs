using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace com.adjust.sdk
{
#if UNITY_IOS
    public class AdjustiOS
    {
        private const string sdkPrefix = "unity5.0.0";

        [DllImport("__Internal")]
        private static extern void _AdjustInitSdk(
            string gameObjectName,
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
            int isDataResidency,
            int isSendingInBackgroundEnabled,
            int isAdServicesEnabled,
            int isIdfaReadingEnabled,
            int isSkanAttributionEnabled,
            int isLinkMeEnabled,
            int isCostDataInAttributionEnabled,
            int isDeviceIdsReadingOnceEnabled,
            int isDeferredDeeplinkOpeningEnabled,
            int isAttributionCallbackImplemented,
            int isEventSuccessCallbackImplemented,
            int isEventFailureCallbackImplemented,
            int isSessionSuccessCallbackImplemented,
            int isSessionFailureCallbackImplemented,
            int isDeferredDeeplinkCallbackImplemented,
            int isSkanUpdatedCallbackImplemented);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackEvent(
            string eventToken,
            double revenue,
            string currency,
            string receipt,
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
        private static extern void _AdjustEnableCoppaCompliance();

        [DllImport("__Internal")]
        private static extern void _AdjustDisableCoppaCompliance();

        [DllImport("__Internal")]
        private static extern void _AdjustIsEnabled(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustSwitchToOfflineMode();

        [DllImport("__Internal")]
        private static extern void _AdjustSwitchBackToOnlineMode();

        [DllImport("__Internal")]
        private static extern void _AdjustSetPushToken(string pushToken);

        [DllImport("__Internal")]
        private static extern void _AdjustProcessDeeplink(string deeplink);

        [DllImport("__Internal")]
        private static extern void _AdjustProcessAndResolveDeeplink(
            string deeplink,
            string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustGetIdfa(string gameObjectName);

        // private delegate void DelegateCallbackIdfvGetter(string idfv);
        [DllImport("__Internal")]
        private static extern void _AdjustGetIdfv(string gameObjectName);
        // private static extern void _AdjustGetIdfv(DelegateCallbackIdfvGetter callback);

        [DllImport("__Internal")]
        private static extern void _AdjustGetAdid(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustGetSdkVersion(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustGdprForgetMe();

        [DllImport("__Internal")]
        private static extern void _AdjustGetAttribution(string gameObjectName);

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
            string receipt,
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

        [DllImport("__Internal")]
        private static extern void _AdjustRequestAppTrackingAuthorizationWithCompletionHandler(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustUpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            int lockWindow,
            string gameObjectName);

        [DllImport("__Internal")]
        private static extern int _AdjustGetAppTrackingAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionStart();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionEnd();

        [DllImport("__Internal")]
        private static extern void _AdjustGetLastDeeplink(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _AdjustVerifyAppStorePurchase(
            string transactionId,
            string productId,
            string receipt,
            string gameObjectName);

        public AdjustiOS() {}

        public static void InitSdk(AdjustConfig adjustConfig)
        {
            string gameObjectName = adjustConfig.gameObjectName != null ? adjustConfig.gameObjectName : "ADJ_INVALID";
            string appToken = adjustConfig.appToken != null ? adjustConfig.appToken : "ADJ_INVALID";
            string environment = adjustConfig.environment.ToLowercaseString();
            string defaultTracker = adjustConfig.defaultTracker != null ? adjustConfig.defaultTracker : "ADJ_INVALID";
            string externalDeviceId = adjustConfig.externalDeviceId != null ? adjustConfig.externalDeviceId : "ADJ_INVALID";
            string stringJsonUrlStrategyDomains = AdjustUtils.ConvertListToJson(adjustConfig.urlStrategyDomains);
            int attConsentWaitingInterval = AdjustUtils.ConvertInt(adjustConfig.attConsentWaitingInterval);
            int eventDeduplicationIdsMaxSize = AdjustUtils.ConvertInt(adjustConfig.eventDeduplicationIdsMaxSize);
            int logLevel = AdjustUtils.ConvertLogLevel(adjustConfig.logLevel);
            int isSendingInBackgroundEnabled = AdjustUtils.ConvertBool(adjustConfig.isSendingInBackgroundEnabled);
            int isAdServicesEnabled = AdjustUtils.ConvertBool(adjustConfig.isAdServicesEnabled);
            int isIdfaReadingEnabled = AdjustUtils.ConvertBool(adjustConfig.isIdfaReadingEnabled);
            int allowSuppressLogLevel = AdjustUtils.ConvertBool(adjustConfig.allowSuppressLogLevel);
            int isDeferredDeeplinkOpeningEnabled = AdjustUtils.ConvertBool(adjustConfig.isDeferredDeeplinkOpeningEnabled);
            int isSkanAttributionEnabled = AdjustUtils.ConvertBool(adjustConfig.isSkanAttributionEnabled);
            int isLinkMeEnabled = AdjustUtils.ConvertBool(adjustConfig.isLinkMeEnabled);
            int isCostDataInAttributionEnabled = AdjustUtils.ConvertBool(adjustConfig.isCostDataInAttributionEnabled);
            int isDeviceIdsReadingOnceEnabled = AdjustUtils.ConvertBool(adjustConfig.isDeviceIdsReadingOnceEnabled);
            int shouldUseSubdomains = AdjustUtils.ConvertBool(adjustConfig.shouldUseSubdomains);
            int isDataResidency = AdjustUtils.ConvertBool(adjustConfig.isDataResidency);
            int isAttributionCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetAttributionChangedDelegate() != null);
            int isEventSuccessCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetEventSuccessDelegate() != null);
            int isEventFailureCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetEventFailureDelegate() != null);
            int isSessionSuccessCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetSessionSuccessDelegate() != null);
            int isSessionFailureCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetSessionFailureDelegate() != null);
            int isDeferredDeeplinkCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetDeferredDeeplinkDelegate() != null);
            int isSkanUpdatedCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.GetSkanUpdatedDelegate() != null);

            _AdjustInitSdk(
                gameObjectName,
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
                isDataResidency,
                isSendingInBackgroundEnabled,
                isAdServicesEnabled,
                isIdfaReadingEnabled,
                isSkanAttributionEnabled,
                isLinkMeEnabled,
                isCostDataInAttributionEnabled,
                isDeviceIdsReadingOnceEnabled,
                isDeferredDeeplinkOpeningEnabled,
                isAttributionCallbackImplemented,
                isEventSuccessCallbackImplemented,
                isEventFailureCallbackImplemented,
                isSessionSuccessCallbackImplemented,
                isSessionFailureCallbackImplemented,
                isDeferredDeeplinkCallbackImplemented,
                isSkanUpdatedCallbackImplemented);
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            double revenue = AdjustUtils.ConvertDouble(adjustEvent.revenue);
            string eventToken = adjustEvent.eventToken;
            string currency = adjustEvent.currency;
            string receipt = adjustEvent.receipt;
            string productId = adjustEvent.productId;
            string transactionId = adjustEvent.transactionId;
            string callbackId = adjustEvent.callbackId;
            string deduplicationId = adjustEvent.deduplicationId;
            string stringJsonCallbackParameters = AdjustUtils.ConvertListToJson(adjustEvent.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(adjustEvent.partnerList);

            _AdjustTrackEvent(
                eventToken,
                revenue,
                currency,
                receipt,
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

        public static void EnableCoppaCompliance()
        {
            _AdjustEnableCoppaCompliance();
        }

        public static void DisableCoppaCompliance()
        {
            _AdjustDisableCoppaCompliance();
        }

        public static void IsEnabled(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustIsEnabled(cGameObjectName);
        }

        public static void SwitchToOfflineMode()
        {
            _AdjustSwitchToOfflineMode();
        }

        public static void SwitchBackToOnlineMode()
        {
            _AdjustSwitchBackToOnlineMode();
        }

        public static void ProcessDeeplink(string deeplink)
        {
            _AdjustProcessDeeplink(deeplink);
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
            string source = adRevenue.source;
            double revenue = AdjustUtils.ConvertDouble(adRevenue.revenue);
            string currency = adRevenue.currency;
            int adImpressionsCount = AdjustUtils.ConvertInt(adRevenue.adImpressionsCount);
            string adRevenueNetwork = adRevenue.adRevenueNetwork;
            string adRevenueUnit = adRevenue.adRevenueUnit;
            string adRevenuePlacement = adRevenue.adRevenuePlacement;
            string stringJsonCallbackParameters = AdjustUtils.ConvertListToJson(adRevenue.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(adRevenue.partnerList);

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
            string price = subscription.price;
            string currency = subscription.currency;
            string transactionId = subscription.transactionId;
            string receipt = subscription.receipt;
            string transactionDate = subscription.transactionDate;
            string salesRegion = subscription.salesRegion;
            string stringJsonCallbackParameters = AdjustUtils.ConvertListToJson(subscription.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(subscription.partnerList);
            
            _AdjustTrackAppStoreSubscription(
                price,
                currency,
                transactionId,
                receipt,
                transactionDate,
                salesRegion,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void TrackThirdPartySharing(AdjustThirdPartySharing thirdPartySharing)
        {
            int enabled = AdjustUtils.ConvertBool(thirdPartySharing.isEnabled);
            List<string> jsonGranularOptions = new List<string>();
            foreach (KeyValuePair<string, List<string>> entry in thirdPartySharing.granularOptions)
            {
                jsonGranularOptions.Add(entry.Key);
                jsonGranularOptions.Add(AdjustUtils.ConvertListToJson(entry.Value));
            }
            List<string> jsonPartnerSharingSettings = new List<string>();
            foreach (KeyValuePair<string, List<string>> entry in thirdPartySharing.partnerSharingSettings)
            {
                jsonPartnerSharingSettings.Add(entry.Key);
                jsonPartnerSharingSettings.Add(AdjustUtils.ConvertListToJson(entry.Value));
            }

            _AdjustTrackThirdPartySharing(enabled, AdjustUtils.ConvertListToJson(jsonGranularOptions), AdjustUtils.ConvertListToJson(jsonPartnerSharingSettings));
        }

        public static void TrackMeasurementConsent(bool enabled)
        {
            _AdjustTrackMeasurementConsent(AdjustUtils.ConvertBool(enabled));
        }

        public static void RequestAppTrackingAuthorizationWithCompletionHandler(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustRequestAppTrackingAuthorizationWithCompletionHandler(cGameObjectName);
        }

        public static void UpdateSkanConversionValue(
            int conversionValue,
            string coarseValue,
            bool lockedWindow,
            string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustUpdateSkanConversionValue(
                conversionValue,
                coarseValue,
                AdjustUtils.ConvertBool(lockedWindow),
                cGameObjectName);
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

        public static void GetIdfa(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetIdfa(cGameObjectName);
        }

        public static void GetIdfv(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetIdfv(cGameObjectName);
            // _AdjustGetIdfv(IdfvGetterTriggered);
        }

        public static void GetAdid(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetAdid(cGameObjectName);
        }

        // TODO: consider sending the prefix down the drain to do the concatenation natively
        public static void GetSdkVersion(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetSdkVersion(cGameObjectName);
        }

        public static void GdprForgetMe()
        {
            _AdjustGdprForgetMe();
        }

        public static void GetAttribution(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetAttribution(cGameObjectName);
        }

        public static void GetLastDeeplink(string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustGetLastDeeplink(cGameObjectName);
        }

        public static void VerifyAppStorePurchase(AdjustAppStorePurchase purchase, string gameObjectName)
        {
            string transactionId = purchase.transactionId;
            string productId = purchase.productId;
            string receipt = purchase.receipt;
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            
            _AdjustVerifyAppStorePurchase(
                transactionId,
                productId,
                receipt,
                cGameObjectName);
        }

        public static void ProcessAndResolveDeeplink(string url, string gameObjectName)
        {
            string cGameObjectName = gameObjectName != null ? gameObjectName : "ADJ_INVALID";
            _AdjustProcessAndResolveDeeplink(url, cGameObjectName);
        }

        // Used for testing only.
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

        // // mono pinvoke
        // [MonoPInvokeCallback(typeof(DelegateCallbackIdfvGetter))] 
        // private static void IdfvGetterTriggered(string idfv) {
        //     Debug.Log("IDFV message received: " + idfv);
        // }
    }
#endif
}
