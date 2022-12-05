using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adjust.sdk.samsung
{
#if UNITY_ANDROID
    public class AdjustSamsungReferrerAndroid
    {
        private static AndroidJavaClass ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.samsung.AdjustSamsungReferrer");
		private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadSamsungReferrer()
        {
            if (ajcAdjustOaid == null)
            {
                ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.samsung.AdjustSamsungReferrer");
            }
            ajcAdjustOaid.CallStatic("readSamsungReferrer", ajoCurrentActivity);
        }

        public static void DoNotReadSamsungReferrer()
        {
            if (ajcAdjustOaid == null)
            {
                ajcAdjustOaid = new AndroidJavaClass("com.adjust.sdk.samsung.AdjustSamsungReferrer");
            }
            ajcAdjustOaid.CallStatic("doNotReadSamsungReferrer");
        }
    }
#endif
}
