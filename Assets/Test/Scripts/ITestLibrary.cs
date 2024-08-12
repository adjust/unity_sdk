namespace AdjustSdk.Test
{
    public interface ITestLibrary
    {
        void StartTestSession();
        void AddInfoToSend(string key, string paramValue);
        void SendInfoToServer(string basePath);
        void AddTest(string testName);
        void AddTestDirectory(string testDirectory);
    }
}
