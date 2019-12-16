using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adjust.sdk.imei
{
#if UNITY_ANDROID
    public class AdjustImeiAndroid
    {
        private static AndroidJavaClass ajcAdjustImei = new AndroidJavaClass("com.adjust.sdk.imei.AdjustImei");
        private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        public static void ReadImei()
        {
            if (ajcAdjustImei == null)
            {
                ajcAdjustImei = new AndroidJavaClass("com.adjust.sdk.imei.AdjustImei");
            }
            ajcAdjustImei.CallStatic("readImei", ajoCurrentActivity);
        }

        public static void DoNotReadImei()
        {
            if (ajcAdjustImei == null)
            {
                ajcAdjustImei = new AndroidJavaClass("com.adjust.sdk.imei.AdjustImei");
            }
            ajcAdjustImei.CallStatic("doNotReadImei");
        }
    }
#endif
}
