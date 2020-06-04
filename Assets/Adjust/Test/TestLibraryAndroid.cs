using UnityEngine;

namespace com.adjust.sdk.test
{
#if UNITY_ANDROID
    public class TestLibraryAndroid : ITestLibrary
    {
        private AndroidJavaObject ajoTestLibrary;
        private CommandListenerAndroid onCommandReceivedListener;

        public TestLibraryAndroid(string baseUrl, string gdprUrl, string subscriptionUrl, string controlUrl)
        {
            CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl, gdprUrl, subscriptionUrl);
            onCommandReceivedListener = new CommandListenerAndroid(commandExecutor);
            ajoTestLibrary = new AndroidJavaObject(
                "com.adjust.test.TestLibrary",
                baseUrl,
                controlUrl,
                onCommandReceivedListener);
        }

        public void StartTestSession()
        {
            ajoTestLibrary.Call("startTestSession", Adjust.getSdkVersion());
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
#endif
}
