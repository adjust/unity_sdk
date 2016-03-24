using System;
using System.Collections;
using System.Runtime.InteropServices;

using UnityEngine;
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour
{
	private int nr_buttons = 8;

	private static bool isEnabled;
	private bool showPopUp = false;

	private string txtSetEnabled = "Disable SDK";
	private string txtManualLaunch = "Manual Launch";
	private string txtSetOfflineMode = "Turn Offline Mode ON";
	
	void OnGUI ()
	{
		if (showPopUp)
		{
			GUI.Window(0, new Rect ((Screen.width / 2) - 150, (Screen.height / 2) - 65, 300, 130), showGUI, "Is SDK enabled?");
		}

		if (GUI.Button (new Rect (0, Screen.height * 0 / nr_buttons, Screen.width, Screen.height / nr_buttons), txtManualLaunch))
		{
			if (!string.Equals (txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
			{
				AdjustConfig adjustConfig = new AdjustConfig ("{YourAppToken}", AdjustEnvironment.Sandbox);
				adjustConfig.setLogLevel (AdjustLogLevel.Verbose);
				adjustConfig.setAttributionChangedDelegate (this.AttributionChangedCallback);
				
				Adjust.start (adjustConfig);
				isEnabled = true;

				txtManualLaunch = "SDK Launched";
			}
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 1 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Simple Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent ("{YourEventToken}");
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 2 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Revenue Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent ("{YourEventToken}");
			adjustEvent.setRevenue (0.25, "EUR");
			
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 3 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Callback Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent ("{YourEventToken}");
			
			adjustEvent.addCallbackParameter ("key", "value");
			adjustEvent.addCallbackParameter ("foo", "bar");
			
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 4 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Partner Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent ("{YourEventToken}");
			
			adjustEvent.addPartnerParameter ("key", "value");
			adjustEvent.addPartnerParameter ("foo", "bar");
			
			Adjust.trackEvent (adjustEvent);
		}

		if (GUI.Button (new Rect (0, Screen.height * 5 / nr_buttons, Screen.width, Screen.height / nr_buttons), txtSetOfflineMode))
		{
			if (string.Equals (txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setOfflineMode (true);
				txtSetOfflineMode = "Turn Offline Mode OFF";
			}
			else
			{
				Adjust.setOfflineMode (false);
				txtSetOfflineMode = "Turn Offline Mode ON";
			}
		}

		if (GUI.Button (new Rect (0, Screen.height * 6 / nr_buttons, Screen.width, Screen.height / nr_buttons), txtSetEnabled))
		{
			if (string.Equals (txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setEnabled (false);
				txtSetEnabled = "Enable SDK";
			}
			else
			{
				Adjust.setEnabled (true);
				txtSetEnabled = "Disable SDK";
			}
		}

		if (GUI.Button (new Rect (0, Screen.height * 7 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Is SDK Enabled?"))
		{
			isEnabled = Adjust.isEnabled ();
			showPopUp = true;
		}
	}

	void showGUI (int windowID)
    {
    	if (isEnabled)
    	{
        	GUI.Label (new Rect (65, 40, 200, 30), "Adjust SDK is ENABLED!");
    	}
    	else
    	{
    		GUI.Label (new Rect (65, 40, 200, 30), "Adjust SDK is DISABLED!");
    	}
       
        if (GUI.Button (new Rect (90, 75, 120, 40), "OK"))
        {
            showPopUp = false;
        }
    }
	
	public void AttributionChangedCallback (AdjustAttribution attributionData)
	{
		Debug.Log ("Attribution changed!");

		if (attributionData.trackerName != null)
		{
			Debug.Log ("trackerName " + attributionData.trackerName);
		}

		if (attributionData.trackerToken != null)
		{
			Debug.Log ("trackerToken " + attributionData.trackerToken);
		}

		if (attributionData.network != null)
		{
			Debug.Log ("network " + attributionData.network);
		}

		if (attributionData.campaign != null)
		{
			Debug.Log ("campaign " + attributionData.campaign);
		}

		if (attributionData.adgroup != null)
		{
			Debug.Log ("adgroup " + attributionData.adgroup);
		}

		if (attributionData.creative != null)
		{
			Debug.Log ("creative " + attributionData.creative);
		}

		if (attributionData.clickLabel != null)
		{
			Debug.Log ("clickLabel" + attributionData.clickLabel);
		}
	}

	public void EventSuccessCallback (AdjustEventSuccess eventSuccessData)
	{
		Debug.Log ("Event tracked successfully!");

		if (eventSuccessData.Message != null)
		{
			Debug.Log ("Message: " + eventSuccessData.Message);
		}

		if (eventSuccessData.Timestamp != null)
		{
			Debug.Log ("Timestamp: " + eventSuccessData.Timestamp);
		}

		if (eventSuccessData.Adid != null)
		{
			Debug.Log ("Adid: " + eventSuccessData.Adid);
		}

		if (eventSuccessData.EventToken != null)
		{
			Debug.Log ("EventToken: " + eventSuccessData.EventToken);
		}

		if (eventSuccessData.JsonResponse != null)
		{
			Debug.Log ("JsonResponse: ");
			eventSuccessData.PrintJsonResponse ();
		}
	}

	public void EventFailureCallback (AdjustEventFailure eventFailureData)
	{
		Debug.Log ("Event tracking failed!");

		if (eventFailureData.Message != null)
		{
			Debug.Log ("Message: " + eventFailureData.Message);
		}

		if (eventFailureData.Timestamp != null)
		{
			Debug.Log ("Timestamp: " + eventFailureData.Timestamp);
		}

		if (eventFailureData.Adid != null)
		{
			Debug.Log ("Adid: " + eventFailureData.Adid);
		}

		if (eventFailureData.EventToken != null)
		{
			Debug.Log ("EventToken: " + eventFailureData.EventToken);
		}

		Debug.Log ("WillRetry: " + eventFailureData.WillRetry.ToString ());

		if (eventFailureData.JsonResponse != null)
		{
			Debug.Log ("JsonResponse: ");
			eventFailureData.PrintJsonResponse ();
		}
	}

	public void SessionSuccessCallback (AdjustSessionSuccess sessionSuccessData)
	{
		Debug.Log ("Session tracked successfully!");

		if (sessionSuccessData.Message != null)
		{
			Debug.Log ("Message: " + sessionSuccessData.Message);
		}

		if (sessionSuccessData.Timestamp != null)
		{
			Debug.Log ("Timestamp: " + sessionSuccessData.Timestamp);
		}

		if (sessionSuccessData.Adid != null)
		{
			Debug.Log ("Adid: " + sessionSuccessData.Adid);
		}

		if (sessionSuccessData.JsonResponse != null)
		{
			Debug.Log ("JsonResponse: ");
			sessionSuccessData.PrintJsonResponse ();
		}
	}

	public void SessionFailureCallback (AdjustSessionFailure sessionFailureData)
	{
		Debug.Log ("Session tracking failed!");

		if (sessionFailureData.Message != null)
		{
			Debug.Log ("Message: " + sessionFailureData.Message);
		}

		if (sessionFailureData.Timestamp != null)
		{
			Debug.Log ("Timestamp: " + sessionFailureData.Timestamp);
		}

		if (sessionFailureData.Adid != null)
		{
			Debug.Log ("Adid: " + sessionFailureData.Adid);
		}

		Debug.Log ("WillRetry: " + sessionFailureData.WillRetry.ToString ());

		if (sessionFailureData.JsonResponse != null)
		{
			Debug.Log ("JsonResponse: ");
			sessionFailureData.PrintJsonResponse ();
		}
	}
}
