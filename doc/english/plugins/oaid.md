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

You can as well use the OAID plugin as prefab which is located under `AdjustOaid/Prefab` folder. If you want to solely rely on prefab, make sure that `Start Manually` option is **not checked** and then feel free to check `Read Oaid` option to enable OAID reading. It is important to be aware that if prefab is used, `AdjustOaid` prefab **must** be loaded **before** `Adjust` prefab, since instruction that OAID should be read must precede SDK initialisation.


[readme]:    ../../../README.md
[releases]:  https://github.com/adjust/unity_sdk/releases
