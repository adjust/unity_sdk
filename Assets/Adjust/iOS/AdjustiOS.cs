using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace com.adjust.sdk {
#if UNITY_IOS
    public class AdjustiOS : IAdjust {
        #region Fields
        private const string sdkPrefix = "unity4.11.3";
        #endregion

        #region External methods
        [DllImport("__Internal")]
        private static extern void _AdjustLaunchApp(string appToken, string environment, string sdkPrefix, int allowSuppressLogLevel,
            int logLevel, int eventBuffering, int sendInBackground, double delayStart, string userAgent, int launchDeferredDeeplink,
            string sceneName, int isAttributionCallbackImplemented, int isEventSuccessCallbackImplemented,int isEventFailureCallbackImplemented,
            int isSessionSuccessCallbackImplemented, int isSessionFailureCallbackImplemented, int isDeferredDeeplinkCallbackImplemented);

        [DllImport("__Internal")]
        private static extern void _AdjustTrackEvent(string eventToken, double revenue, string currency, string receipt, string transactionId,
            int isReceiptSet, string jsonCallbackParameters, string jsonPartnerParameters);

        [DllImport("__Internal")]
        private static extern void _AdjustSetEnabled(int enabled);
        
        [DllImport("__Internal")]
        private static extern int _AdjustIsEnabled();
        
        [DllImport("__Internal")]
        private static extern void _AdjustSetOfflineMode(int enabled);

        [DllImport("__Internal")]
        private static extern void _AdjustSetDeviceToken(string deviceToken);

        [DllImport("__Internal")]
        private static extern string _AdjustGetIdfa();

        [DllImport("__Internal")]
        private static extern string _AdjustGetAdid();

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

        #endregion

        #region Constructors
        public AdjustiOS() {}
        #endregion

        #region Public methods
        public void start(AdjustConfig adjustConfig) {
            string appToken = adjustConfig.appToken;
            string sceneName = adjustConfig.sceneName;
            string userAgent = adjustConfig.userAgent != null ? adjustConfig.userAgent : String.Empty;
            string environment = adjustConfig.environment.lowercaseToString();

            double delayStart = AdjustUtils.ConvertDouble(adjustConfig.delayStart);

            int logLevel = AdjustUtils.ConvertLogLevel(adjustConfig.logLevel);
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
                allowSuppressLogLevel,
                logLevel,
                eventBufferingEnabled,
                sendInBackground,
                delayStart,
                userAgent,
                launchDeferredDeeplink,
                sceneName,
                isAttributionCallbackImplemented,
                isEventSuccessCallbackImplemented,
                isEventFailureCallbackImplemented,
                isSessionSuccessCallbackImplemented,
                isSessionFailureCallbackImplemented,
                isDeferredDeeplinkCallbackImplemented);
        }

        public void trackEvent(AdjustEvent adjustEvent) {
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

        public void setEnabled(bool enabled) {
            _AdjustSetEnabled(AdjustUtils.ConvertBool(enabled));
        }

        public bool isEnabled() {
            var iIsEnabled = _AdjustIsEnabled();

            return Convert.ToBoolean(iIsEnabled);
        }

        public void setOfflineMode(bool enabled) {
            _AdjustSetOfflineMode(AdjustUtils.ConvertBool(enabled));
        }

        public void sendFirstPackages() {
            _AdjustSendFirstPackages();
        }

        public static void addSessionPartnerParameter(string key, string value) {
            _AdjustAddSessionPartnerParameter(key, value);
        }

        public static void addSessionCallbackParameter(string key, string value) {
            _AdjustAddSessionCallbackParameter(key, value);
        }

        public static void removeSessionPartnerParameter(string key) {
            _AdjustRemoveSessionPartnerParameter(key);
        }

        public static void removeSessionCallbackParameter(string key) {
            _AdjustRemoveSessionCallbackParameter(key);
        }

        public static void resetSessionPartnerParameters() {
            _AdjustResetSessionPartnerParameters();
        }

        public static void resetSessionCallbackParameters() {
            _AdjustResetSessionCallbackParameters();
        }

        // iOS specific methods
        public void setDeviceToken(string deviceToken) {
            _AdjustSetDeviceToken(deviceToken);
        }

        public string getIdfa() {
            return _AdjustGetIdfa();
        }

        public string getAdid() {
            return _AdjustGetAdid();
        }

        public AdjustAttribution getAttribution() {
            string attributionString = _AdjustGetAttribution();

            if (null == attributionString) {
                return null;
            }

            var attribution = new AdjustAttribution(attributionString);

            return attribution;
        }

        // Android specific methods
        public void onPause() {}

        public void onResume() {}

        public void setReferrer(string referrer) {}

        public void getGoogleAdId(Action<string> onDeviceIdsRead) {}
        #endregion
    }
#endif
}
