using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adjust.sdk.imei
{
#if UNITY_ANDROID
    public class AdjustImeiAndroid
    {
        private static AndroidJavaClass ajcAdjustImei = new AndroidJavaClass("com.adjust.sdk.imei.AdjustImei");

        public static void ReadImei()
        {
            if (ajcAdjustImei == null)
            {
                ajcAdjustImei = new AndroidJavaClass("com.adjust.sdk.imei.AdjustImei");
            }
            ajcAdjustImei.CallStatic("readImei");
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
