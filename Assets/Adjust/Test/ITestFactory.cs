namespace com.adjust.sdk.test
{
	public interface ITestFactory
	{
		void StartTestSession(string testNames = null);
		void AddInfoToSend(string key, string paramValue);
		void SendInfoToServer(string basePath);
	}
}

