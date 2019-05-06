using System;

namespace TestLibraryInterface
{
    public class TestLibraryInterface
    {
        public void Init(IAdjustCommandExecutor adjustCommandExecutor, string baseUrl, string controlUrl, Action<string> logDelegate = null)
        {
        }

        public static void SetTestOptions(AdjustTestOptionsDto adjustTestOptionsDto)
        {
        }

        public void StartTestSession(string clientSdk)
        {
        }

        public void AddInfoToSend(string key, string paramValue)
        {
        }

        public void SendInfoToServer(string basePath)
        {
        }

        public void AddTest(string testName)
        {
        }

        public void AddTestDirectory(string testDirectory)
        {
        }
    }
}
