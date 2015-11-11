using System;

namespace com.adjust.sdk
{
	public class AdjustConfig
	{
		internal string appToken;
		internal string sceneName;
		internal string defaultTracker;

		internal bool? eventBufferingEnabled;

		internal AdjustLogLevel? logLevel;
		internal AdjustEnvironment environment;
		internal Action<AdjustAttribution> attributionChangedDelegate;

		// iOS specific members
		internal bool macMd5TrackingEnabled;

		// Android specific members
		internal string processName;

		public AdjustConfig (string appToken, AdjustEnvironment environment)
		{
			this.sceneName = "";
			this.appToken = appToken;
			this.environment = environment;
			this.macMd5TrackingEnabled = false;
			this.processName = "";
		}

		public void setLogLevel (AdjustLogLevel logLevel)
		{
			this.logLevel = logLevel;
		}

		public void setDefaultTracker (string defaultTracker)
		{
			this.defaultTracker = defaultTracker;
		}

		public void setEventBufferingEnabled (bool eventBufferingEnabled)
		{
			this.eventBufferingEnabled = eventBufferingEnabled;
		}

		public void setAttributionChangedDelegate (Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
		{
			this.attributionChangedDelegate = attributionChangedDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustAttribution> getAttributionChangedDelegate ()
		{
			return this.attributionChangedDelegate;
		}

		// iOS specific methods
		public void setMacMd5TrackingEnabled (bool macMd5TrackingEnabled)
		{
			this.macMd5TrackingEnabled = macMd5TrackingEnabled;
		}

		// Android specific methods
		public void setProcessName (string processName)
		{
			this.processName = processName;
		}
	}
}
