using System;
using System.Runtime.InteropServices;

namespace com.adjust.sdk.test
{
	#if UNITY_IOS
	public class TestLibraryiOS
	{
		[DllImport("__Internal")]
		private static extern void _ATLStartTestSession(string clientSdk);

		[DllImport("__Internal")]
		private static extern void _ATLAddInfoToSend(string key, string paramValue);

		[DllImport("__Internal")]
		private static extern void _ATLSendInfoToServer(string basePath);

		public TestLibraryiOS () { }

		public static void StartTestSession(string clientSdk)
		{
			_ATLStartTestSession (clientSdk);
		}

		public static void AddInfoToSend(string key, string paramValue)
		{
			_ATLAddInfoToSend (key, paramValue);
		}

		public static void SendInfoToServer(string basePath)
		{
			_ATLSendInfoToServer (basePath);
		}
	}
	#endif
}

