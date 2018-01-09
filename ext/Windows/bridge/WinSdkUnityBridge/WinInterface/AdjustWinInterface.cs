#if NETFX_CORE
using AdjustSdk;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

#if WIN_INTERFACE_10
namespace Win10Interface
#elif WIN_INTERFACE_81
namespace Win81Interface
#elif WIN_INTERFACE_WS
namespace WinWsInterface
#else
namespace WinInterface
#endif
{
    public class AdjustWinInterface
    {
        public static void ApplicationLaunching(AdjustConfigDto adjustConfigDto)
        {
#if NETFX_CORE
            LogLevel logLevel;
            Enum.TryParse(adjustConfigDto.LogLevelString, out logLevel);

            var config = new AdjustConfig(adjustConfigDto.AppToken, adjustConfigDto.Environment,
                adjustConfigDto.LogDelegate, logLevel)
            {
                DefaultTracker = adjustConfigDto.DefaultTracker,
                SdkPrefix = adjustConfigDto.SdkPrefix,
                SendInBackground = adjustConfigDto.SendInBackground,
                DelayStart = TimeSpan.FromSeconds(adjustConfigDto.DelayStart)
            };

            if (adjustConfigDto.SecretId.HasValue && adjustConfigDto.AppSecretInfo1.HasValue &&
                adjustConfigDto.AppSecretInfo2.HasValue && adjustConfigDto.AppSecretInfo3.HasValue &&
                adjustConfigDto.AppSecretInfo4.HasValue)
            {
                config.SetAppSecret(adjustConfigDto.SecretId.Value,
                    adjustConfigDto.AppSecretInfo1.Value, adjustConfigDto.AppSecretInfo2.Value,
                    adjustConfigDto.AppSecretInfo3.Value, adjustConfigDto.AppSecretInfo4.Value);
            }

            config.SetUserAgent(adjustConfigDto.UserAgent);

            if (adjustConfigDto.IsDeviceKnown.HasValue)
            {
                config.SetDeviceKnown(adjustConfigDto.IsDeviceKnown.Value);
            }

            if (adjustConfigDto.EventBufferingEnabled.HasValue)
            {
                config.EventBufferingEnabled = adjustConfigDto.EventBufferingEnabled.Value;
            }

            if (adjustConfigDto.ActionAttributionChangedData != null)
            {
                config.AttributionChanged = attribution =>
                {
                    var attributionMap = AdjustAttribution
                        .ToDictionary(attribution)
                        // convert from <string, object> to <string, string>
                        .ToDictionary(x => x.Key, x => x.Value.ToString());
                    adjustConfigDto.ActionAttributionChangedData(attributionMap);
                };
            }

            if (adjustConfigDto.ActionSessionSuccessData != null)
            {
                config.SesssionTrackingSucceeded = session =>
                    adjustConfigDto.ActionSessionSuccessData(Util.ToDictionary(session));
            }

            if (adjustConfigDto.ActionSessionFailureData != null)
            {
                config.SesssionTrackingFailed = session =>
                    adjustConfigDto.ActionSessionFailureData(Util.ToDictionary(session));
            }

            if (adjustConfigDto.ActionEventSuccessData != null)
            {
                config.EventTrackingSucceeded = adjustEvent =>
                    adjustConfigDto.ActionEventSuccessData(Util.ToDictionary(adjustEvent));
            }

            if (adjustConfigDto.ActionEventFailureData != null)
            {
                config.EventTrackingFailed = adjustEvent =>
                    adjustConfigDto.ActionEventFailureData(Util.ToDictionary(adjustEvent));
            }

            if (adjustConfigDto.FuncDeeplinkResponseData != null)
            {
                config.DeeplinkResponse = uri =>
                    adjustConfigDto.FuncDeeplinkResponseData(uri.AbsoluteUri);
            }

            Adjust.ApplicationLaunching(config);
#endif
        }

        public static void TrackEvent(string eventToken, double? revenue, string currency,
            string purchaseId, List<string> callbackList, List<string> partnerList)
        {
#if NETFX_CORE
            var adjustEvent = new AdjustEvent(eventToken)
            { PurchaseId = purchaseId };

            if (revenue.HasValue)
            {
                adjustEvent.SetRevenue(revenue.Value, currency);
            }

            if (callbackList != null)
            {
                for (int i = 0; i < callbackList.Count; i += 2)
                {
                    var key = callbackList[i];
                    var value = callbackList[i + 1];

                    adjustEvent.AddCallbackParameter(key, value);
                }
            }

            if (partnerList != null)
            {
                for (int i = 0; i < partnerList.Count; i += 2)
                {
                    var key = partnerList[i];
                    var value = partnerList[i + 1];

                    adjustEvent.AddPartnerParameter(key, value);
                }
            }

            Adjust.TrackEvent(adjustEvent);
#endif
        }

        public static void ApplicationActivated()
        {
#if NETFX_CORE
            Adjust.ApplicationActivated();
#endif
        }

        public static void ApplicationDeactivated()
        {
#if NETFX_CORE
            Adjust.ApplicationDeactivated();
#endif
        }

        public static bool IsEnabled()
        {
#if NETFX_CORE
            return Adjust.IsEnabled();
#else
            return false;
#endif
        }

        public static void SetEnabled(bool enabled)
        {
#if NETFX_CORE
            Adjust.SetEnabled(enabled);
#endif
        }

        public static void SetOfflineMode(bool offlineMode)
        {
#if NETFX_CORE
            Adjust.SetOfflineMode(offlineMode);
#endif
        }

        public static void SendFirstPackages()
        {
#if NETFX_CORE
            Adjust.SendFirstPackages();
#endif
        }

        public static void SetDeviceToken(string deviceToken)
        {
#if NETFX_CORE
            Adjust.SetPushToken(deviceToken);
#endif
        }

        public static Dictionary<string, string> GetAttribution()
        {
#if NETFX_CORE
            AdjustAttribution attribution = Adjust.GetAttributon();
            if (attribution == null)
                return new Dictionary<string, string>();
            
            var adjustAttributionMap = AdjustAttribution.ToDictionary(attribution);
            if (adjustAttributionMap == null)
                return new Dictionary<string, string>();

            // convert from <string, object> to <string, string>
            return adjustAttributionMap.ToDictionary(x => x.Key, x => x.Value.ToString());
#else
            return null;
#endif
        }

        public static string GetWindowsAdId()
        {
#if NETFX_CORE
            return Adjust.GetWindowsAdId();
#else
            return string.Empty;
#endif
        }

        public static string GetAdid()
        {
#if NETFX_CORE
            return Adjust.GetAdid();
#else
            return string.Empty;
#endif
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
#if NETFX_CORE
            Adjust.AddSessionCallbackParameter(key, value);
#endif
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
#if NETFX_CORE
            Adjust.AddSessionPartnerParameter(key, value);
#endif
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
#if NETFX_CORE
            Adjust.RemoveSessionCallbackParameter(key);
#endif
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
#if NETFX_CORE
            Adjust.RemoveSessionPartnerParameter(key);
#endif
        }

        public static void ResetSessionCallbackParameters()
        {
#if NETFX_CORE
            Adjust.ResetSessionCallbackParameters();
#endif
        }

        public static void ResetSessionPartnerParameters()
        {
#if NETFX_CORE
            Adjust.ResetSessionPartnerParameters();
#endif
        }
    }
}
