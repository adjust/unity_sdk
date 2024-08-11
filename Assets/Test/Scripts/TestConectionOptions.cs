using UnityEngine;

namespace AdjustSdk.Test
{
#if UNITY_ANDROID
    public class TestConnectionOptions
    {
        public static void SetTestConnectionOptions()
        {
            AndroidJavaClass ajcTestConnectionOptions = new AndroidJavaClass("com.adjust.test_options.TestConnectionOptions");
            ajcTestConnectionOptions.CallStatic("setTestConnectionOptions");
        }
    }
#endif
}
