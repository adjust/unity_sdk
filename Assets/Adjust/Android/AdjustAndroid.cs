using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using com.adjust.sdk.test;

namespace com.adjust.sdk
{
#if UNITY_ANDROID
    public class AdjustAndroid
    {
        private const string sdkPrefix = "unity4.14.1";
        private static bool launchDeferredDeeplink = true;

        private static AndroidJavaClass ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
        // TODO: Check whether currentActivity should be disposed after usage.
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass
            ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        private static DeferredDeeplinkListener onDeferredDeeplinkListener;
        private static AttributionChangeListener onAttributionChangedListener;
        private static EventTrackingFailedListener onEventTrackingFailedListener;
        private static EventTrackingSucceededListener onEventTrackingSucceededListener;
        private static SessionTrackingFailedListener onSessionTrackingFailedListener;
        private static SessionTrackingSucceededListener onSessionTrackingSucceededListener;

        public static void Start(AdjustConfig adjustConfig)
        {
            // Get environment variable.
            AndroidJavaObject ajoEnvironment = adjustConfig.environment == AdjustEnvironment.Sandbox ? 
                new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX") :
                    new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION");

            // Create adjust config object.
            AndroidJavaObject ajoAdjustConfig;

            // Check if suppress log leve is supported.
            if (adjustConfig.allowSuppressLogLevel != null)
            {
                ajoAdjustConfig = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, ajoEnvironment, adjustConfig.allowSuppressLogLevel);
            }
            else
            {
                ajoAdjustConfig = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", ajoCurrentActivity, adjustConfig.appToken, ajoEnvironment);
            }

            // Check if deferred deeplink should be launched by SDK.
            launchDeferredDeeplink = adjustConfig.launchDeferredDeeplink;

            // Check log level.
            if (adjustConfig.logLevel != null)
            {
                AndroidJavaObject ajoLogLevel;

                if (adjustConfig.logLevel.Value.ToUppercaseString().Equals("SUPPRESS"))
                {
                    ajoLogLevel = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
                }
                else
                {
                    ajoLogLevel = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.logLevel.Value.ToUppercaseString());
                }

                if (ajoLogLevel != null)
                {
                    ajoAdjustConfig.Call("setLogLevel", ajoLogLevel);
                }
            }

            // Set unity SDK prefix.
            ajoAdjustConfig.Call("setSdkPrefix", sdkPrefix);

            // Check if user has configured the delayed start option.
            if (adjustConfig.delayStart != null)
            {
                ajoAdjustConfig.Call("setDelayStart", adjustConfig.delayStart);
            }

            // Check event buffering setting.
            if (adjustConfig.eventBufferingEnabled != null)
            {
                AndroidJavaObject ajoIsEnabled = new AndroidJavaObject("java.lang.Boolean", adjustConfig.eventBufferingEnabled.Value);
                ajoAdjustConfig.Call("setEventBufferingEnabled", ajoIsEnabled);
            }

            // Check if user enabled tracking in the background.
            if (adjustConfig.sendInBackground != null)
            {
                ajoAdjustConfig.Call("setSendInBackground", adjustConfig.sendInBackground.Value);
            }

            // Check if user has set user agent value.
            if (adjustConfig.userAgent != null)
            {
                ajoAdjustConfig.Call("setUserAgent", adjustConfig.userAgent);
            }

            // Check if user has set default process name.
            if (!String.IsNullOrEmpty(adjustConfig.processName))
            {
                ajoAdjustConfig.Call("setProcessName", adjustConfig.processName);
            }

            // Check if user has set default tracker token.
            if (adjustConfig.defaultTracker != null)
            {
                ajoAdjustConfig.Call("setDefaultTracker", adjustConfig.defaultTracker);
            }

