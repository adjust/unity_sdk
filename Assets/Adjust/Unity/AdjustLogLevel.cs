namespace com.adjust.sdk
{
	public enum AdjustLogLevel
	{
		Verbose = 1,
		Debug,
		Info,
		Warn,
		Error,
		Assert
	}
	public static class AdjustLogLevelExtension
	{
		public static string lowercaseToString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
				case AdjustLogLevel.Verbose:
					return "verbose";
				case AdjustLogLevel.Debug:
					return "debug";
				case AdjustLogLevel.Info:
					return "info";
				case AdjustLogLevel.Warn:
					return "warn";
				case AdjustLogLevel.Error:
					return "error";
				case AdjustLogLevel.Assert:
					return "assert";
				default:
					return "unknown";
			}
		}

		public static string uppercaseToString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
				case AdjustLogLevel.Verbose:
					return "VERBOSE";
				case AdjustLogLevel.Debug:
					return "DEBUG";
				case AdjustLogLevel.Info:
					return "INFO";
				case AdjustLogLevel.Warn:
					return "WARN";
				case AdjustLogLevel.Error:
					return "ERROR";
				case AdjustLogLevel.Assert:
					return "ASSERT";
				default:
					return "unknown";
			}
		}

	}

}
