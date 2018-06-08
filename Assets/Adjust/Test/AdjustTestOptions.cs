using System;
using UnityEngine;

namespace com.adjust.sdk.test
{
    public class AdjustTestOptions
    {
        public string BaseUrl { get; set; }
        public string GdprUrl { get; set; }
        public string BasePath { get; set; }
        public string GdprPath { get; set; }
        public bool? Teardown { get; set; }
        public bool? DeleteState { get; set; }
        public bool? UseTestConnectionOptions { get; set; }
        public bool? NoBackoffWait { get; set; }

        // Default value => Constants.ONE_MINUTE
        public long? TimerIntervalInMilliseconds { get; set; }
        // Default value => Constants.ONE_MINUTE
        public long? TimerStartInMilliseconds { get; set; }
        // Default value => Constants.THIRTY_MINUTES
        public long? SessionIntervalInMilliseconds { get; set; }
        // Default value => Constants.ONE_SECOND
        public long? SubsessionIntervalInMilliseconds { get; set; }

#if UNITY_ANDROID
        public AndroidJavaObject ToAndroidJavaObject(AndroidJavaObject ajoCurrentActivity)
        {
            AndroidJavaObject ajoTestOptions = new AndroidJavaObject("com.adjust.sdk.AdjustTestOptions");
            ajoTestOptions.Set<String>("baseUrl", BaseUrl);
            ajoTestOptions.Set<String>("gdprUrl", GdprUrl);

            if (!string.IsNullOrEmpty(BasePath)) 
            {
                ajoTestOptions.Set<String>("basePath", BasePath);
            }
            if (!string.IsNullOrEmpty(GdprPath)) 
            {
                ajoTestOptions.Set<String>("gdprPath", GdprPath);
            }
            if (DeleteState.GetValueOrDefault(false) && ajoCurrentActivity != null)
            {
                ajoTestOptions.Set<AndroidJavaObject>("context", ajoCurrentActivity);
            }
            if (UseTestConnectionOptions.HasValue) 
            {
                AndroidJavaObject ajoUseTestConnectionOptions = new AndroidJavaObject("java.lang.Boolean", UseTestConnectionOptions.Value);
                ajoTestOptions.Set<AndroidJavaObject>("useTestConnectionOptions", ajoUseTestConnectionOptions);
            }
            if (TimerIntervalInMilliseconds.HasValue) 
            {
                AndroidJavaObject ajoTimerIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", TimerIntervalInMilliseconds.Value);
                ajoTestOptions.Set<AndroidJavaObject>("timerIntervalInMilliseconds", ajoTimerIntervalInMilliseconds);
            }
            if (TimerStartInMilliseconds.HasValue) 
            {
                AndroidJavaObject ajoTimerStartInMilliseconds = new AndroidJavaObject("java.lang.Long", TimerStartInMilliseconds.Value);
                ajoTestOptions.Set<AndroidJavaObject>("timerStartInMilliseconds", ajoTimerStartInMilliseconds);
            }
            if (SessionIntervalInMilliseconds.HasValue) 
            {
                AndroidJavaObject ajoSessionIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", SessionIntervalInMilliseconds.Value);
                ajoTestOptions.Set<AndroidJavaObject>("sessionIntervalInMilliseconds", ajoSessionIntervalInMilliseconds);
            }
            if (SubsessionIntervalInMilliseconds.HasValue) 
            {
                AndroidJavaObject ajoSubsessionIntervalInMilliseconds = new AndroidJavaObject("java.lang.Long", SubsessionIntervalInMilliseconds.Value);
                ajoTestOptions.Set<AndroidJavaObject>("subsessionIntervalInMilliseconds", ajoSubsessionIntervalInMilliseconds);
            }
            if (Teardown.HasValue)
            {
                AndroidJavaObject ajoTeardown = new AndroidJavaObject("java.lang.Boolean", Teardown.Value);
                ajoTestOptions.Set<AndroidJavaObject>("teardown", ajoTeardown);
            }
            if (NoBackoffWait.HasValue)
            {
                AndroidJavaObject ajoNoBackoffWait = new AndroidJavaObject("java.lang.Boolean", NoBackoffWait.Value);
                ajoTestOptions.Set<AndroidJavaObject>("noBackoffWait", ajoNoBackoffWait);
            }

            return ajoTestOptions;
        }
#endif
    }
}