            // Check if user has set app secret.
            if (IsAppSecretSet(adjustConfig))
            {
                ajoAdjustConfig.Call("setAppSecret",
                    adjustConfig.secretId.Value,
                    adjustConfig.info1.Value,
                    adjustConfig.info2.Value,
                    adjustConfig.info3.Value,
                    adjustConfig.info4.Value);
            }

            // Check if user has set device as known.
            if (adjustConfig.isDeviceKnown.HasValue)
            {
                ajoAdjustConfig.Call("setDeviceKnown", adjustConfig.isDeviceKnown.Value);
            }

            // Check if user has enabled reading of IMEI and MEID.
            if (adjustConfig.readImei.HasValue)
            {
                ajoAdjustConfig.Call("setReadMobileEquipmentIdentity", adjustConfig.readImei.Value);
            }

            // Check attribution changed delagate setting.
            if (adjustConfig.attributionChangedDelegate != null)
            {
                onAttributionChangedListener = new AttributionChangeListener(adjustConfig.attributionChangedDelegate);
                ajoAdjustConfig.Call("setOnAttributionChangedListener", onAttributionChangedListener);
            }

            // Check event success delegate setting.
            if (adjustConfig.eventSuccessDelegate != null)
            {
                onEventTrackingSucceededListener = new EventTrackingSucceededListener(adjustConfig.eventSuccessDelegate);
                ajoAdjustConfig.Call("setOnEventTrackingSucceededListener", onEventTrackingSucceededListener);
            }

            // Check event failure delagate setting.
            if (adjustConfig.eventFailureDelegate != null)
            {
                onEventTrackingFailedListener = new EventTrackingFailedListener(adjustConfig.eventFailureDelegate);
                ajoAdjustConfig.Call("setOnEventTrackingFailedListener", onEventTrackingFailedListener);
            }

            // Check session success delegate setting.
            if (adjustConfig.sessionSuccessDelegate != null)
            {
                onSessionTrackingSucceededListener = new SessionTrackingSucceededListener(adjustConfig.sessionSuccessDelegate);
                ajoAdjustConfig.Call("setOnSessionTrackingSucceededListener", onSessionTrackingSucceededListener);
            }

            // Check session failure delegate setting.
            if (adjustConfig.sessionFailureDelegate != null)
            {
                onSessionTrackingFailedListener = new SessionTrackingFailedListener(adjustConfig.sessionFailureDelegate);
                ajoAdjustConfig.Call("setOnSessionTrackingFailedListener", onSessionTrackingFailedListener);
            }

            // Check deferred deeplink delegate setting.
            if (adjustConfig.deferredDeeplinkDelegate != null)
            {
                onDeferredDeeplinkListener = new DeferredDeeplinkListener(adjustConfig.deferredDeeplinkDelegate);
                ajoAdjustConfig.Call("setOnDeeplinkResponseListener", onDeferredDeeplinkListener);
            }

            // Initialise and start the SDK.
            ajcAdjust.CallStatic("onCreate", ajoAdjustConfig);
            ajcAdjust.CallStatic("onResume");
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            AndroidJavaObject ajoAdjustEvent = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", adjustEvent.eventToken);

            // Check if user has set revenue for the event.
            if (adjustEvent.revenue != null)
            {
                ajoAdjustEvent.Call("setRevenue", (double)adjustEvent.revenue, adjustEvent.currency);
            }

            // Check if user has added any callback parameters to the event.
            if (adjustEvent.callbackList != null)
            {
                for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
                {
                    string key = adjustEvent.callbackList[i];
                    string value = adjustEvent.callbackList[i + 1];
                    ajoAdjustEvent.Call("addCallbackParameter", key, value);
                }
            }

            // Check if user has added any partner parameters to the event.
            if (adjustEvent.partnerList != null)
            {
                for (int i = 0; i < adjustEvent.partnerList.Count; i += 2)
                {
                    string key = adjustEvent.partnerList[i];
                    string value = adjustEvent.partnerList[i + 1];
                    ajoAdjustEvent.Call("addPartnerParameter", key, value);
                }
            }

