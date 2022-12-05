using System;
using UnityEngine;

namespace com.adjust.sdk.samsung
{
    public class AdjustSamsungReferrer : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdjustSamsungReferrer]: Adjust Samsung Referrer plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdjustSamsungReferrer]: Adjust Samsung Referrer plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readSamsungReferrer = false;

        void Awake()
        {
            if (IsEditor()) { return; }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readSamsungReferrer)
                {
                    AdjustSamsungReferrer.ReadSamsungReferrer();
                }
                else
                {
                    AdjustSamsungReferrer.DoNotReadSamsungReferrer();
                }
            }
        }

        public static void ReadSamsungReferrer()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustSamsungReferrerAndroid.ReadSamsungReferrer();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadSamsungReferrer()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustSamsungReferrerAndroid.DoNotReadSamsungReferrer();
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
