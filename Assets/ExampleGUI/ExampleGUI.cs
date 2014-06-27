using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour {

	private int nr_buttons = 5;
	private static bool isEnabled = false;

	void OnGUI () {
		if (GUI.Button (new Rect (0, Screen.height * 0 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                "manual launch")) {
			Adjust.appDidLaunch("querty123456", AdjustUtil.AdjustEnvironment.Sandbox, AdjustUtil.LogLevel.Verbose, false);
			isEnabled = true;
		}

		if (GUI.Button (new Rect (0, Screen.height * 1 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                "track Event")) {
			Adjust.trackEvent("eve001");

			var parameters = new System.Collections.Generic.Dictionary<string, string> (2);
			parameters.Add("key", "value");
			parameters.Add("foo", "bar");
			Adjust.trackEvent("eve002", parameters);
		}

		if (GUI.Button (new Rect (0, Screen.height * 2 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                "track Revenue")) {
			Adjust.trackRevenue(3.44);

			Adjust.trackRevenue(3.45, "rev001");

			var parameters = new System.Collections.Generic.Dictionary<string, string> (2);
			parameters.Add("key", "value");
			parameters.Add("foo", "bar");
			Adjust.trackRevenue(0.1, "rev002", parameters);
		}

		if (GUI.Button (new Rect (0, Screen.height * 3 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                "callback")) {
			Adjust.setResponseDelegate(responseDelegate);
		}

		var switch_sdk = isEnabled ? "disable sdk" : "enable sdk";
		if (GUI.Button (new Rect (0, Screen.height * 4 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                switch_sdk)) {
			isEnabled = !Adjust.isEnabled();
			Adjust.setEnabled(isEnabled);
		}
	}

	public void responseDelegate (ResponseData responseData)
	{
		Debug.Log ("activitykind " + responseData.activityKind.ToString ());
		Debug.Log ("trackerName " + responseData.trackerName);
		Debug.Log ("error " + responseData.error);
	}

}
