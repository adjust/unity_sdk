using System;

namespace AdjustSdk
{
    public class AdjustDeeplink
    {
        public string Deeplink { get; private set; }
        public string Referrer { get; set; }

        public AdjustDeeplink(string deeplink)
        {
            this.Deeplink = deeplink;
        }
    }
}
