using UnityEngine;
using AdjustUnityWS;
using System.Collections.Generic;
using System;

namespace com.adjust.sdk {
	
	public class AdjustMetro : IAdjust {
		
		public void appDidLaunch (string appToken, AdjustUtil.AdjustEnvironment environment, string sdkPrefix, AdjustUtil.LogLevel logLevel, bool eventBuffering)
		{
			string sEnvironment = environment.ToString ().ToLower ();
			string sLogLevel = logLevel.ToString ().ToLower ();

			AdjustWS.AppDidLaunch (appToken, sEnvironment, sdkPrefix, sLogLevel, eventBuffering);
		}
		public void trackEvent (string eventToken, Dictionary<string, string> parameters = null)
		{
			AdjustWS.TrackEvent (eventToken, parameters);
		}
		public void trackRevenue (double cents, string eventToken = null, Dictionary<string, string> parameters = null)
		{
			AdjustWS.TrackRevenue (cents, eventToken, parameters);
		}
		public void onPause ()
		{
			AdjustWS.AppDidDeactivate ();
		}
		public void onResume ()
		{
			AdjustWS.AppDidActivate ();
		}
		public void setResponseDelegate (string sceneName) { }
		
		public void setResponseDelegateString(Action<string> responseDelegate)
		{
			AdjustWS.SetResponseDelegateString (responseDelegate);
		}
		public void setEnabled (bool enabled)
		{
			AdjustWS.SetEnabled (enabled);
		}
		public bool isEnabled ()
		{
			return AdjustWS.IsEnabled ();
		}
	}
}
