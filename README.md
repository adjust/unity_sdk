## Summary

This is the Unity SDK of Adjust™. It supports iOS, Android, Windows Store 8.1, Windows Phone 8.1 and Windows 10 targets. You can read more about Adjust™ at [adjust.com]. 

**Note**: As of version **4.12.0**, Adjust Unity SDK is compatible with **Unity 5 and newer** versions.

**Note**: As of version **4.19.2**, Adjust Unity SDK is compatible with **Unity 2017.1.1 and newer** versions.

**Note**: As of version **4.21.0**, Adjust Unity SDK is compatible with **Unity 2017.4.1 and newer** versions.

Read this in other languages: [English][en-readme], [中文][zh-readme], [日本語][ja-readme], [한국어][ko-readme].

## Table of contents

### Quick start

   * [Getting started](#qs-getting-started)
      * [Get the SDK](#qs-get-sdk)
      * [Add the SDK to your project](#qs-add-sdk)
      * [Integrate the SDK into your app](#qs-integrate-sdk)
      * [Adjust logging](#qs-adjust-logging)
      * [Google Play Services](#qs-gps)
      * [Proguard settings](#qs-android-proguard)
      * [Google Install Referrer](#qs-install-referrer)
      * [Post-build process](#qs-post-build-process)
        * [iOS post-build process](#qs-post-build-ios)
        * [Android post-build process](#qs-post-build-android)
      * [SDK signature](#qs-sdk-signature)

### Deeplinking

   * [Deeplinking overview](#dl)
   * [Standard deeplinking](#dl-standard)
   * [Deferred deeplinking](#dl-deferred)
   * [Deeplink handling in Android apps](#dl-app-android)
   * [Deeplink handling in iOS apps](#dl-app-ios)
      
### Event tracking

   * [Track event](#et-tracking)
   * [Track revenue](#et-revenue)
   * [Deduplicate revenue](#et-revenue-deduplication)
   * [Verify in-app purchase](#et-purchase-verification)

### Custom parameters

   * [Custom parameters overview](#cp)
   * [Event parameters](#cp-event-parameters)
      * [Event callback parameters](#cp-event-callback-parameters)
      * [Event partner parameters](#cp-event-partner-parameters)
      * [Event callback identifier](#cp-event-callback-id)
   * [Session parameters](#cp-session-parameters)
      * [Session callback parameters](#cp-session-callback-parameters)
      * [Session partner parameters](#cp-session-partner-parameters)
      * [Delay start](#cp-delay-start)

### Additional features

   * [Push token (uninstall tracking)](#ad-push-token)
   * [Attribution callback](#ad-attribution-callback)
   * [Ad revenue tracking](#ad-ad-revenue)
   * [Session and event callbacks](#ad-session-event-callbacks)
   * [User attribution](#ad-user-attribution)
   * [Device IDs](#ad-device-ids)
      * [iOS advertising identifier](#ad-idfa)
      * [Google Play Services advertising identifier](#ad-gps-adid)
      * [Amazon advertising identifier](#ad-amazon-adid)
      * [Adjust device identifier](#ad-adid)
   * [Pre-installed trackers](#ad-pre-installed-trackers)
   * [Offline mode](#ad-offline-mode)
   * [Disable tracking](#ad-disable-tracking)
   * [Event buffering](#ad-event-buffering)
   * [Background tracking](#ad-background-tracking)
   * [GDPR right to be forgotten](#ad-gdpr-forget-me)
   * [Disable third-party sharing](#ad-disable-third-party-sharing)

### Testing and troubleshooting
   * [Debug information in iOS](#tt-debug-ios)

### License
  * [License agreement](#license)


## Quick start

### <a id="qs-getting-started"></a>Getting started

To integrate the Adjust SDK into your Unity project, follow these steps.

### <a id="qs-get-sdk"></a>Get the SDK

As of version `4.19.2`, you can add Adjust SDK from [Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/adjust-sdk-160890) to your app. Alternativly, you can download the latest version from our [releases page][releases].

### <a id="qs-add-sdk"></a>Add the SDK to your project

Open your project in the Unity Editor, go to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

![][import_package]

### <a id="qs-integrate-sdk"></a>Integrate the SDK into your app

Add the prefab from `Assets/Adjust/Adjust.prefab` to the first scene.

You can edit the Adjust script parameters in the prefab `Inspector menu` to set up the following options:

* [start manually](#start-manually)
* [event buffering](#event-buffering)
* [send in background](#background-tracking)
* [launch deferred deeplink](#deeplinking-deferred-open)
* [app token](#app-token)
* [log level](#adjust-logging)
* [environment](#environment)

![][adjust_editor]

<a id="app-token">Replace `{YourAppToken}` with your actual App Token. Follow [these steps](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token) to find it in the dashboard. 

<a id="environment">Depending on whether you are building your app for testing or for production, change the `Environment` setting to either 'Sandbox' or 'Production'.

**Important:** Set the value to `Sandbox` if you or someone else is testing your app. Make sure to set the environment to `Production` before you publish the app. Set it back to `Sandbox` if you start testing again. Also, have in mind that by default Adjust dashboard is showing production traffic of your app, so in case you want to see traffic you generated while testing in sandbox mode, make sure to switch to sandbox traffic view within dashboard.

We use the environment setting to distinguish between real traffic and artificial traffic from test devices. Please make sure to keep your environment setting updated.

<a id="start-manually">If you don't want the Adjust SDK to start automatically with the app's `Awake` event, select `Start Manually`. With this option, you'll initialize and start the Adjust SDK from the within the code by calling the `Adjust.start` method with the `AdjustConfig` object as a parameter.

You can find an example scene with a button menu showing these options here: `Assets/Adjust/ExampleGUI/ExampleGUI.unity`. 

The source for this scene is located at `Assets/Adjust/ExampleGUI/ExampleGUI.cs`.

### <a id="qs-adjust-logging"></a>Adjust logging

You can increase or decrease the granularity of the logs you see by changing the value of `Log Level` to one of the following:

- `Verbose` - enable all logs
- `Debug` - disable verbose logs
- `Info` - disable debug logs (default)
- `Warn` - disable info logs
- `Error` - disable warning logs
- `Assert` - disable error logs
- `Suppress` - disable all logs

If you want to disable all of your log output when initializing the Adjust SDK manually, set the log level to suppress and use a constructor for the `AdjustConfig` object. This opens a boolean parameter where you can enter whether the suppress log level should be supported or not:

```cs
string appToken = "{YourAppToken}";
AdjustEnvironment environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

If your target is Windows-based and you want to see the compiled logs from our library in `released` mode, redirect the log output to your app while testing it in `debug` mode.

Call the method `setLogDelegate` in the `AdjustConfig` instance before starting the SDK.

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="qs-gps"></a>Google Play Services

Since August 1st 2014, apps in the Google Play Store must use the [Google Advertising ID][google_ad_id] to uniquely identify devices. To allow the Adjust SDK to use the Google Advertising ID, integrate [Google Play Services][google_play_services]. To do this, copy the `google-play-services_lib` folder (part of the Android SDK) into the `Assets/Plugins/Android` folder of your Unity project.

There are two main ways to download the Android SDK. Any tool using the `Android SDK Manager` will offer a quick link to downlaod and install the Android SDK tools. Once installed, you can find the libraries in the `SDK_FOLDER/extras/google/google_play_services/libproject/` folder.

![][android_sdk_location]

If you aren't using any tools with the Android SDK Manager, download the official standalone [Android SDK][android_sdk_download]. Next, download the Andoird SDK Tools by following the instructions in the `SDK Readme.txt` README provided by Google, located in the Android SDK folder.

**Update**: With the latest Android SDK version, Google has changed the structure of the Google Play Services folders inside of the root SDK folder. It now looks like this:

![][android_sdk_location_new]

You can now add only the part of the Google Play Services library that the Adjust SDK needs––the basement. To do this, add the `play-services-basement-x.y.z.aar` file to your `Assets/Plugins/Android` folder. 

With Google Play Services library 15.0.0, Google has moved the classes needed to get the Google advertising ID into a  `play-services-ads-identifier` package. Add this package to your app if you are using library version 15.0.0 or later. When you’re finished, please test to make sure the Adjust SDK correctly obtains the Google advertising ID; we have noticed some inconsistencies, depending upon which Unity integrated development environment (IDE) version you use. 

#### Testing for the Google advertising ID

To check whether the Adjust SDK is receiving the Google advertising ID, start your app by configuring the SDK to run in `sandbox` mode and set the log level to `verbose`. After that, track a session or an event in the app and check the list of parameters recorded in the verbose logs. If you see the `gps_adid` parameter, our SDK has successfully read the Google advertising ID.

If you encounter any issues getting the Google advertising ID, please open an issue in our Github repository or contact support@adjust.com.

### <a id="qs-android-proguard"></a>Proguard settings

If you are using Proguard, add these lines to your Proguard file:

```
-keep public class com.adjust.sdk.** { *; }
-keep class com.google.android.gms.common.ConnectionResult {
    int SUCCESS;
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient {
    com.google.android.gms.ads.identifier.AdvertisingIdClient$Info getAdvertisingIdInfo(android.content.Context);
}
-keep class com.google.android.gms.ads.identifier.AdvertisingIdClient$Info {
    java.lang.String getId();
    boolean isLimitAdTrackingEnabled();
}
-keep public class com.android.installreferrer.** { *; }
```

### <a id="qs-install-referrer"></a>Google Install Referrer

In order to attribute the install of an Android app, Adjust needs information about the Google install referrer. You can set up your app to get this by using the **Google Play Referrer API** or by catching the **Google Play Store intent** with a broadcast receiver. 

Google introduced the Google Play Referrer API in order to provide a more reliable and secure way than the Google Play Store intent to obtain install referrer information and to help attribution providers fight click injections. The Google Play Store intent will exist in parallel with the API temporarily, but is set to be deprecated in the future. We encourage you to support this. 

The Adjust post-build process catches the Google Play Store intent; you can take a few additional steps to add support for the new Google Play Referrer API.

To add support for the Google Play Referrer API, download the [install referrer library][install-referrer-aar] from Maven repository and place the AAR file into your `Plugins/Android` folder.

### <a id="qs-post-build-process"></a>Post-build process

To complete the app build process, the Adjust Unity package performs custom post-build actions to ensure the Adjust SDK can work properly inside the app. 

This process is performed by the `OnPostprocessBuild` method in `AdjustEditor.cs`. Log output messages are written to the Unity IDE console output window.

#### <a id="qs-post-build-ios"></a>iOS post-build process

To execute the iOS post-build process properly, use Unity 5 or later and have `iOS build support` installed. The iOS post-build process makes the following changes to your generated Xcode project:

- Adds the `iAd.framework` (needed for Apple Search Ads tracking)
- Adds the `AdSupport.framework` (needed for reading IDFA)
- Adds the `CoreTelephony.framework` (needed for reading MMC and MNC)
- Adds the other linker flag `-ObjC` (needed to recognize Adjust Objective-C categories during build time)
- Enables `Objective-C exceptions`

#### <a id="qs-post-build-android"></a>Android post-build process

The Android post-build process makes changes to the `AndroidManifest.xml` file located in `Assets/Plugins/Android/`. It also checks for the presence of the `AndroidManifest.xml` file in the Android plugins folder. If the file is not there, it creates a copy from our compatible manifest file `AdjustAndroidManifest.xml`. If there is already an `AndroidManifest.xml` file, it makes the following changes:

- Adds the `INTERNET` permission (needed for Internet connection)
- Adds the `ACCESS_WIFI_STATE` permission (needed if you are not distributing your app via the Play Store)
- Adds the `ACCESS_NETWORK_STATE` permission (needed for reading the MMC and MNC)
- Adds the `BIND_GET_INSTALL_REFERRER_SERVICE` permission (needed for the new Google install referrer API to work)
- Adds the Adjust broadcast receiver (needed for getting install referrer information via Google Play Store intent). For more details, consult the official [Android SDK README][android]. 

**Note:** If you are using your own broadcast receiver to handle the `INSTALL_REFERRER` intent, you don't need to add the Adjust broadcast receiver to your manifest file. Remove it, but add the call to the Adjust broadcast receiver inside your own receiver, as described in the [Android guide][android-custom-receiver].

### <a id="qs-sdk-signature"></a>SDK signature

An account manager can activate the Adjust SDK signature for you. Contact Adjust support at support@adjust.com if you want to use this feature.

If the SDK signature is enabled on your account and you have access to App Secrets in your dashboard, add all secret parameters (`secretId`, `info1`, `info2`, `info3`, `info4`) to the `setAppSecret` method of `AdjustConfig` instance:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

The SDK signature is now integrated in your app. 


## Deeplinking

### <a id="dl"></a>Deeplinking Overview

**We support deeplinking on iOS and Android platforms.**

If you are using Adjust tracker URLs with deeplinking enabled, it is possible to receive information about the deeplink URL and its content. Users may interact with the URL regardless of whether they have your app installed on their device (standard deeplinking) or not (deferred deeplinking). 

With standard deeplinking, the Android platform lets you receive deeplink content; however, Android does not automatically support deferred deeplinking. To access deferred deeplink content, you can use the Adjust SDK.

Set up deeplink handling in your app on a **native level** within your generated Xcode project (for iOS) and Android Studio / Eclipse project (for Android).

### <a id="dl-standard"></a>Standard deeplinking

Information about standard deeplinks cannot be delivered to you in Unity C# code. Once you enable your app to handle deeplinking, you’ll get information about the deeplink on a native level. For more information, here’s how to enable deeplinking for [Android](#dl-app-android) and [iOS](#dl-app-ios) apps.

### <a id="dl-deferred"></a>Deferred deeplinking

In order to get content information about the deferred deeplink, set a callback method on the `AdjustConfig` object. This will receive one `string` parameter where the content of the URL is delivered. Set this method on the config object by calling the method `setDeferredDeeplinkDelegate`:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

<a id="deeplinking-deferred-open"></a>With deferred deeplinking, there is an additional setting you can set on the `AdjustConfig` object. Once the Adjust SDK gets the deferred deeplink information, you can choose whether our SDK should open the URL or not. You can set this option by calling the `setLaunchDeferredDeeplink` method on the config object:

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log ("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setLaunchDeferredDeeplink(true);
adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

If nothing is set, **the Adjust SDK will always try to launch the URL by default**.

To enable your apps to support deeplinking, set up schemes for each supported platform.

### <a id="dl-app-android"></a>Deeplink handling in Android apps

To set up deeplink handling in an Android app on a native level, follow the instructions in our official [Android SDK README][android-deeplinking].

This should be done in native Android Studio / Eclipse project.

### <a id="dl-app-ios"></a>Deeplink handling in iOS apps

**This should be done in native Xcode project.**

To set up deeplink handling in an iOS app on a nativel level, please use a native Xcode project and follow the instructions in our official [iOS SDK README][ios-deeplinking].

## Event tracking

### <a id="et-tracking"></a>Track an event

You can use Adjust to track any event in your app. If you want to track every tap on a button, [create a new event token](https://help.adjust.com/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adjust-dashboard) in your dashboard. Let's say that the event token is `abc123`. In your button's click handler method, add the following lines to track the click:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

### <a id="et-revenue"></a>Track revenue

If your users generate revenue by engaging with advertisements or making in-app purchases, you can track this with events. For example: if one add tap is worth one Euro cent, you can track the revenue event like this:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

When you set a currency token, Adjust will automatically convert the incoming revenues using the openexchange API into a reporting revenue of your choice. [Read more about currency conversion here](http://help.adjust.com/tracking/revenue-events/currency-conversion).

If you want to track in-app purchases, please make sure to call `trackEvent` only if the purchase is finished and the item has been purchased. This is important in order to avoid tracking revenue your users did not actually generate.


### <a id="et-revenue-deduplication"></a>Revenue deduplication

Add an optional transaction ID to avoid tracking duplicated revenues. The SDK remembers the last ten transaction IDs and skips revenue events with duplicate transaction IDs. This is especially useful for tracking in-app purchases. 

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

### <a id="et-purchase-verification"></a>In-app purchase verification

Verify in-app purchases using [Adjust's Purchase Verification][unity-purchase-sdk], a server-side receipt verification tool.  

## Custom parameters

### <a id="cp"></a>Custom parameters overview

In addition to the data points the Adjust SDK collects by default, you can use the Adjust SDK to track and add as many custom values as you need (user IDs, product IDs, etc.) to the event or session. Custom parameters are only available as raw data and will **not** appear in your Adjust dashboard.

Use [callback parameters](https://help.adjust.com/en/manage-data/export-raw-data/callbacks/best-practices-callbacks) for the values you collect for your own internal use, and partner parameters for those you share with external partners. If a value (e.g. product ID) is tracked both for internal use and external partner use, we recommend using both callback and partner parameters.

### <a id="cp-event-parameters"></a>Event parameters

### <a id="cp-event-callback-parameters"></a>Event callback parameters

If you register a callback URL for events in your [dashboard], we will send a GET request to that URL whenever the event is tracked. You can also put key-value pairs in an object and pass it to the `trackEvent` method. We will then append these parameters to your callback URL.

For example, if you've registered the URL `http://www.example.com/callback`, then you would track an event like this:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

In this case we would track the event and send a request to:

```
http://www.example.com/callback?key=value&foo=bar
```

Adjust supports a variety of placeholders, for example `{idfa}` for iOS or `{gps_adid}` for Android, which can be used as parameter values.  Using this example, in the resulting callback we would replace the placeholder with the IDFA/ Google Play Services ID of the current device. Read more about [real-time callbacks](https://help.adjust.com/en/manage-data/export-raw-data/callbacks) and see our full list of [placeholders](https://partners.adjust.com/placeholders/). 

**Note:** We don't store any of your custom parameters. We only append them to your callbacks. If you haven't registered a callback for an event, we will not read these parameters.


### <a id="cp-event-partner-parameters"></a>Event partner parameters

Once your parameters are activated in the dashboard, you can send them to your network partners. Read more about [module partners](https://docs.adjust.com/en/special-partners/) and their extended integration.

This works the same way as callback parameters; add them by calling the `addPartnerParameter` method on your `AdjustEvent` instance.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

You can read more about special partners and these integrations in our [guide to special partners][special-partners].

### <a id="cp-event-callback-id"></a>Event callback identifier

You can add custom string identifiers to each event you want to track. We report this identifier in your event callbacks, letting you know which event was successfully tracked. Set the identifier by calling the `setCallbackId` method on your `AdjustEvent` instance:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="cp-session-parameters"></a>Session parameters

Session parameters are saved locally and sent with every Adjust SDK **event and session**. Whenever you add these parameters, we save them (so you don't need to add them again). Adding the same parameter twice will have no effect.

It's possible to send session parameters before the Adjust SDK has launched. Using the [SDK delay](#cp-delay-start), you can therefore retrieve additional values (for instance, an authentication token from the app's server), so that all information can be sent at once with the SDK's initialization. 

### <a id="cp-session-callback-parameters"></a>Session callback parameters

You can save event callback parameters to be sent with every Adjust SDK session.

The session callback parameters' interface is similar to the one for event callback parameters. Instead of adding the key and its value to an event, add them via a call to the `addSessionCallbackParameter` method of the `Adjust` instance:

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

Session callback parameters merge with event callback parameters, sending all of the information as one, but event callback parameters take precedence over session callback parameters. If you add an event callback parameter with the same key as a session callback parameter, we will show the event value.

You can remove a specific session callback parameter by passing the desired key to the `removeSessionCallbackParameter` method of the `Adjust` instance.

```cs
Adjust.removeSessionCallbackParameter("foo");
```

To remove all keys and their corresponding values from the session callback parameters, you can reset them with the `resetSessionCallbackParameters` method of the `Adjust` instance.

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>Session partner parameters

In the same way that [session callback parameters](#cp-session-callback-parameters) are sent with every event or session that triggers our SDK, there are also session partner parameters.

These are transmitted to network partners for all of the integrations activated in your [dashboard].

The session partner parameters interface is similar to the event partner parameters interface, however instead of adding the key and its value to an event, add it by calling the `addSessionPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

Session partner parameters merge with event partner parameters. However, event partner parameters take precedence over session partner parameters. If you add an event partner parameter with the same key as a session partner parameter, we will show the event value.

To remove a specific session partner parameter, pass the desired key to the `removeSessionPartnerParameter` method of the `Adjust` instance.

```cs
Adjust.removeSessionPartnerParameter("foo");
```

To remove all keys and their corresponding values from the session partner parameters, reset it with the `resetSessionPartnerParameters` method of the `Adjust` instance.

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="cp-delay-start"></a>Delay start

Delaying the start of the Adjust SDK gives your app time to receive any session parameters (such as unique identifiers) you may want to send on install.

Set the initial delay time in seconds with the method `setDelayStart` in the `AdjustConfig` instance:

```cs
adjustConfig.setDelayStart(5.5);
```

In this example, the Adjust SDK is prevented from sending the initial install session and any new event for 5.5 seconds. After 5.5 seconds (or if you call `Adjust.sendFirstPackages()` during that time), every session parameter is added to the delayed install session and events, and the Adjust SDK will work as usual.

You can delay the start time of the Adjust SDK for a maximum of 10 seconds.

## Additional features

Once you integrate the Adjust SDK into your project, you can take advantage of the following features:

### <a id="ad-push-token"></a>Push token (uninstall tracking)

Push tokens are used for Audience Builder and client callbacks; they are also required for uninstall and reinstall tracking.

To send us a push notification token, call the `setDeviceToken` method on the `Adjust` instance when you obtain your app's push notification token (or whenever its value changes):

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>Attribution callback

You can set up a callback to be notified about attribution changes. We consider a variety of different sources for attribution, so we provide this information asynchronously. Make sure to consider [applicable attribution data policies][attribution_data] before sharing any of your data with third-parties. 

Follow these steps to add the optional callback in your application:

1. Create a method with the signature of the delegate `Action<AdjustAttribution>`.

2. After creating the `AdjustConfig` object, call the `adjustConfig.setAttributionChangedDelegate` with the previously created method. You can also use a lambda with the same signature.

3. If instead of using the `Adjust.prefab` the `Adjust.cs` script was added to another `GameObject`, be sure to pass the name of the `GameObject` as the second parameter of `AdjustConfig.setAttributionChangedDelegate`.

Because the callback is configured using the `AdjustConfig` instance, call `adjustConfig.setAttributionChangedDelegate` before calling `Adjust.start`.

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

The callback function will be called when the SDK receives final attribution data. Within the callback function you have access to the `attribution` parameter. Here is a quick summary of its properties:

- `string trackerToken` the tracker token of the current attribution
- `string trackerName` the tracker name of the current attribution
- `string network` the network grouping level of the current attribution
- `string campaign` the campaign grouping level of the current attribution
- `string adgroup` the ad group grouping level of the current attribution
- `string creative` the creative grouping level of the current attribution
- `string clickLabel` the click label of the current attribution
- `string adid` the Adjust device identifier

### <a id="ad-ad-revenue"></a>Ad revenue tracking

You can track ad revenue information with the Adjust SDK by using the following method:

```csharp
Adjust.trackAdRevenue(source, payload);
```

The method parameters you need to pass are:

- `source` - `string` object which indicates the source of ad revenue info.
- `payload` - `string` object which contains ad revenue JSON in string form.

Currently we support the following `source` parameter values:

- `AdjustConfig.AdjustAdRevenueSourceMopub` - represents the [MoPub mediation platform][sdk2sdk-mopub]

### <a id="ad-session-event-callbacks"></a>Session and event callbacks

You can set up callbacks to notify you of successful and failed events and/or sessions.

Follow these steps to add the callback function for successfully tracked events:

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

Add the following callback function for failed tracked events:

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

For successfully tracked sessions:

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

For failed tracked sessions:

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

Callback functions will be called after the SDK tries to send a package to the server. Within the callback you have access to a response data object specifically for the callback. Here is a quick summary of the session response data properties:

- `string Message` the message from the server or the error logged by the SDK
- `string Timestamp` timestamp from the server
- `string Adid` a unique device identifier provided by Adjust
- `Dictionary<string, object> JsonResponse` the JSON object with the response from the server

Both event response data objects contain:

- `string EventToken` the event token, if the package tracked was an event
- `string CallbackId` the custom defined callback ID set on an event object

Both event and session failed objects also contain:

- `bool WillRetry` indicates there will be an attempt to resend the package at a later time

### <a id="ad-user-attribution"></a>User attribution

This callback, like an attribution callback, is triggered whenever the attribution information changes. Access your user's current attribution information whenever you need it by calling the following method of the `Adjust` instance:

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**Note**: Current attribution information is available after our backend tracks the app install and triggers the attribution callback. It is not possible to access a user's attribution value before the SDK has been initialized and the attribution callback has been triggered.

### <a id="ad-device-ids"></a>Device IDs

The Adjust SDK lets you receive device identifiers.

### <a id="ad-idfa">iOS Advertising Identifier

To obtain the IDFA, call the function `getIdfa` of the `Adjust` instance:

```cs
string idfa = Adjust.getIdfa();
```

### <a id="ad-gps-adid"></a>Google Play Services advertising identifier

The Google advertising ID can only be read in a background thread. If you call the method `getGoogleAdId` of the `Adjust` instance with an `Action<string>` delegate, it will work in any situation:

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

You will now have access to the Google advertising ID as the variable `googleAdId`.

### <a id="ad-amazon-adid"></a>Amazon advertising identifier

If you need to get the Amazon advertising ID, call the `getAmazonAdId` method on `Adjust` instance:

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="ad-adid"></a>Adjust device identifier

Our backend generates a unique Adjust device identifier (known as an `adid`) for every device that has your app installed. In order to get this identifier, call this method on `Adjust` instance:

```cs
String adid = Adjust.getAdid();
```

Information about the adid is only available after our backend tracks the app install. It is not possible to access the adid value before the SDK has been initialized and the installation of your app has been successfully tracked.

### <a id="ad-pre-installed-trackers"></a>Pre-installed trackers

To use the Adjust SDK to recognize users whose devices came with your app pre-installed, follow these steps:

1. Create a new tracker in your [dashboard].
2. Set the default tracker of your `AdjustConfig`:

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

  Replace `{TrackerToken}` with the tracker token you created in step 2. E.g. `{abc123}`
  
Although the dashboard displays a tracker URL (including `http://app.adjust.com/`), in your source code you should only enter the six or seven-character token and not the entire URL.

3. Build and run your app. You should see a line like the following in the log output:

    ```
    Default tracker: 'abc123'
    ```

### <a id="ad-offline-mode"></a>Offline mode

Offline mode suspends transmission to our servers while retaining tracked data to be sent at a later point. While the Adjust SDK is in offline mode, all information is saved in a file. Please be careful not to trigger too many events in offline mode.

Activate offline mode by calling `setOfflineMode` with the parameter `true`.

```cs
Adjust.setOfflineMode(true);
```

Deactivate offline mode by calling `setOfflineMode` with `false`. When you put the Adjust SDK back into online mode, all saved information is sent to our servers with the correct time information.

This setting is not remembered between sessions, meaning that the SDK is in online mode whenever it starts, even if the app was terminated in offline mode.

### <a id="ad-disable-tracking"></a>Disable tracking

You can disable Adjust SDK tracking by invoking the method `setEnabled` with the enabled parameter as `false`. This setting is remembered between sessions, but it can only be activated after the first session.

```cs
Adjust.setEnabled(false);
```

You can check if the Adjust SDK is currently active with the method `isEnabled`. It is always possible to activate the Adjust SDK by invoking `setEnabled` with the `enabled` parameter set to `true`.

### <a id="ad-event-buffering"></a>Event buffering

If your app makes heavy use of event tracking, you might want to delay some network requests in order to send them in one batch every minute. You can enable event buffering with your `AdjustConfig` instance:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

If nothing is set, event buffering is disabled by default.

### <a id="ad-background-tracking"></a>Background tracking

The default behaviour of the Adjust SDK is to pause sending network requests while the app is in the background. You can change this in your `AdjustConfig` instance:

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR right to be forgotten

In accordance with article 17 of the EU's General Data Protection Regulation (GDPR), you can notify Adjust when a user has exercised their right to be forgotten. Calling the following method will instruct the Adjust SDK to communicate the user's choice to be forgotten to the Adjust backend:

```cs
Adjust.gdprForgetMe();
```

Upon receiving this information, Adjust will erase the user's data and the Adjust SDK will stop tracking the user. No requests from this device will be sent to Adjust in the future.

Please note that even when testing, this decision is permanent. It is not reversible.


### <a id="ad-disable-third-party-sharing"></a>Disable third-party sharing for specific users

You can now notify Adjust when a user has exercised their right to stop sharing their data with partners for marketing purposes, but has allowed it to be shared for statistics purposes. 

Call the following method to instruct the Adjust SDK to communicate the user's choice to disable data sharing to the Adjust backend:

```csharp
Adjust.disableThirdPartySharing();
```

Upon receiving this information, Adjust will block the sharing of that specific user's data to partners and the Adjust SDK will continue to work as usual.

## Testing and troubleshooting

### <a id="tt-debug-ios"></a>Debug information in iOS

Even with the post build script it is possible that the project is not ready to run out of the box.

If needed, disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.


[dashboard]:  http://dash.adjust.com
[adjust.com]: http://adjust.com

[en-readme]:  README.md
[zh-readme]:  doc/chinese/README.md
[ja-readme]:  doc/japanese/README.md
[ko-readme]:  doc/korean/README.md

[sdk2sdk-mopub]:    doc/english/sdk-to-sdk/mopub.md

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
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/1.0/installreferrer-1.0.aar
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

## License

### <a id="license"></a>License agreement

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License").
You may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-2019 Adjust GmbH, http://www.adjust.com

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
