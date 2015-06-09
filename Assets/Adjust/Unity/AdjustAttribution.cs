using System;

using SimpleJSON;

namespace com.adjust.sdk
{
	public class AdjustAttribution
	{
		public string trackerToken { get; set; }

		public string trackerName { get; set; }

		public string network { get; set; }

		public string campaign { get; set; }

		public string adgroup { get; set; }

		public string creative { get; set; }

		public AdjustAttribution ()
		{
		}

		public AdjustAttribution (string jsonString)
		{
			var jsonNode = JSON.Parse (jsonString);
			
			if (jsonNode == null) {
				return;
			}

			trackerName = getJsonString (jsonNode, "trackerName");
			trackerToken = getJsonString (jsonNode, "trackerToken");
			network = getJsonString (jsonNode, "network");
			campaign = getJsonString (jsonNode, "campaign");
			adgroup = getJsonString (jsonNode, "adgroup");
			creative = getJsonString (jsonNode, "creative");
		}
		
		private String getJsonString (JSONNode node, string key)
		{
			var jsonValue = getJsonValue (node, key);
			
			if (jsonValue == null) {
				return null;
			}
			
			return jsonValue.Value;
		}
		
		private JSONNode getJsonValue (JSONNode node, string key)
		{
			if (node == null) {
				return null;
			}
			
			var nodeValue = node[key];

			if (nodeValue.GetType () == typeof(JSONLazyCreator)) {
				return null;
			}
			
			return nodeValue;
		}
	}
}
