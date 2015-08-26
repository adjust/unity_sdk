using System;
using System.Collections.Generic;

using UnityEngine;

namespace com.adjust.sdk
{
	public class Adjust : MonoBehaviour
	{
		private const string errorMessage = "adjust: SDK not started. Start it manually using the 'start' method.";

		private static IAdjust instance = null;
		private static Action<AdjustAttribution> attributionChangedDelegate = null;

		public bool startManually = false;
		public bool eventBuffering = false;
		public bool printAttribution = false;

		public string appToken = "{Your App Token}";

		public AdjustLogLevel logLevel = AdjustLogLevel.Info;
		public AdjustEnvironment environment = AdjustEnvironment.Sandbox;

		#region Unity lifecycle methods

		void Awake ()
		{
			DontDestroyOnLoad (transform.gameObject);

			if (!this.startManually) {
				AdjustConfig adjustConfig = new AdjustConfig (this.appToken, this.environment);
				adjustConfig.setLogLevel (this.logLevel);
				adjustConfig.setEventBufferingEnabled (eventBuffering);

				if (printAttribution) {
					adjustConfig.setAttributionChangedDelegate (responseDelegate);
				}

				Adjust.start (adjustConfig);
			}
		}

		void OnApplicationPause (bool pauseStatus) 
		{
			if (Adjust.instance == null) {
				return;
			}
			
			if (pauseStatus) {
				Adjust.instance.onPause ();
			} else {
				Adjust.instance.onResume ();
			}
		}

		#endregion

		#region Adjust methods

		public static void start (AdjustConfig adjustConfig)
		{
			if (Adjust.instance != null) {
				Debug.Log ("adjust: Error, SDK already started.");
				return;
			}

			if (adjustConfig == null) {
				Debug.Log ("adjust: Missing config to start.");
				return;
			}

			#if UNITY_EDITOR
				Adjust.instance = null;
			#elif UNITY_IOS
				Adjust.instance = new AdjustiOS ();
			#elif UNITY_ANDROID
				Adjust.instance = new AdjustAndroid ();
			#else
				Adjust.instance = null;
			#endif

			if (Adjust.instance == null) {
				Debug.Log ("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
				return;
			}

			Adjust.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate ();

			Adjust.instance.start (adjustConfig);
		}

		public static void trackEvent (AdjustEvent adjustEvent)
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}
			
			if (adjustEvent == null) {
				Debug.Log ("adjust: Missing event to track.");
				return;
			}
			
			Adjust.instance.trackEvent (adjustEvent);
		}

		public static void setEnabled (bool enabled) 
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}

			Adjust.instance.setEnabled (enabled);
		}
		
		public static bool isEnabled () 
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return false;
			}

			return Adjust.instance.isEnabled ();
		}
		
		public static void setOfflineMode (bool enabled) 
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}

			Adjust.instance.setOfflineMode (enabled);
		}

		public static void setDeviceToken (string deviceToken)
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}

			Adjust.instance.setDeviceToken (deviceToken);
		}

		public static void setReferrer (string referrer)
		{
			if (Adjust.instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}

			Adjust.instance.setReferrer (referrer);
		}

		#endregion

		#region Attribution callback

		public void getNativeMessage (string sAttributionData)
		{
			Adjust.runAttributionChangedDelegate (sAttributionData);
		}
		
		public static void runAttributionChangedDelegate (string stringAttributionData)
		{
			if (instance == null) {
				Debug.Log (Adjust.errorMessage);
				return;
			}

			if (Adjust.attributionChangedDelegate == null) {
				Debug.Log ("adjust: Attribution changed delegate was not set.");
				return;
			}
			
			var attribution = new AdjustAttribution (stringAttributionData);
			Adjust.attributionChangedDelegate (attribution);
		}

		#endregion

		#region Private & helper methods

		// Our delegate for detecting attribution changes if choosen not to start manually.
		private void responseDelegate (AdjustAttribution responseData)
		{
			Debug.Log ("Attribution changed!");

			if (responseData.trackerName != null) {
				Debug.Log ("trackerName " + responseData.trackerName);
			}

			if (responseData.trackerToken != null) {
				Debug.Log ("trackerToken " + responseData.trackerToken);
			}

			if (responseData.network != null) {
				Debug.Log ("network " + responseData.network);
			}

			if (responseData.campaign != null) {
				Debug.Log ("campaign " + responseData.campaign);
			}

			if (responseData.adgroup != null) {
				Debug.Log ("adgroup " + responseData.adgroup);
			}

			if (responseData.creative != null) {
				Debug.Log ("creative " + responseData.creative);
			}

			if (responseData.clickLabel != null) {
				Debug.Log ("clickLabel" + responseData.clickLabel);
			}
		}

		#endregion
	}
}
