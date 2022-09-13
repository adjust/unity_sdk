## 요약

Adjust™의 Unity SDK 관련 문서입니다. Unity SDK는 iOS, Android, Windows Store 8.1, Windows Phone 8.1 및 Windows 10을 지원합니다. [adjust.com]에서 Adjust™에 대한 정보를 더 자세히 알아보세요. 

**참고**: Adjust Unity SDK **4.12.0** 버전부터는 **Unity 5 이상** 버전과 호환됩니다.

**참고**: Adjust Unity SDK **4.19.2** 버전부터는 **Unity 2017.1.1 이상** 버전과 호환됩니다.

**참고**: Adjust Unity SDK **4.21.0** 버전부터는 **Unity 2017.4.1 이상** 버전과 호환됩니다.

다른 언어로 읽기: [English][en-readme], [中文][zh-readme], [日本語][ja-readme], [한국어][ko-readme].

## 목차

### 빠른 시작

   * [시작하기](#qs-getting-started)
      * [SDK 설치](#qs-get-sdk)
      * [프로젝트에 SDK 추가](#qs-add-sdk)
      * [앱에 SDK 연동](#qs-integrate-sdk)
      * [Adjust 로깅(logging)](#qs-adjust-logging)
      * [Google Play 서비스](#qs-gps)
      * [Proguard 설정](#qs-android-proguard)
      * [Google 설치 참조자](#qs-install-referrer)
      * [Huawei 리퍼러 API](#qs-huawei-referrer-api)
      * [빌드 후 절차](#qs-post-build-process)
        * [iOS 빌드 후 절차](#qs-post-build-ios)
        * [Android 빌드 후 절차](#qs-post-build-android)
      * [SDK 서명](#qs-sdk-signature)

### 딥링크

   * [딥링크 개요](#dl)
   * [표준 딥링크](#dl-standard)
   * [디퍼드 딥링크](#dl-deferred)
   * [Android 앱에서 딥링크 처리하기](#dl-app-android)
   * [iOS 앱에서 딥링크 처리하기](#dl-app-ios)
      
### 이벤트 추적

   * [이벤트 추적](#et-tracking)
   * [매출 추적](#et-revenue)
   * [매출 중복 제거](#et-revenue-deduplication)
   * [인앱 결제 검증](#et-purchase-verification)

### 커스텀 파라미터

   * [커스텀 파라미터 개요](#cp)
   * [이벤트 파트너 파라미터](#cp-event-partner-parameters)
      * [이벤트 콜백 파라미터](#cp-event-callback-parameters)
      * [이벤트 파트너 파라미터](#cp-event-partner-parameters)
      * [이벤트 콜백 식별자](#cp-event-callback-id)
   * [세션 파라미터](#cp-session-parameters)
      * [세션 콜백 파라미터](#cp-session-callback-parameters)
      * [세션 파트너 파라미터](#cp-session-partner-parameters)
      * [지연 시작](#cp-delay-start)

### 부가 기능

   * [AppTrackingTransparency 프레임워크](#ad-att-framework)
      * [앱 추적 인증 래퍼](#ad-ata-wrapper)
   * [SKAdNetwork 프레임워크](#ad-skadn-framework)
   * [푸시 토큰(삭제 추적)](#ad-push-token)
   * [어트리뷰션 콜백](#ad-attribution-callback)
   * [광고 매출 추적](#ad-ad-revenue)
   * [구독 추적](#ad-subscriptions)
   * [세션 및 이벤트 콜백](#ad-session-event-callbacks)
   * [사용자 어트리뷰션](#ad-user-attribution)
   * [기기 ID](#ad-device-ids)
      * [iOS 광고 식별자](#ad-idfa)
      * [Google Play 서비스 광고 식별자](#ad-gps-adid)
      * [Amazon 광고 식별자](#ad-amazon-adid)
      * [Adjust 기기 식별자](#ad-adid)
   * [사전 설치 트래커](#ad-pre-installed-trackers)
   * [오프라인 모드](#ad-offline-mode)
   * [추적 비활성화](#ad-disable-tracking)
   * [이벤트 버퍼링](#ad-event-buffering)
   * [백그라운드 추적](#ad-background-tracking)
   * [GDPR 잊혀질 권리(Right to be Forgotten)](#ad-gdpr-forget-me)
   * [타사 공유 비활성화](#ad-disable-third-party-sharing)

### 테스트 및 문제 해결
   * [iOS에서 정보 디버그](#tt-debug-ios)

### 라이센스
  * [라이센스 계약](#license)


## 빠른 시작

### <a id="qs-getting-started"></a>시작하기

Unity 프로젝트에 Adjust SDK를 연동하려면 다음과 같이 실행하세요.

### <a id="qs-get-sdk"></a>SDK 설치

최신 버전은 [Adjust releases 페이지](https://github.com/adjust/unity_sdk/releases)에서 다운로드하실 수 있습니다.

### <a id="qs-add-sdk"></a>프로젝트에 SDK 추가

Unity 에디터에서 프로젝트를 열고, `Assets → Import Package → Custom Package`로 이동한 다음 다운로드한 Unity 패키지 파일을 선택합니다.

![][import_package]

### <a id="qs-integrate-sdk"></a>앱에 SDK 연동

첫 씬에 `Assets/Adjust/Adjust.prefab`의 프리팹(prefab)을 추가합니다.

`Inspector menu` 프리팹의 Adjust 스크립트 파라미터를 수정하여 다음 옵션을 설정할 수 있습니다.

* [수동으로 시작](#start-manually)
* [이벤트 버퍼링](#event-buffering)
* [백그라운드에서 보내기](#background-tracking)
* [디퍼드 딥링크 실행](#deeplinking-deferred-open)
* [앱 토큰](#app-token)
* [로그 수준](#adjust-logging)
* [환경](#environment)

![][adjust_editor]

<a id="app-token">{YourAppToken}`을 실제 앱 토큰으로 교체합니다. 대시보드에서 앱 토큰을 찾으려면 [이 단계](https://help.adjust.com/en/dashboard/apps/app-settings#view-your-app-token)를 수행하세요. 

<a id="environment">테스트와 배포 중 어느 목적으로 앱을 빌드하는지에 따라 환경 설정을 `Sandbox` 나 `Production` 으로 변경해야 합니다.

**중요:** 앱을 테스트하는 경우 값을 `Sandbox`로 설정해야 합니다. 앱을 퍼블리시할 준비가 완료되면 환경 설정을 `Production`으로 변경하고, 앱 테스트를 새로 시작한다면 `Sandbox`로 다시 설정하세요. Adjust 대시보드는 기본적으로 앱의 Production 트래픽을 표시하므로, Sandbox 모드에서 테스트하는 동안 생성된 트래픽을 보려면 대시보드에서 Sandbox 트래픽 보기로 전환하세요.

Adjust는 테스트 기기에서 발생하는 인위적 트래픽과 실제 트래픽을 구분하기 위해 환경 설정을 사용합니다. 환경 설정을 최신 상태로 유지하세요.

<a id="start-manually">앱의 `Awake` 이벤트 실행 시 Adjust SDK를 자동으로 시작하려면 `Start Manually` 를 선택합니다. 이 옵션을 선택하면 `AdjustConfig` 객체를 파라미터로 `Adjust.start` 메서드를 호출하여 코드 내에서 Adjust SDK를 초기화하고 시작합니다.

`Assets/Adjust/ExampleGUI/ExampleGUI.unity`.에서 버튼 메뉴가 있는 씬 예시를 확인할 수 있습니다. 

이 씬의 소스는 `Assets/Adjust/ExampleGUI/ExampleGUI.cs`.에 있습니다.

### <a id="qs-adjust-logging"></a>Adjust 로깅(logging)

`Log Level`을 다음과 같이 변경하여 로그의 데이터 분할도를 조절할 수 있습니다.

- `Verbose` - 모든 로그 활성화
- `Debug` - 자세한 로그 비활성화
- `Info` - 디버그 로그 비활성화(기본)
- `Warn` - 정보 로그 비활성화
- `Error` - 경고 로그 비활성화
- `Assert` - 오류 로그 비활성화
- `Suppress` - 모든 로그 비활성화

수동으로 Adjust SDK 초기화 시 모든 로그 출력을 비활성화하려면 로그 수준을 `Suppress`로 설정하고 `AdjustConfig` 객체에 생성자를 사용하세요. 그러면 Suppress 로그 수준 지원 여부를 입력할 수 있는 부울자료 파라미터가 열립니다.

```cs
string appToken = "{YourAppToken}";
AdjustEnvironment environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

타겟이 Windows 기반이며 Adjust 라이브러리의 컴파일된 로그를 `released` 모드로 보고 싶으시다면 `debug` 모드에서 앱을 테스트하면서 로그 출력을 앱으로 리다이렉트합니다.

SDK를 시작하기 전에 `AdjustConfig` 인스턴스에서 `setLogDelegate` 메서드를 호출합니다.

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="qs-gps"></a>Google Play 서비스

2014년 8월 1일부터 고유 기기 식별을 위해 Google Play Store 앱에 대해 [Google 광고 식별자][google_ad_id] 사용이 의무화되었습니다. Adjust SDK가 Google 광고 식별자를 사용하도록 허용하려면 [Google Play 서비스][google_play_services]를 연동해야 합니다. 이를 위해서는 `google-play-services_lib` 폴더(Android SDK의 일부)를 Unity 프로젝트의 `Assets/Plugins/Android` 폴더로 복사합니다.

주로 두 가지 방법으로 Android SDK를 다운로드할 수 있습니다. `Android SDK Manager`를 사용하는 모든 도구에는 Android SDK 도구를 다운로드 및 설치할 수 있는 빠른 링크가 있습니다. Android SDK 도구를 설치한 후에는 `SDK_FOLDER/extras/google/google_play_services/libproject/` 폴더에서 라이브러리를 찾을 수 있습니다.

![][android_sdk_location]

Android SDK Manager와 함께 사용하는 도구가 없다면 공식 독자적 솔루션인 [Android SDK][android_sdk_download]를 다운로드하세요. 그런 다음 `SDK Readme.txt`의 지침에 따라 Android SDK 도구를 다운로드하세요. Google이 제공한 README는 Android SDK 폴더에 있습니다.

**업데이트**: Google은 최신 Android SDK 버전에서 루트 SDK 폴더 내 Google Play Services 폴더의 구조를 다음과 같이 변경했습니다.

![][android_sdk_location_new]

이제 Adjust SDK에서 필요로 하는 Google Play Services 라이브러리의 일부인 basement만 추가할 수 있습니다. 이를 위해 `Assets/Plugins/Android` 폴더에 `play-services-basement-x.y.z.aar` 파일을 추가하세요. 

Google Play 서비스 라이브러리 15.0.0 이후 버전부터 Google은 Google 광고 식별자 획득에 필요한 클래스를 `play-services-ads-identifier`] 패키지로 이동했습니다. 라이브러리 15.0.0 이후 버전을 사용하는 경우 이 패키지를 앱에 추가하세요. 완료되면 Adjust SDK가 Google 광고 식별자를 올바르게 획득하는지 테스트하세요. 사용되는 Unity 통합 개발 환경(IDE)에 따라 일부 불일치가 발생하는 경우가 확인되었습니다. 

#### Google 광고 식별자 테스트

Adjust SDK에서 Google 광고 식별자를 수신하고 있는지 확인하려면 `sandbox` 모드에서 구동되도록 SDK를 설정하고 로그 수준을 `verbose`로 지정한 후, 앱을 실행합니다. 세션이나 앱 내 이벤트를 추적하고, 추적이 완료되면 자세한 로그에 기록된 파라미터 리스트를 확인합니다. `gps_adid`라는 파라미터가 확인되면 Adjust SDK가 Google 광고 식별자를 읽는 데 성공한 것입니다.

Google 광고 식별자를 읽는 데 문제가 있다면 Adjust의 Github 리포지토리에서 문제를 제시하거나 support@adjust.com으로 메일 주시기 바랍니다.

### <a id="qs-android-proguard"></a>Proguard 설정

Proguard를 사용하는 경우, Proguard 파일에 다음 줄을 추가하세요.

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

### <a id="qs-install-referrer"></a>Google 설치 리퍼러 

Android 앱의 설치를 올바르게 어트리뷰션하려면 Adjust에 Google 설치 리퍼러 대한 정보를 제공해야 합니다. 이를 위해 **Google Play Referrer API**를 사용하거나 **Google Play Store intent** 브로드캐스트 리시버를 캐치할 수 있습니다. 

Google은 Google Play Store intent보다 더 신뢰할 수 있고 안전한 방법으로 설치 참조자 정보를 획득하고 어트리뷰션 제공자가 클릭 인젝션에 대응할 수 있는 수단으로 Google Play Referrer API를 도입했습니다. Google Play Store intent는 당분간은 API와 함께 사용 가능하지만, 향후 지원이 중단될 예정입니다. 이러한 조치에 협조해주시기를 부탁드립니다. 

Adjust의 빌드 후 절차는 Google Play Store intent를 감지합니다. 몇 가지 단계를 수행하여 새로운 Google Play Referrer API 지원을 추가하세요.

Google Play Referrer API 지원을 추가하려면 Maven 리포지토리에서 [설치 리퍼러 라이브러리][install-referrer-aar]를 다운로드하고 `Plugins/Android` 폴더로 AAR 파일을 가져오세요.

#### <a id="qs-huawei-referrer-api"></a>Huawei 리퍼러 API

Adjust SDK 4.21.1 버전부터는 Huawei 앱 갤러리 버전이 10.4 이상인 Huawei 기기에 설치 추적을 지원합니다. Huawei 리퍼러 API를 사용하기 위해 추가적인 연동 단계를 수행하지 않아도 됩니다.

### <a id="qs-post-build-process"></a>빌드 후 절차

Adjust Unity 패키지를 사용하면 앱 빌드 절차를 완료하기 위해 맞춤 빌드 후 작업을 수행하여 Adjust SDK가 앱 내에서 제대로 작동하도록 할 수 있습니다. 

이 절차는 `AdjustEditor.cs`의 `OnPostprocessBuild` 메서드에서 수행합니다. 또한 Unity IDE 콘솔 출력 창에 로그 출력 메시지가 작성됩니다.

### <a id="qs-post-build-ios"></a>iOS 빌드 후 절차

iOS 빌드 후 절차를 올바르게 실행하려면 Unity 이후 버전을 사용하고`iOS build support`를 설치하세요. iOS 빌드 후 절차는 생성한 Xcode 프로젝트를 다음과 같이 변경합니다.

- `iAd.framework` 추가 (Apple Search Ads 추적에 필요)
- `AdSupport.framework` 추가 (IDFA 읽기에 필요)
- `CoreTelephony.framework` 추가 (네트워크 기기의 읽기 유형 연결에 필요)
- 다른 링커 플래그 `-ObjC` 추가(빌드 시간 동안 Objective-C 인식에 필요)
- `Objective-C exceptions` 활성화

iOS 14 지원을 활성화하면(`Assets/Adjust/Toggle iOS 14 Support`), 두 개의 프레임워크가 iOS 빌드 후 절차를 통해 Xcode 프로젝트에 추가됩니다.

- `AppTrackingTransparency.framework` 추가(추적에 대한 사용자 동의 요청 및 동의 상태 획득에 필요)
- `StoreKit.framework` 추가 (SKAdNetwork 프레임워크와의 커뮤니케이션에 필요)

### <a id="qs-post-build-android"></a>Android 빌드 후 절차

Android 빌드 후 절차는 `Assets/Plugins/Android/`에 있는 `AndroidManifest.xml` 파일을 변경하며, Android 플러그인 폴더에 `AndroidManifest.xml` 파일이 있는지도 확인합니다. 파일이 없으면 빌드 후 절차가 호환되는 manifest 파일`AdjustAndroidManifest.xml`의 사본을 생성합니다. 이미 `AndroidManifest.xml` 파일이 있다면 빌드 후 절차는 파일을 다음과 같이 변경합니다.

- `INTERNET` 권한 추가(인터넷 연결에 필요)
- `ACCESS_WIFI_STATE` 권한 추가(Play Store를 통해 앱을 배포하지 않는 경우 필요)
- `ACCESS_NETWORK_STATE` 권한 추가(네트워크 기기의 읽기 유형 연결에 필요)
- `BIND_GET_INSTALL_REFERRER_SERVICE` 권한 추가(새로운 Google install referrer API 작동에 필요)
- Adjust 브로드캐스트 리시버 추가(Google Play Store intent를 통해 설치 참조자 정보를 획득하는 데 필요). 자세한 내용은 [Android SDK README][android]를 참조하세요. 

**참고:**: INSTALL_REFERRER intent를 처리하는 자체 브로드캐스트 리시버를 사용하는 경우, Adjust 브로드캐스트 리시버를 manifest 파일에 추가할 필요가 없습니다. 이를 삭제할 수 있지만, [Android 가이드][android-custom-receiver]에 설명된 대로 자체 리시버 내에서 호출을 Adjust 브로드캐스트 리시버에 추가하세요.

### <a id="qs-sdk-signature"></a>SDK 서명

계정 관리자는 여러분 대신 Adjust SDK 서명을 활성화할 수 있습니다. 이 기능의 사용에 관심이 있는 경우 Adjust 고객 지원팀(support@adjust.com)에 문의하시기 바랍니다.

계정에서 SDK 서명을 활성화했으며 대시보드의 앱 시크릿에 액세스할 수 있는 경우, `AdjustConfig` 인스턴스에 `setAppSecret` 메서드에 모든 비밀 파라미터(`secretId`, `info1`, `info2`, `info3`, `info4`)를 추가합니다.

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

SDK 서명이 앱에 통합되었습니다. 


## 딥링크

### <a id="dl"></a>딥링크 개요

**Adjust는 iOS 및 Android 플랫폼에서 딥링크를 지원합니다.**

Adjust 트래커 URL에 딥링크를 활성화한 경우, 딥링크 URL과 그 내용에 대한 정보를 받아볼 수 있습니다. 기기에 여러분의 앱을 설치(표준 딥링크)한 사용자와 설치하지 않은(디퍼드 딥링크) 사용자 모두 URL과 상호작용할 수 있습니다. 

표준 딥링크의 경우 Android 플랫폼에서 딥링크 내용을 받아볼 수 있지만, 디퍼드 링크는 Android는 디퍼드 딥링크를 자동 지원되지 않습니다. 디퍼드 딥링크 내용에 액세스하려면 Adjust SDK를 사용하세요.

생성된 Xcode 프로젝트(iOS) 및 Android Studio/Eclipse 프로젝트(Android) 내에서 **네이티브 수준**에 딥링크 처리를 설정합니다.

### <a id="dl-standard"></a>표준 딥링크

표준 딥링크 정보를 Unity C# 코드로 받아볼 수 없습니다. 앱에서 딥링크 처리를 활성화하면 네이티브 수준의 딥링크 정보를 받게 됩니다. 딥링크를 활성화하는 방법에 관한 자세한 내용([Android](#dl-app-android) 및 [iOS](#dl-app-ios) 앱)을 알아보세요.

### <a id="dl-deferred"></a>디퍼드 딥링크

디퍼드 딥링크 내용에 대한 정보를 획득하려면 `AdjustConfig` 객체에 콜백 메서드를 설정해야 합니다. 그러면 URL 내용이 전달되는`string` 파라미터 1개를 받게 됩니다. 다음과 같이 `setDeferredDeeplinkDelegate` 메서드를 호출하여 설정 객체에 이 메서드를 설정합니다.

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

<a id="deeplinking-deferred-open"></a>디퍼드 딥링크에서 `AdjustConfig` 개체에 설정할 수 있는 추가 설정이 하나 있습니다. Adjust SDK가 디퍼드 딥링크 정보를 획득하면 SDK가 이 URL을 열도록 할지 선택할 수 있습니다. 설정 개체의 `setLaunchDeferredDeeplink` 메서드를 호출하여 이 옵션을 설정할 수 있습니다.

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

아무 것도 설정되지 않은 경우 **Adjust SDK는 기본적으로 항상 URL 실행을 시도합니다**.

앱에서 딥링크 지원을 활성화하려면 지원되는 플랫폼별로 스킴을 설정해야 합니다.

### <a id="dl-app-android"></a>Android 앱에서 딥링크 처리하기

Android 앱에서 네이티브 수준의 딥링크 처리를 설정하려면 Adjust의 공식 [Android SDK README][android-deeplinking] 지침을 따르세요.

네이티브 Android Studio/Eclipse 프로젝트에 수행하는 작업입니다.

### <a id="dl-app-ios"></a>iOS 앱에서 딥링크 처리하기

**네이티브 Xcode 프로젝트에 수행하는 작업입니다.**

iOS 앱에서 네이티브 수준의 딥링크 처리를 설정하려면 네이티브 Xcode 프로젝트를 사용하고 Adjust의 공식 [iOS SDK README][ios-deeplinking] 지침을 따르세요.

## 이벤트 추적

### <a id="et-tracking"></a>이벤트 추적

Adjust를 사용하여 앱의 모든 이벤트를 추적할 수 있습니다. 버튼을 누르는 동작을 모두 추적하려면 대시보드에서 [새로운 이벤트 토큰을 생성](https://help.adjust.com/en/tracking/in-app-events/basic-event-setup#generate-event-tokens-in-the-adjust-dashboard)하세요. 이벤트 토큰이 abc123`이라고 가정해 보겠습니다. 버튼의 클릭 핸들러 메서드에 다음 라인을 추가하면 클릭을 추적할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

### <a id="et-revenue"></a>매출 추적

사용자가 광고에 인게이지하거나 앱 내 구매를 진행하여 매출이 창출되면 이벤트를 이용하여 이를 추적할 수 있습니다. 예를 들어, 광고를 한 번 누르는 행위에 €0.01의 매출 금액이 발생한다고 가정해 보겠습니다. 이 경우, 매출 이벤트를 다음과 같이 추적할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

사용자가 통화 토큰을 설정하면 Adjust는 openexchange API를 이용하여 사용자의 선택에 따라 발생 매출을 보고 매출로 자동 전환합니다. [여기에서 통화 전환에 대해 자세히 알아보세요](http://help.adjust.com/tracking/revenue-events/currency-conversion).

앱 내 구매를 추적하려는 경우, 구매가 완료되고 품목이 구매되었을 때만 `trackEvent`를 호출하십시오. 이는 실제로 사용자에게서 발생하지 않은 매출을 추적하는 것을 방지할 수 있는 중요한 조치입니다.


### <a id="et-revenue-deduplication"></a>매출 중복 제거

중복되는 매출을 추적하는 것을 방지하기 위해 트랜잭션 ID를 선택적으로 추가하세요. SDK가 마지막 열 개의 트랜잭션 ID를 기억하며, 중복되는 트랜잭션 ID가 있는 매출 이벤트는 건너뜁니다. 이러한 방식은 인앱 결제 추적에 특히 유용합니다. 

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

### <a id="et-purchase-verification"></a>인앱 결제 검증

서버측 수신 검증 도구인 [Adjust의 결제 검증][unity-purchase-sdk]을 사용하여 앱 결제를 검증하세요.  

## 맞춤 파라미터

### <a id="cp"></a>맞춤 파라미터 개요

Adjust SDK가 기본적으로 수집하는 데이터 포인트 외에도 Adjust SDK를 사용하여 이벤트나 세션에 필요한 만큼의 맞춤 값을 추적하고 추가할 수 있습니다(사용자 ID, 제품 ID 등). 맞춤 파라미터는 로데이터로만 제공되며 Adjust 대시보드에 표시되지 **않습니다**.

내부용으로 수집하는 값의 경우 [콜백 파라미터](https://help.adjust.com/en/manage-data/export-raw-data/callbacks/best-practices-callbacks)를 사용하고, 외부 파트너와 공유하는 값의 경우 파트너 파라미터를 사용하세요. 내/외부 공용으로 추적하는 값(예: 제품 ID)의 경우 콜백 파라미터와 파트너 파라미터를 모두 사용하도록 권장합니다.

### <a id="cp-event-parameters"></a>이벤트 파라미터

### <a id="cp-event-callback-parameters"></a>이벤트 콜백 파라미터

[대시보드에서] 이벤트에 대한 콜백 URL을 등록하면 이벤트가 추적될 때마다 Adjust가 해당 URL에 GET 요청을 전송합니다. 객체에 키-값 쌍을 입력하고 `trackEvent` 메서드에 전달할 수도 있습니다. 그런 다음 Adjust는 이러한 파라미터를 사용자의 콜백 URL에 추가합니다.

예를 들어, 사용자가 이벤트를 위해 http://www.adjust.com/callback URL을 등록했으며 다음과 같은 이벤트를 추적한다고 가정해 보겠습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

이 경우, Adjust가 이벤트를 추적하여 다음으로 요청을 전송합니다.

```
http://www.example.com/callback?key=value&foo=bar
```

Adjust는 iOS용 `{idfa}`나 Android용 `{gps_adid}` 등 파라미터 값으로 사용할 수 있는 다양한 자리 표시자를 지원합니다.  이 예에서는 결과 콜백의 자리 표시자를 현재 기기의 IDFA/Google Play 서비스 ID로 바꾸었습니다. [실시간 콜백](https://help.adjust.com/en/manage-data/export-raw-data/callbacks)에 대해 자세히 알아보고 [자리 표시자](https://partners.adjust.com/placeholders/) 전체 목록을 확인하세요. 

**참고:** Adjust는 맞춤 파라미터를 보관하지 않으며, 콜백에 추가하기만 하기 때문에 이벤트 콜백을 등록하지 않으면 Adjust가 맞춤 파라미터를 읽을 수 없습니다.


### <a id="cp-event-partner-parameters"></a>이벤트 파트너 파라미터

대시보드에서 파라미터를 활성화한 후에는 네트워크 파트너에게 전송할 수 있습니다. [모듈 파트너](https://docs.adjust.com/en/special-partners/)와 확장 연동 방법에 대해 자세히 알아보세요.

이는 콜백 파라미터와 동일한 방식으로 이루어집니다. `AdjustEvent` 인스턴스의 `addPartnerParameter` 메서드를 호출하여 추가할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

[특별 파트너 가이드][special-partners]에서 특별 파트너와 연동 방법에 대한 자세한 내용을 알아보실 수 있습니다.

### <a id="cp-event-callback-id"></a>이벤트 콜백 식별자

추적할 각 이벤트에 맞춤 문자열 ID를 추가할 수도 있습니다. 이 식별자는 이후에 이벤트 콜백에서 보고되며, 이를 통해 성공적으로 추적된 이벤트를 확인할 수 있습니다. AdjustEvent 인스턴스에 setCallbackId 메서드를 호출하여 이 ID를 설정할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="cp-session-parameters"></a>세션 파라미터

세션 파라미터는 로컬에 저장되며 모든 Adjust SDK **이벤트 및 세션**과 함께 전송됩니다. 이러한 파라미터를 추가할 때마다 Adjust가 해당 파라미터를 저장하므로, 다시 추가할 필요가 없습니다. 동일한 파라미터를 다시 추가해도 아무 일도 일어나지 않습니다.

Adjust SDK가 실행되기 전에 세션 파라미터를 전송할 수 있습니다. [SDK 지연](#cp-delay-start)을 사용하면 추가적인 값(예: 앱 서버의 검증 토큰)을 획득하여 모든 정보를 SDK 초기화 시 일괄 전송할 수 있습니다. 

### <a id="cp-session-callback-parameters"></a>세션 콜백 파라미터

이벤트 콜백 파라미터를 저장하여 Adjust 세션마다 전송할 수 있습니다.

세션 콜백 파라미터의 인터페이스는 이벤트 콜백 파라미터와 유사합니다. 키와 값을 이벤트에 추가하는 대신,`Adjust` 인스턴스의 `addSessionCallbackParameter` 메서드로의 호출을 통해 추가합니다.

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

세션 콜백 파라미터는 콜백 파라미터와 병합되어 모든 정보를 일괄 전송하지만, 이벤트에 콜백 파라미터는 세션 콜백 파라미터보다 우선순위가 높습니다. 세션 콜백 파라미터와 동일한 키로 이벤트 콜백 파라미터를 추가하면 이벤트 값이 표시됩니다.

원하는 키를 `Adjust` 인스턴스의`removeSessionCallbackParameter` 메서드에 전달하여 특정 세션 콜백 파라미터를 삭제할 수 있습니다.

```cs
Adjust.removeSessionCallbackParameter("foo");
```

세션 콜백 파라미터에서 모든 키와 키에 상응하는 값을 삭제하려면 Adjust` 클래스의 `resetSessionCallbackParameters` 메서드로 재설정하면 됩니다.

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="cp-session-partner-parameters"></a>세션 파트너 파라미터

Adjust SDK를 트리거하는 모든 이벤트 또는 세션마다 전송되는 [세션 콜백 파라미터](#cp-session-callback-parameters)가 있는 것처럼 세션 파트너 파라미터도 있습니다.

이러한 세션 파트너 파라미터는 [대시보드]에서 활성화된 모든 네트워크 파트너 연동에 전송됩니다.

세션 파트너 파라미터는 이벤트 파트너 파라미터와 유사한 인터페이스를 가집니다. 키와 값을 이벤트에 추가하는 대신,`Adjust` 인스턴스의 `addSessionPartnerParameter` 메서드로의 호출을 통해 추가합니다.

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

세션 파트너 파라미터는 이벤트 파트너 파라미터와 병합됩니다. 단, 이벤트 파트너 파라미터는 세션 파트너 파라미터보다 높은 우선순위를 가집니다. 세션 파트너 파라미터와 동일한 키로 이벤트 파트너 파라미터를 추가하면 이벤트 값이 표시됩니다.

원하는 키를 `Adjust` 인스턴스의 removeSessionPartnerParameter` 메서드에 전달하여 특정 세션 파트너 파라미터를 삭제할 수 있습니다.

```cs
Adjust.removeSessionPartnerParameter("foo");
```

세션 파트너 파라미터에서 모든 키와 키에 상응하는 값을 삭제하려면 Adjust` 클래스의 ``resetSessionPartnerParameters` 메서드로 재설정하면 됩니다.

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="cp-delay-start"></a>시작 지연

Adjust SDK의 시작을 지연시키면 앱이 고유 ID와 같은 세션 파라미터를 획득할 시간이 확보되므로, 세션 파라미터를 설치 시에 전송할 수 있게 됩니다.

AdjustConfig 인스턴스의 setDelayStart 필드로 초기 지연 시간을 초 단위로 설정하세요.

```cs
adjustConfig.setDelayStart(5.5);
```

이 예에서는 Adjust SDK가 초기 설치 세션과 5.5초 이내에 새로 생성된 이벤트를 전송하지 않습니다. 이 시간이 만료되거나 그동안 `Adjust.sendFirstPackages()`를 호출하면 모든 세션 파라미터가 지연된 설치 세션 및 이벤트에 추가되며 Adjust SDK가 평소대로 되돌아갑니다.

Adjust SDK의 최대 시작 지연 시간은 10초입니다.

## 부가 기능

Adjust SDK를 프로젝트에 연동하면 다음 기능을 활용할 수 있습니다.

### <a id="ad-att-framework"></a>AppTrackingTransparency 프레임워크

**참고**: 이 기능은 iOS 플랫폼에서만 사용할 수 있습니다.

전송된 각 패키지에 대해 Adjust 백엔드는 사용자 또는 기기를 추적하는 데 사용할 수 있는 앱 관련 데이터에 대한 액세스 동의를 다음 네 가지 상태 중 하나로 수신합니다.

- Authorized
- Denied
- Not Determined
- Restricted

기기가 사용자 기기 추적에 사용되는 앱 관련 데이터에 대한 액세스를 승인하는 인증 요청을 수신한 후에는 Authorized 또는 Denied 상태가 반환됩니다.

기기가 사용자 또는 기기를 추적하는 데 사용되는 앱 관련 데이터에 대한 액세스 인증 요청을 수신하기 전에는 Not Determined 상태가 반환됩니다.

앱 추적 데이터 인증 권한이 제한되면 Restricted 상태가 반환됩니다.

사용자에게 표시되는 대화 상자 팝업을 맞춤 설정하지 않으려는 경우, SDK에는 사용자가 대화 상자 팝업에 응답하면 업데이트된 상태를 수신하는 자체 메커니즘이 있습니다. 새로운 동의 상태를 백엔드에 편리하고 효율적으로 전달하기 위해 Adjust SDK는 다음 챕터 '앱 트래킹 인증 래퍼'에 설명된 앱 트래킹 인 메서드와 관련한 래퍼를 제공합니다.

### <a id="ad-ata-wrapper"></a>앱 트래킹 인증 래퍼

**참고**: 이 기능은 iOS 플랫폼에서만 사용할 수 있습니다.

Adjust SDK를 사용하면 앱 관련 데이터에 액세스하는 데 대한 사용자 인증을 요청할 수 있습니다. Adjust SDK에는 [requestTrackingAuthorizationWithCompletionHandler:](https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorizationwith?language=objc) 상에 빌드된 래퍼가 있습니다. 여기서 콜백 메서드를 정의하여 사용자의 선택에 대한 정보를 얻을 수도 있습니다. 또한 이 래퍼를 사용하면 사용자가 팝업 대화 상자에 응답하는 즉시 콜백 메서드를 사용하여 다시 전달됩니다. 또한 SDK는 사용자의 선택 정보를 백엔드에 알립니다. `NSUInteger` 값은 다음과 같은 의미로 콜백 메서드를 통해 전달됩니다.

- 0: `ATTrackingManagerAuthorizationStatusNotDetermined`
- 1: `ATTrackingManagerAuthorizationStatusRestricted`
- 2: `ATTrackingManagerAuthorizationStatusDenied`
- 3: `ATTrackingManagerAuthorizationStatusAuthorized`

이 래퍼를 사용하려면 다음과 같이 호출하면 됩니다:

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

### <a id="ad-skadn-framework"></a>SKAdNetwork 프레임워크

**참고**: 이 기능은 iOS 플랫폼에서만 사용할 수 있습니다.

Adjust iOS SDK v4.23.0 이상을 설치했으며 iOS 14에서 앱을 실행하는 경우, SKAdNetwork와의 통신이 기본적으로 활성화되며 비활성화하도록 설정할 수 있습니다. 활성화하면 SDK가 실행될때 SKAdNetwork 어트리뷰션에 대해 Adjust가 자동으로 등록합니다. 이벤트가 Adjust 대시보드에서 전환 값을 수신하도록 설정된 경우, Adjust 백엔드가 전환 값 데이터를 SDK로 전송합니다. 그런 다음 SDK가 전환 값을 설정합니다. Adjust가 SKAdNetwork 콜백 데이터를 수신한 후에는 해당 정보가 대시보드에 표시됩니다.

Adjust SDK가 SKAdNetwork와 자동으로 통신하지 않도록 하려면 구성 객체에 대해 다음 메서드를 호출하여 해당 메서드를 사용하지 않도록 설정할 수 있습니다:

```csharp
adjustConfig.deactivateSKAdNetworkHandling();
```

### <a id="ad-push-token"></a>푸시 토큰(삭제 추적)

푸시 토큰은 Audience Builder 및 클라이언트 콜백에 사용되며 삭제 및 재설치 추적 기능에 필요합니다.

Adjust에 푸시 알림 토큰을 전송하려면 앱의 푸시 알림 토큰을 획득할 때(또는 앱 푸시 알림 토큰의 값이 바뀔 때)`Adjust` 인스턴스에서 `setDeviceToken` 메서드를 호출하세요.

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

### <a id="ad-attribution-callback"></a>어트리뷰션 콜백

콜백을 설정하여 어트리뷰션의 변경 사항에 대한 알림을 받을 수 있습니다. Adjust는 다양한 어트리뷰션 소스를 고려하여 이러한 정보를 비동기식으로 제공합니다. 타사와 데이터를 공유하기 전에 [관련 어트리뷰션 데이터 정책][attribution_data]을 검토하세요. 

선택적 콜백을 애플리케이션에 추가하려면 다음 단계를 따르세요.

1. `Action 위임<AdjustAttribution>`의 서명이 포함된 메서드를 생성합니다.

2. `AdjustConfig` 객체를 생성한 다음, 이전에 생성한 메서드와 함께 `adjustConfig.setAttributionChangedDelegate`를 호출합니다. 같은 서명과 람다(lambda)를 함께 사용할 수도 있습니다.

3. `Adjust.prefab`을 사용하지 않고 다른 `GameObject`에 `Adjust.cs`를 추가한 경우,`GameObject`의 이름을 `AdjustConfig.setAttributionChangedDelegate`의 두 번째 파라미터로 전달해야 합니다.

콜백이 `AdjustConfig` 인스턴스를 사용하여 구성되었기 때문에 `Adjust.start`를 호출하기 전에 `adjustConfig.setAttributionChangedDelegate`를 호출해야 합니다.

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

SDK가 최종 어트리뷰션 데이터를 수신하면 콜백 함수가 호출됩니다. 콜백 함수 내에서 attribution 파라미터에 액세스할 수 있습니다. 그 속성에 대한 요약 정보는 다음과 같습니다.

- `string trackerToken` 현재 어트리뷰션의 트래커 토큰
- `string trackerName` 현재 어트리뷰션의 트래커 이름
- `string network` 현재 어트리뷰션의 네트워크 그룹화 수준
- `string campaign` 현재 어트리뷰션의 캠페인 그룹화 수준
- `string adgroup` 현재 어트리뷰션의 광고 그룹 그룹화 수준
- `string creative` 현재 어트리뷰션의 크리에이티브 그룹화 수준
- `string clickLabel` 현재 어트리뷰션의 클릭 레이블
- `string adid` Adjust 기기 식별자

### <a id="ad-ad-revenue"></a>광고 매출 추적

다음 메서드를 사용하여 Adjust SDK로 광고 매출 정보를 추적할 수 있습니다.

```csharp
Adjust.trackAdRevenue(source, payload);
```

전달해야 할 메서드 파라미터는 다음과 같습니다.

- `source` - 광고 매출 정보의 소스를 나타내는 `string` 객체
- `payload` - 스트링 형태의 광고 매출 JSON을 포함하는 `string` 객체

Adjust는 현재 다음의 `source` 파라미터 값을 지원합니다.

- `AdjustConfig.AdjustAdRevenueSourceMopub` - represents the [MoPub mediation platform][sdk2sdk-mopub]

### <a id="ad-subscriptions"></a>구독 추적

**참고**: 이 기능은 SDK 4.22.0 버전 이상에서만 사용할 수 있습니다.

App Store 및 Play 스토어 구독을 추적한 후 Adjust SDK로 유효성을 검증할 수 있습니다. 구독 항목이 구매되면 다음을 Adjust SDK로 호출하세요.

**App Store 구독의 경우**

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

**Play 스토어 구독의 경우**

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

App Store 구독에 대한 구독 추적 파라미터

- [가격](https://developer.apple.com/documentation/storekit/skproduct/1506094-price?language=objc)
- 통화([현지 가격](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) 객체의 [통화 코드](https://developer.apple.com/documentation/foundation/nslocale/1642836-currencycode?language=objc)를 전달해야 함)
- [거래 ID](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411288-transactionidentifier?language=objc)
- [영수증](https://developer.apple.com/documentation/foundation/nsbundle/1407276-appstorereceipturl)
- [거래 일자](https://developer.apple.com/documentation/storekit/skpaymenttransaction/1411273-transactiondate?language=objc)
- 판매 지역([현지 가격](https://developer.apple.com/documentation/storekit/skproduct/1506145-pricelocale?language=objc) 객체의 [국가 코드](https://developer.apple.com/documentation/foundation/nslocale/1643060-countrycode?language=objc)를 전달해야 함)

Play 스토어 구독에 대한 구독 추적 파라미터

- [가격](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpriceamountmicros)
- [통화](https://developer.android.com/reference/com/android/billingclient/api/SkuDetails#getpricecurrencycode)
- [sku](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsku)
- [주문 ID](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getorderid)
- [서명](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getsignature)
- [구매 토큰](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetoken)
- [구매 시간](https://developer.android.com/reference/com/android/billingclient/api/Purchase#getpurchasetime)

**참고:** Adjust SDK의 구독 추적 API를 사용하려면 모든 파라미터가 `string` 값으로 전달되어야 합니다. 위에 기술한 파라미터는 구독 추적 이전에 구독 객체로 전달되어야 합니다. Unity에서 인앱 구매를 처리하는 다양한 라이브러리는 각각 구독 구매가 완료된 후 위에 기술된 정보를 특정 형태로 반환해야 합니다. 인앱 구매를 위해 사용하고 있는 라이브러리에서 얻은 응답으로 이 파라미터가 어디에 있는지 확인한 후 그 값을 추출하고, Adjust API에 문자열 값으로 전달해야 합니다.

이벤트 추적과 마찬가지로 콜백 및 파트너 파라미터를 구독 객체에 연결할 수 있습니다.

**App Store 구독의 경우**

```csharp
AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
    price,
    currency,
    transactionId,
    receipt);
subscription.setTransactionDate(transactionDate);
subscription.setSalesRegion(salesRegion);

// add callback parameters
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

Adjust.trackAppStoreSubscription(subscription);
```

**Play 스토어 구독의 경우**

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
subscription.addCallbackParameter("key", "value");
subscription.addCallbackParameter("foo", "bar");

// add partner parameters
subscription.addPartnerParameter("key", "value");
subscription.addPartnerParameter("foo", "bar");

Adjust.trackPlayStoreSubscription(subscription);
```

### <a id="ad-session-event-callbacks"></a>세션 및 이벤트 콜백

콜백을 설정하여 성공적으로 추적되었거나 추적에 실패한 이벤트 및/또는 세션에 대한 알림을 받을 수 있습니다.

성공적으로 추적된 이벤트에 대한 콜백 함수를 추가하려면 다음 단계를 따르세요.

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

실패한 추적 이벤트의 경우 다음 콜백 함수를 추가합니다.

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

성공적으로 추적된 세션의 경우:

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

추적에 실패한 세션의 경우:

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

SDK가 패키지를 서버로 전송하려고 시도한 후에 콜백 함수가 호출됩니다. 콜백 내에서 콜백에 대한 반응 데이터 개체에 액세스할 수 있습니다. 세션 반응 데이터 속성에 대한 요약 정보는 다음과 같습니다.

- `string Message` 서버로부터의 메시지 또는 SDK에 의해 로깅된 오류.
- `string Timestamp` 서버의 타임스탬프.
- `string Adid` Adjust에서 제공하는 고유 기기 식별자.
- `Dictionary<string, object> JsonResponse` 서버로부터의 응답을 포함하는 JSON 객체.

두 이벤트 응답 데이터 개체는 다음을 포함합니다.

- `string EventToken` 추적된 패키지가 이벤트인 경우 해당 이벤트 토큰.
- `string CallbackId` 이벤트 오브젝트에 맞춤 설정된 콜백 ID.

두 이벤트 및 세션 실패 개체는 다음을 포함합니다.

- `bool WillRetry` 이후 패키지 재전송 시도가 있을 것임을 알립니다.

### <a id="ad-user-attribution"></a>사용자 어트리뷰션

이 콜백은 어트리뷰션 콜백과 마찬가지로 어트리뷰션 정보가 변경될 때마다 트리거됩니다. 필요할 때마다`Adjust` 인스턴스의 다음 메서드를 호출하여 사용자의 현재 어트리뷰션 정보에 액세스하세요.

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**참고**: 현재 어트리뷰션 정보는 Adjust 백엔드에서 앱 설치를 추적하고 어트리뷰션 콜백을 유발한 후 이용할 수 있습니다. SDK가 초기화되고 어트리뷰션 콜백이 실행되기 전까지는 사용자의 어트리뷰션 값에 액세스할 수 없습니다.

### <a id="ad-device-ids"></a>기기 ID

Adjust SDK를 사용하여 기기 식별자를 받아볼 수 있습니다.

### <a id="ad-idfa">iOS 광고 식별자

IDFA를 획득하려면`Adjust` 인스턴스의`getIdfa` 함수를 호출합니다.

```cs
string idfa = Adjust.getIdfa();
```

### <a id="ad-gps-adid"></a>Google Play 서비스 광고 식별자

Google 광고 식별자는 백그라운드 스레드에서만 읽을 수 있습니다. 다음과 같이 `Adjust` 인스턴스의 `getGoogleAdId` 메서드를 `Action<string>` 위임으로 호출하면 어떤 상황에서든 작동합니다.

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

이제 `googleAdId`. 변수로 Google 광고 식별자에 액세스할 수 있습니다.

### <a id="ad-amazon-adid"></a>Amazon 광고 식별자

Amazon 광고 식별자를 확보하려면 `Adjust` 인스턴스에서`getAmazonAdId` 메서드를 호출합니다.

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="ad-adid"></a>Adjust 기기 식별자

Adjust의 백엔드에서 여러분의 앱을 설치한 모든 기기를 위해 고유한 Adjust 기기 식별자(`adid`)를 생성합니다. 이 식별자를 받으려면`Adjust` 인스턴스에서 다음 메서드를 호출합니다.

```cs
String adid = Adjust.getAdid();
```

adid 정보는 Adjust 백엔드가 앱의 설치를 추적한 다음에만 확보할 수 있습니다. 따라서 SDK가 초기화되고 앱 설치가 추적되기 전까지는 adid 값에 액세스할 수 없습니다.

### <a id="ad-pre-installed-trackers"></a>사전 설치 트래커

Adjust SDK를 사용하여 본인의 앱이 사전에 설치된 기기가 있는 사용자를 인식하려면 다음 단계를 따르세요.

1. [대시보드]에서 새 트래커를 생성합니다.
2. `AdjustConfig`의 기본 트래커를 설정합니다.

  ```cs
  AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
  adjustConfig.setDefaultTracker("{TrackerToken}");
  Adjust.start(adjustConfig);
  ```

  `{TrackerToken}`을 2단계에서 생성한 트래커 토큰({abc123}`)으로 교체합니다.
  
대시보드에 `http://app.adjust.com/`을 포함한 트래커 URL이 표시되더라도 소스 코드에 전체 URL이 아닌 6~7글자의 토큰만 입력해야 합니다.

3. 앱을 빌드하고 실행합니다. 로그 출력에서 다음과 같은 라인이 표시됩니다.

    ```
    Default tracker: 'abc123'
    ```

### <a id="ad-offline-mode"></a>오프라인 모드

오프라인 모드는 Adjust 서버로의 전송을 연기하고 추적된 데이터가 이후에 전송되도록 유지합니다. Adjust SDK가 오프라인 모드이면 모든 정보가 파일에 저장되기 때문에 오프라인 모드에서 너무 많은 이벤트를 발생시키지 않도록 주의해야 합니다.

오프라인 모드를 활성화하려면 `setOfflineMode`를 호출하고 파라미터를 `true`로 설정합니다.

```cs
Adjust.setOfflineMode(true);
```

오프라인 모드를 비활성화하려면 `setOfflineMode`를 호출하고 파라미터를 `false`로 설정합니다. Adjust SDK가 다시 온라인 모드가 되면 저장된 모든 정보가 정확한 시간 정보와 함께 Adjust 서버로 전송됩니다.

이 설정은 세션 간에 유지되지 않습니다. 즉, 앱이 오프라인 모드에서 종료되었더라도 Adjust SDK는 항상 온라인 모드로 시작됩니다.

### <a id="ad-disable-tracking"></a>추적 비활성화

Adjust 인스턴스의 setEnabled 메서드를 호출하고 활성화된 파라미터를 false로 설정하여 Adjust SDK의 추적을 비활성화할 수 있습니다. 이 설정은 세션 간에 유지되지만, 첫 세션 이후에만 활성화될 수 있습니다.

```cs
Adjust.setEnabled(false);
```

Adjust SDK가 Adjust 인스턴스의 IsEnabled 메서드를 사용하여 현재 활성화된 경우 검증할 수 있습니다. 언제든지 `setEnabled` 메서드를 호출하고 `enabled` 파라미터를`true`로 설정하여 Adjust SDK를 활성화할 수 있습니다.

### <a id="ad-event-buffering"></a>이벤트 버퍼링

앱이 이벤트 추적을 많이 사용하는 경우, 일부 네트워크 요청을 연기하여 네트워크 요청을 1분에 한 번씩 일괄로 보낼 수 있습니다. `AdjustConfig` 인스턴스를 통해 이벤트 버퍼링을 활성화할 수 있습니다.

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

아무것도 설정되지 않으면 이벤트 버퍼링이 기본적으로 비활성화됩니다.

### <a id="ad-background-tracking"></a>백그라운드 추적

Adjust SDK는 기본적으로 앱이 백그라운드에서 작동하는 동안 네트워크 요청 전송을 일시 중지하도록 설정되어 있습니다. 이 설정은 다음을 통해 `AdjustConfig` 인스턴스에서 변경할 수 있습니다.

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="ad-gdpr-forget-me"></a>GDPR 잊혀질 권리

EU의 개인정보보호법(GDPR) 제 17조에 따라, 사용자는 잊혀질 권리(Right to be Forgotten)를 행사했음을 Adjust에 알릴 수 있습니다. 다음 메서드를 호출하면 Adjust SDK가 잊혀질 권리에 대한 사용자의 선택과 관련된 정보를 Adjust 백엔드에 보냅니다.

```cs
Adjust.gdprForgetMe();
```

이 정보를 수신한 후 Adjust는 해당 사용자의 데이터를 삭제하며 Adjust SDK는 해당 사용자에 대한 추적을 중지합니다. 이 기기로부터의 요청은 향후 Adjust에 전송되지 않습니다.

이러한 설정은 테스트에서 적용하더라도 영구적으로 유지되며, 취소할 수 없습니다.


### <a id="ad-disable-third-party-sharing"></a>특정 사용자의 경우 타사 공유 비활성화

이제 사용자가 마케팅 목적으로 파트너와 데이터가 공유되지 않도록 중단할 수 있는 권리를 행사했으나 통계 목적으로는 공유할 수 있도록 허용한 경우, 이를 Adjust에 알릴 수 있습니다. 

다음 메서드를 호출하여 Adjust SDK가 데이터 공유 비활성화에 대한 사용자의 선택과 관련된 정보를 Adjust 백엔드에 보냅니다:

```csharp
Adjust.disableThirdPartySharing();
```

이 정보를 수신하면 Adjust는 특정 사용자의 데이터를 파트너와 공유하는 것을 차단하고 Adjust SDK는 계속 정상적으로 작동합니다.

## 테스트 및 문제 해결

### <a id="tt-debug-ios"></a>iOS에서 정보 디버그

빌드 후 스크립트가 있어도 프로젝트를 즉시 실행하지 못할 수 있습니다.

필요한 경우 dSYM 파일을 비활성화하세요. `Project Navigator`에서`Unity-iPhone` 프로젝트를 선택합니다. `Build Settings` 탭을 클릭하고 `debug information`을 검색합니다. `Debug Information Format` 또는 `DEBUG_INFORMATION_FORMAT` 옵션이 표시됩니다. 이 옵션의 설정을 `DWARF with dSYM File`에서 `DWARF`로 변경합니다.


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

## 라이센스

### <a id="license"></a>라이센스 계약

mod_pbxproj.py 파일은 Apache 라이센스 버전 2.0('라이센스') 하에 사용이 허가됩니다.
이 파일은 반드시 라이센스를 준수하여 사용해야 합니다.
라이센스 사본은 http://www.apache.org/licenses/LICENSE-2.0에서 받을 수 있습니다.

Adjust SDK는 MIT 라이센스 하에 사용이 허가됩니다.

Copyright (c) 2012-2020 Adjust GmbH, http://www.adjust.com

다음 조건하에서 본 소프트웨어와 관련 문서 파일
(이하 "소프트웨어")의 사본을 보유한 제3자에게 소프트웨어의
사용, 복사, 수정, 병합, 게시, 배포, 재실시권 및/또는 사본의 판매 등을 포함하여
소프트웨어를 제한 없이 사용할 수 있는 권한을 무료로 부여하며,
해당 제3자는 소프트웨어를 보유한 이에게
이러한 이용을 허가할 수 있습니다.

본 소프트웨어의 모든 사본 또는 상당 부분에
위 저작권 공고와 본 권한 공고를 포함해야 합니다.

소프트웨어는 "있는 그대로" 제공되며,
소프트웨어의 상품성과 특정 목적에의 적합성 및 비 침해성에 대해
명시적이거나 묵시적인 일체의 보증을 하지 않습니다. 저자 또는 저작권자는
본 소프트웨어나 이의 사용 또는 기타 소프트웨어 관련 거래로 인해
발생하는 모든 클레임, 손해 또는 기타 법적 책임에 있어서
계약 또는 불법 행위와 관련된 소송에 대해 어떠한 책임도 부담하지
않습니다.
