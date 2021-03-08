#if (UNITY_ANDROID || UNITY_IOS)
using System;
#endif
using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestApp : MonoBehaviour
    {
        public static readonly string TAG = "[TestApp]";

#if (UNITY_STANDALONE_WIN)
        private const string PORT = ":8080";
        private const string PROTOCOL = "http://";
        private const string IP = "localhost";
#elif UNITY_ANDROID
        private const string PORT = ":8443";
        private const string PROTOCOL = "https://";
        private const string IP = "192.168.86.50";
#elif UNITY_IOS
        private const string PORT = ":8080";
        private const string PROTOCOL = "http://";
        private const string IP = "192.168.86.50";
        private TestLibraryiOS _testLibraryiOS;
#endif
        private const string BASE_URL = PROTOCOL + IP + PORT;
        private const string GDPR_URL = PROTOCOL + IP + PORT;
        private const string SUBSCRIPTION_URL = PROTOCOL + IP + PORT;
        private const string CONTROL_URL = "ws://" + IP + ":1987";

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, Screen.height * 0 / 2, Screen.width, Screen.height / 2), "Start test"))
            {
                StartTestSession();
            }
        }

        private void StartTestSession() 
        {
            ITestLibrary testLibrary = GetPlatformSpecificTestLibrary();
#if UNITY_IOS
            _testLibraryiOS = testLibrary as TestLibraryiOS;
#endif
            // Set specific tests to run.
            // testLibrary.AddTest("current/eventBuffering/Test_EventBuffering_sensitive_packets");
            // testLibrary.AddTest("Test_ThirdPartySharing_after_install");
            // testLibrary.AddTestDirectory ("current/deeplink-deferred");

            Log("Starting test session.");
            testLibrary.StartTestSession();
        }

        private ITestLibrary GetPlatformSpecificTestLibrary()
        {
#if UNITY_IOS
            return new TestLibraryiOS(BASE_URL, GDPR_URL, SUBSCRIPTION_URL, CONTROL_URL);
#elif UNITY_ANDROID
            return new TestLibraryAndroid(BASE_URL, GDPR_URL, SUBSCRIPTION_URL, CONTROL_URL);
#elif (UNITY_WSA || UNITY_WP8)
            return new TestLibraryWindows(BASE_URL, CONTROL_URL, GDPR_URL);
#else
            Debug.Log("Cannot run integration tests (Error in TestApp.GetPlatformSpecificTestLibrary(...)). None of the supported platforms selected.");
            return null;
#endif
        }

#if UNITY_IOS
        public void ExecuteCommand(string commandJson)
        {
            _testLibraryiOS.ExecuteCommand(commandJson);
        }
#endif
        public static void Log(string message, bool useUnityDebug = false)
        {
#if UNITY_ANDROID
            var now = DateTime.Now;
            string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
            string output = string.Format("[{0}{1}]: {2}", currentTimeString, TAG, message);
            if (!useUnityDebug)
            {
                Console.WriteLine(output);
            }
            else
            {
                Debug.Log(output);
            }
#else
            Debug.Log(message);
#endif
        }

        public static void LogError(string message, bool useUnityDebug = false)
        {
#if UNITY_ANDROID
            var now = DateTime.Now;
            string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
            string output = string.Format("[{0}{1}][Error!]: {2}", currentTimeString, TAG, message);
            if (!useUnityDebug)
            {
                Console.WriteLine(output);
            }
            else
            {
                Debug.Log(output);
            }
#else
            Debug.LogError(message);
#endif
        }
    }
}
