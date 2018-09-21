using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestLibraryAndroid : ITestLibrary
    {
        private AndroidJavaObject ajoTestLibrary;
        private CommandListenerAndroid onCommandReceivedListener;

        public TestLibraryAndroid(string baseUrl, string gdprUrl)
        {
            CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl, gdprUrl);
            onCommandReceivedListener = new CommandListenerAndroid(commandExecutor);
            ajoTestLibrary = new AndroidJavaObject(
                "com.adjust.testlibrary.TestLibrary",
                baseUrl,
                onCommandReceivedListener);
        }

        public void StartTestSession()
        {
            ajoTestLibrary.Call("startTestSession", TestApp.CLIENT_SDK);
        }

        public void AddInfoToSend(string key, string paramValue)
        {
            ajoTestLibrary.Call("addInfoToSend", key, paramValue);
        }

        public void SendInfoToServer(string basePath)
        {
            ajoTestLibrary.Call("sendInfoToServer", basePath);
        }

        public void AddTest(string testName)
        {
            ajoTestLibrary.Call("addTest", testName);
        }

        public void AddTestDirectory(string testDirectory)
        {
            ajoTestLibrary.Call("addTestDirectory", testDirectory);
        }
    }
}
