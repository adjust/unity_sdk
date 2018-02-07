#if (UNITY_WSA || UNITY_WP8)
using TestLibraryInterface;
#else
using System;
#endif

namespace com.adjust.sdk.test
{
    public class TestFactoryWindows : ITestFactory
    {
#if (UNITY_WSA || UNITY_WP8)
        private readonly TestLibraryInterface.TestLibraryInterface _testLibraryInterface
            = new TestLibraryInterface.TestLibraryInterface();

        public TestFactoryWindows(string baseUrl)
        {
            IAdjustCommandExecutor adjustCommandExecutor = new CommandExecutor(this, baseUrl);
            _testLibraryInterface.Init(adjustCommandExecutor, baseUrl, UnityEngine.Debug.Log);
        }

        public void StartTestSession(string testNames = null)
        {
            TestApp.Log("TestFactory -> StartTestSession()");
            _testLibraryInterface.StartTestSession(TestApp.CLIENT_SDK, testNames);
        }

        public void AddInfoToSend(string key, string paramValue)
        {
            _testLibraryInterface.AddInfoToSend(key, paramValue);
        }

        public void SendInfoToServer(string basePath)
        {
            _testLibraryInterface.SendInfoToServer(basePath);
        }
#else
        public void AddInfoToSend(string key, string paramValue)
        {
            throw new NotImplementedException();
        }

        public void SendInfoToServer(string basePath)
        {
            throw new NotImplementedException();
        }

        public void StartTestSession(string testNames = null)
        {
            throw new NotImplementedException();
        }
#endif
    }
}

