using System;
using UnityEngine;

namespace com.adjust.sdk.imei
{
    public class AdjustImei : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdjustImei]: Adjust IMEI plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdjustImei]: Adjust IMEI plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readImei = false;

        void Awake()
        {
            if (IsEditor()) { return; }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readImei)
                {
                    AdjustImei.ReadImei();
                }
                else
                {
                    AdjustImei.DoNotReadImei();
                }
            }
        }

        public static void ReadImei()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustImeiAndroid.ReadImei();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadImei()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustImeiAndroid.DoNotReadImei();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        private static bool IsEditor()
        {
#if UNITY_EDITOR
            Debug.Log(errorMsgEditor);
            return true;
#else
            return false;
#endif
        }
    }
}
