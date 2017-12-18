using System.Collections.Generic;

#if WIN_STUB_10
namespace Win10Interface
#elif WIN_STUB_81
namespace Win81Interface
#elif WIN_STUB_WS
namespace WinWsInterface
#else
namespace WinInterface
#endif
{
    public class AdjustWinInterface
    {
        public static void ApplicationLaunching(AdjustConfigDto adjustConfigDto)
        {
        }

        public static void TrackEvent(string eventToken, double? revenue, string currency,
            string purchaseId, List<string> callbackList, List<string> partnerList)
        {
        }

        public static void ApplicationActivated()
        {
        }

        public static void ApplicationDeactivated()
        {
        }

        public static bool IsEnabled()
        {
            return false;
        }

        public static void SetEnabled(bool enabled)
        {
        }

        public static void SetOfflineMode(bool offlineMode)
        {
        }

        public static void SendFirstPackages()
        {
        }

        public static void SetDeviceToken(string deviceToken)
        {
        }

        public static Dictionary<string, string> GetAttribution()
        {
            return null;
        }

        public static string GetWindowsAdId()
        {
            return string.Empty;
        }

        public static string GetAdid()
        {
            return string.Empty;
        }

        public static void AddSessionCallbackParameter(string key, string value)
        {
        }

        public static void AddSessionPartnerParameter(string key, string value)
        {
        }

        public static void RemoveSessionCallbackParameter(string key)
        {
        }

        public static void RemoveSessionPartnerParameter(string key)
        {
        }

        public static void ResetSessionCallbackParameters()
        {
        }

        public static void ResetSessionPartnerParameters()
        {
        }
    }
}
