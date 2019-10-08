## OAID plugin

OAID is a new advertising ID available in devices with HMS (Huawei Mobile Service) version 2.6.2 or later. You can  use it to attribute and track Android devices in markets where Google Play Services is not available. 

The OAID plugin enables the Adjust Unity SDK to read a device’s OAID value *in addition* to the other device IDs it searches for by default. 

Before getting started, make sure you have read the official [Unity SDK README][readme] and successfully integrated the Adjust SDK into your app.

To enable the Adjust SDK to collect and track OAID, follow these steps.

### Add the OAID plugin to your app

You can download Adjust OAID plugin Unity package from our [releases page][releases] and add it to your app.

### Use the plugin

To read OAID values, call `AdjustOaid.ReadOaid()` before starting the SDK:

```cs
AdjustOaid.ReadOaid();

// ...

Adjust.start(config);
```

To stop the SDK from reading OAID values, call `AdjustOaid.DoNotReadOaid()`.

### Use the plugin as a Prefab

To use the OAID plugin as a Prefab, first find it in the `AdjustOaid/Prefab` folder. To only use the Prefab to read OAID values, make sure the `Start Manually` option is **not checked**. Then check the `Read Oaid` option to turn on OAID reading. 

With this option, the `AdjustOaid` Prefab must be loaded **before** the `Adjust` Prefab. This ensures the instruction to read the OAID precedes the SDK initialization.


[readme]:    ../../../README.md
[releases]:  https://github.com/adjust/unity_sdk/releases
