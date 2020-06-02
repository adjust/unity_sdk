using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustAppStoreSubscription
    {
        internal string price;
        internal string currency;
        internal string transactionId;
        internal string receipt;
        internal string billingStore;
        internal string transactionDate;
        internal string salesRegion;
        internal List<string> partnerList;
        internal List<string> callbackList;

        public AdjustAppStoreSubscription(string price, string currency, string transactionId, string receipt)
        {
            this.price = price;
            this.currency = currency;
            this.transactionId = transactionId;
            this.receipt = receipt;
        }

        public void setTransactionDate(string transactionDate)
        {
            this.transactionDate = transactionDate;
        }

        public void setSalesRegion(string salesRegion)
        {
            this.salesRegion = salesRegion;
        }

        public void addCallbackParameter(string key, string value)
        {
            if (callbackList == null)
            {
                callbackList = new List<string>();
            }
            callbackList.Add(key);
            callbackList.Add(value);
        }

        public void addPartnerParameter(string key, string value)
        {
            if (partnerList == null)
            {
                partnerList = new List<string>();
            }
            partnerList.Add(key);
            partnerList.Add(value);
        }
    }
}
