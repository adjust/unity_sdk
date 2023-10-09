## Summary

This is the Unity SDK of Adjust™. It supports iOS, Android, Windows Store 8.1 and Windows 10 targets. You can read more about Adjust™ at [adjust.com]. 

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
      * [Huawei Referrer API](#qs-huawei-referrer-api)
      * [Post-build process](#qs-post-build-process)
        * [iOS post-build process](#qs-post-build-ios)
        * [Android post-build process](#qs-post-build-android)
      * [SDK signature](#qs-sdk-signature)

### Deeplinking

   * [Deeplinking overview](#dl)
   * [Standard deeplinking](#dl-standard)
      * [Deeplink handling in Android apps](#dl-app-android)
      * [Deeplink handling in iOS apps](#dl-app-ios)
   * [Deferred deeplinking](#dl-deferred)
      
### Event tracking

   * [Track event](#et-tracking)
   * [Track revenue](#et-revenue)
   * [Deduplicate revenue](#et-revenue-deduplication)

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

   * [AppTrackingTransparency framework](#ad-att-framework)
      * [App-tracking authorisation wrapper](#ad-ata-wrapper)
      * [Get current authorisation status](#ad-ata-getter)
      * [Check for ATT status change](#ad-att-status-change)
   * [SKAdNetwork framework](#ad-skadn-framework)
      * [Update SKAdNetwork conversion value](#ad-skadn-update-conversion-value)
      * [Conversion value updated callback](#ad-skadn-cv-updated-callback)
   * [Push token (uninstall tracking)](#ad-push-token)
   * [Attribution callback](#ad-attribution-callback)
   * [Ad revenue tracking](#ad-ad-revenue)
   * [Subscription tracking](#ad-subscriptions)
   * [Session and event callbacks](#ad-session-event-callbacks)
   * [User attribution](#ad-user-attribution)
   * [Device IDs](#ad-device-ids)
      * [iOS advertising identifier](#ad-idfa)
      * [Google Play Services advertising identifier](#ad-gps-adid)
      * [Amazon advertising identifier](#ad-amazon-adid)
      * [Adjust device identifier](#ad-adid)
   * [Set external device ID](#set-external-device-id)
   * [Preinstalled apps](#ad-preinstalled-apps)
   * [Offline mode](#ad-offline-mode)
   * [Disable tracking](#ad-disable-tracking)
   * [Event buffering](#ad-event-buffering)
   * [Background tracking](#ad-background-tracking)
   * [GDPR right to be forgotten](#ad-gdpr-forget-me)
   * [Third-party sharing](#ad-third-party-sharing)
      * [Disable third-party sharing](#ad-disable-third-party-sharing)
      * [Enable third-party sharing](#ad-enable-third-party-sharing)
   * [Measurement consent](#ad-measurement-consent)
   * [Data residency](#ad-data-residency)
   * [COPPA compliance](#ad-coppa-compliance)
   * [Play Store Kids Apps](#ad-play-store-kids-apps)

### Testing and troubleshooting
   * [Debug information in iOS](#tt-debug-ios)

### License
  * [License agreement](#license)


## Quick start

### <a id="qs-getting-started"></a>Getting started

To integrate the Adjust SDK into your Unity project, follow these steps.

### <a id="qs-get-sdk"></a>Get the SDK

You can download the latest version from our [releases page][releases].

### <a id="qs-add-sdk"></a>Add the SDK to your project

Open your project in the Unity Editor, go to `Assets → Import Package → Custom Package` and select the downloaded Unity package file.

### <a id="qs-integrate-sdk"></a>Integrate the SDK into your app

Add the prefab from `Assets/Adjust/Adjust.prefab` to the first scene.

You can edit the Adjust script parameters in the prefab `Inspector menu` to set up varios options.

<img src="https://raw.github.com/adjust/sdks/master/Resources/unity/prefab-sdk-settings.png" width="500" height="700" />

**Note:** You can chose to initialize Adjust SDK in two different ways:

- Initialize it based on prefab settings you have set in inspector (for this to happen, you need to have `START SDK MANUALLY` option **not checked**).
- Initialize it from your app's code (for this to happen, you need to have `START SDK MANUALLY` option **checked**).

If you decide to proceed with initialization based on prefab settings in inspector, Adjust SDK will be initialized as soon as `Awake` method of the scene you have added prefab to has been invoked.

Regardless of which way you pick, in order to initialize SDK, you will need to specify app token and environment. Follow [these steps](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token) to find it in the dashboard. Depending on whether you are building your app for testing or for production, change the `Environment` setting to either 'Sandbox' or 'Production'.

**Important:** Set the value to `Sandbox` if you or someone else is testing your app. Make sure to set the `Environment` to `Production` before you publish the app. Set it back to `Sandbox` if you start testing again. Also, have in mind that by default Adjust dashboard is showing production traffic of your app, so in case you want to see traffic you generated while testing in sandbox mode, make sure to switch to sandbox traffic view within dashboard.

We use the environment setting to distinguish between real traffic and artificial traffic from test devices. Please make sure to keep your environment setting updated.

In order to initialize SDK manually, make sure to do the following:

```cs
AdjustConfig config = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
Adjust.start(config);
```

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
AdjustConfig config = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox, true);
config.setLogLevel(AdjustLogLevel.Suppress);
Adjust.start(config);
```

If your target is Windows-based and you want to see the compiled logs from our library in `Release` mode, redirect the log output to your app while testing it in `Debug` mode.

Call the method `setLogDelegate` in the `AdjustConfig` instance before starting the SDK.

```cs
// ...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
// ...
Adjust.start(adjustConfig);
```

### <a id="qs-gps"></a>Google Play Services

Since August 1st 2014, apps in the Google Play Store must use the [Google Advertising ID](https://developer.android.com/google/play-services/id.html) to uniquely identify devices. To allow the Adjust SDK to use the Google Advertising ID, make sure to add latest version of [`play-services-ads-identifier`](https://mvnrepository.com/artifact/com.google.android.gms/play-services-ads-identifier?repo=google) AAR into the `Assets/Plugins/Android` folder of your Unity project.

If you are using `Google External Dependency Manager` plugin, you can also add this dependecy by stating it inside of your `Dependencies.xml` file:

```xml
<androidPackages>
    <androidPackage spec="com.google.android.gms:play-services-ads-identifier:18.0.1" />
</androidPackages>
```

#### Testing for the Google advertising ID
  
To check whether the Adjust SDK is receiving the Google advertising ID, start your app by configuring the SDK to run in `sandbox` mode and set the log level to `verbose`. After that, track a session or an event in the app and check the list of parameters recorded in the verbose logs. If you see the `gps_adid` parameter, our SDK has successfully read the Google advertising ID.

If you encounter any issues getting the Google advertising ID, please open an issue in our Github repository or contact support@adjust.com.

### <a id="qs-android-proguard"></a>Proguard settings

If you are using Proguard, add these lines to your Proguard file:

```
-keep class com.adjust.sdk.** { *; }
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

The Adjust post-build process makes sure that SDK will be able to capture the Google Play Store intent; you need take a few additional steps to add support for the new Google Play Referrer API.

To add support for the Google Play Referrer API, download the latest [install referrer library](https://mvnrepository.com/artifact/com.android.installreferrer/installreferrer) AAR from Maven repository and place it into your `Plugins/Android` folder.

If you are using `Google External Dependency Manager` plugin, you can also add this dependecy by stating it inside of your `Dependencies.xml` file:

```xml
<androidPackages>
    <androidPackage spec="com.android.installreferrer:installreferrer:2.2" />
</androidPackages>
```

#### <a id="qs-huawei-referrer-api"></a>Huawei Referrer API

As of v4.21.1, the Adjust SDK supports install tracking on Huawei devices with Huawei App Gallery version 10.4 and higher. No additional integration steps are needed to start using the Huawei Referrer API.

### <a id="qs-post-build-process"></a>Post-build process

To complete the app build process, the Adjust Unity package performs custom post-build actions to ensure the Adjust SDK can work properly inside the app.

As of Adjust SDK v4.30.0, you can customize this process directly from inspector settings of the Adjust prefab.

![][prefab-post-build-settings]

Log output messages describing the post build process are written to the Unity IDE console output window.

#### <a id="qs-post-build-ios"></a>iOS post-build process

When it comes to iOS post-build process, you have the ability to control which native iOS frameworks you would like to see linked with your app. Each one of these frameworks offers certain functionality which Adjust SDK will take the advantage of, in case corresponding framework is linked with your app. Here is the list of frameworks you can select and short explanation why would you want to have them linked:

- `AdServices.framework`: needed for Apple Search Ads tracking (new API, available as of iOS 14.3)
- `AdSupport.framework`: needed for reading IDFA
- `AppTrackingTransparency.framework`: needed to ask for user's consent to be tracked and obtain status of that consent
- `StoreKit.framework`: needed for communication with SKAdNetwork framework

You can enter `User Tracking Description` message which will be displayed when you present tracking consent dialog to your user. More more information about this in [App-tracking authorisation wrapper](#ad-ata-wrapper) chapter.

You can also specify scheme based links as well as universal link domains associated with your iOS app. More information about this in [Deeplinking overview](#dl) chapter.

There are couple of things which iOS post-build process does by default and which you don't have control of. Those things are:

- Adding the other linker flag `-ObjC` (needed to recognize Adjust Objective-C categories during build time)
- Enabling `Objective-C exceptions`

#### <a id="qs-post-build-android"></a>Android post-build process

The Android post-build process makes changes to the `AndroidManifest.xml` file located in `Assets/Plugins/Android/`. It also checks for the presence of the `AndroidManifest.xml` file in the Android plugins folder. If the file is not there, it creates a copy from our compatible manifest file `AdjustAndroidManifest.xml`. If there you already have your own `AndroidManifest.xml` file (which should most probably be the case), you have the ability to select what actions will take place during the Android post-build process.

You can control which permissions you would like the post-build process to add to your `AndroidManifest.xml` file. Each one of these permission enables certain functionality which Adjust SDK will take the advantage of, in case corresponding permission is added to your app. Here is the list of permissions you can select and short explanation why would you want to have them linked:

- `android.permission.INTERNET`: Needed for Internet connection (must be added).
- `android.permission.ACCESS_NETWORK_STATE`: Needed for reading type of network device is connected to.
- `com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE`: Needed for the new Google install referrer API to work.
- `com.google.android.gms.permission.AD_ID`: If you are targeting Android 12 and above (API level 31), you need to add this permission to read the Google's advertising ID. For more information, see [Google's `AdvertisingIdClient.Info` documentation](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info#public-string-getid).

You can also specify scheme based links associated with your Android app. More information about this in [Deeplinking overview](#dl) chapter.

One thing which is automatically being done as part of Android post-build process is that it adds the Adjust broadcast receiver (needed for getting install referrer information via Google Play Store intent). If you are using your own broadcast receiver to handle the `INSTALL_REFERRER` intent, you don't need to add the Adjust broadcast receiver to your manifest file. Remove it, but add the call to the Adjust broadcast receiver inside your own receiver, as described in the [Android guide](https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md).

### <a id="qs-sdk-signature"></a>SDK signature

When you set up the SDK Signature, each SDK communication package is "signed". This lets Adjust’s servers easily detect and reject any install activity that is not legitimate. 

There are just a few steps involved in setting up the SDK Signature. Please contact your Technical Account Manager or support@adjust.com to get started.

## Deeplinking

### <a id="dl"></a>Deeplinking Overview

**We support deeplinking on iOS and Android platforms.**

If you are using Adjust tracker URLs with deeplinking enabled, it is possible to receive information about the deeplink URL and its content. Users may interact with the URL regardless of whether they have your app installed on their device (standard deeplinking) or not (deferred deeplinking).

### <a id="dl-standard"></a>Standard deeplinking

Standard deeplinking is scenario in which click on a specific link opens your app which is already installed on user's device.

As of Adjust SDK v4.30.0, [limited support](#why-limited-support) for deeplinking setup is bundled into iOS and Android post-build processes so that you don't need to jump to native Xcode and Android Studio projects and add that support manually.

**Important:** In case you already had deeplinking set up in your app to support Adjust reattribution via deeplinks, you don't necessarily need to perform steps described in implementation chapters below for iOS and Android platforms. In case you had deeplinking support added to your apps and would like to switch to more convenient approach which Adjust SDK v4.30.0 (or later version) is offering, then please make sure to:

- **For iOS platform:** Remove calls to `[Adjust appWillOpenUrl:url];` method inside of your app's `AppDelegate` callbacks methods. This part will be automatically done if enabled in inspector menu of Adjust SDK v4.30.0 or later.
- **For Android platform:** Remove calls to `Adjust.appWillOpenUrl(url);` method inside of your app's default `Activity` class methods only if you are using `UNITY_2019_2_OR_NEWER` version. This part will be automatically done if enabled in inspector menu of Adjust SDK v4.30.0 or later. If you are using lower Unity version, then make sure to leave native deeplinking support in your Android app project.

For more information, check how to enable deeplinking for [iOS](#dl-app-ios) and [Android](#dl-app-android) apps.

### <a id="dl-app-ios"></a>Deeplink handling in iOS apps

In order to set up deeplinking support for iOS platform, make sure to perform the following steps:

- **For scheme based links:** In the inspector, fill in `iOS URL Identifier` text field (this should usually be your iOS bundle ID) and inside of the `iOS URL Schemes` section, add all the schemes which you want your iOS app to handle. **Note:** Pay attention to tooltip which says that when you are entering schemes in the list, you should write them without `://` part at the end.
- **For universal links:** In the inspector, make sure to add each associated domain which your app handles into the `iOS Universal Links Domains` list. **Note:** Pay attention to tooltip which says that when you are entering universal links associated domains, you should write them without `applinks:` part in front of the domain (also without `https://`).

What iOS post-build process will perform under the hood will be swizzling of some of your app's default `AppDelegate` methods in order to intercept the link which has opened the app and then forward the call further up the hierarchy to your app's default `AppDelegate` callback method implementation. This implementation intercepts the links only to let Adjust SDK know about them and SDK will perform everything which is needed to potentially reattribute your users via deeplinking. SDK does not offer any way of forwarding of deeplinks into Unity layer to be picked up in some custom callback method. If you would like to see information about deeplink captured inside of the Unity layer in your app, make sure to check [Application.deepLinkActivated](https://docs.unity3d.com/ScriptReference/Application-deepLinkActivated.html) method offered by Unity. Be aware that this method is available only as of `UNITY_2019_2_OR_NEWER`. If you would want to obtain information about iOS deeplink in earlier versions of Unity, you would need to implement this mechanism on your own.

### <a id="dl-app-android"></a>Deeplink handling in Android apps

In order to set up deeplinking support for Android platform, make sure to add all the schemes you would like your app to handle into the `Android URI Schemes` list. **Note:** Pay attention to tooltip which says that when you are entering URI schemes in the list, you should write them with `://` part at the end.

Unlike iOS counter part, Android post-build process will not perform any injection of custom Unity `Activity` class in order to intercept deeplinks which have opened your Android app. Instead, Adjust SDK internally relies on above mentioned [Application.deepLinkActivated](https://docs.unity3d.com/ScriptReference/Application-deepLinkActivated.html) method to get information about deeplink directly from Unity API. SDK will automatically perform everything which is needed to potentially reattribute your users via deeplinking. And, like already mentioned above - feel free to implement this same method in order to obtain deeplink which has opened your Android app.

<a name="why-limited-support"></a>Above mentioned Android deeplinking support implementation is why it was said that support for deeplinking was limited - on Android platform this mechanism will work only on `UNITY_2019_2_OR_NEWER` versions of Unity. If you are using older version, you will need to add support for deeplinking on your own inside of your Android Studio app project. Information on how that should be done can be found in [official Android SDK README](https://github.com/adjust/android_sdk#standard-deep-linking-scenario).

### <a id="dl-deferred"></a>Deferred deeplinking

In order to get content information about the deferred deeplink, set a callback method on the `AdjustConfig` object. This will receive one `string` parameter where the content of the URL is delivered. Set this method on the config object by calling the method `setDeferredDeeplinkDelegate`:

```cs
// ...
private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log("Deeplink URL: " + deeplinkURL);
   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
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

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setLaunchDeferredDeeplink(true);
adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
Adjust.start(adjustConfig);
```

If nothing is set, **the Adjust SDK will always try to launch the URL by default**.

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

### <a id="ad-att-framework"></a>AppTrackingTransparency framework

**Note**: This feature exists only in iOS platform.

For each package sent, the Adjust backend receives one of the following four (4) states of consent for access to app-related data that can be used for tracking the user or the device:

- Authorized
- Denied
- Not Determined
- Restricted

After a device receives an authorization request to approve access to app-related data, which is used for user device tracking, the returned status will either be Authorized or Denied.

Before a device receives an authorization request for access to app-related data, which is used for tracking the user or device, the returned status will be Not Determined.

If authorization to use app tracking data is restricted, the returned status will be Restricted.

The SDK has a built-in mechanism to receive an updated status after a user responds to the pop-up dialog, in case you don't want to customize your displayed dialog pop-up. To conveniently and efficiently communicate the new state of consent to the backend, Adjust SDK offers a wrapper around the app tracking authorization method described in the following chapter, App-tracking authorization wrapper.

### <a id="ad-ata-wrapper"></a>App-tracking authorisation wrapper

**Note**: This feature exists only in iOS platform.

Adjust SDK offers the possibility to use it for requesting user authorization in accessing their app-related data. Adjust SDK has a wrapper built on top of the [requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc) method, where you can as well define the callback method to get information about a user's choice. In order for this method to work, you need to specify a text which is going to be displayed as part of the tracking request dialog to your user. This setting is located inside of your iOS app `Info.plist` file under `NSUserTrackingUsageDescription` key. In case you don't want to add specify this on your own in your Xcode project, you can check Adjust prefab settings in inspector and specify this text under `User Tracking Description`. If specified there, Adjust iOS post-build process will make sure to add this setting into your app's `Info.plist` file.

Also, with the use of this wrapper, as soon as a user responds to the pop-up dialog, it's then communicated back using your callback method. The SDK will also inform the backend of the user's choice. The `NSUInteger` value will be delivered via your callback method with the following meaning:

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`
- 1: `ATTrackingManagerAuthorizationStatusRestricted`
- 2: `ATTrackingManagerAuthorizationStatusDenied`
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`

To use this wrapper, you can call it as such:

```cs
Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
{
    switch (status)
    {
        case 0:
            // ATTrackingManagerAuthorizationStatusNotDetermined case
            break;
        case 1:
            // ATTrackingManagerAuthorizationStatusRestricted case
            break;
        case 2:
            // ATTrackingManagerAuthorizationStatusDenied case
            break;
        case 3:
            // ATTrackingManagerAuthorizationStatusAuthorized case
            break;
    }
});
```

### <a id="ad-ata-getter"></a>Get current authorisation status

**Note**: This feature exists only in iOS platform.

To get the current app tracking authorization status you can call `getAppTrackingAuthorizationStatus` method of `Adjust` class that will return one of the following possibilities:

* `0`: The user hasn't been asked yet
* `1`: The user device is restricted
* `2`: The user denied access to IDFA
* `3`: The user authorized access to IDFA
* `-1`: The status is not available

### <a id="ad-att-status-change"></a>Check for ATT status change

In cases where you are not using [Adjust app-tracking authorization wrapper](#ad-ata-wrapper), Adjust SDK will not be able to know immediately upon answering the dialog what is the new value of app-tracking status. In situations like this, if you would want Adjust SDK to read the new app-tracking status value and communicate it to our backend, make sure to make a call to this method:

```cs
Adjust.checkForNewAttStatus();
```

### <a id="ad-skadn-framework"></a>SKAdNetwork framework

**Note**: This feature exists only in iOS platform.

If you have implemented the Adjust iOS SDK v4.23.0 or above and your app is running on iOS 14 and above, the communication with SKAdNetwork will be set on by default, although you can choose to turn it off. When set on, Adjust automatically registers for SKAdNetwork attribution when the SDK is initialized. If events are set up in the Adjust dashboard to receive conversion values, the Adjust backend sends the conversion value data to the SDK. The SDK then sets the conversion value. After Adjust receives the SKAdNetwork callback data, it is then displayed in the dashboard.

In case you don't want the Adjust SDK to automatically communicate with SKAdNetwork, you can disable that by calling the following method on configuration object:

```cs
adjustConfig.deactivateSKAdNetworkHandling();
```

### <a id="ad-skadn-update-conversion-value"></a>Update SKAdNetwork conversion value

**Note**: This feature exists only in iOS platform.

You can use Adjust SDK wrapper method `updateConversionValue` to update SKAdNetwork conversion value for your user:

```cs
Adjust.updateConversionValue(6);
```

### <a id="ad-skadn-cv-updated-callback"></a>Conversion value updated callback

You can register callback to get notified each time when Adjust SDK updates conversion value for the user.

```cs
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour {
    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "callback")) {
            AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
            adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
            adjustConfig.setConversionValueUpdatedDelegate(ConversionValueUpdatedCallback);

            Adjust.start(adjustConfig);
        }
    }

    private void ConversionValueUpdatedCallback(int conversionValue)
    {
        Debug.Log("Conversion value update reported!");
        Debug.Log("Conversion value: " + conversionValue);
    }
}
```

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
- `string costType` the cost type string
- `double? costAmount` the cost amount
- `string costCurrency` the cost currency string
- `string fbInstallReferrer` the Facebook install referrer information

**Note**: The cost data - `costType`, `costAmount` & `costCurrency` are only available when configured in `AdjustConfig` by calling `setNeedsCost` method. If not configured or configured, but not being part of the attribution, these fields will have value `null`. This feature is available in SDK v4.24.0 and above.

### <a id="ad-ad-revenue"></a>Ad revenue tracking

**Note**: This ad revenue tracking API is available only in the native SDK v4.29.0 and above.

You can track ad revenue information with Adjust SDK by invoking the following method:

```objc
// initialise with AppLovin MAX source
AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue("source");
// set revenue and currency
adjustAdRevenue.setRevenue(1.00, "USD");
// optional parameters
adjustAdRevenue.setAdImpressionsCount(10);
adjustAdRevenue.setAdRevenueNetwork("network");
adjustAdRevenue.setAdRevenueUnit("unit");
adjustAdRevenue.setAdRevenuePlacement("placement");
// callback & partner parameters
adjustAdRevenue.addCallbackParameter("key", "value");
adjustAdRevenue.addPartnerParameter("key", "value");
// track ad revenue
Adjust.trackAdRevenue(adjustAdRevenue);
```

Currently we support the below `source` parameter values:

- `AdjustConfig.AdjustAdRevenueSourceAppLovinMAX` - representing AppLovin MAX platform.
- `AdjustConfig.AdjustAdRevenueSourceMopub` - representing MoPub platform.
- `AdjustConfig.AdjustAdRevenueSourceAdMob` - representing AdMob platform.
- `AdjustConfig.AdjustAdRevenueSourceIronSource` - representing IronSource platform.
- `AdjustConfig.AdjustAdRevenueSourceUnity` - representing Unity platform.
- `AdjustConfig.AdjustAdRevenueSourceHeliumChartboost` - representing Helium Chartboost platform.

**Note**: Additional documentation which explains detailed integration with every of the supported sources will be provided outside of this README. Also, in order to use this feature, additional setup is needed for your app in Adjust dashboard, so make sure to get in touch with our support team to make sure that everything is set up correctly before you start to use this feature.

### <a id="ad-subscriptions"></a>Subscription tracking

**Note**: This feature is only available in the SDK v4.22.0 and above.

You can track App Store and Play Store subscriptions and verify their validity with the Adjust SDK. After a subscription has been successfully purchased, make the following call to the Adjust SDK:

**For App Store subscription:**

```cs
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

Adjust.trackAppStoreSubscription(subscription);
```

**For Play Store subscription:**

```cs
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

Adjust.trackPlayStoreSubscription(subscription);
```

Subscription tracking parameters for App Store subscription:

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency (you need to pass [currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- receipt(you need to pass properly formatted JSON `receipt` field of your purchased object returned from Unity IAP API)
- [transactionDate](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- salesRegion (you need to pass [countryCode](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc) of the [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) object)

Subscription tracking parameters for Play Store subscription:

- [price](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [currency](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [orderId](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [signature](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [purchaseToken](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [purchaseTime](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

**Note:** Subscription tracking API offered by Adjust SDK expects all parameters to be passed as `string` values. Parameters described above are the ones which API exects you to pass to subscription object prior to tracking subscription. There are various libraries which are handling in app purchases in Unity and each one of them should return information described above in some form upon successfully completed subscription purchase. You should locate where these parameters are placed in response you are getting from library you are using for in app purchases, extract those values and pass them to Adjust API as string values.

Just like with event tracking, you can attach callback and partner parameters to the subscription object as well:

**For App Store subscription:**

```cs
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

// add callback parameters
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

Adjust.trackAppStoreSubscription(subscription);
```

**For Play Store subscription:**

```cs
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

// add callback parameters
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

Adjust.trackPlayStoreSubscription(subscription);
```

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
  
The Google Play Services Advertising Identifier (Google advertising ID) is a unique identifier for a device. Users can opt out of sharing their Google advertising ID by toggling the "Opt out of Ads Personalization" setting on their device. When a user has enabled this setting, the Adjust SDK returns a string of zeros when trying to read the Google advertising ID.
  
> **Important**: If you are targeting Android 12 and above (API level 31), you need to add the [`com.google.android.gms.AD_ID` permission](#gps-adid-permission) to your app. If you do not add this permission, you will not be able to read the Google advertising ID even if the user has not opted out of sharing their ID.

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
  
### <a id="set-external-device-id"></a>Set external device ID

> **Note** If you want to use external device IDs, please contact your Adjust representative. They will talk you through the best approach for your use case.

An external device identifier is a custom value that you can assign to a device or user. They can help you to recognize users across sessions and platforms. They can also help you to deduplicate installs by user so that a user isn't counted as multiple new installs.

You can also use an external device ID as a custom identifier for a device. This can be useful if you use these identifiers elsewhere and want to keep continuity.

Check out our [external device identifiers article](https://help.adjust.com/en/article/external-device-identifiers) for more information.

> **Note** This setting requires Adjust SDK v4.20.0 or later.

To set an external device ID, assign the identifier to the `externalDeviceId` property of your config instance. Do this before you initialize the Adjust SDK.

```cs
AdjustConfig.setExternalDeviceId("{Your-External-Device-Id}")
```

> **Important** You need to make sure this ID is **unique to the user or device** depending on your use-case. Using the same ID across different users or devices could lead to duplicated data. Talk to your Adjust representative for more information.

If you want to use the external device ID in your business analytics, you can pass it as a session callback parameter. See the section on [session callback parameters](#cp-session-parameters) for more information.

You can import existing external device IDs into Adjust. This ensures that the backend matches future data to your existing device records. If you want to do this, please contact your Adjust representative.  

### <a id="ad-preinstalled-apps"></a>Preinstalled apps

You can use the Adjust SDK to recognize users whose devices had your app preinstalled during manufacturing. Adjust offers two solutions: one which uses the system payload, and one which uses a default tracker. 

In general, we recommend using the system payload solution. However, there are certain use cases which may require the tracker. First check the available [implementation methods](https://help.adjust.com/en/article/pre-install-tracking#Implementation_methods) and your preinstall partner’s preferred method. If you are unsure which solution to implement, reach out to integration@adjust.com

#### Use the system payload

- The Content Provider, System Properties, or File System method is supported from SDK v4.23.0 and above.

- The System Installer Receiver method is supported from SDK v4.27.0 and above.

Enable the Adjust SDK to recognise preinstalled apps by calling `setPreinstallTrackingEnabled` with the parameter `true` after creating the config object:


```cs
adjustConfig.setPreinstallTrackingEnabled(true);
```

Depending upon your implmentation method, you may need to make a change to your `AndroidManifest.xml` file. Find the required code change using the table below.

<table>
<tr>
<td>
  <b>Method</b>
</td>
<td>
  <b>AndroidManifest.xml change</b>
</td>
</tr>
<tr>
<td>Content Provider</td>
<td>Add permission:</br>

```
<uses-permission android:name="com.adjust.preinstall.READ_PERMISSION"/>
```
</td>
</tr>
<tr>
<td>System Installer Receiver</td>
<td>Declare receiver:</br>

```
<receiver android:name="com.adjust.sdk.AdjustPreinstallReferrerReceiver">
    <intent-filter>
        <action android:name="com.attribution.SYSTEM_INSTALLER_REFERRER" />
    </intent-filter>
</receiver>
```
</td>
</tr>
</table>

#### Use a default tracker

- Create a new tracker in your [dashboard].
- Open your app delegate and set the default tracker of your config:

  ```cs
  adjustConfig.setDefaultTracker("{TrackerToken}");
  ```

- Replace `{TrackerToken}` with the tracker token you created in step one. Please note that the dashboard displays a tracker URL (including `http://app.adjust.com/`). In your source code, you should specify only the six or seven-character token and not the entire URL.

- Build and run your app. You should see a line like the following in your LogCat:

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
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setEventBufferingEnabled(true);
Adjust.start(adjustConfig);
```

If nothing is set, event buffering is disabled by default.

### <a id="ad-background-tracking"></a>Background tracking

The default behaviour of the Adjust SDK is to pause sending network requests while the app is in the background. You can change this in your `AdjustConfig` instance:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
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

## <a id="ad-third-party-sharing"></a>Third-party sharing for specific users

You can notify Adjust when a user disables, enables, and re-enables data sharing with third-party partners.

### <a id="ad-disable-third-party-sharing"></a>Disable third-party sharing for specific users

Call the following method to instruct the Adjust SDK to communicate the user's choice to disable data sharing to the Adjust backend:

```cs
AdjustThirdPartySharing adjustThirdPartySharing = new AdjustThirdPartySharing(false);
Adjust.trackThirdPartySharing(adjustThirdPartySharing);
```

Upon receiving this information, Adjust will block the sharing of that specific user's data to partners and the Adjust SDK will continue to work as usual.

### <a id="ad-enable-third-party-sharing">Enable or re-enable third-party sharing for specific users</a>

Call the following method to instruct the Adjust SDK to communicate the user's choice to share data or change data sharing, to the Adjust backend:

```cs
AdjustThirdPartySharing adjustThirdPartySharing = new AdjustThirdPartySharing(true);
Adjust.trackThirdPartySharing(adjustThirdPartySharing);
```

Upon receiving this information, Adjust changes sharing the specific user's data to partners. The Adjust SDK will continue to work as expected.

Call the following method to instruct the Adjust SDK to send the granular options to the Adjust backend:

```cs
AdjustThirdPartySharing adjustThirdPartySharing = new AdjustThirdPartySharing(null);
adjustThirdPartySharing.addGranularOption("PartnerA", "foo", "bar");
Adjust.trackThirdPartySharing(adjustThirdPartySharing);
```

### <a id="ad-measurement-consent"></a>Consent measurement for specific users

You can notify Adjust when a user exercises their right to change data sharing with partners for marketing purposes, but they allow data sharing for statistical purposes. 

Call the following method to instruct the Adjust SDK to communicate the user's choice to change data sharing, to the Adjust backend:

```cs
Adjust.trackMeasurementConsent(true);
```

Upon receiving this information, Adjust changes sharing the specific user's data to partners. The Adjust SDK will continue to work as expected.

### <a id="ad-data-residency"></a>Data residency

In order to enable data residency feature, make sure to make a call to `setUrlStrategy` method of the `AdjustConfig` instance with one of the following constants:

```objc
adjustConfig.setUrlStrategy(AdjustConfig.AdjustDataResidencyEU); // for EU data residency region
adjustConfig.setUrlStrategy(AdjustConfig.AdjustDataResidencyTR); // for Turkey data residency region
adjustConfig.setUrlStrategy(AdjustConfig.AdjustDataResidencyUS); // for US data residency region
```

### <a id="ad-coppa-compliance"></a>COPPA compliance

By default Adjust SDK doesn't mark app as COPPA compliant. In order to mark your app as COPPA compliant, make sure to call `setCoppaCompliantEnabled` method of `AdjustConfig` instance with boolean parameter `true`:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setCoppaCompliantEnabled(true);
Adjust.start(adjustConfig);
```

**Note:** By enabling this feature, third-party sharing will be automatically disabled for the users. If later during the app lifetime you decide not to mark app as COPPA compliant anymore, third-party sharing **will not be automatically re-enabled**. Instead, next to not marking your app as COPPA compliant anymore, you will need to explicitly re-enable third-party sharing in case you want to do that.

### <a id="ad-play-store-kids-apps"></a>Play Store Kids Apps

By default Adjust SDK doesn't mark Android app as Play Store Kids App. In order to mark your app as the app which is targetting kids in Play Store, make sure to call `setPlayStoreKidsAppEnabled` method of `AdjustConfig` instance with boolean parameter `true`:

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setPlayStoreKidsAppEnabled(true);
Adjust.start(adjustConfig);
```

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
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/2.2/installreferrer-2.2.aar
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

[prefab-sdk-settings]:        https://raw.github.com/adjust/sdks/master/Resources/unity/prefab-sdk-settings.png
[prefab-post-build-settings]: https://raw.github.com/adjust/sdks/master/Resources/unity/prefab-post-build-settings.png

## License

### <a id="license"></a>License

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-Present Adjust GmbH, http://www.adjust.com

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
