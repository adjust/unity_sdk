## Migrate your Adjust SDK for Unity3d to 4.35.1 from 3.4.4

### Migration procedure

Starting from version 4.0.0, the structure of this repository is adjusted to Unity 5. All files which are part of the
adjust SDK are now moved to the `Assets/Adjust` folder, since Unity 5 allows that native files can now be placed 
outside of `Assets/Plugins` folder. This is done so that adjust files are no longer mixed with files you may be 
keeping in `Assets/Plugins` folder.

For migration purposes, we have prepared the Adjust SDK uninstall script written in Python (`adjust_uninstall.py`).

Migration requires the following steps:

1. Copy the `adjust_uninstall.py` script to your root Unity project directory and run it. This script should
delete all adjust source files from the previous SDK version you had.

2. Depending on which Unity version you are using, import the appropriate Unity package file into your `Assets` folder.

After this, the adjust SDK should be successfully integrated into your Unity project.

### SDK initialization

We have changed how you configure and start the adjust SDK. All initial setup is now done with a new 
instance of the `AdjustConfig` object. The following steps should now be taken to configure the adjust SDK:

1. Create an instance of an `AdjustConfig` config object with the app token and environment.
2. Optionally, you can now call methods of the `AdjustConfig` object to specify available options.
3. Launch the SDK by invoking `Adjust.start` with the config object.

Here is an example of how the setup might look before and after the migration:

##### Before

```cs
Adjust.appDidLaunch("{YourAppToken}", AdjustUtil.AdjustEnvironment.Sandbox, AdjustUtil.LogLevel.Verbose, false);
```

##### After

```cs
AdjustConfig adjustConfig = new AdjustConfig ("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel (AdjustLogLevel.Verbose);

Adjust.start (adjustConfig);
```

### Event tracking

We also introduced proper event objects that are set up before they are tracked. Again, an example of how it 
might look like before and after:

##### Before

```cs
var parameters = new System.Collections.Generic.Dictionary<string, string> (2);
parameters.Add("key", "value");
parameters.Add("foo", "bar");

Adjust.trackEvent("{EventToken}", parameters);
```

##### After

```cs
AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
adjustEvent.addCallbackParameter ("key", "value");
adjustEvent.addCallbackParameter ("foo", "bar");

Adjust.trackEvent (adjustEvent);
```

### Revenue tracking

Revenues are now handled like normal events. You just set a revenue and a currency to track revenues. 
Note that it is no longer possible to track revenues without associated event tokens. You might need 
to create an additional event token in your dashboard.

*Please note* - the revenue format has been changed from a cent float to a whole currency-unit float. 
Current revenue tracking must be adjusted to whole currency units (i.e., divided by 100) in order to 
remain consistent.

##### Before

```cs
Adjust.trackRevenue(1.0, "{EventToken}");
```

##### After

```cs
AdjustEvent adjustEvent = new AdjustEvent ("{EventToken}");
adjustEvent.setRevenue (0.01, "EUR");

Adjust.trackEvent (adjustEvent);
```
