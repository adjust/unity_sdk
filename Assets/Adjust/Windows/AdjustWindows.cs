#if UNITY_WSA
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        private const string sdkPrefix = "unity4.29.5";
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
                callbackId: adjustEvent.callbackId,           
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

        public static string GetSdkVersion()
        {
            return sdkPrefix + "@" + AdjustWinInterface.GetSdkVersion();
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

        public static void SetTestOptions(Dictionary<string, string> testOptions)
        {
            string basePath = testOptions.ContainsKey(AdjustUtils.KeyTestOptionsBasePath) ? 
                testOptions[AdjustUtils.KeyTestOptionsBasePath] : null;
            string gdprPath = testOptions.ContainsKey(AdjustUtils.KeyTestOptionsGdprPath) ?
                testOptions[AdjustUtils.KeyTestOptionsGdprPath] : null;
            long timerIntervalMls = -1;
            long timerStartMls = -1;
            long sessionIntMls = -1;
            long subsessionIntMls = -1;
            bool teardown = false;
            bool deleteState = false;
            bool noBackoffWait = false;

            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds))
            {
                timerIntervalMls = long.Parse(testOptions[AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsTimerStartInMilliseconds))
            {
                timerStartMls = long.Parse(testOptions[AdjustUtils.KeyTestOptionsTimerStartInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds))
            {
                sessionIntMls = long.Parse(testOptions[AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds]);
            }
            if (testOptions.ContainsKey(AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds))
            {
                subsessionIntMls = long.Parse(testOptions[AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds]);
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

            Type testLibInterfaceType = Type.GetType("TestLibraryInterface.TestLibraryInterface, TestLibraryInterface");
            Type adjustTestOptionsDtoType = Type.GetType("TestLibraryInterface.AdjustTestOptionsDto, TestLibraryInterface");
            if (testLibInterfaceType == null || adjustTestOptionsDtoType == null)
            {
                return;
            }

            PropertyInfo baseUrlInfo = adjustTestOptionsDtoType.GetProperty("BaseUrl");
            PropertyInfo gdprUrlInfo = adjustTestOptionsDtoType.GetProperty("GdprUrl");
            PropertyInfo basePathInfo = adjustTestOptionsDtoType.GetProperty("BasePath");
            PropertyInfo gdprPathInfo = adjustTestOptionsDtoType.GetProperty("GdprPath");
            PropertyInfo sessionIntervalInMillisecondsInfo = adjustTestOptionsDtoType.GetProperty("SessionIntervalInMilliseconds");
            PropertyInfo subsessionIntervalInMillisecondsInfo = adjustTestOptionsDtoType.GetProperty("SubsessionIntervalInMilliseconds");
            PropertyInfo timerIntervalInMillisecondsInfo = adjustTestOptionsDtoType.GetProperty("TimerIntervalInMilliseconds");
            PropertyInfo timerStartInMillisecondsInfo = adjustTestOptionsDtoType.GetProperty("TimerStartInMilliseconds");
            PropertyInfo deleteStateInfo = adjustTestOptionsDtoType.GetProperty("DeleteState");
            PropertyInfo teardownInfo = adjustTestOptionsDtoType.GetProperty("Teardown");
            PropertyInfo noBackoffWaitInfo = adjustTestOptionsDtoType.GetProperty("NoBackoffWait");

            object adjustTestOptionsDtoInstance = Activator.CreateInstance(adjustTestOptionsDtoType);
            baseUrlInfo.SetValue(adjustTestOptionsDtoInstance, testOptions[AdjustUtils.KeyTestOptionsBaseUrl], null);
            gdprUrlInfo.SetValue(adjustTestOptionsDtoInstance, testOptions[AdjustUtils.KeyTestOptionsGdprUrl], null);
            basePathInfo.SetValue(adjustTestOptionsDtoInstance, basePath, null);
            gdprPathInfo.SetValue(adjustTestOptionsDtoInstance, gdprPath, null);
            sessionIntervalInMillisecondsInfo.SetValue(adjustTestOptionsDtoInstance, sessionIntMls, null);
            subsessionIntervalInMillisecondsInfo.SetValue(adjustTestOptionsDtoInstance, subsessionIntMls, null);
            timerIntervalInMillisecondsInfo.SetValue(adjustTestOptionsDtoInstance, timerIntervalMls, null);
            timerStartInMillisecondsInfo.SetValue(adjustTestOptionsDtoInstance, timerStartMls, null);
            deleteStateInfo.SetValue(adjustTestOptionsDtoInstance, deleteState, null);
            teardownInfo.SetValue(adjustTestOptionsDtoInstance, teardown, null);
            noBackoffWaitInfo.SetValue(adjustTestOptionsDtoInstance, noBackoffWait, null);

            MethodInfo setTestOptionsMethodInfo = testLibInterfaceType.GetMethod("SetTestOptions");
            setTestOptionsMethodInfo.Invoke(null, new object[] { adjustTestOptionsDtoInstance });
        }
    }
}
#endif
