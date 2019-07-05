## 概要
こちらはAdjustTMのUnity SDKです。iOS、Android、Windows Store 8.1、Windows Phone 8.1、およびWindows 10をサポートしています。AdjustTMについて
は、[adjust.com]をご覧ください。

**注**:バージョン**4.12.0**以降のAdjust Unity SDKは、**Unity 5以降**のバージョンと互換性があります。

Read this in other languages: [English][en-readme], [中文][zh-readme], [日本語][ja-readme], [한국어][ko-readme]

<section id='toc-section'>
</section>

## <a id="basic-integration"></a>基本的な連携方法

Adjust SDKをUnityプロジェクトに連携させる最少のステップをここでご紹介します。

### <a id="sdk-get"></a>SDKダウンロード

[リリースページ][releases]から最新バージョンをダウンロードしてください。

### <a id="sdk-add"></a>プロジェクトへのSDKの追加

Unityエディタでプロジェクトを開き`Assets → Import Package → Custom Package`と進み、ダウンロードしたUnityパッケージファイルを選択してください。

![][import_package]

### <a id="sdk-integrate"></a>アプリへのSDKの連携

`Assets/Adjust/Adjust.prefab`にあるプレハブを1番目のシーンに追加してください。

追加したプレハブの`Inspector menu`にて、Adjustスクリプトのパラメータを編集してください。

![][adjust_editor]

Adjustプレハブで以下のオプションの設定ができます。

