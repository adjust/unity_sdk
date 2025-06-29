### Version 5.4.1 (26th June 2025)
#### Added
- Added support for Google On-Device-Measurement. For more details about this feature, refer to the [official documentation](https://dev.adjust.com/en/sdk/unity/plugins/google-odm).
- Optimized the logic behind the `ProcessAndResolveDeeplink` method to immediately return links that have already been resolved.

#### Changed
- Updated the Adjust Signature library version to 3.47.0.

#### Native SDKs
- [iOS@v5.4.1][ios_sdk_v5.4.1]
- [Android@v5.4.1][android_sdk_v5.4.1]

---

### Version 5.4.0 (May 19th 2025)
#### Added
- Added support for configuring store information via the `AdjustStoreInfo` object. You can now specify the store name and store app ID assigning `StoreInfo` property on your `AdjustConfig` instance. This enables the SDK to record the intended app store source during initialization.

#### Native SDKs
- [iOS@v5.4.0][ios_sdk_v5.4.0]
- [Android@v5.4.0][android_sdk_v5.4.0]

---

### Version 5.3.0 (April 17th 2025)
#### Added
- Added ability to initialize the SDK for the first session in delayed mode. You can start the SDK in the delayed mode by setting the `IsFirstSessionDelayEnabled` property on your `AdjustConfig` instance to `true`. To end the delay, make sure to call `EndFirstSessionDelay` method of `Adjust` instance. For more details about this feature, refer to the [official documentation](https://dev.adjust.com/en/sdk/unity/features/first-session-delay).
- Added ability to send organic search referrer together with deep link. You can send it by setting the `Referrer` property of the `AdjustDeeplink` instance.
- Added ability to disable SDK's interaction with `AppTrackingTransparency.framework` API. You can disable it by setting the `IsAppTrackingTransparencyUsageEnabled` property on your `AdjustConfig` instance to `false`.

#### Native SDKs
- [iOS@v5.3.0][ios_sdk_v5.3.0]
- [Android@v5.3.0][android_sdk_v5.3.0]

---

### Version 5.1.3 (March 24th 2025)
#### Fixed
- Fixed null reference exceptions when migrating from pre 5.1.0 version (https://github.com/adjust/unity_sdk/issues/330).

#### Native SDKs
- [iOS@v5.1.1][ios_sdk_v5.1.1]
- [Android@v5.1.0][android_sdk_v5.1.0]

---

### Version 5.1.2 (March 6th 2025)
#### Added
- Added support for custom `CODE_SIGN_ENTITLEMENTS` file name (https://github.com/adjust/unity_sdk/pull/327).

#### Native SDKs
- [iOS@v5.1.1][ios_sdk_v5.1.1]
- [Android@v5.1.0][android_sdk_v5.1.0]

---

### Version 5.1.1 (March 5th 2025)
#### Fixed
- Added missing null checks for native `jsonResponse` fields (https://github.com/adjust/flutter_sdk/pull/160).

#### Native SDKs
- [iOS@v5.1.1][ios_sdk_v5.1.1]
- [Android@v5.1.0][android_sdk_v5.1.0]

---

### Version 5.1.0 (28th February 2025)
#### Added
- Added `JsonResponse` to `AdjustAttribution` where every key-value pair sent by the backend as part of the attribution response can be found.
- Added the ability to specify the name of custom Android activity which you might be using instead of the default `com.unity3d.player.UnityPlayerActivity` (https://github.com/adjust/unity_sdk/issues/273).

#### Changed
- Switched to adding `<intent-filter>` per specified Android URI scheme instead of adding them all as `<data>` entries to the same intent filter (https://github.com/adjust/unity_sdk/issues/266).
- Moved execution of all the C# callbacks into the main Unity thread (https://github.com/adjust/unity_sdk/issues/277, https://github.com/adjust/unity_sdk/issues/310).

#### Native SDKs
- [iOS@v5.1.1][ios_sdk_v5.1.1]
- [Android@v5.1.0][android_sdk_v5.1.0]

---

### Version 5.0.7 (4th February 2025)
#### Added
- Added sending of the additional parameter to improve troubleshooting of `SKAdNetwork` related issues.

#### Fixed
- Fixed occasional occurrences in which ATT waiting interval timer was not being started.
- Fixed occasional NPE occurrences when app was entering background (https://github.com/adjust/android_sdk/issues/630).

#### Native SDKs
- [iOS@v5.1.0][ios_sdk_v5.1.0]
- [Android@v5.1.0][android_sdk_v5.1.0]

---

### Version 5.0.6 (5th December 2024)
#### Fixed
- Cleaned up unused methods from the API.

#### Changed
- Switched to native Android SDK version that depends on a specific version of the signature library.

#### Native SDKs
- [iOS@v5.0.1][ios_sdk_v5.0.1]
- [Android@v5.0.2][android_sdk_v5.0.2]

---

### Version 5.0.5 (24th October 2024)
#### Fixed
- Tagged the version which is now having submodules pointing to public repositories allowing OpenUPM to properly clone this repository (https://github.com/adjust/unity_sdk/issues/314).
- Fixed issue introduced with Unity SDK v5.0.4 where the old name of native Android log level enum was being referenced (https://github.com/adjust/unity_sdk/issues/316).
- Fixed error being thrown because of Adjust editor script in cases where iOS Build Support Unity module was not installed (https://github.com/adjust/unity_sdk/issues/317).

#### Native SDKs
- [iOS@v5.0.1][ios_sdk_v5.0.1]
- [Android@v5.0.1][android_sdk_v5.0.1]

---

### Version 5.0.4 (18th October 2024)
#### Changed
- Updated minimal supported Unity version to `2018.2`.
- Improved support for properly linking Adjust SDK dependencies regardless of the EDM4U iOS Resolver settings. With this change, there is no longer need to uncheck the `Link frameworks statically` option from EDM4U iOS Resolver settings. This should address issues like https://github.com/adjust/unity_sdk/issues/308#issuecomment-2417099823.
- Improved handling and disposal of `AndroidJavaObject` instances (https://github.com/adjust/unity_sdk/pull/303).
- Removed `Example` directory from `.unitypackage`. Example scene is now part of the repository only. This should address this issue: https://github.com/adjust/unity_sdk/issues/311.
- Removed the `Assets` menu item logic from `AdjustEditor` script which was generating SDK's `.unitypackage`.

#### Native SDKs
- [iOS@v5.0.1][ios_sdk_v5.0.1]
- [Android@v5.0.1][android_sdk_v5.0.1]

---

### Version 5.0.3 (17th September 2024)
#### Fixed
- Fixed not functional URL strategy feature for Android platform.
- Updated to native [iOS SDK v5.0.1](https://github.com/adjust/ios_sdk/releases/tag/v5.0.1) which fixes `Adjust.modulemap not found` error in certain cases of importing of the native iOS SDK via CocoaPods.

#### Native SDKs
- [iOS@v5.0.1][ios_sdk_v5.0.1]
- [Android@v5.0.0][android_sdk_v5.0.0]

---

### Version 5.0.2 (27th August 2024)
#### Added
- Enabled obtaining of the last opened deep link (`Adjust.GetLastDeeplink`) on Android platform.

#### Fixed
- Fixed method for getting Google Play Advertising ID (https://github.com/adjust/unity_sdk/issues/305).
- Fixed ambiguous overloads errors occurring when invoking some of the asynchronous getter methods on Android platform (https://github.com/adjust/unity_sdk/issues/288).

#### Native SDKs
- [iOS@v5.0.0][ios_sdk_v5.0.0]
- [Android@v5.0.0][android_sdk_v5.0.0]

---

### Version 5.0.1 (19th August 2024)
#### Fixed
- Fixed handling of deferred deep links on iOS platform.

#### Changed
- Added attribution and deferred deep link callbacks to example app.

#### Native SDKs
- [iOS@v5.0.0][ios_sdk_v5.0.0]
- [Android@v5.0.0][android_sdk_v5.0.0]

---

### Version 5.0.0 (12th August 2024)

We're excited to release our major new SDK version (v5). Among many internal improvements, our spoofing protection solution is now included out of the box, reinforcing our commitment to accurate, actionable, and fraud-free data.

To try out SDK v5 in your app, you can follow our new v4 to v5 [migration guide](https://dev.adjust.com/en/sdk/unity/migration/v4-to-v5).

If you are a current Adjust client and have questions about SDK v5, please email [sdk-v5@adjust.com](mailto:sdk-v5@adjust.com).

In case you were using beta version of the SDK v5, please switch to the official v5 release.

#### Native SDKs
- [iOS@v5.0.0][ios_sdk_v5.0.0]
- [Android@v5.0.0][android_sdk_v5.0.0]

---

### Version 4.38.1 (30th April 2024)
#### Added
- Added sending of the additional iOS SDK observability parameters for debugging purposes.

#### Native SDKs
- [iOS@v4.38.2][ios_sdk_v4.38.2]
- [Android@v4.38.3][android_sdk_v4.38.3]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.38.0 (8th April 2024)
#### Added
- Added new domains and corresponding payload restrictions for the Adjust SDK to direct the iOS traffic to:
    - https://consent.adjust.com - for consented users
    - https://analytics.adjust.com - for non-consented users

**Note:** For the time being, Adjust SDK does not bundle Privacy Manifest by default. Until the support for that is added, make sure to add content of the [Adjust Privacy Manifest](https://github.com/adjust/ios_sdk/blob/master/Adjust/PrivacyInfo.xcprivacy) to your app Privacy Manifest file.

#### Native SDKs
- [iOS@v4.38.0][ios_sdk_v4.38.0]
- [Android@v4.38.3][android_sdk_v4.38.3]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.37.2 (21st March 2024)
#### Fixed
- Fixed occasional invalid signature cases in Android when SDK package payload would be altered right before sending.

#### Changed
- Added tracking of `third_party_sharing` and `measurement_consent` packages if invoked before SDK initialization in subsequent SDK initializations.

#### Native SDKs
- [iOS@v4.37.2][ios_sdk_v4.37.2]
- [Android@v4.38.2][android_sdk_v4.38.2]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.37.1 (19th February 2024)
#### Added
- Added support for `TradPlus` ad revenue tracking.

#### Changed
- Changed the semantics of the `AdjustAppStorePurchase`'s `receipt` property to represent UnityIAP base64 encoded receipt string to avoid the need for the base64 decoding by the clients.

#### Native SDKs
- [iOS@v4.37.1][ios_sdk_v4.37.1]
- [Android@v4.38.1][android_sdk_v4.38.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.37.0 (16th January 2024)
#### Added
- Added ability to process shortened deep links and provide the unshortened link back as a response. You can achieve this by invoking `processDeeplink` method of the `Adjust` instance.
- Added ability to inject iOS in-app purchase receipt directly as base64 encoded string when you are performing purchase event verification. You can achieve this by passing the base64 encoded string receipt via `setReceiptBase64` method of the `AdjustEvent` instance.

#### Native SDKs
- [iOS@v4.37.0][ios_sdk_v4.37.0]
- [Android@v4.38.0][android_sdk_v4.38.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.36.0 (24th November 2023)
#### Added
- Added ability to get IDFV value of the iOS device. You can achieve this by invoking `getIdfv()` method of the `Adjust` instance.
- Added support for Meta install referrer.
- Added support for Google Play Games on PC.
- Added support for `TopOn` and `AD(X)` ad revenue tracking.
- Added a new type of URL strategy called `AdjustConfig.AdjustUrlStrategyCnOnly`. This URL strategy represents `AdjustConfig.AdjustUrlStrategyCn` strategy, but without fallback domains.
- Added `setReadDeviceInfoOnceEnabled(bool)` method to `AdjustConfig` to indicate if Android device info should be read only once.

#### Native SDKs
- [iOS@v4.36.0][ios_sdk_v4.36.0]
- [Android@v4.37.0][android_sdk_v4.37.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.35.2 (9th October 2023)
#### Added
- Added sending of `event_callback_id` parameter (if set) with the event payload.

#### Native SDKs
- [iOS@v4.35.2][ios_sdk_v4.35.2]
- [Android@v4.35.1][android_sdk_v4.35.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.35.1 (2nd October 2023)
#### Fixed
- Fixed issue with signing iOS requests post ATT delay timer expiry.

#### Native SDKs
- [iOS@v4.35.1][ios_sdk_v4.35.1]
- [Android@v4.35.0][android_sdk_v4.35.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.35.0 (27th September 2023)
#### Added
- Added support for SigV3 library. Update authorization header building logic to use `adj_signing_id`.
- Added `setAndroidFinalAttributionEnabled` method to `AdjustConfig` to indicate if only final Android attribution is needed in attribution callback (by default attribution callback return intermediate attribution as well before final attribution if not enabled with this setter method).

#### Native SDKs
- [iOS@v4.35.0][ios_sdk_v4.35.0]
- [Android@v4.35.0][android_sdk_v4.35.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.34.1 (6th September 2023)
#### Added
- Added more logging around ATT delay timer feature to indicate that it's activated.

#### Fixed
- Fixed issue where subsequent calls to active state callback would make ATT delay timer elapse sooner.
- Removed unnecessary `AdjustSettings.asset` file out of `.unitypackage`.

#### Native SDKs
- [iOS@v4.34.2][ios_sdk_v4.34.2]
- [Android@v4.34.0][android_sdk_v4.34.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.34.0 (21st August 2023)
#### Added
- Added ability to delay SDK start on iOS platform in order to wait for an answer to the ATT dialog. You can set the number of seconds to wait (capped internally to 120) by calling the `setAttConsentWaitingInterval` method of the `AdjustConfig` instance.
- Added support for purchase verification. In case you are using this feature, you can now use it by calling `verifyAppStorePurchase` (for iOS) and `verifyPlayStorePurchase` (for Android) methods of the `Adjust` instance.

#### Native SDKs
- [iOS@v4.34.1][ios_sdk_v4.34.1]
- [Android@v4.34.0][android_sdk_v4.34.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.33.2 (25th July 2023)
#### Fixed
- Fixed iOS compile time errors caused by hanging references to removed `iAd.framework` handling logic (https://github.com/adjust/unity_sdk/issues/271).

#### Native SDKs
- [iOS@v4.33.6][ios_sdk_v4.33.6]
- [Android@v4.33.5][android_sdk_v4.33.5]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.33.1 (7th July 2023)
#### Added
- Added ability to read App Set ID on Android platform in case you opt in by adding the `com.google.android.gms:play-services-appset` dependency to your Android app.

#### Native SDKs
- [iOS@v4.33.4][ios_sdk_v4.33.4]
- [Android@v4.33.5][android_sdk_v4.33.5]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.33.0 (8th December 2022)
#### Added
- Added support for SKAD 4.0.
- Added support for Samsung install referrer.
- Added support to OAID plugin for MSA SDK v2.0.0.
- Added support for setting a new China URL Strategy. You can choose this setting by calling `setUrlStrategy` method of `AdjustConfig` instance with `AdjustConfig.AdjustUrlStrategyCn` parameter.

#### Fixed
- Added addition of `GCC_ENABLE_OBJC_EXCEPTIONS` to `UnityFramework` target (if present) from `AdjustEditor`.

#### Native SDKs
- [iOS@v4.33.2][ios_sdk_v4.33.2]
- [Android@v4.33.2][android_sdk_v4.33.2]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.32.2 (14th November 2022)
#### Fixed
- Fixed conversion bug which caused `transaction_date` parameter of iOS subscription tracking to be wrongly formatted.

#### Native SDKs
- [iOS@v4.32.0][ios_sdk_v4.32.0]
- [Android@v4.32.0][android_sdk_v4.32.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.32.1 (26th September 2022)
#### Fixed
- Fixed issue where signature was not being loaded for Unity IDE versions which are using `UnityFramework` Xcode target.

#### Native SDKs
- [iOS@v4.32.0][ios_sdk_v4.32.0]
- [Android@v4.32.0][android_sdk_v4.32.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.32.0 (15th September 2022)
#### Added
- Added partner sharing settings to the third party sharing feature.
- Added `getLastDeeplink` getter to `Adjust` API to be able to get last tracked deep link by the SDK for iOS platform.

#### Changed
- Switched to adding permission `com.google.android.gms.permission.AD_ID` in the Android app's mainfest by default.

#### Native SDKs
- [iOS@v4.32.0][ios_sdk_v4.32.0]
- [Android@v4.32.0][android_sdk_v4.32.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.31.0 (3rd August 2022)
#### Added
- Added support for `LinkMe` feature.
- Added support to get Facebook install referrer information in attribution callback.
- Added README instructions about how to add Android dependencies via `Google External Dependency Manager` (https://github.com/adjust/unity_sdk/issues/229).

#### Native SDKs
- [iOS@v4.31.0][ios_sdk_v4.31.0]
- [Android@v4.31.0][android_sdk_v4.31.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.30.0 (5th May 2022)
#### Added
- Added all the missing SDK configuration settings into the Adjust prefab.
- Added possibility for users to control iOS and Android post-build process directly from inspector menu of the Adjust prefab.
- Added possibility for users to set up deep linking directly from Adjust prefab inspector menu. For up to date information, please check `Deeplinking overview` chapter of the `README`.
- Added possibility for users to set up iOS tracking request dialog text directly from Adjust prefab inspector menu.
- Added ability to mark your app as COPPA compliant. You can enable this setting by calling `setCoppaCompliantEnabled` method of `AdjustConfig` instance with boolean parameter `true`.
- Added ability to mark your Android app as app for the kids in accordance to Google Play Families policies. You can enable this setting by calling `setPlayStoreKidsAppEnabled` method of `AdjustConfig` instance with boolean parameter `true`.
- Added `checkForNewAttStatus` method to `Adjust` API to allow iOS apps to instruct to SDK to check if `att_status` might have changed in the meantime.

#### Changed
- Redesigned Adjust prefab inspector menu appearance.

#### Native SDKs
- [iOS@v4.30.0][ios_sdk_v4.30.0]
- [Android@v4.30.0][android_sdk_v4.30.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

#### Kudos

Kudos to [Ivan](https://github.com/MatkovIvan) and [Evgeny](https://github.com/evgenyKozlovAkvelon) for all the contributions to this release.

---

### Version 4.29.7 (4th March 2022)
#### Fixed
- Fixed crash occurrences in scenarios where one was passing `null` as value of either key or value of callback or partner parameter on iOS platform.
- Fixed crash occurrences in scenarios where one was passing `null` as value of any of the parameters of the granular third party sharing options on iOS platform.

#### Native SDKs
- [iOS@v4.29.7][ios_sdk_v4.29.7]
- [Android@v4.29.1][android_sdk_v4.29.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.6 (9th February 2022)
#### Added
- Added support for `Unity` ad revenue tracking.
- Added support for `Helium Chartboost` ad revenue tracking.
- Added support for `MSA SDK v1.1.0` to OAID plugin.

#### Native SDKs
- [iOS@v4.29.7][ios_sdk_v4.29.7]
- [Android@v4.29.1][android_sdk_v4.29.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.5 (7th December 2021)
#### Added
- Added support for `Admost` ad revenue tracking.

#### Native SDKs
- [iOS@v4.29.6][ios_sdk_v4.29.6]
- [Android@v4.28.8][android_sdk_v4.28.8]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.4 (7th September 2021)
#### Added
- Added support for `MSA SDK v1.0.26` to OAID plugin.
- Added sending of information when was the call to `registerAppForAdNetworkAttribution` method made in iOS.

#### Native SDKs
- [iOS@v4.29.5][ios_sdk_v4.29.5]
- [Android@v4.28.4][android_sdk_v4.28.4]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.3 (27th July 2021)
#### Fixed
- Fixed missing authorization header in retry requests on Android.

#### Native SDKs
- [iOS@v4.29.3][ios_sdk_v4.29.3]
- [Android@v4.28.3][android_sdk_v4.28.3]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.2 (23rd June 2021)
#### Changed
- Added deep link URL decoding before parsing its parameters for Android platform.

#### Native SDKs
- [iOS@v4.29.3][ios_sdk_v4.29.3]
- [Android@v4.28.2][android_sdk_v4.28.2]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.1 (13th May 2021)
#### Added
- [beta] Added data residency support for US region. You can choose this setting by calling `setUrlStrategy` method of `AdjustConfig` instance with `AdjustConfig.AdjustDataResidencyUS` parameter.

#### Fixed
- Removed 5 decimal places formatting for ad revenue value.

#### Native SDKs
- [iOS@v4.29.2][ios_sdk_v4.29.2]
- [Android@v4.28.1][android_sdk_v4.28.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.29.0 (27th April 2021)
#### Added
- Added `conversionValueUpdatedDelegate` callback to `AdjustConfig` which can be used to get information when Adjust SDK updates conversion value for the user.
- [beta] Added data residency support for Turkey region. You can choose this setting by calling `setUrlStrategy` method of `AdjustConfig` instance with `AdjustConfig.AdjustDataResidencyTR` parameter.
- Added `trackAdRevenue(AdjustAdRevenue)` method to `Adjust` interface to allow tracking of ad revenue by passing `AdjustAdRevenue` object as parameter. 
- Added support for `AppLovin MAX` ad revenue tracking.

#### Changed
- Removed unused ad revenue constants.

#### Native SDKs
- [iOS@v4.29.0][ios_sdk_v4.29.0]
- [Android@v4.28.0][android_sdk_v4.28.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.28.0 (1th April 2021)
#### Changed
- Removed native iOS legacy code.

#### Native SDKs
- [iOS@v4.28.0][ios_sdk_v4.28.0]
- [Android@v4.27.0][android_sdk_v4.27.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.27.0 (29th March 2021)
#### Added
- Added data residency feature. Support for EU data residency region is added. You can choose this setting by calling `setUrlStrategy` method of `AdjustConfig` instance with `AdjustConfig.AdjustDataResidencyEU` parameter.
- Added preinstall tracking with usage of system installer receiver on Android platform.
- Added support for MSA SDK v1.0.25 to OAID plugin.

#### Fixed
- Fixed attribution value comparison logic which might cause same attribution value to be delivered into attribution callback on iOS platform.

#### Native SDKs
- [iOS@v4.27.1][ios_sdk_v4.27.1]
- [Android@v4.27.0][android_sdk_v4.27.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.26.1 (12th February 2021)
#### Fixed
- Fixed ambiguous API invocation error in certain Unity IDE setups.

#### Native SDKs
- [iOS@v4.26.2][ios_sdk_v4.26.2]
- [Android@v4.26.1][android_sdk_v4.26.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.26.0 (11th February 2021)
#### Added
- Added support for Apple Search Ads attribution with usage of `AdServices.framework`.
- Added `setAllowAdServicesInfoReading` method to `AdjustConfig` to allow option for users to prevent SDK from performing any tasks related to Apple Search Ads attribution with usage of `AdServices.framework`.
- Added wrapper method `updateConversionValue` method to `Adjust` to allow updating SKAdNetwork conversion value via SDK API.
- Added `getAppTrackingAuthorizationStatus` getter to `Adjust` instance to be able to get current iOS app tracking status.
- Added improved measurement consent management and third party sharing mechanism.
- Added public constants to be used as sources for ad revenue tracking with `trackAdRevenue` method.

#### Fixed
- Fixed hardcoded scene name under the hood of `requestTrackingAuthorizationWithCompletionHandler` method which caused that game object was always expected to be named "Adjust".

#### Native SDKs
- [iOS@v4.26.2][ios_sdk_v4.26.2]
- [Android@v4.26.1][android_sdk_v4.26.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.24.1 (24th December 2020)
#### Fixed
- Fixed handling of native `null` values for cost amount.

#### Native SDKs
- [iOS@v4.24.0][ios_sdk_v4.24.0]
- [Android@v4.25.0][android_sdk_v4.25.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.24.0 (11th December 2020)
#### Added
- Added possibility to get cost data information in attribution callback.
- Added `setNeedsCost` method to `AdjustConfig` to indicate if cost data is needed in attribution callback (by default cost data will not be part of attribution callback if not enabled with this setter method).
- Added `setPreinstallTrackingEnabled` method to `AdjustConfig` to allow enabling of preintall tracking (this feature is OFF by default).
- Added support for MSA SDK v1.0.23 in OAID plugin.

#### Native SDKs
- [iOS@v4.24.0][ios_sdk_v4.24.0]
- [Android@v4.25.0][android_sdk_v4.25.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.23.2 (2nd October 2020)
#### Added
- Added note to `Assets/Adjust` menu toggling actions that project needs to be saved in order for actions to take effect.

#### Changed
- Switched to usage of relative paths when working with `AdjustSettings.asset` file.

#### Native SDKs
- [iOS@v4.23.2][ios_sdk_v4.23.2]
- [Android@v4.24.1][android_sdk_v4.24.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.23.1 (29th September 2020)
#### Fixed
- Fixed duplicate `ADJUrlStrategy` symbol error.

#### Changed
- Switched to usage of `AssetDatabase` instead of `EditorPrefs` to store relevant information from `AdjustEditor`.

#### Native SDKs
- [iOS@v4.23.2][ios_sdk_v4.23.2]
- [Android@v4.24.1][android_sdk_v4.24.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.23.0 (21st August 2020)
#### Added
- Added communication with SKAdNetwork framework by default on iOS 14.
- Added method `deactivateSKAdNetworkHandling` method to `AdjustConfig` to switch off default communication with SKAdNetwork framework in iOS 14.
- Added wrapper method `requestTrackingAuthorizationWithCompletionHandler` to `Adjust` to allow asking for user's consent to be tracked in iOS 14 and immediate propagation of user's choice to backend.
- Added handling of new iAd framework error codes introduced in iOS 14.
- Added sending of value of user's consent to be tracked with each package.
- Added `setUrlStrategy` method to `AdjustConfig` class to allow selection of URL strategy for specific market.
- Added new entries to `Assets/Adjust` menu (`Assets/Adjust/Is iOS 14 Support Enabled` and `Assets/Adjust/Toggle iOS 14 Support`) to enable / disable iOS 14 support when building iOS project. If enabled, resulting Xcode project will get `StoreKit.framework` and `AppTrackingTransparency.framework` linked to it.

⚠️ **Note**: iOS 14 beta versions prior to 5 appear to have an issue when trying to use iAd framework API like described in [here](https://github.com/adjust/ios_sdk/issues/452). For testing of v4.23.0 version of SDK in iOS, please make sure you're using **iOS 14 beta 5 or later**.

#### Native SDKs
- [iOS@v4.23.0][ios_sdk_v4.23.0]
- [Android@v4.24.0][android_sdk_v4.24.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.22.1 (5th June 2020)
#### Fixed
- Fixed `copyWithZone:` method implementation in `ADJSubscription.m` (native iOS SDK update).

#### Native SDKs
- [iOS@v4.22.1][ios_sdk_v4.22.1]
- [Android@v4.22.0][android_sdk_v4.22.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.22.0 (5th June 2020)
#### Added
- Added subscription tracking feature.

#### Native SDKs
- [iOS@v4.22.0][ios_sdk_v4.22.0]
- [Android@v4.22.0][android_sdk_v4.22.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.21.3 (4th May 2020)
#### Fixed
- Removed iAd timer from iOS native SDK.

#### Native SDKs
- [iOS@v4.21.3][ios_sdk_v4.21.3]
- [Android@v4.21.1][android_sdk_v4.21.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.21.2 (15th April 2020)
#### Fixed
- Added check for iOS timer source and block existence prior to starting it.

#### Native SDKs
- [iOS@v4.21.2][ios_sdk_v4.21.2]
- [Android@v4.21.1][android_sdk_v4.21.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.21.1 (9th April 2020)
#### Added
- Added support for Huawei App Gallery install referrer.

#### Changed
- Updated communication flow with `iAd.framework`.

#### Native SDKs
- [iOS@v4.21.1][ios_sdk_v4.21.1]
- [Android@v4.21.1][android_sdk_v4.21.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.21.0 (31st March 2020)
#### Added
- Added support for signature library as a plugin.
- Added more aggressive sending retry logic for install session package.
- Added additional parameters to `ad_revenue` package payload.

#### Changed
- Built this SDK version with `Unity 2017.4.1`.

#### Native SDKs
- [iOS@v4.21.0][ios_sdk_v4.21.0]
- [Android@v4.21.0][android_sdk_v4.21.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.20.1 (21st February 2020)
#### Fixed
- Fixed usage of deprecated `TargetGuidByName` API as of Unity 2019.3.0 (thanks to @epsmarkh).
- Fixed unnecessary saving of `AndroidManifest.xml` file whose content was not changed during post build process (thanks to @nanasi880).

#### Native SDKs
- [iOS@v4.20.0][ios_sdk_v4.20.0]
- [Android@v4.20.0][android_sdk_v4.20.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.20.0 (16th January 2020)
#### Added
- Added external device ID support.

#### Native SDKs
- [iOS@v4.20.0][ios_sdk_v4.20.0]
- [Android@v4.20.0][android_sdk_v4.20.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.19.2 (14th January 2020)
#### Added
- Added Adjust SDK to Unity Asset Store. As of this version, you can add Adjust SDK from [Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/adjust-sdk-160890) as well.

#### Changed
- Removed support for Unity 5. This version of SDK is built with `Unity 2017.1.1f1`.
- Changed plugin folder structure to make GitHub releases in sync with the ones from Unity Asset Store.

#### Native SDKs
- [iOS@v4.19.0][ios_sdk_v4.19.0]
- [Android@v4.19.1][android_sdk_v4.19.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.19.1 (3rd January 2020)
#### Fixed
- Fixed compile time error on UWP platform (thanks to @e1iman).

#### Added
- Added support to Adjust OAID plugin for reading OAID identifier with usage of MSA SDK if present in app and supported on device.

#### Native SDKs
- [iOS@v4.19.0][ios_sdk_v4.19.0]
- [Android@v4.19.1][android_sdk_v4.19.1]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.19.0 (9th December 2019)
#### Added
- Added `disableThirdPartySharing` method to `Adjust` interface to allow disabling of data sharing with third parties outside of Adjust ecosystem.

#### Native SDKs
- [iOS@v4.19.0][ios_sdk_v4.19.0]
- [Android@v4.19.0][android_sdk_v4.19.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.18.2 (7th October 2019)
#### Added
- Added `AdjustOaid` Unity plugin to enable reading of Huawei advertising identifier for apps outside of Google Play Store which need this feature.

#### Native SDKs
- [iOS@v4.18.3][ios_sdk_v4.18.3]
- [Android@v4.18.2][android_sdk_v4.18.2]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.18.1 (6th August 2019)
#### Fixed
- Fixed `Unity 2019.2.0` (and probably later versions as well) JNI bug in accessing native Android string constants causing Adjust environment not to be properly read. For anyone using `Unity 2019.2.0` or later, it is recommended to use this SDK version or newer.

#### Native SDKs
- [iOS@v4.18.0][ios_sdk_v4.18.0]
- [Android@v4.18.0][android_sdk_v4.18.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.18.0 (26th June 2019)
#### Added
- Added `trackAdRevenue` method to `Adjust` interface to allow tracking of ad revenue. With this release added support for `MoPub` ad revenue tracking.
- Added reading of Facebook anonymous ID if available on iOS platform.

#### Native SDKs
- [iOS@v4.18.0][ios_sdk_v4.18.0]
- [Android@v4.18.0][android_sdk_v4.18.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.17.2 (29th March 2019)
#### Fixed
- Fixed bug in `AdjustWindows.cs` which prevented the project to be built for Universal Windows Platform.

#### Native SDKs
- [iOS@v4.17.2][ios_sdk_v4.17.2]
- [Android@v4.17.0][android_sdk_v4.17.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.17.1 (21st March 2019)
#### Fixed
- Fixed bug in `AdjustEditor.cs` which would add multiple instance of Adjust broadcast receiver to `AndroidManifest.xml` (https://github.com/adjust/unity_sdk/issues/144).

#### Native SDKs
- [iOS@v4.17.2][ios_sdk_v4.17.2]
- [Android@v4.17.0][android_sdk_v4.17.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.17.0 (13th December 2018)
#### Added
- Added `getSdkVersion()` method to `Adjust` interface to obtain current SDK version string.
- Added `AdjustImei` Unity plugin to enable reading of IMEI and MEID identifiers for apps outside of Google Play Store which need this feature.

#### Native SDKs
- [iOS@v4.17.1][ios_sdk_v4.17.1]
- [Android@v4.17.0][android_sdk_v4.17.0]
- [Windows@v4.17.0][windows_sdk_v4.17.0]

---

### Version 4.15.0 (21st September 2018)
#### Added
- Added `setCallbackId` method on `AdjustEvent` object for users to set custom ID on event object which will later be reported in event success/failure callbacks.
- Added `CallbackId` property to `AdjustEventSuccess` class.
- Added `CallbackId` property to `AdjustEventFailure` class.

#### Fixed
- Fixed JNI issues when converting `null`-ed native Java objects to C# objects that were causing crashes on certain Unity IDE versions (more info: https://github.com/adjust/unity_sdk/issues/137)

#### Changed
- Marked `setReadMobileEquipmentIdentity` method of `AdjustConfig` object as deprecated.
- SDK will now fire attribution request each time upon session tracking finished in case it lacks attribution info.

#### Native SDKs
- [iOS@v4.15.0][ios_sdk_v4.15.0]
- [Android@v4.15.1][android_sdk_v4.15.1]
- [Windows@v4.15.0][windows_sdk_v4.15.0]

---

### Version 4.14.1 (18th June 2018)
#### Changed
- Updated the way how iOS native bridge handles push tokens from Unity interface - they are now being passed directly as strings to native iOS SDK.

#### Native SDKs
- [iOS@v4.14.1][ios_sdk_v4.14.1]
- [Android@v4.14.0][android_sdk_v4.14.0]
- [Windows@v4.14.0][windows_sdk_v4.14.0]

---

### Version 4.14.0 (8th June 2018)
#### Added
- Added deep link caching in case `appWillOpenUrl`/`AppWillOpenUrl` method is called natively before SDK is initialised.

#### Native SDKs
- [iOS@v4.14.0][ios_sdk_v4.14.0]
- [Android@v4.14.0][android_sdk_v4.14.0]
- [Windows@v4.14.0][windows_sdk_v4.14.0]

---

### Version 4.13.0 (15th May 2018)
#### Added
- Added `Adjust.gdprForgetMe()` method to enable possibility for user to be forgotten in accordance with GDPR law.

#### Native SDKs
- [iOS@v4.13.0][ios_sdk_v4.13.0]
- [Android@v4.13.0][android_sdk_v4.13.0]
- [Windows@v4.13.0][windows_sdk_v4.13.0]

---

### Version 4.12.5 (19th March 2018)
#### Changed
- Changed the way how to detect `Editor` presence from `Application.isEditor` to usage of preprocessor macros.

#### Native SDKs
- [iOS@v4.12.3][ios_sdk_v4.12.3]
- [Android@v4.12.4][android_sdk_v4.12.4]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.12.4 (9th March 2018)
#### Fixed
- Fixed JNI exception occurences upon triggering session/event failure callbacks when `Performance Reporting` is turned ON while building Android app (https://github.com/adjust/unity_sdk/issues/126).
- Changed target platform for iOS source files and static library to iOS only (https://github.com/adjust/unity_sdk/issues/128).

#### Native SDKs
- [iOS@v4.12.3][ios_sdk_v4.12.3]
- [Android@v4.12.4][android_sdk_v4.12.4]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.12.3 (17th February 2018)
#### Native changes
- https://github.com/adjust/ios_sdk/blob/master/CHANGELOG.md#version-4122-13th-february-2018

#### Fixed
- Fixed Unity and Objective-C data types alignment issues and improved support for 32-bit CPU types (https://github.com/adjust/unity_sdk/issues/123 and special thanks to @alexeSGN).

#### Native SDKs
- [iOS@v4.12.2][ios_sdk_v4.12.2]
- [Android@v4.12.1][android_sdk_v4.12.1]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.12.2 (9th February 2018)
#### Fixed
- Fixed Adjust SDK behaviour once tried to be run in `Editor` - no errors displayed anymore.
- Fixed random crashes on iOS 10.1.x devices when trying to initialise SDK.

#### Native SDKs
- [iOS@v4.12.1][ios_sdk_v4.12.1]
- [Android@v4.12.1][android_sdk_v4.12.1]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.12.1 (1st February 2018)
#### Native changes
- https://github.com/adjust/android_sdk/blob/master/CHANGELOG.md#version-4121-31st-january-2018

#### Native SDKs
- [iOS@v4.12.1][ios_sdk_v4.12.1]
- [Android@v4.12.1][android_sdk_v4.12.1]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.12.0 (26th January 2018)
#### Added
- Added `getAmazonAdId` method to `Adjust` interface.
- Added `setAppSecret` method to `AdjustConfig` interface.
- Added `setReadMobileEquipmentIdentity` method to `AdjustConfig` interface.

#### Changed
- Dropped support for Unity 4.
- Updated Windows platform to be feature wise up to date with Android and iOS.

#### Fixed
- Fixed handling of default tracker for iOS platform.

#### Native changes
- https://github.com/adjust/ios_sdk/blob/master/CHANGELOG.md#version-4120-13th-december-2017
- https://github.com/adjust/ios_sdk/blob/master/CHANGELOG.md#version-4121-13th-december-2017
- https://github.com/adjust/android_sdk/blob/master/CHANGELOG.md#version-4120-13th-december-2017
- https://github.com/adjust/windows_sdk/blob/master/CHANGELOG.md#version-4120-13th-december-2017

#### Native SDKs
- [iOS@v4.12.1][ios_sdk_v4.12.1]
- [Android@v4.12.0][android_sdk_v4.12.0]
- [Windows@v4.12.0][windows_sdk_v4.12.0]

---

### Version 4.11.4 (28th September 2017)
#### Added
- Improved iOS 11 support.

#### Changed
- Re-added support for Xcode 7.
- Removed iOS connection validity checks.

---

### Version 4.11.3 (19th May 2017)
#### Added
- Added check if `sdk_click` package response contains attribution information.
- Added sending of attributable parameters with every `sdk_click` package.

#### Changed
- Replaced `assert` level logs with `warn` level.

---

### Version 4.11.2 (6th March 2017)
#### Changed
- Removed connection validity checks.
- Refactored native networking code.
- Updated native Android SDK to version **4.11.3**.

---

### Version 4.11.1 (29th March 2017)
#### Added
- Added nullability annotations to public headers for Swift 3.0 compatibility.
- Added `BITCODE_GENERATION_MODE` to iOS framework for `Carthage` support.
- Added support for iOS 10.3.
- Added connection validity checks.
- Added sending of the app's install time.
- Added sending of the app's update time.
- Added support for Windows Phone 8.1 and Windows 10 to Unity 5 package.

#### Fixed
- Fixed not processing of `sdk_info` package type causing logs not to print proper package name once tracked.
- Fixed query string parsing.
- Fixed random occurrence of attribution request being fired before session request.
- Fixed handling of `null` being passed as currency value for iOS platform (https://github.com/adjust/unity_sdk/issues/102).
- Fixed issue of creating and destroying lots of threads on certain Android API levels (https://github.com/adjust/android_sdk/issues/265).
- Protected `Package Manager` from throwing unexpected exceptions (https://github.com/adjust/android_sdk/issues/266).

#### Changed
- Garanteed that access of `Activity Handler` to internal methods is done through it's executor.
- Updated native iOS SDK to version **4.11.3**.
- Updated native Android SDK to version **4.11.2**.

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


[ios_sdk_v3.4.0]: https://github.com/adjust/ios_sdk/tree/v3.4.0
[ios_sdk_v4.2.7]: https://github.com/adjust/ios_sdk/tree/v4.2.7
[ios_sdk_v4.4.1]: https://github.com/adjust/ios_sdk/tree/v4.4.1
[ios_sdk_v4.5.0]: https://github.com/adjust/ios_sdk/tree/v4.5.0
[ios_sdk_v4.5.4]: https://github.com/adjust/ios_sdk/tree/v4.5.4
[ios_sdk_v4.10.2]: https://github.com/adjust/ios_sdk/tree/v4.10.2
[ios_sdk_v4.10.3]: https://github.com/adjust/ios_sdk/tree/v4.10.3
[ios_sdk_v4.11.0]: https://github.com/adjust/ios_sdk/tree/v4.11.0
[ios_sdk_v4.11.3]: https://github.com/adjust/ios_sdk/tree/v4.11.3
[ios_sdk_v4.11.4]: https://github.com/adjust/ios_sdk/tree/v4.11.4
[ios_sdk_v4.11.5]: https://github.com/adjust/ios_sdk/tree/v4.11.5
[ios_sdk_v4.12.1]: https://github.com/adjust/ios_sdk/tree/v4.12.1
[ios_sdk_v4.12.2]: https://github.com/adjust/ios_sdk/tree/v4.12.2
[ios_sdk_v4.12.3]: https://github.com/adjust/ios_sdk/tree/v4.12.3
[ios_sdk_v4.13.0]: https://github.com/adjust/ios_sdk/tree/v4.13.0
[ios_sdk_v4.14.0]: https://github.com/adjust/ios_sdk/tree/v4.14.0
[ios_sdk_v4.14.1]: https://github.com/adjust/ios_sdk/tree/v4.14.1
[ios_sdk_v4.15.0]: https://github.com/adjust/ios_sdk/tree/v4.15.0
[ios_sdk_v4.17.1]: https://github.com/adjust/ios_sdk/tree/v4.17.1
[ios_sdk_v4.17.2]: https://github.com/adjust/ios_sdk/tree/v4.17.2
[ios_sdk_v4.18.0]: https://github.com/adjust/ios_sdk/tree/v4.18.0
[ios_sdk_v4.18.3]: https://github.com/adjust/ios_sdk/tree/v4.18.3
[ios_sdk_v4.19.0]: https://github.com/adjust/ios_sdk/tree/v4.19.0
[ios_sdk_v4.20.0]: https://github.com/adjust/ios_sdk/tree/v4.20.0
[ios_sdk_v4.21.0]: https://github.com/adjust/ios_sdk/tree/v4.21.0
[ios_sdk_v4.21.1]: https://github.com/adjust/ios_sdk/tree/v4.21.1
[ios_sdk_v4.21.2]: https://github.com/adjust/ios_sdk/tree/v4.21.2
[ios_sdk_v4.22.0]: https://github.com/adjust/ios_sdk/tree/v4.22.0
[ios_sdk_v4.22.1]: https://github.com/adjust/ios_sdk/tree/v4.22.1
[ios_sdk_v4.23.0]: https://github.com/adjust/ios_sdk/tree/v4.23.0
[ios_sdk_v4.23.2]: https://github.com/adjust/ios_sdk/tree/v4.23.2
[ios_sdk_v4.24.0]: https://github.com/adjust/ios_sdk/tree/v4.24.0
[ios_sdk_v4.26.2]: https://github.com/adjust/ios_sdk/tree/v4.26.2
[ios_sdk_v4.27.1]: https://github.com/adjust/ios_sdk/tree/v4.27.1
[ios_sdk_v4.28.0]: https://github.com/adjust/ios_sdk/tree/v4.28.0
[ios_sdk_v4.29.0]: https://github.com/adjust/ios_sdk/tree/v4.29.0
[ios_sdk_v4.29.2]: https://github.com/adjust/ios_sdk/tree/v4.29.2
[ios_sdk_v4.29.3]: https://github.com/adjust/ios_sdk/tree/v4.29.3
[ios_sdk_v4.29.5]: https://github.com/adjust/ios_sdk/tree/v4.29.5
[ios_sdk_v4.29.6]: https://github.com/adjust/ios_sdk/tree/v4.29.6
[ios_sdk_v4.29.7]: https://github.com/adjust/ios_sdk/tree/v4.29.7
[ios_sdk_v4.30.0]: https://github.com/adjust/ios_sdk/tree/v4.30.0
[ios_sdk_v4.31.0]: https://github.com/adjust/ios_sdk/tree/v4.31.0
[ios_sdk_v4.32.0]: https://github.com/adjust/ios_sdk/tree/v4.32.0
[ios_sdk_v4.33.2]: https://github.com/adjust/ios_sdk/tree/v4.33.2
[ios_sdk_v4.33.4]: https://github.com/adjust/ios_sdk/tree/v4.33.4
[ios_sdk_v4.34.1]: https://github.com/adjust/ios_sdk/tree/v4.34.1
[ios_sdk_v4.34.2]: https://github.com/adjust/ios_sdk/tree/v4.34.2
[ios_sdk_v4.35.0]: https://github.com/adjust/ios_sdk/tree/v4.35.0
[ios_sdk_v4.35.1]: https://github.com/adjust/ios_sdk/tree/v4.35.1
[ios_sdk_v4.35.2]: https://github.com/adjust/ios_sdk/tree/v4.35.2
[ios_sdk_v4.36.0]: https://github.com/adjust/ios_sdk/tree/v4.36.0
[ios_sdk_v4.37.0]: https://github.com/adjust/ios_sdk/tree/v4.37.0
[ios_sdk_v4.37.1]: https://github.com/adjust/ios_sdk/tree/v4.37.1
[ios_sdk_v4.37.2]: https://github.com/adjust/ios_sdk/tree/v4.37.2
[ios_sdk_v4.38.0]: https://github.com/adjust/ios_sdk/tree/v4.38.0
[ios_sdk_v4.38.2]: https://github.com/adjust/ios_sdk/tree/v4.38.2
[ios_sdk_v5.0.0]: https://github.com/adjust/ios_sdk/tree/v5.0.0
[ios_sdk_v5.0.1]: https://github.com/adjust/ios_sdk/tree/v5.0.1
[ios_sdk_v5.1.0]: https://github.com/adjust/ios_sdk/tree/v5.1.0
[ios_sdk_v5.1.1]: https://github.com/adjust/ios_sdk/tree/v5.1.1
[ios_sdk_v5.3.0]: https://github.com/adjust/ios_sdk/tree/v5.3.0
[ios_sdk_v5.4.0]: https://github.com/adjust/ios_sdk/tree/v5.4.0
[ios_sdk_v5.4.1]: https://github.com/adjust/ios_sdk/tree/v5.4.1

[android_sdk_v3.5.0]: https://github.com/adjust/android_sdk/tree/v3.5.0
[android_sdk_v4.1.0]: https://github.com/adjust/android_sdk/tree/v4.1.0
[android_sdk_v4.1.3]: https://github.com/adjust/android_sdk/tree/v4.1.3
[android_sdk_v4.2.0]: https://github.com/adjust/android_sdk/tree/v4.2.0
[android_sdk_v4.2.3]: https://github.com/adjust/android_sdk/tree/v4.2.3
[android_sdk_v4.10.2]: https://github.com/adjust/android_sdk/tree/v4.10.2
[android_sdk_v4.10.4]: https://github.com/adjust/android_sdk/tree/v4.10.4
[android_sdk_v4.11.0]: https://github.com/adjust/android_sdk/tree/v4.11.0
[android_sdk_v4.11.1]: https://github.com/adjust/android_sdk/tree/v4.11.1
[android_sdk_v4.11.3]: https://github.com/adjust/android_sdk/tree/v4.11.3
[android_sdk_v4.11.4]: https://github.com/adjust/android_sdk/tree/v4.11.4
[android_sdk_v4.12.0]: https://github.com/adjust/android_sdk/tree/v4.12.0
[android_sdk_v4.12.1]: https://github.com/adjust/android_sdk/tree/v4.12.1
[android_sdk_v4.12.4]: https://github.com/adjust/android_sdk/tree/v4.12.4
[android_sdk_v4.13.0]: https://github.com/adjust/android_sdk/tree/v4.13.0
[android_sdk_v4.14.0]: https://github.com/adjust/android_sdk/tree/v4.14.0
[android_sdk_v4.15.1]: https://github.com/adjust/android_sdk/tree/v4.15.1
[android_sdk_v4.17.0]: https://github.com/adjust/android_sdk/tree/v4.17.0
[android_sdk_v4.18.0]: https://github.com/adjust/android_sdk/tree/v4.18.0
[android_sdk_v4.18.2]: https://github.com/adjust/android_sdk/tree/v4.18.2
[android_sdk_v4.19.0]: https://github.com/adjust/android_sdk/tree/v4.19.0
[android_sdk_v4.19.1]: https://github.com/adjust/android_sdk/tree/v4.19.1
[android_sdk_v4.20.0]: https://github.com/adjust/android_sdk/tree/v4.20.0
[android_sdk_v4.21.0]: https://github.com/adjust/android_sdk/tree/v4.21.0
[android_sdk_v4.21.1]: https://github.com/adjust/android_sdk/tree/v4.21.1
[android_sdk_v4.22.0]: https://github.com/adjust/android_sdk/tree/v4.22.0
[android_sdk_v4.24.0]: https://github.com/adjust/android_sdk/tree/v4.24.0
[android_sdk_v4.24.1]: https://github.com/adjust/android_sdk/tree/v4.24.1
[android_sdk_v4.25.0]: https://github.com/adjust/android_sdk/tree/v4.25.0
[android_sdk_v4.26.1]: https://github.com/adjust/android_sdk/tree/v4.26.1
[android_sdk_v4.27.0]: https://github.com/adjust/android_sdk/tree/v4.27.0
[android_sdk_v4.28.0]: https://github.com/adjust/android_sdk/tree/v4.28.0
[android_sdk_v4.28.1]: https://github.com/adjust/android_sdk/tree/v4.28.1
[android_sdk_v4.28.2]: https://github.com/adjust/android_sdk/tree/v4.28.2
[android_sdk_v4.28.3]: https://github.com/adjust/android_sdk/tree/v4.28.3
[android_sdk_v4.28.4]: https://github.com/adjust/android_sdk/tree/v4.28.4
[android_sdk_v4.28.8]: https://github.com/adjust/android_sdk/tree/v4.28.8
[android_sdk_v4.29.1]: https://github.com/adjust/android_sdk/tree/v4.29.1
[android_sdk_v4.30.0]: https://github.com/adjust/android_sdk/tree/v4.30.0
[android_sdk_v4.31.0]: https://github.com/adjust/android_sdk/tree/v4.31.0
[android_sdk_v4.32.0]: https://github.com/adjust/android_sdk/tree/v4.32.0
[android_sdk_v4.33.2]: https://github.com/adjust/android_sdk/tree/v4.33.2
[android_sdk_v4.33.5]: https://github.com/adjust/android_sdk/tree/v4.33.5
[android_sdk_v4.34.0]: https://github.com/adjust/android_sdk/tree/v4.34.0
[android_sdk_v4.35.0]: https://github.com/adjust/android_sdk/tree/v4.35.0
[android_sdk_v4.35.1]: https://github.com/adjust/android_sdk/tree/v4.35.1
[android_sdk_v4.37.0]: https://github.com/adjust/android_sdk/tree/v4.37.0
[android_sdk_v4.38.0]: https://github.com/adjust/android_sdk/tree/v4.38.0
[android_sdk_v4.38.1]: https://github.com/adjust/android_sdk/tree/v4.38.1
[android_sdk_v4.38.2]: https://github.com/adjust/android_sdk/tree/v4.38.2
[android_sdk_v4.38.3]: https://github.com/adjust/android_sdk/tree/v4.38.3
[android_sdk_v5.0.0]: https://github.com/adjust/android_sdk/tree/v5.0.0
[android_sdk_v5.0.1]: https://github.com/adjust/android_sdk/tree/v5.0.1
[android_sdk_v5.0.2]: https://github.com/adjust/android_sdk/tree/v5.0.2
[android_sdk_v5.1.0]: https://github.com/adjust/android_sdk/tree/v5.1.0
[android_sdk_v5.3.0]: https://github.com/adjust/android_sdk/tree/v5.3.0
[android_sdk_v5.4.0]: https://github.com/adjust/android_sdk/tree/v5.4.0
[android_sdk_v5.4.1]: https://github.com/adjust/android_sdk/tree/v5.4.1

[windows_sdk_v4.12.0]: https://github.com/adjust/windows_sdk/tree/v4.12.0
[windows_sdk_v4.13.0]: https://github.com/adjust/windows_sdk/tree/v4.13.0
[windows_sdk_v4.14.0]: https://github.com/adjust/windows_sdk/tree/v4.14.0
[windows_sdk_v4.15.0]: https://github.com/adjust/windows_sdk/tree/v4.15.0
[windows_sdk_v4.17.0]: https://github.com/adjust/windows_sdk/tree/v4.17.0
