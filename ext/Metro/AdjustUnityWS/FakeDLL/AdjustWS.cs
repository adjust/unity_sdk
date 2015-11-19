using System;
using System.Collections.Generic;

namespace AdjustUnityWS
{
    public class AdjustWS
    {
        public static void ApplicationLaunching(string appToken, string environment, string logLevelString, string defaultTracker, bool? eventBufferingEnabled, string sdkPrefix, Action<Dictionary<string, string>> attributionChangedDic) { }

        public static void TrackEvent(string eventToken, double? revenue, string currency, List<string> callbackList, List<string> partnerList) { }

        public static void ApplicationActivated() { }

        public static void ApplicationDeactivated() { }

        public static void SetEnabled(bool enabled) { }

        public static void SetOfflineMode(bool offlineMode) { }

        public static bool IsEnabled()
        {
            return false;
        }
    }
}