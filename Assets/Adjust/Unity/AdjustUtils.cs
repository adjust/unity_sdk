using System;
using System.Collections.Generic;

using UnityEngine;

namespace com.adjust.sdk
{
    public class AdjustUtils
    {
        public static string KeyAdid = "adid";
        public static string KeyMessage = "message";
        public static string KeyNetwork = "network";
        public static string KeyAdgroup = "adgroup";
        public static string KeyCampaign = "campaign";
        public static string KeyCreative = "creative";
        public static string KeyWillRetry = "willRetry";
        public static string KeyTimestamp = "timestamp";
        public static string KeyCallbackId = "callbackId";
        public static string KeyEventToken = "eventToken";
        public static string KeyClickLabel = "clickLabel";
        public static string KeyTrackerName = "trackerName";
        public static string KeyTrackerToken = "trackerToken";
        public static string KeyJsonResponse = "jsonResponse";
        public static string KeyCostType = "costType";
        public static string KeyCostAmount = "costAmount";
        public static string KeyCostCurrency = "costCurrency";

        // For testing purposes.
        public static string KeyTestOptionsBaseUrl = "baseUrl";
        public static string KeyTestOptionsGdprUrl = "gdprUrl";
        public static string KeyTestOptionsSubscriptionUrl = "subscriptionUrl";
        public static string KeyTestOptionsExtraPath = "extraPath";
        public static string KeyTestOptionsBasePath = "basePath";
        public static string KeyTestOptionsGdprPath = "gdprPath";
        public static string KeyTestOptionsDeleteState = "deleteState";
        public static string KeyTestOptionsUseTestConnectionOptions = "useTestConnectionOptions";
        public static string KeyTestOptionsTimerIntervalInMilliseconds = "timerIntervalInMilliseconds";
        public static string KeyTestOptionsTimerStartInMilliseconds = "timerStartInMilliseconds";
        public static string KeyTestOptionsSessionIntervalInMilliseconds = "sessionIntervalInMilliseconds";
        public static string KeyTestOptionsSubsessionIntervalInMilliseconds = "subsessionIntervalInMilliseconds";
        public static string KeyTestOptionsTeardown = "teardown";
        public static string KeyTestOptionsNoBackoffWait = "noBackoffWait";
        public static string KeyTestOptionsiAdFrameworkEnabled = "iAdFrameworkEnabled";
        public static string KeyTestOptionsAdServicesFrameworkEnabled = "adServicesFrameworkEnabled";

        public static int ConvertLogLevel(AdjustLogLevel? logLevel)
        {
            if (logLevel == null)
            {
                return -1;
            }

            return (int)logLevel;
        }

