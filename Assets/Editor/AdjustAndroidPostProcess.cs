//#define ADJUST_LOG_CONSOLE

using System;
using System.IO;
using System.Text;
using System.Xml;

/// <summary>
/// Kept this in it's own file as it was converted from the PostprocessBuildPlayer_AdjustPostBuildAndroid.py script.
/// </summary>
public class AdjustAndroidPostProcess
{
#if !ADJUST_LOG_CONSOLE
	/// <summary>
	/// Path the log file is writted out to.
	/// </summary>
	private static string logFilePath = "AdjustPostBuildAndroidLog.txt";

	private StreamWriter logFile;
#endif

	/// <summary>
	/// Temporary structure allocated as result of CheckManifest
	/// </summary>
	struct CheckManifestResults
	{
		public XmlDocument manifestXml;
		public bool hasAdjustReceiver;
		public bool hasInternetPermission;
		public bool hasWifiPermission;
	}

	/// <summary>
	/// Entry point for Android Post Process logic.
	/// </summary>
	/// <param name="assetsPath">Path to the assets folder of unity3d</param>
	/// <param name="isPrebuild">Used to check and change the AndroidManifest.xml to conform to the Adjust SDK</param>
	/// <returns></returns>
	public int Start (string assetsPath, bool isPrebuild)
	{
		int exitCode = 0;

#if !ADJUST_LOG_CONSOLE
		using (logFile = new StreamWriter (logFilePath, false))
		{
#endif
		// get the path of the android and adjust plugin folders
		string androidPluginPath = Path.Combine (assetsPath, "Plugins/Android/");
		string adjustAndroidPath = Path.Combine (assetsPath, "Adjust/Android/");
		Log ("Android plugin path: {0}", androidPluginPath);
		Log ("Android adjust path: {0}", adjustAndroidPath);

		// try to open an existing manifest file
		try
		{
			string manifestPath = Path.Combine (androidPluginPath, "AndroidManifest.xml");

			XmlDocument editedXml = null;
			using (FileStream mf = File.OpenRead (manifestPath))
			{
				CheckManifestResults checkResult = CheckManifest(mf);
				// check if manifest has all changes needed
				bool allCheck = checkResult.hasAdjustReceiver && checkResult.hasInternetPermission && checkResult.hasWifiPermission;
				// edit manifest if has any change missing
				if (!allCheck)
				{
					// warn unity if it was pos-build, if something is missing
					if (!isPrebuild)
						Log ("Android manifest used in unity did not had all the changes adjust SDK needs. Please build again the package.");
					editedXml = EditManifest (ref checkResult, androidPluginPath);
				}
			}

			// write changed xml
			if (editedXml != null)
			{
				using (FileStream mf = File.Open (manifestPath, FileMode.Create))
				{
					editedXml.Save (mf);
				}
				exitCode = 1;
			}
		}
		catch (IOException e)
		{
			// if it does not exist 
			if (e is FileNotFoundException || e is DirectoryNotFoundException)
			{
				// warn unity that needed manifest wasn't used
				if (!isPrebuild)
				{
					Log ("Used default Android manifest file from unity. Please build again the package to include the changes for adjust SDK");
				}

				CopyAdjustManifest (androidPluginPath, adjustAndroidPath);
				exitCode = 1;
			}
			else
			{
				Log ("Error - {0}", e);
				exitCode = -1;
			}
		}
		catch (Exception e)
		{
			Log ("Error - {0}", e);
			exitCode = -1;
		}
#if !ADJUST_LOG_CONSOLE
		}
#endif

		// exit with return code for unity
		return exitCode;
	}

	CheckManifestResults CheckManifest (Stream manifestFile)
	{
		CheckManifestResults results;
		results.manifestXml = new XmlDocument ();
		results.manifestXml.Load (manifestFile);
		//LogXml (results.manifestXml);

		results.hasAdjustReceiver = HasElementAttr (results.manifestXml, "receiver", "android:name", "com.adjust.sdk.AdjustReferrerReceiver");
		Log ("has adjust install referrer receiver?: {0}", results.hasAdjustReceiver);

		results.hasInternetPermission = HasElementAttr (results.manifestXml, "uses-permission", "android:name", "android.permission.INTERNET");
		Log ("has internet permission?: {0}", results.hasInternetPermission);

		results.hasWifiPermission = HasElementAttr (results.manifestXml, "uses-permission", "android:name", "android.permission.ACCESS_WIFI_STATE");
		Log ("has wifi permission?: {0}", results.hasWifiPermission);

		return results;
	}

