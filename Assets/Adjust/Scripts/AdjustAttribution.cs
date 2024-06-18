using System;
using System.Collections.Generic;

namespace AdjustSdk
{
    public class AdjustAttribution
    {
        public string TrackerToken { get; set; }
        public string TrackerName { get; set; }
        public string Network { get; set; }
        public string Campaign { get; set; }
        public string Adgroup { get; set; }
        public string Creative { get; set; }
        public string ClickLabel { get; set; }
        public string CostType { get; set; }
        public double? CostAmount { get; set; }
        public string CostCurrency { get; set; }
        // Android only
        public string FbInstallReferrer { get; set; }

        public AdjustAttribution() {}

        public AdjustAttribution(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            this.TrackerName = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTrackerName);
            this.TrackerToken = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTrackerToken);
            this.Network = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyNetwork);
            this.Campaign = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCampaign);
            this.Adgroup = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdgroup);
            this.Creative = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCreative);
            this.ClickLabel = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyClickLabel);
            this.CostType = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCostType);
            try
            {
                this.CostAmount = double.Parse(
                    AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCostAmount),
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // attribution response doesn't contain cost amount attached
                // value will default to null
            }
            this.CostCurrency = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCostCurrency);
            this.FbInstallReferrer = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyFbInstallReferrer);
        }

        public AdjustAttribution(Dictionary<string, string> dicAttributionData)
        {
            if (dicAttributionData == null)
            {
                return;
            }

            this.TrackerName = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
            this.TrackerToken = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
            this.Network = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
            this.Campaign = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
            this.Adgroup = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
            this.Creative = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
            this.ClickLabel = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
            this.CostType = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostType);
            try
            {
                this.CostAmount = double.Parse(
                    AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostAmount),
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // attribution response doesn't contain cost amount attached
                // value will default to null
            }
            this.CostCurrency = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCostCurrency);
            this.FbInstallReferrer = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyFbInstallReferrer);
        }
    }
}
