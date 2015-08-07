using AdjustSdk;
using System;
using System.Collections.Generic;

namespace AdjustUnityWS
{
    public class AdjustWS
    {        
        public static void ApplicationLaunching(string appToken, string environment, string logLevelString, string defaultTracker, bool? eventBufferingEnabled, string sdkPrefix, Action<Dictionary<string, string>> attributionChangedDic)
        {
            LogLevel logLevel;
            if (Enum.TryParse(logLevelString, out logLevel))
            {
                Adjust.SetupLogging(logDelegate: msg => System.Diagnostics.Debug.WriteLine(msg),
                    logLevel: logLevel);
            } 
            else
            {
                Adjust.SetupLogging(logDelegate: msg => System.Diagnostics.Debug.WriteLine(msg));
            }

            var config = new AdjustConfig(appToken, environment);

            config.DefaultTracker = defaultTracker;

            if (eventBufferingEnabled.HasValue)
            {
                config.EventBufferingEnabled = eventBufferingEnabled.Value;
            }

            config.SdkPrefix = sdkPrefix;
            
            if (attributionChangedDic != null)
            {
                config.AttributionChanged = (attribution) => attributionChangedDic(attribution.ToDictionary());
            }
            
            Adjust.ApplicationLaunching(config);
        }

        public static void TrackEvent(string eventToken, double? revenue, string currency, List<string> callbackList, List<string> partnerList)
        {
            var adjustEvent = new AdjustEvent(eventToken);

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
        }
        
        public static void ApplicationActivated()
        {
            Adjust.ApplicationActivated();
        }

        public static void ApplicationDeactivated()
        {
            Adjust.ApplicationDeactivated();
        }

        public static void SetEnabled(bool enabled)
        {
            Adjust.SetEnabled(enabled);
        }

        public static void SetOfflineMode(bool offlineMode)
        {
            Adjust.SetOfflineMode(offlineMode);
        }
        public static bool IsEnabled()
        {
            return Adjust.IsEnabled();
        }
    }
}