using System;
using System.Collections.Generic;

namespace com.adjust.sdk {
    public interface IAdjust {
        bool isEnabled();
        string getAdid();
        AdjustAttribution getAttribution();

        void onPause();
        void onResume();
        void sendFirstPackages();

        void setEnabled(bool enabled);
        void setOfflineMode(bool enabled);
        void setDeviceToken(string deviceToken);

        void start(AdjustConfig adjustConfig);
        void trackEvent(AdjustEvent adjustEvent);

        // iOS specific methods
        string getIdfa();

        // Android specific methods
        void setReferrer(string referrer);
        void getGoogleAdId(Action<string> onDeviceIdsRead);
    }
}
