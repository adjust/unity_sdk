using System;
using System.Collections.Generic;

namespace AdjustUnityWP
{
    public class AdjustWP
    {
        public static void AppDidLaunch(string appToken, string sEnvironment, string sdkPrefix, string sLogLevel, bool eventBuffering) { }

        public static void AppDidActivate() { }

        public static void AppDidDeactivate() { }

        public static void TrackEvent(string eventToken, Dictionary<string, string> callbackParameters = null) { }

        public static void TrackRevenue(double amountInCents, string eventToken = null, Dictionary<string, string> callbackParameters = null) { }

        public static void SetEnabled(bool enabled) { }

        public static bool IsEnabled()
        {
            return false;
        }

        public static void SetResponseDelegateString(Action<string> responseDelegate) { }
    }
}