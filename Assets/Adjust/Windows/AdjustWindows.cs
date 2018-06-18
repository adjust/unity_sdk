#if UNITY_WSA
using com.adjust.sdk.test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WSA_10_0
using Win10Interface;
#elif UNITY_WP_8_1
using Win81Interface;
#elif UNITY_WSA
using WinWsInterface;
#endif

namespace com.adjust.sdk
{
    public class AdjustWindows
    {
        private const string sdkPrefix = "unity4.14.1";
        private static bool appLaunched = false;

        public static void Start(AdjustConfig adjustConfig)
        {
            string logLevelString = null;
            string environment = adjustConfig.environment.ToLowercaseString();

            Action<Dictionary<string, string>> attributionChangedAction = null;
            Action<Dictionary<string, string>> sessionSuccessChangedAction = null;
            Action<Dictionary<string, string>> sessionFailureChangedAction = null;
            Action<Dictionary<string, string>> eventSuccessChangedAction = null;
            Action<Dictionary<string, string>> eventFailureChangedAction = null;
            Func<string, bool> deeplinkResponseFunc = null;

            if (adjustConfig.logLevel.HasValue)
            {
                logLevelString = adjustConfig.logLevel.Value.ToLowercaseString();
            }

            if (adjustConfig.attributionChangedDelegate != null)
            {
                attributionChangedAction = (attributionMap) =>
                {
                    var attribution = new AdjustAttribution(attributionMap);
                    adjustConfig.attributionChangedDelegate(attribution);
                };
            }

            if (adjustConfig.sessionSuccessDelegate != null)
            {
                sessionSuccessChangedAction = (sessionMap) =>
                {
                    var sessionData = new AdjustSessionSuccess(sessionMap);
                    adjustConfig.sessionSuccessDelegate(sessionData);
                };
            }

            if (adjustConfig.sessionFailureDelegate != null)
            {
                sessionFailureChangedAction = (sessionMap) =>
                {
                    var sessionData = new AdjustSessionFailure(sessionMap);
                    adjustConfig.sessionFailureDelegate(sessionData);
                };
            }

            if (adjustConfig.eventSuccessDelegate != null)
            {
                eventSuccessChangedAction = (eventMap) =>
                {
                    var eventData = new AdjustEventSuccess(eventMap);
                    adjustConfig.eventSuccessDelegate(eventData);
                };
            }

            if (adjustConfig.eventFailureDelegate != null)
            {
                eventFailureChangedAction = (eventMap) =>
                {
                    var eventData = new AdjustEventFailure(eventMap);
                    adjustConfig.eventFailureDelegate(eventData);
                };
            }

            if (adjustConfig.deferredDeeplinkDelegate != null)
            {
                deeplinkResponseFunc = uri =>
                {
                    if (adjustConfig.launchDeferredDeeplink)
                    {
                        adjustConfig.deferredDeeplinkDelegate(uri);
                    }
                    
                    return adjustConfig.launchDeferredDeeplink;
                };
            }

            bool sendInBackground = false;
            if (adjustConfig.sendInBackground.HasValue)
            {
                sendInBackground = adjustConfig.sendInBackground.Value;
            }

            double delayStartSeconds = 0;
            if (adjustConfig.delayStart.HasValue)
            {
                delayStartSeconds = adjustConfig.delayStart.Value;
            }

            AdjustConfigDto adjustConfigDto = new AdjustConfigDto {
                AppToken = adjustConfig.appToken,
                Environment = environment,
                SdkPrefix = sdkPrefix,
                SendInBackground = sendInBackground,
                DelayStart = delayStartSeconds,
                UserAgent = adjustConfig.userAgent,
                DefaultTracker = adjustConfig.defaultTracker,
                EventBufferingEnabled = adjustConfig.eventBufferingEnabled,
                LaunchDeferredDeeplink = adjustConfig.launchDeferredDeeplink,
                LogLevelString = logLevelString,
                LogDelegate = adjustConfig.logDelegate,
                ActionAttributionChangedData = attributionChangedAction,
                ActionSessionSuccessData = sessionSuccessChangedAction,
                ActionSessionFailureData = sessionFailureChangedAction,
                ActionEventSuccessData = eventSuccessChangedAction,
                ActionEventFailureData = eventFailureChangedAction,
                FuncDeeplinkResponseData = deeplinkResponseFunc,
                IsDeviceKnown = adjustConfig.isDeviceKnown,
                SecretId = adjustConfig.secretId,
                Info1 = adjustConfig.info1,
                Info2 = adjustConfig.info2,
                Info3 = adjustConfig.info3,
                Info4 = adjustConfig.info4
            };

            AdjustWinInterface.ApplicationLaunching(adjustConfigDto);
            AdjustWinInterface.ApplicationActivated();
            appLaunched = true;
        }

