using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustAttribution
	{
		#region Properties
		public string network { get; set; }
		public string adgroup { get; set; }
		public string campaign { get; set; }
		public string creative { get; set; }
		public string clickLabel { get; set; }
		public string trackerName { get; set; }
		public string trackerToken { get; set; }
		#endregion

		#region Constructors
		public AdjustAttribution ()
		{
		}

		public AdjustAttribution (string jsonString)
		{
			var jsonNode = JSON.Parse (jsonString);
			
			if (jsonNode == null)
			{
				return;
			}

			trackerName = AdjustUtils.GetJsonString (jsonNode, "trackerName");
			trackerToken = AdjustUtils.GetJsonString (jsonNode, "trackerToken");
			network = AdjustUtils.GetJsonString (jsonNode, "network");
			campaign = AdjustUtils.GetJsonString (jsonNode, "campaign");
			adgroup = AdjustUtils.GetJsonString (jsonNode, "adgroup");
			creative = AdjustUtils.GetJsonString (jsonNode, "creative");
			clickLabel = AdjustUtils.GetJsonString (jsonNode, "clickLabel");
		}

		public AdjustAttribution (Dictionary<string, string> dicAttributionData)
		{
			if (dicAttributionData == null)
			{
				return;
			}

			trackerName = TryGetValue (dicAttributionData, "trackerName");
			trackerToken = TryGetValue (dicAttributionData, "trackerToken");
			network = TryGetValue (dicAttributionData, "network");
			campaign = TryGetValue (dicAttributionData, "campaign");
			adgroup = TryGetValue (dicAttributionData, "adgroup");
			creative = TryGetValue (dicAttributionData, "creative");
			clickLabel = TryGetValue (dicAttributionData, "clickLabel");
		}
		#endregion

		#region Private & helper methods
		private static string TryGetValue (Dictionary<string, string> dic, string key)
		{
			string value;

			if (dic.TryGetValue (key, out value))
			{
				return value;
			} 
			else
			{
				return null;
			}
		}
		#endregion
	}
}
