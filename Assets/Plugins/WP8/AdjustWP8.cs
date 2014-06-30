using UnityEngine;
using System.Collections.Generic;
using AdjustUnityWP;
using System;

namespace com.adjust.sdk {
	public class AdjustWP8 : IAdjust {
		
		public void appDidLaunch (string appToken, AdjustUtil.AdjustEnvironment environment, string sdkPrefix, AdjustUtil.LogLevel logLevel, bool eventBuffering)
		{
			string sEnvironment = environment.ToString ().ToLower ();
			string sLogLevel = logLevel.ToString ().ToLower ();
			
			AdjustWP.AppDidLaunch (appToken, sEnvironment, sdkPrefix, sLogLevel, eventBuffering);
		}
		public void trackEvent (string eventToken, Dictionary<string, string> parameters = null)
		{
			AdjustWP.TrackEvent (eventToken, parameters);
		}
		public void trackRevenue (double cents, string eventToken = null, Dictionary<string, string> parameters = null)
		{
			AdjustWP.TrackRevenue (cents, eventToken, parameters);
		}
		public void onPause ()
		{
			AdjustWP.AppDidDeactivate ();
		}
		public void onResume ()
		{
			AdjustWP.AppDidActivate ();
		}
		public void setResponseDelegate (string sceneName) { }
		
		public void setResponseDelegateString(Action<string> responseDelegate)
		{
			AdjustWP.SetResponseDelegateString (responseDelegate);
		}
		public void setEnabled (bool enabled)
		{
			AdjustWP.SetEnabled (enabled);
		}
		public bool isEnabled ()
		{
			return AdjustWP.IsEnabled ();
		}
	}
}
