## Summary

This is the [Unity](http://unity3d.com/) SDK of AdjustIo. You can read more about AdjustIo at
[adjust.io](http://adjust.io).

## Basic Installation

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Extract the
archive in a folder of your choice.

### 2. Add it to your project

Open your project in the Unity Editor and navigate to Assets, Import Package. Click on Custom Package and locate the AdjustIoPlugin_..._.unityPackage file. Don't download the AndroidManifest.xml file if you already have it in your project.

![][import]

### 3. Integrate AdjustIo into your app

1. Add the prefab Assets/AdjustIo/AdjustIo.prefab to the first scene of the app (only to the first scene!).

2. Change the app token on the AdjustIo object in your hierachy. You can find your app token on your [dashboard].

3. Change the log level, environment and buffering settings to the desired settings. For more info on these settings, go to https://github.com/adeven/adjust_android_sdk/ or https://github.com/adeven/adjust_ios_sdk/ . On Android the log level is defined in AndroidManifest.xml file. 

![][settings]

### 4. Adjust Android manifest

Ignore this step if you imported the AndroidManifest.xml file from this package.

1. Add or uncomment the ```<uses-permission>``` tags for ```INTERNET``` and ```ACCESS_WIFI_STATE``` if they aren't present already:

![][permissions]

2. Add broadcast receiver:

![][receiver]

3. Add AdjustIo settings

Still in the `AndroidManifest.xml`, add the following `meta-data` tags inside
the `application` tag.

```xml
<meta-data android:name="AdjustIoLogLevel"    android:value="info" />
```

![][metadata]

You can increase or decrease the amount of logs you see by changing the value
of `AdjustIoLogLevel` to one of the following:

- `verbose` - enable all logging
- `debug` - enable more logging
- `info` - the default
- `warn` - disable info logging
- `error` - disable warnings as well
- `assert` - disable errors as well

## Additional features

Once you integrated the AdjustIo SDK into your project, you can take advantage
of the following features.

### Add tracking of custom events.

You can tell AdjustIo about every event you want. Suppose you want to track
every tap on a button. You would have to create a new Event Token in your
[dashboard]. Let's say that Event Token is `abc123`. In your button's
click handler method you could then add the following line to track the click:

```actionscript
AdjustIo.TrackEvent("abc123");
```

You can also register a callback URL for that event in your [dashboard] and we
will send a GET request to that URL whenever the event gets tracked. In that
case you can also put some key-value-pairs in an object and pass it to the
`trackEvent` method. We will then append these named parameters to your
callback URL.

For example, suppose you have registered the URL
`http://www.adeven.com/callback` for your event with Event Token `abc123` and
execute the following lines:

<pre><code>
Dictionary<string,string> parameters = new Dictionary<string, string>();
parameters.Add("key","value");
parameters.Add("foo","bar");

AdjustIo.TrackEvent("abc123", parameters);
</code></pre>

In that case we would track the event and send a request to:

    http://www.adeven.com/callback?key=value&foo=bar

It should be mentioned that we support a variety of placeholders like `{idfa}`
that can be used as parameter values. In the resulting callback this
placeholder would be replaced with the ID for Advertisers of the current
device. Also note that we don't store any of your custom parameters, but only
append them to your callbacks. If you haven't registered a callback for an
event, these parameters won't even be read.

### Add tracking of revenue

If your users can generate revenue by clicking on advertisements or making
in-app purchases you can track those revenues. If, for example, a click is
worth one cent, you could make the following call to track that revenue:

```actionscript
AdjustIo.TrackRevenue(1.0);
```

The parameter is supposed to be in cents and will get rounded to one decimal
point. If you want to differentiate between different kinds of revenue you can
get different Event Tokens for each kind. Again, you need to create those Event
Tokens in your [dashboard]. In that case you would make a call like this:

```actionscript
AdjustIo.TrackRevenue(1.0, "abc123");
```

Again, you can register a callback and provide a dictionary of named
parameters, just like it worked with normal events.

<pre><code>
Dictionary<string,string> parameters = new Dictionary<string, string>();
parameters.Add("key","value");
parameters.Add("foo","bar");

AdjustIo.TrackRevenue(1.0, "abc123", parameters);
</code></pre>

[import]: http://github.com/adeven/adjust_sdk/master/Resources/unity/UnityImport.png
[settings]: http://github.com/adeven/adjust_sdk/master/Resources/unity/AdjustIoSettings.png
[adjust.io]: http://adjust.io
[dashboard]: http://adjust.io
[releases]: https://github.com/adeven/adjust_unity_sdk/releases
[permissions]: https://raw.github.com/adeven/adjust_sdk/master/Resources/unity/UnityPermissions.png
[receiver]: https://raw.github.com/adeven/adjust_sdk/master/Resources/unity/UnityReceiver.png
[metadata] : http://github.com/adeven/adjust_sdk/master/Resources/unity/UnityMetaData.png

## License

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

The adjust-sdk is licensed under the MIT License.

Copyright (c) 2012-2013 adeven GmbH,
http://www.adeven.com

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
