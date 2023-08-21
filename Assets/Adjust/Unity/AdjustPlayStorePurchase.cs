using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustPlayStorePurchase
    {
        internal string productId;
        internal string purchaseToken;

        public AdjustPlayStorePurchase(string productId, string purchaseToken)
        {
            this.productId = productId;
            this.purchaseToken = purchaseToken;
        }
    }
}
