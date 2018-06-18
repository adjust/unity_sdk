using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using com.adjust.sdk.test;

namespace com.adjust.sdk
{
#if UNITY_IOS
    public class AdjustiOS
    {
        private const string sdkPrefix = "unity4.14.1";

        [DllImport("__Internal")]
        private static extern void _AdjustLaunchApp(
            string appToken,
            string environment,
            string sdkPrefix,
            string userAgent,
            string defaultTracker,
            string sceneName,
            int allowSuppressLogLevel,
            int logLevel,
            int isDeviceKnown,
            int eventBuffering,
            int sendInBackground,
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
        private static extern void _AdjustGdprForgetMe();

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
        private static extern void _AdjustSetTestOptions(
            string baseUrl,
            string basePath,
            string gdprUrl,
            string gdprPath,
            long timerIntervalInMilliseconds,
            long timerStartInMilliseconds,
            long sessionIntervalInMilliseconds,
            long subsessionIntervalInMilliseconds,
            int teardown,
            int deleteState,
            int noBackoffWait);

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
            int allowSuppressLogLevel = AdjustUtils.ConvertBool(adjustConfig.allowSuppressLogLevel);
            int launchDeferredDeeplink = AdjustUtils.ConvertBool(adjustConfig.launchDeferredDeeplink);

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
                sceneName,
                allowSuppressLogLevel,
                logLevel,
                isDeviceKnown,
                eventBufferingEnabled,
                sendInBackground,
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
            string stringJsonCallBackParameters = AdjustUtils.ConvertListToJson(adjustEvent.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson(adjustEvent.partnerList);

            _AdjustTrackEvent(eventToken, revenue, currency, receipt, transactionId, isReceiptSet, stringJsonCallBackParameters, stringJsonPartnerParameters);
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

        // iOS specific methods
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

        public static void GdprForgetMe()
        {
            _AdjustGdprForgetMe();
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

        public static void SetTestOptions(AdjustTestOptions testOptions)
        {
            long timerIntervalMls = testOptions.TimerIntervalInMilliseconds.HasValue ? testOptions.TimerIntervalInMilliseconds.Value : -1;
            long timerStartMls = testOptions.TimerStartInMilliseconds.HasValue ? testOptions.TimerStartInMilliseconds.Value : -1;
            long sessionIntMls = testOptions.SessionIntervalInMilliseconds.HasValue ? testOptions.SessionIntervalInMilliseconds.Value : -1;
            long subsessionIntMls = testOptions.SubsessionIntervalInMilliseconds.HasValue ? testOptions.SubsessionIntervalInMilliseconds.Value : -1;
            bool teardown = testOptions.Teardown.HasValue ? testOptions.Teardown.Value : false;
            bool deleteState = testOptions.DeleteState.HasValue ? testOptions.DeleteState.Value : false;
            bool noBackoffWait = testOptions.NoBackoffWait.HasValue ? testOptions.NoBackoffWait.Value : false;

            _AdjustSetTestOptions(
                testOptions.BaseUrl,
                testOptions.BasePath,
                testOptions.GdprUrl,
                testOptions.GdprPath,
                timerIntervalMls,
                timerStartMls,
                sessionIntMls,
                subsessionIntMls, 
                AdjustUtils.ConvertBool(teardown),
                AdjustUtils.ConvertBool(deleteState),
                AdjustUtils.ConvertBool(noBackoffWait));
        }

        public static void TrackSubsessionStart()
        {
            _AdjustTrackSubsessionStart();
        }

        public static void TrackSubsessionEnd()
        {
            _AdjustTrackSubsessionEnd();
        }
    }
#endif
}