        public static int ConvertBool(bool? value)
        {
            if (value == null)
            {
                return -1;
            }
            if (value.Value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static double ConvertDouble(double? value)
        {
            if (value == null)
            {
                return -1;
            }

            return (double)value;
        }

        public static int ConvertInt(int? value)
        {
            if (value == null)
            {
                return -1;
            }

            return (int)value;
        }

        public static long ConvertLong(long? value)
        {
            if (value == null)
            {
                return -1;
            }

            return (long)value;
        }

        public static string ConvertListToJson(List<String> list)
        {
            if (list == null)
            {
                return null;
            }

            var jsonArray = new JSONArray();
            foreach (var listItem in list)
            {
                jsonArray.Add(new JSONData(listItem));
            }

            return jsonArray.ToString();
        }

        public static string GetJsonResponseCompact(Dictionary<string, object> dictionary)
        {
            string logJsonResponse = "";

            if (dictionary == null)
            {
                return logJsonResponse;
            }
            else
            {
                int preLoopCounter = 0;
                logJsonResponse += "{";

                foreach (KeyValuePair<string, object> pair in dictionary)
                {
                    String valueString = pair.Value as string;

                    if (valueString != null)
                    {
                        if (++preLoopCounter > 1)
                        {
                            logJsonResponse += ",";
                        }

                        // if the value is another JSON/complex-structure
                        if (valueString.StartsWith("{") && valueString.EndsWith("}"))
                        {
                            logJsonResponse += "\"" + pair.Key + "\"" + ":" + valueString;
                        }
                        else
                        {
                            logJsonResponse += "\"" + pair.Key + "\"" + ":" + "\"" + valueString + "\"";
                        }

                        continue;
                    }

                    Dictionary<string, object> valueDictionary = pair.Value as Dictionary<string, object>;

                    if (++preLoopCounter > 1)
                    {
                        logJsonResponse += ",";
                    }

                    logJsonResponse += "\"" + pair.Key + "\"" + ":";
                    logJsonResponse += GetJsonResponseCompact(valueDictionary);
                }

                logJsonResponse += "}";
            }

            return logJsonResponse;
        }

        public static String GetJsonString(JSONNode node, string key)
        {
            if (node == null)
            {
                return null;
            }

            // Access value object and cast it to JSONData.
            var nodeValue = node[key] as JSONData;

            if (nodeValue == null)
            {
                return null;
            }

            // https://github.com/adjust/unity_sdk/issues/137
            if (nodeValue == "")
            {
                return null;
            }

            return nodeValue.Value;
        }

        public static void WriteJsonResponseDictionary(JSONClass jsonObject, Dictionary<string, object> output)
        {
            foreach (KeyValuePair<string, JSONNode> pair in jsonObject)
            {
                // Try to cast value as a complex object.
                var subNode = pair.Value.AsObject;
                var key = pair.Key;

                // Value is not a complex object.
                if (subNode == null)
                {
                    var value = pair.Value.Value;
                    output.Add(key, value);
                    continue;
                }

                // Create new dictionary for complex type.
                var newSubDictionary = new Dictionary<string, object>();

                // Save it in the current dictionary.
                output.Add(key, newSubDictionary);

                // Recursive call to fill new dictionary.
                WriteJsonResponseDictionary(subNode, newSubDictionary);
            }
        }

        public static string TryGetValue(Dictionary<string, string> dictionary, string key)
        {
            string value;
            if (dictionary.TryGetValue(key, out value))
            {
                // https://github.com/adjust/unity_sdk/issues/137
                if (value == "")
                {
                    return null;
                }
                return value;
            }
            return null;
        }

#if UNITY_ANDROID
        public static AndroidJavaObject TestOptionsMap2AndroidJavaObject(Dictionary<string, string> testOptionsMap, AndroidJavaObject ajoCurrentActivity)
        {
            AndroidJavaObject ajoTestOptions = new AndroidJavaObject("com.adjust.sdk.AdjustTestOptions");
            ajoTestOptions.Set<String>("baseUrl", testOptionsMap[KeyTestOptionsBaseUrl]);
            ajoTestOptions.Set<String>("gdprUrl", testOptionsMap[KeyTestOptionsGdprUrl]);
            ajoTestOptions.Set<String>("subscriptionUrl", testOptionsMap[KeyTestOptionsSubscriptionUrl]);

            if (testOptionsMap.ContainsKey(KeyTestOptionsExtraPath) && !string.IsNullOrEmpty(testOptionsMap[KeyTestOptionsExtraPath]))
            {
                ajoTestOptions.Set<String>("basePath", testOptionsMap[KeyTestOptionsExtraPath]);
                ajoTestOptions.Set<String>("gdprPath", testOptionsMap[KeyTestOptionsExtraPath]);
                ajoTestOptions.Set<String>("subscriptionPath", testOptionsMap[KeyTestOptionsExtraPath]);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsDeleteState) && ajoCurrentActivity != null)
            {
                ajoTestOptions.Set<AndroidJavaObject>("context", ajoCurrentActivity);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsUseTestConnectionOptions)) 
            {
                bool useTestConnectionOptions = testOptionsMap[KeyTestOptionsUseTestConnectionOptions].ToLower() == "true";
                AndroidJavaObject ajoUseTestConnectionOptions = new AndroidJavaObject("java.lang.Boolean", useTestConnectionOptions);
                ajoTestOptions.Set<AndroidJavaObject>("useTestConnectionOptions", ajoUseTestConnectionOptions);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsTimerIntervalInMilliseconds)) 
            {
                var timerIntervalInMilliseconds = long.Parse(testOptionsMap[KeyTestOptionsTimerIntervalInMilliseconds]);
                AndroidJavaObject ajoTimerIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", timerIntervalInMilliseconds);
                ajoTestOptions.Set<AndroidJavaObject>("timerIntervalInMilliseconds", ajoTimerIntervalInMilliseconds);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsTimerStartInMilliseconds)) 
            {
                var timerStartInMilliseconds = long.Parse(testOptionsMap[KeyTestOptionsTimerStartInMilliseconds]);
                AndroidJavaObject ajoTimerStartInMilliseconds = new AndroidJavaObject("java.lang.Long", timerStartInMilliseconds);
                ajoTestOptions.Set<AndroidJavaObject>("timerStartInMilliseconds", ajoTimerStartInMilliseconds);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsSessionIntervalInMilliseconds)) 
            {   
                var sessionIntervalInMilliseconds = long.Parse(testOptionsMap[KeyTestOptionsSessionIntervalInMilliseconds]);
                AndroidJavaObject ajoSessionIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", sessionIntervalInMilliseconds);
                ajoTestOptions.Set<AndroidJavaObject>("sessionIntervalInMilliseconds", ajoSessionIntervalInMilliseconds);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsSubsessionIntervalInMilliseconds)) 
            {
                var subsessionIntervalInMilliseconds = long.Parse(testOptionsMap[KeyTestOptionsSubsessionIntervalInMilliseconds]);
                AndroidJavaObject ajoSubsessionIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", subsessionIntervalInMilliseconds);
                ajoTestOptions.Set<AndroidJavaObject>("subsessionIntervalInMilliseconds", ajoSubsessionIntervalInMilliseconds);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsTeardown))
            {
                bool teardown = testOptionsMap[KeyTestOptionsTeardown].ToLower() == "true";
                AndroidJavaObject ajoTeardown = new AndroidJavaObject("java.lang.Boolean", teardown);
                ajoTestOptions.Set<AndroidJavaObject>("teardown", ajoTeardown);
            }
            if (testOptionsMap.ContainsKey(KeyTestOptionsNoBackoffWait))
            {
                bool noBackoffWait = testOptionsMap[KeyTestOptionsNoBackoffWait].ToLower() == "true";
                AndroidJavaObject ajoNoBackoffWait = new AndroidJavaObject("java.lang.Boolean", noBackoffWait);
                ajoTestOptions.Set<AndroidJavaObject>("noBackoffWait", ajoNoBackoffWait);
            }

            return ajoTestOptions;
        }
#endif
    }
}
