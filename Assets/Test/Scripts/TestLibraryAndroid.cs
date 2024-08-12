using UnityEngine;

namespace AdjustSdk.Test
{
#if UNITY_ANDROID
    public class TestLibraryAndroid : ITestLibrary
    {
        private AndroidJavaObject ajoTestLibrary;
        private CommandListenerAndroid onCommandReceivedListener;

        public TestLibraryAndroid(string overwriteUrl, string controlUrl)
        {
            CommandExecutor commandExecutor = new CommandExecutor(this, overwriteUrl);
            onCommandReceivedListener = new CommandListenerAndroid(commandExecutor);
            ajoTestLibrary = new AndroidJavaObject(
                "com.adjust.test.TestLibrary",
                overwriteUrl,
                controlUrl,
                onCommandReceivedListener);
        }

        public void StartTestSession()
        {
            Adjust.GetSdkVersion(sdkVersion =>
            {
                ajoTestLibrary.Call("startTestSession", sdkVersion);
            });
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
