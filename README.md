## Summary

This is the Unity3d SDK of adjust.io™. You can read more about adjust.io™ at [adjust.io].

## Basic Installation

These are the minimal steps required to integrate the adjust SDK into your
Unity3d project.

### 1. Get the SDK

Download the latest version from our [releases page][releases]. Download the
unityPaackge in a folder of your choice.

### 2. Add it to your project

Open your project in the Unity Editor and navigate to Assets, Import Package. Click on Custom Package and select the downloaded unityPackage file.

### 3. Integrate adjust into your app

Add the prefab located at `Assets/Adjust.prefab` to the first scene. 

In the Inspector menu of the added prefab, replace `{YourAppToken}` with your App Token. 
You can find in your [dashboard].

You can increase or decrease the amount of logs you see by changing the value
of `Log Level` to one of the following:

- `Verbose` - enable all logging
- `Debug` - enable more logging
- `Info` - the default
- `Warn` - disable info logging
- `Error` - disable warnings as well
- `Assert` - disable errors as well

Depending on whether or not you build your app for testing or for production
you must change `Environment` with one of these values:

```
    'Sandbox'
    'Production'
```

**Important:** This value should be set to `Sandbox` if and only if you or
someone else is testing your app. Make sure to set the environment to
`Production` just before you publish the app. Set it back to `Sandbox` when you
start testing it again.

We use this environment to distinguish between real traffic and artificial
traffic from test devices. It is very important that you keep this value
meaningful at all times! Especially if you are tracking revenue.

If your app makes heavy use of event tracking, you might want to delay some
HTTP requests in order to send them in one batch every minute. You can enable
event buffering by ticking the box for `Event Buffering`.

If you want to use the values located in the Unity editor at the start of the scene, tick 
the box `Start On Awake`. If you wish to launch the adjust SDK at another time leave the box unticked 
and call the method `Adjust.appDidLaunch` with the respective parameters.

For an example of scene with a button menu with these options, open the scente located at 
`Assets/ExampleGUI/ExampleGUI.unity`. The source for this scene is located at `Assets/ExampleGUI/ExampleGUI.cs`.

## Additional features

Once you integrated the adjust SDK into your project, you can take advantage
of the following features.

### 4. Add tracking of custom events.

You can tell adjust about every event you want. Suppose you want to track
every tap on a button. You would have to create a new Event Token in your
[dashboard]. Let's say that Event Token is `abc123`. In your button's
click handler method you could then add the following line to track the click:

```cs
Adjust.TrackEvent("abc123");
```

You can also register a callback URL for that event in your [dashboard] and we
will send a GET request to that URL whenever the event gets tracked. In that
case you can also put some key-value-pairs in an object and pass it to the
`trackEvent` method. We will then append these named parameters to your
callback URL.

For example, suppose you have registered the URL
`http://www.adjust.com/callback` for your event with Event Token `abc123` and
execute the following lines:

```cs
Dictionary<string,string> parameters = new Dictionary<string, string>();
parameters.Add("key","value");
parameters.Add("foo","bar");

Adjust.TrackEvent("abc123", parameters);
```

In that case we would track the event and send a request to:

```
http://www.adjust.com/callback?key=value&foo=bar
```

It should be mentioned that we support a variety of placeholders like `{idfa}`
for iOS or `{android_id}` for Android that can be used as parameter values.  In
the resulting callback the `{idfa}` placeholder would be replaced with the ID
for Advertisers of the current device for iOS and the `{android_id}` would be
replaced with the AndroidID of the current device for Android. Also note that
we don't store any of your custom parameters, but only append them to your
callbacks.  If you haven't registered a callback for an event, these parameters
won't even be read.

### 5. Add tracking of revenue

If your users can generate revenue by clicking on advertisements or making
in-app purchases you can track those revenues. If, for example, a click is
worth one cent, you could make the following call to track that revenue:

```cs
Adjust.TrackRevenue(1.0);
```

The parameter is supposed to be in cents and will get rounded to one decimal
point. If you want to differentiate between different kinds of revenue you can
get different Event Tokens for each kind. Again, you need to create those Event
Tokens in your [dashboard]. In that case you would make a call like this:

```cs
Adjust.TrackRevenue(1.0, "abc123");
```

Again, you can register a callback and provide a dictionary of named
parameters, just like it worked with normal events.

```cs
Dictionary<string,string> parameters = new Dictionary<string, string>();
parameters.Add("key","value");
parameters.Add("foo","bar");

Adjust.TrackRevenue(1.0, "abc123", parameters);
```

### 6. Receive delegate callbacks

Every time your app tries to track a session, an event or some revenue, you can
be notified about the success of that operation and receive additional
information about the current install. Follow these steps to implement a
delegate to this event.

1. Create a method with the signature of the delegate `Action<ResponseData>`.

2. After calling the launch of the adjust SDK, call the `Adjust.SetResponseDelegate` 
with the previously created method. It is also be possible to use a lambda with the same signature.

3. If instead of using the `Adjust.prefab`, the `Adjust.cs` script was added to another `GameObject`.
Don't forget to pass the name of that `GameObject` as the second parameter of `Adjust.SetResponseDelegate`.

The delegate method will get called every time any activity was tracked or
failed to track. Within the delegate method you have access to the
`responseData` parameter. Here is a quick summary of its attributes:

- `ActivityKind activityKind` indicates what kind of activity was tracked. It has
one of these values:

```
Session
Event
Revenue
```

- `bool success` indicates whether or not the tracking attempt was
  successful.
- `bool willRetry` is true when the request failed, but will be retried.
- `string error` an error message when the activity failed to track or
  the response could not be parsed. Is `null` otherwise.
- `string trackerToken` the tracker token of the current install. Is `null` if
  request failed or response could not be parsed.
- `string trackerName` the tracker name of the current install. Is `null` if
  request failed or response could not be parsed.

## Possible problems

The `Unity3d` iOS generated project is not compatible with the adjust SDK out of the box.
If you have problems building the iOS project, follow this steps:

1. Enable Objective-C Exceptions. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `exceptions`. There should be an `Enable Objective-C Exceptions` or `GCC_ENABLE_OBJC_EXCEPTIONS` option. Change it from `No` to `Yes`.

2. Disable dSYM File. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `debug information`. There should be an `Debug Information Format` or `DEBUG_INFORMATION_FORMAT` option. Change it from `DWARF with dSYM File` to `DWARF`.

3. Enable ARC. In the `Project Navigator`, select the `Unity-iPhone` project. Click the `Build Settings` tab and search for `automatic`. There should be an `Enable Objective-C Exceptions` or `GCC_ENABLE_OBJC_EXCEPTIONS` option. Change it from `No` to `Yes`.
Click the `Build Phases` tab and open the `Compile Sources` drop down. Add the compile flag `-fno-objc-arc` to all `.mm` files from the `Unity3d` platform. These files should be `main.mm` plus all between `iPhone_View.mm` and `DeviceSettings.mm` list.

[adjust.io]: http://adjust.io
[dashboard]: http://adjust.io
[releases]: https://github.com/adjust/adjust_unity_sdk/releases

## License

The adjust-sdk is licensed under the MIT License.

Copyright (c) 2012-2014 adeven GmbH,
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
