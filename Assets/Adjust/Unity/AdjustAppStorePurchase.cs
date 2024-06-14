using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustAppStorePurchase
    {
        public string TransactionId { get; private set; }
        public string ProductId { get; private set; } 
        public string Receipt { get; private set; }

        public AdjustAppStorePurchase(string transactionId, string productId, string receipt)
        {
            this.TransactionId = transactionId;
            this.ProductId = productId;
            this.Receipt = receipt;
        }
    }
}
