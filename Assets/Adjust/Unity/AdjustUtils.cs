using System;
using System.Collections.Generic;

using UnityEngine;

namespace com.adjust.sdk
{
	public class AdjustUtils
	{
        #region Constants
        public static string KeyAdid = "adid";
		public static string KeyMessage = "message";
        public static string KeyWillRetry = "willRetry";
        public static string KeyTimestamp = "timestamp";
        public static string KeyEventToken = "eventToken";
        public static string KeyJsonResponse = "jsonResponse";
        #endregion

        #region Public methods
        public static int ConvertLogLevel (AdjustLogLevel? logLevel)
        {
            if (logLevel == null)
            {
                return -1;
            }

            return (int)logLevel;
        }

        public static int ConvertBool (bool? value)
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

        public static double ConvertDouble (double? value)
        {
            if (value == null)
            {
                return -1;
            }

            return (double)value;
        }

        public static string ConvertListToJson (List<String> list)
        {
            if (list == null)
            {
                return null;
            }

            var jsonArray = new JSONArray ();
            
            foreach (var listItem in list)
            {
                jsonArray.Add (new JSONData (listItem));
            }

            return jsonArray.ToString ();
        }

        public static void PrintJsonResponse (Dictionary<string, object> dictionary)
        {
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                Type t = pair.Value.GetType ();
                bool isDict = t.IsGenericType && t.GetGenericTypeDefinition () == typeof(Dictionary<,>);

                if (isDict == true)
                {
                    PrintJsonResponse ((Dictionary<string, object>)pair.Value);
                }
                else
                {
                    Debug.Log ("Key = " + pair.Key);
                    Debug.Log ("Value = " + pair.Value);
                }
            }
        }

        public static String GetJsonString (JSONNode node, string key)
        {
            var jsonValue = GetJsonValue (node, key);
            
            if (jsonValue == null)
            {
                return null;
            }
            
            return jsonValue.Value;
        }
        
        public static JSONNode GetJsonValue (JSONNode node, string key)
        {
            if (node == null)
            {
                return null;
            }
            
            var nodeValue = node [key];

            if (nodeValue.GetType () == typeof (JSONLazyCreator))
            {
                return null;
            }
            
            return nodeValue;
        }

        public static void WriteJsonResponseDictionary (JSONClass jsonObject, Dictionary<string, object> output)
        {
            foreach (KeyValuePair<string, JSONNode> pair in jsonObject)
            {
                if (pair.Value.AsObject == null)
                {
                    output.Add (pair.Key, pair.Value);
                }
                else
                {
                    output.Add (pair.Key, new Dictionary<string, object> ());
                    WriteJsonResponseDictionary (pair.Value.AsObject, (Dictionary<string, object>)output [pair.Key]);
                }
            }
        }
        #endregion
	}
}
