## IMEI plugin

For specific markets, IMEI and MEID can be used for attribution on Android platform. In order to use this feature, please complete the [required steps][imei-doc] within your Adjust Dashboard and then use this plugin.

This IMEI plugin respects the behavior of the native Adjust Android SDK in terms of device ID reading **while additionally** allowing the Adjust SDK to read the IMEI and MEID values of a device.

**Important:** This Adjust plugin is meant to be used only in apps that are **NOT being published to the Google Play Store**.

Before using this plugin, please make sure that you have read official [Unity SDK README][readme] and successfully completed Adjust SDK integration into your app. After that, please make sure to perform these additional steps if you want to enable Adjust SDK to collect and track IMEI identifier.

### Add IMEI plugin to your app

You can download Adjust IMEI plugin Unity package from our [releases page][releases] and add it to your app.

### IMEI plugin post build task

`AdjustImeiEditor.cs` script of IMEI plugin will run post build task in which it will check if your Android app's manifest file contains `android.permission.READ_PHONE_STATE` permission which is needed for IMEI identifier to be read. In case it doesn't contain it, it will be added.

Remember that after `Android 6.0` it might be necessary to [request app permission](https://developer.android.com/training/permissions/requesting) if the Android OS has not already been altered to avoid it.

### Use the plugin

Finally, in order to read IMEI and MEID values, you need to call `AdjustImei.ReadImei()` **before starting the SDK**:

```cs
AdjustImei.ReadImei();

// ...

Adjust.start(config);
```

You can call a method `AdjustImei.DoNotReadImei()` to stop the SDK from reading IMEI and MEID values.

You can as well use the IMEI plugin as prefab which is located under `AdjustImei/Prefab` folder. If you want to solely rely on prefab, make sure that `Start Manually` option is **not checked** and then feel free to check `Read Imei` option to enable IMEI reading. It is important to be aware that if prefab is used, `AdjustImei` prefab **must** be loaded **before** `Adjust` prefab, since instruction that IMEI should be read must precede SDK initialisation.

### Final note

**Please keep in mind** that IMEI and MEID are persistent identifiers and that it is your responsibility to ensure that the collection and processing of this personal data from your app's end-users is lawful.

[readme]:    ../../../README.md
[releases]:  https://github.com/adjust/unity_sdk/releases
[imei-doc]:  https://docs.adjust.com/en/imei-and-meid-attribution-for-android

