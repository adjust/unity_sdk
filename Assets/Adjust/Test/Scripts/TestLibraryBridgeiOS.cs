using System;
using System.Runtime.InteropServices;

namespace AdjustSdk.Test
{
    #if UNITY_IOS
    public static class TestLibraryBridgeiOS
    {
        [DllImport("__Internal")]
        private static extern void _ATLInitialize(string overwriteUrl, string controlUrl);

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

        public static void Initialize(string overwriteUrl, string controlUrl)
        {
            _ATLInitialize(overwriteUrl, controlUrl);
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
