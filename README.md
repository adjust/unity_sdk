## Summary

This is the Unity3d SDK of adjust™. It supports iOS, Android, Windows 8.1 and Windows phone 8.1 targets. You can read more about adjust™ at [adjust.com].

## Basic Installation

These are the minimal steps required to integrate the adjust SDK into your
Unity3d project.

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Unzip the
Unity package in a folder of your choice.

### 2. Add it to your project

Open your project in the Unity Editor and navigate to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

![][import_package]

### 3. Integrate adjust into your app

Add the prefab located at `Assets/Adjust/Adjust.prefab` to the first scene.

Edit the parameters of the Adjust script in the Inspector menu of the added prefab.

![][adjust_editor]

Replace `{YourAppToken}` with your App Token. You can find in your [dashboard].

You can increase or decrease the amount of logs you see by changing the value
of `Log Level` to one of the following:

- `Verbose` - enable all logging
- `Debug` - enable more logging
- `Info` - the default
- `Warn` - disable info logging
- `Error` - disable warnings as well
- `Assert` - disable errors as well

If your target is windows based, to see the compiled logs from our library in `released` mode, it is
necessary to redirect the log output to your app while it's being tested in `debug` mode.

Call the method `setLogDelegate` in the `AdjustConfig` instance before starting the sdk.

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start (adjustConfig);
```

You can increase or decrease the amount of logs you see in tests by setting the
second argument of the `SetupLogging` method, `logLevel`, with one of the following values:

Depending on whether or not you build your app for testing or for production
you must change `Environment` with one of these values:

```
    'Sandbox'
    'Production'
```

**Important:** This value should be set to `Sandbox` if and only if you or
someone else is testing your app. Make sure to set the environment to
`Production` just before you publish the app. Set it back to `Sandbox` when you
start testing it again.

We use this environment to distinguish between real traffic and artificial
traffic from test devices. It is very important that you keep this value
meaningful at all times! Especially if you are tracking revenue.

If your app makes heavy use of event tracking, you might want to delay some
HTTP requests in order to send them in one batch every minute. You can enable
event buffering by ticking the box for `Event Buffering`.

If you don't want to start the adjust SDK at the `Awake` event of the game, tick the box `Start Manually`. Call the method `Adjust.start` with the `AdjustConfig` object as a parameter to start the adjust SDK instead.

For an example of scene with of a button menu with these options and others, open the example scene located at
`Assets/Adjust/ExampleGUI/ExampleGUI.unity`. The source for this scene is located at `Assets/Adjust/ExampleGUI/ExampleGUI.cs`.

### 4. Add Google Play Services

Since August 1st, 2014, apps in the Google Play Store must use the
[Google Advertising ID][google_ad_id] to uniquely identify devices. To allow
the adjust SDK to use the Google Advertising ID, you must integrate the [Google
Play Services][google_play_services]. If you haven't done this yet, you should
copy `google-play-services_lib` folder into the `Assets/Plugins/Android`
folder of your Unity project and after building your app, Google Play Services
should be integrated.

`google-play-services_lib` is part of the Android SDK, which you may already have installed.

There are two main ways to download the Android SDK. If you are using any tool which has the 
`Android SDK Manager`, you should download `Android SDK Tools`. Once installed, you can find 
the libraries in the `SDK_FOLDER/extras/google/google_play_services/libproject/` folder.

![][android_sdk_location]

If you are not using any tool which has Android SDK Manager, you should download the standalone version
of Android SDK from [official page][android_sdk_download]. By downloading this, you will have only a basic
version of the Android SDK which doesn't include the Android SDK Tools. There are more detailed instructions 
on how to download these in the readme file provided by Google, called `SDK Readme.txt`, which is placed in 
Android SDK folder.

### 5. Build scripts

To facilitate the build process we integrated build scripts for both Android and iOS. The script runs after each build and is called by the file `Assets/Editor/AdjustEditor.cs`. They require at least `python 2.7` installed to work.

It's possible to disable the post processing by clicking on the menu `Assets → Adjust → Change post processing status`.
Press the same button to re-enable it.

#### iOS

The iOS build script is located at `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py`. It changes the Unity3d iOS generated project in the following ways:

1. Adds the iAd and AdSupport frameworks to the project. This is required by the adjust SDK - check out the adjust
[iOS][ios] page for more details.

2. Adds the other linker flag `-ObjC`. This allows the adjust Objective-C categories to be recognized during the build time.

If you have a custom build that puts the Unity3d iOS generated project in a different location,
inform the script by clicking on the menu `Assets → Adjust → Set iOS build path` and choosing the build path of the iOS project.

After running, the script writes the log file `AdjustPostBuildiOSLog.txt` at the root of the Unity3d project with log messages of the script run.

#### Android

The android build script is located at `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py`. It changes the `AndroidManifest.xml` file located at `Assets/Plugins/Android/`. The problem with this approach is that, the manifest file used for the Android package was the one before the build process ended.

To mitigate this, simply run the build again, using the manifest created or changed by the previous run, or click on the menu `Assets → Adjust → Fix AndroidManifest.xml` so the script can run before the build process. Either way, it is only necessary to do this step once, as long the manifest file remains compatible with the adjust SDK.

![][menu_android]

If there is not a `AndroidManifest.xml` file at `Assets/Plugins/Android/` it creates a copy from our compatible manifest file `AdjustAndroidManifest.xml`. If there is already an `AndroidManifest.xml` file, it checks and changes the following:

1. Adds a broadcast receiver. For more details consult the adjust [Android][android] page for more details.

2. Adds the permission to connect to the internet.

3. Adds the permission to access information about Wi-Fi networks.

After running, the script writes the log file `AdjustPostBuildAndroidLog.txt` at the root of the Unity3d project with log messages of the script run.

## Additional features

Once you integrated the adjust SDK into your project, you can take advantage
of the following features.

### 6. Add tracking of custom events

You can tell adjust about any event you wish. Suppose you want to track
every tap on a button. You would have to create a new Event Token in your
[dashboard]. Let's say that Event Token is `abc123`. In your button's
click handler method you could then add the following line to track the click:

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");
Adjust.trackEvent (adjustEvent);
```

