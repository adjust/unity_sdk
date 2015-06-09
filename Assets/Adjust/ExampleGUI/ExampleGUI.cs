using System.Collections;
using System.Runtime.InteropServices;

using UnityEngine;
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour
{
	private int nr_buttons = 5;
	private static bool? isEnabled;
	
	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, Screen.height * 0 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Manual Launch")) {
			AdjustConfig adjustConfig = new AdjustConfig ("{YourAppToken}", AdjustEnvironment.Sandbox);
			adjustConfig.setLogLevel (AdjustLogLevel.Verbose);
			adjustConfig.setAttributionChangedDelegate (this.attributionChangedDelegate);
			
			Adjust.start (adjustConfig);
			isEnabled = true;
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 1 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Simple Event")) {
			AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
			
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 2 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Revenue Event")) {
			AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
			adjustEvent.setRevenue (0.25, "EUR");
			
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 3 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Callback Event")) {
			AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
			
			adjustEvent.addCallbackParameter ("key", "value");
			adjustEvent.addCallbackParameter ("foo", "bar");
			
			Adjust.trackEvent (adjustEvent);
		}
		
		if (GUI.Button (new Rect (0, Screen.height * 4 / nr_buttons, Screen.width, Screen.height / nr_buttons), "Track Partner Event")) {
			AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
			
			adjustEvent.addPartnerParameter ("key", "value");
			adjustEvent.addPartnerParameter ("foo", "bar");
			
			Adjust.trackEvent (adjustEvent);
		}
	}
	
	public void attributionChangedDelegate (AdjustAttribution attribution)
	{
		Debug.Log ("Attribution changed");
		
		if (attribution.trackerName != null)
			Debug.Log ("trackerName " + attribution.trackerName);
		if (attribution.trackerToken != null)
			Debug.Log ("trackerToken " + attribution.trackerToken);
		if (attribution.network != null)
			Debug.Log ("network " + attribution.network);
		if (attribution.campaign != null)
			Debug.Log ("campaign " + attribution.campaign);
		if (attribution.adgroup != null)
			Debug.Log ("adgroup " + attribution.adgroup);
		if (attribution.creative != null)
			Debug.Log ("creative " + attribution.creative);
	}
}
