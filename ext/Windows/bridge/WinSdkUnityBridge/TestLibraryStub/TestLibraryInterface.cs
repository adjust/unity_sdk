using System;

namespace TestLibraryInterface
{
    public class TestLibraryInterface
    {
        public void Init(IAdjustCommandExecutor adjustCommandExecutor, string baseUrl, Action<string> logDelegate = null)
        {
        }

        public static void SetTestOptions(AdjustTestOptionsDto adjustTestOptionsDto)
        {
        }

        public void StartTestSession(string clientSdk, string testNames = null)
        {
        }

        public void AddInfoToSend(string key, string paramValue)
        {
        }

        public void SendInfoToServer(string basePath)
        {
        }
    }
}
