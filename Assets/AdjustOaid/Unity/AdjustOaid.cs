using System;
using UnityEngine;

namespace com.adjust.sdk.oaid
{
    public class AdjustOaid : MonoBehaviour
    {
        private const string errorMsgEditor = "[AdjustOaid]: Adjust OAID plugin can not be used in Editor.";
        private const string errorMsgPlatform = "[AdjustOaid]: Adjust OAID plugin can only be used in Android apps.";

        public bool startManually = true;
        public bool readOaid = false;

        void Awake()
        {
            if (IsEditor()) { return; }

            DontDestroyOnLoad(transform.gameObject);

            if (!this.startManually)
            {
                if (this.readOaid)
                {
                    AdjustOaid.ReadOaid();
                }
                else
                {
                    AdjustOaid.DoNotReadOaid();
                }
            }
        }

        public static void ReadOaid()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustOaidAndroid.ReadOaid();
#else
            Debug.Log(errorMsgPlatform);
#endif
        }

        public static void DoNotReadOaid()
        {
            if (IsEditor()) { return; }

#if UNITY_ANDROID
            AdjustOaidAndroid.DoNotReadOaid();
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
