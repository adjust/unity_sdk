using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class AdjustEditor {
    private static bool isPostProcessingEnabled = true;

    [MenuItem ("Assets/Adjust/Check post processing status")]
    public static void CheckPostProcessingPermission() {
        EditorUtility.DisplayDialog ("adjust", "The post processing for adjust is " + (isPostProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [MenuItem ("Assets/Adjust/Change post processing status")]
    public static void ChangePostProcessingPermission() {
        isPostProcessingEnabled = !isPostProcessingEnabled;
        EditorUtility.DisplayDialog ("adjust", "The post processing for adjust is now " + (isPostProcessingEnabled ? "enabled." : "disabled."), "OK");
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string projectPath) {
        // Check what is user setting about allowing adjust SDK to perform post build tasks.
        // If user disabled it, oh well, we won't do a thing.
        if (!isPostProcessingEnabled) {
            UnityEngine.Debug.Log("adjust: You have forbidden the adjust SDK to perform post processing tasks.");
            UnityEngine.Debug.Log("adjust: Skipping post processing tasks.");

            return;
        }

        RunPostBuildScript(target:target, preBuild:false, projectPath:projectPath);
    }

    private static void RunPostBuildScript(BuildTarget target, bool preBuild, string projectPath = "") {
        if (target == BuildTarget.Android) {
            UnityEngine.Debug.Log("adjust: Starting to perform post build tasks for Android platform.");
            RunPostProcessTasksAndroid();
        } else if (target == BuildTarget.iOS) {
            #if UNITY_IOS
            UnityEngine.Debug.Log("adjust: Starting to perform post build tasks for iOS platform.");
            
            string xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject xcodeProject = new PBXProject();
            xcodeProject.ReadFromFile(xcodeProjectPath);

            // The adjust SDK needs two frameworks to be added to the project:
            // - AdSupport.framework
            // - iAd.framework

            string xcodeTarget = xcodeProject.TargetGuidByName("Unity-iPhone");
            
            UnityEngine.Debug.Log("adjust: Adding AdSupport.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "AdSupport.framework", true);
            UnityEngine.Debug.Log("adjust: AdSupport.framework added successfully.");

            UnityEngine.Debug.Log("adjust: Adding iAd.framework to Xcode project.");
            xcodeProject.AddFrameworkToProject(xcodeTarget, "iAd.framework", true);
            UnityEngine.Debug.Log("adjust: iAd.framework added successfully.");

            // The adjust SDK needs to have Obj-C exceptions enabled.
            // GCC_ENABLE_OBJC_EXCEPTIONS=YES

            UnityEngine.Debug.Log("adjust: Enabling Obj-C exceptions by setting GCC_ENABLE_OBJC_EXCEPTIONS value to YES.");
            xcodeProject.AddBuildProperty(xcodeTarget, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

            UnityEngine.Debug.Log("adjust: Obj-C exceptions enabled successfully.");

            // The adjust SDK needs to have -ObjC flag set in other linker flags section because of it's categories.
            // OTHER_LDFLAGS -ObjC
            
            UnityEngine.Debug.Log("adjust: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS).");
            xcodeProject.AddBuildProperty(xcodeTarget, "OTHER_LDFLAGS", "-ObjC");

            UnityEngine.Debug.Log("adjust: -ObjC successfully added to other linker flags.");

            // Save the changes to Xcode project file.
            xcodeProject.WriteToFile(xcodeProjectPath);
            #endif
        }
    }

    private static void RunPostProcessTasksiOS(string projectPath) {
        
    }

    private static void RunPostProcessTasksAndroid() {
        bool isAdjustManifestUsed = false;
        string androidPluginsPath = Path.Combine(Application.dataPath, "Plugins/Android");
        string adjustManifestPath = Path.Combine(Application.dataPath, "Adjust/Android/AdjustAndroidManifest.xml");
        string appManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");

        // Check if user has already created AndroidManifest.xml file in its location.
        // If not, use already predefined AdjustAndroidManifest.xml as default one.
        if (!File.Exists(appManifestPath)) {
            if (!Directory.Exists(androidPluginsPath)) {
                Directory.CreateDirectory(androidPluginsPath);
            }
            
            isAdjustManifestUsed = true;
            File.Copy(adjustManifestPath, appManifestPath);

            UnityEngine.Debug.Log("adjust: User defined AndroidManifest.xml file not found in Plugins/Android folder.");
            UnityEngine.Debug.Log("adjust: Creating default app's AndroidManifest.xml from AdjustAndroidManifest.xml file.");
        } else {
            UnityEngine.Debug.Log("adjust: User defined AndroidManifest.xml file located in Plugins/Android folder.");
        }

        // If adjust manifest is used, we have already set up everything in it so that 
        // our native Android SDK can be used properly.
        if (!isAdjustManifestUsed) {
            // However, if you already had your own AndroidManifest.xml, we'll now run
            // some checks on it and tweak it a bit if needed to add some stuff which
            // our native Android SDK needs so that it can run properly.

            // Let's open the app's AndroidManifest.xml file.
            XmlDocument manifestFile = new XmlDocument();
            manifestFile.Load(appManifestPath);
            
            // Add needed permissions if they are missing.
            AddPermissions(manifestFile);

            // Add intent filter to main activity if it is missing.
            AddBroadcastReceiver(manifestFile);

            // Save the changes.
            manifestFile.Save(appManifestPath);

            // Clean the manifest file.
            CleanManifestFile(appManifestPath);

            UnityEngine.Debug.Log("adjust: App's AndroidManifest.xml file check and potential modification completed.");
            UnityEngine.Debug.Log("adjust: Please check if any error message was displayed during this process " 
                + "and make sure to fix all issues in order to properly use the adjust SDK in your app.");
        }
    }

    private static void AddPermissions(XmlDocument manifest) {
        // The adjust SDK needs two permissions to be added to you app's manifest file:
        // <uses-permission android:name="android.permission.INTERNET" />
        // <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />

        UnityEngine.Debug.Log("adjust: Checking if all permissions needed for the adjust SDK are present in the app's AndroidManifest.xml file.");

        bool hasInternetPermission = false;
        bool hasAccessWifiStatePermission = false;

        XmlElement manifestRoot = manifest.DocumentElement;

        // Check if permissions are already there.
        foreach (XmlNode node in manifestRoot.ChildNodes) {
            if (node.Name == "uses-permission") {
                foreach (XmlAttribute attribute in node.Attributes) {
                    if (attribute.Value.Contains("android.permission.INTERNET")) {
                        hasInternetPermission = true;
                    } else if (attribute.Value.Contains("android.permission.ACCESS_WIFI_STATE")) {
                        hasAccessWifiStatePermission = true;
                    }
                }
            }
        }

        // If android.permission.INTERNET permission is missing, add it.
        if (!hasInternetPermission) {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.INTERNET");
            manifestRoot.AppendChild(element);

            UnityEngine.Debug.Log("adjust: android.permission.INTERNET permission successfully added to your app's AndroidManifest.xml file.");
        } else {
            UnityEngine.Debug.Log("adjust: Your app's AndroidManifest.xml file already contains android.permission.INTERNET permission.");
        }

        // If android.permission.ACCESS_WIFI_STATE permission is missing, add it.
        if (!hasAccessWifiStatePermission) {
            XmlElement element = manifest.CreateElement("uses-permission");
            element.SetAttribute("android__name", "android.permission.ACCESS_WIFI_STATE");
            manifestRoot.AppendChild(element);

            UnityEngine.Debug.Log("adjust: android.permission.ACCESS_WIFI_STATE permission successfully added to your app's AndroidManifest.xml file.");
        } else {
            UnityEngine.Debug.Log("adjust: Your app's AndroidManifest.xml file already contains android.permission.ACCESS_WIFI_STATE permission.");
        }
    }

    private static void AddBroadcastReceiver(XmlDocument manifest) {
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
        //     <uses-permission android:name="android.permission.ACCESS_WIFI_STATE"
        //     <uses-permission android:name="android.permission.INTERNET" />
        // </manifest>

        UnityEngine.Debug.Log("adjust: Checking if app's AndroidManifest.xml file contains receiver for INSTALL_REFERRER intent.");

        XmlElement manifestRoot = manifest.DocumentElement;
        XmlNode applicationNode = null;

        // Let's find the application node.
        foreach(XmlNode node in manifestRoot.ChildNodes) {
            if (node.Name == "application") {
                applicationNode = node;
                break;
            }
        }

        // If there's no applicatio node, something is really wrong with your AndroidManifest.xml.
        if (applicationNode == null) {
            UnityEngine.Debug.LogError("adjust: Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
            UnityEngine.Debug.LogError("adjust: Unable to add the adjust broadcast receiver to AndroidManifest.xml.");

            return;
        }

        // Okay, there's an application node in the AndroidManifest.xml file.
        // Let's now check if user has already defined a receiver which is listening to INSTALL_REFERRER intent.
        // If that is already defined, don't force the adjust broadcast receiver to the manifest file.
        // If not, add the adjust broadcast receiver to the manifest file.
        bool isThereAnyCustomBroadcastReiver = false;

        foreach (XmlNode node in applicationNode.ChildNodes) {
            if (node.Name == "receiver") {
                foreach (XmlNode subnode in node.ChildNodes) {
                    if (subnode.Name == "intent-filter") {
                        foreach (XmlNode subsubnode in subnode.ChildNodes) {
                            if (subsubnode.Name == "action") {
                                foreach(XmlAttribute attribute in subsubnode.Attributes) {
                                    if (attribute.Value.Contains("INSTALL_REFERRER")) {
                                        isThereAnyCustomBroadcastReiver = true;
                                        break;
                                    }
                                }
                            }

                            if (isThereAnyCustomBroadcastReiver) {
                                break;
                            }
                        }
                    }

                    if (isThereAnyCustomBroadcastReiver) {
                        break;
                    }
                }
            }

            if (isThereAnyCustomBroadcastReiver) {
                break;
            }
        }

        // Let's see what we have found so far.
        if (isThereAnyCustomBroadcastReiver) {
            UnityEngine.Debug.Log("adjust: It seems like you are using your own broadcast receiver.");
            UnityEngine.Debug.Log("adjust: Please, add the calls to the adjust broadcast receiver like described in here: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md");
        } else {
            // Generate adjust broadcast receiver entry and add it to the application node.
            XmlElement receiverElement = manifest.CreateElement("receiver");
            receiverElement.SetAttribute("android__name", "com.adjust.sdk.AdjustReferrerReceiver");
            receiverElement.SetAttribute("android__exported", "true");

            XmlElement intentFilterElement = manifest.CreateElement("intent-filter");
            XmlElement actionElement = manifest.CreateElement("action");
            actionElement.SetAttribute("android__name", "com.android.vending.INSTALL_REFERRER");

            intentFilterElement.AppendChild(actionElement);
            receiverElement.AppendChild(intentFilterElement);
            applicationNode.AppendChild(receiverElement);

            UnityEngine.Debug.Log("adjust: Adjust broadcast receiver successfully added to your app's AndroidManifest.xml file.");
        }
    }

    private static void CleanManifestFile(String manifestPath) {
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

    private static int GetUnityIdeVersion() {
        int unityVersion;
        Int32.TryParse(Application.unityVersion[0].ToString(), out unityVersion);

        return unityVersion;
    }
}
