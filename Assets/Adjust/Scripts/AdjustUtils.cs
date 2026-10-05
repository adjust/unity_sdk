using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AdjustSdk
{
    public class AdjustUtils
    {
        public const string KeyAdid = "adid";
        public const string KeyMessage = "message";
        public const string KeyNetwork = "network";
        public const string KeyAdgroup = "adgroup";
        public const string KeyCampaign = "campaign";
        public const string KeyCreative = "creative";
        public const string KeyWillRetry = "willRetry";
        public const string KeyTimestamp = "timestamp";
        public const string KeyCallbackId = "callbackId";
        public const string KeyEventToken = "eventToken";
        public const string KeyClickLabel = "clickLabel";
        public const string KeyLabel = "label";
        public const string KeyPayload = "payload";
        public const string KeyTrackerName = "trackerName";
        public const string KeyTrackerToken = "trackerToken";
        public const string KeyJsonResponse = "jsonResponse";
        public const string KeyCostType = "costType";
        public const string KeyCostAmount = "costAmount";
        public const string KeyCostCurrency = "costCurrency";
        public const string KeyFbInstallReferrer = "fbInstallReferrer";
        public const string KeySkanConversionValue = "fineValue";
        public const string KeySkanCoarseValue = "coarseValue";
        public const string KeySkanLockWindow = "lockWindow";
        public const string KeyCode = "code";
        public const string KeyVerificationStatus = "verificationStatus";

        // For testing purposes.
        public const string KeyTestOptionsBaseUrl = "baseUrl";
        public const string KeyTestOptionsGdprUrl = "gdprUrl";
        public const string KeyTestOptionsSubscriptionUrl = "subscriptionUrl";
        public const string KeyTestOptionsPurchaseVerificationUrl = "purchaseVerificationUrl";
        public const string KeyTestOptionsOverwriteUrl = "urlOverwrite";
        public const string KeyTestOptionsExtraPath = "extraPath";
        public const string KeyTestOptionsBasePath = "basePath";
        public const string KeyTestOptionsGdprPath = "gdprPath";
        public const string KeyTestOptionsDeleteState = "deleteState";
        public const string KeyTestOptionsUseTestConnectionOptions = "useTestConnectionOptions";
        public const string KeyTestOptionsTimerIntervalInMilliseconds = "timerIntervalInMilliseconds";
        public const string KeyTestOptionsTimerStartInMilliseconds = "timerStartInMilliseconds";
        public const string KeyTestOptionsSessionIntervalInMilliseconds = "sessionIntervalInMilliseconds";
        public const string KeyTestOptionsSubsessionIntervalInMilliseconds = "subsessionIntervalInMilliseconds";
        public const string KeyTestOptionsTeardown = "teardown";
        public const string KeyTestOptionsNoBackoffWait = "noBackoffWait";
        public const string KeyTestOptionsAdServicesFrameworkEnabled = "adServicesFrameworkEnabled";
        public const string KeyTestOptionsAttStatus = "attStatus";
        public const string KeyTestOptionsIdfa = "idfa";
        public const string KeyTestOptionsIgnoreSystemLifecycleBootstrap = "ignoreSystemLifecycleBootstrap";

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

        public static string ConvertReadOnlyCollectionToJson(ReadOnlyCollection<string> list)
        {
            if (list == null)
            {
                return null;
            }

            List<string> processedList = new List<string>();
            for (int i = 0; i < list.Count; i += 1)
            {
                string item = list[i];

                if (item == null)
                {
                    continue;
                }

                processedList.Add(item);
            }

            // create JSON array
            var jsonArray = new JSONArray();
            foreach (var listItem in processedList)
            {
                jsonArray.Add(new JSONData(listItem));
            }

            return jsonArray.ToString();
        }

        public static string ConvertReadOnlyCollectionOfPairsToJson(ReadOnlyCollection<string> list)
        {
            if (list == null)
            {
                return null;
            }
            // list of callback / partner parameters must contain even number of elements
            if (list.Count % 2 != 0)
            {
                return null;
            }

            List<string> processedList = new List<string>();
            for (int i = 0; i < list.Count; i += 2)
            {
                string key = list[i];
                string value = list[i + 1];

                if (key == null || value == null)
                {
                    continue;
                }

                processedList.Add(key);
                processedList.Add(value);
            }

            // create JSON array
            var jsonArray = new JSONArray();
            foreach (var listItem in processedList)
            {
                jsonArray.Add(new JSONData(listItem));
            }

            return jsonArray.ToString();
        }

        public static string ConvertReadOnlyCollectionOfTripletsToJson(ReadOnlyCollection<string> list)
        {
            if (list == null)
            {
                return null;
            }
            // list of third party sharing parameters must contain number of elements divisible by 3
            if (list.Count % 3 != 0)
            {
                return null;
            }

            List<string> processedList = new List<string>();
            for (int i = 0; i < list.Count; i += 3)
            {
                string partnerName = list[i];
                string key = list[i + 1];
                string value = list[i + 2];

                if (partnerName == null || key == null || value == null)
                {
                    continue;
                }

                processedList.Add(partnerName);
                processedList.Add(key);
                processedList.Add(value);
            }

            // create JSON array
            var jsonArray = new JSONArray();
            foreach (var listItem in processedList)
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
                    string valueString = pair.Value as string;

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

        public static string GetJsonString(JSONNode node, string key)
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

        public static Dictionary<string, string> GetSkanUpdateDataDictionary(string skanUpdateData)
        {
            Dictionary<string, string> skanUpdateDataDictionary = new Dictionary<string, string>();
            var skanUpdateDataNode = JSON.Parse(skanUpdateData);
            if (skanUpdateDataNode != null && skanUpdateDataNode.AsObject != null)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                AdjustUtils.WriteJsonResponseDictionary(skanUpdateDataNode.AsObject, temp);
                foreach (KeyValuePair<string, object> entry in temp)
                {
                    skanUpdateDataDictionary.Add(entry.Key, entry.Value.ToString());
                }
            }
            return skanUpdateDataDictionary;
        }

        public static Dictionary<string, object> GetAttributionJsonResponse(string attributionJsonResponse)
        {
            Dictionary<string, object> attributionJsonResponseDictionary = new Dictionary<string, object>();
            var attributionJsonResponseNode = JSON.Parse(attributionJsonResponse);
            if (attributionJsonResponseNode != null && attributionJsonResponseNode.AsObject != null)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                AdjustUtils.WriteJsonResponseDictionary(attributionJsonResponseNode.AsObject, temp);
                foreach (KeyValuePair<string, object> entry in temp)
                {
                    attributionJsonResponseDictionary.Add(entry.Key, entry.Value);
                }
            }
            return attributionJsonResponseDictionary;
        }

        public static string GetValueOrEmptyToNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

#if UNITY_ANDROID
        public static AndroidJavaObject TestOptionsMap2AndroidJavaObject(Dictionary<string, string> testOptionsMap, AndroidJavaObject ajoCurrentActivity)
        {
            AndroidJavaObject ajoTestOptions = new AndroidJavaObject("com.adjust.sdk.AdjustTestOptions");
            ajoTestOptions.Set<string>("baseUrl", testOptionsMap[KeyTestOptionsBaseUrl]);
            ajoTestOptions.Set<string>("gdprUrl", testOptionsMap[KeyTestOptionsGdprUrl]);
            ajoTestOptions.Set<string>("subscriptionUrl", testOptionsMap[KeyTestOptionsSubscriptionUrl]);
            ajoTestOptions.Set<string>("purchaseVerificationUrl", testOptionsMap[KeyTestOptionsPurchaseVerificationUrl]);

            if (testOptionsMap.ContainsKey(KeyTestOptionsExtraPath) && !string.IsNullOrEmpty(testOptionsMap[KeyTestOptionsExtraPath]))
            {
                ajoTestOptions.Set<string>("basePath", testOptionsMap[KeyTestOptionsExtraPath]);
                ajoTestOptions.Set<string>("gdprPath", testOptionsMap[KeyTestOptionsExtraPath]);
                ajoTestOptions.Set<string>("subscriptionPath", testOptionsMap[KeyTestOptionsExtraPath]);
                ajoTestOptions.Set<string>("purchaseVerificationPath", testOptionsMap[KeyTestOptionsExtraPath]);
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
            if (testOptionsMap.ContainsKey(KeyTestOptionsIgnoreSystemLifecycleBootstrap))
            {
                // TODO: fix native logic not to have negation in naming throughout the chain of calls
                bool doNotIgnoreSystemLifecycleBootstrap = testOptionsMap[KeyTestOptionsIgnoreSystemLifecycleBootstrap].ToLower() == "true";
                AndroidJavaObject ajoDoNotIgnoreSystemLifecycleBootstrap = new AndroidJavaObject("java.lang.Boolean", doNotIgnoreSystemLifecycleBootstrap);
                ajoTestOptions.Set<AndroidJavaObject>("ignoreSystemLifecycleBootstrap", ajoDoNotIgnoreSystemLifecycleBootstrap);
            }

            return ajoTestOptions;
        }
#endif
    }
}
