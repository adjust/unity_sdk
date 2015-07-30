using System;

namespace com.adjust.sdk
{
	public class AdjustConfig
	{
		internal string appToken;
		internal string sceneName;
		internal string defaultTracker;

		internal bool? startAutomatically;
		internal bool? eventBufferingEnabled;

		internal AdjustLogLevel? logLevel;
		internal AdjustEnvironment environment;
		internal Action<AdjustAttribution> attributionChangedDelegate;

		public AdjustConfig (string appToken, AdjustEnvironment environment)
		{
			this.sceneName = "";
			this.appToken = appToken;
			this.environment = environment;
			this.startAutomatically = false;
		}

		public void setLogLevel (AdjustLogLevel logLevel)
		{
			this.logLevel = logLevel;
		}

		public void setStartAutomatically (bool shouldStartAutomatically)
		{
			this.startAutomatically = shouldStartAutomatically;
		}

		public void setDefaultTracker (string defaultTracker)
		{
			this.defaultTracker = defaultTracker;
		}

		public void setEventBufferingEnabled (bool eventBufferingEnabled)
		{
			this.eventBufferingEnabled = eventBufferingEnabled;
		}

		public void setAttributionChangedDelegate(Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
		{
			this.attributionChangedDelegate = attributionChangedDelegate;
			this.sceneName = sceneName;
		}

		public Action<AdjustAttribution> getAttributionChangedDelegate()
		{
			return this.attributionChangedDelegate;
		}
	}
}
