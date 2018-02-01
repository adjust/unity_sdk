using System;
using UnityEngine;

namespace com.adjust.sdk.test
{
	public class TestApp : MonoBehaviour
	{
		public static readonly string TAG = "[TestApp]";

		private TestFactory _testFactory;
		private static bool _isLaunched = false;

		//private string _baseUrl = "https://10.0.2.2:8443";
		private string _baseUrl = "https://192.168.8.158:8443";
	
		void OnGUI()
		{
			if (_isLaunched) { return; }

			if (_testFactory == null) 
			{
				_testFactory = new TestFactory(_baseUrl);
			}

			Log ("Starting test session...");
			_testFactory.StartTestSession();
			_isLaunched = true;
		}

		public static void Log(string message, bool useUnityDebug = false)
		{
			string currentTimeString = DateTime.Now.ToShortDateString ();
			string output = string.Format ("[{0}{1}]: {2}", currentTimeString, TAG, message);
			if(!useUnityDebug)
				Console.WriteLine (output);
			else
				Debug.Log (output);
		}

		public static void LogError(string message, bool useUnityDebug = false)
		{
			string currentTimeString = DateTime.Now.ToShortDateString ();
			string output = string.Format ("[{0}{1}][Error!]: {2}", currentTimeString, TAG, message);
			if(!useUnityDebug)
				Console.WriteLine (output);
			else
				Debug.Log (output);
		}
	}
}

