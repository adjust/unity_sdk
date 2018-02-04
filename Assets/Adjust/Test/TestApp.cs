using System;
using UnityEngine;

namespace com.adjust.sdk.test
{
	public class TestApp : MonoBehaviour
	{
		public static readonly string TAG = "[TestApp]";
		private static bool _isLaunched = false;
		private const string BASE_URL = "https://10.0.2.2:8443";
		//private const string BASE_URL = "https://192.168.8.170:8443";
	
		void OnGUI()
		{
			if (_isLaunched) { return; }

			ITestFactory testFactory = GetPlatformSpecificTestLibrary ();

			// set specific tests to run
			string testNames = GetTestNames();
			testNames = null;

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

			testNames += testDir + "sessionCount/Test_SessionCount;";
			testNames += testDir + "referrer/Test_ReftagReferrer_before_install_killw_in_between;";
			// push token saved before start. sdk killed and restarted - saved push token not being read
			// error message: Adjust not initialized correctly
			testNames += testDir + "sdkInfo/Test_PushToken_before_install_kill_in_between;";
			// setPushToken called between start and resume
			// sdk_info package being sent, but seems like it should not
			testNames += testDir + "sdkInfo/Test_PushToken_between_create_and_resume;";

			return testNames;
		}

		public static void Log(string message, bool useUnityDebug = false)
		{
			var now = DateTime.Now;
			string currentTimeString = string.Format ("{0}:{1}", now.ToShortTimeString (), now.Second);
			string output = string.Format ("[{0}{1}]: {2}", currentTimeString, TAG, message);
			if(!useUnityDebug)
				Console.WriteLine (output);
			else
				Debug.Log (output);
		}

		public static void LogError(string message, bool useUnityDebug = false)
		{
			var now = DateTime.Now;
			string currentTimeString = string.Format ("{0}:{1}", now.ToShortTimeString (), now.Second);
			string output = string.Format ("[{0}{1}][Error!]: {2}", currentTimeString, TAG, message);
			if(!useUnityDebug)
				Console.WriteLine (output);
			else
				Debug.Log (output);
		}
	}
}

