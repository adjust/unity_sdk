using System;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_WSA || UNITY_WP8)
using TestLibraryInterface;
#endif

namespace com.adjust.sdk.test
{
#if (UNITY_WSA || UNITY_WP8)
    public class CommandExecutor : IAdjustCommandExecutor
#else
    public class CommandExecutor
#endif
    {
        public string ExtraPath { get; set; }

        private Dictionary<int, AdjustConfig> _savedConfigs = new Dictionary<int, AdjustConfig>();
        private Dictionary<int, AdjustEvent> _savedEvents = new Dictionary<int, AdjustEvent>();
        private string _baseUrl;
        private string _gdprUrl;
        private string _subscriptionUrl;
        private Command _command;
        private ITestLibrary _testLibrary;

        public CommandExecutor(ITestLibrary testLibrary, string baseUrl, string gdprUrl, string subscriptionUrl)
        {
            _baseUrl = baseUrl;
            _gdprUrl = gdprUrl;
            _subscriptionUrl = subscriptionUrl;
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
                    case "setReferrer": SetReferrer(); break;
                    case "setOfflineMode": SetOfflineMode(); break;
                    case "sendFirstPackages": SendFirstPackages(); break;
                    case "addSessionCallbackParameter": AddSessionCallbackParameter(); break;
                    case "addSessionPartnerParameter": AddSessionPartnerParameter(); break;
                    case "removeSessionCallbackParameter": RemoveSessionCallbackParameter(); break;
                    case "removeSessionPartnerParameter": RemoveSessionPartnerParameter(); break;
                    case "resetSessionCallbackParameters": ResetSessionCallbackParameters(); break;
                    case "resetSessionPartnerParameters": ResetSessionPartnerParameters(); break;
                    case "setPushToken": SetPushToken(); break;
                    case "openDeeplink": OpenDeepLink(); break;
                    case "sendReferrer": SetReferrer(); break;
                    case "gdprForgetMe": GdprForgetMe(); break;
                    case "trackAdRevenue": TrackAdRevenue(); break;
                    case "disableThirdPartySharing": DisableThirdPartySharing(); break;
                    case "trackSubscription": TrackSubscription(); break;
                    case "thirdPartySharing": ThirdPartySharing(); break;
                    case "measurementConsent": MeasurementConsent(); break;
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
            testOptions[AdjustUtils.KeyTestOptionsBaseUrl] = _baseUrl;
            testOptions[AdjustUtils.KeyTestOptionsGdprUrl] = _gdprUrl;
            testOptions[AdjustUtils.KeyTestOptionsSubscriptionUrl] = _subscriptionUrl;

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
            // iAd.framework will not be used in test app by default
            testOptions [AdjustUtils.KeyTestOptionsiAdFrameworkEnabled] = "false";
            if (_command.ContainsParameter("iAdFrameworkEnabled"))
            {
                testOptions[AdjustUtils.KeyTestOptionsiAdFrameworkEnabled] = _command.GetFirstParameterValue("iAdFrameworkEnabled");
            }
            // AdServices.framework will not be used in test app by default
            testOptions [AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled] = "false";
            if (_command.ContainsParameter("adServicesFrameworkEnabled"))
            {
                testOptions[AdjustUtils.KeyTestOptionsAdServicesFrameworkEnabled] = _command.GetFirstParameterValue("adServicesFrameworkEnabled");
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
                    adjustConfig.setLogLevel(logLevel.Value);
                }

#if (UNITY_WSA || UNITY_WP8)
                adjustConfig.logDelegate = msg => Debug.Log(msg);
#endif
                _savedConfigs.Add(configNumber, adjustConfig);
            }

            if (_command.ContainsParameter("sdkPrefix"))
            {
                // SDK prefix not tested for non natives.
            }

            if (_command.ContainsParameter("defaultTracker"))
            {
                adjustConfig.setDefaultTracker(_command.GetFirstParameterValue("defaultTracker"));
            }

            if (_command.ContainsParameter("externalDeviceId"))
            {
                adjustConfig.setExternalDeviceId(_command.GetFirstParameterValue("externalDeviceId"));
            }

            if (_command.ContainsParameter("delayStart"))
            {
                var delayStartStr = _command.GetFirstParameterValue("delayStart");
                var delayStart = double.Parse(delayStartStr);
                adjustConfig.setDelayStart(delayStart);
            }

            if (_command.ContainsParameter("appSecret"))
            {
                var appSecretList = _command.Parameters["appSecret"];
                if (!string.IsNullOrEmpty(appSecretList[0]) && appSecretList.Count == 5)
                {
                    long secretId, info1, info2, info3, info4;
                    long.TryParse(appSecretList[0], out secretId);
                    long.TryParse(appSecretList[1], out info1);
                    long.TryParse(appSecretList[2], out info2);
                    long.TryParse(appSecretList[3], out info3);
                    long.TryParse(appSecretList[4], out info4);

                    adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);
                }
            }

