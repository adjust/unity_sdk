using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace com.adjust.sdk
{
#if UNITY_IOS
    public class AdjustiOS : IAdjust
    {
        #region Fields
        private const string sdkPrefix = "unity4.7.0";
        #endregion

        #region External methods
        [DllImport ("__Internal")]
        private static extern void _AdjustLaunchApp (string appToken, string environment, string sdkPrefix, int logLevel, int eventBuffering, int sendInBackground, int launchDeferredDeeplink, string sceneName);

        [DllImport ("__Internal")]
        private static extern void _AdjustTrackEvent (string eventToken, double revenue, string currency, string receipt, string transactionId, int isReceiptSet, string jsonCallbackParameters, string jsonPartnerParameters);

        [DllImport ("__Internal")]
        private static extern void _AdjustSetEnabled (int enabled);
        
        [DllImport ("__Internal")]
        private static extern int _AdjustIsEnabled ();
        
        [DllImport ("__Internal")]
        private static extern void _AdjustSetOfflineMode (int enabled);

        [DllImport ("__Internal")]
        private static extern void _AdjustSetDeviceToken (string deviceToken);

        [DllImport ("__Internal")]
        private static extern string _AdjustGetIdfa ();
        #endregion

        #region Constructors
        public AdjustiOS ()
        {
        }
        #endregion

        #region Public methods
        public void start (AdjustConfig adjustConfig)
        {
            string appToken = adjustConfig.appToken;
            string sceneName = adjustConfig.sceneName;
            string environment = adjustConfig.environment.lowercaseToString ();

            int logLevel = AdjustUtils.ConvertLogLevel (adjustConfig.logLevel);
            int sendInBackground = AdjustUtils.ConvertBool (adjustConfig.sendInBackground);
            int eventBufferingEnabled = AdjustUtils.ConvertBool (adjustConfig.eventBufferingEnabled);
            int launchDeferredDeeplink = AdjustUtils.ConvertBool (adjustConfig.launchDeferredDeeplink);

            _AdjustLaunchApp (appToken, environment, sdkPrefix, logLevel, eventBufferingEnabled, sendInBackground, launchDeferredDeeplink, sceneName);
        }

        public void trackEvent (AdjustEvent adjustEvent)
        {
            int isReceiptSet = AdjustUtils.ConvertBool (adjustEvent.isReceiptSet);
            double revenue = AdjustUtils.ConvertDouble (adjustEvent.revenue);

            string eventToken = adjustEvent.eventToken;
            string currency = adjustEvent.currency;
            string receipt = adjustEvent.receipt;
            string transactionId = adjustEvent.transactionId;
            string stringJsonCallBackParameters = AdjustUtils.ConvertListToJson (adjustEvent.callbackList);
            string stringJsonPartnerParameters = AdjustUtils.ConvertListToJson (adjustEvent.partnerList);
            
            _AdjustTrackEvent (eventToken, revenue, currency, receipt, transactionId, isReceiptSet, stringJsonCallBackParameters, stringJsonPartnerParameters);
        }

        public void setEnabled (bool enabled)
        {
            _AdjustSetEnabled (AdjustUtils.ConvertBool (enabled));
        }

        public bool isEnabled ()
        {
            var iIsEnabled = _AdjustIsEnabled ();

            return Convert.ToBoolean (iIsEnabled);
        }

        public void setOfflineMode (bool enabled)
        {
            _AdjustSetOfflineMode (AdjustUtils.ConvertBool (enabled));
        }

        // iOS specific methods
        public void setDeviceToken(string deviceToken)
        {
            _AdjustSetDeviceToken (deviceToken);
        }

        public string getIdfa()
        {
            return _AdjustGetIdfa ();
        }

        // Android specific methods
        public void onPause ()
        {
        }

        public void onResume ()
        {
        }

        public void setReferrer(string referrer) 
        {
        }

        public void getGoogleAdId (Action<string> onDeviceIdsRead)
        {
        }
        #endregion
    }
#endif
}
