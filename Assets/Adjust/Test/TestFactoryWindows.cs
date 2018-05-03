#if (UNITY_WSA || UNITY_WP8)
using TestLibraryInterface;
#endif
using System;

namespace com.adjust.sdk.test
{
    public class TestFactoryWindows : ITestFactory
    {
#if (UNITY_WSA || UNITY_WP8)
        private readonly TestLibraryInterface.TestLibraryInterface _testLibraryInterface
            = new TestLibraryInterface.TestLibraryInterface();

        public TestFactoryWindows(string baseUrl, string gdprUrl)
        {
            IAdjustCommandExecutor adjustCommandExecutor = new CommandExecutor(this, baseUrl, gdprUrl);
            _testLibraryInterface.Init(adjustCommandExecutor, baseUrl, UnityEngine.Debug.Log);
        }

        public void StartTestSession()
        {
            TestApp.Log("TestFactory -> StartTestSession()");
            _testLibraryInterface.StartTestSession(TestApp.CLIENT_SDK);
        }

        public void AddInfoToSend(string key, string paramValue)
        {
            _testLibraryInterface.AddInfoToSend(key, paramValue);
        }

        public void SendInfoToServer(string basePath)
        {
            _testLibraryInterface.SendInfoToServer(basePath);
        }

        public void AddTest(string testName)
        {
            _testLibraryInterface.AddTest(testName);
        }

        public void AddTestDirectory(string testDirectory)
        {
            _testLibraryInterface.AddTestDirectory(testDirectory);
        }
#else
        public void StartTestSession()
        {
            throw new NotImplementedException();
        }
            
        public void AddInfoToSend(string key, string paramValue)
        {
            throw new NotImplementedException();
        }

        public void SendInfoToServer(string basePath)
        {
            throw new NotImplementedException();
        }

        public void AddTest(string testName)
        {
            throw new NotImplementedException();
        }

        public void AddTestDirectory(string testDirectory)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