            if (_command.ContainsParameter("deviceKnown"))
            {
                var deviceKnownS = _command.GetFirstParameterValue("deviceKnown");
                var deviceKnown = deviceKnownS.ToLower() == "true";
                adjustConfig.setIsDeviceKnown(deviceKnown);
            }

            if (_command.ContainsParameter("eventBufferingEnabled"))
            {
                var eventBufferingEnabledS = _command.GetFirstParameterValue("eventBufferingEnabled");
                var eventBufferingEnabled = eventBufferingEnabledS.ToLower() == "true";
                adjustConfig.setEventBufferingEnabled(eventBufferingEnabled);
            }

            if (_command.ContainsParameter("sendInBackground"))
            {
                var sendInBackgroundS = _command.GetFirstParameterValue("sendInBackground");
                var sendInBackground = sendInBackgroundS.ToLower() == "true";
                adjustConfig.sendInBackground = sendInBackground;
            }

            if (_command.ContainsParameter("allowiAdInfoReading"))
            {
                var allowiAdInfoReadingS = _command.GetFirstParameterValue("allowiAdInfoReading");
                var allowiAdInfoReading = allowiAdInfoReadingS.ToLower() == "true";
                adjustConfig.allowiAdInfoReading = allowiAdInfoReading;
            }

            if (_command.ContainsParameter("allowAdServicesInfoReading"))
            {
                var allowAdServicesInfoReadingS = _command.GetFirstParameterValue("allowAdServicesInfoReading");
                var allowAdServicesInfoReading = allowAdServicesInfoReadingS.ToLower() == "true";
                adjustConfig.allowAdServicesInfoReading = allowAdServicesInfoReading;
            }

            if (_command.ContainsParameter("allowIdfaReading"))
            {
                var allowIdfaReadingS = _command.GetFirstParameterValue("allowIdfaReading");
                var allowIdfaReading = allowIdfaReadingS.ToLower() == "true";
                adjustConfig.allowIdfaReading = allowIdfaReading;
            }

            if (_command.ContainsParameter("userAgent"))
            {
                var userAgent = _command.GetFirstParameterValue("userAgent");
                if (userAgent.Equals("null"))
                {
                    adjustConfig.setUserAgent(null);
                }
                else
                {
                    adjustConfig.setUserAgent(userAgent);
                }
            }

