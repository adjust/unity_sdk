### Version 4.11.3 (19th May 2017)
#### Added
- **[IOS][AND]** Added check if `sdk_click` package response contains attribution information.
- **[IOS][AND]** Added sending of attributable parameters with every `sdk_click` package.

#### Changed
- **[IOS][AND]** Replaced `assert` level logs with `warn` level.

---

### Version 4.11.2 (6th March 2017)
#### Changed
- **[AND]** Removed connection validity checks.
- **[AND]** Refactored native networking code.
- **[AND]** Updated native Android SDK to version **4.11.3**.

---

### Version 4.11.1 (29th March 2017)
#### Added
- **[iOS]** Added nullability annotations to public headers for Swift 3.0 compatibility.
- **[iOS]** Added `BITCODE_GENERATION_MODE` to iOS framework for `Carthage` support.
- **[iOS]** Added support for iOS 10.3.
- **[iOS][AND]** Added connection validity checks.
- **[iOS][AND]** Added sending of the app's install time.
- **[iOS][AND]** Added sending of the app's update time.
- **[WIN]** Added support for Windows Phone 8.1 and Windows 10 to Unity 5 package.

#### Fixed
- **[iOS]** Fixed not processing of `sdk_info` package type causing logs not to print proper package name once tracked.
- **[AND]** Fixed query string parsing.
- **[iOS][AND]** Fixed random occurrence of attribution request being fired before session request.
- **[iOS]** Fixed handling of `null` being passed as currency value for iOS platform (https://github.com/adjust/unity_sdk/issues/102).
- **[AND]** Fixed issue of creating and destroying lots of threads on certain Android API levels (https://github.com/adjust/android_sdk/issues/265).
- **[AND]** Protected `Package Manager` from throwing unexpected exceptions (https://github.com/adjust/android_sdk/issues/266).

#### Changed
- **[AND]** Garanteed that access of `Activity Handler` to internal methods is done through it's executor.
- **[iOS]** Updated native iOS SDK to version **4.11.3**.
- **[AND]** Updated native Android SDK to version **4.11.2**.
- **[DOC]** Introduced `[iOS]`, `[AND]`, `[WIN]` and `[DOC]` tags to `CHANGELOG` to highlight the platform the change is referring to.

---

### Version 4.11.0 (18th January 2017)
#### Added
- Added `adid` property to the attribution callback response.
- Added `Adjust.getAdid()` method to be able to get adid value at any time after obtaining it, not only when session/event callbacks have been triggered.
- Added `Adjust.getAttribution()` method to be able to get current attribution value at any time after obtaining it, not only when attribution callback has been triggered.
- Added sending of **Amazon Fire Advertising Identifier** for Android platform.
- Added possibility to set default tracker for the app by adding `adjust_config.properties` file to the `assets` folder of your Android app. Mostly meant to be used by the `Adjust Store & Pre-install Tracker Tool` (https://github.com/adjust/android_sdk/blob/master/doc/english/pre_install_tracker_tool.md).

#### Fixed
- Now reading push token value from activity state file when sending package.
- Fixed memory leak by closing network session for iOS platform.
- Fixed `TARGET_OS_TV` pre processor check for iOS platform.
- Fixed `UNITY_EDITOR_OSX` symbol usage in `AdjustEditor.cs` for Windows platform (https://github.com/adjust/unity_sdk/issues/95).

#### Changed
- Firing attribution request as soon as install has been tracked, regardless of presence of attribution callback implementation in user's app.
- Saving iAd/AdSearch details to prevent sending duplicated `sdk_click` packages for iOS platform.
- Updated docs.
- Updated native iOS SDK to version **4.11.0**.
- Updated native Android SDK to version **4.11.0**.
- Native SDKs stability updates and improvements.

---

### Version 4.10.3 (7th December 2016)
#### Added
- Added method swizzling for iOS platform so that only implemented callbacks in Unity are getting implemented and called in the iOS.

#### Fixed
- Fixed the suppress log level settings on Android platform.
- Fixed the revenue deduplication on Android platform.
- No need anymore to have attribution callback implemented in order to get deferred deep link.

#### Changed
- Updated Native iOS SDK to version **4.10.4**.
- Updated Native Android SDK to version **4.10.3**.

---

### Version 4.10.2 (31st October 2016)
#### Fixed
- Fixed wrong click label parameter name which was causing this parameter to be empty in iOS.

---

### Version 4.10.1 (13th October 2016)
#### Changed
- Updated Native iOS SDK to version **4.10.2**.
- Updated Native Android SDK to version **4.10.2**.

#### Fixed
- Fixed bug in network communication on Android for some Android API levels from SDK v4.10.0.

---

### Version 4.10.0 (15th September 2016)
**SDK v4.10.0 should not be integrated in Android apps, since we noticed errors in network communication for some Android API levels.**

#### Added
- Added possibility to set session callback and partner parameters on `Adjust` instance with `addSessionCallbackParameter` and `addSessionPartnerParameter` methods.
- Added possibility to remove session callback and partner parameters by key on `Adjust` instance with `removeSessionCallbackParameter` and `removeSessionPartnerParameter` methods.
- Added possibility to remove all session callback and partner parameters on `Adjust` instance with `resetSessionCallbackParameters` and `resetSessionPartnerParameters` methods.
- Added new `Suppress` log level and for it new `AdjustConfig` constructor which gets `bool` indicating whether suppress log level should be supported or not.
- Added possibility to delay initialisation of the SDK while maybe waiting to obtain some session callback or partner parameters with `setDelayStart` method on `AdjustConfig` instance.
- Added possibility to set user agent manually with `setUserAgent` method on `AdjustConfig` instance.
- Added support for iOS 10.

#### Changed
- Deferred deep link info will now arrive as part of the attribution response and not as part of the answer to first session.
- Removed Python post build scripts in the adjust SDK unity package for Unity 5 IDE.
- Native SDKs stability updates and improvements.
- Updated docs.
- Updated Native iOS SDK to version **4.10.1**.
- Updated Native Android SDK to version **4.10.1**.

---

### Version 4.7.0 (1st August 2016)
#### Added
- Added `setSendInBackground` method on `AdjustConfig` object for enabling/disabling tracking while app is in background.
- Added `setLaunchDeferredDeeplink` method on `AdjustConfig` object for allowing/preventing the SDK to launch deferred deeplink.
- Added `setDeferredDeeplinkDelegate` method on `AdjustConfig` object for setting a callback to be triggered when deferred deeplink is received.

#### Changed
- Changed `AdjustEditor.cs` to use `BuildTarget` values instead of #defines.
- Updated docs.
- Updated Native iOS SDK to version **4.8.0**.
- Updated Native Android SDK to version **4.7.0**.

---

### Version 4.6.0 (13th April 2016)
#### Added
- Added `setEventSuccessDelegate` method on `AdjustConfig` object for setting a callback to be triggered if event is successfully tracked.
- Added `setEventFailureDelegate` method on `AdjustConfig` object for setting a callback to be triggered if event tracking failed.
- Added `setSessionSuccessDelegate` method on `AdjustConfig` object for setting a callback to be triggered if session is successfully tracked.
- Added `setSessionSuccessDelegate` method on `AdjustConfig` object for setting a callback to be triggered if session tracking failed.

#### Changed
- Changed `SimpleJSON` namespace from `SimpleJSON` to `com.adjust.sdk` to avoid conflicts if `SimpleJSON` is already being used in Unity3d project.
- Updated Native iOS SDK to version **4.6.0**.
- Updated Native Android SDK to version **4.6.0**.

---

### Version 4.1.3 (12th February 2016)
#### Added
- Added `Bitcode` support for iOS framework.
- Added `getIdfa` method for getting `IDFA` on iOS device.
- Added `getGoogleAdId` method for getting Google `Play Services Ad Id` on Android device.

#### Changed
- Updated Native iOS SDK to version **4.5.4**.
- Updated Native Android SDK to version **4.2.3**.

---

### Version 4.1.2 (20th January 2016)
#### Added
- Added support for iOS iAd v3.

#### Changed
- Removed MAC MD5 tracking feature for iOS platform completely.
- Changed Native iOS SDK updated to version **4.5.0**.
- Changed Native Android SDK updated to version **4.2.1**.

---

### Version 4.1.1 (23rd December 2015)
#### Added
- Added Changelog to the repository.

#### Changed
- Made Adjust prefab `Start Manually` option **TRUE** by default (uncheck it if you want Adjust.prefab to be loaded automatically with settings you set in Unity Editor).
- Made Adjust prefab `Print Attribution` option **TRUE** by default.

#### Fixed
- If `Adjust.instance` is already initialized, re-trying initialization (if you have chosen to initialize SDK automatically) will no longer happen in the `Awake` method.
- `WACK` no longer fails when validating an app package in Visual Studio.
- Users no longer face post build scripts execution issues.

---

### Version 4.1.0 (19th November 2015)
#### Added
- Added Windows 8.1 target.
- Added Windows Phone 8.1 target.
- Added Native Windows SDK version **4.0.2**.

---

### Version 4.0.4 (12th November 2015)
#### Fixed
- Handling `OnApplicationPause` not being called by Unity 5.x.x on scene load.

#### Changed
- Updated Native iOS SDK to version **4.4.1**.
- Updated Native Android SDK to version **4.1.3**.

---

### Version 4.0.3 (27th August 2015)
#### Added
- Added `.py` extension to scripts for better handling on different operating systems.
- Added `macMd5TrackingEnabled` property to AdjustConfig for iOS.
- Added `processName` property to AdjustConfig for Android.
- Exposing Android native `setReferrer` method.
- Exposing iOS native `setDeviceToken` method.
- Exposing setReceipt and `setTransactionId` methods to AdjustEvent for iOS receipt validation.

#### Changed
- Updated Native iOS SDK to version **4.2.8**.

---

### Version 4.0.2 (3rd August 2015)
#### Addded
- Added `startAutomatically` field in AdjustConfig for Android platform.

#### Changed
- Updated documentation.
- Disabled user to set SDK prefix.
- Removed `-all_load` flag from XCode other linker flags, added `-ObjC` instead.
- Updated Native iOS SDK to version **4.2.7**.
- Updated Native Android SDK to version **4.1.1**.

---

### Version 4.0.1 (30th June 2015)
#### Fixed
- Fixed Boolean handling in JNI.

---

### Version 4.0.0 (9th June 2015)
#### Added
- Added Native Android and iOS SDK functionalities from version 4.

#### Changed
- Updated Native iOS SDK to version **4.2.4**.
- Updated Native Android SDK to version **4.0.6**.
- Windows target not supported for now.

---

### Version 3.4.4 (8th January 2015)
#### Added
- Added exception for Unity editor.

#### Fixed
- Prevent SDK relaunch.

---

### Version 3.4.3 (22th December 2014)
#### Added
- Changed Android SDK target to 21 for compatibility with Unity3d version 3.6.

#### Changed
- Updated Native Android SDK to version **3.6.2**.

---

### Version 3.4.2 (14th October 2014)
#### Changed
- Updated Native iOS SDK to version **3.4.0**.
- Updated Native Android SDK to version **3.6.1**.

#### Fixed
- Fixed postbuild scripts.

---

### Version 3.4.1 (13th October 2014)
#### Added
- Added support for `PostprocessBuildPlayer_PlayGames` script.

#### Changed
- Updated Native Windows SDK to version **3.5.0**.

---

### Version 3.4.0 (28th July 2014)
#### Added
- Changed default device identifier of the Native Android SDK to `Google Play Advertising Id`.

#### Changed
- Updated Native Android SDK to version **3.5.0**.

---

### Version 3.3.0 (30th June 2014)
#### Added
- Parsing new response data fields.

#### Changed
- Renamed Util and Environment to avoid name conflicts.
- Updated Native iOS SDK to version **3.3.4**.
- Updated Native Android SDK to version **3.3.4**.
- Updated Native Windows SDK to version **3.3.1**.

---

### Version 3.2.3 (18th June 2014)
#### Changed
- Updated Native iOS SDK to version **3.3.3**.

---

### Version 3.2.2 (20th May 2014)
#### Changed
- Updated Native iOS SDK to version **3.3.1**.

---

### Version 3.2.1 (14th May 2014)
#### Added
- Added target for Windows Phone 8.
- Added target for Windows Store Apps.

---

### Version 3.2.0 (2nd May 2014)
#### Added
- SDK can now be disabled.
- Post process build can now be disabled.

#### Changed
- iOS build target location.

---

### Version 3.0.1 (4th April 2014)
#### Added
- Added build scripts for iOS and Android.

---

### Version 3.0.0 (27th March 2014)
#### Added
- Added Native Android and iOS SDK functionalities from version 3.
- Added In-App source access.

#### Changed
- Code refactoring.

---

### Version 2.1.1 (5th February 2014)
#### Changed
- Updated Native iOS SDK to version **2.2.0**.
- Updated Native Android SDK to version **2.1.6**.

---

### Version 2.1.0 (6th December 2013)
#### Added
- Initial release of the adjust SDK for Unity 3D.
- Central initialization for AppToken (Universal App Support).
