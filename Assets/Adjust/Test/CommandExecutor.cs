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
        private Dictionary<int, AdjustConfig> _savedConfigs = new Dictionary<int, AdjustConfig>();
        private Dictionary<int, AdjustEvent> _savedEvents = new Dictionary<int, AdjustEvent>();
        public string BasePath { get; set; }
        private string _baseUrl;
        private Command _command;
        private ITestFactory _testFactory;

        public CommandExecutor(ITestFactory testFactory, string baseUrl)
        {
            _baseUrl = baseUrl;
            _testFactory = testFactory;
        }
			
		public void ExecuteCommand(string className, string methodName, Dictionary<string, List<string>> parameters)
		{
			this.ExecuteCommand(new Command(className, methodName, parameters));
		}

        public void ExecuteCommand(Command command)
        {
            _command = command;

            TestApp.Log(string.Format("\t>>> EXECUTING METHOD: [{0}.{1}] <<<", _command.ClassName, _command.MethodName));

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

                    default: CommandNotFound(_command.ClassName, _command.MethodName); break;
                }
            }
            catch (Exception ex)
            {
                TestApp.LogError(string.Format("{0} ---- {1}",
                    "executeCommand: failed to parse command. Check commands' syntax", ex.ToString()));
            }
        }

        private void TestOptions()
        {
            TestApp.Log("Configuring and setting Testing Options...");

            AdjustTestOptions testOptions = new AdjustTestOptions();
            testOptions.BaseUrl = _baseUrl;

            if (_command.ContainsParameter("basePath"))
            {
                BasePath = _command.GetFirstParameterValue("basePath");
            }

            if (_command.ContainsParameter("timerInterval"))
            {
                long timerInterval = long.Parse(_command.GetFirstParameterValue("timerInterval"));
                testOptions.TimerIntervalInMilliseconds = timerInterval;
            }

            if (_command.ContainsParameter("timerStart"))
            {
                long timerStart = long.Parse(_command.GetFirstParameterValue("timerStart"));
                testOptions.TimerStartInMilliseconds = timerStart;
            }

            if (_command.ContainsParameter("sessionInterval"))
            {
                long sessionInterval = long.Parse(_command.GetFirstParameterValue("sessionInterval"));
                testOptions.SessionIntervalInMilliseconds = sessionInterval;
            }

            if (_command.ContainsParameter("subsessionInterval"))
            {
                long subsessionInterval = long.Parse(_command.GetFirstParameterValue("subsessionInterval"));
                testOptions.SubsessionIntervalInMilliseconds = subsessionInterval;
            }

            if (_command.ContainsParameter("teardown"))
            {
                List<string> teardownOptions = _command.Parameters["teardown"];
                foreach (string teardownOption in teardownOptions)
                {
                    if (teardownOption == "resetSdk")
                    {
                        testOptions.Teardown = true;
                        testOptions.BasePath = BasePath;
                        testOptions.UseTestConnectionOptions = true;
                    }
                    if (teardownOption == "deleteState")
                    {
                        testOptions.DeleteState = true;
                    }
                    if (teardownOption == "resetTest")
                    {
                        _savedEvents = new Dictionary<int, AdjustEvent>();
                        _savedConfigs = new Dictionary<int, AdjustConfig>();
                        testOptions.TimerIntervalInMilliseconds = -1;
                        testOptions.TimerStartInMilliseconds = -1;
                        testOptions.SessionIntervalInMilliseconds = -1;
                        testOptions.SubsessionIntervalInMilliseconds = -1;
                    }
                    if (teardownOption == "sdk")
                    {
                        testOptions.Teardown = true;
                        testOptions.BasePath = null;
                        testOptions.UseTestConnectionOptions = false;
                    }
                    if (teardownOption == "test")
                    {
                        _savedEvents = null;
                        _savedConfigs = null;
                        testOptions.TimerIntervalInMilliseconds = -1;
                        testOptions.TimerStartInMilliseconds = -1;
                        testOptions.SessionIntervalInMilliseconds = -1;
                        testOptions.SubsessionIntervalInMilliseconds = -1;
                    }
                }
            }

            Adjust.SetTestOptions(testOptions);
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
                adjustConfig.SdkPrefix = _command.GetFirstParameterValue("sdkPrefix");
            }

            if (_command.ContainsParameter("defaultTracker"))
            {
                adjustConfig.setDefaultTracker(_command.GetFirstParameterValue("defaultTracker"));
            }

            if (_command.ContainsParameter("delayStart"))
            {
                var delayStartStr = _command.GetFirstParameterValue("delayStart");
                var delayStart = double.Parse(delayStartStr);
                TestApp.Log("Delay start set to: " + delayStart);
                adjustConfig.setDelayStart(delayStart);
            }

            if (_command.ContainsParameter("appSecret"))
            {
                var appSecretList = _command.Parameters["appSecret"];
                TestApp.Log("Received AppSecret array: " + string.Join(",", appSecretList.ToArray()));

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
                else
                {
                    TestApp.LogError("App secret list does not contain 5 elements! Skip setting app secret.");
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
                adjustConfig.setDeferredDeeplinkDelegate(uri =>
                {
                    if (uri == null)
                    {
                        TestApp.Log("DeeplinkResponse, uri = null");
                        adjustConfig.setLaunchDeferredDeeplink(false);
                    }

                    TestApp.Log("DeeplinkResponse, uri = " + uri.ToString());

                    if (!uri.StartsWith("adjusttest"))
                    {
                        adjustConfig.setLaunchDeferredDeeplink(false);
                    }

                    adjustConfig.setLaunchDeferredDeeplink(true);
                });
            }

            if (_command.ContainsParameter("attributionCallbackSendAll"))
            {
                string localBasePath = BasePath;
                adjustConfig.setAttributionChangedDelegate(attribution =>
                {
                    TestApp.Log("AttributionChanged, attribution = " + attribution);

                    _testFactory.AddInfoToSend("trackerToken", attribution.trackerToken);
                    _testFactory.AddInfoToSend("trackerName", attribution.trackerName);
                    _testFactory.AddInfoToSend("network", attribution.network);
                    _testFactory.AddInfoToSend("campaign", attribution.campaign);
                    _testFactory.AddInfoToSend("adgroup", attribution.adgroup);
                    _testFactory.AddInfoToSend("creative", attribution.creative);
                    _testFactory.AddInfoToSend("clickLabel", attribution.clickLabel);
                    _testFactory.AddInfoToSend("adid", attribution.adid);
                    _testFactory.SendInfoToServer(localBasePath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendSuccess"))
            {
                string localBasePath = BasePath;
                adjustConfig.setSessionSuccessDelegate(sessionSuccessResponseData =>
                {
                    TestApp.Log("SesssionTrackingSucceeded, sessionSuccessResponseData = " + sessionSuccessResponseData);

                    _testFactory.AddInfoToSend("message", sessionSuccessResponseData.Message);
                    _testFactory.AddInfoToSend("timestamp", sessionSuccessResponseData.Timestamp);
                    _testFactory.AddInfoToSend("adid", sessionSuccessResponseData.Adid);
                    if (sessionSuccessResponseData.JsonResponse != null)
                    {
                        _testFactory.AddInfoToSend("jsonResponse", sessionSuccessResponseData.GetJsonResponse());
                    }
                    _testFactory.SendInfoToServer(localBasePath);
                });
            }

            if (_command.ContainsParameter("sessionCallbackSendFailure"))
            {
                string localBasePath = BasePath;
                adjustConfig.setSessionFailureDelegate(sessionFailureResponseData =>
                {
                    TestApp.Log("SesssionTrackingFailed, sessionFailureResponseData = " + sessionFailureResponseData);

                    _testFactory.AddInfoToSend("message", sessionFailureResponseData.Message);
                    _testFactory.AddInfoToSend("timestamp", sessionFailureResponseData.Timestamp);
                    _testFactory.AddInfoToSend("adid", sessionFailureResponseData.Adid);
                    _testFactory.AddInfoToSend("willRetry", sessionFailureResponseData.WillRetry.ToString().ToLower());
                    if (sessionFailureResponseData.JsonResponse != null)
                    {
                        _testFactory.AddInfoToSend("jsonResponse", sessionFailureResponseData.GetJsonResponse());
                    }
                    _testFactory.SendInfoToServer(localBasePath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendSuccess"))
            {
                string localBasePath = BasePath;
                adjustConfig.setEventSuccessDelegate(eventSuccessResponseData =>
                {
                    TestApp.Log("EventTrackingSucceeded, eventSuccessResponseData = " + eventSuccessResponseData);

                    _testFactory.AddInfoToSend("message", eventSuccessResponseData.Message);
                    _testFactory.AddInfoToSend("timestamp", eventSuccessResponseData.Timestamp);
                    _testFactory.AddInfoToSend("adid", eventSuccessResponseData.Adid);
                    _testFactory.AddInfoToSend("eventToken", eventSuccessResponseData.EventToken);
                    if (eventSuccessResponseData.JsonResponse != null)
                    {
                        _testFactory.AddInfoToSend("jsonResponse", eventSuccessResponseData.GetJsonResponse());
                    }
                    _testFactory.SendInfoToServer(localBasePath);
                });
            }

            if (_command.ContainsParameter("eventCallbackSendFailure"))
            {
                string localBasePath = BasePath;
                adjustConfig.setEventFailureDelegate(eventFailureResponseData =>
                {
                    TestApp.Log("EventTrackingFailed, eventFailureResponseData = " + eventFailureResponseData);

                    _testFactory.AddInfoToSend("message", eventFailureResponseData.Message);
                    _testFactory.AddInfoToSend("timestamp", eventFailureResponseData.Timestamp);
                    _testFactory.AddInfoToSend("adid", eventFailureResponseData.Adid);
                    _testFactory.AddInfoToSend("eventToken", eventFailureResponseData.EventToken);
                    _testFactory.AddInfoToSend("willRetry", eventFailureResponseData.WillRetry.ToString().ToLower());
                    if (eventFailureResponseData.JsonResponse != null)
                    {
                        _testFactory.AddInfoToSend("jsonResponse", eventFailureResponseData.GetJsonResponse());
                    }
                    _testFactory.SendInfoToServer(localBasePath);
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
			AdjustiOS.TrackSubsessionStart();
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
			AdjustiOS.TrackSubsessionEnd();
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

        private void CommandNotFound(string className, string methodName)
        {
            TestApp.Log("adjust test: Method '" + methodName + "' not found for class '" + className + "'");
        }
    }
}
