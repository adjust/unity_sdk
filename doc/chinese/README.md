## 摘要

这是Adjust™的Unity SDK包，支持iOS、安卓、Windows商店8.1、Windows Phone 8.1和Windows 10。您可以在[adjust.com](https://adjust.com)了解有关Adjust™的更多信息。
 
**注意**:自版本**4.12.0**开始，Adjust Unity SDK与**Unity 5和以上**版本兼容。 

Read this in other languages: [English][en-readme], [中文][zh-readme], [日本語][ja-readme], [한국어][ko-readme].

## 目录

* [基本集成](#basic-integration)
   * [获取SDK](#sdk-get)
   * [添加SDK至您的项目](#sdk-add)
   * [集成SDK至您的应用](#sdk-integrate)
   * [Adjust日志](#adjust-logging)
   * [Google Play服务](#google-play-services)
   * [Proguard设置](#android-proguard)
   * [Google Install Referrer](#google-install-referrer)
   * [Post build process](#post-build-process)
      * [iOS post build process](#post-build-ios)
      * [安卓post build process](#post-build-android)
* [附加功能](#additional-features)
   * [事件跟踪](#event-tracking)
      * [收入跟踪](#revenue-tracking)
      * [收入重复数据删除](#revenue-deduplication)
      * [应用收入验证](#iap-verification)
      * [回调参数](#callback-parameters)
      * [合作伙伴参数](#partner-parameters)
      * [回调ID](#callback-id)
   * [会话参数](#session-parameters)
      * [会话回调参数](#session-callback-parameters)
      * [会话合作伙伴参数](#session-partner-parameters)
      * [延迟启动](#delay-start)
   * [归因回传](#attribution-callback)
   * [广告收入跟踪](#ad-revenue)
   * [会话和事件回传](#session-event-callbacks)
   * [禁用跟踪](#disable-tracking)
   * [离线模式](#offline-mode)
   * [事件缓冲](#event-buffering)
   * [GDPR 的被遗忘权](#gdpr-forget-me)
   * [SDK签名](#sdk-signature)
   * [后台跟踪](#background-tracking)
   * [设备ID](#device-ids)
      * [iOS广告ID](#di-idfa)
      * [Google Play服务广告ID](#di-gps-adid)
      * [Amazon广告ID](#di-fire-adid)
      * [Adjust设备ID](#di-adid)
   * [用户归因](#user-attribution)
   * [推送标签（Push token）](#push-token)
   * [预安装跟踪码](#pre-installed-trackers)
   * [深度链接](#deeplinking)
      * [标准深度链接场景](#deeplinking-standard)
      * [延迟深度链接场景](#deeplinking-deferred)
      * [安卓应用的深度链接处理](#deeplinking-app-android)
      * [iOS应用的深度链接处理](#deeplinking-app-ios)
* [故障排查](#troubleshooting)
   * [iOS中的调试信息](#ts-debug-ios)
* [许可协议](#license)

## <a id="basic-integration"></a>基本集成

以下介绍了将Adjust SDK集成到Unity项目的基本步骤。

### <a id="sdk-get"></a>获取SDK

从我们的[发布专页][releases]下载最新版本。

### <a id="sdk-add"></a>添加SDK至您的项目

在Unity Editor中打开您的项目，导航至`Assets → Import Package → Custom Package`，选择已下载的Unity package文件。

![][import_package]

### <a id="sdk-integrate"></a>集成SDK至您的应用

将位于`Assets/Adjust.prefab`的prefab（预制件）添加到第一场景。

在已添加prefab的`Inspector menu`中编辑Adjust脚本参数。

![][adjust_editor]

您可以在Adjust prefab中设置如下内容：

* [手动启动](#start-manually)
* [事件缓冲](#event-buffering)
* [后台发送](#background-tracking)
* [打开延迟深度链接](#deeplinking-deferred-open)
* [应用识别码](#app-token)
* [日志级别](#adjust-logging)
* [环境模式](#environment)

<a id="app-token">将`{YourAppToken}`替换为您的应用识别码（app token）。您可以在[控制面板][dashboard]中找到该应用识别码。

<a id="environment">取决于所构建的应用是用于测试还是发布，您必须将`Environment`改为以下两值之一：

```
'Sandbox'
'Production'
```

**重要：** 仅当您或其他人测试您的应用时，该值应设为`Sandbox`。在您发布应用之前，请确保将环境改设为`Production`。当再次测试软件时，应将此值设置为`Sandbox`。

我们按照设置的环境来区分真实流量和来自测试设备的测试流量。非常重要的是，您必须始终让该值保持有意义！这一点在您进行收入跟踪时尤为重要。

<a id="start-manually">如果您不希望Adjust SDK在应用的`Awake`事件中自动启动，请勾选`Start Manually`。勾选后，您必须在代码内初始化和启动Adjust SDK。以`AdjustConfig`对象为参数调用`Adjust.start`方法，以启动Adjust SDK。

打开位于`Assets/Adjust/ExampleGUI/ExampleGUI.unity`的场景示例，该场景示例提供了包含这些选项及其他选项的按键菜单。该场景的源文件位于`Assets/Adjust/ExampleGUI/ExampleGUI.cs`。

### <a id="adjust-logging"></a>Adjust日志

您可以增加或减少在测试中看到的日志数量，方法是用以下参数之一来更改`Log Level`的值：

- `Verbose` - 启用所有日志
- `Debug` - 启用更多日志
- `Info` - 默认
- `Warn` - 禁用信息日志
- `Error` - 禁用警告信息
- `Assert` - 禁用出错信息
- `Suppress` - 禁用所有日志

如果您希望禁用所有日志输出，并且您正在从代码内手动初始化Adjust SDK,除了将日志级别设置为抑制以外，您还应该对`AdjustConfig`对象使用构建函数，它将获取boolean参数来显示是否应该支持抑制日志级别：

```cs
string appToken = "{YourAppToken}";
string environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

如果您以Windows为目标对象，为了以`released`模式查看我们库的编译日志，您必须在处于`debug`模式测试时将日志输入重定向到您的应用。

在启动SDK之前调用`AdjustConfig`实例中的`setLogDelegate`方式。

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="google-play-services"></a>Google Play服务

从2014年8月1日起，Google Play商店的应用必须使用[Google广告ID][google_ad_id]来唯一识别设备。为了使Adjust SDK能够使用Google广告ID，您必须集成[Google Play服务][google_play_services]。如果您还没有设置，请把`google-play-services_lib`文件夹复制到Unity项目的`Assets/Plugins/Android`文件夹。创建应用后，您应已成功集成了Google Play服务。

`google-play-services_lib`是安卓SDK的一部分，您可能已经安装了该SDK。

下载安卓SDK的方式主要有两种。如果您使用的工具包含`Android SDK Manager`，请下载`Android SDK Tools`。成功安装后，您将在`SDK_FOLDER/extras/google/google_play_services/libproject/`文件夹找到所需的库。

![][android_sdk_location]

如果您使用的工具不含`Android SDK Manager`，请从[官方专页][android_sdk_download]下载安卓SDK的独立版本。该版本是最基本的安卓SDK版本，不包括安卓SDK工具。您可以从安卓SDK文件夹内Google提供的`SDK Readme.txt`README文件中查看下载说明。

**更新**: 如果您已经安装了较新的安卓SDK版本，Google已经修改了根SDK文件夹中Google Play服务文件夹的结构。上述结构已经更改如下：

![][android_sdk_location_new]

新结构支持您访问Google Play服务库内的不同部分，而不是如之前一样整个库，您可以只添加Adjust SDK需要的部分——basement。添加`play-services-basement-x.y.z.aar`文件到您的`Assets/Plugins/Android`文件夹，现在您应该已经成功集成了Adjust SDK所需的Google Play服务。

**更新**：自Google Play服务库15.0.0 版本起，Google已将用于获取Google广告标识符的类别移至 [`play-services-ads-identifier`](https://mvnrepository.com/artifact/com.google.android.gms/play-services-ads-identifier)包。如果您正在使用15.0.0或以上版本Google Play服务库，请确保您已将此包添加到应用中。我们也注意到在读取Google广告标识符的过程中会出现某些不一致，这取决于您所使用的Unity DIE版本。无论您使用哪种版本和以何种方式将Google Play服务依赖项添加到您的应用，请务必**测试Adjust SDK是否成功获取了Google广告标识符**。

如要检查Adjust SDK是否正在接收Google广告标识符，请在启动应用时将SDK配置为“Sandbox“测试模式，并将日志级别设置为 `verbose`。接着，在您的应用中跟踪会话或某些事件，并在成功跟踪会话或事件后，观察在verbose日志中读取的参数列表。如果您看到名为 `gps_adid` 的参数，那么说明我们的SDK已成功读取Google广告标识符。

如果您对读取Google广告标识符有任何疑问，请随时在我们的 Github资源库中提问，或是发送邮件至 support@adjust.com。

### <a id="android-proguard"></a>Proguard设置

如果您正在使用Proguard,请添加如下代码行至您的Proguard文件:

```
-keep public class com.adjust.sdk.** { *; }
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

### <a id="google-install-referrer"></a>Google Install Referrer

为了将安卓应用的安装正确归因到其来源，Adjust需要关于**install referrer（安装引荐来源）**的信息。这可以通过**Google Play Referrer API** 或使用广播接收器（broadcast receiver)捕捉**Google Play Store intent**来获得。Adjust post build process支持您通过Google Play Store intent获取install referrer，但如需使用新的Google Referrer API，您需要进行额外设置。

**重要**: Google Play Referrer API是Google近期推出的，旨在提供更加可靠和安全地获取install referrer信息的方法，并帮助归因提供商更有效地对抗点击劫持（click injection)。我们**强烈建议**在您的应用中支持它。相比之下，通过Google Play Store intent获取install referrer的方法则安全性较低。目前该方式与新的Google Play Referrer API并行可用，但未来将被弃用。

要添加对Google Play Referrer API的支持，请从Maven存储库中下载[install referrer库][install-referrer-aar]。将AAR文件放在您的`Plugins/Android`文件夹中即可。Adjust post build process将为您完成所需的`AndroidManifest.xml`调整。

### <a id="post-build-process"></a>Post build process

为了简化构建流程，Adjust unity包将执行post build process以确保Adjust SDK正常工作。

这个过程将由`AdjustEditor.cs`中的`OnPostprocessBuild`方法执行。为了正确执行iOS Post build process，您应当在`Unity 5或以上版本`上安装`iOS build support`。

该脚本将日志输出消息写入Unity IDE控制台输出窗口。

#### <a id="post-build-ios"></a>iOS post build process

iOS post build process将在您生成的Xcode项目中执行以下修改：

- 添加 `iAd.framework` (用于Apple搜索广告跟踪)
- 添加 `AdSupport.framework` (用于读取IDFA)
- 添加 `CoreTelephony.framework` (用于读取MMC和MNC)
- 添加other linker flag `-ObjC` (用于在构建期间识别Adjust Objective-C类别)
- 启用 `Objective-C exceptions`

#### <a id="post-build-android"></a>安卓post build process

安卓post build process将修改位于`Assets/Plugins/Android/`的`AndroidManifest.xml`文件。

安卓post build process首先会检查安卓插件文件夹中是否存在`AndroidManifest.xml`文件。如果`Assets/Plugins/Android/`中没有`AndroidManifest.xml`文件，则会从兼容的manifest文件`AdjustAndroidManifest.xml`中创建一个副本。如果已存在`AndroidManifest.xml`，它将检查并执行以下修改:

- 添加 `INTERNET` 权限 (用于互联网连接)
- 添加 `ACCESS_WIFI_STATE` 权限 (用于未通过Play商店发布应用的情况）
- 添加 `ACCESS_NETWORK_STATE` 权限 (用于读取MMC和MNC)
- 添加 `BIND_GET_INSTALL_REFERRER_SERVICE` 权限 (用于支持新的Google install referrer API)
- 添加Adjust广播接收器（broadcast receiver） (用于通过Google Play Store intent获取install referrer信息)。更多详情，请参考官方[安卓SDK README][android]。请注意，如果您正在使用**自己的广播接收器**来处理`INSTALL_REFERRER` intent，则无需在manifest文件中添加Adjust广播接收器。请删除它，并在您自己的接收器内添加对Adjust广播接收器的调用，具体步骤请参考[安卓指南][android-custom-receiver]。

## <a id="additional-features"></a>附加功能

一旦您成功将Adjust SDK集成到您的项目中，您便可以使用以下功能。

### <a id="event-tracking"></a>事件跟踪

您可以使用Adjust来跟踪应用中的任何事件。假设您想要跟踪具体按钮的每次点击，您必须在[控制面板]中创建一个新的事件识别码（Event Token）。例如事件识别码是`abc123`，在按钮的点击处理方式中，您可以添加以下代码行来跟踪点击：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-tracking"></a>收入跟踪

如果您的用户可以通过点击广告或应用内购为您带来收入，您可以按照事件来跟踪这些收入。假设一次点击值一欧分，那么您可以这样来跟踪收入事件：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-deduplication"></a>收入重复数据删除

您也可以输入可选的交易ID，以避免跟踪重复收入。最近的十个交易ID将被记录下来，重复交易ID的收入事件将被跳过。这对于应用内购跟踪尤其有用。参见以下例子。

如果您想要跟踪应用内购，请确保只有在交易完成以及商品被购入后调用`trackEvent`。这样您可以避免跟踪实际未产生的收入。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

#### <a id="iap-verification"></a>应用收入验证

如果您想要验证应用内购，您可以使用Adjust的收入验证产品——我们的服务器端收据验证工具。请查看我们的`Unity purchase SDK`并在[这里][unity-purchase-sdk]了解更多。

#### <a id="callback-parameters"></a>回调参数

您可以在[控制面板][dashboard]中为您的事件登记回调URL。跟踪到事件时，我们会向该URL发送GET请求。在这种情况下，您还可以在对象中放入一些键值对，然后将其传送到`trackEvent`方式。然后我们会将这些已命名的参数添加至您的回调URL。

假设您已经为事件登记了URL`http://www.adjust.com/callback`，事件识别码为`abc123`，然后如下行跟踪事件：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

在这种情况下，我们会跟踪该事件并发送请求至：

```
http://www.adjust.com/callback?key=value&foo=bar
```

请注意，我们支持各种可以用作参数值的占位符，例如用于iOS的`{idfa}`或用于安卓的`{gps_adid}`。在接下来的回调中，`{idfa}`占位符将被当前iOS设备的广告ID所替代，`{gps_adid}`则被当前安卓设备的Google Play服务ID所替代。同时请注意，我们不保存您的任何定制参数，而只是将它们添加到您的回调中。如果您没有为事件输入回调地址，这些参数甚至不会被读取。

#### <a id="partner-parameters"></a>合作伙伴参数

您还可以针对您已在Adjust控制面板中激活的渠道合作伙伴添加被发送至合作伙伴的参数。

方式和上述提及的回调参数类似，可以通过调用您的`AdjustEvent`实例上的`addPartnerParameter`方法来添加。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

您可在我们的[特殊合作伙伴指南][special-partners]中了解到有关特殊合作伙伴和集成的更多信息。

### <a id="callback-id"></a>回调ID

您还可为想要跟踪的每个事件添加自定义字符串ID。此ID将在之后的事件成功和/或事件失败回调中被报告，以便您了解哪些事件跟踪成功或者失败。您可通过调用`AdjustEvent`实例上的`setCallbackId`方法来设置此ID。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="session-parameters"></a>会话参数

一些参数被保存发送到Adjust SDK的每一个事件和会话中。一旦您已经添加任一这些参数，您无需再每次添加它们，因为这些参数已经被保存至本地。如果您添加同样参数两次，也不会有任何效果。

这些会话参数在Adjust SDK上线之前可以被调用，以确保它们即使在安装时也可被发送。如果您需要在安装同时发送参数，但只有在SDK上线后才能获取所需的值，您可以通过[延迟](#delay-start)Adjust SDK第一次上线以允许该行为。

### <a id="session-callback-parameters"></a>会话回调参数

注册在[事件](#callback-parameters)中的相同回调参数也可以被保存发送至Adjust SDK的每一个事件或会话中。

会话回调参数拥有与事件回调参数类似的接口。该参数是通过调用`Adjust`实例的`addSessionCallbackParameter`方式被添加，而不是通过添加Key和值至事件:

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

会话回调参数将与被添加至事件的回调参数合并。被添加至事件的回调参数拥有高于会话回调参数的优先级。这意味着，当被添加至事件的回调参数拥有与会话回调参数同样Key时，以被添加至事件的回调参数值为准。

您可以通过传递Key至`Adjust`实例的`removeSessionCallbackParameter`方式来删除特定会话回调参数。

```cs
Adjust.removeSessionCallbackParameter("foo");
```

如果您希望删除会话回调参数中所有的Key及相应值，您可以通过`Adjust`实例的`resetSessionCallbackParameters`方式重置：

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="session-partner-parameters"></a>会话合作伙伴参数

与[会话回调参数](#session-callback-parameters)的方式一样，会话合作伙伴参数也将被发送至Adjust SDK的每一个事件和会话中。

它们将被传送至渠道合作伙伴，以集成您在Adjust[控制面板]上已经激活的模块。

会话合作伙伴参数具有与事件合作伙伴参数类似的接口。该参数是通过调用`Adjust`实例的`addSessionPartnerParameter`方式被添加，而不是通过添加Key和值至事件:

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

会话合作伙伴参数将与被添加至事件的合作伙伴参数合并。被添加至事件的合作伙伴参数具有高于会话合作伙伴参数的优先级。这意味着，当被添加至事件的合作伙伴参数拥有与会话合作伙伴参数同样Key时，以被添加至事件的合作伙伴参数值为准。

您可以通过传递Key至`Adjust`实例的`removeSessionPartnerParameter`方式来删除特定的会话合作伙伴参数：

```cs
Adjust.removeSessionPartnerParameter("foo");
```

如果您希望删除会话合作伙伴参数中所有的Key及其相应值，您可以通过`Adjust`实例的`resetSessionPartnerParameters`方式重置：

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="delay-start"></a>延迟启动

延迟Adjust的SDK启动可以给您的应用一些时间获取被发送至安装的会话参数，如唯一识别码（unique identifiers）等。

通过在`AdjustConfig` 实例中的`setDelayStart`（设置延迟启动）方式以秒为单位设置初始延迟时间：

```cs
adjustConfig.setDelayStart(5.5);
```

在此种情况下，Adjust SDK不会在5.5秒内发送初始安装会话以及创建任何事件。在该时间过期后或您同时调用`Adjust.sendFirstPackages()`，每个会话参数将被添加至延迟安装的会话和事件中，Adjust SDK将恢复正常。

**Adjust SDK最长的延迟启动时间为10秒。**

### <a id="attribution-callback"></a>归因回传

您可以注册一个回传以获取跟踪链接归因变化的通知。由于考虑到归因的不同来源，归因信息无法被同步提供。请按照以下步骤在您的应用中执行回传（可选）：

请您务必考虑我们的[适用归因数据政策][attribution-data]。

1. 创建一个带有`Action<AdjustAttribution>`委托签名的方式。

2. 创建`AdjustConfig`对象后，用上一步创建的方式调用`adjustConfig.setAttributionChangedDelegate`。您也可以使用带相同签名的lambda。

3. 如果您不是使用`Adjust.prefab`，而是添加了`Adjust.cs`脚本到另一个`GameObject`。请不要忘记将`GameObject`的名称作为`AdjustConfig.setAttributionChangedDelegate`的第二参数来传递。

由于使用了AdjustConfig实例来配置回传，您应在调用`Adjust.start`之前调用`adjustConfig.setAttributionChangedDelegate`。

回传函数将在SDK接收到最终归因数据时被调用。在回传函数中，您可以访问`attribution`(归因)参数。以下为其属性摘要：

- `String trackerToken` 目前归因的跟踪码token
- `String trackerName`  目前归因的跟踪码名称
- `String network`      目前归因的渠道分组级别
- `String campaign`     目前归因的推广分组级别
- `String adgroup`      目前归因的广告组分组级别
- `String creative`     目前归因的创意分组级别
- `String clickLabel`   目前归因的点击标签
- `String adid`         Adjust设备ID

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

### <a id="ad-revenue"></a>广告收入跟踪

您可以通过调用以下方法，使用 Adjust SDK 对广告收入进行跟踪：

```csharp
Adjust.trackAdRevenue(source, payload);
```

您需要传递的方法参数包括：

- `source` - 表明广告收入来源信息的`string`对象。
- `payload` - 包含广告收入 JSON 的`string`对象。

目前，我们支持以下 `source` 参数值：

- `AdjustConfig.AdjustAdRevenueSourceMopub`- 代表 MoPub 广告聚合平台（更多相关信息，请查看 [集成指南][sdk2sdk-mopub]）

### <a id="session-event-callbacks"></a>会话和事件回传

您可以注册一个回传，以在事件或者会话被跟踪时获取通知。

按照同样的步骤，执行以下回传函数，于成功跟踪事件时调用：

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

以下为事件跟踪失败的回传函数：

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

跟踪成功的会话：

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

跟踪失败的会话：

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

回传函数将于SDK发送包（package）到服务器后调用。在回传内，您能访问专为回传所设的响应数据对象。会话的响应数据对象字段摘要如下：

- `string Message` 服务器信息或者SDK纪录的错误信息
- `String Timestamp` 服务器的时间戳
- `String Adid` Adjust提供的设备唯一识别码
- `Dictionary<string, object> JsonResponse`JSON对象及服务器响应

两个事件响应数据对象都包含：

- 如果跟踪的包是一个事件，`string EventToken` 代表事件识别码。
- `string CallbackId` 为事件对象设置的自定义回调ID。

事件和会话跟踪不成功的对象也包含：

- `bool WillRetry` 表示稍后将再尝试发送数据包。

### <a id="disable-tracking"></a>禁用跟踪

您可以通过调用`setEnabled`，启用参数为`false`，来禁用Adjust SDK的跟踪功能。**该设置在会话间保存**，但只能在第一个会话后被激活。

```cs
Adjust.setEnabled(false);
```

您可以通过调用`isEnabled`方式来查看Adjust SDK目前是否被启用。您始终可以通过调用`setEnabled`，将`enabled`参数设置为`true`，来激活Adjust SDK。

### <a id="offline-mode"></a>离线模式

您可以把Adjust SDK设置离线模式，以暂停发送数据到我们的服务器，但仍然继续跟踪及保存数据并在之后发送。当设为离线模式时，所有数据将存放于一个文件中，所以请注意不要于离线模式触发太多事件。

您可以调用`setOfflineMode`，启用参数为`true`，以激活离线模式。

```cs
Adjust.setOfflineMode(true);
```

相反地，您可以调用`setOfflineMode`，启用参数为`false`，以终止离线模式。当Adjust SDK回到在线模式时，所有被保存的数据将被发送到我们的服务器，并保留正确的时间信息。

跟禁用跟踪设置不同的是，此设置在会话之间将**不被保存**。这意味着，即使应用在离线模式时被终止，每当SDK启动时都必定处于在线模式。

### <a id="event-buffering"></a>事件缓冲

如果您的应用大量使用事件跟踪，您可能想要延迟部分HTTP请求，以便按分钟成批发送这些请求。您可以调用`AdjustConfig`实例启用事件缓冲：

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

如果不作任何设置，事件缓冲功能将**默认禁用**。

### <a id="gdpr-forget-me"></a>GDPR 的被遗忘权

根据欧盟的《一般数据保护条例》(GDPR) 第 17 条规定，用户行使被遗忘权时，您可以通知 Adjust。调用以下方法，Adjust SDK 将会收到指示向 Adjust 后端传达用户选择被遗忘的信息：

```cs
Adjust.gdprForgetMe();
```

收到此信息后，Adjust 将清除该用户数据，并且 Adjust SDK 将停止跟踪该用户。以后不会再向 Adjust 发送来自此设备的请求。

### <a id="sdk-signature"></a>SDK签名

账户管理员必须启用Adjust SDK签名。如果您希望使用该功能，请联系Adjust技术支持(support@adjust.com)。

如果您已经在账户中启用了SDK签名，并可访问Adjust控制面板的应用密钥，请使用以下方法来集成SDK签名到您的应用。

您可通过传递所有密钥参数(`secretId`, `info1`, `info2`, `info3`, `info4`)到`AdjustConfig`实例的`setAppSecret`方式来设置应用密钥。

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

### <a id="background-tracking"></a>后台跟踪

Adjust SDK的默认行为是**当应用处于后台时暂停发送HTTP请求**。您可以调用`AdjustConfig`实例更改该设置：

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="device-ids"></a>设备ID

某些服务（如Google Analytics）要求您协调设备及客户ID以避免重复报告。

### <a id="di-idfa">iOS广告ID

调用`Adjust`实例的`getIdfa`函数来获取IDFA：

```cs
string idfa = Adjust.getIdfa();
```

### <a id="di-gps-adid"></a>Google Play服务广告ID

如果您需要获取Google广告ID，则仅限于在后台线程里读取。如您调用带有`Action<string>`委托的`Adjust`实例的`getGoogleAdId`方式，那么在任何情况下都可成功获取ID：

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

您将以`googleAdId`变数来访问Google广告ID。

### <a id="di-fire-adid"></a>Amazon广告ID

如果您需要获取Amazon广告ID，请在`Adjust`实例上调用`getAmazonAdId`方式：

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="di-adid"></a>Adjust设备ID

Adjust后台将为每一台安装了您应用的设备生成一个唯一的**Adjust设备ID** (**adid**)。您可在`Adjust`实例上调用以下方法来获取该ID:

```cs
String adid = Adjust.getAdid();
```

**注意**: 只有在Adjust后台跟踪到应用安装后，您才能获取**adid**的相关信息。自此之后，Adjust SDK已经拥有关于设备**adid**的信息，您可以使用此方法来访问它。因此，在SDK被初始化以及您的应用安装被成功跟踪之前，您将**无法访问adid**。

### <a id="user-attribution"></a>用户归因

归因回传通过[归因回传章节](#attribution-callback)所描述的方法被触发，以向您提供关于用户归因值的任何更改信息。如果您想要在任何其他时间访问用户当前归因值的信息，您可以通过对`Adjust`实例调用如下方法来实现：

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**注意**: 只有在Adjust后台跟踪到应用安装以及归因回传被触发后，您才能获取有关当前归因的信息。自此之后，Adjust SDK已经拥有用户归因信息，您可以使用此方法来访问它。因此，在SDK被初始化以及归因回传被触发之前，您将**无法访问用户归因值**。

### <a id="push-token"></a>推送标签（Push token）

**每当您获取推送标签或标签值发生变化时**，请在`Adjust`实例上调用`setDeviceToken`方式,以发送推送标签给我们：

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

推送标签用于Adjust受众分群工具（Audience Builder）和客户回传，且是将来发布的卸载跟踪功能的必需信息。

### <a id="pre-installed-trackers"></a>预安装跟踪码

如果您希望使用Adjust SDK来识别已在设备中预安装应用的用户，请执行以下步骤。

1. 在[控制面板][dashboard]中创建一个新的跟踪码。
2. 设置`AdjustConfig`的默认跟踪码:

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

  用您在步骤1中创建的跟踪码替换`{TrackerToken}`（跟踪码）。请注意，控制面板中显示的是跟踪URL(包括 `http://app.adjust.com/`)。在源代码中，您应该仅指定六个字符的识别码，而不是整个URL。

3. 创建并运行您的应用。您应该可以在应用日志输出中看到如下行：

   ```
   Default tracker: 'abc123'
   ```

### <a id="deeplinking"></a>深度链接

**仅iOS和安卓平台支持深度链接。**

如果您正在使用可从URL深度链接至您应用的Adjust跟踪URL，您将有机会获取深度链接URL及其内容的相关信息。点击URL的情况发生在用户已经安装了您的应用（标准深度链接场景），或用户尚未在其设备上安装您的应用（延迟深度链接场景）。在标准深度链接场景中，安卓平台原生支持您获取关于深度链接内容的信息。但是，安卓平台不提供对延迟深度链接场景的支持。在此情况下，Adjust SDK可以帮助您获取有关深度链接内容的信息。

您需要在应用的**原生级别**设置深度链接处理——即在生成的iOS Xcode项目和安卓Studio/Eclipse项目中进行设置。

#### <a id="deeplinking-standard"></a>标准深度链接场景

很遗憾，在该场景中我们暂无法以Unity C#代码传递深度链接信息。一旦您在应用中启用深度链接，您将在原生级别获取深度链接信息。请参考以下章节来了解如何分别为安卓和iOS应用启用深度链接。

#### <a id="deeplinking-deferred"></a>延迟深度链接场景

为了在延迟深度链接场景下获取关于URL内容的信息，您需要在`AdjustConfig`对象中设置一个回传方式，该方式将接收一个`string`参数，URL内容将被传送到该参数。您可以通过调用`setDeferredDeeplinkDelegate`方式在配置对象上设置该回传方式：

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

<a id="deeplinking-deferred-open">在延迟深度链接场景中，您还需在`AdjustConfig`对象上作一个额外设置。一旦Adjust SDK从后台接收到延迟深度链接信息，您可以选择是否由我们的SDK打开该URL。您可在配置对象上调用`setLaunchDeferredDeeplink`设置该选项：

```cs
// ...

private void DeferredDeeplinkCallback(string deeplinkURL) {
   Debug.Log ("Deeplink URL: " + deeplinkURL);

   // ...
}

AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setLaunchDeferredDeeplink(true);
adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);

Adjust.start(adjustConfig);
```

如果不作任何设置, **Adjust SDK将默认始终尝试打开URL**。

为了让您的应用支持深度链接，您应为每个支持的平台设置方案。

#### <a id="deeplinking-app-android"></a>安卓应用的深度链接处理

**请在原生安卓Studio/Eclipse项目中进行设置。**

请参考官方安卓SDK README中的[相关指南][android-deeplinking]在原生级别设置安卓应用的深度链接处理。

#### <a id="deeplinking-app-ios"></a>iOS应用的深度链接处理

**请在原生Xcode项目中进行设置。**

请参考官方iOS SDK README中的[相关指南][ios-deeplinking]在原生级别设置iOS应用的深度链接处理。

## <a id="troubleshooting"></a>故障排查

### <a id="ts-debug-ios"></a>iOS中的调试信息

即便使用post build脚本，项目可能还未准备就绪。

必要情况下请禁用dSYM文件。在`Project Navigator`中，选择`Unity-iPhone`项目。点击`Build Settings`，搜索`debug information`。您应当可以看到`Debug Information Format` 或者 `DEBUG_INFORMATION_FORMAT` 选项。将其从 `DWARF with dSYM File` 修改到`DWARF`。


[dashboard]:  http://adjust.com
[adjust.com]: http://adjust.com

[en-readme]:  ../../README.md
[zh-readme]:  ../chinese/README.md
[ja-readme]:  ../japanese/README.md
[ko-readme]:  ../korean/README.md

[sdk2sdk-mopub]:    ../chinese/sdk-to-sdk/mopub.md

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/adjust_unity_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/ios_sdk/#deeplinking-reattribution
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/zh/special-partners
[unity-purchase-sdk]:      https://github.com/adjust/unity_purchase_sdk
[android-deeplinking]:     https://github.com/adjust/android_sdk#deep-linking
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/1.0/installreferrer-1.0.aar
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

## <a id="license">许可协议

mod_pbxproj.py文件是在Apache License版本2.0（"License"）下授权许可的。

除遵守本许可协议，否则您不得使用此文件。

您可以通过http://www.apache.org/licenses/LICENSE-2.0下载该许可协议副本。

The Adjust SDK is licensed under the MIT License.

Copyright (c) 2012-2019 Adjust GmbH, http://www.adjust.com

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
