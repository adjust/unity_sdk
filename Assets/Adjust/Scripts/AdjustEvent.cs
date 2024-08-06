using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdjustSdk
{
    public class AdjustEvent
    {
        private List<string> innerCallbackParameters;
        private List<string> innerPartnerParameters;

        public string EventToken { get; private set; }
        public double? Revenue { get; private set; }
        public string Currency { get; private set; }
        public string CallbackId { get; set; }
        public string DeduplicationId { get; set; }
        public string ProductId { get; set; }
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
        // ios specific
        public string TransactionId { get; set; }
        // android specific
        public string PurchaseToken;

        public AdjustEvent(string eventToken)
        {
            this.EventToken = eventToken;
        }

        public void SetRevenue(double amount, string currency)
        {
            this.Revenue = amount;
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
