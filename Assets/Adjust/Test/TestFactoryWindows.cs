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

        public TestFactoryWindows(string baseUrl)
        {
            IAdjustCommandExecutor adjustCommandExecutor = new CommandExecutor(this, baseUrl);
            _testLibraryInterface.Init(adjustCommandExecutor, baseUrl, UnityEngine.Debug.Log);
        }

        public void StartTestSession()
        {
            TestApp.Log("TestFactory -> StartTestSession()");
			// TODO: add possibility to add specific tests and/or test dirs to WinSDK Bridge
			string testNames = null;
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

		public void AddTest(string testName)
		{
			// TODO:
			throw new NotImplementedException ();
		}

		public void AddTestDirectory(string testDirectory)
		{
			// TODO:
			throw new NotImplementedException ();
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
