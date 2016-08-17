using System;
using System.Collections.Generic;

namespace com.adjust.sdk {
    public class AdjustEvent {
        #region Fields
        internal double? revenue;

        internal string currency;
        internal string eventToken;

        internal List<string> partnerList;
        internal List<string> callbackList;

        // iOS specific members
        internal string receipt;
        internal string transactionId;

        internal bool isReceiptSet;
        #endregion

        #region Constructors
        public AdjustEvent(string eventToken) {
            this.eventToken = eventToken;
            this.isReceiptSet = false;
        }
        #endregion

        #region Public methods
        public void setRevenue(double amount, string currency) {
            this.revenue = amount;
            this.currency = currency;
        }

        public void addCallbackParameter(string key, string value) {
            if (callbackList == null) {
                callbackList = new List<string>();
            }

            callbackList.Add(key);
            callbackList.Add(value);
        }

        public void addPartnerParameter(string key, string value) {
            if (partnerList == null) {
                partnerList = new List<string>();
            }

            partnerList.Add(key);
            partnerList.Add(value);
        }

        // iOS specific methods
        public void setTransactionId(string transactionId) {
            this.transactionId = transactionId;
        }

        [Obsolete("This is an obsolete method. Please use the adjust purchase SDK for purchase verification (https://github.com/adjust/unity_purchase_sdk)")]
        public void setReceipt(string receipt, string transactionId) {
            this.receipt = receipt;
            this.transactionId = transactionId;
            this.isReceiptSet = true;
        }
        #endregion
    }
}
