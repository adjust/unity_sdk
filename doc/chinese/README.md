## 摘要

这是 Adjust™ 的 Unity SDK。它支持 iOS、安卓、Windows Store 8.1、Windows Phone 8.1 以及 Windows 10。您可以在 [adjust.com] 中了解更多有关 Adjust™ 的信息。 

**注意**：从版本 **4.12.0** 开始，Adjust Unity SDK 与 **Unity 5 及以上**版本兼容。

**注意**：从版本 **4.19.2** 开始，Adjust Unity SDK 与 **Unity 2017.1.1 及以上**版本兼容。

**注意**：从版本 **4.21.0** 开始，Adjust Unity SDK 与 **Unity 2017.4.1 及以上**版本兼容。

阅读本文的其他语言版本：[English][en-readme]、[中文][zh-readme]、[日本語][ja-readme]、[한국어][ko-readme]。

## 目录

### 快速入门

   * [入门指南](#qs-getting-started)
      * [获取 SDK](#qs-get-sdk)
      * [添加 SDK 至您的项目](#qs-add-sdk)
      * [集成 SDK 至您的应用](#qs-integrate-sdk)
      * [Adjust 日志记录](#qs-adjust-logging)
      * [Google Play 服务](#qs-gps)
      * [ProGuard 设置](#qs-android-proguard)
      * [Google Install Referrer](#qs-install-referrer)
      * [华为 Referrer API](#qs-huawei-referrer-api)
      * [创建后流程](#qs-post-build-process)
        * [iOS 创建后流程](#qs-post-build-ios)
        * [安卓创建后流程](#qs-post-build-android)
      * [SDK 签名](#qs-sdk-signature)

### 深度链接

   * [深度链接概览](#dl)
   * [标准深度链接](#dl-standard)
   * [延迟深度链接](#dl-deferred)
   * [安卓应用中的深度链接处理](#dl-app-android)
   * [iOS 应用中的深度链接处理](#dl-app-ios)
      
### 事件跟踪

   * [跟踪事件](#et-tracking)
   * [跟踪收入](#et-revenue)
   * [收入重复数据删除](#et-revenue-deduplication)
   * [验证应用内购买](#et-purchase-verification)

### 自定义参数

   * [自定义参数概览](#cp)
   * [事件参数](#cp-event-parameters)
      * [事件回传参数](#cp-event-callback-parameters)
      * [事件合作伙伴参数](#cp-event-partner-parameters)
      * [事件回传标识符](#cp-event-callback-id)
   * [会话参数](#cp-session-parameters)
      * [会话回传参数](#cp-session-callback-parameters)
      * [会话合作伙伴参数](#cp-session-partner-parameters)
      * [延迟启动](#cp-delay-start)

### 其他功能

   * [AppTrackingTransparency 框架](#ad-att-framework)
      * [应用跟踪授权包装器](#ad-ata-wrapper)
   * [SKAdNetwork 框架](#ad-skadn-framework)
   * [推送标签（卸载跟踪）](#ad-push-token)
   * [归因回传](#ad-attribution-callback)
   * [广告收入跟踪](#af-ad-revenue)
   * [订阅跟踪](#ad-subscriptions)
   * [会话与事件回传](#ad-session-event-callbacks)
   * [用户归因](#ad-user-attribution)
   * [设备 ID](#ad-device-ids)
      * [iOS 广告标识符](#ad-idfa)
      * [Google Play 服务广告标识符](#ad-gps-adid)
      * [Amazon 广告标识符](#ad-amazon-adid)
      * [Adjust 设备标识符](#ad-adid)
   * [预安装的跟踪链接](#ad-pre-installed-trackers)
   * [离线模式](#ad-offline-mode)
   * [禁用跟踪](#ad-disable-tracking)
   * [事件缓冲](#ad-event-buffering)
   * [后台跟踪](#ad-background-tracking)
   * [GDPR 被遗忘权](#ad-gdpr-forget-me)
   * [禁用第三方分享](#ad-disable-third-party-sharing)

### 测试与故障排查
   * [iOS 中的调试信息](#tt-debug-ios)

### 许可
  * [许可协议](#license)


## 快速入门

### <a id="qs-getting-started"></a>入门指南

如需将 Adjust SDK 集成到您的 Unity 项目中，请按照以下步骤操作。

### <a id="qs-get-sdk"></a>获取 SDK

您可以在我们的 [发布页面][releases] 下载最新版本。

### <a id="qs-add-sdk"></a>添加 SDK 至您的项目

在 Unity 编辑器中打开您的项目，前往 `Assets → Import Package → Custom Package`，然后选择已下载的 Unity 包文件。

![][import_package]

### <a id="qs-integrate-sdk"></a>集成 SDK 至您的应用

将 `Assets/Adjust/Adjust.prefab` 的预设(Prefab)添加至第一个场景。

您可以在预设检视器 (Inspector)菜单中编辑 Adjust 脚本参数，以设置以下选项：

* [手动启动](#start-manually)
* [事件缓冲](#event-buffering)
* [后台发送](#background-tracking)
* [启动延迟深度链接](#deeplinking-deferred-open)
* [应用识别码](#app-token)
* [日志级别](#adjust-logging)
* [环境](#environment)

![][adjust_editor]

<a id="app-token">利用实际的应用识别码替换`{YourAppToken}`。在控制面板中按照 [以下步骤](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token) 即可找到该识别码。 

<a id="environment">取决于您创建应用的目的是测试还是生产，将`环境`设置更改为 `Sandbox` 或 `Production`。

**重要提示：**如果您或其他人正在测试您的应用，请将该值设置为 “Sandbox”。在发布应用之前，请务必将环境设置为“Production”。如果再次开始测试，请将其重新设置为 “Sandbox”。此外还请注意，在默认情况下，Adjust 控制面板会显示应用的 Production (生产) 流量。如果您想查看在 Sandbox 模式下测试时产生的流量，请务必在控制面板中切换到 sandbox (沙盒) 流量视图。

我们利用环境设置来区分真实流量和来自测试设备的人工流量。请务必随时更新您的环境设置。

<a id="start-manually">如果您不希望 Adjust SDK 随应用的唤醒 (Awake) 事件自动启动，请选择“手动启动”。利用此选项，您可以通过调用以 `AdjustConfig` 对象作为参数的 `Adjust.start` 方法，从代码内部初始化并启动 Adjust SDK。

您可以利用显示这些选项的按钮菜单，在此处找到示例场景：`Assets/Adjust/ExampleGUI/ExampleGUI.unity`。 

此场景的来源位于 `Assets/Adjust/ExampleGUI/ExampleGUI.cs`。

### <a id="qs-adjust-logging"></a>Adjust 日志记录

将“日志级别”的值更改为以下其中一项，即可增加或减低所看到的日志精细度：

- `Verbose` - 启用所有日志
- `Debug` - 禁用详细日志
- `Info` - 禁用调试日志（默认）
- `Warn` - 禁用信息日志
- `Error` - 禁用警告日志
- `Assert` - 禁用错误日志
- `Suppress` - 禁用所有日志

如果要在手动初始化 Adjust SDK 时禁用所有导出的日志，请将日志级别设置为禁止 (Suppress)，并对 `AdjustConfig` 对象使用构造函数。此函数会打开一个布尔参数，您可以在其中输入是否支持禁止日志级别：

```cs
string appToken = "{YourAppToken}";
AdjustEnvironment environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

如果您的目标对象为 Windows，并希望以已发布 (Released) 模式查看我们库中的编译日志，请在以调试 (Debug) 模式进行测试时，将导出的日志重定向至您的应用。

启动 SDK 前，请在 `AdjustConfig` 实例中调用方法 `setLogDelegate`。

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="qs-gps"></a>Google Play 服务

自 2014 年 8 月 1 日起，Google Play 商店中的应用必须使用 [Google 广告 ID][google_ad_id] 来对设备进行唯一标识。为了使 Adjust SDK 能够使用 Google 广告 ID，请集成 [Google Play 服务][google_play_services]。为此，请将 `google-play-services_lib` 文件夹（安卓 SDK 的一部分）复制到 Unity 项目的 `Assets/Plugins/Android` 文件夹中。

下载安卓 SDK 的方法主要有两种。使用“安卓 SDK 管理器”的所有工具都会为下载和安装安卓 SDK 工具提供快速链接。安装完成后，您可以在 `SDK_FOLDER/extras/google/google_play_services/libproject/` 文件夹中找到这些库。

![][android_sdk_location]

如果您使用的工具不带“安卓 SDK 管理器”，请下载官方独立的 [安卓 SDK][android_sdk_download]。下一步，请按照 `SDK Readme.txt` 自述文件中的说明下载安卓 SDK 工具，该自述文件由 Google 提供，位于安卓 SDK 文件夹中。

**更新**：Google 利用最新的安卓 SDK 版本，更改了 SDK 根文件夹中的 Google Play 服务文件夹的结构。现在看上去像这样：

![][android_sdk_location_new]

现在，您只需添加 Google Play 服务库中的地下室（basement）来对应 Adjust SDK 的需求。为此，请将 `play-services-basement-x.y.z.aar` 文件添加到您的 `Assets/Plugins/Android` 文件夹中。 

使用 Google Play 服务库 15.0.0，Google 已将获取 Google 广告 ID 所需的类移至 `play-services-ads-identifier` 包。如果您使用的库版本为 15.0.0 及更高版本，请将此包添加到您的应用中。完成后，请进行测试以确保 Adjust SDK 正确获取 Google 广告 ID；我们注意到有不一致的情况产生，然而具体视您所用的 Unity 集成开发环境 (IDE) 版本而定。 

#### 针对 Google 广告 ID 的测试

要检查 Adjust SDK 是否正在接收 Google 广告 ID，请在启动应用时将 SDK 配置为以测试 (Sandbox) 模式运行，并将日志级别设置为详细 (Verbose)。之后，在应用中跟踪会话或事件，并查看详细日志中记录的参数列表。如果您看到 `gps_adid` 参数，则我们的 SDK 已成功读取 Google 广告 ID。

如果在获取 Google 广告 ID 时遇到任何问题，请在我们的 Github 库中提问，或联系 support@adjust.com。

### <a id="qs-android-proguard"></a>ProGuard 设置

如果您使用的是 Proguard，请将如下代码行添加至您的 Proguard 文件：

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

### <a id="qs-install-referrer"></a>Google Install Referrer

为了归因安卓应用的安装，Adjust 需要获取有关 Google Install Referrer 的信息。您可以使用 **Google Play Referrer API** 或通过广播接收器捕获 **Google Play 商店 intent ** 的方式，将应用设置为获取这些信息。 

Google 引入 Google Play Referrer API 是为了提供一种比 Google Play 商店 intent 更可靠、更安全的方法，以获取 Install Referrer 信息并帮助归因供应商对抗点击劫持。Google Play 商店 intent 将暂时与 API 并行存在，但在将来会被弃用。我们鼓励大家对此给予支持。 

Adjust 创建后流程可以捕获 Google Play 商店 intent；您可以采取一些其他步骤来加大对新 Google Play Referrer API 的支持。

要加大对 Google Play Referrer API 的支持，请从 Maven 库中下载 [Install Referrer 库][install-referrer-aar]，并将 AAR 文件放入 `Plugins/Android` 文件夹中。

#### <a id="qs-huawei-referrer-api"></a>华为 Referrer API

从版本 4.21.1 开始，Adjust SDK 将支持对装有华为 App Gallery 10.4 或更新版本的设备进行安装跟踪。无需其他集成步骤，就可以开始使用华为 Referrer API。

### <a id="qs-post-build-process"></a>创建后流程

如需完成应用创建流程，Adjust Unity 包需执行自定义创建后操作，以确保 Adjust SDK 可以在应用内正常运行。 

此流程由 `AdjustEditor.cs` 中的 `OnPostprocessBuild` 方法执行。导出的日志消息会写入 Unity IDE 控制台输出窗口。

#### <a id="qs-post-build-ios"></a>iOS 创建后流程

若要正确执行 iOS 创建后流程，请使用 Unity 5 及更高版本，并安装“iOS 创建支持”。iOS 创建后流程会对生成的 Xcode 项目作出以下更改：

- 添加 `iAd.framework`（Apple Search Ads 跟踪所需）
- 添加 `AdSupport.framework`（读取 IDFA 所需）
- 添加 `CoreTelephony.framework`(读取设备所连接到的网络类型所需)
- 添加其他链接器标记 `-ObjC`（在创建期间识别 Adjust Objective-C 类别所需）
- 启用“Objective-C 例外情况”

如果您启用了 iOS 14 支持 (`Assets/Adjust/Toggle iOS 14 Support`)，iOS 创建后流程会向您的 Xcode 项目中加入两个额外的框架：

- 添加 `AppTrackingTransparency.framework` (用来请求用户授予跟踪许可，并获知用户许可状态)
- 添加 `StoreKit.framework`(与 SKAdNetwork 框架通讯所需)

#### <a id="qs-post-build-android"></a>安卓创建后流程

安卓创建后流程会对位于 `Assets/Plugins/Android/` 中的 `AndroidManifest.xml` 文件进行更改，还会检查安卓插件文件夹中是否存在 `AndroidManifest.xml` 文件。如果该文件不存在，该流程会从我们兼容的清单文件 `AdjustAndroidManifest.xml` 中创建一个副本。如果已经有 `AndroidManifest.xml` 文件，该流程会进行以下更改：

- 添加 “`INTERNET” 权限（互联网连接所需）
- 添加 `ACCESS_WIFI_STATE` 权限（未通过 Play Store 分发应用时所需）
- 添加 `ACCESS_NETWORK_STATE` 权限 (读取设备所连接到的网络类型所需)
- 添加 `BIND_GET_INSTALL_REFERRER_SERVICE` 权限（新的 Google Install Referrer API 正常运作所需）
- 添加 Adjust 广播接收器（通过 Google Play 商店 intent 获取 Install Referrer 信息时所需）有关更多详情，请查阅官方的 [安卓 SDK 自述文件][安卓]。 

**注意：**如果您使用自己的广播接收器来处理 `INSTALL_REFERRER` intent，则无需将 Adjust 广播接收器添加到清单文件中。请将其删除，但是按照 [安卓指南][android-custom-receiver] 中的说明，将调用添加到自己接收器中的 Adjust 广播接收器。

### <a id="qs-sdk-signature"></a>SDK 签名

账户管理员可以为您激活 Adjust SDK 签名。如果您希望使用该功能，请发送电子邮件至 support@adjust.com 联系 Adjust 支持部门。

如果您的账户已启用 SDK 签名，并且您可访问控制面板中的应用密钥，则将所有的密钥参数（`secretId、`info2`、`info3`、`info4`）添加至 `AdjustConfig` 实例的 `setAppSecret` 方法：

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

现在，SDK 签名已集成到您的应用中。 


## 深度链接

### <a id="dl"></a>深度链接概览

**我们支持在 iOS 和安卓平台上使用深度链接。**

如果您使用的是已启用深度链接的 Adjust 跟踪链接，则可以接收有关深度链接 URL 及其内容的信息。不论用户的设备上已经安装了应用（标准深度链接）还是尚未安装应用（延迟深度链接），用户都可以与 URL 交互。 

利用标准深度链接，您可以通过安卓平台接收深度链接内容；但是，安卓并不自动提供对延迟深度链接的支持。如需访问延迟深度链接内容，您可以使用 Adjust SDK。

在生成的 Xcode 项目（适用于 iOS）和安卓 Studio/Eclipse 项目（适用于安卓）中，以 **原生级别** 在应用中设置深度链接处理。

### <a id="dl-standard"></a>标准深度链接

与标准深度链接相关的信息无法以 Unity C# 代码的形式提供给您。一旦您启用应用来处理深度链接，您将在原生级别获取有关深度链接的信息。下面介绍针对 [安卓](#dl-app-android) 和 [iOS](#dl-app-ios) 应用启用深度链接的方法，您可在其中获得更多信息。

### <a id="dl-deferred"></a>延迟深度链接

为了获取有关延迟深度链接的内容信息，请在 `AdjustConfig` 对象上设置回传方法。该方法将接收一个“字符串”参数，其中将传送 URL 内容。通过调用 `setDeferredDeeplinkDelegate` 方法，在 config 对象上设置该方法：

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

<a id="deeplinking-deferred-open"></a>利用延迟深度链接，您可以在 `AdjustConfig` 对象上进行一个额外设置。一旦 Adjust SDK 获得延迟深度链接信息，您便可选择我们的 SDK 是否应该打开 URL。您可在 config 对象上调用 `setLaunchDeferredDeeplink` 方法来设置此选项：

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

如果未进行任何设置，**Adjust SDK默认情况下，将始终尝试打开 URL**。

为了让您的应用支持深度链接，请为每个受支持的平台设置方案。

### <a id="dl-app-android"></a>安卓应用中的深度链接处理

如需在原生级别设置安卓应用中的深度链接处理，请按照我们官方 [安卓 SDK 自述文件][android-deeplinking] 中的说明进行操作。

此设置应在原生安卓 Studio/Eclipse 项目中完成。

### <a id="dl-app-ios"></a>iOS 应用中的深度链接处理

**此设置应在原生 Xcode 项目中完成。**

如需在原生级别设置 iOS 应用中的深度链接处理，请使用原生的 Xcode 项目并按照我们官方 [iOS SDK 自述文件][ios-deeplinking] 中的说明进行操作。

## 事件跟踪

### <a id="et-tracking"></a>跟踪事件

您可以使用 Adjust 来跟踪应用中的任何事件。如果您想要跟踪某个按钮的每次点击，请在您的控制面板中 [创建新的事件识别码](https://help.adjust.com/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adjust-dashboard)。假设事件识别码为 `abc123`。在按钮的点击处理程序方法中，添加以下行来跟踪点击：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

### <a id="et-revenue"></a>跟踪收入

如果您的用户通过与广告互动或进行应用内购买的方式为您带来收入，您可以通过事件来跟踪此类收入：例如：如果增加一次点击值一欧分，您可以这样来跟踪收入事件：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

设置货币识别码后，Adjust 会使用 openexchange API 自动将收入转化为您所选的报告收入。[在此处了解更多有关货币兑换的信息](http://help.adjust.com/tracking/revenue-events/currency-conversion)。

如果您想要跟踪应用内购买，请确保仅在购买完成且商品已购买后才调用 `trackEvent`。要想避免跟踪用户实际未产生的收入，这点十分重要。


### <a id="et-revenue-deduplication"></a>收入重复数据删除

添加可选的交易 ID，以避免跟踪重复的收入。SDK 会记住最近的十个交易 ID，并跳过交易 ID 重复的收入事件。这对于跟踪应用内购买尤其有用。 

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01,"EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

### <a id="et-purchase-verification"></a>应用内收入验证

利用服务器端收据验证工具 [Adjust 的收入验证][unity-purchase-sdk] 验证应用内购买。  

## 自定义参数

### <a id="cp"></a>自定义参数概览

除了 Adjust SDK 默认收集的数据点之外，您还可以使用 Adjust SDK 进行跟踪，并根据需要添加任意数量的自定义值（用户 ID、产品 ID 等）到事件或会话中。自定义参数仅作为原始数据提供，且 **不会** 出现在 Adjust 控制面板中。

针对自己内部使用而收集的值，使用 [回传参数](https://help.adjust.com/en/manage-data/export-raw-data/callbacks/best-practices-callbacks)，并对与外部合作伙伴共享的值使用合作伙伴参数。如果某个值（如产品 ID）同时由于内外部合作伙伴使用而受到跟踪，我们建议同时使用回传和合作伙伴参数来跟踪该值。

### <a id="cp-event-parameters"></a>事件参数

### <a id="cp-event-callback-parameters"></a>事件回传参数

如果您在 [控制面板] 中为事件标记了回传 URL，则无论事件何时受到跟踪，我们都会向该 URL 发送 GET 请求。您还可以在对象中放入键值对，然后将其传递至 `trackEvent` 方法。然后我们会将这些参数附加至您的回传 URL。

例如，如果您已注册 URL `http://www.example.com/callback`，则您将这样跟踪事件：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

在这种情况下，我们会跟踪该事件并发送请求至：

```
http://www.example.com/callback?key=value&foo=bar
```

Adjust 支持各种占位符，例如用于 iOS 的 `{idfa}` 或用于安卓的 `{gps_adid}`，这些占位符可用作参数值。使用此示例，我们会在产生的回传中将占位符替换为当前设备的 IDFA/Google Play 服务 ID。了解更多关于 [实时回传](https://help.adjust.com/en/manage-data/export-raw-data/callbacks) 的信息，并查看我们完整的 [占位符](https://partners.adjust.com/placeholders/) 列表。 

**注意：**我们不会存储您的任何自定义参数。我们仅将这些参数附加到您的回传中。如果您尚未针对事件注册回传，我们不会读取这些参数。


### <a id="cp-event-partner-parameters"></a>事件合作伙伴参数

参数在控制面板中激活后，您可以将其发送至网络合作伙伴。了解更多关于 [模块合作伙伴](https://docs.adjust.com/en/special-partners/) 及其扩展集成的信息。

工作方式与回传参数相同；可以通过调用 `AdjustEvent` 实例上的 `addPartnerParameter` 方法来进行添加。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

您可以在我们的 [特殊合作伙伴指南][special-partners] 中了解更多有关特殊合作伙伴以及这些集成的信息。

### <a id="cp-event-callback-id"></a>事件回传标识符

您可以为想要跟踪的每个事件添加自定义字符串标识符。我们在事件回传中报告此标识符，以便您了解哪些事件得以成功跟踪。通过调用 `AdjustEvent` 实例上的 `setCallbackId` 方法来设置标识符：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="cp-session-parameters"></a>会话参数

会话参数在本地保存，并随每个 Adjust SDK **事件和会话** 一同发送。无论您何时添加这些参数，我们都会对其进行保存（因此您无需再次添加）。添加同样的参数两次不会有任何影响。

可以在启动 Adjust SDK 前发送会话参数。因此，利用 [SDK 延迟](#cp-delay-start)，您可以检索其他值（例如应用服务器的身份验证识别码），以便通过 SDK 的初始化一次性发送所有信息。 

### <a id="cp-session-callback-parameters"></a>会话回传参数

您可以保存与每个 Adjust SDK 会话一同发送的事件回传参数。

会话回传参数的接口与事件回传参数的接口类似。通过调用 `Adjust` 实例的 `addSessionCallbackParameter` 方法来添加密钥及其值，而不是将它们添加至事件：

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

会话回传参数与事件回传参数相合并，从而将所有信息以整体的方式进行发送；但事件回传参数的优先级高于会话回传参数。如果您添加的事件回传参数与会话回传参数有相同的密钥，我们将显示事件值。

您可以通过将所需的密钥传递至 `Adjust` 实例的 `removeSessionCallbackParameter` 方法来删除特定的会话回传参数。

```cs
Adjust.removeSessionCallbackParameter("foo");
```

若要从会话回传参数中删除所有密钥及其对应的值，您可以利用 `Adjust` 实例的 `resetSessionCallbackParameters` 方法来进行重置。

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>会话合作伙伴参数

与 [会话回传参数](#cp-session-callback-parameters) 的方式一样，会话合作伙伴参数也会与触发 SDK 的每个事件或会话一同发送。

这些参数会传送至在 [控制面板] 中已激活相关集成的渠道合作伙伴。

会话合作伙伴参数接口与事件合作伙伴参数接口类似；但是，并非将密钥及其值添加至事件，而是通过调用 `Adjust` 实例的 `addSessionCallbackParameter` 方法进行添加：

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

会话合作伙伴与事件合作伙伴参数相合并。但是，事件合作伙伴参数的优先级高于会话合作伙伴参数。如果您添加的事件合作伙伴参数与会话合作伙伴参数有相同的密钥，我们将显示事件值。

要删除特定的会话合作伙伴参数，请将所需的密钥传递至 `Adjust` 实例的 `removeSessionPartnerParameter` 方法。

```cs
Adjust.removeSessionPartnerParameter("foo");
```

若要从会话合作伙伴参数中删除所有的密钥及其对应的值，请利用 `Adjust` 实例的 `resetSessionPartnerParameters` 方法来将其重置。

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="cp-delay-start"></a>延迟启动

延迟 Adjust SDK 的启动可以为您的应用提供更充裕的时间，来接收所有您想要随安装发送的会话参数（例如：唯一标识符）。

利用 `AdjustConfig` 实例中的 `setDelayStart` 方法，以秒为单位设置初始延迟时间：

```cs
adjustConfig.setDelayStart(5.5);
```

在此示例中，Adjust SDK 无法在 5.5 秒内发送初始安装会话和任何新事件。5.5 秒后（或您在其此期间调用 `Adjust.sendFirstPackages()`），每个会话参数会添加至延迟的安装会话和事件，并且 Adjust SDK 会照常工作。

您最多可以将 Adjust SDK 的启动时间延长 10 秒。

## 其他功能

将 Adjust SDK 集成到项目中后，您即可利用以下功能：

### <a id="ad-att-framework"></a>AppTrackingTransparency 框架

**注意**：此功能仅限 iOS 平台。

每发送一个包，Adjust 的后端就会收到下列四 (4) 种许可状态之一，了解用户是否授权分享应用相关数据，用于用户或设备跟踪：

- Authorized (授权)
- Denied (拒绝)
- Not Determined (待定)
- Restricted (受限)

如果设备收到了用于用户设备跟踪目的应用相关数据访问授权请求，那么返回的状态要么是 Authorized，要么是 Denied。

如果设备尚未收到用于用户设备跟踪目的应用相关数据访问授权请求，那么返回的状态是 Not Determined。

如果应用跟踪数据授权受限，那么返回的状态是 Restricted。

如果您不需要自定义显示的弹出对话框，SDK 拥有内置机制可在用户回复弹出对话框后接收更新后的状态。为了简便、高效地向后端发送用户许可的新状态，Adjust SDK 会提供一个应用跟踪授权方法包装器，详情请参阅下一章节 "应用跟踪授权包装器"。

### <a id="ad-ata-wrapper"></a>应用跟踪授权包装器

**注意**：此功能仅限 iOS 平台。

您可以使用 Adjust SDK 请求用户授权，让用户允许您访问他们的应用相关数据。基于[requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc)方法，Adjust SDK 打造了一个包装器，您可以定义回传方法，了解用户是否授予了数据跟踪许可。借助该包装器，只要用户回复弹出对话框，这一信息就能通过您定义的回传方式传递回来。SDK 也会通知后端用户的许可选择。`NSUInteger` 值将通过您的回传方法传递，不同值的含义如下：

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined` (授权状态待定)
- 1: `ATTrackingManagerAuthorizationStatusRestricted` (授权状态受限)
- 2: `ATTrackingManagerAuthorizationStatusDenied`(已拒绝)
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`(已授权)

要使用该包装器，您可以按照下列方法进行调用：

```csharp
Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
{
    switch (status)
    {
        case 0:
            // ATTrackingManagerAuthorizationStatusNotDetermined case
            break;
        case 1:
            // ATTrackingManagerAuthorizationStatusRestricted case
            break;
        case 2:
            // ATTrackingManagerAuthorizationStatusDenied case
            break;
        case 3:
            // ATTrackingManagerAuthorizationStatusAuthorized case
            break;
    }
});
```

### <a id="ad-skadn-framework"></a>SKAdNetwork 框架

**注意**：此功能仅限 iOS 平台。

如果您已经安装了 Adjust iOS SDK v4.23.0 或更新版本，且您的应用在 iOS 14 端运行，那么与 SKAdNetwork 之间的通讯会默认启用，但您可以自行禁用通讯。启用状态下，Adjust 会在 SDK 初始化时自动注册 SKAdNetwork 归因。如果您在 Adjust 控制面板中对事件进行了接收转化值设置，那么 Adjust 后端就会将转化值数据发送给 SDK。然后 SDK 会设定转化值。Adjust 收到 SKAdNetwork 回传数据后，会在控制面板中予以显示。

如果您不希望 Adjust SDK 自动与 SKAdNetwork 通讯，可以针对配置对象调用如下方法：

```csharp
adjustConfig.deactivateSKAdNetworkHandling();
```

### <a id="ad-push-token"></a>推送标签（卸载跟踪）

推送标签用于受众分群工具和客户回传；也是跟踪卸载和重装所需的信息。

如需向我们发送推送通知标签，请在获取应用的推送通知标签（或每当其值更改时）时，调用 `Adjust` 实例上的 `setDeviceToken` 方法：

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>归因回传

您可以设置一个回传来获取归因变更的通知。我们考虑到归因有各种不同的来源，所以我们异步提供这些信息。与第三方共享您的任何数据之前，请务必考虑 [适用的归因数据政策][attribution_data]。 

请按照以下步骤在您的应用程序中添加可选的回传：

1. 创建一个带有委托 `Action<AdjustAttribution>` 签名的方法。

2. 创建 `AdjustConfig` 对象后，利用之前创建的方法调用 `adjustConfig.setAttributionChangedDelegate`。您也可以使用带有相同签名的 lambda。

3. 如果不是使用 `Adjust.prefab`，而是添加了 `Adjust.cs` 脚本到另一个 `GameObject`，请务必将 `GameObject` 的名称作为 `AdjustConfig.setAttributionChangedDelegate` 的第二参数来传递。

因为使用了 `AdjustConfig` 实例来配置回传，所以请在调用 `Adjust.start` 前调用 `adjustConfig.setAttributionChangedDelegate`。

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

SDK 收到最终归因数据后，会调用回传函数。您可以在回传函数中利用“归因参数”。以下是其属性的快速摘要：

- `string trackerToken` 当前归因的跟踪码
- `string trackerName` 当前归因的跟踪链接名称
- `string network` 当前归因的渠道分组级别
- `string campaign` 当前归因的推广活动分组级别
- `string adgroup` 当前归因的广告组分组级别
- `string creative` 当前归因的素材分组级别
- `string clickLabel` 当前归因的点击标签
- `string adid` Adjust 设备标识符

### <a id="ad-ad-revenue"></a>广告收入跟踪

您可以通过使用以下方法，利用 Adjust SDK 对广告收入信息进行跟踪：

```csharp
Adjust.trackAdRevenue(source, payload);
```

您需要传递的方法参数包括：

-`source` - 来源，表明广告收入信息来源的“字符串”对象。
-`payload` -- 负载，包含字符串形式广告收入 JSON 的“字符串”对象。

目前，我们支持以下`source` 参数值：

- `AdjustConfig.AdjustAdRevenueSourceMopub` - 代表 [MoPub 聚合平台][sdk2sdk-mopub]

### <a id="ad-subscriptions"></a>订阅跟踪

**注意**：此功能仅适用于 SDK 4.22.0 及以上版本。

您可以用 Adjust SDK 跟踪 App Store 和 Play 应用商店的订阅，并验证这些订阅是否有效。订阅购买成功后，请向 Adjust SDK 进行如下调用：

**针对 App Store 订阅：**

```csharp
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

Adjust.trackAppStoreSubscription(subscription);
```

**针对 Play 应用商店订阅：**

```csharp
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

Adjust.trackPlayStoreSubscription(subscription);
```

针对 App Store 订阅的订阅跟踪参数：

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency (您需要发送 [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) 对象的 [currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc) )
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- [receipt](https://developer.apple.com/documentation/foundation/nsbundle/1407276-appstorereceipturl)
- [transactionDate](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- salesRegion (您需要发送 [priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) 对象的 [countryCode](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc) )

针对 Play 应用商店订阅的订阅跟踪参数：

- [price](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [currency](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [orderId](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [signature](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [purchaseToken](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [purchaseTime](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

**注意：** Adjust SDK 提供的订阅跟踪 API 需要所有参数都以 `string` (字符串) 值的形式发送。在跟踪订阅前，API 需要您将上文中提到的参数发送给订阅对象。Unity 中处理应用购买的库有多种，每一种都会在订阅购买成功完成时，以具体的格式返回上文描述的信息。请找到处理应用购买的库，在库返回的响应中找到这些参数，提取参数值，并将参数值以字符串值 (string values) 的形式发送给 Adjust API。

与事件跟踪一样，您也可以向订阅对象附加回传和合作伙伴参数：

**针对 App Store 订阅：**

```csharp
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

// add callback parameters
subscription.addCallbackParameter("key","value");
subscription.addCallbackParameter("foo","bar");

// add partner parameters
subscription.addPartnerParameter("key","value");
subscription.addPartnerParameter("foo","bar");

Adjust.trackAppStoreSubscription(subscription);
```

**针对 Play 应用商店订阅：**

```csharp
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

// add callback parameters
subscription.addCallbackParameter("key","value");
subscription.addCallbackParameter("foo","bar");

// add partner parameters
subscription.addPartnerParameter("key","value");
subscription.addPartnerParameter("foo","bar");

Adjust.trackPlayStoreSubscription(subscription);
```

### <a id="ad-session-event-callbacks"></a>会话和事件回传

您可以设置各种回传，以通知您成功和失败的事件和/或会话。

请按照以下步骤为跟踪成功的事件添加回传函数：

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

为跟踪失败的事件添加以下回传函数：

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

对于跟踪成功的会话：

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

对于跟踪失败的会话：

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

SDK 尝试向服务器发送包后，将会调用回传函数。在回传中，您可以访问专门用于回传的响应数据对象。会话响应数据属性的摘要如下：

- `string Message` 来自服务器的消息或 SDK 记录的错误
- `string Timestamp` 服务器时间戳
- `string Adid` 由 Adjust 提供的设备唯一标识符
- `Dictionary JsonResponse` JSON对象及服务器响应

两个事件响应数据对象都包含：

- `string EventToken` 跟踪的包为事件时的事件识别码
- `string CallbackId` 为事件对象设置的自定义回传 ID

事件和会话都跟踪失败的对象也包含：

- `bool WillRetry` 表示稍后将尝试重新发送包

### <a id="ad-user-attribution"></a>用户归因

只要归因信息发生更改，就会触发此回传，如归因回传。想要随时访问用户当前的归因信息，您可通过调用 `Adjust` 实例的以下方法来实现：

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**注意**：只有在我们的后台跟踪到应用安装并触发归因回传后，您才能获取当前的归因信息。因此，在 SDK 经过初始化以及归因回传触发前，您无法访问用户的归因值。

### <a id="ad-device-ids"></a>设备 ID

Adjust SDK 支持您接收设备标识符。

### <a id="ad-idfa">iOS 广告标识符

如需获取 IDFA，请调用 `Adjust` 实例的函数 `getIdfa`：

```cs
string idfa = Adjust.getIdfa();
```

### <a id="ad-gps-adid"></a>Google Play 服务广告标识符

Google 广告 ID 只能在后台线程中读取。如果您利用 `Action<string>` 委托调用 `Adjust` 实例的 `getGoogleAdId` 方法，则在任何情况下都能成功：

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

现在，您将能够以变量 `googleAdId` 来访问 Google 广告 ID。

### <a id="ad-amazon-adid"></a>Amazon 广告标识符

如果您需要获取 Amazon 广告 ID，请调用` Adjust` 实例的 `getAmazonAdId` 方法：

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="ad-adid"></a>Adjust 设备标识符

我们的后台会为安装了您应用的每台设备，生成唯一的 Adjust 设备标识符（称为 `adid`）。为了获得此标识符，请调用 `Adjust` 实例的此方法：

```cs
String adid = Adjust.getAdid();
```

只有在我们的后台跟踪到应用安装后，您才能获取有关 adid 的信息。因此，在 SDK 经过初始化以及成功跟踪到应用安装前，您无法访问 adid 值。

### <a id="ad-pre-installed-trackers"></a>预安装跟踪链接

如需使用 Adjust SDK 来识别已在设备中预安装应用的用户，请按照以下步骤操作：

1. 在 [控制面板] 中创建新的跟踪链接。
2. 设置 ``AdjustConfig` 的默认跟踪链接：

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

  用您在步骤 2 中创建的跟踪码替换 `{TrackerToken}`。例如 `{abc123}`
  
尽管控制面板中显示的是跟踪链接（包括 `http://app.adjust.com/`），但在源代码中，您应该仅输入六个或七个字符的识别码，而不是整个 URL。

3. 创建并运行应用。您应该可以在导出的日志中看到以下行：

    ```
    默认跟踪链接：'abc123'
    ```

### <a id="ad-offline-mode"></a>脱机模式

脱机模式会暂停向我们的服务传输数据，但会保留要在以后发送的跟踪数据。Adjust SDK 处于脱机模式时，所有信息都会保存在一个文件中。请注意不要在脱机模式下触发太多事件。

调用参数为 `true` 的 `setOfflineMode` 即可激活脱机模式。

```cs
Adjust.setOfflineMode(true);
```

调用参数为 `false` 的 `setOfflineMode` 即可禁用脱机模式。当您将 Adjust SDK 调回在线模式后，保存的所有信息都会发送到我们的服务器，并保留正确的时间信息。

每次会话之间都不会记住该设置；这意味，即使应用在处于脱机模式时终止，每当 SDK 启动时，其都会处于在线模式。

### <a id="ad-disable-tracking"></a>禁用跟踪

您可以通过调用“启用”参数为 `false` 的 `setEnabled` 方法来禁用 Adjust SDK 的跟踪功能。每次会话之间都会记住该设置，但只能在第一个会话后被激活。

```cs
Adjust.setEnabled(false);
```

您可以利用 `isEnabled` 方法来查看 Adjust SDK 目前是否已激活。您始终可以通过调用`enabled` 参数设置为 `true` 的 `setEnabled` 来激活 Adjust SDK。

### <a id="ad-event-buffering"></a>事件缓冲

如果您的应用大量使用事件跟踪，您可能想要延迟部分网络请求，以便每分钟按批量发送。您可以利用 `AdjustConfig` 实例来启用事件缓冲：

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

如果未进行任何设置，事件缓冲功能将默认处于禁用状态。

### <a id="ad-background-tracking"></a>后台跟踪

Adjust SDK 的默认行为，是当应用处于后台时暂停发送网络请求。您可以在 `AdjustConfig` 实例中更改此设置：

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR 被遗忘权

根据欧盟的《一般数据保护条例》(GDPR) 第 17 条规定，用户行使被遗忘权时，您可以通知 Adjust。调用以下方法时，Adjust SDK 将会收到指示向 Adjust 后端传达用户选择被遗忘的信息：

```cs
Adjust.gdprForgetMe();
```

收到此信息后，Adjust 将清除用户数据，并且 Adjust SDK 将停止跟踪该用户。以后不会再向 Adjust 发送来自此设备的请求。

请注意，即便在测试环境中，此决定也是永久性的，不可逆转。


### <a id="ad-disable-third-party-sharing"></a>针对特定用户禁用第三方分享

如果用户行使自己的权利，拒绝与合作伙伴分享自己的数据用于营销目的，但允许用于统计目的的数据分享，您现在可以向 Adjust 发送通知，告知这一情况。 

请调用以下方法，指示 Adjust SDK 将用户禁用数据分享的选择传递给 Adjust 后端：

```csharp
Adjust.disableThirdPartySharing();
```

收到此信息后，Adjust 会停止向合作伙伴分享该用户的数据，而 Adjust SDK 将会继续如常运行。

## 测试与故障排查

### <a id="tt-debug-ios"></a> iOS 中的调试信息

即便使用创建后脚本，项目也可能还未准备好开箱即用。

如果需要，请禁用 dSYM 文件。在 `Project Navigator `（项目导航器）中，选择 `Unity-iPhone` 项目。点击 `Build Settings`（创建设置）选项卡，然后搜索“调试信息”。此时应当出现 `Debug Information Format` （调试信息格式）或 `DEBUG_INFORMATION_FORMAT` 选项。将其从 `DWARF with dSYM File` 更改为 `DWARF`。


[dashboard]:  http://dash.adjust.com
[adjust.com]: http://adjust.com

[en-readme]:  ../../README.md
[zh-readme]:  ../chinese/README.md
[ja-readme]:  ../japanese/README.md
[ko-readme]:  ../korean/README.md

[sdk2sdk-mopub]:    doc/english/sdk-to-sdk/mopub.md

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/adjust_unity_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/ios_sdk/#deeplinking-reattribution
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/en/special-partners
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

## 许可

### <a id="license"></a>许可管理

mod_pbxproj.py 文件已获得 Apache License 版本 2.0（“许可”）的授权。
除非遵守本许可，否则不得使用此文件。
您可以在 http://www.apache.org/licenses/LICENSE-2.0 上获取该许可的副本。

Adjust SDK 已获得 MIT 许可的授权。

Copyright (c) 2012-2020 Adjust GmbH, http://www.adjust.com

特此授权，持有本软件及相关文档文件（“软件”）的任何人
均可无限制地处理本软件，
其范围包括但不限于使用、复制、修改、合并、发布、分发、再许可
和/或销售本软件的副本；
具备本软件上述权限之人员
需遵守以下条件：

上述版权声明和本许可声明应包含在
本软件的所有副本或主要部分中。

本软件按“原样”提供，
不提供任何形式的明示或暗示保证，
包括但不限于有关适销性、适用于特定用途以及非侵权性的保证。在任何情况下，
作者或版权所有者都不应承担任何索赔、损害赔偿或其他责任，
无论是由本软件或本软件的使用或其他活动引起的
相关或无关的合同行为、侵权行为
或其他行为。