* [手動スタート](#start-manually)
* [イベントバッファリング](#event-buffering)
* [バックグラウンドでの送信](#background-tracking)
* [ディファード・ディープリンク](#deeplinking-deferred-open)
* [Appトークン](#app-token)
* [ログレベル](#adjust-logging)
* [環境設定](#environment)

<a id="app-token">`{YourAppToken}`にアプリのトークンを入力してください。これは[ダッシュボード][dashboard]で確認できます。

<a id="environment">`Environment`に以下のどちらかを設定してください。これはテスト用アプリか本番用アプリかによって異なります。

```
'Sandbox'
'Production'
```

**重要** この値はアプリのテスト中のみ`Sandbox`に設定してください。アプリを提出する前に`Production`になっていることを必ず確認してください。再度開発やテストをする際は`Sandbox`に戻してください。

この変数は実際のトラフィックとテスト端末からのテストのトラフィックを区別するために利用されます。正しく計測するために、この値の設定には常に注意してください。収益のトラッキングの際には特に重要です。

<a id="start-manually">アプリの`Awake`イベントでAdjust SDKを起動したくない場合、`Start Manually`にチェックしてください。このオプションが選択されていると、ソースコード上でAdjust SDKの初期化および起動を行う必要があります。`AdjustConfig`オブジェクトをパラメータとして`Adjust.start`メソッドををコールし、Adjust SDKを起動してください。

例えばこれらのオプションなどのメニューボタンがあるシーンの場合、`Assets/Adjust/ExampleGUI/ExampleGUI.unity`にあるサンプルのシーンを開いてください。このシーンのソースは`Assets/Adjust/ExampleGUI/ExampleGUI.cs`にあります。

### <a id="adjust-logging"></a>Adjustログ

`Log Level`に設定するパラメータを変更することによって記録するログのレベルを調節できます。パラメータは以下の種類があります。

- `Verbose` - すべてのログを有効にする
- `Debug` - より詳細なログを記録する
- `Info` - デフォルト
- `Warn` - infoのログを無効にする
- `Error` - warningsを無効にする
- `Assert` - errorsを無効にする
- `Suppress` - すべてのログを無効にする

すべてのログ出力を非表示にしたい場合、Adjust SDKをソースコードから手動で初期化しているならば、ログレベルをSuppressにする以外にも`AdjustConfig`オブジェクトのコンストラクタを使う必要があります。これはSuppressログレベルがサポートされているかどうかを示すBoolean値のパラメータを取得します。

```cs
string appToken = "{YourAppToken}";
string environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

Windowsベースのターゲットの場合、コンパイル時のログを`released`モードでライブラリから見るには、`debug`モードでテストされている間のアプリへのログ出力をリダイレクトする必要があります。

SDKを起動する前に、`AdjustConfig`インスタンスの`setLogDelegate`メソッドをコールしてください。

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="google-play-services"></a>Google Playサービス

2014年8月1日以降、Google Playストア内のアプリはデバイスの特定のために[Google広告ID][google_ad_id]を使うことが必須とされています。Adjust SDKでGoogle広告IDを使うためには、[Google Playサービス][google_play_services]を連携させる必要があります。Google Playサービスの連携がお済みでない場合は、`google-play-services_lib`フォルダをUnityプロジェクトの`Assets/Plugins/Android`フォルダにコピーしてください。アプリのビルドをするとGoogle Playサービスが連携されます。

`google-play-services_lib`はAndroid SDKにも含まれていますので、すでにインストールされている場合もあります。

Android SDKをダウンロードするには主に2つの方法があります。`Android SDK Manager`が入ったツールをお使いの場合は、`Android SDK Tools`をダウンロードしてください。インストールすると、`SDK_FOLDER/extras/google/google_play_services/libproject/`フォルダ内にライブラリが入っています。

![][android_sdk_location]

Android SDK Managerの入ったツールをお使いでない場合、[公式ページ][android_sdk_download]からスタンドアローンバージョンのAndroid SDKをダウンロードしてください。ダウンロードすると、Android SDK Toolsを含まない基本バージョンのAndroid SDKを得ることになります。`SDK Readme.txt`という名前のGoogleのReadmeファイルにそれらのダウンロード方法について詳しく書かれています。これはAndroid SDKフォルダに入っています。

**更新** 新しいバージョンのAndroid SDKをインストールしている場合は、GoogleはSDKのルートフォルダ内のGoogle Playサービスフォルダの構成を変更しています。上述の構造は変更されており、新しいバージョンでは次のようになっています。

![][android_sdk_location_new]

以前のようにGoogle Playサービスの全ライブラリだけでなく、部分的なライブラリの箇所に個別にアクセスができるようになりました。これにより、Adjust SDKが必要とする部分のGoogle Playサービスのライブラリだけを追加すればいいことになります。`Assets/Plugins/Android`フォルダに`play-services-basement-x.y.z.aar`ファイルを追加するだけで、Adjust SDKに必要なGoogle Playサービスの連携は完了します。

**更新**: Google Play開発者サービスライブラリ (Google Play Services library) 15.0.0 より、Google は Google広告ID (Google Advertising Identifier) の取得に必要なクラスを、[`play-services-ads-identifier`](https://mvnrepository.com/artifact/com.google.android.gms/play-services-ads-identifier) パッケージに移動させました。Google Play 開発者サービスのバージョン 15.0.0 以上をご使用の場合、このパッケージをアプリに追加するようにしてください。Adjust は、Google広告IDに関するいくつかの矛盾点を認識していますが、これはご利用するUnity IDE のバージョンに伴います。使用中のバージョンや Google Play開発者サービスのdependencyブロックをどのようにアプリに追加したかに関わらず、**Google広告IDが Adjust SDK を使って正しく取得できるかどうかをテスト**するようお願いいたします。

Adjust SDK がGoogle広告IDを受け取っているかどうかを確認するためには、SDK をサンドボックス (`sandbox` ) モードで稼動させ、ログレベルをverboseに設定してください。その後、セッションまたはアプリの一部のイベントをトラキングし、セッションまたはイベントのトラックが完了したら、verboseログにある読み込み済みパラメーターを確認します。`gps_adid`と呼ばれる広告IDを表示するパラメータが存在すれば、Adjust SDK がGoogle広告IDを問題なく読み込んでいるということになります。

Google広告IDに関して問題やご質問がございましたら、AdjustのGithub リポジトリに質問を投稿するか、support@adjust.com までEメールでご連絡ください。

### <a id="android-proguard"></a>Proguardの設定

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
-keep public class com.android.installreferrer.** { *; }
```

### <a id="google-install-referrer"></a>Googleインストールリファラ‬

Adjustでは、Androidアプリのインストールをその流入元に正確にアトリビュートするため、**インストールリファラ**の情報を取得する必要があります。そのために**Google PlayリファラAPI**を利用するか、またはブロードキャストレシーバーで**Google Playストアのインテント**を取得してください。Adjustのポストビルドプロセスにより、Google Playストアのインテントを使用したシナリオは自動的にサポートされますが、新しいGoogle PlayリファラAPIのサポートを追加するには、ユーザー側で追加手順を行う必要があります。‬
 
‪**重要**：Google PlayリファラAPIは、インストールリファラの情報を信頼でき、安全な方法で提供し、またアトリビューションプロバイダがクリックインジェクションの不正に対抗するするためにGoogleが新たに導入したものです。アプリケーションでこれをサポートすることを**強く推奨**します。Google Playストアのインテントは、インストールリファラ情報を取得する上で安全性が低い方法です。当面、新しいGoogle PlayリファラAPIと並行して引き続き存在しますが、将来廃止される予定となっています。‬
 
‪そのサポートを追加するには、Mavenリポジトリから[install referrer library][install-referrer-aar]をダウンロードしてください。`Plugins/Android`フォルダーにAARファイルを保存すれば完了です。Adjustのポストビルドプロセスが、必要な`AndroidManifest.xml`の調整を行います。‬

### <a id="post-build-process"></a>ポストビルドプロセス‬
 
‪ビルドプロセスを促進し、Adjust SDKを適切に動作させるため、Adjust Unityパッケージでポストビルドプロセスが実行されます。‬
 
‪このプロセスは、`AdjustEditor.cs`の`OnPostprocessBuild`メソッドで実行されます。iOSポストビルドプロセスを適切に実行するには、`Unity 5以降`に`iOSビルドサポート`をインストールしておく必要があります。‬
 
‪このスクリプトがUnity IDEコンソール出力ウィンドウにログ出力メッセージを書き込みます。‬
 
#### <a id="post-build-ios"></a>iOSポストビルドプロセス‬
 
‪iOSポストビルドプロセスは、生成されたXcodeプロジェクトで次のような変更を実行します。‬

- `iAd.framework`を追加します（Apple Search Adsのトラッキングに必要）。
- `AdSupport.framework`を追加します（IDFAの読み取りに必要）。‬
- `CoreTelephony.framework`を追加します（MMCとMNCの読み取りに必要）。‬
- 別のリンカフラグ `-ObjC`を追加します（ビルド中にAdjust Objective-Cカテゴリーを認識するために必要）。‬
- `Objective-C exceptions`を有効にします。‬
 
#### <a id="post-build-android"></a>Androidポストビルドプロセス‬
 
‪Androidポストビルドプロセスは、`Assets / Plugins / Android /`にある`AndroidManifest.xml`ファイルの変更を実行します。‬
 
‪Androidポストビルドプロセスは、最初にAndroidプラグインフォルダー内に`AndroidManifest.xml`ファイルがあるかどうかを確認します。`Assets / Plugins / Android /`に`AndroidManifest.xml`ファイルがない場合は、互換性のあるマニフェストファイルの`AdjustAndroidManifest.xml`からコピーを作成します。すでに`AndroidManifest.xml`ファイルがある場合は、次の内容を確認して変更します。‬
- `INTERNET`のパーミッションを追加します（インターネット接続に必要）。‬
- `ACCESS_WIFI_STATE`のパーミッションを追加します（アプリをPlayストア経由で配布しない場合に必要）。
- `ACCESS_NETWORK_STATE`のパーミッションを追加します（MMCとMNCの読み取りに必要）。‬
- `BIND_GET_INSTALL_REFERRER_SERVICE`のパーミッションを追加します（新しいGoogleインストールリファラAPIの使用に必要）。‬
- Adjustブロードキャストレシーバーを追加します（Google Playストアのインテントを介してインストールリファラ情報を取得するために必要です）。詳細については、公式の[Android SDK README][android]をご覧ください。なお、**独自のブロードキャストレシーバー**を使用して`INSTALL_REFERRER`インテントを処理している場合には、マニフェストファイルにAdjustブロードキャストレシーバーを追加する必要はありません。それを削除し、[Android guide][android-custom-receiver]のように、独自のレシーバー内にAdjustブロードキャストレシーバーへのコールを追加します。‬

## <a id="additional-features"></a>追加機能

プロジェクトにAdjust SDKを連携させると、以下の機能をご利用できるようになります。

### <a id="event-tracking"></a>イベントトラッキング

Adjustを使ってアプリ内のイベントをトラッキングすることができます。ここではあるボタンのタップを毎回トラックしたい場合について説明します。[dashboard]にてイベントトークンを作成し、そのイベントトークンは仮に`abc123`というイベントトークンと関連しているとします。クリックをトラックするため、ボタンのクリックハンドラーメソッドに以下のような記述を追加します。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-tracking"></a>収益のトラッキング

広告をタップした時やアプリ内課金をした時などにユーザーが報酬を得る仕組みであれば、そういったイベントもトラッキングできます。1回のタップで1ユーロセントの報酬と仮定すると、報酬イベントは以下のようになります。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-deduplication"></a>収益の重複排除

報酬を重複してトラッキングすることを防ぐために、トランザクションIDを随意で設定することができます。最新の10のトランザクションIDが記憶され、重複したトランザクションIDの収益イベントは除外されます。これはアプリ内課金のトラッキングに特に便利です。下記に例を挙げます。

アプリ内課金をトラッキングする際は、トランザクションが終了しアイテムが購入された時のみ`trackEvent`をコールするようにしてください。こうすることで、実際には生成されない報酬をトラッキングすることを防ぐことができます。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

#### <a id="iap-verification"></a>アプリ内課金の検証

Adjustのサーバーサイドのレシート検証ツール、Purchase Verificationを使ってアプリ内で行われたアプリ内課金の妥当性を調べる際は、`Unity purchase SDK`をご利用ください。詳しくは[こちら][unity-purchase-sdk]をご覧ください。

#### <a id="callback-parameters"></a>コールバックパラメータ

[dashboard]でイベントにコールバックURLを登録することができます。イベントがトラッキングされるたびにそのURLにGETリクエストが送信されます。
この場合、オブジェクトにキーと値の組を入れ`trackEvent`メソッドに渡すこともできます。渡されたパラメータはコールバックURLに追加されます。

例えば、コールバックURLに`http://www.adjust.com/callback`を登録し、イベントトークンを`abc123`とした場合、イベントトラッキングは以下のようになります。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

この場合、Adjustはこのイベントをトラッキングし以下にリクエストが送られます。

```
http://www.adjust.com/callback?key=value&foo=bar
```

パラメータの値として使われることのできるプレースホルダーは、iOSの`{idfa}`やAndroidの`{gps_adid}`のような様々な形に対応しています。得られるコールバック内で、`{idfa}`プレースホルダーは該当デバイスの広告主用のIDに置き換えられ、`{gps_adid}`プレースホルダーは該当デバイスのGoogle PlayサービスIDに置き換えられます。独自に設定されたパラメータには何も格納しませんが、コールバックに追加されます。イベントにコールバックを登録していない場合は、これらのパラメータは使われません。

#### <a id="partner-parameters"></a>パートナーパラメータ

Adjustのダッシュボード上で連携が有効化されているネットワークパートナーに送信するパラメータを設定することができます。

これは上記のコールバックパラメータと同様に機能しますが、`AdjustEvent`インスタンスの`addPartnerParameter`メソッドをコールすることにより追加されます。

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

連携パートナーとその連携方法について詳しくは[連携パートナーガイド][special-partners]をご覧ください。

### <a id="callback-id"></a>コールバック ID
トラッキングしたいイベントにカスタムIDを追加できます。このIDはイベントをトラッキングし、成功か失敗かの通知を受け取けとれるようコールバックを登録することができます。このIDは`AdjustEvent`インスタンスの`setCallbackId`メソッドと呼ぶように設定できます：

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="session-parameters"></a>セッションパラメータ

いくつかのパラメータは、Adjust SDKのイベントごと、セッションごとに送信するために保存されます。このいずれかのパラメータを追加すると、これらはローカルに保存されるため、毎回追加する必要はありません。同じパラメータを再度追加しても何も起こりません。

これらのセッションパラメータはAdjust SDKが立ち上がる前にコールすることができるので、インストール時に送信を確認することもできます。インストール時に送信したい場合は、Adjust SDKの初回立ち上げを[遅らせる](#delay-start)ことができます。ただし、必要なパラメータの値を得られるのは立ち上げ後となります。

### <a id="session-callback-parameters"></a>セッションコールバックパラメータ

[イベント](#callback-parameters)で設定された同じコールバックパラメータを、Adjust SDKのイベントごとまたはセッションごとに送信するために保存することもできます。

セッションコールバックパラメータのインターフェイスとイベントコールバックパラメータは似ています。イベントにキーと値を追加する代わりに、`Adjust`メソッドの`addSessionCallbackParameter`へのコールで追加されます。

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

セッションコールバックパラメータは、イベントに追加されたコールバックパラメータとマージされます。イベントに追加されたコールバックパラメータは、セッションコールバックパラメータより優先されます。イベントに追加されたコールバックパラメータがセッションから追加されたパラメータと同じキーを持っている場合、イベントに追加されたコールバックパラメータの値が優先されます。

`removeSessionCallbackParameter`メソッドに指定のキーを渡すことで、特定のセッションコールバックパラメータを削除することができます。

```cs
Adjust.removeSessionCallbackParameter("foo");
```

セッションコールバックパラメータからすべてのキーと値を削除したい場合は、`resetSessionCallbackParameters`メソッドを使ってリセットすることができます。

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="session-partner-parameters"></a>セッションパートナーパラメータ

Adjust SDKのイベントごとやセッションごとに送信される[セッションコールバックパラメータ](#session-callback-parameters)があるように、セッションパートナーパラメータも用意されています。

これらはネットワークパートナーに送信され、Adjust[ダッシュボード][dashboard]で有効化されている連携のために利用されます。

セッションパートナーパラメータのインターフェイスとイベントパートナーパラメータは似ています。イベントにキーと値を追加する代わりに、`Adjust`メソッドの`addSessionPartnerParameter`へのコールで追加されます。

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

セッションパートナーパラメータはイベントに追加されたパートナーパラメータとマージされます。イベントに追加されたパートナーパラメータは、セッションパートナーパラメータより優先されます。イベントに追加されたパートナーパラメータがセッションから追加されたパラメータと同じキーを持っている場合、イベントに追加されたパートナーパラメータの値が優先されます。

`removeSessionPartnerParameter`メソッドに指定のキーを渡すことで、特定のセッションパートナーパラメータを削除することができます。

```cs
Adjust.removeSessionPartnerParameter("foo");
```

セッションパートナーパラメータからすべてのキーと値を削除したい場合は、`resetSessionPartnerParameters`メソッドを使ってリセットすることができます。

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="delay-start"></a>ディレイスタート

Adjust SDKのスタートを遅らせると、ユニークIDなどのセッションパラメータを取得しインストール時に送信できるようにすることができます。

`AdjustConfig`インスタンスの`setDelayStart`メソッドで、遅らせる時間を秒単位で設定できます。

```cs
adjustConfig.setDelayStart(5.5);
```

この場合、Adjust SDKは最初のインストールセッションと生成されるイベントを初めの5.5秒間は送信しません。この時間が過ぎるまで、もしくは`Adjust.sendFirstPackages()`がコールされるまで、セッションパラメータはすべてディレイインストールセッションとイベントに追加され、Adjust SDKは通常通り再開します。

**Adjust SDKのディレイスタートは最大で10秒です**。

### <a id="attribution-callback"></a>アトリビューションコールバック

トラッカーのアトリビューション変化の通知を受けるために、コールバックを登録することができます。アトリビューションには複数のソースがあり得るため、この情報は同時に送ることができません。アプリにこのコールバックを実装するには次の手順に進んでください。

[アトリビューションデータに関するポリシー][attribution_data]を必ずご確認ください。

1. デリゲート`Action<AdjustAttribution>`の署名を使ってメソッドを作成してください。

2. `AdjustConfig`オブジェクトの作成後、上で作成したメソッドで`adjustConfig.setAttributionChangedDelegate`をコールしてください。同じ署名でラムダ式を使うことも可能です。

3. `Adjust.prefab`を使う代わりに、the `Adjust.cs`スクリプトが他の`GameObject`に追加されている場合は、その`GameObject`の名前を`AdjustConfig.setAttributionChangedDelegate`の2番目のパラメータとして必ず追加してください。

コールバックがAdjustConfigインスタンスを使っているため、`Adjust.start`をコールする前に`adjustConfig.setAttributionChangedDelegate`をコールする必要があります。

このコールバック関数は、SDKが最後のアトリビューションデータを取得した時に作動します。コールバック関数内で`attribution`パラメータを確認することができます。このパラメータのプロパティの概要は以下の通りです。

- `string trackerToken` 最新アトリビューションのトラッカートークン
- `string trackerName` 最新アトリビューションのトラッカー名
- `string network` 最新アトリビューションのネットワークのグループ階層
- `string campaign` 最新アトリビューションのキャンペーンのグループ階層
- `string adgroup` 最新アトリビューションのアドグループのグループ階層
- `string creative` 最新アトリビューションのクリエイティブのグループ階層
- `string clickLabel` 最新アトリビューションのクリックラベル
- `string adid` AdjustのデバイスID

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

### <a id="ad-revenue"></a>広告収益の計測

Adjust SDKを利用して、以下のメソッドを呼び出し広告収益情報を計測することができます。

```csharp
Adjust.trackAdRevenue(source, payload);
```

Adjust SDKにパスするメソッドの引数は以下の通りです。

- `source` - 広告収益情報のソースを指定する`string`オブジェクト
- `payload` - 広告収益のJSONを格納する`string`オブジェクト

現在、弊社は以下の`source`パラメータの値のみ対応しています。

- `AdjustConfig.AdjustAdRevenueSourceMopub` - メディエーションプラットフォームのMoPubを示します。（詳細は、[統合ガイド][sdk2sdk-mopub]を参照ください）

### <a id="session-event-callbacks"></a>セッションとイベントのコールバック

イベントとセッションの双方もしくはどちらかをトラッキングし、成功か失敗かの通知を受け取れるようリスナを登録することができます。

成功したイベントへのコールバック関数を以下のように実装してください。

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

失敗したイベントへは以下のように実装してください。

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

同様に、成功したセッション

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

失敗したセッション

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

コールバック関数はSDKがサーバーにパッケージ送信を試みた後で呼ばれます。コールバック内でコールバック用のレスポンスデータオブジェクトを確認することができます。レスポンスデータのプロパティの概要は以下の通りです。

- `string Message` サーバーからのメッセージまたはSDKのエラーログ
- `string Timestamp` サーバーからのタイムスタンプ
- `string Adid` Adjustから提供されるユニークデバイスID
- `Dictionary<string, object> JsonResponse` サーバーからのレスポンスのJSONオブジェクト

イベントのレスポンスデータは以下を含みます。

- `string EventToken` トラッキングされたパッケージがイベントだった場合、そのイベントトークン
- `string CallbackId` イベントオブジェクトにカスタム設定されたコールバックID

失敗したイベントとセッションは以下を含みます。

- `bool WillRetry` しばらく後に再送を試みる予定であるかどうかを示します。

### <a id="disable-tracking"></a>トラッキングの無効化

`setEnabled`にパラメータ`false`を渡すことで、Adjust SDKが行うデバイスのアクティビティのトラッキングをすべて無効にすることができます。**この設定はセッション間で記憶されます**。最初のセッションの後でしか有効化できません。

```cs
Adjust.setEnabled(false);
```

Adjust SDKが現在有効かどうか、`isEnabled`関数を呼び出せば確認できます。また、`setEnabled`関数に`true`を渡せば、Adjust SDKを有効にすることができます。

### <a id="offline-mode"></a>オフラインモード

Adjustのサーバーへの送信を一時停止し、保持されているトラッキングデータを後から送信するためにAdjust SDKをオフラインモードにすることができます。オフラインモード中はすべての情報がファイルに保存されるので、イベントをたくさん発生させすぎないようにご注意ください。

`true`パラメータで`setOfflineMode`を呼び出すとオフラインモードを有効にできます。

```cs
Adjust.setOfflineMode(true);
```

反対に、`false`パラメータで`setOfflineMode`を呼び出せばオフラインモードを解除できます。Adjust SDKがオンラインモードに戻った時、保存されていた情報は正しいタイムスタンプでAdjustのサーバーに送られます。

トラッキングの無効化とは異なり、この設定はセッション間で**記憶されません**。オフラインモード時にアプリを終了しても、次に起動した時にはオンラインモードとしてアプリが起動します。

### <a id="event-buffering"></a>イベントバッファリング

イベントトラッキングを酷使している場合、HTTPリクエストを遅らせて1分毎にまとめて送信したほうがいい場合があります。その場合は、`AdjustConfig`インスタンスでイベントバッファリングを有効にしてください。

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

設定されていない場合、イベントバッファリングは**デフォルトで無効**になっています。

### <a id="gdpr-forget-me"></a>GDPR消去する権利（忘れられる権利）

次のメソッドを呼び出すと、EUの一般データ保護規制（GDPR）第17条に従い、ユーザーが消去する権利（忘れられる権利）を行使した際にAdjust SDKがAdjustバックエンドに情報を通知します。

```cs
Adjust.gdprForgetMe();
```

この情報を受け取ると、Adjustはユーザーのデータを消去し、Adjust SDKはユーザーの追跡を停止します。この削除された端末からのリクエストは今後、Adjustに送信されません。

### <a id="sdk-signature"></a>SDKシグネチャー‬

アカウントマネージャーがAdjust SDKシグネチャーを有効化する必要があります。この機能を使用する場合は、Adjustのサポート（support@adjust.com）までお問い合わせください。‬
 
‪すでにアカウントでSDKシグネチャーが有効になっており、Adjust管理画面のアプリシークレット（App Secret）にアクセスできる場合は、以下の方法を使用してアプリにSDKシグネチャーを実装してください。‬
 
‪アプリシークレットは、`AdjustConfig`インスタンスの`setAppSecret`メソッドにすべてのシークレットパラメーター（`secretId`、`info1`、`info2`、`info3`、`info4`）を`渡すことで設定されます。‬

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

### <a id="background-tracking"></a>バックグラウンドでのトラッキング

Adjust SDKはデフォルドでは**アプリがバックグラウンドにある時はHTTPリクエストを停止します**。この設定は`AdjustConfig`インスタンスで変更できます。

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="device-ids"></a>デバイスID

Google Analyticsなどの一部のサービスでは、レポートの重複を防ぐためにデバイスIDとクライアントIDを連携させることが求められます。

### <a id="di-idfa"></a>iOS広告ID

IDFAを取得するには、`Adjust`インスタンスの`getIdfa`関数をコールしてください。

```cs
string idfa = Adjust.getIdfa()
```

### <a id="di-gps-adid"></a>Google Playサービス広告ID

Google広告IDを取得する必要がある場合、広告IDはバックグラウンドでのスレッドでしか読み込みできないという制約があります。`getGoogleAdId`関数を`Action<string>`デリゲートでコールすると、この条件以外でも取得できるようになります。

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

Google広告IDを`googleAdId`変数として利用できます。

### <a id="di-fire-adid"></a>Amazon広告ID‬
 
‪Amazon広告IDを取得する必要がある場合は、`Adjust`インスタンスで`getAmazonAdId`メソッドをコールすることができます。‬

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="di-adid"></a>AdjustデバイスID

アプリがインストールされている各端末に対して、Adjustサーバーはユニークな**AdjustデバイスID** (**adid**)を生成します。このIDを取得するには、`Adjust`インスタンスの以下のメソッドをコールしてください。

```cs
String adid = Adjust.getAdid();
```

**注意**: **adid**に関する情報は、Adjustサーバーがアプリのインストールをトラッキングした後でのみ利用できます。それ以降ならば、Adjust SDKは**adid**に関する情報を持っていて、メソッドを使ってこれにアクセスすることができます。ですので、SDKが初期化されアプリのインストールが正常にトラッキングされる前に**adid**を使うことは**できません**。

### <a id="user-attribution"></a>ユーザーアトリビューション

アトリビューションコールバックは、[アトリビューションコールバックの章](#attribution-callback)で説明された通り、メソッドを使って引き起こされます。これらはユーザーのアトリビューション値の変化をお知らせします。ユーザーの現在のアトリビューション値に関する情報にアクセスする場合は、`Adjust`インスタンスの次のメソッドをコールしてください。

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**注意** ユーザーの現在のアトリビューション値に関する情報は、Adjustサーバーがアプリのインストールをトラッキングし最初のアトリビューションコールバックが呼ばれた後でのみ利用できます。それ以降であれば、Adjust SDKはユーザーのアトリビューション値に関する情報を持っていて、メソッドを使ってこれにアクセスすることができます。ですので、SDKが初期化され最初のアトリビューションコールバックが呼ばれる前にユーザーのアトリビューション値を使うことは**できません**。

### <a id="push-token"></a>Pushトークン

プッシュ通知のトークンを送信するには、**トークンを取得次第またはその値が変更され次第**、`Adjust`インスタンスで`setDeviceToken`メソッドをコールしてください。

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="pre-installed-trackers"></a>プレインストールのトラッカー

すでにアプリをインストールしたことのあるユーザーをAdjust SDKを使って識別したい場合は、次の手順で設定を行ってください。

1. [ダッシュボード]上で新しいトラッカーを作成してください。
2. `AdjustConfig`のデフォルトトラッカーを設定してください。

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

`{TrackerToken}`にステップ2で作成したトラッカートークンを入れてください。ダッシュボードには`http://app.adjust.com/`を含む
トラッカーURLが表示されます。ソースコード内にはこのURLすべてではなく、6文字のトークンを抜き出して指定してください。

3. アプリをビルドして実行してください。ログ出力に下記のような行が表示されるはずです。

    ```
    Default tracker: 'abc123'
    ```

### <a id="deeplinking"></a>ディープリンキング

**ディープリンキングはiOSとAndroidプラットフォームでのみサポートされています**。

URLからアプリへのディープリンクを使ったAdjustトラッカーURLをご利用の場合、ディープリンクURLとその内容の情報を得られる可能性があります。ユーザーがすでにアプリをインストールしている状態でそのURLに訪れた場合(スタンダード・ディープリンキング)と、アプリをインストールしていないユーザーがURLを開いた場合(ディファード・ディープリンキング)が有り得ます。スタンダード・ディープリンキングの場合、Androidのプラットフォームにはネイティブでディープリンクの内容を取得できる仕組みがあります。ディファード・ディープリンキングに対してはAndroidプラットフォームはサポートしていませんので、Adjust SDKがディープリンクの内容を取得するメカニズムを提供します。

ディープリンキングの操作は**ネイティブで**設定する必要があります。iOSではXcodeプロジェクトで、AndroidではAndroid StudioかEclipseプロジェクトで設定してください。

#### <a id="deeplinking-standard"></a>スタンダード・ディープリンキング

UnityのC#コードではスタンダード・ディープリンキングにおいてディープリンクの情報を届けることができません。ディープリンキングの取り扱いをアプリで有効化すると、ネイティブレベルでディープリンクの情報が得られます。AndroidとiOSアプリでのディープリンキングの有効化については以下の章をご覧ください。

#### <a id="deeplinking-deferred"></a>ディファード・ディープリンキング

ディファード・ディープリンキングにおいてURLの内容情報を得るには、`AdjustConfig`オブジェクトにコールバックメソッドを設定する必要があります。これはひとつの`string`パラメータを受信し、そのパラメータにURLの内容が格納されます。`setDeferredDeeplinkDelegate`メソッドをコールしてConfigオブジェクトでこのメソッドを実装してください。

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

<a id="deeplinking-deferred-open">ディファード・ディープリンキングでは、`AdjustConfig`オブジェクトで設定できる追加機能があります。Adjust SDKがディファード・ディープリンクの情報を得ると、Adjust SDKがそのURLを開くかどうか決めることができます。これを設定するには、Configオブジェクトで`setLaunchDeferredDeeplink`メソッドをコールしてください。

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

これが設定されていない場合、**Adjust SDKはデフォルトで常にディープリンクを開きます**。

アプリでディープリンキングを有効化するには、サポートするプラットフォーム毎にスキームを設定してください。

#### <a id="deeplinking-app-android"></a>Androidアプリでのディープリンキング

**これはネイティブのAndroid StudioもしくはEclipseプロジェクトで行ってください**。

Androidアプリでネイティブレベルでディープリンキングを設定するには、公式Android SDK READMEの[ガイド][android-deeplinking]をご確認ください。

#### <a id="deeplinking-app-ios"></a>アプリでのディープリンキング

**これはネイティブのXcodeプロジェクトで行ってください**。

iOSアプリでネイティブレベルでディープリンキングを設定するには、公式iOS SDK READMEの[ガイド][ios-deeplinking]をご確認ください。

## <a id="troubleshooting"></a>トラブルシューティング

### <a id="ts-debug-ios"></a>iOSでのデバッグ情報

ビルド後のプロセスのスクリプトを使っていても、プロジェクトがすぐに使える状態で実行できるとは限りません。

必要であれば、dSYMファイルを無効化してください。`Project Navigator`から`Unity-iPhone`プロジェクトを選び、`Build Settings`タブをクリックして`debug information`を検索してください。`Debug Information Format`もしくは`DEBUG_INFORMATION_FORMAT`オプションが見つかるはずです。`DWARF with dSYM File`を`DWARF`に変更してください。


[dashboard]:    http://adjust.com
[adjust.com]:   http://adjust.com

[en-readme]:    ../../README.md
[zh-readme]:    ../chinese/README.md
[ja-readme]:    ../japanese/README.md
[ko-readme]:    ../korean/README.md

[sdk2sdk-mopub]:    ../japanese/sdk-to-sdk/mopub.md

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/adjust_unity_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/ios_sdk/#deeplink-reattributions
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/en/special-partners
[unity-purchase-sdk]:      https://github.com/adjust/unity_purchase_sdk
[android-deeplinking]:     https://github.com/adjust/android_sdk#deep-linking
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[install-referrer-aar]:    https://maven.google.com/com/android/installreferrer/installreferrer/1.0/installreferrer-1.0.aar
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

## <a id="license">ライセンス

mod_pbxproj.pyファイルはApache License Version 2.0（「本ライセンス」）に基づいてライセンスされます。
あなたがこのファイルを使用するためには、本ライセンスに従わなければなりません。本ライセンスのコピーは下記の場所から入手できます。
http://www.apache.org/licenses/LICENSE-2.0

adjust SDKはMITライセンスを適用しています。

Copyright (c) 2012-2019 Adjust GmbH,
http://www.adjust.com

以下に定める条件に従い、本ソフトウェアおよび関連文書のファイル（以下「ソフトウェア」）の複製を取得するすべての人に対し、
ソフトウェアを無制限に扱うことを無償で許可します。これには、ソフトウェアの複製を使用、複写、変更、結合、掲載、頒布、サブライセンス、
および/または販売する権利、およびソフトウェアを提供する相手に同じことを許可する権利も無制限に含まれます。

上記の著作権表示および本許諾表示を、ソフトウェアのすべての複製または重要な部分に記載するものとします。

ソフトウェアは「現状のまま」で、明示であるか暗黙であるかを問わず、何らの保証もなく提供されます。
ここでいう保証とは、商品性、特定の目的への適合性、および権利非侵害についての保証も含みますが、それに限定されるものではありません。 
作者または著作権者は、契約行為、不法行為、またはそれ以外であろうと、ソフトウェアに起因または関連し、
あるいはソフトウェアの使用またはその他の扱いによって生じる一切の請求、損害、その他の義務について何らの責任も負わないものとします。