	private XmlDocument EditManifest (ref CheckManifestResults check, string androidPluginPath)
	{
		XmlDocument manifestXml = check.manifestXml;

		// add the adjust install referrer to the application element
		if (!check.hasAdjustReceiver)
		{
			string receiverString = "<?xml version=\"1.0\"?>" +
			"<receiver xmlns:android=\"http://schemas.android.com/apk/res/android\" android:name=\"com.adjust.sdk.AdjustReferrerReceiver\" android:exported=\"true\">" +
			"    <intent-filter>" +
			"        <action android:name=\"com.android.vending.INSTALL_REFERRER\"/>" +
			"    </intent-filter>" +
			"</receiver>";
			XmlDocument receiverXml = new XmlDocument ();
			receiverXml.LoadXml (receiverString);
			receiverXml.DocumentElement.RemoveAttribute ("xmlns:android");

			foreach (XmlNode appElement in manifestXml.GetElementsByTagName("application"))
			{
				// Awkward reparting required otherwise it complains about the node belonging to another document
				XmlNode importedNode = manifestXml.ImportNode (receiverXml.DocumentElement, true);
				appElement.AppendChild (importedNode);
			}

			Log ("added adjust install referrer receiver");
		}

		// add the internet permission to the manifest element
		if (!check.hasInternetPermission)
		{
			XmlElement ipElement = manifestXml.CreateElement ("uses-permission");
			XmlAttribute attr = manifestXml.CreateAttribute ("android", "name", "http://schemas.android.com/apk/res/android");
			attr.InnerText = "android.permission.INTERNET";
			ipElement.SetAttributeNode (attr);
			manifestXml.DocumentElement.AppendChild (ipElement);
			Log ("added internet permission");
		}

		//if google play services are not included
		// add the access wifi state permission to the manifest element
		//if google play services are included
		// don't add
		string googlePlayServicesPath = Path.Combine (androidPluginPath, "google-play-services_lib");

		if (!Directory.Exists (googlePlayServicesPath))
		{
			if (!check.hasWifiPermission)
			{
				XmlElement ipElement = manifestXml.CreateElement ("uses-permission");
				XmlAttribute attr = manifestXml.CreateAttribute ("android", "name", "http://schemas.android.com/apk/res/android");
				attr.InnerText = "android.permission.ACCESS_WIFI_STATE";
				ipElement.SetAttributeNode (attr);
				manifestXml.DocumentElement.AppendChild (ipElement);
				Log ("added access wifi permission");
			}
		}

		//LogXml(manifestXml);
		return manifestXml;
	}

	bool HasElementAttr (XmlDocument xmlDom, string tagName, string attrName, string attrValue)
	{
		foreach (XmlNode node in xmlDom.GetElementsByTagName (tagName))
		{
			XmlAttribute attrDom = node.Attributes[attrName];
			if (attrDom != null && attrDom.Value.Equals (attrValue))
				return true;
		}

		return false;
	}

	private void CopyAdjustManifest (string androidPluginPath, string adjustAndroidPath)
	{
		string adjustManifestPath = Path.Combine (adjustAndroidPath, "AdjustAndroidManifest.xml");
		string newManifestPath = Path.Combine (androidPluginPath, "AndroidManifest.xml");

		// Make sure the destination directory exists
		if (!Directory.Exists (androidPluginPath))
			Directory.CreateDirectory (androidPluginPath);

		try
		{
			File.Copy (adjustManifestPath, newManifestPath);
			Log ("Manifest copied from {0} to {1}", adjustManifestPath, newManifestPath);
		}
		catch (Exception e)
		{
			Log ("Error - {0}", e);
		}
	}

	/// <summary>
	/// Method for writing out an XmlDocument to the log in a readable format.
	/// </summary>
	/// <param name="doc"></param>
	private void LogXml (XmlDocument doc)
	{
		using (StringWriter stringWriter = new StringWriter ())
		{
			// Write out pretty xml
			XmlWriterSettings settings = new XmlWriterSettings ();
			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.Encoding = Encoding.UTF8;
			using (XmlWriter xmlTextWriter = XmlWriter.Create (stringWriter, settings))
			{
				doc.WriteTo (xmlTextWriter);
				xmlTextWriter.Flush ();
				Log (stringWriter.ToString ());
			}
		}
	}

	/// <summary>
	/// Internal logging method, writes to file or console depending on whether LOG_FILE is defined.
	/// </summary>
	/// <param name="format">Either basic string or format string as handled by string.Format method.</param>
	/// <param name="parameters">Optional parameters for format string.</param>
	private void Log (string format, params object[] parameters)
	{
		string logEntry = string.Format (format, parameters);
#if !ADJUST_LOG_CONSOLE
		logFile.WriteLine (logEntry);
#else
		UnityEngine.Debug.Log (logEntry);
#endif
	}
}
