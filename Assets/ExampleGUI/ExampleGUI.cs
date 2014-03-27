using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour {

	private int nr_buttons = 4;

	void OnGUI () {
		if (GUI.Button (new Rect (0, Screen.height * 0 / nr_buttons, Screen.width, Screen.height / nr_buttons),
		                "manual launch")) {
			Adjust.appDidLaunch("querty123456", Util.Environment.Sandbox, Util.LogLevel.Verbose, false);
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
	}

	public void responseDelegate (ResponseData responseData)
	{
		Debug.Log ("activitykind " + responseData.activityKind.ToString ());
		Debug.Log ("trackerName " + responseData.trackerName);
	}

}
