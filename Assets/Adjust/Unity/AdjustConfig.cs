using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustConfig
    {
        internal string gameObjectName;
        internal string appToken;
        internal string defaultTracker;
        internal string externalDeviceId;
        internal bool? isSendingInBackgroundEnabled;
        internal bool? isCostDataInAttributionEnabled;
        internal bool? isDeviceIdsReadingOnceEnabled;
        internal bool isDeferredDeeplinkOpeningEnabled;
        internal bool? allowSuppressLogLevel;
        internal bool? isDataResidency;
        internal bool? shouldUseSubdomains;
        internal int? eventDeduplicationIdsMaxSize;
        internal List<string> urlStrategyDomains;
        internal AdjustLogLevel? logLevel;
        internal AdjustEnvironment environment;
        internal Action<AdjustAttribution> attributionChangedDelegate;
        internal Action<AdjustEventSuccess> eventSuccessDelegate;
        internal Action<AdjustEventFailure> eventFailureDelegate;
        internal Action<AdjustSessionSuccess> sessionSuccessDelegate;
        internal Action<AdjustSessionFailure> sessionFailureDelegate;
        internal Action<string> deferredDeeplinkDelegate;
        internal Action<Dictionary<string, string>> skanUpdatedDelegate;

        // iOS specific
        internal bool? isAdServicesEnabled;
        internal bool? isIdfaReadingEnabled;
        internal bool? isSkanAttributionEnabled;
        internal bool? isLinkMeEnabled;
        internal int? attConsentWaitingInterval;

        // Android specific
        internal string processName;
        internal bool? isPreinstallTrackingEnabled;
        internal string preinstallFilePath;
        internal string fbAppId;

        public AdjustConfig(string appToken, AdjustEnvironment environment)
        {
            this.gameObjectName = "";
            this.processName = "";
            this.appToken = appToken;
            this.environment = environment;
        }

        public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
        {
            this.gameObjectName = "";
            this.processName = "";
            this.appToken = appToken;
            this.environment = environment;
            this.allowSuppressLogLevel = allowSuppressLogLevel;
        }

        public void SetLogLevel(AdjustLogLevel logLevel)
        {
            this.logLevel = logLevel;
        }

        public void SetDefaultTracker(string defaultTracker)
        {
            this.defaultTracker = defaultTracker;
        }

        public void SetExternalDeviceId(string externalDeviceId)
        {
            this.externalDeviceId = externalDeviceId;
        }

        public void DisableDeferredDeeplinkOpening()
        {
            this.isDeferredDeeplinkOpeningEnabled = false;
        }

        public void EnableSendingInBackground()
        {
            this.isSendingInBackgroundEnabled = true;
        }

        public void EnableCostDataInAttribution()
        {
            this.isCostDataInAttributionEnabled = true;
        }

        public void SetUrlStrategy(
            List<string> urlStrategyDomains,
            bool shouldUseSubdomains,
            bool isDataResidency)
        {
            this.urlStrategyDomains = urlStrategyDomains;
            this.shouldUseSubdomains = shouldUseSubdomains;
            this.isDataResidency = isDataResidency;
        }

        public void SetDeferredDeeplinkDelegate(
            Action<string> deferredDeeplinkDelegate,
            string gameObjectName = "Adjust")
        {
            this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<string> GetDeferredDeeplinkDelegate()
        {
            return this.deferredDeeplinkDelegate;
        }

        public void SetAttributionChangedDelegate(
            Action<AdjustAttribution> attributionChangedDelegate,
            string gameObjectName = "Adjust")
        {
            this.attributionChangedDelegate = attributionChangedDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<AdjustAttribution> GetAttributionChangedDelegate()
        {
            return this.attributionChangedDelegate;
        }

        public void SetEventSuccessDelegate(
            Action<AdjustEventSuccess> eventSuccessDelegate,
            string gameObjectName = "Adjust")
        {
            this.eventSuccessDelegate = eventSuccessDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<AdjustEventSuccess> GetEventSuccessDelegate()
        {
            return this.eventSuccessDelegate;
        }

        public void SetEventFailureDelegate(
            Action<AdjustEventFailure> eventFailureDelegate,
            string gameObjectName = "Adjust")
        {
            this.eventFailureDelegate = eventFailureDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<AdjustEventFailure> GetEventFailureDelegate()
        {
            return this.eventFailureDelegate;
        }

        public void SetSessionSuccessDelegate(
            Action<AdjustSessionSuccess> sessionSuccessDelegate,
            string gameObjectName = "Adjust")
        {
            this.sessionSuccessDelegate = sessionSuccessDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<AdjustSessionSuccess> GetSessionSuccessDelegate()
        {
            return this.sessionSuccessDelegate;
        }

        public void SetSessionFailureDelegate(
            Action<AdjustSessionFailure> sessionFailureDelegate,
            string gameObjectName = "Adjust")
        {
            this.sessionFailureDelegate = sessionFailureDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<AdjustSessionFailure> GetSessionFailureDelegate()
        {
            return this.sessionFailureDelegate;
        }

        public void DisableAdServices()
        {
            this.isAdServicesEnabled = false;
        }

        public void DisableIdfaReading()
        {
            this.isIdfaReadingEnabled = false;
        }

        public void DisableSkanAttribution()
        {
            this.isSkanAttributionEnabled = false;
        }

        public void EnableLinkMe()
        {
            this.isLinkMeEnabled = true;
        }

        public void SetSkanUpdatedDelegate(
            Action<Dictionary<string, string>> skanUpdatedDelegate,
            string gameObjectName = "Adjust")
        {
            this.skanUpdatedDelegate = skanUpdatedDelegate;
            this.gameObjectName = gameObjectName;
        }

        public Action<Dictionary<string, string>> GetSkanUpdatedDelegate()
        {
            return this.skanUpdatedDelegate;
        }

        public void SetAttConsentWaitingInterval(int numberOfSeconds)
        {
            this.attConsentWaitingInterval = numberOfSeconds;
        }

        public void SetEventDeduplicationIdsMaxSize(int eventDeduplicationIdsMaxSize)
        {
            this.eventDeduplicationIdsMaxSize = eventDeduplicationIdsMaxSize;
        }

        // android specific
        public void EnablePreinstallTracking()
        {
            this.isPreinstallTrackingEnabled = true;
        }

        public void EnableDeviceIdsReadingOnce()
        {
            this.isDeviceIdsReadingOnceEnabled = true;
        }

        public void SetProcessName(string processName)
        {
            this.processName = processName;
        }

        public void SetPreinstallFilePath(string preinstallFilePath)
        {
            this.preinstallFilePath = preinstallFilePath;
        }

        public void SetFbAppId(string fbAppId)
        {
            this.fbAppId = fbAppId;
        }
    }
}
