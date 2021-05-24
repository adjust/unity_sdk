using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor;
using System.Xml;
using System;
using System.Text.RegularExpressions;

#if UNITY_2018_1_OR_NEWER
public class AdjustEditorPreprocessor : IPreprocessBuildWithReport
#else
public class AdjustEditorPreprocessor : IPreprocessBuild
#endif
{
    public int callbackOrder
    {
        get
        {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        RunPostProcessTasksAndroid();
    }

    private static void RunPostProcessTasksAndroid()
    {
        bool isAdjustManifestUsed = false;
        string androidPluginsPath = Path.Combine(Application.dataPath, "Plugins/Android");
        string adjustManifestPath = Path.Combine(Application.dataPath, "Adjust/Android/AdjustAndroidManifest.xml");
        string appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

        // Check if user has already created AndroidManifest.xml file in its location.
        // If not, use already predefined AdjustAndroidManifest.xml as default one.
        if (!File.Exists(appManifestPath))
        {
            if (!Directory.Exists(androidPluginsPath))
            {
                Directory.CreateDirectory(androidPluginsPath);
            }

            isAdjustManifestUsed = true;
            File.Copy(adjustManifestPath, appManifestPath);

            UnityEngine.Debug.Log("[Adjust]: User defined AndroidManifest.xml file not found in Plugins/Android folder.");
            UnityEngine.Debug.Log("[Adjust]: Creating default app's AndroidManifest.xml from AdjustAndroidManifest.xml file.");
        }
        else
        {
            UnityEngine.Debug.Log("[Adjust]: User defined AndroidManifest.xml file located in Plugins/Android folder.");
        }

        // If Adjust manifest is used, we have already set up everything in it so that 
        // our native Android SDK can be used properly.
        if (!isAdjustManifestUsed)
        {
            // However, if you already had your own AndroidManifest.xml, we'll now run
            // some checks on it and tweak it a bit if needed to add some stuff which
            // our native Android SDK needs so that it can run properly.

            // Let's open the app's AndroidManifest.xml file.
            XmlDocument manifestFile = new XmlDocument();
            manifestFile.Load(appManifestPath);

            bool manifestHasChanged = false;

            // Add needed permissions if they are missing.
            manifestHasChanged |= AddPermissions(manifestFile);

            // Add intent filter to main activity if it is missing.
            manifestHasChanged |= AddBroadcastReceiver(manifestFile);

            // Add intent filter to URL schemes for deeplinking
            manifestHasChanged |= AddURLSchemes(manifestFile);

            if (manifestHasChanged)
            {
                // Save the changes.
                manifestFile.Save(appManifestPath);

                // Clean the manifest file.
                CleanManifestFile(appManifestPath);

                UnityEngine.Debug.Log("[Adjust]: App's AndroidManifest.xml file check and potential modification completed.");
                UnityEngine.Debug.Log("[Adjust]: Please check if any error message was displayed during this process "
                                      + "and make sure to fix all issues in order to properly use the Adjust SDK in your app.");
            }
            else
            {
                UnityEngine.Debug.Log("[Adjust]: App's AndroidManifest.xml file check completed.");
                UnityEngine.Debug.Log("[Adjust]: No modifications performed due to app's AndroidManifest.xml file compatibility.");
            }
        }
        else
        {
            // Let's open the app's AndroidManifest.xml file.
            XmlDocument manifestFile = new XmlDocument();
            manifestFile.Load(appManifestPath);

            bool manifestHasChanged = false;

            // Add intent filter to URL schemes for deeplinking
            manifestHasChanged |= AddURLSchemes(manifestFile);

            if (manifestHasChanged)
            {
                // Save the changes.
                manifestFile.Save(appManifestPath);

                // Clean the manifest file.
                CleanManifestFile(appManifestPath);

                UnityEngine.Debug.Log("[Adjust]: App's AndroidManifest.xml file check and potential modification completed.");
                UnityEngine.Debug.Log("[Adjust]: Please check if any error message was displayed during this process "
                                      + "and make sure to fix all issues in order to properly use the Adjust SDK in your app.");
            }
            else
            {
                UnityEngine.Debug.Log("[Adjust]: App's AndroidManifest.xml file check completed.");
                UnityEngine.Debug.Log("[Adjust]: No modifications performed due to app's AndroidManifest.xml file compatibility.");
            }
        }
    }

    private static bool AddURLSchemes(XmlDocument manifest)
    {
        Debug.Log("[Adjust]: Start addition of URI schemes");
        const string intentFilter = "intent-filter";
        XmlNode intentRoot = manifest.DocumentElement.SelectSingleNode("/manifest/application/activity");
        bool manifestHasChanged = false;
        bool usedIntentFiltersChanged = false;
        XmlElement usedIntentFilters = manifest.CreateElement(intentFilter);

        usedIntentFilters = CreateActionAndCategoryNodes(usedIntentFilters, manifest);

        foreach (var uriScheme in AdjustSettings.AndroidUriSchemes)
        {
            //The first element is android:scheme and the second one is android:host
            Uri uri = new Uri(uriScheme);

            if (!IntentFilterAlreadyExist(manifest, uri))
            {
                Debug.Log("[Adjust]: Adding new URI with scheme: " + uri.Scheme + ", and host: " + uri.Host);
                XmlElement androidSchemeNode = manifest.CreateElement("data");
                androidSchemeNode.SetAttribute("android__scheme", uri.Scheme);
                androidSchemeNode.SetAttribute("android__host", uri.Host);
                usedIntentFilters.AppendChild(androidSchemeNode);
                usedIntentFiltersChanged = true;

                Debug.Log("[Adjust]: Android deeplink URL scheme successfully added to your app's AndroidManifest.xml file.");

                manifestHasChanged = true;
            }
        }
        if (usedIntentFiltersChanged)
        {
            intentRoot.AppendChild(usedIntentFilters);
        }
        return manifestHasChanged;
    }

    private static bool IntentFilterAlreadyExist(XmlDocument manifest, Uri link)
    {
        var namespaceManager = new XmlNamespaceManager(manifest.NameTable);
        namespaceManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");
        var xpath = string.Format("/manifest/application/activity/intent-filter/data[@android:scheme='{0}' and @android:host='{1}']", link.Scheme, link.Host);
        return manifest.DocumentElement.SelectSingleNode(xpath, namespaceManager) != null;
    }

    private static XmlElement CreateActionAndCategoryNodes(XmlElement intentFilter, XmlDocument manifest)
    {
        const string andoirdName = "android__name";
        const string category = "category";

        XmlElement actionElement = manifest.CreateElement("action");
        actionElement.SetAttribute(andoirdName, "android.intent.action.VIEW");
        intentFilter.AppendChild(actionElement);

        XmlElement defaultCategory = manifest.CreateElement(category);
        defaultCategory.SetAttribute(andoirdName, "android.intent.category.DEFAULT");
        intentFilter.AppendChild(defaultCategory);

        XmlElement browsableCategory = manifest.CreateElement(category);
        browsableCategory.SetAttribute(andoirdName, "android.intent.category.BROWSABLE");
        intentFilter.AppendChild(browsableCategory);

        Debug.Log(intentFilter.ToString());

        return intentFilter;
    }

    private static bool AddPermissions(XmlDocument manifest)
    {
        // The Adjust SDK needs two permissions to be added to you app's manifest file:
        // <uses-permission android:name="android.permission.INTERNET" />
        // <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
        // <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
        // <uses-permission android:name="com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE" />

        UnityEngine.Debug.Log("[Adjust]: Checking if all permissions needed for the Adjust SDK are present in the app's AndroidManifest.xml file.");

        bool hasInternetPermission = false;
        bool hasAccessWifiStatePermission = false;
        bool hasAccessNetworkStatePermission = false;
        bool hasInstallReferrerServicePermission = false;

        XmlElement manifestRoot = manifest.DocumentElement;

        // Check if permissions are already there.
        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "uses-permission")
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Value.Contains("android.permission.INTERNET"))
                    {
                        hasInternetPermission = true;
                    }
                    else if (attribute.Value.Contains("android.permission.ACCESS_WIFI_STATE"))
                    {
                        hasAccessWifiStatePermission = true;
                    }
                    else if (attribute.Value.Contains("android.permission.ACCESS_NETWORK_STATE"))
                    {
                        hasAccessNetworkStatePermission = true;
                    }
                    else if (attribute.Value.Contains("com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE"))
                    {
                        hasInstallReferrerServicePermission = true;
                    }
                }
            }
        }

        bool manifestHasChanged = false;

        // If android.permission.INTERNET permission is missing, add it.
        if (!hasInternetPermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.INTERNET");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[Adjust]: android.permission.INTERNET permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[Adjust]: Your app's AndroidManifest.xml file already contains android.permission.INTERNET permission.");
        }

        // If android.permission.ACCESS_WIFI_STATE permission is missing, add it.
        if (!hasAccessWifiStatePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.ACCESS_WIFI_STATE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[Adjust]: android.permission.ACCESS_WIFI_STATE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[Adjust]: Your app's AndroidManifest.xml file already contains android.permission.ACCESS_WIFI_STATE permission.");
        }

        // If android.permission.ACCESS_NETWORK_STATE permission is missing, add it.
        if (!hasAccessNetworkStatePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.ACCESS_NETWORK_STATE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[Adjust]: android.permission.ACCESS_NETWORK_STATE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[Adjust]: Your app's AndroidManifest.xml file already contains android.permission.ACCESS_NETWORK_STATE permission.");
        }

        // If com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission is missing, add it.
        if (!hasInstallReferrerServicePermission)
        {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE");
            manifestRoot.AppendChild(element);
            UnityEngine.Debug.Log("[Adjust]: com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission successfully added to your app's AndroidManifest.xml file.");
            manifestHasChanged = true;
        }
        else
        {
            UnityEngine.Debug.Log("[Adjust]: Your app's AndroidManifest.xml file already contains com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission.");
        }

        return manifestHasChanged;
    }

    private static bool AddBroadcastReceiver(XmlDocument manifest)
    {
        // We're looking for existance of broadcast receiver in the AndroidManifest.xml
        // Check out the example below how that usually looks like:

        // <manifest
        //     <!-- ... -->>
        // 
        //     <supports-screens
        //         <!-- ... -->/>
        // 
        //     <application
        //         <!-- ... -->>
        //         <receiver
        //             android:name="com.adjust.sdk.AdjustReferrerReceiver"
        //             android:permission="android.permission.INSTALL_PACKAGES"
        //             android:exported="true" >
        //             
        //             <intent-filter>
        //                 <action android:name="com.android.vending.INSTALL_REFERRER" />
        //             </intent-filter>
        //         </receiver>
        //         
        //         <activity android:name="com.unity3d.player.UnityPlayerActivity"
        //             <!-- ... -->
        //         </activity>
        //     </application>
        // 
        //     <!-- ... -->>
        //
        // </manifest>

        UnityEngine.Debug.Log("[Adjust]: Checking if app's AndroidManifest.xml file contains receiver for INSTALL_REFERRER intent.");

        XmlElement manifestRoot = manifest.DocumentElement;
        XmlNode applicationNode = null;

        // Let's find the application node.
        foreach (XmlNode node in manifestRoot.ChildNodes)
        {
            if (node.Name == "application")
            {
                applicationNode = node;
                break;
            }
        }

        // If there's no applicatio node, something is really wrong with your AndroidManifest.xml.
        if (applicationNode == null)
        {
            UnityEngine.Debug.LogError("[Adjust]: Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
            UnityEngine.Debug.LogError("[Adjust]: Unable to add the Adjust broadcast receiver to AndroidManifest.xml.");
            return false;
        }

        // Okay, there's an application node in the AndroidManifest.xml file.
        // Let's now check if user has already defined a receiver which is listening to INSTALL_REFERRER intent.
        // If that is already defined, don't force the Adjust broadcast receiver to the manifest file.
        // If not, add the Adjust broadcast receiver to the manifest file.

        List<XmlNode> customBroadcastReceiversNodes = getCustomRecieverNodes(applicationNode);
        if (customBroadcastReceiversNodes.Count > 0)
        {
            bool foundAdjustBroadcastReceiver = false;
            for (int i = 0; i < customBroadcastReceiversNodes.Count; i += 1)
            {
                foreach (XmlAttribute attribute in customBroadcastReceiversNodes[i].Attributes)
                {
                    if (attribute.Value.Contains("com.adjust.sdk.AdjustReferrerReceiver"))
                    {
                        foundAdjustBroadcastReceiver = true;
                    }
                }
            }

            if (!foundAdjustBroadcastReceiver)
            {
                UnityEngine.Debug.Log("[Adjust]: It seems like you are using your own broadcast receiver.");
                UnityEngine.Debug.Log("[Adjust]: Please, add the calls to the Adjust broadcast receiver like described in here: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md");
            }
            else
            {
                UnityEngine.Debug.Log("[Adjust]: It seems like you are already using Adjust broadcast receiver. Yay.");
            }

            return false;
        }
        else
        {
            // Generate Adjust broadcast receiver entry and add it to the application node.
            XmlElement receiverElement = manifest.CreateElement("receiver");
            receiverElement.SetAttribute("android__name", "com.adjust.sdk.AdjustReferrerReceiver");
            receiverElement.SetAttribute("android__permission", "android.permission.INSTALL_PACKAGES");
            receiverElement.SetAttribute("android__exported", "true");

            XmlElement intentFilterElement = manifest.CreateElement("intent-filter");
            XmlElement actionElement = manifest.CreateElement("action");
            actionElement.SetAttribute("android__name", "com.android.vending.INSTALL_REFERRER");

            intentFilterElement.AppendChild(actionElement);
            receiverElement.AppendChild(intentFilterElement);
            applicationNode.AppendChild(receiverElement);

            UnityEngine.Debug.Log("[Adjust]: Adjust broadcast receiver successfully added to your app's AndroidManifest.xml file.");

            return true;
        }
    }

    private static void CleanManifestFile(String manifestPath)
    {
        // Due to XML writing issue with XmlElement methods which are unable
        // to write "android:[param]" string, we have wrote "android__[param]" string instead.
        // Now make the replacement: "android:[param]" -> "android__[param]"

        TextReader manifestReader = new StreamReader(manifestPath);
        string manifestContent = manifestReader.ReadToEnd();
        manifestReader.Close();

        Regex regex = new Regex("android__");
        manifestContent = regex.Replace(manifestContent, "android:");

        TextWriter manifestWriter = new StreamWriter(manifestPath);
        manifestWriter.Write(manifestContent);
        manifestWriter.Close();
    }

    private static List<XmlNode> getCustomRecieverNodes(XmlNode applicationNode)
    {
        List<XmlNode> nodes = new List<XmlNode>();
        foreach (XmlNode node in applicationNode.ChildNodes)
        {
            if (node.Name == "receiver")
            {
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Name == "intent-filter")
                    {
                        foreach (XmlNode subsubnode in subnode.ChildNodes)
                        {
                            if (subsubnode.Name == "action")
                            {
                                foreach (XmlAttribute attribute in subsubnode.Attributes)
                                {
                                    if (attribute.Value.Contains("com.android.vending.INSTALL_REFERRER"))
                                    {
                                        nodes.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return nodes;
    }
}
