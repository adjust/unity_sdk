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

		// Android specific members
		internal string processName;

        // Windows specific members
        internal Action<String> logDelegate;

        public AdjustConfig (string appToken, AdjustEnvironment environment)
		{
			this.sceneName = "";
			this.appToken = appToken;
			this.environment = environment;
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

		// Android specific methods
		public void setProcessName (string processName)
		{
			this.processName = processName;
		}

        public void setLogDelegate(Action<String> logDelegate)
        {
            this.logDelegate = logDelegate;
        }
	}
}
