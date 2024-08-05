#if (UNITY_ANDROID || UNITY_IOS)
using System;
#endif
using UnityEngine;
using AdjustSdk.Test;

public class TestApp : MonoBehaviour
{
    public static readonly string TAG = "[TestApp]";

#if UNITY_ANDROID
    private const string PORT = ":8443";
    private const string PROTOCOL = "https://";
    private const string IP = "192.168.0.27";
#elif UNITY_IOS
    private const string PORT = ":8080";
    private const string PROTOCOL = "http://";
    private const string IP = "192.168.0.27";
    private TestLibraryiOS _testLibraryiOS;
#else
    private const string PORT = ":8080";
    private const string PROTOCOL = "http://";
    private const string IP = "localhost";
#endif
    private const string OVERWRITE_URL = PROTOCOL + IP + PORT;
    private const string CONTROL_URL = "ws://" + IP + ":1987";

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, Screen.height * 0 / 2, Screen.width, Screen.height / 2), "Start test"))
        {
            StartTestSession();
        }
    }

    private void StartTestSession() 
    {
        ITestLibrary testLibrary = GetPlatformSpecificTestLibrary();
#if UNITY_IOS
        _testLibraryiOS = testLibrary as TestLibraryiOS;
#endif
        // Set specific tests to run.
        // testLibrary.AddTest("Test_ThirdPartySharing_after_install");
        // testLibrary.AddTestDirectory("purchase-verification");

        Log("Starting test session.");
        testLibrary.StartTestSession();
    }

    private ITestLibrary GetPlatformSpecificTestLibrary()
    {
#if UNITY_IOS
        return new TestLibraryiOS(OVERWRITE_URL, CONTROL_URL);
#elif UNITY_ANDROID
        return new TestLibraryAndroid(OVERWRITE_URL, CONTROL_URL);
#else
        Debug.Log("Cannot run integration tests (Error in TestApp.GetPlatformSpecificTestLibrary(...)). None of the supported platforms selected.");
        return null;
#endif
    }

#if UNITY_IOS
    public void ExecuteCommand(string commandJson)
    {
        _testLibraryiOS.ExecuteCommand(commandJson);
    }
#endif
    public static void Log(string message, bool useUnityDebug = false)
    {
#if UNITY_ANDROID
        var now = DateTime.Now;
        string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
        string output = string.Format("[{0}{1}]: {2}", currentTimeString, TAG, message);
        if (!useUnityDebug)
        {
            Console.WriteLine(output);
        }
        else
        {
            Debug.Log(output);
        }
#else
        Debug.Log(message);
#endif
    }

    public static void LogError(string message, bool useUnityDebug = false)
    {
#if UNITY_ANDROID
        var now = DateTime.Now;
        string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
        string output = string.Format("[{0}{1}][Error!]: {2}", currentTimeString, TAG, message);
        if (!useUnityDebug)
        {
            Console.WriteLine(output);
        }
        else
        {
            Debug.Log(output);
        }
#else
        Debug.LogError(message);
#endif
    }
}
