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
            // TODO:

            TestApp.Log ("TestFactory -> calling testLib.startTestSession()");
            //TODO: 

        }

        public void AddInfoToSend(string key, string paramValue) 
        {
            //TODO: 

        }

        public void SendInfoToServer(string basePath) 
        {
            //TODO: 

        }
    }
}

