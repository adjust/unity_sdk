using System;

namespace com.adjust.sdk.test
{
    public class TestFactoryIOS : ITestFactory
    {
        private string _baseUrl;

        public TestFactoryIOS (string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public void StartTestSession(string testNames = null) 
        {
            TestApp.Log ("TestFactory -> StartTestSession()");
			TestLibraryiOS.StartTestSession (TestApp.CLIENT_SDK);
        }

        public void AddInfoToSend(string key, string paramValue) 
        {
			TestLibraryiOS.AddInfoToSend (key, paramValue);
        }

        public void SendInfoToServer(string basePath) 
        {
			TestLibraryiOS.SendInfoToServer (basePath);
        }
    }
}

