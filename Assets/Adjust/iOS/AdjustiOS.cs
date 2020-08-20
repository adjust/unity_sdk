using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adjust.sdk
{
#if UNITY_IOS
    public class AdjustiOS
    {
        private const string sdkPrefix = "unity4.23.0";

        [DllImport("__Internal")]
        private static extern void _AdjustLaunchApp(
            string appToken,
            string environment,
            string sdkPrefix,
            string userAgent,
            string defaultTracker,
            string extenralDeviceId,
            string urlStrategy,
            string sceneName,
            int allowSuppressLogLevel,
            int logLevel,
            int isDeviceKnown,
            int eventBuffering,
            int sendInBackground,
            int allowiAdInfoReading,
            int allowIdfaReading,
            int deactivateSkAdNetworkHandling,
            long secretId,
            long info1,
            long info2,
            long info3,
            long info4,
            double delayStart,
            int launchDeferredDeeplink,
            int isAttributionCallbackImplemented, 
            int isEventSuccessCallbackImplemented,
            int isEventFailureCallbackImplemented,
            int isSessionSuccessCallbackImplemented,
            int isSessionFailureCallbackImplemented,
            int isDeferredDeeplinkCallbackImplemented);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackEvent(
            string eventToken,
            double revenue,
            string currency,
            string receipt,
            string transactionId,
            string callbackId,
            int isReceiptSet,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustSetEnabled(int enabled);

        [DllImport("__Internal")]
        private static extern int _AdjustIsEnabled();

        [DllImport("__Internal")]
        private static extern void _AdjustSetOfflineMode(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdjustSetDeviceToken(string deviceToken);

        [DllImport("__Internal")]
        private static extern void _AdjustAppWillOpenUrl(string url);

        [DllImport("__Internal")]
        private static extern string _AdjustGetIdfa();

        [DllImport("__Internal")]
        private static extern string _AdjustGetAdid();

        [DllImport("__Internal")]
        private static extern string _AdjustGetSdkVersion();

        [DllImport("__Internal")]
        private static extern void _AdjustGdprForgetMe();

        [DllImport("__Internal")]
        private static extern void _AdjustDisableThirdPartySharing();

        [DllImport("__Internal")]
        private static extern string _AdjustGetAttribution();

        [DllImport("__Internal")]
        private static extern void _AdjustSendFirstPackages();

        [DllImport("__Internal")]
        private static extern void _AdjustAddSessionPartnerParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdjustAddSessionCallbackParameter(string key, string value);

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveSessionPartnerParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdjustRemoveSessionCallbackParameter(string key);

        [DllImport("__Internal")]
        private static extern void _AdjustResetSessionPartnerParameters();

        [DllImport("__Internal")]
        private static extern void _AdjustResetSessionCallbackParameters();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackAdRevenue(string source, string payload);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackAppStoreSubscription(
            string price,
            string currency,
            string transactionId,
            string receipt,
            string billingStore,
            string transactionDate,
            string salesRegion,
            string jsonCallbackParameters,
            string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustSetTestOptions(
            string baseUrl,
            string gdprUrl,
            string subscriptionUrl,
            string extraPath,
            long timerIntervalInMilliseconds,
            long timerStartInMilliseconds,
            long sessionIntervalInMilliseconds,
            long subsessionIntervalInMilliseconds,
            int teardown,
            int deleteState,
            int noBackoffWait,
            int iAdFrameworkEnabled);

        [DllImport("__Internal")]
        private static extern void _AdjustRequestTrackingAuthorizationWithCompletionHandler();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionStart();

        [DllImport("__Internal")]
        private static extern void _AdjustTrackSubsessionEnd();

        public AdjustiOS() {}

        public static void Start(AdjustConfig adjustConfig)
        {
            string appToken = adjustConfig.appToken != null ? adjustConfig.appToken : "ADJ_INVALID";
            string sceneName = adjustConfig.sceneName != null ? adjustConfig.sceneName : "ADJ_INVALID";
            string userAgent = adjustConfig.userAgent != null ? adjustConfig.userAgent : "ADJ_INVALID";
            string defaultTracker = adjustConfig.defaultTracker != null ? adjustConfig.defaultTracker : "ADJ_INVALID";
            string externalDeviceId = adjustConfig.externalDeviceId != null ? adjustConfig.externalDeviceId : "ADJ_INVALID";
            string urlStrategy = adjustConfig.urlStrategy != null ? adjustConfig.urlStrategy : "ADJ_INVALID";
            string environment = adjustConfig.environment.ToLowercaseString();
            long info1 = AdjustUtils.ConvertLong(adjustConfig.info1);
            long info2 = AdjustUtils.ConvertLong(adjustConfig.info2);
            long info3 = AdjustUtils.ConvertLong(adjustConfig.info3);
            long info4 = AdjustUtils.ConvertLong(adjustConfig.info4);
            long secretId = AdjustUtils.ConvertLong(adjustConfig.secretId);
            double delayStart = AdjustUtils.ConvertDouble(adjustConfig.delayStart);
            int logLevel = AdjustUtils.ConvertLogLevel(adjustConfig.logLevel);
            int isDeviceKnown = AdjustUtils.ConvertBool(adjustConfig.isDeviceKnown);
            int sendInBackground = AdjustUtils.ConvertBool(adjustConfig.sendInBackground);
            int eventBufferingEnabled = AdjustUtils.ConvertBool(adjustConfig.eventBufferingEnabled);
            int allowiAdInfoReading = AdjustUtils.ConvertBool(adjustConfig.allowiAdInfoReading);
            int allowIdfaReading = AdjustUtils.ConvertBool(adjustConfig.allowIdfaReading);
            int allowSuppressLogLevel = AdjustUtils.ConvertBool(adjustConfig.allowSuppressLogLevel);
            int launchDeferredDeeplink = AdjustUtils.ConvertBool(adjustConfig.launchDeferredDeeplink);
            int deactivateSkAdNetworkHandling = AdjustUtils.ConvertBool(adjustConfig.skAdNetworkHandling);
            int isAttributionCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getAttributionChangedDelegate() != null);
            int isEventSuccessCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getEventSuccessDelegate() != null);
            int isEventFailureCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getEventFailureDelegate() != null);
            int isSessionSuccessCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getSessionSuccessDelegate() != null);
            int isSessionFailureCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getSessionFailureDelegate() != null);
            int isDeferredDeeplinkCallbackImplemented = AdjustUtils.ConvertBool(adjustConfig.getDeferredDeeplinkDelegate() != null);

            _AdjustLaunchApp(
                appToken,
                environment,
                sdkPrefix,
                userAgent,
                defaultTracker,
                externalDeviceId,
                urlStrategy,
                sceneName,
                allowSuppressLogLevel,
                logLevel,
                isDeviceKnown,
                eventBufferingEnabled,
                sendInBackground,
                allowiAdInfoReading,
                allowIdfaReading,
                deactivateSkAdNetworkHandling,
                secretId,
                info1,
                info2,
                info3,
                info4,
                delayStart,
                launchDeferredDeeplink,
                isAttributionCallbackImplemented,
                isEventSuccessCallbackImplemented,
                isEventFailureCallbackImplemented,
                isSessionSuccessCallbackImplemented,
                isSessionFailureCallbackImplemented,
                isDeferredDeeplinkCallbackImplemented);
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            int isReceiptSet = AdjustUtils.ConvertBool(adjustEvent.isReceiptSet);
            double revenue = AdjustUtils.ConvertDouble(adjustEvent.revenue);
            string eventToken = adjustEvent.eventToken;
            string currency = adjustEvent.currency;
            string receipt = adjustEvent.receipt;
            string transactionId = adjustEvent.transactionId;
            string callbackId = adjustEvent.callbackId;
            string stringJsonCallbackParameters = AdjustUtils.ConvertListToJson(adjustEvent.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(adjustEvent.partnerList);

            _AdjustTrackEvent(eventToken, revenue, currency, receipt, transactionId, callbackId, isReceiptSet, stringJsonCallbackParameters, stringJsonPartnerParameters);
        }        

        public static void SetEnabled(bool enabled)
        {
            _AdjustSetEnabled(AdjustUtils.ConvertBool(enabled));
        }

        public static bool IsEnabled()
        {
            var iIsEnabled = _AdjustIsEnabled();
            return Convert.ToBoolean(iIsEnabled);
        }

        public static void SetOfflineMode(bool enabled)
        {
            _AdjustSetOfflineMode(AdjustUtils.ConvertBool(enabled));
        }

        public static void SendFirstPackages()
        {
            _AdjustSendFirstPackages();
        }

        public static void AppWillOpenUrl(string url)
        {
            _AdjustAppWillOpenUrl(url);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            _AdjustAddSessionPartnerParameter(key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            _AdjustAddSessionCallbackParameter(key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            _AdjustRemoveSessionPartnerParameter(key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            _AdjustRemoveSessionCallbackParameter(key);
        }

        public static void ResetSessionPartnerParameters()
        {
            _AdjustResetSessionPartnerParameters();
        }

        public static void ResetSessionCallbackParameters()
        {
            _AdjustResetSessionCallbackParameters();
        }

        public static void TrackAdRevenue(string source, string payload)
        {
            _AdjustTrackAdRevenue(source, payload);
        }

        public static void TrackAppStoreSubscription(AdjustAppStoreSubscription subscription)
        {
            string price = subscription.price;
            string currency = subscription.currency;
            string transactionId = subscription.transactionId;
            string receipt = subscription.receipt;
            string billingStore = subscription.billingStore;
            string transactionDate = subscription.transactionDate;
            string salesRegion = subscription.salesRegion;
            string stringJsonCallbackParameters = AdjustUtils.ConvertListToJson(subscription.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(subscription.partnerList);
            
            _AdjustTrackAppStoreSubscription(
                price,
                currency,
                transactionId,
                receipt,
                billingStore,
                transactionDate,
                salesRegion,
                stringJsonCallbackParameters,
                stringJsonPartnerParameters);
        }

        public static void RequestTrackingAuthorizationWithCompletionHandler()
        {
            _AdjustRequestTrackingAuthorizationWithCompletionHandler();
        }

        public static void SetDeviceToken(string deviceToken)
        {
            _AdjustSetDeviceToken(deviceToken);
        }

        public static string GetIdfa()
        {
            return _AdjustGetIdfa();
        }

        public static string GetAdid()
        {
            return _AdjustGetAdid();
        }

        public static string GetSdkVersion()
        {
            return sdkPrefix + "@" + _AdjustGetSdkVersion();
        }

        public static void GdprForgetMe()
        {
            _AdjustGdprForgetMe();
        }

        public static void DisableThirdPartySharing()
        {
            _AdjustDisableThirdPartySharing();
        }

        public static AdjustAttribution GetAttribution()
        {
            string attributionString = _AdjustGetAttribution();
            if (null == attributionString)
            {
                return null;
            }

            var attribution = new AdjustAttribution(attributionString);
            return attribution;
        }

        // Used for testing only.
        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string baseUrl = testOptions[AdjustUtils.KeyTestOptionsBaseUrl];
            string gdprUrl = testOptions[AdjustUtils.KeyTestOptionsGdprUrl];
            string subscriptionUrl = testOptions[AdjustUtils.KeyTestOptionsSubscriptionUrl];
            string extraPath = testOptions.ContainsKey(AdjustUtils.KeyTestOptionsExtraPath) ? testOptions[AdjustUtils.KeyTestOptionsExtraPath] : null;
            long timerIntervalMilis = -1;
            long timerStartMilis = -1;
            long sessionIntMilis = -1;
            long subsessionIntMilis = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;
            bool iAdFrameworkEnabled = false;

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
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsiAdFrameworkEnabled))
            {
                iAdFrameworkEnabled = testOptions[AdjustUtils.KeyTestOptionsiAdFrameworkEnabled].ToLower() == "true";
            }

            _AdjustSetTestOptions(
                baseUrl,
                gdprUrl,
                subscriptionUrl,
                extraPath,
                timerIntervalMilis,
                timerStartMilis,
                sessionIntMilis,
                subsessionIntMilis, 
                AdjustUtils.ConvertBool(teardown),
                AdjustUtils.ConvertBool(deleteState),
                AdjustUtils.ConvertBool(noBackoffWait),
                AdjustUtils.ConvertBool(iAdFrameworkEnabled));
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
    }
#endif
}
