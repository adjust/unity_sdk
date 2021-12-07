using System;

namespace com.adjust.sdk
{
    public class AdjustConfig
    {
        public const string AdjustUrlStrategyChina = "china";
        public const string AdjustUrlStrategyIndia = "india";

        public const string AdjustDataResidencyEU = "data-residency-eu";
        public const string AdjustDataResidencyTR = "data-residency-tr";
        public const string AdjustDataResidencyUS = "data-residency-us";

        public const string AdjustAdRevenueSourceAppLovinMAX = "applovin_max_sdk";
        public const string AdjustAdRevenueSourceMopub = "mopub";
        public const string AdjustAdRevenueSourceAdMob = "admob_sdk";
        public const string AdjustAdRevenueSourceIronSource = "ironsource_sdk";
        public const string AdjustAdRevenueSourceAdmost = "admost_sdk";

        internal string appToken;
        internal string sceneName;
        internal string userAgent;
        internal string defaultTracker;
        internal string externalDeviceId;
        internal string urlStrategy;
        internal long? info1;
        internal long? info2;
        internal long? info3;
        internal long? info4;
        internal long? secretId;
        internal double? delayStart;
        internal bool? isDeviceKnown;
        internal bool? sendInBackground;
        internal bool? eventBufferingEnabled;
        internal bool? allowSuppressLogLevel;
        internal bool? needsCost;
        internal bool launchDeferredDeeplink;
        internal AdjustLogLevel? logLevel;
        internal AdjustEnvironment environment;
        internal Action<string> deferredDeeplinkDelegate;
        internal Action<AdjustEventSuccess> eventSuccessDelegate;
        internal Action<AdjustEventFailure> eventFailureDelegate;
        internal Action<AdjustSessionSuccess> sessionSuccessDelegate;
        internal Action<AdjustSessionFailure> sessionFailureDelegate;
        internal Action<AdjustAttribution> attributionChangedDelegate;
        internal Action<int> conversionValueUpdatedDelegate;

        // Android specific members
        internal bool? readImei;
        internal bool? preinstallTrackingEnabled;
        internal string processName;
        // iOS specific members
        internal bool? allowiAdInfoReading;
        internal bool? allowAdServicesInfoReading;
        internal bool? allowIdfaReading;
        internal bool? skAdNetworkHandling;
        // Windows specific members
        internal Action<String> logDelegate;

        public AdjustConfig(string appToken, AdjustEnvironment environment)
        {
            this.sceneName = "";
            this.processName = "";
            this.appToken = appToken;
            this.environment = environment;
        }

        public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
        {
            this.sceneName = "";
            this.processName = "";
            this.appToken = appToken;
            this.environment = environment;
            this.allowSuppressLogLevel = allowSuppressLogLevel;
        }

        public void setLogLevel(AdjustLogLevel logLevel)
        {
            this.logLevel = logLevel;
        }

        public void setDefaultTracker(string defaultTracker)
        {
            this.defaultTracker = defaultTracker;
        }

        public void setExternalDeviceId(string externalDeviceId)
        {
            this.externalDeviceId = externalDeviceId;
        }

        public void setLaunchDeferredDeeplink(bool launchDeferredDeeplink)
        {
            this.launchDeferredDeeplink = launchDeferredDeeplink;
        }

        public void setSendInBackground(bool sendInBackground)
        {
            this.sendInBackground = sendInBackground;
        }

        public void setEventBufferingEnabled(bool eventBufferingEnabled)
        {
            this.eventBufferingEnabled = eventBufferingEnabled;
        }

        public void setNeedsCost(bool needsCost)
        {
            this.needsCost = needsCost;
        }

        public void setDelayStart(double delayStart)
        {
            this.delayStart = delayStart;
        }

        public void setUserAgent(string userAgent)
        {
            this.userAgent = userAgent;
        }

        public void setIsDeviceKnown(bool isDeviceKnown)
        {
            this.isDeviceKnown = isDeviceKnown;
        }

        public void setUrlStrategy(String urlStrategy)
        {
            this.urlStrategy = urlStrategy;
        }

