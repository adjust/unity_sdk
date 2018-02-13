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

	}
	#endif
}

