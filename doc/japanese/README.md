## 概要

こちらはAdjust™のUnity SDKです。iOS、Android、Windows Store 8.1、Windows Phone 8.1、Windows 10をサポートしています。Adjust™については、[adjust.com]をご覧ください。 

**注**：バージョン**4.12.0**以降より、Adjust Unity SDKは**Unity 5以降**のバージョンと互換性があります。

**注**：バージョン**4.19.2**以降より、Adjust Unity SDKは**Unity 2017.1.1以降**のバージョンと互換性があります。

**注**：バージョン**4.21.0**以降より、Adjust Unity SDKは**Unity 2017.4.1以降**のバージョンと互換性があります。

Read this in other languages：[English][en-readme]、[中文][zh-readme]、[日本語][ja-readme]、[한국어][ko-readme]

## 目次

## クイックスタート

   * [基本的な連携方法](#qs-getting-started)
      * [SDKダウンロード](#qs-get-sdk)
      * [プロジェクトにSDKを追加](#qs-sdk-add)
      * [アプリにSDKを実装](#qs-sdk-integrate)
      * [Adjustログ](#qs-adjust-logging)
      * [Google Play 開発者サービス](#qs-gps)
      * [Proguardの設定](#qs-android-proguard)
      * [Google インストールリファラ](#qs-install-referrer)
      * [Huawei リファラAPI](#qs-huawei-referrer-api)
      * [ポストビルドプロセス](#qs-post-build-process)
        * [iOSポストビルドプロセス](#qs-post-build-ios)
        * [Androidポストビルドプロセス](#qs-post-build-android)
      * [SDK シグネチャー](#qs-sdk-signature)

### ディープリンク

   * [ディープリンクの概要](#dl)
   * [スタンダードディープリンク](#dl-standard)
   * [ディファードディープリンク](#dl-deferred)
   * [Androidアプリでのディープリンク処理](#dl-app-android)
   * [iOSアプリでのディープリンク処理](#dl-app-ios)
      
### イベントトラッキング

   * [イベントトラッキング](#et-tracking)
   * [収益のトラッキング](#et-revenue)
   * [収益の重複排除](#et-revenue-deduplication)
   * [アプリ内購入の検証](#et-purchase-verification)

### カスタムパラメータ

   * [カスタムパラメータの概要](#cp)
   * [イベントパラメータ](#cp-event-parameters)
      * [イベントコールバックパラメータ](#cp-event-callback-parameters)
      * [イベントパートナーパラメータ](#cp-event-partner-parameters)
      * [イベントコールバックID](#cp-event-callback-id)
   * [セッションパラメータ](#cp-session-parameters)
      * [セッションコールバックパラメータ](#cp-session-callback-parameters)
      * [セッションパートナーパラメータ](#cp-session-partner-parameters)
      * [ディレイスタート](#cp-delay-start)

### 追加機能

   * [AppTrackingTransparencyフレームワーク](#ad-att-framework)
      * [アプリトラッキング承認ラッパー](#ad-ata-wrapper)
   * [SKAdNetworkフレームワーク](#ad-skadn-framework)
   * [Pushトークン(アンインストールトラッキング)](#ad-push-token)
   * [アトリビューションコールバック](#ad-attribution-callback)
   * [広告収益トラッキング](#ad-ad-revenue)
   * [サブスクリプション計測](#ad-subscriptions)
   * [セッションとイベントのコールバック](#ad-session-event-callbacks)
   * [ユーザーアトリビューション](#ad-user-attribution)
   * [デバイスID](#ad-device-ids)
      * [iOS広告ID](#ad-idfa)
      * [Google Play 開発者サービス広告ID](#ad-gps-adid)
      * [Amazon広告ID](#ad-amazon-adid)
      * [AdjustデバイスID](#ad-adid)
   * [プリインストールトラッカー](#ad-pre-installed-trackers)
   * [オフラインモード](#ad-offline-mode)
   * [トラッキングの無効化](#ad-disable-tracking)
   * [イベントバッファリング](#ad-event-buffering)
   * [バックグラウンドでのトラッキング](#ad-background-tracking)
   * [GDPRの忘れられる権利](#ad-gdpr-forget-me)
   * [サードパーティーとの共有を無効にする](#ad-disable-third-party-sharing)

### テストとトラブルシューティング
   * [iOSデバッグ情報](#tt-debug-ios)

### ライセンス
  * [ライセンス契約](#license)


## クイックスタート

### <a id="qs-getting-started"></a>基本的な連携方法

Adjust SDKをUnityプロジェクトに連携させるステップをご説明します。

### <a id="qs-get-sdk"></a> SDKダウンロード

[リリースページ](https://github.com/adjust/unity_sdk/releases)より最新バージョンをダウンロードしてください。

### <a id="qs-add-sdk"></a> プロジェクトにSDKを追加

Unityエディターでプロジェクトを開き、`Assets → Import Package → Custom Package` と進み、ダウンロードしたUnityパッケージファイルを選択してください。

![][import_package]

### <a id="qs-integrate-sdk"></a>アプリにSDKを実装

`Assets / Adjust / Adjust.prefab`にあるプレハブを1番目のシーンに追加してください。

プレハブの`Inspector menu`で、Adjustスクリプトのパラメータを編集してください。ここでは以下のオプションの設定ができます。

* [手動スタート](#start-manually)
* [イベントバッファリング](#event-buffering)
* [バックグラウンドでの送信](#background-tracking)
* [ディファードディープリンクの起動](#deeplinking-deferred-open)
* [アプリトークン](#app-token)
* [ログレベル](#adjust-logging)
* [環境設定](#environment)

![][adjust_editor]

<a id="app-token"> `{YourAppToken}`　にアプリトークンを入力してください。 [この手順](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token)は管理画面で確認できます。 

<a id="environment">EnvironmentにSandbox 又はProductionのどちらかを設定してください。これはテスト用アプリか本番用アプリかによって異なります。

**重要：**この値はアプリのテスト中のみ`Sandbox` に設定してください。アプリストアに提出する前に`Production` に更新されていることを必ず確認してください。テストを再度実施する場合は、`Sandbox` に戻してください。また、Adjust管理画面ではデフォルトでアプリの本番環境（プロダクション）のトラフィックを表示しているため、サンドボックスモードでテスト中に生成されたトラフィックを表示したい場合は、管理画面内でサンドボックス・モードに切り替えてください。

この環境設定は、本番環境からのトラフィックとテスト端末からのトラフィックをレポート画面で区別するために利用されます。正しく計測するために、環境設定には常に注意してください。

<a id="start-manually">アプリの`Awake` イベントでAdjust SDKを自動で起動したくない場合、`Start Manually`を選択します。このオプションが選択されている場合、`ソースコード上でAdjust SDKの初期化、および起動を行う必要があります。AdjustConfig`オブジェクトをパラメータとして`Adjust.start` メソッドをコールし、Adjust SDKを起動してください。

例えばメニューボタンを利用したシーンなど、これらのオプションのサンプルは、`Assets/Adjust/ExampleGUI/ExampleGUI.unity`で確認できます。 

このシーンのソースはAssets / Adjust / ExampleGUI / ExampleGUI.csにあります。

### <a id="qs-adjust-logging"></a> Adjustロギング

`Log Level` に設定する値を次のいずれかに変更すると、記録するログの粒度を調節できます。

-`Verbose`- 全てのログを有効にする
- `Debug` - verboseログを無効にする
- `Info` - debugログを無効にする(デフォルト)
- `Warn` - infoログを無効にする
- `Error` - warningログを無効にする
- `Assert` - errorログを無効にする
- `Suppress` - 全てのログを無効にする

Adjust SDKをマニュアルで初期化していて、全てのログ出力を非表示にしたい場合、ログレベルをSupressに設定し`てAdjustConfig`オブジェクトのコンストラクタを使います。これによってBoolean値のパラメータが開きますので、Supressログレベルがサポートされるべきかどうかを入力してください。

```cs
String appToken = "{YourAppToken}";
AdjustEnvironment environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

アプリのターゲットがWindowsベースで、コンパイル時のログをライブラリから`released` モードで見るには、`debug` モードでテストされている間アプリへのログ出力をリダイレクトする必要があります。

SDKを開始する前に、`AdjustConfig`インスタンスの`setLogDelegate`メソッドをコールしてください。

```cs
//...
adjustConfig.setLogDelegate(msg=> Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="qs-gps"></a> Google Play 開発者サービス

2014年8月1日以降、Google Playストア内のアプリは、デバイスの特定のために[Google広告 ID][google_ad_id]の使用が義務付けられています。Adjust SDKでGoogle 広告 IDを使うためには、Google Play 開発者サービス[google_play_services]を連携させる必要があります。連携を行うには、`google-play-services_lib` フォルダ(Android SDKの一部)をUnityプロジェクトの`Assets/Plugins/Android` フォルダにコピーしてください。

Android SDKをダウンロードするには主に2つの方法があります。`Android SDK Manager` が入ったツールをお使いの場合は、Android SDK toolのダウンロードとインストールができるクイックリンクが提供されています。インストールが完了したら、`SDK_FOLDER/extras/google/google_play_services/libproject/` フォルダのライブラリをご覧ください。

![][android_sdk_location]

Android SDK Managerの入ったツールをお使いでない場合は、公式ページから単独SDK[Android SDK][android_sdk_download]をダウンロードしてください。次に、`Google が提供するSDK Readme.txt` の指示に従ってAndoird SDKツールをダウンロードしてください。これは、Android SDKフォルダ内にあります。

**更新**：Android SDKの最新バージョンでは、GoogleはSDKのルートフォルダ内のGoogle Play 開発者サービスフォルダの構造を変更しています。新バージョンはこのように表示されます。

![][android_sdk_location_new]

Adjust SDKが必要とするGoogle Play 開発者サービスのライブラリの一部、つまり、basementのみを追加することができるようになりました。これを行うためには、`play-services-basement-x.y.z.aar` ファイルを`Assets/Plugins/Android`フォルダに追加してください。 

Google Play 開発者サービスのライブラリバージョン15.0.0では、Googleは、Google 広告 IDの取得に必要なクラスを`play-services-ads-identifier` パッケージに移行しました。バージョン15.0.0以降のライブラリを使用している場合は、このパッケージをアプリに追加してください。追加が完了したら、Adjust SDKがGoogle 広告 IDを正しく取得しているかテスト確認を行ってください。使用されているUnity の開発環境 (IDE)のバージョンによっては、いくつかの乖離が発生することが分かっています。 

#### Google 広告 IDのテスト

Adjust SDKがGoogle 広告 IDを取得しているかどうかを確認するには、`sandbox` モードでSDKを設定し、ログレベルを`verbose`にして起動してください。その後、アプリでセッションまたはイベントをトラッキングし、verboseログに記録されたパラメータリストをチェックします。`gps_adid` パラメータが表示されていれば、SDKはGoogle 広告 IDを正常に読み込んでいます。

Google 広告 IDの取得の際に問題が発生した場合は、Githubリポジトリでissueを開くか、support@adjust.comにお問い合わせください。

### <a id="qs-android-proguard"></a>Proguardの設定

Proguardをお使いの場合は、以下をProguardファイルに追加してください。

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
-keep public class com.android.installreferrer.**{ *; }
```

### <a id="qs-install-referrer"> </a> Google インストールリファラ

AdjustがAndroidアプリのインストールをアトリビュートするには、Googleのインストールリファラの情報が必要です。** Google Play リファラAPI **を使用するか、broadcastレシーバで**Google Play Store インテント**を受信するよう設定すると、アプリが情報を取得できます。 

GoogleはGoogle Play リファラ APIを導入しました。インストールリファラの情報を取得し、アトリビューションプロバイダがクリックインジェクション（不正な広告クリック）を阻止する上で、Google Play Storeインテントよりも信頼性が高く安全です。暫定的にGoogle Play StoreインテントはAPIと並行して存在しますが、将来的には廃止される予定です。アプリでこれをサポートすることを強く推奨します。 

Adjustのポストビルドプロセスにより、Google Play Storeインテントを受信します。いくつかの追加手順を実行するだけで、新しいGoogle Play リファラAPIをサポートできます。

Google Play リファラ APIのサポートを追加するには、Mavenリポジトリから[install referrer library][install-referrer-aar]をダウンロードし、AARファイルを`Plugins/Android` フォルダに入れてください。

#### <a id="qs-huawei-referrer-api"></a>HuaweiリファラAPI

v4.21.1以降より、Adjust SDKはHuawei App Galleryバージョン10.4以降のHuawei端末へのインストール計測をサポートしています。HuaweiリファラAPIの使用を開始するために連携手順を追加で設定する必要はありません。

### <a id="qs-post-build-process"></a>ポストビルドプロセス

アプリのビルドプロセスが完了するには、Adjust Unityのパッケージでポストビルドプロセスが実行されます。アプリ内でAdjust SDKが正しく動作するようにするためです。 

このプロセスは、`AdjustEditor.cs` の`OnPostprocessBuild` メソッドで実行されます。Unity IDEコンソールの出力ウィンドウに、ログメッセージが書き込まれます。

#### <a id="qs-post-build-ios"></a> iOSポストビルドプロセス

iOSポストビルドプロセスを適切に実行するためには、Unity 5以降を使用し、`iOS build support` をインストールしてください。iOSポストビルドプロセスは、生成したXcodeプロジェクト内で次のような変更を実行します。

- `iAd.framework` を追加します(Apple Search Adsのトラッキングに必要)
- `AdSupport.framework` を追加します(IDFAの読み取りに必要)
- `CoreTelephony.framework` を追加します（接続されているネットワーク機器の種類の読み取りに必要）
-その他のLinkerフラグ `-ObjC`を追加します(AdjustのObjective-Cカテゴリがビルド中に認識されるために必要)
-`Objective-C exceptions`を有効化します

iOS 14のサポート（`Assets/Adjust/Toggle iOS 14 Support`）を有効にした場合、iOSのポストビルドプロセスでは、Xcodeプロジェクトに2つのフレームワークが追加されます。

- `AppTrackingTransparency.framework` を追加します（ユーザーにトラッキングへの同意を求め、その同意に関するステータスを取得する必要がある）
- `StoreKit.framework` を追加します（SKAdNetworkフレームワークとの通信に必要）

#### <a id="qs-post-build-android"></a> Androidポストビルドプロセス

Androidポストビルドプロセスは、`Assets/Plugins/Android/`にある`AndroidManifest.xml` ファイルの変更を実行します。また、Android プラグインフォルダにAndroidManifest.xmlファイルがあるかどうかを確認します。ファイルがない場合は、互換性のあるマニフェストファイル`AdjustAndroidManifest.xml` からコピーを作成してください。`すでにAndroidManifest.xml` ファイルがある場合は、次の内容を確認し、変更してください。

- `INTERNET` のパーミッションを追加します (インターネット接続に必要)
- `ACCESS_WIFI_STATE` のパーミッションを追加します (Play Store経由でアプリを公開しない場合に必要)
- `ACCESS_NETWORK_STATE` のパーミッションを追加します（接続されているネットワーク機器の種類の読み取りに必要）
- `BIND_GET_INSTALL_REFERRER_SERVICE` のパーミッションを追加します (新しいGoogle install リファラAPI が機能するのに必要)
- Adjustのブロードキャストレシーバーを追加します (Google Play Storeインテント経由でインストールリファラ情報を取得するのに必要)。詳しくは、公式の[Android SDK README][android]を参照してください。 

**注意：**独自のブロードキャストレシーバを使用して`INSTALL_REFERRER`インテントを処理している場合には、manifest fileにAdjustブロードキャストレシーバを追加する必要はありません。これを削除し、代わりに独自のレシーバー内にAdjustブロードキャストレシーバへのコールを追加してください。詳しくは、[Androidガイド][android-custom-receiver]をご覧ください。

### <a id="qs-sdk-signature"></a>SDKシグネチャー

担当のアカウントマネージャーがAdjust SDKシグネチャーを有効化する必要があります。この機能を使用する場合は、Adjustのサポート（support@adjust.com）にお問い合わせください。

アカウントでSDKシグネチャーが有効になっており、Adjust管理画面のアプリシークレットにアクセスできる場合は、全てのシークレットパラメータ(`secretId`, `info1`, `info2`, `info3`, `info4`)を`AdjustConfig`インスタンスの`setAppSecret`メソッドに追加してください。

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId,info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

これでSDKシグネチャーがアプリに実装されました。 


## ディープリンク

### <a id="dl"> </a>ディープリンクの概要

**Adjustでは、iOSとAndroidのプラットフォームでのみディープリンクをサポートしています。**

ディープリンクを有効にした状態でAdjustトラッカーURLをご利用の場合、ディープリンクURLとそのコンテンツに関する情報を取得できます。同じURLをユーザーがクリックした際、ユーザーのデバイスにアプリがインストールされてる場合はスタンダードディープリンクが作動し、インストールされていない場合はディファードディープリンクが作動します。 

スタンダードディープリンクの場合、Androidプラットフォームにはディープリンクのコンテンツを取得できる仕組みがあります。ただし、ディファードディープリンクに対する自動サポートはありません。ディファードディープリンクのコンテンツを取得するには、Adjust SDKを使用してください。

ディープリンクの実装は**ネイティブレベル**で設定する必要があります。Xcodeプロジェクト(iOS向け)やAndroid Studio / Eclipseプロジェクト(Android向け)で設定してください。

### <a id="dl-standard"> </a>スタンダードディープリンク

スタンダードディープリンクに関する情報は、Unity C#コードでは配信できません。ディープリンクの取り扱いをアプリで有効化すると、ネイティブレベルでディープリンクの情報が得られます。[Android](#dl-app-android)アプリや[iOS](#dl-app-ios)アプリでディープリンクを有効化する方法は、こちらのとおりです。

### <a id="dl-deferred"> </a>ディファードディープリンク

ディファードディープリンクのコンテンツ情報を取得するには、`AdjustConfig` オブジェクトにcallbackメソッドを設定してください。これにより、URLのコンテンツが配信される`1つのstring`パラメータを受信します。メソッド `setDeferredDeeplinkDelegate`をコールしてconfigオブジェクトでこのメソッドを実装してください。

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

<a id="deeplinking-deferred-open"> </a>ディファードディープリンクでは、`AdjustConfig` オブジェクトで設定できる追加機能があります。Adjust SDKがディファードディープリンクの情報を取得すると、SDK側でそのURLを開くかどうかを決めることができます。このオプションを設定するには、configオブジェクトで `setLaunchDeferredDeeplink`メソッドをコールしてください。

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

何も設定されていない場合、**Adjust SDKはデフォルトで常にディープリンクを起動します**。

アプリでディープリンクを有効化するには、サポートするプラットフォームごとにスキームを設定してください。

### <a id="dl-app-android"></a>Androidアプリでのディープリンク処理

ネイティブレベルでAndroidアプリのディープリンクを設定するには、公式[Android SDK README][android-deeplinking]の手順をご確認ください。

この設定はネイティブのAndroid Studio もしくは Eclipseプロジェクトで行ってください。

### <a id="dl-app-ios"></a>iOSアプリでのディープリンク処理

**この設定はネイティブのXcodeプロジェクトで行ってください。**

ネイティブレベルでiOSアプリのディープリンクを設定するには、ネイティブのXcodeプロジェクトを使用し、公式[iOS SDK README][ios-deeplinking]の手順をご確認ください。

### イベントトラッキング

### <a id="et-tracking"></a>イベントのトラッキング

Adjustを使ってアプリ内のイベントをトラッキングすることができます。例えば、ボタンのタップを毎回トラッキングされたい場合は、管理画面の[create a new event token](https://help.adjust.com/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adjust-dashboard)にてイベントトークンを作成します。仮にそのイベントトークンを`abc123`とします。クリックをトラッキングするため、ボタンのクリックハンドラーメソッドに以下のような記述を追加します。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

### <a id="et-revenue"></a>収益（課金）のトラッキング

ユーザーがアプリ内でアイテムを購入をした際に収益が発生する場合は、それらを課金イベントとしてトラッキングできます。例えば、1回のタップで1ユーロセントの収益があると仮定すると、その収益イベントは以下のようになります。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01,"EUR");
Adjust.trackEvent(adjustEvent);
```

通貨トークンを設定すると、Adjustはopenexchange APIを使用して、計測された売り上げを設定されたレポート通貨に自動換算します。[通貨換算についての詳細はこちらをご覧ください](http://help.adjust.com/tracking/revenue-events/currency-conversion)

アプリ内購入をトラッキングする場合は、購入手続きが完了し、アイテムが購入された場合にのみ`trackEvent`をコールするようにしてください。こうすることで、実際には発生しなかった収益をトラッキングするのを防ぐことができます。


### <a id="et-revenue-deduplication"></a>収益の重複排除

収益を重複してトラッキングするのを防止するため、随意でトランザクションIDを追加することができます。SDKに最新10件のトランザクションIDが記憶され、トランザクションIDが重複する場合、その収益イベントは除外されます。これは、アプリ内購入のトラッキングに特に便利です。 

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01,"EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

### <a id="et-purchase-verification"></a>アプリ内購入の検証

Adjustのサーバーサイドのレシート検証ツール[Adjust's Purchase Verification][unity-purchase-sdk]を使って、アプリ内購入を確認できます。  

## カスタムパラメータ

### <a id="cp"></a>カスタムパラメータの概要

Adjust SDKがデフォルトで収集するローデータに加えて、Adjust SDKを使用してカスタム値(ユーザーID、製品IDなど)を必要な数だけトラッキングし、イベントまたはセッションに追加できます。カスタムパラメータはローデータとして転送されます。Adjust管理画面には**表示されません**。

[コールバックパラメータ](https://help.adjust.com/en/manage-data/export-raw-data/callbacks/best-practices-callbacks)で収集したデータは社内用に、パートナーパラメータは、外部のパートナー（アドネットワークなど）に対して共有する場合に使用してください。値(製品IDなど)を社内と外部のパートナーのためにトラッキングする場合、コールバックとパートナーパラメータの両方を使用するよう推奨します。

### <a id="cp-event-parameters"> </a>イベントパラメータ

### <a id="cp-event-callback-parameters"> </a>イベントコールバックパラメータ

[管理画面]に表示されたイベントにコールバックURLを登録すると、該当イベントが発生する度にそのURLに対してGETリクエストが送信されます。オブジェクトにキーと値の組を入れ、それを `trackEvent`メソッドに渡すこともできます。カスタムパラメータはコールバックURLに追加されます。

例えば、コールバックURLに `http://www.example.com/callback`と登録した場合、次のようにイベントをトラッキングします。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key","value");
adjustEvent.addCallbackParameter("foo","bar");

Adjust.trackEvent(adjustEvent);
```

この場合、Adjustは以下のGETリクエストを送信します。

```
http://www.example.com/callback?key=value&foo=bar
```

Adjustは数多くのプレースホルダー(パラメータ)をサポートしています。例えば広告IDを取得する際、iOSは`{idfa}` 、Androidは`{gps_adid}` がパラメータとなります。この例を使用して、得られるコールバック内でプレースホルダーを現在のデバイスのIDFAやgps_adid(Google Play開発者サービス 広告ID)に置き換えます。詳しくは[リアルタイムコールバック](https://help.adjust.com/en/manage-data/export-raw-data/callbacks)と[プレースホルダー](https://partners.adjust.com/placeholders/)の全リストをご覧ください。 

**注：**Adjustは、カスタムパラメータを保存せず、コールバックへの追加だけを行います。イベントにコールバックが登録されていない場合は、これらのパラメータを取得しません。


### <a id="cp-event-partner-parameters"></a>イベントパートナーパラメータ

管理画面でパラメータを有効にすると、ネットワークパートナーにパラメータが送信されます。[モジュールパートナー](https://docs.adjust.com/ja/special-partners/)やそれらの拡張連携についてご覧ください。

これはコールバックパラメータと同様に機能します。`AdjustEvent`インスタンスで` addPartnerParameter`メソッドをコールして追加してください。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key","value");
adjustEvent.addPartnerParameter("foo","bar");

Adjust.trackEvent(adjustEvent);
```

スペシャルパートナーとの連携方法の詳細については、[スペシャルパートナーガイド][スペシャルパートナー]をご覧ください。

### <a id="cp-event-callback-id"></a>イベントコールバックID

トラッキングしたいイベントごとにカスタマイズしたストリングIDを追加できます。イベントコールバック時にこのIDを報告し、どのイベントが正常にトラッキングされたかを知らせます。`AdjustEvent` インスタンスで`setCallbackId` メソッドをコールして、IDを設定してください。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="cp-session-parameters"></a>セッションパラメータ

セッションパラメータはローカルに保存され、Adjust SDK **イベントとセッション**ごとに送信されます。これらのパラメータは、追加される度に保存されます(したがって、再度追加する必要はありません)。同じパラメータを再度追加しても、何も起こりません。

Adjust SDKの起動前にセッションパラメータを送信することが可能です。したがって、[SDK ディレイスタート](#cp-delay-start)を使用すると、追加の値(アプリのサーバーからの認証トークンなど)を取得することができ、SDKの初期化タイミングで全ての情報を一度に送信できます。 

### <a id="cp-session-callback-parameters"></a>セッションコールバックパラメータ

Adjust SDKのセッションごとに送信されるイベントコールバックパラメータを保存できます。

セッションコールバックパラメータの仕組みは、イベントコールバックパラメータのインターフェイスに似ています。キーとその値をイベントに追加する代わりに、`Adjust` インスタンスの `addSessionCallbackParameter` メソッドへのコールで追加されます。

```cs
adjustEvent.addCallbackParameter("foo", "bar");
```

セッションコールバックパラメータは、イベントコールバックパラメータに統合されます。全ての情報が1つにまとめられて送信されますが、イベントコールバックパラメータは、セッションコールバックパラメータより優先されます。セッションコールバックパラメータと同じキーを持つイベントパートナーパラメータが追加された場合、イベントに追加されたパラーメータの値が表示されます。

`Adjust` インスタンスのremoveSessionCallbackParameter` メソッドに指定のキーを渡すことで、特定のセッションコールバックパラメータを削除できます。

```cs
AdjustEvent.addCallbackParameter("foo", "bar");
```

セッションコールバックパラメータから全てのキーと値を削除するには、`Adjust` インスタンスの`resetSessionCallbackParameters` メソッドを使ってリセットすることができます。

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>セッションパートナーパラメータ

SDKをトリガーするイベントやセッションごとに[セッションコールバックパラメータ](#cp-session-callback-parameters)が送信されるのと同様に、セッションパートナーパラメータも用意されています。

これらは[管理画面]で連携を有効化した全てのネットワークパートナーに送信されます。

セッションコールバックパラメータの仕組みは、イベントコールバックパラメータに似ています。キーとその値をイベントに追加する代わりに、`Adjust` インスタンスの `addSessionPartnerParameter` メソッドへのコールで追加されます。

```cs
adjustEvent.addCallbackParameter("foo", "bar");
```

セッションパートナーのパラメータは、イベントパートナーのパラメータに統合されます。ただし、イベントパートナーのパラメータは、セッションパートナーのパラメータより優先されます。セッションパートナーパラメータと同じキーを持つイベントパートナーパラメータが追加された場合、イベントに追加されたパラメータの値が表示されます。

Adjust` インスタンスの`removeSessionPartnerParameter` メソッドに指定のキーを渡すことで、特定のセッションパートナーパラメータを削除できます。

```cs
Adjust.removeSessionPartnerParameter("foo");
```

セッションパートナーパラメータから全てのキーと値を削除するには、`Adjust` インスタンスの`resetSessionPartnerParameters` メソッドを使ってリセットすることができます。

```cs
Adjust.resetSessionPartnerParameters();
```

＃＃＃<a id="cp-delay-start"></a>ディレイスタート

Adjust SDKのスタート（初期化）を遅らせると、アプリがユニークIDなどのセッションパラメータを取得しインストール時にコールバックとして送信する時間を確保できます。

`AdjustConfig` インスタンスの` setDelayStart` メソッドで、遅らせる時間を秒単位で設定してください。

```cs
adjustConfig.setDelayStart(5.5);
```

この例では、Adjust SDKは初期化時のインストールセッションと新しいイベントを5.5秒間遅らせます。5.5秒後に(または、その時間内に `Adjust.sendFirstPackages()` がコールされると)、全てのセッションパラメータがディレイインストールセッションとイベントに追加され、Adjust SDKは通常どおり作動します。

ディレイスタートに設定できる最長時間は10秒です。

##追加機能

Adjust SDKをプロジェクトに導入すると、次の追加機能を利用できます。

### <a id="ad-att-framework"></a>AppTrackingTransparencyフレームワーク

**注**：この機能が存在するのはiOSプラットフォームのみです。

各パッケージが送信されるたびに、Adjustのバックエンドは、アプリ関連データへのアクセスに関するユーザーの許諾状況を表す、以下の4つの値のいずれかを受信します。

- Authorized（承認）
- Denied（拒否）
- Not Determined（未決定）
- Restricted（制限あり）

デバイスがアプリ関連データへのアクセスに対するユーザーの許諾状況の承認リクエスト（ユーザーのデバイストラッキングに使用）を受信した後は、返されるステータスはAuthorizedあるいはDeniedになります。

デバイスがアプリ関連データへのアクセスの承認リクエスト（ユーザーあるいはデバイスのトラッキングに使用）を受信する前は、返されるステータスはNot Determinedになります。

アプリのトラッキングデータの使用が制限されている場合は、返されるステータスはRestrictedになります。

表示されるポップアップダイアログのカスタマイズを希望しない場合のために、このSDKには、ユーザーがポップアップダイアログに応答した後に、更新ステータスを受信するメカニズムが組み込まれています。新しい許諾ステータスをバックエンドに簡単かつ効率的に伝達するために、Adjust SDKはアプリのトラッキング承認メソッドのラッパーを提供しています。次の項目の説明をご覧ください。

### <a id="ad-ata-wrapper"></a>アプリトラッキング承認ラッパー(App-tracking authorisation wrapper)

**注**：この機能が存在するのはiOSプラットフォームのみです。

Adjust SDKは、アプリトラッキング承認ラッパーを使用して、アプリ関連データへのアクセスに対するユーザーの許諾状況をリクエストすることができます。Adjust SDKには、[requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc)メソッドに基づいて構築されたラッパーが用意されており、ユーザーの選択についての情報を取得するためのコールバックメソッドを定義することもできます。また、このラッパーを使用することで、ユーザーがポップアップダイアログに応答すると、その内容がコールバックメソッドで直ちに伝達されます。SDKは、ユーザーの選択をバックエンドにも通知します。「NSUInteger」の値はコールバックメソッドによって伝達されます。値の意味は次のとおりです。

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`（承認ステータスは「未決定」）
- 1: `ATTrackingManagerAuthorizationStatusRestricted`（承認ステータスは「制限あり」）
- 2: `ATTrackingManagerAuthorizationStatusDenied`（承認ステータスは「拒否」）
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`（承認ステータスは「承認」）

このラッパーを使用するためには、次のように呼び出してください。

```csharp
Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
{
    switch (status)
    {
        case 0:
            // ATTrackingManagerAuthorizationStatusNotDetermined の場合
            break;
        case 1:
            // ATTrackingManagerAuthorizationStatusRestricted の場合
            break;
        case 2:
            // ATTrackingManagerAuthorizationStatusDenied の場合
            break;
        case 3:
            // ATTrackingManagerAuthorizationStatusAuthorizedの場合
            break;
    }
});
```

### <a id="ad-skadn-framework"></a>SKAdNetworkフレームワーク

**注**：この機能が存在するのはiOSプラットフォームのみです。

Adjust iOS SDK v4.23.0以上を実装済みであり、アプリがiOS14で実行されている場合、SKAdNetworkとの通信はデフォルトでONに設定されますが、選択によりOFFにすることもできます。ONに設定すると、SDKの初期化時にSKAdNetworkのアトリビューションがAdjustによって自動的に登録されます。conversion value（コンバージョン値）を受信するためにAdjust管理画面でイベントを設定する場合、conversaion valueのデータはAdjustバックエンドからSDKに送信されます。その後、SDKによってconversion valueが設定されます。SKAdNetworkコールバックデータをAdjustで受信した後、このデータが管理画面に表示されます。

Adjust SDKがSKAdNetworkと自動的に通信しないようにしたい場合は、設定オブジェクトで次のメソッドを呼び出すことによって通信を無効化できます。

```csharp
adjustConfig.deactivateSKAdNetworkHandling();
```

### <a id="ad-push-token"></a>Pushトークン(アンインストールトラッキング)

Pushトークンは、オーディエンスビルダーやコールバックに使用されます。また、アンインストールや再インストールのトラッキングにも必要です。

Push通知トークンをAdjustに送信するには、アプリのPush通知トークンの取得時に(またはその値の変更のたびに)、`Adjust` インスタンスで`setDeviceToken` メソッドをコールしてください。

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>アトリビューションコールバック

アトリビューション情報の変更通知をアプリ内で受けるようにコールバックを設定できます。アトリビューションには複数の流入元が存在するため、この情報は非同期的に送ります。サードパーティに対してデータを共有する際は、[アトリビューションデータに関するポリシー] [attribution_data]を必ずご確認ください。 

アプリにこのコールバックを追加するには、次の手順に進んでください。

1.デリゲート `Action<AdjustAttribution>`の署名でメソッドを作成します。

2. `AdjustConfig` オブジェクトを作成し、前に作成したメソッドで`adjustConfig.setAttributionChangedDelegate`をコールしてください。同じ署名でラムダ式を使うことも可能です。

3. `Adjust.prefab` を使う代わりに、`Adjust.cs`スクリプトが別の`GameObject` に追加されている場合は、その``GameObject` の名前をAdjustConfig.setAttributionChangedDelegate`の2番目のパラメータとして必ず渡してください。

コールバックが`AdjustConfig` インスタンスを使用しているため、`Adjust.start`をコールする前に`adjustConfig.setAttributionChangedDelegate` をコールする必要があります。

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

このコールバック関数は、SDKが最後のアトリビューションデータを取得した時に作動します。コールバック関数内で`attribution` パラメータを確認することができます。プロパティの概要は次のとおりです。

- `string trackerToken` 最新アトリビューションのトラッカートークン
- `string trackerName` 最新アトリビューションのトラッカー名
- `string network` 最新アトリビューションのネットワークのグループ階層
- `string campaign` 最新アトリビューションのキャンペーンのグループ階層
- `string adgroup` 最新アトリビューションのアドグループのグループ階層
- `string creative` 最新アトリビューションのクリエイティブのグループ階層
- `string clickLabel` 最新アトリビューションのクリックラベル
- `string adid` AdjustユニークID

### <a id="ad-ad-revenue"></a>広告収益のトラッキング

Adjust SDKを利用して、以下のメソッドを呼び出し広告収益情報をトラッキングできます。

```csharp
Adjust.trackAdRevenue(source, payload);
```

Adjust SDKに渡す必要があるメソッドパラメータは次のとおりです。

- `source` - 広告収益情報のソースを指定する`string`オブジェクト
- `payload` -`広告収益のJSONをstring形態で格納する`string`オブジェクト

現在Adjustでは以下の`source` パラメータ値のみをサポートしています。

- `AdjustConfig.AdjustAdRevenueSourceMopub` は、[メディエーションプラットフォームのMoPub mediation][sdk2sdk-mopub]を示します。

### <a id="ad-subscriptions"></a>サブスクリプション計測

**注**：この機能はSDK v4.22.0以降のみ利用可能です。

App StoreとPlay Storeのサブスクリプションをトラッキングし、それぞれの有効性をAdjust SDKで確認できます。サブスクリプションの購入が完了したら、次のようにAdjust SDKを呼び出します。

**App Storeサブスクリプションの場合：**

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

**Play Storeサブスクリプションの場合：**

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

App Storeサブスクリプションのためのサブスクリプション トラッキング パラメータ：

- [price](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- currency（[priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc)オブジェクトの[currencyCode](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc)を渡す必要がある）
- [transactionId](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- [receipt](https://developer.apple.com/documentation/foundation/nsbundle/1407276-appstorereceipturl)
- [transactionDate](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- salesRegion（[priceLocale](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc)オブジェクトの[countryCode](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc)を渡す必要がある）

Play Storeサブスクリプションのサブスクリプション トラッキング パラメータ：

- [price](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [currency](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [orderId](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [signature](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [purchaseToken](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [purchaseTime](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

**注：** Adjust SDKが提供するサブスクリプション計測APIでは、全てのパラメータが `string` 値として渡されることを想定しています。上記で説明したパラメータは、サブスクリプションをトラッキングする前にAPIがサブスクリプションオブジェクトに渡すように要求するパラメータです。Unityにはアプリ内購入で取り扱っている様々なライブラリがありますが、各ライブラリはサブスクリプションの購入が正常に完了した時点で、上記の情報を何らかの形で返す必要があります。アプリ内課金で使用しているライブラリから取得したレスポンスの中で、これらのパラメータが配置されている場所を特定し、その値を抽出して文字列値としてAdjust APIに渡す必要があります。

イベント計測と同様に、コールバックやパートナーのパラメータをサブスクリプションオブジェクトに付与できます。

**App Storeサブスクリプションの場合：**

```csharp
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

// コールバックパラメータの追加
subscription.addCallbackParameter("key","value");
subscription.addCallbackParameter("foo","bar");

// パートナーパラメータの追加
subscription.addPartnerParameter("key","value");
subscription.addPartnerParameter("foo","bar");

Adjust.trackAppStoreSubscription(subscription);
```

**Play Storeサブスクリプションの場合：**

```csharp
AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
    price,
    currency,
    sku,
    orderId,
    signature,
    purchaseToken);
subscription.setPurchaseTime(purchaseTime);

// コールバックパラメータの追加
subscription.addCallbackParameter("key","value");
subscription.addCallbackParameter("key","bar");

// パートナーパラメータの追加
subscription.addPartnerParameter("key","value");
subscription.addPartnerParameter("foo","bar");

Adjust.trackPlayStoreSubscription(subscription);
```

### <a id="ad-session-event-callbacks"></a>セッションとイベントのコールバック

イベントとセッションの両方、もしくはどちらかを計測し、成功か失敗かの通知を受け取れるようコールバックを設定できます。

トラッキングに成功したイベントへのコールバック関数を次のように追加してください。

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

トラッキングに失敗したイベントへのコールバック関数を次のように追加してください。

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

トラッキングに成功したセッションの場合

```cs
// ...

AdjustConfig adjustConfig = new AdjustConfig("{Your App Token}", AdjustEnvironment.Sandbox);
adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);

Adjust.start(adjustConfig);

// ...

public void EventSuccessCallback(AdjustEventSuccess eventSuccessData) {
    // ...
}
```

トラッキングに失敗したセッションの場合

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

コールバック関数は、SDKがサーバーにパッケージ送信を試みた後でコールされます。コールバック内でコールバック用のレスポンスデータオブジェクトを確認することができます。セッションのレスポンスデータのプロパティ概要は次のとおりです。

- `string Message` サーバーからのメッセージまたはSDKのエラーログ
- `string Timestamp` サーバーからのタイムスタンプ
- `string Adid` Adjustが提供するユニークデバイスID
-`Dictionary JsonResponse` サーバーからのレスポンスのJSONオブジェクト

どちらのイベントレスポンスデータオブジェクトも以下を含みます。

- `string EventToken` トラッキングしたパッケージがイベントだった場合、そのイベントトークン
- `string CallbackId` イベントオブジェクトにカスタム設定されたコールバックID

失敗したイベントとセッションのオブジェクトには以下を含みます。

- `bool WillRetry` 後に再送を試みる予定であるかどうかを示します。

### <a id="ad-user-attribution"></a>ユーザーアトリビューション

アトリビューションコールバックと同様に、このコールバックは、アトリビューション値に変化があるたびに作動します。ユーザーの現在のアトビリューション値に関する情報にアクセスする場合は`Adjust`インスタンスの次のメソッドをコールしてください。

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**注**：現在のアトリビューションに関する情報は、バックエンドがアプリのインストールをトラッキングして、アトリビューションコールバックがトリガーされた後でのみ利用できます。SDKが初期化され最初のアトリビューションコールバックが作動する前にユーザーのアトリビューション値にアクセスすることはできません。

### <a id="ad-device-ids"></a>デバイスID

Adjust SDKがデバイスIDを受信するように設定できます。

### <a id="ad-idfa">iOS広告ID

IDFAを取得するには、`Adjust` インスタンスの関数`getIdfa` `をコースしてください。

```cs
string idfa = Adjust.getIdfa();
```

### <a id="ad-gps-adid"></a>Google Play 開発者サービス広告ID

Google 広告ID(gps_adid)は、バックグラウンドでのスレッドでしか読み込みできないという制約があります。Adjust` インスタンスの`getGoogleAdId` メソッドを`Action<string>` デリゲートでコールすると、この条件以外でも取得できるようになります。

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

Google 広告 IDを変数 `googleAdId`変数として利用できます。

### <a id="ad-amazon-adid"></a>Amazon広告ID

Amazon広告 IDを取得する必要がある場合は、`Adjust` インスタンスで`getAmazonAdId`メソッドをコールしてください。

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="ad-adid"></a>AdjustデバイスID

Adjustのバックエンドでは、アプリがインストールされている各デバイスに対してユニークなAdjustデバイスID(`adid` )を生成します。このIDを取得するには、`Adjust` インスタンスの以下のこのメソッドをコールしてください。

```cs
String adid = Adjust.getAdid();
```

adidに関する情報は、Adjustのバックエンドがアプリのインストールを計測した後でのみ利用できます。Adjust SDKが初期化され、アプリのインストールが正常に計測されrまで、adidにアクセスすることができません。

### <a id="ad-pre-installed-trackers"></a>プリインストールトラッカー

Adjust SDKで、アプリがプリインストールされている（端末購入時にあらかじめインストールされている状態）デバイスのユーザーを認識するには、次の手順で設定を行なってください。

1. [管理画面]で新しいトラッカーを作成してください。
2. `AdjustConfig`のデフォルトトラッカーを設定してください。

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  Adjust.start(adjustConfig);
  ```

  `{TrackerToken}`にステップ2で作成したトラッカートークン（たとえば`{abc123}`）を入れてください。
  
管理画面にはトラッカーURL(`http://app.adjust.com/`など)が表示されますが、ソースコード内には、このURL全体ではなく、6文字または7文字のトークンのみを入力してください。

3. アプリをビルドして実行してください。ログ出力に次のような行が表示されるはずです。

    ```
    Default tracker: 'abc123'
    ```

### <a id="ad-offline-mode"></a>オフラインモード

オフラインモードは、後から送信する計測データを保持しつつ、Adjustサーバーへの送信を一時停止します。Adjust SDKがオフラインモード中の場合、全ての情報がファイルに保存されるため、イベントを多く発生させすぎないようご注意ください。

`true`パラメータで`setOfflineMode` を呼び出すと、オフラインモードを有効にできます。

```cs
Adjust.setOfflineMode(true);
```

`false`パラメータで`setOfflineMode` with `false` を呼び出すと、オフラインモードを解除できます。Adjust SDKがオンラインモードに戻った時、保存されていた情報は正しいタイムスタンプでAdjustのサーバーに送信されます。

この設定はセッション間で記憶されません。つまり、オフラインモード時にアプリを終了しても、次にSDKが起動した時にはオンラインモードとしてアプリが起動します。

### <a id="ad-disable-tracking"></a>トラッキングの無効化

` setEnabled`に`false`パラメータを渡すことで、Adjust SDKが行うデバイスアクティビティの計測を全て無効にできます。この設定はセッション間で記憶されますが、最初のセッションの後でしか有効化できません。

```cs
Adjust.setEnabled(false);
```

Adjust SDKが現在有効かどうかは、 `isEnabled`メソッドで確認できます。`enabled` パラメータを`true`に設定して `setEnabled`を呼び出すと、Adjust SDKをいつでも有効にできます。

### <a id="ad-event-buffering"></a>イベントバッファリング

アプリでイベント計測を多用している場合は、一部のネットワークリクエストを遅らせて、1分間ごとにまとめて送信したほうがいい場合があります。`AdjustConfig` インスタンスでイベントのバッファリングを有効にしてください。

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

何も設定されていない場合、イベントバッファリングはデフォルトで無効になっています。

### <a id="ad-background-tracking"></a>バックグラウンドでのトラッキング

Adjust SDKは、アプリがバックグラウンドにある間は、ネットワークリクエストをデフォルトで一時停止するようになっています。この設定は`AdjustConfig` インスタンスで変更できます。

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR 忘れられる権利

EUの一般データ保護規制(GDPR)第17条に基づいて、ユーザーが「忘れられる権利（right to be forgotten）」を行使した場合は、Adjustに通知することができます。次のメソッドを呼び出して、ユーザーの申請をAdjustバックエンドに伝えるようAdjust SDKに指示してください。

```cs
```Adjust.gdprForgetMe();
```

この情報を受け取ると、Adjustは該当ユーザーのデータを消去し、Adjust SDKはユーザーのトラッキングを停止します。以降、そのデバイスからのリクエストはAdjustに送信されません。

テスト段階でこのメソッドを利用した場合も、消去処理は永続的なものであることに注意してください。元に戻すことはできません。


### <a id="ad-disable-third-party-sharing"></a>特定のユーザーに対してサードパーティーとの共有を無効にする

ユーザーがマーケティングを目的とするパートナーとのデータ共有を拒否する権利を行使した一方、統計目的でのデータ共有を許可した場合、Adjustに通知できるようになりました。 

次のメソッドを呼び出して、ユーザーの選択（データ共有を無効にする）をAdjustバックエンドに伝えるようAdjust SDKに指示してください。

```csharp
Adjust.disableThirdPartySharing();
```

この情報を受け取ると、Adjustは特定のユーザーに関してパートナーとのデータ共有をブロックし、Adjust SDKは通常通り機能します。

## テストとトラブルシューティング

### <a id="tt-debug-ios"></a>iOSデバッグ情報

ポストビルドスクリプトを使っていても、プロジェクトがすぐに使える状態で実行できるとは限りません。

必要であれば、dSYMファイルを無効化してください。`Project Navigator` から`Unity-iPhone` プロジェクトを選択し、`Build Settings` タブをクリックして`debug information` を検索してください。`Debug Information Format` または`DEBUG_INFORMATION_FORMAT` オプションがあるはずです。これを`DWARF with dSYM File` から`DWARF`に変更してください。


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

## ライセンス

### <a id="license"></a>ライセンス契約

ファイルmod_pbxproj.pyは、ApacheライセンスVer.2.0(以下「ライセンス」)に基づいて使用が許諾されています。
このファイルを使用するためには、本ライセンスの規約に従ってください。
本ライセンスのコピーはhttp://www.apache.org/licenses/LICENSE-2.0で入手できます。

Adjust SDKはMITライセンスを適用しています。

Copyright (c) 2012-2020 Adjust GmbH, http://www.adjust.com

以下に定める条件に従い、本ソフトウェアおよび関連文書のファイル(以下「ソフトウェア」) の複製を取得する全ての人に対し、
ソフトウェアを無制限に扱うことを無償で許可します。
これには、ソフトウェアの複製を
使用、複写、変更、マージ、掲載、流通、サブライセンス、および/または販売する権利、
また、ソフトウェアを提供する相手に同じ許可を与える
権利も無制限に含まれます。

上記の著作権表示ならびに本許諾表示を、ソフトウェアの全ての
複製または重要な部分に記載するものとします。

本ソフトウェアは「現状のまま」で、明示であるか暗黙であるかを問わず、何らの保証もなく提供されます。
ここでいう保証とは、商品性、特定の目的への適合性、および権利非侵害についての保証を含みますが、それに限定されたものではありません。
いかなる場合でも、
作者または著作権者は、契約行為、不法行為、またはそれ以外であろうと、ソフトウェアに起因または関連し、あるいはソフトウェアの使用
またはその他の取り扱いによって生じる一切の請求、損害、その他の義務について
何らの責任も負わないものと
します。
