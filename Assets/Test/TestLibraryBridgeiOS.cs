using System;
using System.Runtime.InteropServices;

namespace com.adjust.sdk.test
{
    #if UNITY_IOS
    public static class TestLibraryBridgeiOS
    {
        [DllImport("__Internal")]
        private static extern void _ATLInitialize(string baseUrl, string controlUrl);

        [DllImport("__Internal")]
        private static extern void _ATLStartTestSession(string clientSdk);

        [DllImport("__Internal")]
        private static extern void _ATLAddInfoToSend(string key, string paramValue);

        [DllImport("__Internal")]
        private static extern void _ATLSendInfoToServer(string basePath);

        [DllImport("__Internal")]
        private static extern void _ATLAddTest(string testName);

        [DllImport("__Internal")]
        private static extern void _ATLAddTestDirectory(string testDirectory);

        public static void Initialize(string baseUrl, string controlUrl)
        {
            _ATLInitialize(baseUrl, controlUrl);
        }

        public static void StartTestSession(string clientSdk)
        {
            _ATLStartTestSession(clientSdk);
        }

        public static void AddInfoToSend(string key, string paramValue)
        {
            _ATLAddInfoToSend(key, paramValue);
        }

        public static void SendInfoToServer(string basePath)
        {
            _ATLSendInfoToServer(basePath);
        }

        public static void AddTest(string testName)
        {
            _ATLAddTest(testName);
        }

        public static void AddTestDirectory(string testDirectory)
        {
            _ATLAddTestDirectory(testDirectory);
        }
    }
    #endif
}
