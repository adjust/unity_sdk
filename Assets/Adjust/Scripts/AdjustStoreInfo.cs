using System;

namespace AdjustSdk
{
    public class AdjustStoreInfo
    {
        public string StoreName { get; private set; }
        public string StoreAppId { get; set; }

        public AdjustStoreInfo(string storeName)
        {
            this.StoreName = storeName;
        }
    }
}