        public static void TrackEvent(AdjustEvent adjustEvent)
        {
            AdjustWinInterface.TrackEvent(
                eventToken: adjustEvent.eventToken,
                revenue: adjustEvent.revenue,
                currency: adjustEvent.currency,
                purchaseId: adjustEvent.transactionId,
                callbackList: adjustEvent.callbackList,
                partnerList: adjustEvent.partnerList
            );
        }

        public static bool IsEnabled()
        {
            return AdjustWinInterface.IsEnabled();
        }

        public static void OnResume()
        {
            if (!appLaunched)
            {
                return;
            }

            AdjustWinInterface.ApplicationActivated();
        }

        public static void OnPause()
        {
            AdjustWinInterface.ApplicationDeactivated();
        }

        public static void SetEnabled(bool enabled)
        {
            AdjustWinInterface.SetEnabled(enabled);
        }

        public static void SetOfflineMode(bool offlineMode)
        {
            AdjustWinInterface.SetOfflineMode(offlineMode);
        }

        public static void SendFirstPackages()
        {
            AdjustWinInterface.SendFirstPackages();
        }

        public static void SetDeviceToken(string deviceToken)
        {
            AdjustWinInterface.SetDeviceToken(deviceToken);
        }

        public static void AppWillOpenUrl(string url)
        {
            AdjustWinInterface.AppWillOpenUrl(url);
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
            AdjustWinInterface.AddSessionPartnerParameter(key, value);
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
            AdjustWinInterface.AddSessionCallbackParameter(key, value);
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
            AdjustWinInterface.RemoveSessionPartnerParameter(key);
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
            AdjustWinInterface.RemoveSessionCallbackParameter(key);
        }

        public static void ResetSessionPartnerParameters()
        {
            AdjustWinInterface.ResetSessionPartnerParameters();
        }

        public static void ResetSessionCallbackParameters()
        {
            AdjustWinInterface.ResetSessionCallbackParameters();
        }

        public static string GetAdid()
        {
            return AdjustWinInterface.GetAdid();
        }

        public static AdjustAttribution GetAttribution()
        {
            var attributionMap = AdjustWinInterface.GetAttribution();
            if (attributionMap == null)
            {
                return new AdjustAttribution();
            }

            return new AdjustAttribution(attributionMap);
        }

        public static void GdprForgetMe()
        {
            AdjustWinInterface.GdprForgetMe();
        }

        public static string GetWinAdId()
        {
            return AdjustWinInterface.GetWindowsAdId();
        }

        public static void SetTestOptions(AdjustTestOptions testOptions)
        {
            TestLibraryInterface.TestLibraryInterface.SetTestOptions(
                new TestLibraryInterface.AdjustTestOptionsDto
                {
                    BasePath = testOptions.BasePath,
                    GdprPath = testOptions.GdprPath,
                    BaseUrl = testOptions.BaseUrl,
                    GdprUrl = testOptions.GdprUrl,
                    DeleteState = testOptions.DeleteState,
                    SessionIntervalInMilliseconds = testOptions.SessionIntervalInMilliseconds,
                    SubsessionIntervalInMilliseconds = testOptions.SubsessionIntervalInMilliseconds,
                    TimerIntervalInMilliseconds = testOptions.TimerIntervalInMilliseconds,
                    TimerStartInMilliseconds = testOptions.TimerStartInMilliseconds,
                    Teardown = testOptions.Teardown
                });
        }
    }
}
#endif
