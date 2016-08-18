#!/usr/bin/env python

import argparse
import errno
import shutil
import os.path
import sys
from xml.dom.minidom import parse, parseString

def main():
    parser = argparse.ArgumentParser(description="Adjust post build android script")
    parser.add_argument('assets_path', help="path to the assets folder of unity3d")
    
    # Pre build must be set manually.
    parser.add_argument('--pre-build', action="store_true", help="used to check and change the AndroidManifest.xml to conform to the Adjust SDK")
    exit_code = 0
    
    with open('AdjustPostBuildAndroidLog.txt', 'w') as fileLog:
        # Log function with file injected.
        LogFunc = LogInput(fileLog)
        
        # Get the path of the android plugin folder.
        android_plugin_path, adjust_android_path, pre_build = parse_input(LogFunc, parser)

        # Try to open an existing manifest file.
        try:
            manifest_path = os.path.join(android_plugin_path + "AndroidManifest.xml")
            edited_xml = None
            
            with open(manifest_path, 'r+') as mf:
                check_dic = check_manifest(LogFunc, mf)
                
                # Check if manifest has all changes needed.
                all_check = check_dic["has_adjust_receiver"] and \
                            check_dic["has_internet_permission"] and \
                            check_dic["has_wifi_permission"]
                
                # Edit manifest if has any change missing.
                if not all_check:
                    # Warn unity if it was pos-build, if something is missing.
                    if not pre_build:
                        LogFunc("Android manifest used in unity did not " + \
                                "had all the changes adjust SDK needs. " + \
                                "Please build again the package.")
                    edited_xml = edit_manifest(LogFunc, mf, check_dic, android_plugin_path)
            
            # Write changed xml
            if edited_xml:
                with open(manifest_path, 'w+') as mf:
                    edited_xml.writexml(mf)
                exit_code = 1
        except IOError as ioe:
            # If it does not exist,
            if ioe.errno == errno.ENOENT:
                # warn Unity that needed manifest wasn't used.
                if not pre_build:
                    LogFunc("Used default Android manifest file from " + \
                            "unity. Please build again the package to " +
                            "include the changes for adjust SDK")
                copy_adjust_manifest(LogFunc, android_plugin_path, adjust_android_path)
                exit_code = 1
            else:
                LogFunc(ioe)
        except Exception as e:
            LogFunc(e)

    # Exit with return code for Unity.
    sys.exit(exit_code)

def edit_manifest(Log, manifest_file, check_dic, android_plugin_path):
    manifest_xml = check_dic["manifest_xml"]

    # Add the adjust install referrer to the application element.
    if not check_dic["has_adjust_receiver"]:
        receiver_string = """<?xml version="1.0" ?>
        <receiver
            xmlns:android="http://schemas.android.com/apk/res/android"
            android:name="com.adjust.sdk.AdjustReferrerReceiver"
            android:exported="true" >
            <intent-filter>
                <action android:name="com.android.vending.INSTALL_REFERRER" />
            </intent-filter>
        </receiver>
        """
        receiver_xml = parseString(receiver_string)
        receiver_xml.documentElement.removeAttribute("xmlns:android")

        Log("Adding the adjust broadcast receiver.")

        for app_element in manifest_xml.getElementsByTagName("application"):
            app_element.appendChild(receiver_xml.documentElement)
        
        Log("The adjust broadcast receiver successfully added.")

    # Add the internet permission to the manifest element
    if not check_dic["has_internet_permission"]:
        Log("Adding INTERNET permission to app's AndroidManifest.xml file.")
        
        ip_element = manifest_xml.createElement("uses-permission")
        ip_element.setAttribute("android:name", "android.permission.INTERNET")
        manifest_xml.documentElement.appendChild(ip_element)
        
        Log("Successfully added INTERNET permission to app's AndroidManifest.xml file.")

    # If google play services are not included add the access wifi state permission to the manifest element.
    # If google play services are included - don't add.
    google_play_services_path = os.path.join(android_plugin_path + "google-play-services_lib")

    if not os.path.isdir(google_play_services_path):
        if not check_dic["has_wifi_permission"]:
            Log("Adding ACCESS_WIFI_STATE permission to app's AndroidManifest.xml file.")

            ip_element = manifest_xml.createElement("uses-permission")
            ip_element.setAttribute("android:name", "android.permission.ACCESS_WIFI_STATE")
            manifest_xml.documentElement.appendChild(ip_element)
            
            Log("Successfully added ACCESS_WIFI_STATE permission to app's AndroidManifest.xml file.")

    # Log(manifest_xml.toxml())
    return manifest_xml

def check_manifest(Log, manifest_file):
    
    manifest_xml = parse(manifest_file)
    # Log(manifest_xml.toxml())
    
    has_adjust_receiver = has_element_attr(manifest_xml, "receiver", "android:name", "com.adjust.sdk.AdjustReferrerReceiver")
    Log("Does app's AndroidManifest.xml file have the adjust broadcast receiver? {0}", has_adjust_receiver)

    has_internet_permission = has_element_attr(manifest_xml, "uses-permission", "android:name", "android.permission.INTERNET")
    Log("Does app's AndroidManifest.xml file have INTERNET permission? {0}", has_internet_permission)
    
    has_wifi_permission = has_element_attr(manifest_xml, "uses-permission", "android:name", "android.permission.ACCESS_WIFI_STATE")
    Log("Does app's AndroidManifest.xml file have ACCESS_WIFI_STATE permission? {0}", has_wifi_permission)

    return {"manifest_xml" : manifest_xml,
            "has_adjust_receiver" : has_adjust_receiver,
            "has_internet_permission" : has_internet_permission,
            "has_wifi_permission" : has_wifi_permission}

def has_element_attr(xml_dom, tag_name, attr_name, attr_value):
    for node in xml_dom.getElementsByTagName(tag_name):
        attr_dom = node.getAttribute(attr_name)
        if attr_dom == attr_value:
            return True
    return False

def copy_adjust_manifest(Log, android_plugin_path, adjust_android_path):
    adjust_manifest_path = os.path.join(adjust_android_path, "AdjustAndroidManifest.xml")
    new_manifest_path = os.path.join(android_plugin_path, "AndroidManifest.xml")

    if not os.path.exists(android_plugin_path):
        os.makedirs(android_plugin_path)

    try:
        shutil.copyfile(adjust_manifest_path, new_manifest_path)
    except Exception as e:
        Log(e)
    else:
        Log("Manifest copied from {0} to {1}", adjust_manifest_path, new_manifest_path)

def LogInput(writeObject):
    def Log(message, *args):
        messageNLine = str(message) + "\n"
        writeObject.write(messageNLine.format(*args))
    return Log

def parse_input(Log, parser):
    args, ignored_args = parser.parse_known_args()
    assets_path = args.assets_path

    android_plugin_path = os.path.join(assets_path, "Plugins/Android/")
    adjust_android_path = os.path.join(assets_path, "Adjust/Android/");

    Log("Android plugin path: {0}", android_plugin_path)
    Log("Android adjust path: {0}", adjust_android_path)

    return android_plugin_path, adjust_android_path, args.pre_build

if __name__ == "__main__":
    main()
