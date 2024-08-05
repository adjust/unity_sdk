using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdjustSdk.Test
{
    public class CommandExecutor
    {
        public string ExtraPath { get; set; }

        private Dictionary<int, AdjustConfig> _savedConfigs = new Dictionary<int, AdjustConfig>();
        private Dictionary<int, AdjustEvent> _savedEvents = new Dictionary<int, AdjustEvent>();
        private string _overwriteUrl;
        private Command _command;
        private ITestLibrary _testLibrary;

        public CommandExecutor(ITestLibrary testLibrary, string overwriteUrl)
        {
            _overwriteUrl = overwriteUrl;
            _testLibrary = testLibrary;
        }
            
        public void ExecuteCommand(string className, string methodName, Dictionary<string, List<string>> parameters)
        {
            this.ExecuteCommand(new Command(className, methodName, parameters));
        }

        public void ExecuteCommand(Command command)
        {
            _command = command;

            TestApp.Log(string.Format("\tEXECUTING METHOD: [{0}.{1}]", _command.ClassName, _command.MethodName));

            try
            {
                switch (_command.MethodName)
                {
                    case "testOptions": TestOptions(); break;
                    case "config": Config(); break;
                    case "start": Start(); break;
                    case "event": Event(); break;
                    case "trackEvent": TrackEvent(); break;
                    case "resume": Resume(); break;
                    case "pause": Pause(); break;
                    case "setEnabled": SetEnabled(); break;
                    case "setOfflineMode": SetOfflineMode(); break;
                    case "addGlobalCallbackParameter": AddGlobalCallbackParameter(); break;
                    case "addGlobalPartnerParameter": AddGlobalPartnerParameter(); break;
                    case "removeGlobalCallbackParameter": RemoveGlobalCallbackParameter(); break;
                    case "removeGlobalPartnerParameter": RemoveGlobalPartnerParameter(); break;
                    case "removeGlobalCallbackParameters": RemoveGlobalCallbackParameters(); break;
                    case "removeGlobalPartnerParameters": RemoveGlobalPartnerParameters(); break;
                    case "setPushToken": SetPushToken(); break;
                    case "openDeeplink": OpenDeepLink(); break;
                    case "gdprForgetMe": GdprForgetMe(); break;
                    case "trackSubscription": TrackSubscription(); break;
                    case "thirdPartySharing": ThirdPartySharing(); break;
                    case "measurementConsent": MeasurementConsent(); break;
                    case "trackAdRevenue": TrackAdRevenue(); break;
                    case "getLastDeeplink": GetLastDeeplink(); break;
                    case "verifyPurchase": VerifyPurchase(); break;
                    case "processDeeplink": ProcessAndResolveDeeplink(); break;
                    case "attributionGetter": AttributionGetter(); break;
                    case "enablePlayStoreKidsApp" : EnablePlayStoreKidsApp(); break;
                    case "disablePlayStoreKidsApp" : DisablePlayStoreKidsApp(); break;
                    default: CommandNotFound(_command.ClassName, _command.MethodName); break;
                }
            }
            catch (Exception ex)
            {
                TestApp.LogError(string.Format("{0} -- {1}",
                    "executeCommand: failed to parse command. Check commands' syntax", ex.ToString()));
            }
        }

        private void TestOptions()
        {
            Dictionary<string, string> testOptions = new Dictionary<string, string>();
            testOptions[AdjustUtils.KeyTestOptionsBaseUrl] = _overwriteUrl;
            testOptions[AdjustUtils.KeyTestOptionsGdprUrl] = _overwriteUrl;
            testOptions[AdjustUtils.KeyTestOptionsSubscriptionUrl] = _overwriteUrl;
            testOptions[AdjustUtils.KeyTestOptionsPurchaseVerificationUrl] = _overwriteUrl;
            testOptions[AdjustUtils.KeyTestOptionsOverwriteUrl] = _overwriteUrl;

            if (_command.ContainsParameter("basePath"))
            {
                ExtraPath = _command.GetFirstParameterValue("basePath");
            }
            if (_command.ContainsParameter("timerInterval"))
            {
                testOptions[AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds] = _command.GetFirstParameterValue("timerInterval");
            }
            if (_command.ContainsParameter("timerStart"))
            {
                testOptions[AdjustUtils.KeyTestOptionsTimerStartInMilliseconds] = _command.GetFirstParameterValue("timerStart");
            }
            if (_command.ContainsParameter("sessionInterval"))
            {
                testOptions[AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds] = _command.GetFirstParameterValue("sessionInterval");
            }
            if (_command.ContainsParameter("subsessionInterval"))
            {
                testOptions[AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds] = _command.GetFirstParameterValue("subsessionInterval");
            }
            if (_command.ContainsParameter("noBackoffWait"))
            {
                testOptions[AdjustUtils.KeyTestOptionsNoBackoffWait] = _command.GetFirstParameterValue("noBackoffWait");
            }
            // AdServices.framework will not be used in test app by default
            testOptions [AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled] = "false";
            if (_command.ContainsParameter("adServicesFrameworkEnabled"))
            {
                testOptions[AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled] = _command.GetFirstParameterValue("adServicesFrameworkEnabled");
            }
            if (_command.ContainsParameter("attStatus"))
            {
                testOptions[AdjustUtils.KeyTestOptionsAttStatus] = _command.GetFirstParameterValue("attStatus");
            }
            if (_command.ContainsParameter("idfa"))
            {
                testOptions[AdjustUtils.KeyTestOptionsIdfa] = _command.GetFirstParameterValue("idfa");
            }
            if (_command.ContainsParameter("doNotIgnoreSystemLifecycleBootstrap"))
            {
                string strDoNotIgnoreSystemLifecycleBootstrap = _command.GetFirstParameterValue("doNotIgnoreSystemLifecycleBootstrap");
                if (strDoNotIgnoreSystemLifecycleBootstrap != null)
                {
                    bool bDoNotIgnoreSystemLifecycleBootstrap = bool.Parse(strDoNotIgnoreSystemLifecycleBootstrap);
                    if (bDoNotIgnoreSystemLifecycleBootstrap == true)
                    {
                        testOptions[AdjustUtils.KeyTestOptionsIgnoreSystemLifecycleBootstrap] = "false";
                    }
                }
            }
#if UNITY_ANDROID
            bool useTestConnectionOptions = false;
#endif
            if (_command.ContainsParameter("teardown"))
            {
                List<string> teardownOptions = _command.Parameters["teardown"];
                foreach (string teardownOption in teardownOptions)
                {
                    if (teardownOption == "resetSdk")
                    {
                        testOptions[AdjustUtils.KeyTestOptionsTeardown] = "true";
                        testOptions[AdjustUtils.KeyTestOptionsExtraPath] = ExtraPath;
#if UNITY_IOS
                        testOptions[AdjustUtils.KeyTestOptionsUseTestConnectionOptions] = "true";
#endif
#if UNITY_ANDROID
                        useTestConnectionOptions = true;
#endif
                    }
                    if (teardownOption == "deleteState")
                    {
                        testOptions[AdjustUtils.KeyTestOptionsDeleteState] = "true";
                    }
                    if (teardownOption == "resetTest")
                    {
                        _savedEvents = new Dictionary<int, AdjustEvent>();
                        _savedConfigs = new Dictionary<int, AdjustConfig>();
                        testOptions[AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsTimerStartInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds] = "-1";
                    }
                    if (teardownOption == "sdk")
                    {
                        testOptions[AdjustUtils.KeyTestOptionsTeardown] = "true";
                        testOptions[AdjustUtils.KeyTestOptionsExtraPath] = null;
#if UNITY_IOS
                        testOptions[AdjustUtils.KeyTestOptionsUseTestConnectionOptions] = "false";
#endif
                    }
                    if (teardownOption == "test")
                    {
                        _savedEvents = null;
                        _savedConfigs = null;
                        testOptions[AdjustUtils.KeyTestOptionsTimerIntervalInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsTimerStartInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsSessionIntervalInMilliseconds] = "-1";
                        testOptions[AdjustUtils.KeyTestOptionsSubsessionIntervalInMilliseconds] = "-1";
                    }
                }
            }

            Adjust.SetTestOptions(testOptions);
#if UNITY_ANDROID
            if (useTestConnectionOptions)
            {
                TestConnectionOptions.SetTestConnectionOptions();
            }
#endif
        }

        private void Config()
        {
            var configNumber = 0;
            if (_command.ContainsParameter("configName"))
            {
                var configName = _command.GetFirstParameterValue("configName");
                configNumber = int.Parse(configName.Substring(configName.Length - 1));
            }

            AdjustConfig adjustConfig;
            AdjustLogLevel? logLevel = null;
            if (_command.ContainsParameter("logLevel"))
            {
                var logLevelString = _command.GetFirstParameterValue("logLevel");
                switch (logLevelString)
                {
                    case "verbose":
                        logLevel = AdjustLogLevel.Verbose;
                        break;
                    case "debug":
                        logLevel = AdjustLogLevel.Debug;
                        break;
                    case "info":
                        logLevel = AdjustLogLevel.Info;
                        break;
                    case "warn":
                        logLevel = AdjustLogLevel.Warn;
                        break;
                    case "error":
                        logLevel = AdjustLogLevel.Error;
                        break;
                    case "assert":
                        logLevel = AdjustLogLevel.Assert;
                        break;
                    case "suppress":
                        logLevel = AdjustLogLevel.Suppress;
                        break;
                }

                TestApp.Log(string.Format("TestApp LogLevel = {0}", logLevel));
            }

            if (_savedConfigs.ContainsKey(configNumber))
            {
                adjustConfig = _savedConfigs[configNumber];
            }
            else
            {
                var environmentString = _command.GetFirstParameterValue("environment");
                var environment = environmentString == "sandbox" ? AdjustEnvironment.Sandbox : AdjustEnvironment.Production;
                var appToken = _command.GetFirstParameterValue("appToken");

                if (!string.IsNullOrEmpty(appToken))
                {
                    if (appToken == "null")
                    {
                        adjustConfig = new AdjustConfig(null, environment);
                    }
                    else
                    {
                        adjustConfig = new AdjustConfig(appToken, environment);
                    }
                }
                else
                {
                    adjustConfig = new AdjustConfig(null, environment);
                }

                if (logLevel.HasValue)
                {
                    adjustConfig.LogLevel = logLevel.Value;
                }

                _savedConfigs.Add(configNumber, adjustConfig);
            }

            if (_command.ContainsParameter("sdkPrefix"))
            {
                // SDK prefix not tested for non natives.
            }

            if (_command.ContainsParameter("defaultTracker"))
            {
                adjustConfig.DefaultTracker = _command.GetFirstParameterValue("defaultTracker");
            }

            if (_command.ContainsParameter("externalDeviceId"))
            {
                adjustConfig.ExternalDeviceId = _command.GetFirstParameterValue("externalDeviceId");
            }

            if (_command.ContainsParameter("sendInBackground"))
            {
                var sendInBackgroundS = _command.GetFirstParameterValue("sendInBackground");
                var sendInBackground = sendInBackgroundS.ToLower() == "true";
                adjustConfig.IsSendingInBackgroundEnabled = sendInBackground;
            }

            if (_command.ContainsParameter("coppaCompliant"))
            {
                var coppaCompliantS = _command.GetFirstParameterValue("coppaCompliant");
                var coppaCompliant = coppaCompliantS.ToLower() == "true";
                adjustConfig.IsCoppaComplianceEnabled = coppaCompliant;
            }

            if (_command.ContainsParameter("allowAdServicesInfoReading"))
            {
                var allowAdServicesInfoReadingS = _command.GetFirstParameterValue("allowAdServicesInfoReading");
                var allowAdServicesInfoReading = allowAdServicesInfoReadingS.ToLower() == "true";
                adjustConfig.IsAdServicesEnabled = allowAdServicesInfoReading;
            }

            if (_command.ContainsParameter("allowIdfaReading"))
            {
                var allowIdfaReadingS = _command.GetFirstParameterValue("allowIdfaReading");
                var allowIdfaReading = allowIdfaReadingS.ToLower() == "true";
                adjustConfig.IsIdfaReadingEnabled = allowIdfaReading;
            }

            if (_command.ContainsParameter("allowSkAdNetworkHandling"))
            {
                var allowSkAdNetworkHandlingS = _command.GetFirstParameterValue("allowSkAdNetworkHandling");
                var allowSkAdNetworkHandling = allowSkAdNetworkHandlingS.ToLower() == "true";
                adjustConfig.IsSkanAttributionEnabled = allowSkAdNetworkHandling;
            }

            if (_command.ContainsParameter("attConsentWaitingSeconds"))
            {
                var attConsentWaitingSecondsStr = _command.GetFirstParameterValue("attConsentWaitingSeconds");
                var attConsentWaitingSeconds = int.Parse(attConsentWaitingSecondsStr, System.Globalization.CultureInfo.InvariantCulture);
                adjustConfig.AttConsentWaitingInterval = attConsentWaitingSeconds;
            }

            if (_command.ContainsParameter("eventDeduplicationIdsMaxSize"))
            {
                var eventDeduplicationIdsMaxSizeStr = _command.GetFirstParameterValue("eventDeduplicationIdsMaxSize");
                var eventDeduplicationIdsMaxSize = int.Parse(eventDeduplicationIdsMaxSizeStr, System.Globalization.CultureInfo.InvariantCulture);
                adjustConfig.EventDeduplicationIdsMaxSize = eventDeduplicationIdsMaxSize;
            }

            if (_command.ContainsParameter("deferredDeeplinkCallback"))
            {
                bool launchDeferredDeeplink = _command.GetFirstParameterValue("deferredDeeplinkCallback") == "true";
                adjustConfig.IsDeferredDeeplinkOpeningEnabled = launchDeferredDeeplink;

                string localExtraPath = ExtraPath;
                adjustConfig.DeferredDeeplinkDelegate = (uri =>
                {
                    _testLibrary.AddInfoToSend("deeplink", uri);
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("attributionCallbackSendAll"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.AttributionChangedDelegate = (attribution =>
                {
                    _testLibrary.AddInfoToSend("tracker_token", attribution.TrackerToken);
                    _testLibrary.AddInfoToSend("tracker_name", attribution.TrackerName);
                    _testLibrary.AddInfoToSend("network", attribution.Network);
                    _testLibrary.AddInfoToSend("campaign", attribution.Campaign);
                    _testLibrary.AddInfoToSend("adgroup", attribution.Adgroup);
                    _testLibrary.AddInfoToSend("creative", attribution.Creative);
                    _testLibrary.AddInfoToSend("click_label", attribution.ClickLabel);
                    _testLibrary.AddInfoToSend("cost_type", attribution.CostType);
                    _testLibrary.AddInfoToSend("cost_amount", attribution.CostAmount.ToString());
                    _testLibrary.AddInfoToSend("cost_currency", attribution.CostCurrency);
                    _testLibrary.AddInfoToSend("fb_install_referrer", attribution.FbInstallReferrer);
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.SessionSuccessDelegate = (sessionSuccessResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", sessionSuccessResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", sessionSuccessResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", sessionSuccessResponseData.Adid);
                    if (sessionSuccessResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", sessionSuccessResponseData.GetJsonResponseAsString());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendFailure"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.SessionFailureDelegate = (sessionFailureResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", sessionFailureResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", sessionFailureResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", sessionFailureResponseData.Adid);
                    _testLibrary.AddInfoToSend("willRetry", sessionFailureResponseData.WillRetry.ToString().ToLower());
                    if (sessionFailureResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", sessionFailureResponseData.GetJsonResponseAsString());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendSuccess"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.EventSuccessDelegate = (eventSuccessResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", eventSuccessResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", eventSuccessResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", eventSuccessResponseData.Adid);
                    _testLibrary.AddInfoToSend("eventToken", eventSuccessResponseData.EventToken);
                    _testLibrary.AddInfoToSend("callbackId", eventSuccessResponseData.CallbackId);
                    if (eventSuccessResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", eventSuccessResponseData.GetJsonResponseAsString());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendFailure"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.EventFailureDelegate = (eventFailureResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", eventFailureResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", eventFailureResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", eventFailureResponseData.Adid);
                    _testLibrary.AddInfoToSend("eventToken", eventFailureResponseData.EventToken);
                    _testLibrary.AddInfoToSend("callbackId", eventFailureResponseData.CallbackId);
                    _testLibrary.AddInfoToSend("willRetry", eventFailureResponseData.WillRetry.ToString().ToLower());
                    if (eventFailureResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", eventFailureResponseData.GetJsonResponseAsString());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("skanCallback"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.SkanUpdatedDelegate = (skanUpdatedData =>
                {
                    foreach (KeyValuePair<string, string> entry in skanUpdatedData)
                    {
                        _testLibrary.AddInfoToSend(entry.Key, entry.Value);
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }
        }

        private void Start()
        {
            Config();

            var configNumber = 0;
            if (_command.ContainsParameter("configName"))
            {
                var configName = _command.GetFirstParameterValue("configName");
                configNumber = int.Parse(configName.Substring(configName.Length - 1));
            }

            var adjustConfig = _savedConfigs[configNumber];
            Adjust.InitSdk(adjustConfig);
            _savedConfigs.Remove(0);
        }

        private void Event()
        {
            var eventNumber = 0;
            if (_command.ContainsParameter("eventName"))
            {
                var eventName = _command.GetFirstParameterValue("eventName");
                eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
            }

            AdjustEvent adjustEvent = null;
            if (_savedEvents.ContainsKey(eventNumber))
            {
                adjustEvent = _savedEvents[eventNumber];
            }
            else
            {
                var eventToken = _command.GetFirstParameterValue("eventToken");
                adjustEvent = new AdjustEvent(eventToken);
                _savedEvents.Add(eventNumber, adjustEvent);
            }

            if (_command.ContainsParameter("revenue"))
            {
                var revenueParams = _command.Parameters["revenue"];
                var currency = revenueParams[0];
                var revenue = double.Parse(revenueParams[1], System.Globalization.CultureInfo.InvariantCulture);
                adjustEvent.SetRevenue(revenue, currency);
            }

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    adjustEvent.AddCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    adjustEvent.AddPartnerParameter(key, value);
                }
            }

            if (_command.ContainsParameter("callbackId"))
            {
                var callbackId = _command.GetFirstParameterValue("callbackId");
                adjustEvent.CallbackId = callbackId;
            }

            if (_command.ContainsParameter("transactionId"))
            {
                var transactionId = _command.GetFirstParameterValue("transactionId");
                adjustEvent.TransactionId = transactionId;
            }

            if (_command.ContainsParameter("productId"))
            {
                var productId = _command.GetFirstParameterValue("productId");
                adjustEvent.ProductId = productId;
            }

            if (_command.ContainsParameter("receipt"))
            {
                var receipt = _command.GetFirstParameterValue("receipt");
                adjustEvent.Receipt = receipt;
            }

            if (_command.ContainsParameter("purchaseToken"))
            {
                var purchaseToken = _command.GetFirstParameterValue("purchaseToken");
                adjustEvent.PurchaseToken = purchaseToken;
            }

            if (_command.ContainsParameter("deduplicationId"))
            {
                var deduplicationId = _command.GetFirstParameterValue("deduplicationId");
                adjustEvent.DeduplicationId = deduplicationId;
            }
        }

        private void TrackEvent()
        {
            Event();

            var eventNumber = 0;
            if (_command.ContainsParameter("eventName"))
            {
                var eventName = _command.GetFirstParameterValue("eventName");
                eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
            }

            var adjustEvent = _savedEvents[eventNumber];
            Adjust.TrackEvent(adjustEvent);
            _savedEvents.Remove(0);
        }

        private void Resume()
        {
#if UNITY_IOS
            AdjustiOS.TrackSubsessionStart("test");
#elif UNITY_ANDROID
            AdjustAndroid.OnResume("test");
#else
            Debug.Log("TestApp - Command Executor - Error! Cannot Resume. None of the supported platforms selected.");
#endif
        }

        private void Pause()
        {
#if UNITY_IOS
            AdjustiOS.TrackSubsessionEnd("test");
#elif UNITY_ANDROID
            AdjustAndroid.OnPause("test");
#else
            Debug.Log("TestApp - Command Executor - Error! Cannot Pause. None of the supported platforms selected.");
#endif
        }

        private void SetEnabled()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("enabled"));
            if (enabled == true)
            {
                Adjust.Enable();
            }
            else
            {
                Adjust.Disable();
            }
        }

        public void GdprForgetMe()
        {
            Adjust.GdprForgetMe();
        }

        private void SetOfflineMode()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("enabled"));
            if (enabled == true)
            {
                Adjust.SwitchToOfflineMode();
            }
            else
            {
                Adjust.SwitchBackToOnlineMode();
            }
        }

        private void AddGlobalCallbackParameter()
        {
            if (!_command.ContainsParameter("KeyValue"))
            {
                return;
            }

            var keyValuePairs = _command.Parameters["KeyValue"];
            for (var i = 0; i < keyValuePairs.Count; i = i + 2)
            {
                var key = keyValuePairs[i];
                var value = keyValuePairs[i + 1];
                Adjust.AddGlobalCallbackParameter(key, value);
            }
        }

        private void AddGlobalPartnerParameter()
        {
            if (!_command.ContainsParameter("KeyValue"))
            {
                return;
            }

            var keyValuePairs = _command.Parameters["KeyValue"];
            for (var i = 0; i < keyValuePairs.Count; i = i + 2)
            {
                var key = keyValuePairs[i];
                var value = keyValuePairs[i + 1];
                Adjust.AddGlobalPartnerParameter(key, value);
            }
        }

        private void RemoveGlobalCallbackParameter()
        {
            if (!_command.ContainsParameter("key"))
            {
                return;
            }

            var keys = _command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.RemoveGlobalCallbackParameter(key);
            }
        }

        private void RemoveGlobalPartnerParameter()
        {
            if (!_command.ContainsParameter("key"))
            {
                return;
            }

            var keys = _command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.RemoveGlobalPartnerParameter(key);
            }
        }

        private void RemoveGlobalCallbackParameters()
        {
            Adjust.RemoveGlobalCallbackParameters();
        }

        private void RemoveGlobalPartnerParameters()
        {
            Adjust.RemoveGlobalPartnerParameters();
        }

        private void SetPushToken()
        {
            var pushToken = _command.GetFirstParameterValue("pushToken");

            if (!string.IsNullOrEmpty(pushToken))
            {
                if (pushToken.Equals("null"))
                {
                    Adjust.SetPushToken(null);
                }
                else
                {
                    Adjust.SetPushToken(pushToken);
                }
            }
        }

        private void OpenDeepLink()
        {
            var deeplink = _command.GetFirstParameterValue("deeplink");
            AdjustDeeplink adjustDeeplink = new AdjustDeeplink(deeplink);
            Adjust.ProcessDeeplink(adjustDeeplink);
        }

        private void TrackSubscription()
        {
#if UNITY_IOS
            string price = _command.GetFirstParameterValue("revenue");
            string currency = _command.GetFirstParameterValue("currency");
            string transactionId = _command.GetFirstParameterValue("transactionId");
            string receipt = _command.GetFirstParameterValue("receipt");
            string transactionDate = _command.GetFirstParameterValue("transactionDate");
            string salesRegion = _command.GetFirstParameterValue("salesRegion");

            AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
                price,
                currency,
                transactionId,
                receipt);
            subscription.TransactionDate = transactionDate;
            subscription.SalesRegion = salesRegion;

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    subscription.AddCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    subscription.AddPartnerParameter(key, value);
                }
            }

            Adjust.TrackAppStoreSubscription(subscription);
#elif UNITY_ANDROID
            string price = _command.GetFirstParameterValue("revenue");
            string currency = _command.GetFirstParameterValue("currency");
            string purchaseTime = _command.GetFirstParameterValue("transactionDate");
            string productId = _command.GetFirstParameterValue("productId");
            string signature = _command.GetFirstParameterValue("receipt");
            string purchaseToken = _command.GetFirstParameterValue("purchaseToken");
            string orderId = _command.GetFirstParameterValue("transactionId");

            AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
                price,
                currency,
                productId,
                orderId,
                signature,
                purchaseToken);
            subscription.PurchaseTime = purchaseTime;

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    subscription.AddCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    subscription.AddPartnerParameter(key, value);
                }
            }

            Adjust.TrackPlayStoreSubscription(subscription);
#endif
        }

        private void ThirdPartySharing()
        {
            string enabled = _command.GetFirstParameterValue("isEnabled");
            bool? isEnabled = null;
            if (enabled != null)
            {
                isEnabled = bool.Parse(enabled);
            }

            AdjustThirdPartySharing adjustThirdPartySharing = new AdjustThirdPartySharing(isEnabled);

            if (_command.ContainsParameter("granularOptions"))
            {
                var granularOptions = _command.Parameters["granularOptions"];
                for (var i = 0; i < granularOptions.Count; i += 3)
                {
                    var partnerName = granularOptions[i];
                    var key = granularOptions[i+1];
                    var value = granularOptions[i+2];
                    adjustThirdPartySharing.AddGranularOption(partnerName, key, value);
                }
            }

            if (_command.ContainsParameter("partnerSharingSettings"))
            {
                var partnerSharingSettings = _command.Parameters["partnerSharingSettings"];
                for (var i = 0; i < partnerSharingSettings.Count; i += 3)
                {
                    var partnerName = partnerSharingSettings[i];
                    var key = partnerSharingSettings[i+1];
                    var value = partnerSharingSettings[i+2];
                    adjustThirdPartySharing.AddPartnerSharingSetting(partnerName, key, bool.Parse(value));
                }
            }

            Adjust.TrackThirdPartySharing(adjustThirdPartySharing);
        }

        private void MeasurementConsent()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("isEnabled"));
            Adjust.TrackMeasurementConsent(enabled);
        }

        private void TrackAdRevenue()
        {
            string source = _command.GetFirstParameterValue("adRevenueSource");
            AdjustAdRevenue adRevenue = new AdjustAdRevenue(source);

            if (_command.ContainsParameter("revenue"))
            {
                var revenueParams = _command.Parameters["revenue"];
                var currency = revenueParams[0];
                var revenue = double.Parse(revenueParams[1], System.Globalization.CultureInfo.InvariantCulture);
                adRevenue.SetRevenue(revenue, currency);
            }

            if (_command.ContainsParameter("adImpressionsCount"))
            {
                int adImpressionsCount = int.Parse(_command.GetFirstParameterValue("adImpressionsCount"));
                adRevenue.AdImpressionsCount = adImpressionsCount;
            }

            if (_command.ContainsParameter("adRevenueUnit"))
            {
                string adRevenueUnit = _command.GetFirstParameterValue("adRevenueUnit");
                adRevenue.AdRevenueUnit = adRevenueUnit;
            }

            if (_command.ContainsParameter("adRevenuePlacement"))
            {
                string adRevenuePlacement = _command.GetFirstParameterValue("adRevenuePlacement");
                adRevenue.AdRevenuePlacement = adRevenuePlacement;
            }

            if (_command.ContainsParameter("adRevenueNetwork"))
            {
                string adRevenueNetwork = _command.GetFirstParameterValue("adRevenueNetwork");
                adRevenue.AdRevenueNetwork = adRevenueNetwork;
            }

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    adRevenue.AddCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    adRevenue.AddPartnerParameter(key, value);
                }
            }

            Adjust.TrackAdRevenue(adRevenue);
        }

        private void GetLastDeeplink()
        {
#if UNITY_IOS
            Adjust.GetLastDeeplink(LastDeeplinkCallback);
#endif
        }

        private void VerifyPurchase()
        {
#if UNITY_IOS
            string transactionId = _command.GetFirstParameterValue("transactionId");
            string productId = _command.GetFirstParameterValue("productId");
            string receipt = _command.GetFirstParameterValue("receipt");

            AdjustAppStorePurchase purchase = new AdjustAppStorePurchase(
                transactionId,
                productId,
                receipt);

            Adjust.VerifyAppStorePurchase(purchase, VerificationResultCallback);
#elif UNITY_ANDROID
            string productId = _command.GetFirstParameterValue("productId");
            string purchaseToken = _command.GetFirstParameterValue("purchaseToken");

            AdjustPlayStorePurchase purchase = new AdjustPlayStorePurchase(
                productId,
                purchaseToken);

            Adjust.VerifyPlayStorePurchase(purchase, VerificationResultCallback);
#endif
        }

        private void ProcessAndResolveDeeplink()
        {
            var deeplink = _command.GetFirstParameterValue("deeplink");
            AdjustDeeplink adjustDeeplink = new AdjustDeeplink(deeplink);
            Adjust.ProcessAndResolveDeeplink(adjustDeeplink, DeeplinkResolvedCallback);
        }

        private void AttributionGetter()
        {
            string localExtraPath = ExtraPath;
            Adjust.GetAttribution((attribution) => {
                _testLibrary.AddInfoToSend("tracker_token", attribution.TrackerToken);
                _testLibrary.AddInfoToSend("tracker_name", attribution.TrackerName);
                _testLibrary.AddInfoToSend("network", attribution.Network);
                _testLibrary.AddInfoToSend("campaign", attribution.Campaign);
                _testLibrary.AddInfoToSend("adgroup", attribution.Adgroup);
                _testLibrary.AddInfoToSend("creative", attribution.Creative);
                _testLibrary.AddInfoToSend("click_label", attribution.ClickLabel);
                _testLibrary.AddInfoToSend("cost_type", attribution.CostType);
                _testLibrary.AddInfoToSend("cost_amount", attribution.CostAmount.ToString());
                _testLibrary.AddInfoToSend("cost_currency", attribution.CostCurrency);
                _testLibrary.AddInfoToSend("fb_install_referrer", attribution.FbInstallReferrer);
                _testLibrary.SendInfoToServer(localExtraPath);
            });
        }

        private void EnablePlayStoreKidsApp()
        {
            Adjust.EnablePlayStoreKidsApp();
        }

        private void DisablePlayStoreKidsApp()
        {
            Adjust.DisablePlayStoreKidsApp();
        }

        // helper methods
        private void VerificationResultCallback(AdjustPurchaseVerificationResult verificationResult)
        {
            string localExtraPath = ExtraPath;
            _testLibrary.AddInfoToSend("verification_status", verificationResult.VerificationStatus);
            _testLibrary.AddInfoToSend("code", verificationResult.Code.ToString());
            _testLibrary.AddInfoToSend("message", verificationResult.Message);
            _testLibrary.SendInfoToServer(localExtraPath);
        }

        private void DeeplinkResolvedCallback(string resolvedLink)
        {
            string localExtraPath = ExtraPath;
            _testLibrary.AddInfoToSend("resolved_link", resolvedLink);
            _testLibrary.SendInfoToServer(localExtraPath);
        }

        private void LastDeeplinkCallback(string lastDeeplink)
        {
            string localExtraPath = ExtraPath;
            _testLibrary.AddInfoToSend("last_deeplink", lastDeeplink);
            _testLibrary.SendInfoToServer(localExtraPath);
        }

        private void CommandNotFound(string className, string methodName)
        {
            TestApp.Log("Adjust Test: Method '" + methodName + "' not found for class '" + className + "'");
        }
    }
}
