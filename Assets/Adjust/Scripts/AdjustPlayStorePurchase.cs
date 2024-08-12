using System.Collections.Generic;

namespace AdjustSdk
{
    public class AdjustPlayStorePurchase
    {
        public string ProductId { get; private set; }
        public string PurchaseToken { get; private set; }

        public AdjustPlayStorePurchase(string productId, string purchaseToken)
        {
            this.ProductId = productId;
            this.PurchaseToken = purchaseToken;
        }
    }
}
