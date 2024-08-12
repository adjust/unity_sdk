using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdjustSdk
{
    public class AdjustAppStoreSubscription
    {
        private List<string> innerCallbackParameters;
        private List<string> innerPartnerParameters;

        public string Price { get; private set; }
        public string Currency { get; private set; }
        public string TransactionId { get; private set; }
        public string TransactionDate { get; set; }
        public string SalesRegion { get; set; }
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

        public AdjustAppStoreSubscription(string price, string currency, string transactionId)
        {
            this.Price = price;
            this.Currency = currency;
            this.TransactionId = transactionId;
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
