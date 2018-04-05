namespace com.adjust.sdk.test
{
    public interface ITestFactory
    {
        void StartTestSession();
        void AddInfoToSend(string key, string paramValue);
        void SendInfoToServer(string basePath);
		void AddTest(string testName);
		void AddTestDirectory(string testDirectory);
    }
}
