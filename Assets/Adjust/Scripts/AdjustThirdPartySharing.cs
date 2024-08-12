using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdjustSdk
{
    public class AdjustThirdPartySharing
    {
        private List<string> innerGranularOptions;
        private List<string> innerPartnerSharingSettings;

        public bool? IsEnabled { get; private set; }
        public ReadOnlyCollection<string> GranularOptions
        {
            get
            {
                if (innerGranularOptions == null)
                {
                    return null;
                }
                else
                {
                    return innerGranularOptions.AsReadOnly();
                }
            }
        }
        public ReadOnlyCollection<string> PartnerSharingSettings
        {
            get
            {
                if (innerPartnerSharingSettings == null)
                {
                    return null;
                }
                else
                {
                    return innerPartnerSharingSettings.AsReadOnly();
                }
            }
        }

        public AdjustThirdPartySharing(bool? isEnabled)
        {
            this.IsEnabled = isEnabled;
        }

        public void AddGranularOption(string partnerName, string key, string value)
        {
            if (this.innerGranularOptions == null)
            {
                this.innerGranularOptions = new List<string>();
            }
            this.innerGranularOptions.Add(partnerName);
            this.innerGranularOptions.Add(key);
            this.innerGranularOptions.Add(value);
        }

        public void AddPartnerSharingSetting(string partnerName, string key, bool value)
        {
            if (this.innerPartnerSharingSettings == null)
            {
                this.innerPartnerSharingSettings = new List<string>();
            }
            this.innerPartnerSharingSettings.Add(partnerName);
            this.innerPartnerSharingSettings.Add(key);
            this.innerPartnerSharingSettings.Add(value.ToString());
        }
    }
}
