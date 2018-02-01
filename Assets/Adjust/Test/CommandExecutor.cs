using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.UI;

using com.adjust.sdk;

namespace com.adjust.sdk.test
{
	public class CommandExecutor
	{
		private Dictionary<int, AdjustConfig> _savedConfigs = new Dictionary<int, AdjustConfig>();
		private Dictionary<int, AdjustEvent> _savedEvents = new Dictionary<int, AdjustEvent>();

		private string _baseUrl;
		public string BasePath  { get; set; }
		public Command Command { get; set; }
		private TestFactory _testFactory;

		public CommandExecutor (TestFactory testFactory, string baseUrl)
		{
			_baseUrl = baseUrl;
			_testFactory = testFactory;
		}

		public void ExecuteCommand (Command command)
		{
			this.Command = command;
			TestApp.Log(string.Format(" \t>>> EXECUTING METHOD: [{0}.{1}] <<<", command.ClassName, command.MethodName));

			try
			{
				switch (command.MethodName) 
				{
					case "testOptions": TestOptions(); break;
					case "config": Config(); break;					
					case "start" : Start(); break;
					case "event": Event(); break;
					case "trackEvent" : TrackEvent(); break;
					case "resume" : Resume(); break;
					case "pause" : Pause(); break;
					case "setEnabled" : SetEnabled(); break;
					case "setReferrer" : SetReferrer(); break;
					case "setOfflineMode" : SetOfflineMode(); break;
					case "sendFirstPackages" : SendFirstPackages(); break;
					case "addSessionCallbackParameter" : AddSessionCallbackParameter(); break;
					case "addSessionPartnerParameter" : AddSessionPartnerParameter(); break;
					case "removeSessionCallbackParameter" : RemoveSessionCallbackParameter(); break;
					case "removeSessionPartnerParameter" : RemoveSessionPartnerParameter(); break;
					case "resetSessionCallbackParameters" : ResetSessionCallbackParameters(); break;
					case "resetSessionPartnerParameters" : ResetSessionPartnerParameters(); break;
					case "setPushToken" : SetPushToken(); break;
					case "openDeeplink" : OpenDeepLink(); break;
					case "sendReferrer": SetReferrer(); break;

					default : CommandNotFound(command.ClassName, command.MethodName); break;
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
			AdjustTestOptions testOptions = new AdjustTestOptions();
			testOptions.BaseUrl = _baseUrl;

			if (Command.ContainsParameter("basePath"))
			{
				BasePath = Command.GetFirstParameterValue("basePath");
			}

			if (Command.ContainsParameter("timerInterval"))
			{
				long timerInterval = long.Parse(Command.GetFirstParameterValue("timerInterval"));
				testOptions.TimerIntervalInMilliseconds = timerInterval;
			}

			if (Command.ContainsParameter("timerStart"))
			{
				long timerStart = long.Parse(Command.GetFirstParameterValue("timerStart"));
				testOptions.TimerStartInMilliseconds = timerStart;
			}

			if (Command.ContainsParameter("sessionInterval"))
			{
				long sessionInterval = long.Parse(Command.GetFirstParameterValue("sessionInterval"));
				testOptions.SessionIntervalInMilliseconds = sessionInterval;
			}

			if (Command.ContainsParameter("subsessionInterval"))
			{
				long subsessionInterval = long.Parse(Command.GetFirstParameterValue("subsessionInterval"));
				testOptions.SubsessionIntervalInMilliseconds = subsessionInterval;
			}

			if (Command.ContainsParameter("teardown"))
			{
				List<string> teardownOptions = Command.Parameters["teardown"];
				foreach (string teardownOption in teardownOptions)
				{
					if (teardownOption == "resetSdk")
					{
						testOptions.Teardown = true;
						testOptions.BasePath = BasePath;
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
			if (Command.ContainsParameter("configName"))
			{
				var configName = Command.GetFirstParameterValue("configName");
				configNumber = int.Parse(configName.Substring(configName.Length - 1));
			}

			AdjustConfig adjustConfig;
			AdjustLogLevel? logLevel = null;
			if (Command.ContainsParameter("logLevel"))
			{
				var logLevelString = Command.GetFirstParameterValue("logLevel");
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

				TestApp.Log (string.Format ("TestApp LogLevel = {0}", logLevel));
			}

			if (_savedConfigs.ContainsKey(configNumber))
			{
				adjustConfig = _savedConfigs[configNumber];
			}
			else
			{
				var environmentString = Command.GetFirstParameterValue("environment");
				var environment = environmentString == "sandbox" ? AdjustEnvironment.Sandbox : AdjustEnvironment.Production;
				var appToken = Command.GetFirstParameterValue("appToken");

				if (!string.IsNullOrEmpty (appToken)) {
					if (appToken == "null") {
						adjustConfig = new AdjustConfig (null, environment);
					} else {
						adjustConfig = new AdjustConfig (appToken, environment);
					}
				} else {
					adjustConfig = new AdjustConfig (null, environment);
				}

				if(logLevel.HasValue)
					adjustConfig.setLogLevel (logLevel.Value);
					
				#if (UNITY_WSA || UNITY_WP8)
				adjustConfig.logDelegate = msg => Debug.Log (msg);
				#endif

				_savedConfigs.Add(configNumber, adjustConfig);
			}

			if (Command.ContainsParameter("sdkPrefix"))
				adjustConfig.SdkPrefix = Command.GetFirstParameterValue("sdkPrefix");

			if (Command.ContainsParameter ("defaultTracker"))
				adjustConfig.setDefaultTracker (Command.GetFirstParameterValue ("defaultTracker"));

			if (Command.ContainsParameter("delayStart"))
			{
				var delayStartStr = Command.GetFirstParameterValue("delayStart");
				var delayStart = double.Parse(delayStartStr);
				TestApp.Log("Delay start set to: " + delayStart);
				adjustConfig.setDelayStart (delayStart);
			}

			if (Command.ContainsParameter("appSecret"))
			{
				var appSecretList = Command.Parameters["appSecret"];
				TestApp.Log ("Received AppSecret array: " + string.Join (",", appSecretList.ToArray()));

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
					TestApp.LogError("App secret list does not contain 5 elements! Skip setting app secret.");
			}

			if (Command.ContainsParameter("deviceKnown"))
			{
				var deviceKnownS = Command.GetFirstParameterValue("deviceKnown");
				var deviceKnown = deviceKnownS.ToLower() == "true";
				adjustConfig.setIsDeviceKnown(deviceKnown);
			}

			if (Command.ContainsParameter("eventBufferingEnabled"))
			{
				var eventBufferingEnabledS = Command.GetFirstParameterValue("eventBufferingEnabled");
				var eventBufferingEnabled = eventBufferingEnabledS.ToLower() == "true";
				adjustConfig.setEventBufferingEnabled (eventBufferingEnabled);
			}

			if (Command.ContainsParameter("sendInBackground"))
			{
				var sendInBackgroundS = Command.GetFirstParameterValue("sendInBackground");
				var sendInBackground = sendInBackgroundS.ToLower() == "true";
				adjustConfig.sendInBackground = sendInBackground;
			}

			if (Command.ContainsParameter("userAgent"))
			{
				var userAgent = Command.GetFirstParameterValue("userAgent");
				if (userAgent.Equals ("null")) {
					adjustConfig.setUserAgent (null);
				} else {
					adjustConfig.setUserAgent (userAgent);
				}
			}

			if (Command.ContainsParameter("deferredDeeplinkCallback"))
			{
				adjustConfig.setDeferredDeeplinkDelegate (uri => 
				{
					if (uri == null) {
						TestApp.Log ("DeeplinkResponse, uri = null");
						adjustConfig.setLaunchDeferredDeeplink(false);
						//return false;
					}

					TestApp.Log ("DeeplinkResponse, uri = " + uri.ToString ());

					if (!uri.StartsWith ("adjusttest")) 
					{
						adjustConfig.setLaunchDeferredDeeplink(false);
						//return false;
					}

					adjustConfig.setLaunchDeferredDeeplink(true);
					//return true;
				});
			}

			if (Command.ContainsParameter("attributionCallbackSendAll"))
			{
				string localBasePath = BasePath;
				adjustConfig.setAttributionChangedDelegate (attribution => 
				{
					TestApp.Log ("AttributionChanged, attribution = " + attribution);

					this._testFactory.AddInfoToSend ("trackerToken", attribution.trackerToken);
					this._testFactory.AddInfoToSend ("trackerName", attribution.trackerName);
					this._testFactory.AddInfoToSend ("network", attribution.network);
					this._testFactory.AddInfoToSend ("campaign", attribution.campaign);
					this._testFactory.AddInfoToSend ("adgroup", attribution.adgroup);
					this._testFactory.AddInfoToSend ("creative", attribution.creative);
					this._testFactory.AddInfoToSend ("clickLabel", attribution.clickLabel);
					this._testFactory.AddInfoToSend ("adid", attribution.adid);
					this._testFactory.SendInfoToServer (localBasePath);
				});
			}

			if (Command.ContainsParameter("sessionCallbackSendSuccess"))
			{
				string localBasePath = BasePath;
				adjustConfig.setSessionSuccessDelegate (sessionSuccessResponseData => 
				{
					TestApp.Log ("SesssionTrackingSucceeded, sessionSuccessResponseData = " + sessionSuccessResponseData);

					this._testFactory.AddInfoToSend ("message", sessionSuccessResponseData.Message);
					this._testFactory.AddInfoToSend ("timestamp", sessionSuccessResponseData.Timestamp);
					this._testFactory.AddInfoToSend ("adid", sessionSuccessResponseData.Adid);
					if (sessionSuccessResponseData.JsonResponse != null)
						this._testFactory.AddInfoToSend ("jsonResponse", sessionSuccessResponseData.GetJsonResponse ());
					this._testFactory.SendInfoToServer (localBasePath);
				});
			}

			if (Command.ContainsParameter("sessionCallbackSendFailure"))
			{
				string localBasePath = BasePath;
				adjustConfig.setSessionFailureDelegate (sessionFailureResponseData => 
				{
					TestApp.Log ("SesssionTrackingFailed, sessionFailureResponseData = " + sessionFailureResponseData);
					
					this._testFactory.AddInfoToSend ("message", sessionFailureResponseData.Message);
					this._testFactory.AddInfoToSend ("timestamp", sessionFailureResponseData.Timestamp);
					this._testFactory.AddInfoToSend ("adid", sessionFailureResponseData.Adid);
					this._testFactory.AddInfoToSend ("willRetry", sessionFailureResponseData.WillRetry.ToString ().ToLower ());
					if (sessionFailureResponseData.JsonResponse != null)
						this._testFactory.AddInfoToSend ("jsonResponse", sessionFailureResponseData.GetJsonResponse ());
					this._testFactory.SendInfoToServer (localBasePath);
				});
			}

			if (Command.ContainsParameter("eventCallbackSendSuccess"))
			{
				string localBasePath = BasePath;
				adjustConfig.setEventSuccessDelegate (eventSuccessResponseData => 
				{
					TestApp.Log ("EventTrackingSucceeded, eventSuccessResponseData = " + eventSuccessResponseData);

					this._testFactory.AddInfoToSend ("message", eventSuccessResponseData.Message);
					this._testFactory.AddInfoToSend ("timestamp", eventSuccessResponseData.Timestamp);
					this._testFactory.AddInfoToSend ("adid", eventSuccessResponseData.Adid);
					this._testFactory.AddInfoToSend ("eventToken", eventSuccessResponseData.EventToken);
					if (eventSuccessResponseData.JsonResponse != null)
						this._testFactory.AddInfoToSend ("jsonResponse", eventSuccessResponseData.GetJsonResponse ());
					this._testFactory.SendInfoToServer (localBasePath);
				});
			}

			if (Command.ContainsParameter("eventCallbackSendFailure"))
			{
				string localBasePath = BasePath;
				adjustConfig.setEventFailureDelegate (eventFailureResponseData => 
				{
					TestApp.Log ("EventTrackingFailed, eventFailureResponseData = " + eventFailureResponseData);

					this._testFactory.AddInfoToSend ("message", eventFailureResponseData.Message);
					this._testFactory.AddInfoToSend ("timestamp", eventFailureResponseData.Timestamp);
					this._testFactory.AddInfoToSend ("adid", eventFailureResponseData.Adid);
					this._testFactory.AddInfoToSend ("eventToken", eventFailureResponseData.EventToken);
					this._testFactory.AddInfoToSend ("willRetry", eventFailureResponseData.WillRetry.ToString ().ToLower ());
					if (eventFailureResponseData.JsonResponse != null)
						this._testFactory.AddInfoToSend ("jsonResponse", eventFailureResponseData.GetJsonResponse());
					this._testFactory.SendInfoToServer (localBasePath);
				});
			}
		}

		private AdjustEvent GetEvent (JSONNode parameters)
		{
			AdjustEvent adjustEvent = null;

			string eventToken = parameters ["eventToken"] [0].Value;
			string revenue = parameters ["revenue"] [1].Value;
			string currency = parameters ["revenue"] [0].Value;
			string orderId = parameters ["orderId"] [0].Value;

			JSONNode callbackParameters = parameters ["callbackParams"];
			JSONNode partnerParameters = parameters ["partnerParams"];

			if (!String.IsNullOrEmpty (eventToken)) {
				if (eventToken.Equals ("null")) {
					adjustEvent = new AdjustEvent (null);
				} else {
					adjustEvent = new AdjustEvent (eventToken);
				}
			}

			if (!String.IsNullOrEmpty (revenue)) {
				try {
					float revenueValue = float.Parse (revenue, CultureInfo.InvariantCulture.NumberFormat);

					if (!String.IsNullOrEmpty (currency)) {
						if (currency.Equals ("null")) {
							adjustEvent.setRevenue (revenueValue, null);
						} else {
							adjustEvent.setRevenue (revenueValue, currency);
						}
					}
				} catch (Exception e) {
					TestApp.Log ("adjust test: " + e.ToString ());
				}
			}

			if (!String.IsNullOrEmpty (orderId)) {
				if (orderId.Equals ("null")) {
					adjustEvent.setTransactionId (null);
				} else {
					adjustEvent.setTransactionId (orderId);
				}
			}

			if (null != callbackParameters) {
				for (int i = 0; i < callbackParameters.Count; i += 2) {
					string paramKey = callbackParameters [i].Value;
					string paramValue = callbackParameters [i + 1].Value;

					if (paramKey.Equals ("null")) {
						paramKey = null;
					}

					if (paramValue.Equals ("null")) {
						paramValue = null;
					}

					adjustEvent.addCallbackParameter (paramKey, paramValue);
				}
			}

			if (null != partnerParameters) {
				for (int i = 0; i < partnerParameters.Count; i += 2) {
					string paramKey = partnerParameters [i].Value;
					string paramValue = partnerParameters [i + 1].Value;

					if (paramKey.Equals ("null")) {
						paramKey = null;
					}

					if (paramValue.Equals ("null")) {
						paramValue = null;
					}

					adjustEvent.addPartnerParameter (paramKey, paramValue);
				}
			}

			return adjustEvent;
		}

		private void Start()
		{
			Config();

			var configNumber = 0;
			if (Command.ContainsParameter("configName"))
			{
				var configName = Command.GetFirstParameterValue("configName");
				configNumber = int.Parse(configName.Substring(configName.Length - 1));
			}

			var adjustConfig = _savedConfigs[configNumber];

			Adjust.start(adjustConfig);

			_savedConfigs.Remove(0);
		}

		private void Event()
		{
			var eventNumber = 0;
			if (Command.ContainsParameter("eventName"))
			{
				var eventName = Command.GetFirstParameterValue("eventName");
				eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
			}

			AdjustEvent adjustEvent = null;
			if (_savedEvents.ContainsKey(eventNumber))
			{
				adjustEvent = _savedEvents[eventNumber];
			}
			else
			{
				var eventToken = Command.GetFirstParameterValue("eventToken");
				adjustEvent = new AdjustEvent(eventToken);
				_savedEvents.Add(eventNumber, adjustEvent);
			}

			if (Command.ContainsParameter("revenue"))
			{
				var revenueParams = Command.Parameters["revenue"];
				var currency = revenueParams[0];
				var revenue = double.Parse(revenueParams[1]);
				adjustEvent.setRevenue(revenue, currency);
			}

			if (Command.ContainsParameter("callbackParams"))
			{
				var callbackParams = Command.Parameters["callbackParams"];
				for (var i = 0; i < callbackParams.Count; i = i + 2)
				{
					var key = callbackParams[i];
					var value = callbackParams[i + 1];
					adjustEvent.addCallbackParameter(key, value);
				}
			}

			if (Command.ContainsParameter("partnerParams"))
			{
				var partnerParams = Command.Parameters["partnerParams"];
				for (var i = 0; i < partnerParams.Count; i = i + 2)
				{
					var key = partnerParams[i];
					var value = partnerParams[i + 1];
					adjustEvent.addPartnerParameter(key, value);
				}
			}

			if (Command.ContainsParameter("orderId"))
			{
				var orderId = Command.GetFirstParameterValue("orderId");
				adjustEvent.setTransactionId (orderId);
			}
		}

		private void TrackEvent()
		{
			Event();

			var eventNumber = 0;
			if (Command.ContainsParameter("eventName"))
			{
				var eventName = Command.GetFirstParameterValue("eventName");
				eventNumber = int.Parse(eventName.Substring(eventName.Length - 1));
			}

			var adjustEvent = _savedEvents[eventNumber];
			Adjust.trackEvent(adjustEvent);

			_savedEvents.Remove(0);
		}

		private void Resume ()
		{
			#if UNITY_IOS
				// No action, iOS SDK is subscribed to iOS lifecycle notifications.
			#elif UNITY_ANDROID
				AdjustAndroid.OnResume();
			#elif (UNITY_WSA || UNITY_WP8)
				AdjustWindows.OnResume();
			#else
				Debug.Log(errorMsgPlatform);
			#endif
		}

		private void Pause ()
		{
			#if UNITY_IOS
				// No action, iOS SDK is subscribed to iOS lifecycle notifications.
			#elif UNITY_ANDROID
				AdjustAndroid.OnPause();
			#elif (UNITY_WSA || UNITY_WP8)
				AdjustWindows.OnPause();
			#else
				Debug.Log(errorMsgPlatform);
			#endif
		}

		private void SetEnabled()
		{
			var enabled = bool.Parse(Command.GetFirstParameterValue("enabled"));
			Adjust.setEnabled(enabled);
		}

		private void SetOfflineMode()
		{
			var enabled = bool.Parse(Command.GetFirstParameterValue("enabled"));
			Adjust.setOfflineMode(enabled);
		}

		private void SetReferrer() {
			string referrer = Command.GetFirstParameterValue("referrer");
			Adjust.setReferrer(referrer);
		}

		private void AddSessionCallbackParameter()
		{
			if (!Command.ContainsParameter("KeyValue")) return;

			var keyValuePairs = Command.Parameters["KeyValue"];
			for (var i = 0; i < keyValuePairs.Count; i = i + 2)
			{
				var key = keyValuePairs[i];
				var value = keyValuePairs[i + 1];
				Adjust.addSessionCallbackParameter(key, value);
			}
		}

		private void SendFirstPackages ()
		{
			Adjust.sendFirstPackages ();
		}

		private void AddSessionPartnerParameter()
		{
			if (!Command.ContainsParameter("KeyValue")) return;

			var keyValuePairs = Command.Parameters["KeyValue"];
			for (var i = 0; i < keyValuePairs.Count; i = i + 2)
			{
				var key = keyValuePairs[i];
				var value = keyValuePairs[i + 1];
				Adjust.addSessionPartnerParameter(key, value);
			}
		}

		private void RemoveSessionCallbackParameter()
		{
			if (!Command.ContainsParameter("key")) return;

			var keys = Command.Parameters["key"];
			for (var i = 0; i < keys.Count; i = i + 1)
			{
				var key = keys[i];
				Adjust.removeSessionCallbackParameter(key);
			}
		}

		private void RemoveSessionPartnerParameter()
		{
			if (!Command.ContainsParameter("key")) return;

			var keys = Command.Parameters["key"];
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

		private void ResetSessionCallbackParameters (JSONNode parameters)
		{
			Adjust.resetSessionCallbackParameters ();
		}

		private void ResetSessionPartnerParameters (JSONNode parameters)
		{
			Adjust.resetSessionPartnerParameters ();
		}

		//private void SetPushToken (JSONNode parameters)
		private void SetPushToken()
		{
			var pushToken = Command.GetFirstParameterValue("pushToken");

			if (!String.IsNullOrEmpty (pushToken)) {
				if (pushToken.Equals ("null")) {
					Adjust.setDeviceToken (null);
				} else {
					Adjust.setDeviceToken (pushToken);
				}
			}
		}

		private void OpenDeepLink()
		{
			var deeplink = Command.GetFirstParameterValue("deeplink");
			Adjust.appWillOpenUrl(deeplink);
		}

		private void CommandNotFound (string className, string methodName)
		{
			TestApp.Log ("adjust test: Method '" + methodName + "' not found for class '" + className + "'");
		}
	}
}
