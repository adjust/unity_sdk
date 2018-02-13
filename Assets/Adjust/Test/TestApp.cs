#if UNITY_ANDROID
using System;
#endif
using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestApp : MonoBehaviour
    {
        public static readonly string TAG = "[TestApp]";
        private static bool _isLaunched = false;

#if (UNITY_WSA || UNITY_WP8)
        public const string CLIENT_SDK = "unity4.12.0@wuap4.12.0";
        private const string PORT = ":8080";
        private const string PROTOCOL = "http://";
#elif UNITY_ANDROID
        public const string CLIENT_SDK = "unity4.12.0@android4.12.0";
        private const string PORT = ":8443";
        private const string PROTOCOL = "https://";
#elif UNITY_IOS
        public const string CLIENT_SDK = "unity4.12.0@ios.12.0";
        private const string PORT = ":8443";
        private const string PROTOCOL = "https://";
#endif

        //private const string BASE_URL = PROTOCOL + "10.0.2.2" + PORT;
        //private const string BASE_URL = PROTOCOL + "192.168.8.171" + PORT;
        private const string BASE_URL = PROTOCOL + "localhost" + PORT;

        void OnGUI()
        {
            if (_isLaunched) { return; }

            ITestFactory testFactory = GetPlatformSpecificTestLibrary ();

            // set specific tests to run
            string testNames = GetTestNames();
            //testNames = null;

            Log ("Starting test session...");
            testFactory.StartTestSession(testNames);
            _isLaunched = true;
        }

        private ITestFactory GetPlatformSpecificTestLibrary()
        {
#if UNITY_IOS
            return new TestFactoryIOS(BASE_URL);
#elif UNITY_ANDROID
            return new TestFactoryAndroid(BASE_URL);
#elif (UNITY_WSA || UNITY_WP8)
            return new TestFactoryWindows(BASE_URL);
#else
            Debug.Log("Cannot run integration tests (Error in TestApp.GetPlatformSpecificTestLibrary(...)). None of the supported platforms selected.");
#endif
        }

        private string GetTestNames()
        {
            string testDir = "current/";
            string testNames = "";

            testNames += testDir + "event/Test_Event_OrderId;";
            testNames += testDir + "event/Test_Event_Params;";
            testNames += testDir + "event/Test_Event_Count_6events;";

            //testNames += testDir + "sessionCount/Test_SessionCount;";
            //testNames += testDir + "referrer/Test_ReftagReferrer_before_install_killw_in_between;";
            // push token saved before start. sdk killed and restarted - saved push token not being read
            // error message: Adjust not initialized correctly
            //testNames += testDir + "sdkInfo/Test_PushToken_before_install_kill_in_between;";
            // setPushToken called between start and resume
            // sdk_info package being sent, but seems like it should not
            //testNames += testDir + "sdkInfo/Test_PushToken_between_create_and_resume;";

            return testNames;
        }

        public static void Log(string message, bool useUnityDebug = false)
        {
#if UNITY_ANDROID
            var now = DateTime.Now;
            string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
            string output = string.Format("[{0}{1}]: {2}", currentTimeString, TAG, message);
            if (!useUnityDebug)
                Console.WriteLine(output);
            else
                Debug.Log(output);
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
                Console.WriteLine(output);
            else
                Debug.Log(output);
#else
            Debug.LogError(message);
#endif
        }
    }
}

