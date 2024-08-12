using System.Collections.Generic;

namespace AdjustSdk
{
    public class AdjustAppStorePurchase
    {
        public string TransactionId { get; private set; }
        public string ProductId { get; private set; }

        public AdjustAppStorePurchase(string transactionId, string productId)
        {
            this.TransactionId = transactionId;
            this.ProductId = productId;
        }
    }
}
