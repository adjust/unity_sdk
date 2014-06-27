using System;
using SimpleJSON;
using UnityEngine;

namespace com.adjust.sdk
{
	public class ResponseData
	{
		public enum ActivityKind {
			UNKNOWN, SESSION, EVENT, REVENUE, REATTRIBUTION
		}

		public ActivityKind? activityKind { get; private set; }
		public string activityKindString { get; private set; }
		public bool? success { get; private set; }
		public bool? willRetry { get; private set; }
		public string error { get; private set; }
		public string trackerToken { get; private set; }
		public string trackerName { get; private set; }
		public string network { get; private set; }
		public string campaign { get; private set; }
		public string adgroup { get; private set; }
		public string creative { get; private set; }

		public ResponseData(string jsonString) {
			var jsonNode = JSON.Parse (jsonString);

			if (jsonNode == null) {
				return;
			}

			activityKind = ParseActivityKind(getJsonString (jsonNode, "activityKind"));
			activityKindString = activityKind.ToString ().ToLower ();

			success = getJsonBool(jsonNode, "success");
			willRetry = getJsonBool(jsonNode, "willRetry");

			error = getJsonString(jsonNode, "error");
			trackerName = getJsonString(jsonNode, "trackerName");
			trackerToken = getJsonString(jsonNode, "trackerToken");
			network = getJsonString(jsonNode, "network");
			campaign = getJsonString(jsonNode, "campaign");
			adgroup = getJsonString(jsonNode, "adgroup");
			creative = getJsonString(jsonNode, "creative");
		}

		private String getJsonString(JSONNode node, string key) {
			var jsonValue = getJsonValue (node, key);

			if (jsonValue == null)
				return null;

			return jsonValue.Value;
		}

		private bool? getJsonBool(JSONNode node, string key) {
			var jsonValue = getJsonValue (node, key);

			if (jsonValue == null)
				return null;

			return jsonValue.AsBool;
		}

		private JSONNode getJsonValue(JSONNode node, string key) {
			if (node == null)
				return null;

			var nodeValue = node [key];
			if (nodeValue.GetType() == typeof(JSONLazyCreator))
				return null;

			return nodeValue;
		}

		private ActivityKind ParseActivityKind(string sActivityKind) 
		{
			if ("session" == sActivityKind)
				return ActivityKind.SESSION;
			else if ("event" == sActivityKind)
				return ActivityKind.EVENT;
			else if ("revenue" == sActivityKind)
				return ActivityKind.REVENUE;
			else if ("reattribution" == sActivityKind)
				return ActivityKind.REATTRIBUTION;
			else 
				return ActivityKind.UNKNOWN;
		}
	}
}

