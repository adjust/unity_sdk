using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustAttribution
    {
        public string adid { get; set; }
        public string network { get; set; }
        public string adgroup { get; set; }
        public string campaign { get; set; }
        public string creative { get; set; }
        public string clickLabel { get; set; }
        public string trackerName { get; set; }
        public string trackerToken { get; set; }

        public AdjustAttribution() {}

        public AdjustAttribution(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            trackerName = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTrackerName);
            trackerToken = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTrackerToken);
            network = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyNetwork);
            campaign = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCampaign);
            adgroup = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdgroup);
            creative = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCreative);
            clickLabel = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyClickLabel);
            adid = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdid);
        }

        public AdjustAttribution(Dictionary<string, string> dicAttributionData)
        {
            if (dicAttributionData == null)
            {
                return;
            }

            trackerName = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
            trackerToken = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
            network = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
            campaign = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
            adgroup = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
            creative = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
            clickLabel = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
            adid = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
        }
    }
}
