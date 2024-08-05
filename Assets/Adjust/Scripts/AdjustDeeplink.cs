using System;

namespace AdjustSdk
{
    public class AdjustDeeplink
    {
        public string Deeplink { get; private set; }

        public AdjustDeeplink(string deeplink)
        {
            this.Deeplink = deeplink;
        }
    }
}
