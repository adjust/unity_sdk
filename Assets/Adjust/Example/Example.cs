using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using AdjustSdk;

public class Example : MonoBehaviour
{
    private int numberOfButtons = 8;
    private bool isEnabled;
    private bool showPopUp = false;
    private string txtSetEnabled = "Disable SDK";
    private string txtManualLaunch = "Manual Launch";
    private string txtSetOfflineMode = "Turn Offline Mode ON";

    void OnGUI()
    {
        if (showPopUp)
        {
            GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 65, 300, 130), ShowGUI, "Is SDK enabled?");
        }

        if (GUI.Button(new Rect(0, Screen.height * 0 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtManualLaunch))
        {
            if (!string.Equals(txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
            {
                AdjustConfig adjustConfig = new AdjustConfig("2fm9gkqubvpc", AdjustEnvironment.Sandbox);
                adjustConfig.LogLevel = AdjustLogLevel.Verbose;
                adjustConfig.AttributionChangedDelegate = AttributionChangedCallback;
                adjustConfig.DeferredDeeplinkDelegate = DeferredDeeplinkCallback;
                Adjust.InitSdk(adjustConfig);

                isEnabled = true;
                txtManualLaunch = "SDK Launched";
            }
        }
        
        if (GUI.Button(new Rect(0, Screen.height * 1 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Simple Event"))
        {
            AdjustEvent adjustEvent = new AdjustEvent("g3mfiw");
            Adjust.TrackEvent(adjustEvent);
        }

        if (GUI.Button(new Rect(0, Screen.height * 2 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Revenue Event"))
        {
            AdjustEvent adjustEvent = new AdjustEvent("a4fd35");
            adjustEvent.SetRevenue(0.25, "EUR");
            Adjust.TrackEvent(adjustEvent);
        }

        if (GUI.Button(new Rect(0, Screen.height * 3 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Callback Event"))
        {
            AdjustEvent adjustEvent = new AdjustEvent("34vgg9");
            adjustEvent.AddCallbackParameter("key", "value");
            adjustEvent.AddCallbackParameter("foo", "bar");
            Adjust.TrackEvent(adjustEvent);
        }

        if (GUI.Button(new Rect(0, Screen.height * 4 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Partner Event"))
        {
            AdjustEvent adjustEvent = new AdjustEvent("w788qs");
            adjustEvent.AddPartnerParameter("key", "value");
            adjustEvent.AddPartnerParameter("foo", "bar");
            Adjust.TrackEvent(adjustEvent);
        }

        if (GUI.Button(new Rect(0, Screen.height * 5 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtSetOfflineMode))
        {
            if (string.Equals(txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
            {
                Adjust.SwitchToOfflineMode();
                txtSetOfflineMode = "Turn Offline Mode OFF";
            }
            else
            {
                Adjust.SwitchBackToOnlineMode();
                txtSetOfflineMode = "Turn Offline Mode ON";
            }
        }

        if (GUI.Button(new Rect(0, Screen.height * 6 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtSetEnabled))
        {
            if (string.Equals(txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
            {
                Adjust.Disable();
                txtSetEnabled = "Enable SDK";
            }
            else
            {
                Adjust.Enable();
                txtSetEnabled = "Disable SDK";
            }
        }

        if (GUI.Button(new Rect(0, Screen.height * 7 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Is SDK Enabled?"))
        {
            Adjust.IsEnabled(enabled =>
            {
                isEnabled = enabled;
                showPopUp = true;
            });
        }
    }

    void ShowGUI(int windowID)
    {
        if (isEnabled)
        {
            GUI.Label(new Rect(65, 40, 200, 30), "Adjust SDK is ENABLED!");
        }
        else
        {
            GUI.Label(new Rect(65, 40, 200, 30), "Adjust SDK is DISABLED!");
        }
       
        if (GUI.Button(new Rect(90, 75, 120, 40), "OK"))
        {
            showPopUp = false;
        }
    }

    public void HandleGooglePlayId(String adId)
    {
        Debug.Log("Google Play Ad ID = " + adId);
    }

    public void AttributionChangedCallback(AdjustAttribution attributionData)
    {
        Debug.Log("Attribution changed!");

        if (attributionData.TrackerName != null)
        {
            Debug.Log("Tracker name: " + attributionData.TrackerName);
        }
        if (attributionData.TrackerToken != null)
        {
            Debug.Log("Tracker token: " + attributionData.TrackerToken);
        }
        if (attributionData.Network != null)
        {
            Debug.Log("Network: " + attributionData.Network);
        }
        if (attributionData.Campaign != null)
        {
            Debug.Log("Campaign: " + attributionData.Campaign);
        }
        if (attributionData.Adgroup != null)
        {
            Debug.Log("Adgroup: " + attributionData.Adgroup);
        }
        if (attributionData.Creative != null)
        {
            Debug.Log("Creative: " + attributionData.Creative);
        }
        if (attributionData.ClickLabel != null)
        {
            Debug.Log("Click label: " + attributionData.ClickLabel);
        }
    }

    public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
    {
        Debug.Log("Event tracked successfully!");

        if (eventSuccessData.Message != null)
        {
            Debug.Log("Message: " + eventSuccessData.Message);
        }
        if (eventSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
        }
        if (eventSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + eventSuccessData.Adid);
        }
        if (eventSuccessData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventSuccessData.EventToken);
        }
        if (eventSuccessData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventSuccessData.CallbackId);
        }
        if (eventSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponseAsString());
        }
    }

    public void EventFailureCallback(AdjustEventFailure eventFailureData)
    {
        Debug.Log("Event tracking failed!");

        if (eventFailureData.Message != null)
        {
            Debug.Log("Message: " + eventFailureData.Message);
        }
        if (eventFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventFailureData.Timestamp);
        }
        if (eventFailureData.Adid != null)
        {
            Debug.Log("Adid: " + eventFailureData.Adid);
        }
        if (eventFailureData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventFailureData.EventToken);
        }
        if (eventFailureData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventFailureData.CallbackId);
        }
        if (eventFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponseAsString());
        }

        Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
    }

    public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
    {
        Debug.Log("Session tracked successfully!");

        if (sessionSuccessData.Message != null)
        {
            Debug.Log("Message: " + sessionSuccessData.Message);
        }
        if (sessionSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
        }
        if (sessionSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + sessionSuccessData.Adid);
        }
        if (sessionSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponseAsString());
        }
    }

    public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
    {
        Debug.Log("Session tracking failed!");

        if (sessionFailureData.Message != null)
        {
            Debug.Log("Message: " + sessionFailureData.Message);
        }
        if (sessionFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
        }
        if (sessionFailureData.Adid != null)
        {
            Debug.Log("Adid: " + sessionFailureData.Adid);
        }
        if (sessionFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponseAsString());
        }

        Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
    }

    private void DeferredDeeplinkCallback(string deeplinkURL)
    {
        Debug.Log("Deferred deeplink reported!");

        if (deeplinkURL != null)
        {
            Debug.Log("Deeplink URL: " + deeplinkURL);
        }
        else
        {
            Debug.Log("Deeplink URL is null!");
        }
    }
}
