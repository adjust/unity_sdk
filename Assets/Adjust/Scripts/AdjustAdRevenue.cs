using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdjustSdk
{
    public class AdjustAdRevenue
    {
        private List<string> innerCallbackParameters;
        private List<string> innerPartnerParameters;

        public string Source { get; private set; }
        public double? Revenue { get; private set; }
        public string Currency { get; private set; }
        public int? AdImpressionsCount { get; set; }
        public string AdRevenueNetwork { get; set; }
        public string AdRevenueUnit { get; set; }
        public string AdRevenuePlacement { get; set; }
        public ReadOnlyCollection<string> CallbackParameters
        {
            get
            {
                if (innerCallbackParameters == null)
                {
                    return null;
                }
                else
                {
                    return innerCallbackParameters.AsReadOnly();
                }
            }
        }
        public ReadOnlyCollection<string> PartnerParameters
        {
            get
            {
                if (innerPartnerParameters == null)
                {
                    return null;
                }
                else
                {
                    return innerPartnerParameters.AsReadOnly();
                }
            }
        }

        public AdjustAdRevenue(string source)
        {
            this.Source = source;
        }

        public void SetRevenue(double revenue, string currency)
        {
            this.Revenue = revenue;
            this.Currency = currency;
        }

        public void AddCallbackParameter(string key, string value)
        {
            if (this.innerCallbackParameters == null)
            {
                this.innerCallbackParameters = new List<string>();
            }
            this.innerCallbackParameters.Add(key);
            this.innerCallbackParameters.Add(value);
        }

        public void AddPartnerParameter(string key, string value)
        {
            if (this.innerPartnerParameters == null)
            {
                this.innerPartnerParameters = new List<string>();
            }
            this.innerPartnerParameters.Add(key);
            this.innerPartnerParameters.Add(value);
        }
    }
}
