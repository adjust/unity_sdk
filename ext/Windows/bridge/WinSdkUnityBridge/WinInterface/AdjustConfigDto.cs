using System;
using System.Collections.Generic;

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
    public class AdjustConfigDto
    {
        public string AppToken;
        public string Environment;
        public string SdkPrefix;
        public bool SendInBackground;
        public double DelayStart;
        public string UserAgent;
        public string DefaultTracker;
        public bool? EventBufferingEnabled;
        public bool LaunchDeferredDeeplink;
        public string LogLevelString;
        public Action<string> LogDelegate;
        public Action<Dictionary<string, string>> ActionAttributionChangedData;
        public Action<Dictionary<string, string>> ActionSessionSuccessData;
        public Action<Dictionary<string, string>> ActionSessionFailureData;
        public Action<Dictionary<string, string>> ActionEventSuccessData;
        public Action<Dictionary<string, string>> ActionEventFailureData;
        public Func<string, bool> FuncDeeplinkResponseData;
    }
}
