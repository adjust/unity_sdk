using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleUI : MonoBehaviour {

	public string eventToken = "YourEventToken";
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
	}
	
	void OnGUI() {	
		if(GUI.Button(new Rect(Screen.width/4,Screen.height/16,Screen.width/2,Screen.height/8),"Track event")){
			AdjustIo.TrackEvent(eventToken);
		}
		
		if(GUI.Button(new Rect(Screen.width/4,Screen.height*4/16,Screen.width/2,Screen.height/8),"Track event with parameters")){
			Dictionary<string,string> parameters = new Dictionary<string, string>();
			parameters.Add("key","value");
			parameters.Add("foo","bar");
			AdjustIo.TrackEvent(eventToken,parameters);
		}
		
		if(GUI.Button(new Rect(Screen.width/4,Screen.height*7/16,Screen.width/2,Screen.height/8),"Track revenue")){
			double amountInCents = 1.0;
			AdjustIo.TrackRevenue(amountInCents);
		}
		
		if(GUI.Button(new Rect(Screen.width/4,Screen.height*10/16,Screen.width/2,Screen.height/8),"Track revenue with event token")){
			double amountInCents = 1.0;
			AdjustIo.TrackRevenue(amountInCents,eventToken);
		}
		
		if(GUI.Button(new Rect(Screen.width/4,Screen.height*13/16,Screen.width/2,Screen.height/8),"Track revenue with event token and parameters")){
			double amountInCents = 1.0;
			Dictionary<string,string> parameters = new Dictionary<string, string>();
			parameters.Add("key","value");
			parameters.Add("foo","bar");
			AdjustIo.TrackRevenue(amountInCents,eventToken,parameters);
		}
	}
}