            if (_command.ContainsParameter("deferredDeeplinkCallback"))
            {
                bool launchDeferredDeeplink = _command.GetFirstParameterValue("deferredDeeplinkCallback") == "true";
                adjustConfig.setLaunchDeferredDeeplink(launchDeferredDeeplink);
                string localExtraPath = ExtraPath;
                adjustConfig.setDeferredDeeplinkDelegate(uri =>
                {
                    _testLibrary.AddInfoToSend("deeplink", uri);
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("attributionCallbackSendAll"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.setAttributionChangedDelegate(attribution =>
                {
                    _testLibrary.AddInfoToSend("trackerToken", attribution.trackerToken);
                    _testLibrary.AddInfoToSend("trackerName", attribution.trackerName);
                    _testLibrary.AddInfoToSend("network", attribution.network);
                    _testLibrary.AddInfoToSend("campaign", attribution.campaign);
                    _testLibrary.AddInfoToSend("adgroup", attribution.adgroup);
                    _testLibrary.AddInfoToSend("creative", attribution.creative);
                    _testLibrary.AddInfoToSend("clickLabel", attribution.clickLabel);
                    _testLibrary.AddInfoToSend("adid", attribution.adid);
                    _testLibrary.AddInfoToSend("costType", attribution.costType);
                    _testLibrary.AddInfoToSend("costAmount", attribution.costAmount.ToString());
                    _testLibrary.AddInfoToSend("costCurrency", attribution.costCurrency);
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.setSessionSuccessDelegate(sessionSuccessResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", sessionSuccessResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", sessionSuccessResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", sessionSuccessResponseData.Adid);
                    if (sessionSuccessResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", sessionSuccessResponseData.GetJsonResponse());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendFailure"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.setSessionFailureDelegate(sessionFailureResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", sessionFailureResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", sessionFailureResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", sessionFailureResponseData.Adid);
                    _testLibrary.AddInfoToSend("willRetry", sessionFailureResponseData.WillRetry.ToString().ToLower());
                    if (sessionFailureResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", sessionFailureResponseData.GetJsonResponse());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendSuccess"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.setEventSuccessDelegate(eventSuccessResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", eventSuccessResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", eventSuccessResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", eventSuccessResponseData.Adid);
                    _testLibrary.AddInfoToSend("eventToken", eventSuccessResponseData.EventToken);
                    _testLibrary.AddInfoToSend("callbackId", eventSuccessResponseData.CallbackId);
                    if (eventSuccessResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", eventSuccessResponseData.GetJsonResponse());
                    }
                    _testLibrary.SendInfoToServer(localExtraPath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendFailure"))
            {
                string localExtraPath = ExtraPath;
                adjustConfig.setEventFailureDelegate(eventFailureResponseData =>
                {
                    _testLibrary.AddInfoToSend("message", eventFailureResponseData.Message);
                    _testLibrary.AddInfoToSend("timestamp", eventFailureResponseData.Timestamp);
                    _testLibrary.AddInfoToSend("adid", eventFailureResponseData.Adid);
                    _testLibrary.AddInfoToSend("eventToken", eventFailureResponseData.EventToken);
                    _testLibrary.AddInfoToSend("callbackId", eventFailureResponseData.CallbackId);
                    _testLibrary.AddInfoToSend("willRetry", eventFailureResponseData.WillRetry.ToString().ToLower());
                    if (eventFailureResponseData.JsonResponse != null)
                    {
                        _testLibrary.AddInfoToSend("jsonResponse", eventFailureResponseData.GetJsonResponse());
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
            Adjust.start(adjustConfig);
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
                var revenue = double.Parse(revenueParams[1]);
                adjustEvent.setRevenue(revenue, currency);
            }

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    adjustEvent.addCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    adjustEvent.addPartnerParameter(key, value);
                }
            }

            if (_command.ContainsParameter("orderId"))
            {
                var orderId = _command.GetFirstParameterValue("orderId");
                adjustEvent.setTransactionId(orderId);
            }

            if (_command.ContainsParameter("callbackId"))
            {
                var callbackId = _command.GetFirstParameterValue("callbackId");
                adjustEvent.setCallbackId(callbackId);
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
            Adjust.trackEvent(adjustEvent);
            _savedEvents.Remove(0);
        }

        private void Resume()
        {
#if UNITY_IOS
            AdjustiOS.TrackSubsessionStart("test");
#elif UNITY_ANDROID
            AdjustAndroid.OnResume();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.OnResume();
#else
            Debug.Log("TestApp - Command Executor - Error! Cannot Resume. None of the supported platforms selected.");
#endif
        }

        private void Pause()
        {
#if UNITY_IOS
            AdjustiOS.TrackSubsessionEnd("test");
#elif UNITY_ANDROID
            AdjustAndroid.OnPause();
#elif (UNITY_WSA || UNITY_WP8)
            AdjustWindows.OnPause();
#else
            Debug.Log("TestApp - Command Executor - Error! Cannot Pause. None of the supported platforms selected.");
#endif
        }

        private void SetEnabled()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("enabled"));
            Adjust.setEnabled(enabled);
        }

        public void GdprForgetMe()
        {
            Adjust.gdprForgetMe();
        }

        public void DisableThirdPartySharing()
        {
            Adjust.disableThirdPartySharing();
        }

        private void SetOfflineMode()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("enabled"));
            Adjust.setOfflineMode(enabled);
        }

        private void SetReferrer()
        {
            string referrer = _command.GetFirstParameterValue("referrer");
            #pragma warning disable CS0618
            Adjust.setReferrer(referrer);
            #pragma warning restore CS0618
        }

        private void AddSessionCallbackParameter()
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
                Adjust.addSessionCallbackParameter(key, value);
            }
        }

        private void SendFirstPackages()
        {
            Adjust.sendFirstPackages();
        }

        private void AddSessionPartnerParameter()
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
                Adjust.addSessionPartnerParameter(key, value);
            }
        }

        private void RemoveSessionCallbackParameter()
        {
            if (!_command.ContainsParameter("key"))
            {
                return;
            }

            var keys = _command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.removeSessionCallbackParameter(key);
            }
        }

        private void RemoveSessionPartnerParameter()
        {
            if (!_command.ContainsParameter("key"))
            {
                return;
            }

            var keys = _command.Parameters["key"];
            for (var i = 0; i < keys.Count; i = i + 1)
            {
                var key = keys[i];
                Adjust.removeSessionPartnerParameter(key);
            }
        }

        private void ResetSessionCallbackParameters()
        {
            Adjust.resetSessionCallbackParameters();
        }

        private void ResetSessionPartnerParameters()
        {
            Adjust.resetSessionPartnerParameters();
        }

        private void ResetSessionCallbackParameters(JSONNode parameters)
        {
            Adjust.resetSessionCallbackParameters();
        }

        private void ResetSessionPartnerParameters(JSONNode parameters)
        {
            Adjust.resetSessionPartnerParameters();
        }

        private void SetPushToken()
        {
            var pushToken = _command.GetFirstParameterValue("pushToken");

            if (!string.IsNullOrEmpty(pushToken))
            {
                if (pushToken.Equals("null"))
                {
                    Adjust.setDeviceToken(null);
                }
                else
                {
                    Adjust.setDeviceToken(pushToken);
                }
            }
        }

        private void OpenDeepLink()
        {
            var deeplink = _command.GetFirstParameterValue("deeplink");
            Adjust.appWillOpenUrl(deeplink);
        }

        private void TrackAdRevenue()
        {
            string source = _command.GetFirstParameterValue("adRevenueSource");
            string payload = _command.GetFirstParameterValue("adRevenueJsonString");
            Adjust.trackAdRevenue(source, payload);
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
            subscription.setTransactionDate(transactionDate);
            subscription.setSalesRegion(salesRegion);

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    subscription.addCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    subscription.addPartnerParameter(key, value);
                }
            }

            Adjust.trackAppStoreSubscription(subscription);
#elif UNITY_ANDROID
            string price = _command.GetFirstParameterValue("revenue");
            string currency = _command.GetFirstParameterValue("currency");
            string purchaseTime = _command.GetFirstParameterValue("transactionDate");
            string sku = _command.GetFirstParameterValue("productId");
            string signature = _command.GetFirstParameterValue("receipt");
            string purchaseToken = _command.GetFirstParameterValue("purchaseToken");
            string orderId = _command.GetFirstParameterValue("transactionId");

            AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
                price,
                currency,
                sku,
                orderId,
                signature,
                purchaseToken);
            subscription.setPurchaseTime(purchaseTime);

            if (_command.ContainsParameter("callbackParams"))
            {
                var callbackParams = _command.Parameters["callbackParams"];
                for (var i = 0; i < callbackParams.Count; i = i + 2)
                {
                    var key = callbackParams[i];
                    var value = callbackParams[i + 1];
                    subscription.addCallbackParameter(key, value);
                }
            }

            if (_command.ContainsParameter("partnerParams"))
            {
                var partnerParams = _command.Parameters["partnerParams"];
                for (var i = 0; i < partnerParams.Count; i = i + 2)
                {
                    var key = partnerParams[i];
                    var value = partnerParams[i + 1];
                    subscription.addPartnerParameter(key, value);
                }
            }

            Adjust.trackPlayStoreSubscription(subscription);
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
                    adjustThirdPartySharing.addGranularOption(partnerName, key, value);
                }
            }

            Adjust.trackThirdPartySharing(adjustThirdPartySharing);
        }

        private void MeasurementConsent()
        {
            var enabled = bool.Parse(_command.GetFirstParameterValue("isEnabled"));
            Adjust.trackMeasurementConsent(enabled);
        }

        private void CommandNotFound(string className, string methodName)
        {
            TestApp.Log("Adjust Test: Method '" + methodName + "' not found for class '" + className + "'");
        }
    }
}
