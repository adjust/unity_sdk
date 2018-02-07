using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestFactoryAndroid : ITestFactory
    {
        private string _baseUrl;
        private AndroidJavaObject ajoTestLibrary;
        private CommandListenerAndroid onCommandReceivedListener;

        public TestFactoryAndroid(string baseUrl)
        {
            _baseUrl = baseUrl;
            CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl);
            onCommandReceivedListener = new CommandListenerAndroid(commandExecutor);
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
            ajoTestLibrary.Call("startTestSession", TestApp.CLIENT_SDK);
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

