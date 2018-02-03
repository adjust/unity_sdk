using System;
using UnityEngine;

namespace com.adjust.sdk.test
{
	public class TestApp : MonoBehaviour
	{
		public static readonly string TAG = "[TestApp]";

		private TestFactory _testFactory;
		private static bool _isLaunched = false;

		private string _baseUrl = "https://10.0.2.2:8443";
		//private string _baseUrl = "https://192.168.8.170:8443";
	
		void OnGUI()
		{
			if (_isLaunched) { return; }

			if (_testFactory == null) 
			{
				_testFactory = new TestFactory(_baseUrl);
			}

			// set specific tests to run
			string testNames = GetTestNames();

			Log ("Starting test session...");
			_testFactory.StartTestSession(testNames);
			_isLaunched = true;
		}

		private string GetTestNames()
		{
			string testDir = "current/";
			string testNames = "";

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

