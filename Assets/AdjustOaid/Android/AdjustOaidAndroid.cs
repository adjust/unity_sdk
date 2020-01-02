using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adjust.sdk.oaid
{
#if UNITY_ANDROID
    public class AdjustOaidAndroid
    {
        private static AndroidJavaClass ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.oaid.AdjustOaid");
		private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadOaid()
        {
            if (ajcAdjustOaid == null)
            {
                ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.oaid.AdjustOaid");
            }
            ajcAdjustOaid.CallStatic("readOaid", ajoCurrentActivity);
        }

        public static void DoNotReadOaid()
        {
            if (ajcAdjustOaid == null)
            {
                ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.oaid.AdjustOaid");
            }
            ajcAdjustOaid.CallStatic("doNotReadOaid");
        }
    }
#endif
}
