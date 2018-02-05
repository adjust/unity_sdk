using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestFactoryAndroid : ITestFactory
    {
        private string _baseUrl;
        private AndroidJavaObject ajoTestLibrary;
        private CommandListener onCommandReceivedListener;

        public TestFactoryAndroid(string baseUrl)
        {
            _baseUrl = baseUrl;
            CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl);
            onCommandReceivedListener = new CommandListener(commandExecutor);
        }

        public void StartTestSession(string testNames = null)
        {
            TestApp.Log("TestFactory -> StartTestSession()");

            if (ajoTestLibrary == null)
            {
                ajoTestLibrary = new AndroidJavaObject("com.adjust.testlibrary.TestLibrary", _baseUrl,
                    onCommandReceivedListener);
            }

            if (!string.IsNullOrEmpty(testNames))
            {
                ajoTestLibrary.Call("setTests", testNames);
            }

            TestApp.Log("TestFactory -> calling testLib.startTestSession()");
            ajoTestLibrary.Call("startTestSession", "unity4.12.0@android4.12.0");
        }

        public void Teardown(bool shutdownNow)
        {
            if (ajoTestLibrary == null) { return; }
            ajoTestLibrary.Call("teardown", shutdownNow);
        }

        public void AddInfoToSend(string key, string paramValue)
        {
            if (ajoTestLibrary == null) { return; }
            ajoTestLibrary.Call("addInfoToSend", key, paramValue);
        }

        public void SendInfoToServer(string basePath)
        {
            if (ajoTestLibrary == null) { return; }
            ajoTestLibrary.Call("sendInfoToServer", basePath);
        }
    }
}

