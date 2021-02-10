using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustThirdPartySharing
    {
        internal bool? isEnabled;
        internal Dictionary<string, List<string>> granularOptions;

        public AdjustThirdPartySharing(bool? isEnabled)
        {
            this.isEnabled = isEnabled;
            this.granularOptions = new Dictionary<string, List<string>>();
        }

        public void addGranularOption(string partnerName, string key, string value)
        {
            // TODO: consider to add some logs about the error case
            if (partnerName == null || key == null || value == null)
            {
                return;
            }

            List<string> partnerOptions;
            if (granularOptions.ContainsKey(partnerName))
            {
                partnerOptions = granularOptions[partnerName];
            }
            else
            {
                partnerOptions = new List<string>();
                granularOptions.Add(partnerName, partnerOptions);
            }

            partnerOptions.Add(key);
            partnerOptions.Add(value);
        }
    }
}
