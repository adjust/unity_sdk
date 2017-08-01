## Summary

This is the Unity SDK of Adjust™. It supports iOS, Android, Windows 8.1 and Windows phone 8.1 targets. You can read more
about Adjust™ at [adjust.com].

## Table of contents

* [Basic integration](#basic-integration)
   * [Get the SDK](#sdk-get)
   * [Add the SDK to your project](#sdk-add)
   * [Integrate the SDK into your app](#sdk-integrate)
   * [Adjust logging](#adjust-logging)
   * [Google Play Services](#google-play-services)
   * [Post build process](#post-build-process)
      * [iOS post build process](#post-build-ios)
      * [Android post build process](#post-build-android)
* [Additional features](#additional-features)
   * [Event tracking](#event-tracking)
      * [Revenue tracking](#revenue-tracking)
      * [Revenue deduplication](#revenue-deduplication)
      * [In-App Purchase verification](#iap-verification)
      * [Callback parameters](#callback-parameters)
      * [Partner parameters](#partner-parameters)
   * [Session parameters](#session-parameters)
      * [Session callback parameters](#session-callback-parameters)
      * [Session partner parameters](#session-partner-parameters)
      * [Delay start](#delay-start)
   * [Attribution callback](#attribution-callback)
   * [Session and event callbacks](#session-event-callbacks)
   * [Disable tracking](#disable-tracking)
   * [Offline mode](#offline-mode)
   * [Event buffering](#event-buffering)
   * [Background tracking](#background-tracking)
   * [Device IDs](#device-ids)
      * [iOS advertising identifier](#di-idfa)
      * [Google Play Services advertising identifier](#di-gps-adid)
      * [Adjust device identifier](#di-adid)
   * [User attribution](#user-attribution)
   * [Push token](#push-token)
   * [Pre-installed trackers](#pre-installed-trackers)
   * [Deep linking](#deeplinking)
      * [Standard deep linking scenario](#deeplinking-standard)
      * [Deferred deep linking scenario](#deeplinking-deferred)
      * [Deep linking handling in Android app](#deeplinking-app-android)
      * [Deep linking handling in iOS app](#deeplinking-app-ios)
* [Troubleshooting](#troubleshooting)
   * [Debug information in iOS](#ts-debug-ios)
   * [Build scripts fails to be executed](#ts-build-script-fail)
* [License](#license)

## <a id="basic-integration">Basic integration

These are the minimal steps required to integrate the Adjust SDK into your Unity project.

### <a id="sdk-get">Get the SDK

Download the latest version from our [releases page][releases]. In there you will find two Unity packages:

* **Adjust_v4.11.3_Unity_4.unitypackage** - Use this package if you are using **Unity IDE version 4**.
* **Adjust_v4.11.3_Unity_5.unitypackage** - Use this package if you are using **Unity IDE version 5**.

### <a id="sdk-add">Add the SDK to your project

Open your project in the Unity Editor and navigate to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

![][import_package]

### <a id="sdk-integrate">Integrate the SDK into your app

Add the prefab located at `Assets/Adjust/Adjust.prefab` to the first scene.

Edit the parameters of the Adjust script in the `Inspector menu` of the added prefab.

![][adjust_editor]

You have the possibility to set up the following options on the Adjust prefab:

* [Start Manually](#start-manually)
* [Event Buffering](#event-buffering)
* [Print Attribution](#attribution-callback)
* [Send In Background](#background-tracking)
* [Make AdWords Request](#adwords-tracking)
* [Launch Deferred Deep Link](#deeplinking-deferred-open)
* [App Token](#app-token)
* [Log Level](#adjust-logging)
* [Environment](#environment)

<a id="app-token">Replace `{YourAppToken}` with your actual App Token. You can find in your [dashboard].

<a id="environment">Depending on whether you are building your app for testing or for production, you must change `Environment` to one of these values:

```
    'Sandbox'
    'Production'
```

**Important:** This value should be set to `Sandbox` if, and only if, you or someone else is testing your app. Make sure to set the environment to `Production` just before you publish the app. Set it back to `Sandbox` when you start testing it again.

We use this environment to distinguish between real traffic and artificial traffic from test devices. It is very important that you keep this value meaningful at all times! Especially if you are tracking revenue.

<a id="start-manually">If you don't want the Adjust SDK to start automatically at the `Awake` event of the app, check the box `Start Manually`. With this option selected, you will need to initialize and start the Adjust SDK from the within the code. Call the method `Adjust.start` with the `AdjustConfig` object as a parameter to start the Adjust SDK.

For an example of scene with of a button menu with these options and others, open the example scene located at `Assets/Adjust/ExampleGUI/ExampleGUI.unity`. The source for this scene is located at `Assets/Adjust/ExampleGUI/ExampleGUI.cs`.

### <a id="adjust-logging">Adjust logging

You can increase or decrease the amount of logs you see by changing the value of `Log Level` to one of the following:

- `Verbose` - enable all logging
- `Debug` - enable more logging
- `Info` - the default
- `Warn` - disable info logging
- `Error` - disable warnings as well
- `Assert` - disable errors as well
- `Suppress` - disable all logging

If want all your log output to be disabled, and if you are initialising the Adjust SDK manually from code, beside setting the log level to suppress, you should also use  constructor for `AdjustConfig` object, which gets a boolean parameter indicating whether the suppress log level should be supported or not:

```cs
string appToken = "{YourAppToken}";
string environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

If your target is Windows-based, to see the compiled logs from our library in `released` mode, it is necessary to redirect the log output to your app while it's being tested in `debug` mode.

Call the method `setLogDelegate` in the `AdjustConfig` instance before starting the sdk.

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="google-play-services">Google Play Services

Since August 1st, 2014, apps in the Google Play Store must use the [Google Advertising ID][google_ad_id] to uniquely identify devices. To allow the Adjust SDK to use the Google Advertising ID, you must integrate [Google Play Services][google_play_services]. If you haven't done this yet, you should copy `google-play-services_lib` folder into the `Assets/Plugins/Android` folder of your Unity project, and after building your app, Google Play Services should be integrated.

`google-play-services_lib` is part of the Android SDK, which you may already have installed.

There are two main ways to download the Android SDK. If you are using any tool which has the `Android SDK Manager`, you should download `Android SDK Tools`. Once installed, you can find the libraries in the `SDK_FOLDER/extras/google/google_play_services/libproject/` folder.

![][android_sdk_location]

If you are not using any tool which has Android SDK Manager, you should download the standalone version of Android SDK from [official page][android_sdk_download]. By downloading this, you will have only a basic version of the Android SDK which doesn't include the Android SDK Tools. There are more detailed instructions on how to download these in the readme file provided by Google, called `SDK Readme.txt`, which is placed in Android SDK folder.

**Update**: In case you are having newer Android SDK version installed, Google has changed the structure of the Google Play Services folders inside of the root SDK folder. Structure described above is changed and now it looks like this:

![][android_sdk_location_new]

Since now you have possibility to access separate parts of the Google Play Services library and not just the whole library like before, you can add just the part of the Google Play Services library which Adjust SDK needs - the basement part. Add the `play-services-basement-x.y.z.aar` file to your `Assets/Plugins/Android` folder and Google Play Services needed by the Adjust SDK should be successfully integrated.

### <a id="post-build-process">Post build process

To facilitate the build process, post build process will be performed by the Adjust unity package in order to enable the Adjust SDK to work properly. There is a difference in how this process is performed in `Unity 4` and `Unity 5`.

If you are using the Adjust unity package for `Unity 4`, this process is going to be performed by executing post build Python scripts:

- The iOS Python build script is located at `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py`.
- The Android Python build script is located at `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py`.

The script runs after each build and is called by the file `Assets/Editor/AdjustEditor.cs`. They require at least `Python 2.7` installed to work. It's possible to disable post processing by clicking on the menu `Assets → Adjust → Change post processing status`. Press the same button to re-enable it.

If you are using the Adjust unity package for `Unity 5`, this process is going to be performed by `OnPostprocessBuild` method in `AdjustEditor.cs`. In order for iOS post build process to be executed properly, your `Unity 5` should have `iOS build support` installed.

After running in `Unity 4`, the script writes the log file `AdjustPostBuildAndroidLog.txt` at the root of the Unity project with log messages of the script run. In `Unity 5`, all log messages are written to the Unity IDE console output window.

#### <a id="post-build-ios">iOS post build process

iOS post build process is performing the following changes in your generated Xcode projet:

1. Adds the `iAd.framework` and `AdSupport.framework` to the project. This is required by the Adjust SDK - check out the official [iOS SDK README][ios] for more details.
2. Adds the other linker flag `-ObjC`. This allows the Adjust Objective-C categories to be recognized during build time.
3. Enables `Objective-C exceptions`.

If you are using `Unity 4` and if you have a custom build that puts the Unity iOS generated project in a different location, inform the script by clicking on the menu `Assets → Adjust → Set iOS build path` and choosing the build path of the iOS project.

#### <a id="post-build-android">Android post build process

Android post build process is performing changes in `AndroidManifest.xml` file located at `Assets/Plugins/Android/`.

The problem with this approach with `Unity 4` is that the manifest file used for the Android package was the same one as before the build process ended. To mitigate this, simply run the build again, using the manifest created or changed by the previous run, or click on the menu `Assets → Adjust → Fix AndroidManifest.xml` so the script can run before the build process. Either way, it is only necessary to do this step once, as long as the manifest file remains compatible with the Adjust SDK.

![][menu_android]

This doesn't need to be performed for Android post build process in `Unity 5`.

Android post build process initially checks for the presence of `AndroidManifest.xml` file in the Android plugins folder. If there is no `AndroidManifest.xml` file in `Assets/Plugins/Android/` it creates a copy from our compatible manifest file `AdjustAndroidManifest.xml`. If there is already an `AndroidManifest.xml` file, it checks and changes the following:

1. Adds the Adjust broadcast receiver. For more details, consult the official [Android SDK README][android]. Please, have in mind that if you are using your **own broadcast receiver** which handles `INSTALL_REFERRER` intent, you don't need the Adjust broadcast receiver to be added in your manifest file. Remove it, but inside your own receiver add the call to the Adjust broadcast receiver like described in [Android guide][android-custom-receiver].
2. Adds the permission to connect to the internet.
3. Adds the permission to access information about Wi-Fi networks.

## <a id="additional-features">Additional features

Once you integrated the Adjust SDK into your project, you can take advantage of the following features.

### <a id="event-tracking">Event tracking

You can tell Adjust about any event you wish. Suppose you want to track every tap on a button. You would just need to create a new Event Token in your [dashboard]. Let's say that Event Token is `abc123`. In your button's click handler method you could then add the following lines to track the click:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-tracking">Revenue tracking

If your users can generate revenue by tapping on advertisements or making In-App Purchases you can track those revenues with events. Let's say a tap is worth one Euro cent. You could then track the revenue event like this:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-deduplication"></a>Revenue deduplication

You can also add an optional transaction ID to avoid tracking duplicated revenues. The last ten transaction IDs are remembered, and revenue events with duplicated transaction IDs are skipped. This is especially useful for In-App Purchase tracking. You can see an example below.

If you want to track in-app purchases, please make sure to call the `trackEvent` only if the transaction is finished and item is purchased. That way you can avoid tracking revenue that is not actually being generated.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

#### <a id="iap-verification">In-App Purchase verification

If you want to check the validity of In-App Purchases made in your app using Purchase Verification, Adjust's server side receipt verification tool, then check out our `Unity purchase SDK` and read more about it [here][unity-purchase-sdk].

#### <a id="callback-parameters">Callback parameters

You can also register a callback URL for that event in your [dashboard] and we will send a GET request to that URL whenever the event gets tracked. In that case you can also put some key-value pairs in an object and pass it to the `trackEvent` method. We will then append these named parameters to your callback URL.

For example, suppose you have registered the URL `http://www.adjust.com/callback` for your event with Event Token `abc123` and execute the following lines:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

In that case we would track the event and send a request to:

```
http://www.adjust.com/callback?key=value&foo=bar
```

It should be mentioned that we support a variety of placeholders like `{idfa}` for iOS or `{gps_adid}` for Android that can be used as parameter values.  In the resulting callback, the `{idfa}` placeholder would be replaced with the ID for advertisers of the current device for iOS and the `{gps_adid}` would be replaced with the Google Play Services ID of the current device for Android. Also note that we don't store any of your custom parameters, but only append them to your callbacks. If you haven't registered a callback for an event, these parameters won't even be read.

#### <a id="partner-parameters">Partner parameters

You can also add parameters to be transmitted to network partners, for the integrations that have been activated in your Adjust dashboard.

This works similarly to the callback parameters mentioned above, but can be added by calling the `addPartnerParameter` method on your `AdjustEvent` instance.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners][special-partners].

### <a id="session-parameters">Session parameters

Some parameters are saved to be sent in every event and session of the Adjust SDK. Once you have added any of these parameters once, you don't need to add them every time, since they will be saved locally. If you add the same parameter twice, there will be no effect.

These session parameters can be called before the Adjust SDK is launched to make sure they are sent even on install. If you need to send them with an install, but can only obtain the needed values after launch, it's possible to [delay](#delay-start) the first launch of the Adjust SDK to allow this behaviour.

### <a id="session-callback-parameters"> Session callback parameters

The same callback parameters that are registered for [events](#callback-parameters) can also be saved to be sent in every event or session of the Adjust SDK.

The session callback parameters have a similar interface to the event callback parameters. Instead of adding the key and it's value to an event, it's added through a call to `addSessionCallbackParameter` method of the `Adjust` instance.

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

The session callback parameters will be merged with the callback parameters added to an event. The callback parameters added to an event have precedence over the session callback parameters. Meaning that, when adding a callback parameter to an event with the same key to one added from the session, the value that prevails is the callback parameter added to the event.

It's possible to remove a specific session callback parameter by passing the desiring key to the `removeSessionCallbackParameter` method of the `Adjust` instance.

```cs
Adjust.removeSessionCallbackParameter("foo");
```

If you wish to remove all key and values from the session callback parameters, you can reset it with the method `resetSessionCallbackParameters` of the `Adjust` instance.

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="session-partner-parameters">Session partner parameters

In the same way that there are [session callback parameters](#session-callback-parameters) that are sent every in event or session of the Adjust SDK, there is also session partner parameters.

These will be transmitted to network partners, for whom the integrations have been activated in your Adjust [dashboard].

The session partner parameters have a similar interface to the event partner parameters. Instead of adding the key and it's value to an event, it's added through a call to `addSessionPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

The session partner parameters will be merged with the partner parameters added to an event. The partner parameters added to an event have precedence over the session partner parameters. Meaning that, when adding a partner parameter to an event with the same key to one added from the session, the value that prevails is the partner parameter added to the event.

It's possible to remove a specific session partner parameter by passing the desiring key to the `removeSessionPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.removeSessionPartnerParameter("foo");
```

If you wish to remove all key and values from the session partner parameters, you can reset it with the `resetSessionPartnerParameters` method of the `Adjust` instance.

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="delay-start">Delay start

Delaying the start of the Adjust SDK allows your app some time to obtain session parameters, such as unique identifiers, to be send on install.

Set the initial delay time in seconds with the method `setDelayStart` in the `AdjustConfig` instance:

```cs
adjustConfig.setDelayStart(5.5);
```

In this case the Adjust SDK not send the initial install session and any event created for 5.5 seconds. After this time is expired or if you call `Adjust.sendFirstPackages()` in the meanwhile, every session parameter will be added to the delayed install session and events and the Adjust SDK will resume as usual.

**The maximum delay start time of the Adjust SDK is 10 seconds**.

### <a id="attribution-callback">Attribution callback

You can register a callback to be notified of tracker attribution changes. Due to the different sources considered for attribution, this information cannot be provided synchronously. Follow these steps to implement the optional callback in your application:

Please make sure to consider [applicable attribution data policies][attribution_data].

1. Create a method with the signature of the delegate `Action<AdjustAttribution>`.

2. After creating the `AdjustConfig` object, call the `adjustConfig.setAttributionChangedDelegate` with the previously created method. It is also be possible to use a lambda with the same signature.

3. If instead of using the `Adjust.prefab`, the `Adjust.cs` script was added to another `GameObject`. Don't forget to pass the name of that `GameObject` as the second parameter of `AdjustConfig.setAttributionChangedDelegate`.

As the callback is configured using the AdjustConfig instance, you should call `adjustConfig.setAttributionChangedDelegate` before calling `Adjust.start`.

The callback function will get called when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `string trackerToken` the tracker token of the current install.
- `string trackerName` the tracker name of the current install.
- `string network` the network grouping level of the current install.
- `string campaign` the campaign grouping level of the current install.
- `string adgroup` the ad group grouping level of the current install.
- `string creative` the creative grouping level of the current install.
- `string clickLabel` the click label of the current install.
- `string adid` the Adjust device identifier.

```cs
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour {
    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "callback")) {
            AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
            adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
            adjustConfig.setAttributionChangedDelegate(this.attributionChangedDelegate);

            Adjust.start(adjustConfig);
        }
    }

    public void attributionChangedDelegate(AdjustAttribution attribution) {
        Debug.Log("Attribution changed");

        // ...
    }
}
```

### <a id="session-event-callbacks">Session and event callbacks

You can register a callback to be notified of successful and failed events and/or sessions.

Follow the same steps to implement the following callback function for successful tracked events:

```cs
// ...

AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
adjustConfig.setEventSuccessDelegate(EventSuccessCallback);

Adjust.start(adjustConfig);

// ...

public void EventSuccessCallback(AdjustEventSuccess eventSuccessData) {
    // ...
}
```

The following callback function for failed tracked events:

```cs
// ...

AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
adjustConfig.setEventFailureDelegate(EventFailureCallback);

Adjust.start(adjustConfig);

// ...

public void EventFailureCallback(AdjustEventFailure eventFailureData) {
    // ...
}
```

For successful tracked sessions:

```cs
// ...

AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);

Adjust.start(adjustConfig);

// ...

public void SessionSuccessCallback (AdjustSessionSuccess sessionSuccessData) {
    // ...
}
```

And for failed tracked sessions:

```cs
// ...

AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
adjustConfig.setSessionFailureDelegate(SessionFailureCallback);

Adjust.start(adjustConfig);

// ...

public void SessionFailureCallback (AdjustSessionFailure sessionFailureData) {
    // ...
}
```

The callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK.
- `string Timestamp` timestamp from the server.
- `string Adid` a unique device identifier provided by Adjust.
- `Dictionary<string, object> JsonResponse` the JSON object with the response from the server.

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event.

And both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time.

### <a id="disable-tracking">Disable tracking

You can disable the Adjust SDK from tracking by invoking the method `setEnabled` with the enabled parameter as `false`. **This setting is remembered between sessions**, but it can only be activated after the first session.

```cs
Adjust.setEnabled(false);
```

You can verify if the Adjust SDK is currently active with the method `isEnabled`. It is always possible to activate the Adjust SDK by invoking `setEnabled` with the `enabled` parameter set to `true`.

### <a id="offline-mode">Offline mode

You can put the Adjust SDK in offline mode to suspend transmission to our servers, while retaining tracked data to be sent later. When in offline mode, all information is saved in a file, so be careful not to trigger too many events.

You can activate offline mode by calling `setOfflineMode` with the parameter `true`.

```cs
Adjust.setOfflineMode(true);
```

Conversely, you can deactivate offline mode by calling `setOfflineMode` with `false`. When the Adjust SDK is put back in online mode, all saved information is send to our servers with the correct time information.

Unlike disabling tracking, **this setting is not remembered** between sessions. This means that the SDK is in online mode whenever it is started, even if the app was terminated in offline mode.

### <a id="event-buffering">Event buffering

If your app makes heavy use of event tracking, you might want to delay some HTTP requests in order to send them in one batch every minute. You can enable event buffering with your `AdjustConfig` instance:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

If nothing is set, event buffering is **disabled by default**.

### <a id="background-tracking">Background tracking

The default behaviour of the Adjust SDK is to **pause sending HTTP requests while the app is in the background**. You can change this in your `AdjustConfig` instance:

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="device-ids">Device IDs

Certain services (such as Google Analytics) require you to coordinate Device and Client IDs in order to prevent duplicate reporting.

### <a id="di-idfa">iOS Advertising Identifier

To obtain the IDFA, call the function `getIdfa` of the `Adjust` instance:

```cs
string idfa = Adjust.getIdfa()
```

### <a id="di-gps-adid">Google Play Services advertising identifier

If you need to obtain the Google Advertising ID, There is a restriction that only allows it to be read in a background thread. If you call the method `getGoogleAdId` of the `Adjust` instance with an `Action<string>` delegate, it will work in any situation:

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

You will have access to the Google Advertising ID as the variable `googleAdId`.

### <a id="di-adid"></a>Adjust device identifier

For each device with your app installed on it, Adjust backend generates unique **Adjust device identifier** (**adid**). In order to obtain this identifier, you can make a call to following method on `Adjust` instance:

```cs
String adid = Adjust.getAdid();
```

**Note**: Information about **adid** is available after app installation has been tracked by the Adjust backend. From that moment on, the Adjust SDK has information about your device **adid** and you can access it with this method. So, **it is not possible** to access the **adid** value before the SDK has been initialised and installation of your app has been successfully tracked.

### <a id="user-attribution"></a>User attribution

As described in the [attribution callback section](#attribution-callback), this callback is triggered, providing you with information about a new attribution whenever it changes. If you want to access information about a user's current attribution whenever you need it, you can make a call to the following method of the `Adjust` instance:

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**Note**: Information about current attribution is available after app installation has been tracked by the Adjust backend and the attribution callback has been initially triggered. From that moment on, the Adjust SDK has information about a user's attribution and you can access it with this method. So, **it is not possible** to access a user's attribution value before the SDK has been initialized and an attribution callback has been triggered.

### <a id="push-token">Push token

To send us the push notification token, please call `setDeviceToken` method on the `Adjust` instance **when you obtain your app's push notification token and when ever it changes it's value**:

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="pre-installed-trackers">Pre-installed trackers

If you want to use the Adjust SDK to recognize users that found your app pre-installed on their device, follow these steps.

1. Create a new tracker in your [dashboard].
2. Set the default tracker of your `AdjustConfig`:

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. Please note that the dashboard displays a tracker
  URL (including `http://app.adjust.com/`). In your source code, you should specify only the six-character token and not
  the entire URL.

3. Build and run your app. You should see a line like the following in the log output:

    ```
    Default tracker: 'abc123'
    ```

### <a id="deeplinking">Deep linking

**Deep linking is supported only on iOS and Android platforms.**

If you are using the Adjust tracker URL with an option to deep link into your app from the URL, there is the possibility to get information about the deep link URL and its content. Hitting the URL can happen when the user already has your app installed (standard deep linking scenario) or if they don't have the app on their device (deferred deep linking scenario). In the standard deep linking scenario, the Android platform natively offers the possibility for you to get the information about the deep link content. The deferred deep linking scenario is something that the Android platform doesn't support out of the box; in this case, the Adjust SDK will offer you the mechanism to get the information about the deep link content.

You need to set up deep linking handling in your app **on native level** - in your generated Xcode project (for iOS) and Android Studio / Eclipse project (for Android).

#### <a id="deeplinking-standard">Standard deep linking scenario

Unfortunately, in this scenario the information about the deep link can not be delivered to you in your Unity C# code. Once you enable your app to handle deep linking, you will get information about the deep link on native level. For more information check our chapters below on how to enable deep linking for Android and iOS apps.

#### <a id="deeplinking-deferred">Deferred deep linking scenario

In order to get information about the URL content in a deferred deep linking scenario, you should set a callback method on the `AdjustConfig` object which will receive one `string` parameter where the content of the URL will be delivered. You should set this method on the config object by calling the method `setDeferredDeeplinkDelegate`:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

<a id="deeplinking-deferred-open">In deferred deep linking scenario, there is one additional setting which can be set on the `AdjustConfig` object. Once the Adjust SDK gets the deferred deep link information, we offer you the possibility to choose whether our SDK should open this URL or not. You can choose to set this option by calling the `setLaunchDeferredDeeplink` method on the config object:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log ("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setLaunchDeferredDeeplink(true);
adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

If nothing is set, **the Adjust SDK will always try to launch the URL by default**.

To enable your apps to support deep linking, you should set up schemes for each supported platform.

#### <a id="deeplinking-app-android">Deep linking handling in Android app

**This should be done in native Android Studio / Eclipse project.**

To set up your Android app to handle deep linking on native level, please follow our [guide][android-deeplinking] in the official Android SDK README.

#### <a id="deeplinking-app-ios">Deep linking handling in iOS app

**This should be done in native Xcode project.**

To set up your iOS app to handle deep linking on native level, please follow our [guide][ios-deeplinking] in the official iOS SDK README.

## <a id="troubleshooting">Troubleshooting

### <a id="ts-debug-ios">Debug information in iOS

Even with the post build script it is possible that the project is not ready to run out of the box.

If needed, disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.

### <a id="ts-build-script-fail">Build scripts fails to be executed

The post build scripts require execute permissions to be able to run. If the build process freezes in the end and opens one of the script files, this may be that your system is configured to not allow scripts to run by default. If this is the case, use the `chmod` tool in both `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py` and `Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py` to add execute privileges.


[dashboard]:               http://adjust.com
[adjust.com]:              http://adjust.com

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/adjust_unity_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/ios_sdk/#deeplinking-reattribution
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/en/special-partners
[unity-purchase-sdk]:      https://github.com/adjust/unity_purchase_sdk
[android-deeplinking]:     https://github.com/adjust/android_sdk#deep-linking
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

## <a id="license">License

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License").
You may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-2017 Adjust GmbH,
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
