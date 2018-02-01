using System;
using UnityEngine;
using com.adjust.sdk;

namespace com.adjust.sdk.test
{
	public class TestFactory {
		private string baseUrl;
		private AndroidJavaObject ajoTestLibrary;

		private static CommandListener onCommandReceivedListener;

		public TestFactory(string baseUrl) {
			this.baseUrl = baseUrl;

			CommandExecutor commandExecutor = new CommandExecutor(this, baseUrl);
			onCommandReceivedListener = new CommandListener(commandExecutor);
		}

		public void StartTestSession() 
		{
			TestApp.Log ("TestFactory -> StartTestSession()");

			if (ajoTestLibrary == null) {
				ajoTestLibrary = new AndroidJavaObject("com.adjust.testlibrary.TestLibrary", this.baseUrl, onCommandReceivedListener);
			}

			TestApp.Log ("TestFactory -> calling testLib.startTestSession()");
			ajoTestLibrary.Call("startTestSession", "unity4.12.0@android4.12.0");
		}

		public void Teardown(bool shutdownNow) {
			if (ajoTestLibrary == null) { return; }
			ajoTestLibrary.Call("teardown", shutdownNow);
		}

		public void SetTests(string testNames)
		{
			if (ajoTestLibrary == null) { return; }
			ajoTestLibrary.Call("setTests", testNames);
		}

		public void AddInfoToSend(string key, string paramValue) {
			ajoTestLibrary.Call("addInfoToSend", key, paramValue);
		}

		public void SendInfoToServer(string basePath) {
			ajoTestLibrary.Call("sendInfoToServer", basePath);
		}
	}
}
