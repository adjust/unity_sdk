using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor;
using System.Xml;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace AdjustSdk
{
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
    #if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            OnPreprocessBuild(report.summary.platform, string.Empty);
        }
    #endif

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.Android)
            {
    #if UNITY_ANDROID
                RunPostProcessTasksAndroid();
    #endif
            }
        }

    #if UNITY_ANDROID
        private static void RunPostProcessTasksAndroid()
        {
            var isAdjustManifestUsed = false;
            var androidPluginsPath = Path.Combine(Application.dataPath, "Plugins/Android");
            var adjustManifestPath = Path.Combine(Application.dataPath, "Adjust/Native/Android/AdjustAndroidManifest.xml");
            var appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

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

                Debug.Log("[Adjust]: User defined AndroidManifest.xml file not found in Plugins/Android folder.");
                Debug.Log("[Adjust]: Creating default app's AndroidManifest.xml from AdjustAndroidManifest.xml file.");
            }
            else
            {
                Debug.Log("[Adjust]: User defined AndroidManifest.xml file located in Plugins/Android folder.");
            }

            // Let's open the app's AndroidManifest.xml file.
            var manifestFile = new XmlDocument();
            manifestFile.Load(appManifestPath);

            var manifestHasChanged = false;

            // If Adjust manifest is used, we have already set up everything in it so that 
            // our native Android SDK can be used properly.
            if (!isAdjustManifestUsed)
            {
                // However, if you already had your own AndroidManifest.xml, we'll now run
                // some checks on it and tweak it a bit if needed to add some stuff which
                // our native Android SDK needs so that it can run properly.

                // Add needed permissions if they are missing.
                manifestHasChanged |= AddPermissions(manifestFile);

                // Add intent filter to main activity if it is missing.
                manifestHasChanged |= AddBroadcastReceiver(manifestFile);
            }

            // Add intent filter to URL schemes for deeplinking
            manifestHasChanged |= AddURISchemes(manifestFile);

            if (manifestHasChanged)
            {
                // Save the changes.
                manifestFile.Save(appManifestPath);

                Debug.Log("[Adjust]: App's AndroidManifest.xml file check and potential modification completed.");
                Debug.Log("[Adjust]: Please check if any error message was displayed during this process "
                                        + "and make sure to fix all issues in order to properly use the Adjust SDK in your app.");
            }
            else
            {
                Debug.Log("[Adjust]: App's AndroidManifest.xml file check completed.");
                Debug.Log("[Adjust]: No modifications performed due to app's AndroidManifest.xml file compatibility.");
            }
        }

        private static bool AddURISchemes(XmlDocument manifest)
        {
            if (AdjustSettings.AndroidUriSchemes.Length == 0)
            {
                return false;
            }
            Debug.Log("[Adjust]: Start addition of URI schemes");

            var intentRoot = manifest.DocumentElement.SelectSingleNode("/manifest/application/activity[@android:name='com.unity3d.player.UnityPlayerActivity']", GetNamespaceManager(manifest));
            var usedIntentFiltersChanged = false;
            var usedIntentFilters = GetIntentFilter(manifest);
            foreach (var uriScheme in AdjustSettings.AndroidUriSchemes)
            {
                Uri uri;
                try
                {
                    // The first element is android:scheme and the second one is android:host.
                    uri = new Uri(uriScheme);

                    // Uri class converts implicit file paths to explicit file paths with the file:// scheme.
                    if (!uriScheme.StartsWith(uri.Scheme))
                    {
                        throw new UriFormatException();
                    }
                }
                catch (UriFormatException)
                {
                    Debug.LogError(string.Format("[Adjust]: Android deeplink URI scheme \"{0}\" is invalid and will be ignored.", uriScheme));
                    Debug.LogWarning(string.Format("[Adjust]: Make sure that your URI scheme entry ends with ://"));
                    continue;
                }

                if (!IsIntentFilterAlreadyExist(manifest, uri))
                {
                    Debug.Log("[Adjust]: Adding new URI with scheme: " + uri.Scheme + ", and host: " + uri.Host);
                    var androidSchemeNode = manifest.CreateElement("data");
                    AddAndroidNamespaceAttribute(manifest, "scheme", uri.Scheme, androidSchemeNode);
                    AddAndroidNamespaceAttribute(manifest, "host", uri.Host, androidSchemeNode);
                    usedIntentFilters.AppendChild(androidSchemeNode);
                    usedIntentFiltersChanged = true;

                    Debug.Log(string.Format("[Adjust]: Android deeplink URI scheme \"{0}\" successfully added to your app's AndroidManifest.xml file.", uriScheme));
                }
            }

            if (usedIntentFiltersChanged && usedIntentFilters.ParentNode == null)
            {
                intentRoot.AppendChild(usedIntentFilters);
            }

            return usedIntentFiltersChanged;
        }

        private static XmlElement GetIntentFilter(XmlDocument manifest)
        {
            var xpath = "/manifest/application/activity/intent-filter[data/@android:scheme and data/@android:host]";
            var intentFilter = manifest.DocumentElement.SelectSingleNode(xpath, GetNamespaceManager(manifest)) as XmlElement;
            if (intentFilter == null)
            {
                const string androidName = "name";
                const string category = "category";

                intentFilter = manifest.CreateElement("intent-filter");

                var actionElement = manifest.CreateElement("action");
                AddAndroidNamespaceAttribute(manifest, androidName, "android.intent.action.VIEW", actionElement);
                intentFilter.AppendChild(actionElement);

                var defaultCategory = manifest.CreateElement(category);
                AddAndroidNamespaceAttribute(manifest, androidName, "android.intent.category.DEFAULT", defaultCategory);
                intentFilter.AppendChild(defaultCategory);

                var browsableCategory = manifest.CreateElement(category);
                AddAndroidNamespaceAttribute(manifest, androidName, "android.intent.category.BROWSABLE", browsableCategory);
                intentFilter.AppendChild(browsableCategory);
            }
            return intentFilter;
        }

        private static bool IsIntentFilterAlreadyExist(XmlDocument manifest, Uri link)
        {
            var xpath = string.Format("/manifest/application/activity/intent-filter/data[@android:scheme='{0}' and @android:host='{1}']", link.Scheme, link.Host);
            return manifest.DocumentElement.SelectSingleNode(xpath, GetNamespaceManager(manifest)) != null;
        }

        private static bool AddPermissions(XmlDocument manifest)
        {
            // The Adjust SDK needs two permissions to be added to you app's manifest file:
            // <uses-permission android:name="android.permission.INTERNET" />
            // <uses-permission android:name="com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE" />
            // <uses-permission android:name="com.google.android.gms.permission.AD_ID"/>
            // <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />

            Debug.Log("[Adjust]: Checking if all permissions needed for the Adjust SDK are present in the app's AndroidManifest.xml file.");

            var manifestHasChanged = false;

            // If enabled by the user && android.permission.INTERNET permission is missing, add it.
            if (AdjustSettings.androidPermissionInternet == true)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.INTERNET");
            }
            // If enabled by the user && com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE permission is missing, add it.
            if (AdjustSettings.androidPermissionInstallReferrerService == true)
            {
                manifestHasChanged |= AddPermission(manifest, "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE");
            }
            // If enabled by the user && com.google.android.gms.permission.AD_ID permission is missing, add it.
            if (AdjustSettings.androidPermissionAdId == true)
            {
                manifestHasChanged |= AddPermission(manifest, "com.google.android.gms.permission.AD_ID");
            }
            // If enabled by the user && android.permission.ACCESS_NETWORK_STATE permission is missing, add it.
            if (AdjustSettings.androidPermissionAccessNetworkState == true)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_NETWORK_STATE");
            }

            return manifestHasChanged;
        }

        private static bool AddPermission(XmlDocument manifest, string permissionValue)
        {
            if (DoesPermissionExist(manifest, permissionValue))
            {
                Debug.Log(string.Format("[Adjust]: Your app's AndroidManifest.xml file already contains {0} permission.", permissionValue));
                return false;
            }

            var element = manifest.CreateElement("uses-permission");
            AddAndroidNamespaceAttribute(manifest, "name", permissionValue, element);
            manifest.DocumentElement.AppendChild(element);
            Debug.Log(string.Format("[Adjust]: {0} permission successfully added to your app's AndroidManifest.xml file.", permissionValue));

            return true;
        }

        private static bool DoesPermissionExist(XmlDocument manifest, string permissionValue)
        {
            var xpath = string.Format("/manifest/uses-permission[@android:name='{0}']", permissionValue);
            return manifest.DocumentElement.SelectSingleNode(xpath, GetNamespaceManager(manifest)) != null;
        }

        private static bool AddBroadcastReceiver(XmlDocument manifest)
        {
            // We're looking for existence of broadcast receiver in the AndroidManifest.xml
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

            Debug.Log("[Adjust]: Checking if app's AndroidManifest.xml file contains receiver for INSTALL_REFERRER intent.");

            // Find the application node
            var applicationNodeXpath = "/manifest/application";
            var applicationNode = manifest.DocumentElement.SelectSingleNode(applicationNodeXpath);

            // If there's no application node, something is really wrong with your AndroidManifest.xml.
            if (applicationNode == null)
            {
                Debug.LogError("[Adjust]: Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
                Debug.LogError("[Adjust]: Unable to add the Adjust broadcast receiver to AndroidManifest.xml.");
                return false;
            }

            // Okay, there's an application node in the AndroidManifest.xml file.
            // Let's now check if user has already defined a receiver which is listening to INSTALL_REFERRER intent.
            // If that is already defined, don't force the Adjust broadcast receiver to the manifest file.
            // If not, add the Adjust broadcast receiver to the manifest file.

            var customBroadcastReceiversNodes = GetCustomRecieverNodes(manifest);
            if (customBroadcastReceiversNodes.Count > 0)
            {
                if (DoesAdjustBroadcastReceiverExist(manifest))
                {
                    Debug.Log("[Adjust]: It seems like you are already using Adjust broadcast receiver. Yay.");
                }
                else
                {
                    Debug.Log("[Adjust]: It seems like you are using your own broadcast receiver.");
                    Debug.Log("[Adjust]: Please, add the calls to the Adjust broadcast receiver like described in here: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md");
                }

                return false;
            }

            // Generate Adjust broadcast receiver entry and add it to the application node.
            var receiverElement = manifest.CreateElement("receiver");
            AddAndroidNamespaceAttribute(manifest, "name", "com.adjust.sdk.AdjustReferrerReceiver", receiverElement);
            AddAndroidNamespaceAttribute(manifest, "permission", "android.permission.INSTALL_PACKAGES", receiverElement);
            AddAndroidNamespaceAttribute(manifest, "exported", "true", receiverElement);

            var intentFilterElement = manifest.CreateElement("intent-filter");
            var actionElement = manifest.CreateElement("action");
            AddAndroidNamespaceAttribute(manifest, "name", "com.android.vending.INSTALL_REFERRER", actionElement);

            intentFilterElement.AppendChild(actionElement);
            receiverElement.AppendChild(intentFilterElement);
            applicationNode.AppendChild(receiverElement);

            Debug.Log("[Adjust]: Adjust broadcast receiver successfully added to your app's AndroidManifest.xml file.");

            return true;
        }

        private static bool DoesAdjustBroadcastReceiverExist(XmlDocument manifest)
        {
            var xpath = "/manifest/application/receiver[@android:name='com.adjust.sdk.AdjustReferrerReceiver']";
            return manifest.SelectSingleNode(xpath, GetNamespaceManager(manifest)) != null;
        }

        private static List<XmlNode> GetCustomRecieverNodes(XmlDocument manifest)
        {
            var xpath = "/manifest/application/receiver[intent-filter/action[@android:name='com.android.vending.INSTALL_REFERRER']]";
            return new List<XmlNode>(manifest.DocumentElement.SelectNodes(xpath, GetNamespaceManager(manifest)).OfType<XmlNode>());
        }

        private static void AddAndroidNamespaceAttribute(XmlDocument manifest, string key, string value, XmlElement node)
        {
            var androidSchemeAttribute = manifest.CreateAttribute("android", key, "http://schemas.android.com/apk/res/android");
            androidSchemeAttribute.InnerText = value;
            node.SetAttributeNode(androidSchemeAttribute);
        }

        private static XmlNamespaceManager GetNamespaceManager(XmlDocument manifest)
        {
            var namespaceManager = new XmlNamespaceManager(manifest.NameTable);
            namespaceManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");
            return namespaceManager;
        }
    #endif
}
}