            // Check if user has added transaction ID to the event.
            if (adjustEvent.transactionId != null)
            {
                ajoAdjustEvent.Call("setOrderId", adjustEvent.transactionId);
            }

            // Track the event.
            ajcAdjust.CallStatic("trackEvent", ajoAdjustEvent);
        }

        public static bool IsEnabled()
        {
            return ajcAdjust.CallStatic<bool>("isEnabled");
        }

        public static void SetEnabled(bool enabled)
        {
            ajcAdjust.CallStatic("setEnabled", enabled);
        }

        public static void SetOfflineMode(bool enabled)
        {
            ajcAdjust.CallStatic("setOfflineMode", enabled);
        }

        public static void SendFirstPackages()
        {
            ajcAdjust.CallStatic("sendFirstPackages");
        }

        public static void SetDeviceToken(string deviceToken)
        {
            ajcAdjust.CallStatic("setPushToken", deviceToken, ajoCurrentActivity);
        }

        public static string GetAdid()
        {
            return ajcAdjust.CallStatic<string>("getAdid");
        }

        public static void GdprForgetMe()
        {
            ajcAdjust.CallStatic("gdprForgetMe", ajoCurrentActivity);
        }

        public static AdjustAttribution GetAttribution()
        {
            try
            {
                AndroidJavaObject ajoAttribution = ajcAdjust.CallStatic<AndroidJavaObject>("getAttribution");
                if (null == ajoAttribution)
                {
                    return null;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                adjustAttribution.trackerName = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.trackerToken = ajoAttribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.network = ajoAttribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.campaign = ajoAttribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.adgroup = ajoAttribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.creative = ajoAttribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.clickLabel = ajoAttribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.adid = ajoAttribution.Get<string>(AdjustUtils.KeyAdid);
                return adjustAttribution;
            }
            catch (Exception) {}

            return null;
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("addSessionPartnerParameter", key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("addSessionCallbackParameter", key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeSessionPartnerParameter", key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("removeSessionCallbackParameter", key);
        }

        public static void ResetSessionPartnerParameters()
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("resetSessionPartnerParameters");
        }

        public static void ResetSessionCallbackParameters()
        {
            if (ajcAdjust == null)
            {
                ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
            }
            ajcAdjust.CallStatic("resetSessionCallbackParameters");
        }

        public static void AppWillOpenUrl(string url) 
        {
            AndroidJavaClass ajcUri = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject ajoUri = ajcUri.CallStatic<AndroidJavaObject>("parse", url);
            ajcAdjust.CallStatic("appWillOpenUrl", ajoUri, ajoCurrentActivity);
        }

        // Android specific methods.
        public static void OnPause()
        {
            ajcAdjust.CallStatic("onPause");
        }
        
        public static void OnResume()
        {
            ajcAdjust.CallStatic("onResume");
        }

        public static void SetReferrer(string referrer)
        {
            ajcAdjust.CallStatic("setReferrer", referrer, ajoCurrentActivity);
        }

        public static void GetGoogleAdId(Action<string> onDeviceIdsRead) 
        {
            DeviceIdsReadListener onDeviceIdsReadProxy = new DeviceIdsReadListener(onDeviceIdsRead);
            ajcAdjust.CallStatic("getGoogleAdId", ajoCurrentActivity, onDeviceIdsReadProxy);
        }

        public static string GetAmazonAdId()
        {
            return ajcAdjust.CallStatic<string>("getAmazonAdId", ajoCurrentActivity);
        }

        // Used for testing only.
        public static void SetTestOptions(AdjustTestOptions testOptions)
        {
            AndroidJavaObject ajoTestOptions = testOptions.ToAndroidJavaObject(ajoCurrentActivity);
            ajcAdjust.CallStatic("setTestOptions", ajoTestOptions);
        }

        // Private & helper classes.
        private class AttributionChangeListener : AndroidJavaProxy
        {
            private Action<AdjustAttribution> callback;

            public AttributionChangeListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public void onAttributionChanged(AndroidJavaObject attribution)
            {
                if (callback == null)
                {
                    return;
                }

                AdjustAttribution adjustAttribution = new AdjustAttribution();
                adjustAttribution.trackerName = attribution.Get<string>(AdjustUtils.KeyTrackerName);
                adjustAttribution.trackerToken = attribution.Get<string>(AdjustUtils.KeyTrackerToken);
                adjustAttribution.network = attribution.Get<string>(AdjustUtils.KeyNetwork);
                adjustAttribution.campaign = attribution.Get<string>(AdjustUtils.KeyCampaign);
                adjustAttribution.adgroup = attribution.Get<string>(AdjustUtils.KeyAdgroup);
                adjustAttribution.creative = attribution.Get<string>(AdjustUtils.KeyCreative);
                adjustAttribution.clickLabel = attribution.Get<string>(AdjustUtils.KeyClickLabel);
                adjustAttribution.adid = attribution.Get<string>(AdjustUtils.KeyAdid);
                callback(adjustAttribution);
            }
        }

        private class DeferredDeeplinkListener : AndroidJavaProxy
        {
            private Action<string> callback;

            public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResponseListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
            public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
            {
                if (callback == null)
                {
                    return launchDeferredDeeplink;
                }

                string deeplinkURL = deeplink.Call<string>("toString");
                callback(deeplinkURL);
                return launchDeferredDeeplink;
            }
        }

        private class EventTrackingSucceededListener : AndroidJavaProxy
        {
            private Action<AdjustEventSuccess> callback;

            public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback) : base("com.adjust.sdk.OnEventTrackingSucceededListener")
            {
                this.callback = pCallback;
            }

            // Method must be lowercase to match Android method signature.
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
                adjustEventSuccess.Adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventSuccess.Message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventSuccess.Timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventSuccess.EventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
                try
                {
                    AndroidJavaObject ajoJsonResponse = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Should not be happening as of v4.12.5.
                    // Native Android SDK should send empty JSON object if none available.
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

            // Method must be lowercase to match Android method signature.
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
                adjustEventFailure.Adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustEventFailure.Message = eventFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                adjustEventFailure.Timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
                adjustEventFailure.EventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
                try
                {
                    AndroidJavaObject ajoJsonResponse = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Should not be happening as of v4.12.5.
                    // Native Android SDK should send empty JSON object if none available.
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

            // Method must be lowercase to match Android method signature.
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
                adjustSessionSuccess.Adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionSuccess.Message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Should not be happening as of v4.12.5.
                    // Native Android SDK should send empty JSON object if none available.
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

            // Method must be lowercase to match Android method signature.
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
                adjustSessionFailure.Adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
                adjustSessionFailure.Message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
                adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
                adjustSessionFailure.Timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);
                try
                {
                    AndroidJavaObject ajoJsonResponse = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
                    string jsonResponseString = ajoJsonResponse.Call<string>("toString");
                    adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
                }
                catch (Exception)
                {
                    // JSON response reading failed.
                    // Should not be happening as of v4.12.5.
                    // Native Android SDK should send empty JSON object if none available.
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

            // Method must be lowercase to match Android method signature.
            public void onGoogleAdIdRead(string playAdId)
            {
                if (onPlayAdIdReadCallback == null)
                {
                    return;
                }

                this.onPlayAdIdReadCallback(playAdId);
            }

            // Handling of null object.
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

        // Private & helper methods.
        private static bool IsAppSecretSet(AdjustConfig adjustConfig)
        {
            return adjustConfig.secretId.HasValue 
            && adjustConfig.info1.HasValue
            && adjustConfig.info2.HasValue
            && adjustConfig.info3.HasValue
            && adjustConfig.info4.HasValue;
        }
    }
#endif
}
