namespace com.adjust.sdk.test
{
	public interface ITestFactory
	{
		void StartTestSession(string testNames = null);
		void Teardown(bool shutdownNow);
		void AddInfoToSend(string key, string paramValue);
		void SendInfoToServer(string basePath);
	}
}

