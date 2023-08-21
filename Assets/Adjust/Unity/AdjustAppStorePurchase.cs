using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustAppStorePurchase
    {
        internal string transactionId;
        internal string productId;
        internal string receipt;

        public AdjustAppStorePurchase(string transactionId, string productId, string receipt)
        {
            this.transactionId = transactionId;
            this.productId = productId;
            this.receipt = receipt;
        }
    }
}
