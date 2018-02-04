using System;
using UnityEngine;
using com.adjust.sdk;

namespace com.adjust.sdk.test
{
	public class TestFactoryAndroid : ITestFactory {
		private string _baseUrl;
		private AndroidJavaObject ajoTestLibrary;

		private static CommandListener onCommandReceivedListener;

		public TestFactoryAndroid(string baseUrl) {
			_baseUrl = baseUrl;

			CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl);
			onCommandReceivedListener = new CommandListener(commandExecutor);
		}

		public void StartTestSession(string testNames = null) 
		{
			TestApp.Log ("TestFactory -> StartTestSession()");

			if (ajoTestLibrary == null) 
			{
				ajoTestLibrary = new AndroidJavaObject("com.adjust.testlibrary.TestLibrary", this._baseUrl, onCommandReceivedListener);
			}

			if (!string.IsNullOrEmpty (testNames)) 
			{
				ajoTestLibrary.Call("setTests", testNames);
			}

			TestApp.Log ("TestFactory -> calling testLib.startTestSession()");
			ajoTestLibrary.Call("startTestSession", "unity4.12.0@android4.12.0");
		}

		public void Teardown(bool shutdownNow) {
			if (ajoTestLibrary == null) { return; }
			ajoTestLibrary.Call("teardown", shutdownNow);
		}

		public void AddInfoToSend(string key, string paramValue) {
			if (ajoTestLibrary == null) { return; }
			ajoTestLibrary.Call("addInfoToSend", key, paramValue);
		}

		public void SendInfoToServer(string basePath) {
			if (ajoTestLibrary == null) { return; }
			ajoTestLibrary.Call("sendInfoToServer", basePath);
		}
	}
}
