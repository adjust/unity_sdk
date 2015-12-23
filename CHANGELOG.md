### Version 4.1.1 (23rd December 2015)
### Added
- Changelog is now added to the repository.

#### Changed
- Adjust prefab `Start Manually` option is now **TRUE** by default (uncheck it if you want Adjust.prefab to be loaded automatically with settings you set in Unity Editor).
- Adjust prefab `Print Attribution` option is now **TRUE** by default.

#### Fixed
- If `Adjust.instance` is already initialized, re-trying to initialize it (if you have chosen to initialize SDK automatically) won't happen anymore in `Awake` method.
- `WACK` doesn't fail anymore when validating app package in Visual Studio.
- Users shouldn't face post build scripts execution issues anymore.

---

### Version 4.1.0 (19th November 2015)
#### Added
- Windows 8.1 target.
- Windows Phone 8.1 target.
- Native Windows SDK version **4.0.2**.

---

### Version 4.0.4 (12th November 2015)
#### Fixed
- Handling `OnApplicationPause` not being called by Unity 5.x.x on scene load.

#### Changed
- Native iOS SDK updated to version **4.4.1**.
- Native Android SDK updated to version **4.1.3**.

---

### Version 4.0.3 (27th August 2015)
#### Added
- `.py` extension to scripts for better handling on different operating systems.
- `macMd5TrackingEnabled` property to AdjustConfig for iOS.
- `processName` property to AdjustConfig for Android.
- Exposing Android native `setReferrer` method.
- Exposing iOS native `setDeviceToken` method.
- Exposing setReceipt and `setTransactionId` methods to AdjustEvent for iOS receipt validation.

#### Changed
- Native iOS SDK updated to version **4.2.8**.

---

### Version 4.0.2 (3rd August 2015)
#### Addded
- `startAutomatically` field in AdjustConfig for Android platform.

#### Changed
- Updating the docs.
- Disabling user to set SDK prefix.
- Removing `-all_load` flag from XCode other linker flags, adding `-ObjC` instead.
- Native iOS SDK updated to version **4.2.7**.
- Native Android SDK updated to version **4.1.1**.

---

### Version 4.0.1 (30th June 2015)
#### Fixed
- Boolean handling in JNI.

---

### Version 4.0.0 (9th June 2015)
#### Added
- Native Android and iOS SDK functionalities from version 4.

#### Changed
- Native iOS SDK updated to version **4.2.4**.
- Native Android SDK updated to version **4.0.6**.
- Windows target not supported for now.

---

### Version 3.4.4 (8th January 2015)
#### Added
- Exception for Unity editor.

#### Fixed
- Prevent SDK relaunch.

---

### Version 3.4.3 (22th December 2014)
#### Added
- Android SDK target changed to 21, to be compatible with Unity3d version 3.6.

#### Changed
- Native Android SDK updated to version **3.6.2**.

---

### Version 3.4.2 (14th October 2014)
#### Changed
- Native iOS SDK updated to version **3.4.0**.
- Native Android SDK updated to version **3.6.1**.

#### Fixed
- Postbuild scripts.

---

### Version 3.4.1 (13th October 2014)
#### Added
- Support for `PostprocessBuildPlayer_PlayGames` script.

#### Changed
- Native Windows SDK updated to version **3.5.0**.

---

### Version 3.4.0 (28th July 2014)
#### Added
- Native Android SDK now uses `Google Play Advertising Id` as default device identifier.

#### Changed
- Native Android SDK updated to version **3.5.0**.

---

### Version 3.3.0 (30th June 2014)
#### Added
- Parsing new response data fields.

#### Changed
- Renaming Util and Environment to avoid name conflicts.
- Native iOS SDK updated to version **3.3.4**.
- Native Android SDK updated to version **3.3.4**.
- Native Windows SDK updated to version **3.3.1**.

---

### Version 3.2.3 (18th June 2014)
#### Changed
- Native iOS SDK updated to version **3.3.3**.

---

### Version 3.2.2 (20th May 2014)
#### Changed
- Native iOS SDK updated to version **3.3.1**.

---

### Version 3.2.1 (14th May 2014)
#### Added
- Target for Windows Phone 8.
- Target for Windows Store Apps.

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
- Build scripts for iOS and Android.

---

### Version 3.0.0 (27th March 2014)
#### Added
- Native Android and iOS SDK functionalities from version 3.
- In-App source access.

#### Changed
- Code refactoring.

---

### Version 2.1.1 (5th February 2014)
#### Changed
- Native iOS SDK updated to version **2.2.0**.
- Native Android SDK updated to version **2.1.6**.

---

### Version 2.1.0 (6th December 2013)
#### Added
- Initial release of the adjust SDK for Unity 3D.
- Central initialization for AppToken (Universal App Support).
