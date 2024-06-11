using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustEvent
    {
        internal string eventToken;
        internal double? revenue;
        internal string currency;
        internal string callbackId;
        internal string deduplicationId;
        internal List<string> partnerList;
        internal List<string> callbackList;
        // iOS specific
        internal string receipt;
        internal string productId;
        internal string transactionId;
        // Android specific
        internal string orderId;
        internal string purchaseToken;

        public AdjustEvent(string eventToken)
        {
            this.eventToken = eventToken;
        }

        public void SetRevenue(double amount, string currency)
        {
            this.revenue = amount;
            this.currency = currency;
        }

        public void AddCallbackParameter(string key, string value)
        {
            if (callbackList == null)
            {
                callbackList = new List<string>();
            }
            callbackList.Add(key);
            callbackList.Add(value);
        }

        public void AddPartnerParameter(string key, string value)
        {
            if (partnerList == null)
            {
                partnerList = new List<string>();
            }
            partnerList.Add(key);
            partnerList.Add(value);
        }

        public void SetCallbackId(string callbackId)
        {
            this.callbackId = callbackId;
        }

        public void SetDeduplicationId(string deduplicationId)
        {
            this.deduplicationId = deduplicationId;
        }

        public void SetProductId(string productId)
        {
            this.productId = productId;
        }

        // iOS specific
        public void SetTransactionId(string transactionId)
        {
            this.transactionId = transactionId;
        }

        public void SetReceipt(string receipt)
        {
            this.receipt = receipt;
        }

        // Android specific
        public void SetOrderId(string orderId)
        {
            this.orderId = orderId;
        }

        public void SetPurchaseToken(string purchaseToken)
        {
            this.purchaseToken = purchaseToken;
        }
    }
}