### 7. Add tracking of revenue

If your users can generate revenue by tapping on advertisements or making in-app purchases you can track those revenues with events. Lets say a tap is worth one Euro cent. You could then track the revenue event like this:

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");
adjustEvent.setRevenue (0.01, "EUR");
Adjust.trackEvent (adjustEvent);
```

#### iOS

##### <a id="deduplication"></a> Revenue deduplication

You can also pass in an optional transaction ID to avoid tracking duplicate revenues. The last ten transaction 
IDs are remembered and revenue events with duplicate transaction IDs are skipped. This is especially useful for 
in-app purchase tracking. See an example below.

If you want to track in-app purchases, please make sure to call `trackEvent` only if the transaction is finished
and item is purchased. That way you can avoid tracking revenue that is not actually being generated.

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");
adjustEvent.setRevenue (0.01, "EUR");
adjustEvent.setTransactionId ("transactionIdentifier");
Adjust.trackEvent (adjustEvent);
```

##### Receipt verification

If you track in-app purchases, you can also attach the receipt to the tracked event. In that case our servers 
will verify that receipt with Apple and discard the event if the verification failed. To make this work, you 
also need to send us the transaction ID of the purchase. The transaction ID will also be used for SDK side 
deduplication as explained [above](#deduplication):

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");
adjustEvent.setRevenue (0.01, "EUR");
adjustEvent.setReceipt ("receipt", "transactionId");
Adjust.trackEvent (adjustEvent);
```

### 8. Add callback parameters

You can also register a callback URL for that event in your [dashboard] and we
will send a GET request to that URL whenever the event gets tracked. In that
case you can also put some key-value-pairs in an object and pass it to the
`trackEvent` method. We will then append these named parameters to your
callback URL.

For example, suppose you have registered the URL
`http://www.adjust.com/callback` for your event with Event Token `abc123` and
execute the following lines:

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");

adjustEvent.addCallbackParameter ("key", "value");
adjustEvent.addCallbackParameter ("foo", "bar");

Adjust.trackEvent (adjustEvent);
```

In that case we would track the event and send a request to:

```
http://www.adjust.com/callback?key=value&foo=bar
```

It should be mentioned that we support a variety of placeholders like `{idfa}`
for iOS or `{android_id}` for Android that can be used as parameter values.  In
the resulting callback the `{idfa}` placeholder would be replaced with the ID
for Advertisers of the current device for iOS and the `{android_id}` would be
replaced with the AndroidID of the current device for Android. Also note that
we don't store any of your custom parameters, but only append them to your
callbacks.  If you haven't registered a callback for an event, these parameters
won't even be read.

### 9. Partner parameters

You can also add parameters to be transmitted to network partners, for the integrations that have been activated in your adjust dashboard.

This works similarly to the callback parameters mentioned above, but can be added by calling the addPartnerParameter method on your `AdjustEvent` instance.

```cs
AdjustEvent adjustEvent = new AdjustEvent ("abc123");

adjustEvent.addPartnerParameter ("key", "value");
adjustEvent.addPartnerParameter ("foo", "bar");

Adjust.trackEvent (adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners.][special-partners]

### 9. Receive attribution change callback

You can register a callback to be notified of tracker attribution changes. Due to the different sources considered for attribution, this information can not by provided synchronously. Follow these steps to implement the optional callback in your application:

Please make sure to consider [applicable attribution data policies.][attribution_data]

1. Create a method with the signature of the delegate `Action<AdjustAttribution>`.

2. After creating the `AdjustConfig` object, call the `adjustConfig.setAttributionChangedDelegate`
with the previously created method. It is also be possible to use a lambda with the same signature.

3. If instead of using the `Adjust.prefab`, the `Adjust.cs` script was added to another `GameObject`.
Don't forget to pass the name of that `GameObject` as the second parameter of `AdjustConfig.setAttributionChangedDelegate`.

As the callback is configured using the AdjustConfig instance, you should call `adjustConfig.setAttributionChangedDelegate` before calling `Adjust.start`.

The callback function will get called when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `string trackerToken` the tracker token of the current install.
- `string trackerName` the tracker name of the current install.
- `string network` the network grouping level of the current install.
- `string campaign` the campaign grouping level of the current install.
- `string adgroup` the ad group grouping level of the current install.
- `string creative` the creative grouping level of the current install.

```cs
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour {
{
    void OnGUI () {
    {
        if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), "callback")) 
        {
            AdjustConfig adjustConfig = new AdjustConfig ("{Your App Token}", AdjustEnvironment.Sandbox);
            adjustConfig.setLogLevel (AdjustLogLevel.Verbose);
            adjustConfig.setAttributionChangedDelegate (this.attributionChangedDelegate);

            Adjust.start (adjustConfig);
        }
    }
    
    public void attributionChangedDelegate (AdjustAttribution attribution)
    {
        Debug.Log ("Attribution changed");
        
        // ...
    }
}
```

### 10. Disable tracking

You can disable the adjust SDK from tracking by invoking the method `setEnabled`
with the enabled parameter as `false`. This setting is remembered between sessions, but it can only
be activated after the first session.

```cs
Adjust.setEnabled(false);
```

You can verify if the adjust SDK is currently active with the method `isEnabled`. It is always possible
to activate the adjust SDK by invoking `setEnabled` with the `enabled` parameter set to `true`.

### 11. Offline mode

You can put the adjust SDK in offline mode to suspend transmission to our servers, 
while retaining tracked data to be sent later. While in offline mode, all information is saved
in a file, so be careful not to trigger too many events while in offline mode.

You can activate offline mode by calling `setOfflineMode` with the parameter `true`.

```cs
Adjust.setOfflineMode(true);
```

Conversely, you can deactivate offline mode by calling `setOfflineMode` with `false`.
When the adjust SDK is put back in online mode, all saved information is send to our servers 
with the correct time information.

Unlike disabling tracking, this setting is *not remembered*
bettween sessions. This means that the SDK is in online mode whenever it is started,
even if the app was terminated in offline mode.

### 12. Device IDS

Certain services (such as Google Analytics) require you to coordinate Device and Client IDs in order to prevent duplicate reporting. 

#### Android

If you need to obtain the Google Advertising ID, There is a restriction that only allows it to be read in a background thread. 
If you call the function `getGoogleAdId` with a `Action<string>` delegate, it will work in any situation:

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

Inside the method `onGoogleAdIdRead` of the `OnDeviceIdsRead` instance, you will have access to the Google Advertising ID as the variable `googleAdId`.

#### iOS

To obtain the IDFA, call the function `getIdfa`:

```cs
Adjust.getIdfa ()
```

## Troubleshooting

### iOS
Even with the post build script it is possible that the project is not ready to run out of the box.

If needed, disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.

### Build scripts

The post build scripts require execute permissions to be able to run. If the build process freezes in the end and opens one of the script files, this may be that your system is configured to not allow scripts to run by default. If this is the case, use the `chmod` tool in both `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py` and `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py` to add execute privileges.


[adjust.com]: http://adjust.com
[dashboard]: http://adjust.com
[releases]: https://github.com/adjust/adjust_unity_sdk/releases
[import_package]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[adjust_editor]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[menu_android]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[ios]: https://github.com/adjust/ios_sdk
[android]: https://github.com/adjust/android_sdk
[attribution_data]: https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]: https://docs.adjust.com/en/special-partners
[google_ad_id]: https://developer.android.com/google/play-services/id.html
[google_play_services]: http://developer.android.com/google/play-services/setup.html
[android_sdk_download]: https://developer.android.com/sdk/index.html#Other
[android_sdk_location]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png

## License

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

The adjust-sdk is licensed under the MIT License.

Copyright (c) 2012-2015 adjust GmbH,
http://www.adjust.com

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
