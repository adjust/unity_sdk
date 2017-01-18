using System;
using System.Collections.Generic;

namespace com.adjust.sdk {
    public class AdjustAttribution {
        #region Properties
        public string adid {get; set; }
        public string network { get; set; }
        public string adgroup { get; set; }
        public string campaign { get; set; }
        public string creative { get; set; }
        public string clickLabel { get; set; }
        public string trackerName { get; set; }
        public string trackerToken { get; set; }
        #endregion

        #region Constructors
        public AdjustAttribution() {}

        public AdjustAttribution(string jsonString) {
            var jsonNode = JSON.Parse(jsonString);
            
            if (jsonNode == null) {
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

        public AdjustAttribution(Dictionary<string, string> dicAttributionData) {
            if (dicAttributionData == null) {
                return;
            }

            trackerName = TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
            trackerToken = TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
            network = TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
            campaign = TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
            adgroup = TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
            creative = TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
            clickLabel = TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
            adid = TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
        }
        #endregion

        #region Private & helper methods
        private static string TryGetValue(Dictionary<string, string> dic, string key) {
            string value;

            if (dic.TryGetValue(key, out value)) {
                return value;
            } else {
                return null;
            }
        }
        #endregion
    }
}
