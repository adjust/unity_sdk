using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public interface IAdjust
	{
		bool isEnabled();

		void onPause ();
		void onResume ();
		void setEnabled (bool enabled);
		void setOfflineMode (bool enabled);
		void start (AdjustConfig adjustConfig);
		void trackEvent (AdjustEvent adjustEvent);

		// iOS specific methods
		void setDeviceToken (string deviceToken);

		// Android specific methods
		void setReferrer (string referrer);
	}
}
