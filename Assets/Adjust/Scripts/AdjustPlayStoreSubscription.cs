using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdjustSdk
{
    public class AdjustPlayStoreSubscription
    {
        private List<string> innerCallbackParameters;
        private List<string> innerPartnerParameters;

        public string Price { get; private set; }
        public string Currency { get; private set; }
        public string ProductId { get; private set; }
        public string OrderId { get; private set; }
        public string Signature { get; private set; }
        public string PurchaseToken { get; private set; }
        public string PurchaseTime { get; set; }
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

        public AdjustPlayStoreSubscription(
            string price,
            string currency,
            string productId,
            string orderId,
            string signature,
            string purchaseToken)
        {
            this.Price = price;
            this.Currency = currency;
            this.ProductId = productId;
            this.OrderId = orderId;
            this.Signature = signature;
            this.PurchaseToken = purchaseToken;
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