        public void deactivateSKAdNetworkHandling()
        {
            this.skAdNetworkHandling = true;
        }

        public void setDeferredDeeplinkDelegate(Action<string> deferredDeeplinkDelegate, string sceneName = "Adjust")
        {
            this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
            this.sceneName = sceneName;
        }

        public Action<string> getDeferredDeeplinkDelegate()
        {
            return this.deferredDeeplinkDelegate;
        }

        public void setAttributionChangedDelegate(Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
        {
            this.attributionChangedDelegate = attributionChangedDelegate;
            this.sceneName = sceneName;
        }

        public Action<AdjustAttribution> getAttributionChangedDelegate()
        {
            return this.attributionChangedDelegate;
        }

        public void setEventSuccessDelegate(Action<AdjustEventSuccess> eventSuccessDelegate, string sceneName = "Adjust")
        {
            this.eventSuccessDelegate = eventSuccessDelegate;
            this.sceneName = sceneName;
        }

        public Action<AdjustEventSuccess> getEventSuccessDelegate()
        {
            return this.eventSuccessDelegate;
        }

        public void setEventFailureDelegate(Action<AdjustEventFailure> eventFailureDelegate, string sceneName = "Adjust")
        {
            this.eventFailureDelegate = eventFailureDelegate;
            this.sceneName = sceneName;
        }

        public Action<AdjustEventFailure> getEventFailureDelegate()
        {
            return this.eventFailureDelegate;
        }

        public void setSessionSuccessDelegate(Action<AdjustSessionSuccess> sessionSuccessDelegate, string sceneName = "Adjust")
        {
            this.sessionSuccessDelegate = sessionSuccessDelegate;
            this.sceneName = sceneName;
        }

        public Action<AdjustSessionSuccess> getSessionSuccessDelegate()
        {
            return this.sessionSuccessDelegate;
        }

        public void setSessionFailureDelegate(Action<AdjustSessionFailure> sessionFailureDelegate, string sceneName = "Adjust")
        {
            this.sessionFailureDelegate = sessionFailureDelegate;
            this.sceneName = sceneName;
        }

        public Action<AdjustSessionFailure> getSessionFailureDelegate()
        {
            return this.sessionFailureDelegate;
        }

        public void setConversionValueUpdatedDelegate(Action<int> conversionValueUpdatedDelegate, string sceneName = "Adjust")
        {
            this.conversionValueUpdatedDelegate = conversionValueUpdatedDelegate;
            this.sceneName = sceneName;
        }

        public Action<int> getConversionValueUpdatedDelegate()
        {
            return this.conversionValueUpdatedDelegate;
        }

        public void setAppSecret(long secretId, long info1, long info2, long info3, long info4)
        {
            this.secretId = secretId;
            this.info1 = info1;
            this.info2 = info2;
            this.info3 = info3;
            this.info4 = info4;
        }

        // iOS specific methods.
        public void setAllowiAdInfoReading(bool allowiAdInfoReading)
        {
            this.allowiAdInfoReading = allowiAdInfoReading;
        }

        public void setAllowAdServicesInfoReading(bool allowAdServicesInfoReading)
        {
            this.allowAdServicesInfoReading = allowAdServicesInfoReading;
        }

        public void setAllowIdfaReading(bool allowIdfaReading)
        {
            this.allowIdfaReading = allowIdfaReading;
        }

        // Android specific methods.
        public void setProcessName(string processName)
        {
            this.processName = processName;
        }

        [Obsolete("This is an obsolete method.")]
        public void setReadMobileEquipmentIdentity(bool readMobileEquipmentIdentity)
        {
            // this.readImei = readMobileEquipmentIdentity;
        }

        public void setPreinstallTrackingEnabled(bool preinstallTrackingEnabled)
        {
            this.preinstallTrackingEnabled = preinstallTrackingEnabled;
        }

        // Windows specific methods.
        public void setLogDelegate(Action<String> logDelegate)
        {
            this.logDelegate = logDelegate;
        }
    }
}
