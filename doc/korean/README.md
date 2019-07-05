## 요약

이 항목에서는 Adjust™의 Unity SDK에 대해 설명합니다. 이 SDK는 iOS, 안드로이드, Windows Store 8.1, Windows Phone 8.1, 그리고 Windows 10 대상을 지원합니다. Adjust에 대한 자세한 내용은 [adjust.com]을 참조하십시오.

**참고**: 버전 4.12.0 출시와 더불어 Adjust Unity SDK는 Unity 5 이상의 버전과 호환이 가능합니다.

Read this in other languages: [English][en-readme], [中文][zh-readme], [日本語][ja-readme], [한국어][ko-readme]

## 목차

* [기본 연동](#basic-integration)
   * [SDK 받기](#sdk-get)
   * [프로젝트에 SDK 추가](#sdk-add)
   * [앱에 SDK 연동](#sdk-integrate)
   * [Adjust 로그 기록(logging)](#adjust-logging)
   * [Google Play 서비스](#google-play-services)
   * [Proguard 설정](#sdk-proguard)
   * [Google Install Referrer](#install-referrer)
   * [빌드 후 프로세스](#post-build-process)
      * [iOS 빌드 후 프로세스](#post-build-ios)   
      * [안드로이드 빌드 후 프로세스](#post-build-android)
* [부가 기능](#additional-features)
   * [이벤트 추적](#event-tracking)
      * [수익 추적](#revenue-tracking)
      * [수익 중복 제거](#revenue-deduplication)
      * [인앱 구매 검증](#iap-verification)
      * [콜백 파라미터](#callback-parameters)
      * [파트너 파라미터](#partner-parameters)
      * [콜백 ID](#callback-id)
    * [세션 파라미터](#session-parameters)
      * [세션 콜백 파라미터](#session-callback-parameters)
      * [세션 파트너 파라미터](#session-partner-parameters)
      * [예약 시작(delay start)](#delay-start)
    * [어트리뷰션 콜백](#attribution-callback)
    * [광고 매출 트래킹](#ad-revenue)
    * [세션 및 이벤트 콜백](#session-event-callbacks)
    * [추적 사용 중지](#disable-tracking)
    * [오프라인 모드](#offline-mode)
    * [이벤트 버퍼링(buffering)](#event-buffering)
    * [GDPR(일반 개인정보 보호법) 상의 잊힐 권리](#gdpr-forget-me)
    * [SDK 서명](#sdk-signature)
    * [백그라운드 추적](#background-tracking)
    * [기기 ID](#device-ids)
      * [iOS 광고 식별자(identifier)](#di-idfa) 
      * [Google Play Services 광고 식별자](#di-gps-adid)
      * [Amazon 광고 식별자](#di-amz-adid)
      * [Adjust 기기 식별자](#di-adid)
    * [사용자 어트리뷰션](#user-attribution)
    * [푸시 토큰(push token)](#push-token)
    * [사전 설치 트래커(pre-installed trackers)](#pre-installed-trackers)
    * [딥링크](#deeplinking)
        * [기본 딥링크](#deeplinking-standard)
        * [지연된(deferred) 딥링크](#deeplinking-deferred)
        * [안드로이드 앱 용 딥링크](#deeplinking-android)
        * [iOS 앱에서 딥링크 관리](#deeplinking-ios)
* [문제 해결](#troubleshooting)
    * [iOS 정보 디버깅](#ts-debug-ios)
* [라이선스](#license)

## <a id="basic-integration">기본 연동

다음은 Adjust SDK를 Unity3d 프로젝트와 연동하기 위해 최소한으로 수행해야 하는 절차입니다.

### <a id="sdk-get">SDK 받기

[릴리스 페이지][releases]에서 최신 버전을 다운로드합니다. 페이지에는 Unity 패키지 두 가지가 있습니다.

### <a id="sdk-add">프로젝트에 SDK 추가

프로젝트를 Unity Editor에서 열고 `Assets → Import Package → Custom Package`로 이동하여 다운로드한 Unity 패키지 파일을 선택합니다.

![][import_package]

### <a id="sdk-integrate">앱에 SDK 연동

`Assets/Adjust/Adjust.prefab`에 있는 prefab을 첫 번째 화면에 추가합니다.

추가한 prefab의 Inspector 메뉴에서 Adjust 스크립트의 파라미터를 편집합니다.

![][adjust_editor]

Adjust prefab에서는 다음 옵션을 설정할 수 있습니다.

 * [수동으로 시작 Start Manually](#start-manually)
 * [이벤트 버퍼링 (Event Buffering)](#event-buffering)
 * [지연된 딥링크 구현 (Launch Deferred Deep Link)](#deeplinking-deferred-open)
 * [앱 토큰 (App Token)](#app-token)
 * [로그 레벨 (Log Level)](#adjust-logging)
 * [환경 (Environment)](#environment)
   
<a id="app-token">`{YourAppToken}`을 앱 토큰으로 변경합니다. 앱 토큰은 [대시보드][adjust.com]에서 찾을 수 있습니다.

<a id="environment">앱 빌드가 테스트 용인지 제작 용인지에 따라 `Environment`를 다음 값 중 하나로 변경해야 합니다.

```
    'Sandbox'
    'Production'
```

**중요:** 이 값은 앱을 테스트하는 경우에만 `Sandbox`로 설정해야 합니다. 앱을 게시하기 직전에 environment를 `Production`으로 설정해야 합니다. 다시 테스트할 때 `Sandbox`로 재설정하십시오.

이 environment는 실제 트래픽과 테스트 장치의 테스트 트래픽을 구별하기 위해 사용합니다. 이 값은 항상 유의미하게 유지해야 합니다! 특히 매출 추적 시에 중요합니다.

앱의 `Awake` 이벤트에서 Adjust SDK를 자동 시작하지 않으려면 `Start Manually` 상자에 체크합니다. 이 옵션을 선택하면 코드 내에서 Adjust ADK를 직접 초기화하고 시작해야 합니다. `AdjustConfig` 개체를 매개변수로 하여 `Adjust.start` 메서드를 호출하면 Adjust SDK를 시작할 수 있습니다.

이 옵션 등이 있는 버튼 메뉴 화면의 예를 보려면 `Assets/Adjust/ExampleGUI/ExampleGUI.unity`에 있는 화면 예제를 여십시오. 이 화면의 원본은 `Assets/Adjust/ExampleGUI/ExampleGUI.cs`에 있습니다.

### <a id="adjust-logging">Adjust 로그 기록

다음 항목의 `Log Level` 값을 조정하여 로그 기록 분량을 늘이거나 줄일 수 있습니다.

 * `Verbose` - 로그 전체 활성화
 * `Debug` - 더 많은 로그 활성화
 * `Info` - 디폴트
 * `Warn` - F 로그 기록 비활성화
 * `Error` - 경고(warnings) 역시 비활성화
 * `Assert` - 오류 (errors) 역시 비활성화
 * `Suppress` - 로그 전체 비활성화

로그 기록 출력 전체를 비활성화하고 Adjust SDK를 코드에서 수동으로 초기화하려면, 로그 레벨을 Suppress에 두고 `AdjustConfig` 개체를 생성자(constructor)로 써야 합니다. 이 개체는 suppress 로그 레벨을 유지할 지 아닐 지를 가리키는 부울자료(boolean) 파라미터를 받습니다. 

```cs
string appToken = "{YourAppToken}";
string environment = AdjustEnvironment.Sandbox;

AdjustConfig config = new AdjustConfig(appToken, environment, true);
config.setLogLevel(AdjustLogLevel.Suppress);

Adjust.start(config);
```

대상이 윈도우 기반일 때 `released` 모드에서 Adjust 라이브러리로부터 모은 로그 기록을 보려면, `debug` 모드에서 테스트하는 동안 로그 출력을 앱으로 재전송해야 합니다. 

SDK를 시작하기 전 `AdjustConfig` 인스턴스에서 `setLogDelegate` 메서드를 호출하세요. 

```cs
//...
adjustConfig.setLogDelegate(msg => Debug.Log(msg));
//...
Adjust.start(adjustConfig);
```

### <a id="google-play-services">Google Play 서비스

2014년 8월 1일 부로 Google Play Store의 앱은 고유 장치 식별을 위해 [Google 광고 ID][google_ad_id]를 사용해야 합니다. Adjust SDK에서 Google 광고 ID를 사용할 수 있게 하려면 [Google Play 서비스][google_play_services]를 연동해야 합니다. 아직 연동하지 않았다면 `google-play-services_lib` 폴더를 Unity 프로젝트의 `Assets/Plugins/Android` 폴더로 복사하세요. 그러면 앱을 작성한 후 Google Play 서비스가 연동됩니다.

`google-play-services_lib`는 Android SDK의 일부분으로, 이미 설치했을 수 있습니다.

Android SDK는 두 가지 방법으로 다운로드할 수 있습니다. `Android SDK Manager`가 있는 도구를 사용하는 경우 `Android SDK Tools`를 다운로드해야 합니다. 설치 후 `SDK_FOLDER/extras/google/google_play_services/libproject/` 폴더에서 라이브러리를 찾을 수 있습니다.

![][android_sdk_location]

안드로이드 SDK Manager가 있는 도구를 사용하지 않는 경우 [공식 페이지][android_sdk_download]에서 안드로이드 SDK의 단독 실행형 버전을 다운로드해야 합니다. 이 버전을 다운로드하면 안드로이드 SDK Tools가 포함되지 않은 기본 안드로이드 SDK 버전만 갖게 됩니다. 자세한 내용은 Google에서 제공하는 `SDK Readme.txt` 파일을 참조하십시오. 이 파일은 안드로이드 SDK 폴더에 있습니다.

**업데이트:** 새 안드로이드 SDK 버전을 설치한 상태라면, Google이 SDK 루트 폴더 내의 Google Play Services 폴더 구조를 변경했습니다. 위에 설명한 구조는 이제 아래와 같이 보일 것입니다. 

![][android_sdk_location_new]

이제 예전처럼 Google Play 서비스 라이브러리 전체에 대해서가 아니라 개별 부분에만 접근 권한을 가지고 있을 수 있으므로, Google Play 서비스 라이브러리에서 Adjust SDK에게 필요한 basement 부분을 추가하는 게 좋습니다. `Assets/Plugins/Android` 폴더에 `play-services-basement-x.y.z.aar` 파일을 추가하면 Adjust SDK가 Google Play 서비스에서 필요한 부분을 성공적으로 연동할 수 있습니다.

**업데이트**: Google Play 서비스 라이브러리 15.0.0 이후 버전부터 Google은 Google 광고 식별자 획득에 필요한 클래스를 [`play-services-ads-identifier`](https://mvnrepository.com/artifact/com.google.android.gms/play-services-ads-identifier) 패키지로 옮겼습니다.  15.0.0 버전 이상의 Google Play 서비스 라이브러리를 사용하는 경우, 이 패키지를 앱에 추가하시기 바랍니다. 또한, 사용 중인 Unity IDE 버전에 따라 Google 광고 식별자를 읽어들일 때 약간의 차이가 있을 수 있음을 알려드립니다. 앱에 Google Play 서비스 의존성을 추가하는 방식 및 사용하는 버전에 관계 없이 **Adjust SDK를 통해 Google 광고 식별자를 제대로 획득하고 있는지 테스트해야 합니다**.

Adjust SDK에서 Google 광고 식별자를 수신하고 있는지 확인하려면 `sandbox` 모드에서 구동되도록 SDK를 설정하고 로그 수준을 `verbose`로 지정한 후, 앱을 실행합니다. 세션이나 앱 내 특정 이벤트를 추적하고, 추적이 완료되면 verbose 로그에서 읽어 들이는 파라미터 리스트를 확인합니다. `gps_adid`라는 파라미터가 확인되면, 이제 Adjust SDK가 Google 광고 식별자를 읽을 수 있습니다.

Google 광고 식별자를 읽는 데 문제가 발생하거나 궁금한 점이 있다면, Github repository에서 issue를 오픈해 주시거나 support@adjust.com으로 메일 주시기 바랍니다.

### <a id="sdk-proguard">빌드 후 프로세스

Proguard를 사용 중인 경우 다음 행을 Proguard 파일에 추가합니다.

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

### <a id="install-referrer"></a>Google Install Referrer

Adjust는 앱 설치를 소스에 제대로 어트리뷰트하기 위해 **Install referrer** 관련 정보를 필요로 합니다. 이 정보는 **Google Play Referrer API**를 사용하거나 브로드캐스트 리시버(broadcast receiver)로 **Google Play Store intent**를 잡아 얻을 수 있습니다. 

**중요**: Google Play referrer API는 보다 안전하고 믿을만한 install referrer 정보 획득 방식을 제공하는 동시에 클릭 인젝션(click injection)으로부터 어트리뷰션 제공자를 보호할 목적으로 Google이 새롭게 도입한 방식입니다. 따라서 앱에서 지원할 것을 **강력히 권장합니다**. Google Play Store intent는 이보다 덜 안전한 install referrer 정보 획득 방식입니다. 당분간은 새로운 Google Play referrer API와 공존하지만 향후에는 더 이상 사용하지 않을 예정입니다.

install referrer를 지원하려면 Maven 리포지토리에서 [install referrer 라이브러리][install referrer library]를 다운로드하십시오. `Plugins/Android` 폴더에 AAR 파일을 넣기만 하면 됩니다. Adjust가 빌드 후 프로세스로 `AndroidManifest.xml` 조정을 처리해 드립니다. 

### <a id="post-build-process">빌드 후 프로세스

Adjust unity 패키지는 빌드 프로세스를 원활하게 만들기 위해 빌드 후 프로세스를 수행하여 SDK가 제대로 작동하도록 해줍니다. 

이 과정은 `AdjustEditor.cs`에 있는 `OnPostprocessBuild` 메서드가 수행합니다. iOS에서는 `Unity 5 이상` 패키지를 사용하며 `iOS build support`를 다운로드한 상태여야 합니다. 

이 스크립트는 Unity IDE 콘솔 결과 창에 로그 결과 메시지를 기록합니다.
   
### <a id="post-build-ios">**iOS 빌드 후 프로세스**

iOS 빌드 후 프로세스에서는 생성된 Xcode 프로젝트를 다음과 같이 변경합니다.

 * `iAd.framework` 추가 (Apple 검색 광고 추적에 필요)
 * `AdSupport.framework` 추가 (IDFA를 읽어들이는 데 필요)
 * `CoreTelephony.framework` 추가 (MMC 및 MNC를 읽어들이는 데 필요)
 * 또다른 링커 플래그인 `-ObjC` 추가 (빌드 시 Adjust Objective-C 카테고리를 인식하는 데 필요)
 * `Objective-C exceptions`를 활성화

### <a id="post-build-android">**안드로이드 빌드 후 프로세스**

안드로이드 빌드 후 프로세스는 `Assets/Plugins/Android/`에 있는 `AndroidManifest.xml` 파일에서 변경사항을 수행합니다. 

안드로이드 빌드 후 프로세스에서는 먼저 안드로이드 플러그인 폴더에서 `AndroidManifest.xml` 파일의 존재여부를 확인합니다. `Assets/Plugins/Android/`에 이 파일이 없으면 호환 가능한 매니페스트 파일인 `AdjustAndroidManifest.xml`을 만듭니다. 해당 파일이 이미 있다면 다음 사항을 확인하고 변경합니다.

 * `INTERNET` 권한 추가 (인터넷 연결에 필요)
 * `ACCESS_WIFI_STATE` 권한 추가 (Play Store 등을 통해 앱을 배포하지 않을 경우에 필요)
 * `ACCESS_NETWORK_STATE` 권한 추가 (MMC 및 MNC를 읽어들이는 데 필요)
 * `BIND_GET_INSTALL_REFERRER_SERVICE` 권한 추가 (Google 설치 referrer API 작동에 필요)
 * Adjust 브로드캐스트 수신기 추가 (Google Play Store 인텐트를 통해 설치 referrer 정보를 얻는데 필요). 자세한 내용은 [안드로이드 SDK README][android] 페이지를 참조하십시오. `INSTALL_REFERRER` 인텐트를 취급하는 **자신의 브로드캐스트 리시버**를 사용하고 있는 경우 매니페스트 파일에 Adjust 브로드캐스트 수신기를 추가하지 않아도 된다는 사실을 명심하십시오. 지워도 무방하지만, [안드로이드 설명서][android-custom-receiver]에 설명한 대로  Adjust 브로드캐스트 수신기에 대한 호출을 자신의 브로드캐스트 리시버 안에 추가해야 합니다.

## <a id="additional-features">추가 기능

Adjust SDK를 프로젝트에 연동한 후에는 다음 기능을 사용할 수 있습니다.

### <a id="event-tracking">이벤트 추적

원하는 이벤트를 Adjust에 알릴 수 있습니다. 버튼의 모든 탭을 추적하려는 경우 [대시보드][adjust.com]에서 새 이벤트 토큰을 만들기만 하면 됩니다. 예를 들어 이벤트 토큰이 `abc123`일 경우 버튼의 클릭 핸들러 메서드에 다음 행을 추가하여 클릭을 추적할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-tracking">매출 추적

사용자가 광고를 누르거나 인앱 구매를 하여 수익이 발생할 수 있는 경우 이벤트를 사용하여 해당 매출을 추적할 수 있습니다. 예를 들어 광고 탭 한 번에 0.01 유로의 수익이 발생할 경우 수익 이벤트를 다음과 같이 추적할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");
adjustEvent.setRevenue(0.01, "EUR");
Adjust.trackEvent(adjustEvent);
```

#### <a id="revenue-deduplication"></a>매출 중복 제거

거래 ID를 선택 사항으로 추가하여 매출 중복 추적을 피할 수 있습니다. 가장 최근에 사용한 거래 ID 10개를 기억하며, 중복 거래 ID로 이루어진 수익 이벤트는 집계하지 않습니다. 인앱 구매 추적 시 특히 유용합니다. 사용 예는 아래에 나와 있습니다.

인앱 구매를 추적할 경우, 거래가 완료되고 아이템을 구매했을 때만 `trackEvent`를 호출해야 한다는 사실을 기억하십시오. 그렇게 해야 실제로 발생하지 않은 수익을 추적하는 일을 피할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setRevenue(0.01, "EUR");
adjustEvent.setTransactionId("transactionId");

Adjust.trackEvent(adjustEvent);
```

#### <a id="iap-verification">인앱 구매 검증

Adjust의 서버 측 수신 확인 도구인 구매 검증(Purchase Verification)을 사용하여 앱에서 이루어지는 구매의 유효성을 확인하려면 `Unity purchase SDK`를 확인하십시오. 자세한 내용은 [여기][unity-purchase-sdk]를 참조하십시오.

#### <a id="callback-parameters">콜백 파라미터

[대시보드][adjust.com]에서 해당 이벤트의 콜백 URL을 등록할 수도 있으며, 그러면 이벤트를 추적할 때마다 GET 요청이 해당 URL로 전송됩니다. 이 경우 몇 개의 키-값 쌍을 개체에 포함하여 `trackEvent` 메서드에 전달할 수도 있습니다. 그러면 명명된 파라미터가 콜백 URL에 추가됩니다.

예를 들어 이벤트 토큰 `abc123`을 사용하여 이벤트에 대해 `http://www.adjust.com/callback` URL을 등록하고 다음 행을 실행한다고 가정해 봅시다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addCallbackParameter("key", "value");
adjustEvent.addCallbackParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

이 경우에는 이벤트를 추적하기 위해 다음 주소로 요청을 전송합니다.

```
http://www.adjust.com/callback?key=value&foo=bar
```

Adjust에서는 iOS의 `{idfa}` 또는 안드로이드의 `{gps_adid}`처럼 파라미터 값으로 사용할 수 있는 다양한 자리 표시자(placeholder)를 지원합니다. 그 결과로 생성된 콜백에서는 iOS의 경우 `{idfa}` 자리 표시자가 현재 장치의 광고주 ID로 변경되며, 안드로이드의 경우 `{gps_adid}`가 현재 장치의 안드로이드 Google Play 서비스 ID로 변경됩니다. 사용자 지정 파라미터는 저장이 되지 않으며 콜백에만 추가된다는 사실도 기억해 주십시오. 이벤트 콜백을 등록하지 않은 경우 해당 파라미터는 읽을 수 없습니다.

#### <a id="partner-parameters">파트너 파라미터

Adjust 대시보드에서 활성화된 연동의 경우 네트워크 파트너로 전송할 파라미터도 추가할 수 있습니다.

위에서 설명한 콜백 파라미터와 비슷하지만, `AdjustEvent` 인스턴스에서 `addPartnerParameter` 메서드를 호출해야 추가할 수 있습니다.

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.addPartnerParameter("key", "value");
adjustEvent.addPartnerParameter("foo", "bar");

Adjust.trackEvent(adjustEvent);
```

특별 파트너와 그 연동에 대한 자세한 내용은 [특별 파트너 설명서][special-partners]를 참조하십시오.

### <a id="callback-id"></a>콜백 ID

추적하고자 하는 각 이벤트에 개별 스트링 ID를 따로 붙일 수도 있습니다. 나중에 이벤트 성공/실패 콜백에서 해당 ID에 전달하여 이벤트 트래킹의 성공 또는 실패 여부를 추적할 수 있게 해 줍니다. `AdjustEvent` 인스턴스에서  `setCallbackId` 메서드를 호출하여 설정할 수 있습니다:

```cs
AdjustEvent adjustEvent = new AdjustEvent("abc123");

adjustEvent.setCallbackId("Your-Custom-Id");

Adjust.trackEvent(adjustEvent);
```

### <a id="session-parameters">세션 파라미터

일부 파라미터는 Adjust SDK 이벤트 및 세션 발생시마다 전송을 위해 저장합니다. 어느 파라미터든 한 번 저장하면 로컬에 바로 저장되므로 매번 새로 추가할 필요가 없습니다. 같은 파라미터를 두 번 저장해도 효력이 없습니다.

설치 시에도 전송할 수 있게 할 목적으로 이들 세션 파라미터는 Adjust SDK 런칭 전에도 호출이 가능합니다. 설치 시에 전송하지만 필요한 값은 런칭 후에야 들어갈 수 있게 하고 싶다면, Adjust SDK 런칭 시 [예약 시작](#delay-start)을 걸 수 있습니다. 

### <a id="session-callback-parameters">세션 콜백 파라미터

[이벤트](#callback-parameters)에 등록한 콜백 파라미터는 Adjust SDK 전체 이벤트 및 세션 시 전송을 목적으로 저장할 수 있습니다.

세션 콜백 파라미터는 이벤트 콜백 파라마터와 비슷한 인터페이스를 지녔지만, 이벤트에 키와 값을 추가하는 대신 `Adjust` 인스턴스에 있는 `addSessionCallbackParameter` 메서드를 호출하여 추가합니다.

```cs
Adjust.addSessionCallbackParameter("foo", "bar");
```

세션 콜백 파라미터는 이벤트에 추가된 콜백 파라미터와 합쳐지며, 이벤트에 추가된 콜백 파라미터가 우선권을 지닙니다. 그러나 세션에서와 같은 키로 이벤트에 콜백 파라미터를 추가한 경우 새로 추가한 콜백 파라미터가 우선권을 가집니다.

원하는 키를 `Adjust` 인스턴스의 `removeSessionCallbackParameter` 메서드로 전달하여 특정 세션 콜백 파라미터를 제거할 수 있습니다.

```cs
Adjust.removeSessionCallbackParameter("foo");
```

세션 콜백 파라미터의 키와 값을 전부 없애고 싶다면 `Adjust` 인스턴스의 `resetSessionCallbackParameters` 메서드로 재설정하면 됩니다.

```cs
Adjust.resetSessionCallbackParameters();
```

### <a id="session-partner-parameters">세션 파트너 파라미터

Adjust SDK 내 모든 이벤트 및 세션에서 전송되는 [세션 콜백 파라미터](#session-callback-parameters)가 있는 것처럼, 세션 파트너 파라미터도 있습니다.

이들 파라미터는 Adjust [대시보드][adjust.com]에서 연동 및 활성화된 네트워크 파트너에게 전송할 수 있습니다.

세션 파트너 파라미터는 이벤트 파트너 파라미터와 인터페이스가 비슷하지만, 이벤트에 키와 값을 추가하는 대신 `Adjust` 인스턴스에서 `addSessionPartnerParameter` 메서드를 호출하여 추가합니다.

```cs
Adjust.addSessionPartnerParameter("foo", "bar");
```

세션 파트너 파라미터는 이벤트에 추가한 파트너 파라미터와 합쳐지며, 이벤트에 추가된 파트너 파라미터가 우선순위를 지닙니다. 그러나 세션에서와 같은 키로 이벤트에 파트너 파라미터를 추가한 경우, 새로 추가한 파트너 파라미터가 우선권을 가집니다.

원하는 키를 `Adjust` 인스턴스의 `removeSessionPartnerParameter` 메서드로 전달하여 특정 세션 파트너 파라미터를 제거할 수 있습니다.

```cs
Adjust.removeSessionPartnerParameter("foo");
```

세션 파트너 파라미터의 키와 값을 전부 없애고 싶다면 `Adjust` 인스턴스의 `resetSessionPartnerParameters` 메서드로 재설정하면 됩니다.

```cs
Adjust.resetSessionPartnerParameters();
```

### <a id="delay-start">예약 시작

Adjust SDK에 예약 시작을 걸면 앱이 고유 식별자 등의 세션 파라미터를 얻어 인스톨 시에 전송할 시간을 벌 수 있습니다.

`AdjustConfig` 인스턴스의 `setDelayStart` 메서드에서 예약 시작 시각을 초 단위로 설정하세요.

```cs
config.setDelayStart(5.5);
```

이 경우 Adjust SDK는 최초 인스톨 세션 및 생성된 이벤트를 5.5초간 기다렸다가 전송합니다. 이 시간이 지난 후, 또는 그 사이에 `Adjust.sendFirstPackages()`을 호출했을 경우 모든 세션 파라미터가 지연된 인스톨 세션 및 이벤트에 추가되며 Adjust SDK는 원래대로 돌아옵니다.

**Adjust SDK의 최대 지연 예약 시작 시간은 10초입니다**.

### <a id="attribution-callback">어트리뷰션 콜백

트래커 어트리뷰션 변경 알림을 받기 위해 콜백을 등록할 수 있습니다. 어트리뷰션에서 고려하는 소스가 각각 다르기 때문에 이 정보는 동시간에 제공할 수 없습니다. 다음 단계를 거쳐 어플리케이션에 어트리뷰션 콜백을 시행하세요.

[해당 어트리뷰션 데이터 정책][attribution_data]을 반드시 고려하세요.

1. `Action<AdjustAttribution>` 위임 서명(signature)을 지닌 메서드를 만듭니다.

2. `AdjustConfig` 개체를 만든 후 이전에 만든 메서드를 사용하여`adjustConfig.setAttributionChangedDelegate`를 호출합니다. 서명이 같은 lambda도 사용할 수 있습니다.

3. `Adjust.prefab`을 사용하는 대신 `Adjust.cs` 스크립트가 다른 `GameObject`에 추가되었다면, 해당 `GameObject`의 이름을 `AdjustConfig.setAttributionChangedDelegate`의 두 번째 파라미터로 전달해야 합니다.

콜백은 AdjustConfig 인스턴스를 사용하여 구성되므로, `Adjust.start`를 호출하기 전에 `adjustConfig.setAttributionChangedDelegate`를 호출해야 합니다.

SDK에서 최종 어트리뷰션 데이터를 수신하면 콜백 함수가 호출됩니다. 콜백 함수를 통해 `attribution` 파라미터에 액세스할 수 있습니다. 어트리뷰션 파라미터의 개요는 다음과 같습니다.

- `string trackerToken` 현재 설치된 트래커 토큰.
- `string trackerName` 현재 설치된 트래커 이름.
- `string network` 현재 설치된 네트워크 그룹화 기준.
- `string campaign` 현재 설치된 캠페인 그룹화 기준.
- `string adgroup` 현재 설치도의 ad group 그룹화 기준.
- `string creative` 현재 설치의 크리에이티브 그룹화 기준.
- `string clickLabel` 현재 설치의 클릭 레이블.
- `string adid` Adjust 장치 식별자.

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

### <a id="ad-revenue"></a>광고 매출 트래킹

다음 메서드를 호출하여 Adjust SDK로 광고 매출 정보를 트래킹할 수 있습니다.

```csharp
Adjust.trackAdRevenue(source, payload);
```

전달해야 하는 메서드 파라미터는 다음과 같습니다.

- `source` - 광고 매출 정보의 소스를 나타내는`string`객체
- `payload` - 광고 매출 JSON을 포함하는`string`객체

애드저스트는 현재 다음의 `source` 파라미터 값을 지원합니다.

- `AdjustConfig.AdjustAdRevenueSourceMopub` - MoPub 미디에이션 플랫폼을 나타냄(자세한 정보는 [연동 가이드][sdk2sdk-mopub] 확인)

### <a id="session-event-callbacks">세션 및 이벤트 콜백

콜백을 등록하여 이벤트 및/또는 세션 추적 성공 또는 실패 시 알림을 받을 수 있습니다.

동일한 단계에 따라 이벤트 추적 성공 시 다음 콜백 함수를 구현하십시오.

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

이벤트 추적 실패 시 콜백 함수입니다.

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

세션 추적 성공의 경우입니다.

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

그리고 세션 추적 실패의 경우입니다.

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

콜백 함수는 SDK에서 서버로 패키지를 보내려고 시도한 후에 호출됩니다. 
콜백에서는 콜백 전용 응답 데이터 개체에 액세스할 수 있습니다. 
세션 응답 데이터 속성에 대한 개요는 다음과 같습니다.

- `string Message` 서버에서 전송한 메시지 또는 SDK에 기록된 오류.
- `string Timestamp` 서버에서 전송한 데이터의 타임스탬프.
- `string Adid` Adjust가 제공하는 고유 장치 식별자.
- `Dictionary<string, object> JsonResponse` 서버에서 전송한 응답이 있는 JSON 개체.

두 개의 이벤트 응답 데이터 개체에는 다음 정보가 들어갑니다.

- `string EventToken` 추적한 패키지가 이벤트인 경우 이벤트 토큰.
- `String callbackId`: 이벤트 객체에서 사용자가 설정하는 콜백 ID.

그리고 이벤트 및 세션 실패 개체에는 모두 다음이 들어갑니다.

- `bool WillRetry` 나중에 패키지를 다시 보내려는 시도가 있을 것임을 나타냅니다.

### <a id="disable-tracking">추적 사용 중지

`false` 파라미터를 사용할 수 있는 `setEnabled` 메서드를 호출하여 Adjust SDK에서 추적 사용을 중지하도록 설정할 수 있습니다. **이 설정은 세션 간에 기억됩니다.** 그러나 첫 번째 세션 후에만 활성화할 수 있습니다.

```cs
Adjust.setEnabled(false);
```

`isEnabled` 메서드를 사용하여 Adjust SDK가 현재 활성화되어 있는지 확인할 수 있습니다. `enabled` 파라미터가 `true`로 설정된 `setEnabled`를 호출하여 Adjust SDK를 언제든지 활성화할 수 있습니다.

### <a id="offline-mode">오프라인 모드

Adjust SDK를 오프라인 모드로 전환하여 Adjust 서버로 전송하는 작업을 일시 중단하고 추적 데이터를 보관하여 나중에 보낼 수 있습니다. 오프라인 모드일 때는 모든 정보가 파일에 저장되므로 오프라인 모드에서 너무 많은 이벤트를 촉발(trigger)하지 않도록 주의하십시오.

`setOfflineMode`를 `true`로 설정한 상태로 호출하면 오프라인 모드를 활성화할 수 있습니다.

```cs
Adjust.setOfflineMode(true);
```

반대로 `setOfflineMode`를 `false`로 설정한 상태로 호출하면 오프라인 모드를 비활성화할 수 있습니다.
Adjust SDK를 다시 온라인 모드로 전환하면 저장된 정보가 모두 올바른 시간 정보와 함께 Adjust 서버로 전송됩니다.

추적 사용 중지와 달리 **이 설정은 세션 간에 기억되지 않습니다**. 따라서 앱을 오프라인 모드에서 종료한 경우에도 SDK는 항상 온라인 모드로 시작됩니다.

### <a id="event-buffering">이벤트 버퍼링

앱이 이벤트 추적을 많이 사용하는 경우, 매 분마다 배치(batch) 하나씩만 보내도록 하기 위해 일부 HTTP 요청을 지연시키고자 할 경우가 있을 수 있습니다. `AdjustConfig` 인스턴스에서 이벤트 버퍼링을 적용할 수 있습니다.

```cs
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setEventBufferingEnabled(true);

Adjust.start(adjustConfig);
```

여기에 설정한 내용이 없으면 이벤트 버퍼링은 **기본값으로 해제됩니다**.

### <a id="gdpr-forget-me"></a>GDPR(일반 개인정보 보호법) 상의 잊힐 권리

유럽연합(EU) 일반 개인정보 보호법 제 17조에 의거하여, 사용자가 잊힐 권리를 행사하였을 경우  Adjust에 이를 통보할 수 있습니다. 다음 매서드를 호출하면 Adjust SDK는 사용자가 잊힐 권리를 사용하기로 했음을 Adjust 백엔드에 전달합니다:

```cs
Adjust.gdprForgetMe();
```

이 정보를 받는 즉시 Adjust는 사용자의 데이터를 삭제하며 Adjust SDK는 해당 사용자 추적을 중단합니다. 향후 이 기기로부터 어떤 요청도 Adjust에 전송되지 않습니다.

### <a id="sdk-signature"></a>SDK 서명

계정 매니저가 Adjust SDK 서명을 활성화해야 합니다. 이 기능을 사용해 보고자 할 경우 Adjust 지원 팀<a href="mailto:support@adjust.com">(support@adjust.com)</a>으로 연락해 주십시오.

SDK 서명이 계정에서 이미 사용 가능 상태로 Adjust 대시보드에서 App Secret에 억세스할 수 있는 상태라면, 아래 매서드를 사용하여 SDK 서명을 앱에 연동하십시오. 

비밀 파라미터 전체(`secretId`, `info1`, `info2`, `info3`, `info4`)를 `AdjustConfig` 인스턴스에 있는 `setAppSecret` 메서드에 전달하면 App Secret이 설정됩니다.

```cpp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setAppSecret(secretId, info1, info2, info3, info4);

Adjust.start(adjustConfig);
```

### <a id="background-tracking">백그라운드 추적

Adjust SDK에 설정된 기본값 행위는 **앱이 백그라운드에 있을 동안에는 HTTP 요청 전송을 잠시 중지**하는 것입니다. `AdjustConfig` 인스턴스에서 이를 바꿀 수 있습니다.

```csharp
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", "{YourEnvironment}");

adjustConfig.setSendInBackground(true);

Adjust.start(adjustConfig);
```

### <a id="device-ids">기기 ID

Google Analytics와 같은 서비스를 사용하려면 중복 보고가 발생하지 않도록 기기 ID와 클라이언트 ID를 조정해야 합니다. 

### <a id="di-idfa">iOS 광고 식별자

IDFA를 얻으려면 `Adjust` 인스턴스에서 `getIdfa` 함수를 호출합니다.

```cs
string idfa = Adjust.getIdfa()
```

### <a id="di-gps-adid">Google Play 서비스 광고 식별자

Google 광고 식별자 ID를 얻으려면 백그라운드 스레드에서만 읽을 수 있다는 제한이 있습니다. `Action<string>` 위임을 지닌 `Adjust` 인스턴스의 `getGoogleAdId` 메서드를 호출하면 상황에 관계 없이 작동합니다. 

```cs
Adjust.getGoogleAdId((string googleAdId) => {
    // ...
});
```

Google 광고 ID에 `googleAdId` 변수로 액세스할 수 있습니다.

### <a id="di-amz-adid"></a>Amazon 광고 식별자

Amazon 광고 ID를 얻으려면 `Adjust` 인스턴스에서 `getAmazonAdId` 메서드를 호출하면 됩니다.

```cs
string amazonAdId = Adjust.getAmazonAdId();
```

### <a id="di-adid"></a>Adjust 기기 식별자

Adjust 백엔드는 앱을 인스톨한 장치에서 고유한 **Adjust 기기 식별자** (**adid**)를 생성합니다. 이 식별자를 얻으려면 `Adjust` 인스턴스에서 다음 메서드를 호출하면 됩니다.

```cs
String adid = Adjust.getAdid();
```

**주의**: **adid** 관련 정보는 Adjust 백엔드가 앱 인스톨을 추적한 후에만 얻을 수 있습니다. 그 순간부터 Adjust SDK는 기기 **adid** 정보를 갖게 되며 이 메서드로 억세스할 수 있습니다. 따라서 SDK가 초기화되고 앱 인스톨 추적이 성공적으로 이루어지기 전에는 **adid** 억세스가 **불가능합니다**.

### <a id="user-attribution"></a>사용자 어트리뷰션

[어트리뷰션 콜백 섹션](#attribution-callback)에서 설명한 바와 같이, 이 콜백은 변동이 있을 때마다 새로운 속성 관련 정보를 전달할 목적으로 촉발됩니다. 사용자의 현재 속성 값 관련 정보를 언제든 억세스하고 싶다면, `Adjust` 인스턴스의 다음 메서드를 호출하면 됩니다.

```cs
AdjustAttribution attribution = Adjust.getAttribution();
```

**주의**: 사용자의 현재 어트리뷰션 관련 정보는 Adjust 백엔드가 앱 인스톨을 추적하여 최초 어트리뷰션 콜백이 촉발된 후에만 얻을 수 있습니다. 그 순간부터 Adjus SDK는 사용자 어트리뷰션 정보를 갖게 되며 이 메소드로 억세스할 수 있습니다. 따라서 SDK가 초기화되고 최초 어트리뷰션 콜백이 촉발되기 전에는 사용자 어트리뷰션 값 억세스가 **불가능합니다**.

### <a id="push-token">푸시 토큰

푸시 알림 토큰을 전송하려면 **앱에서 토큰을 받거나 업데이트가 있을 때마다** `Adjust` 인스턴스의 `setDeviceToken` 메서드를 호출하세요.

```cs
Adjust.setDeviceToken("YourPushNotificationToken");
```

푸시 토큰은 Audience Builder 및 클라이언트 콜백에 사용되며, 향후 선보일 설치 취소(uninstall) 추적 기능에 필요합니다.

### <a id="pre-installed-trackers">사전 설치 트래커

Adjust SDK를 사용하여 앱이 사전 설치된 장치를 지닌 유저를 인식하고 싶다면 다음 절차를 따르세요.

1. [대시보드][adjust.com]에 새 트래커를 생성합니다.
2. `AdjustConfig` 인스턴스의 기본값 트래커를 다음과 같이 설정합니다.

```cs
AdjustConfig adjustConfig = new AdjustConfig(appToken, environment);
adjustConfig.setDefaultTracker("{TrackerToken}");
Adjust.start(adjustConfig);
```

`{TrackerToken}`을 2에서 생성한 트래커 토큰으로 대체합니다. 대시보드에서는  (`http://app.adjust.com/`을 포함하는) 트래커 URL을 표시한다는 사실을 명심하세요. 소스코드에서는 전체 URL을 표시할 수 없으며 6자로 이루어진 토큰만을 표시해야 합니다.

3. 앱 빌드를 실행하세요. 앱 로그 출력 시 다음과 같은 라인을 볼 수 있을 것입니다.

```
Default tracker: 'abc123'
```

### <a id="deeplinking">딥링크

**딥링크는 iOS와 안드로이드 플랫폼에서만 지원합니다.**

URL에서 앱으로 딥링크를 거는 옵션이 있는 Adjust 트래커 URL을 사용하고 있다면, 딥링크 URL과 그 내용 관련 정보를 얻을 가능성이 있습니다. 해당 URL 클릭 시 사용자가 이미 앱을 설치한 상태(기본 딥링크)일 수도, 앱을 설치하지 않은 상태(지연된 딥링크)일 수도 있습니다. 기본 딥링크 상황에서 안드로이드는 딥링크 내용에 관한 정보 인출을 기본 지원합니다. 안드로이드는 지연된 딥링크를 기본 지원하지 않지만, Adjust SDK는 지연된 딥링크 정보를 인출하는 메커니즘을 제공합니다.

생성한 (iOS용) Xcode 프로젝트 및 (안드로이드용) Android Studio / Eclipse 프로젝트에서 딥링크 취급을 앱 내 **네이티브(native) 레벨**로 설정해야 합니다. 

#### <a id="deeplinking-standard">기본 딥링크

아쉽게도 이 경우 Unity C# 코드에서 딥링크 정보가 전달되지 않습니다. 앱에서 딥링크 취급을 구동시키면, 딥링크 관련 정보를 네이티브 레벨로 받게 됩니다. 안드로이드 및 iOS 앱에서 딥링크를 구동하는 법에 관한 자세한 정보는 아래 장에서 확인하세요.

#### <a id="deeplinking-deferred">지연된 딥링크

지연된 딥링크(Deferred deep linking)인 경우 URL 내용 정보를 받으려면, 이를 전달하는 파라미터인 `string`을 받는 `AdjustConfig` 객체에 콜백 메서드를 설정해야 합니다. `setDeferredDeeplinkDelegate` 메서드를 호출하여 설정하면 됩니다.

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

지연된 딥링크에서는 `AdjustConfig` 객체에서 설정이 하나 더 필요합니다. Adjust SDK가 
딥링크 정보를 받으면, SDK가 이 URL을 열 것인지를 선택할 수 있습니다. 이 옵션은 객체에서 `setLaunchDeferredDeeplink` 메서드를 호출하여 설정할 수 있습니다.

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

설정 내용이 없을 경우 **Adjust SDK는 URL을 언제나 기본값으로 런칭합니다**.

앱에서 딥링크를 활성화하려면, 지원하는 각 플랫폼에서 스킴을 설정해야 합니다.

#### <a id="deeplinking-android">안드로이드 앱에서 딥링크 관리

**기본 Android Studio / Eclipse 프로젝트에서 수행해야 합니다.**

안드로이드 앱이 딥링크를 네이티브 수준에서 취급하도록 설정하려면, 공식 안드로이드 SDK README에서 [설명서][android-deeplinking] 지침을 따르세요. 

#### <a id="deeplinking-ios">iOS 앱에서 딥링크 관리

**기본 Xcode 프로젝트에서 수행해야 합니다.** 

iOS 앱이 딥링크를 네이티브 수준에서 취급하도록 설정하려면, 공식 iOS SDK README에서 [설명서][ios-deeplinking] 지침을 따르세요. 

## <a id="troubleshooting">문제 해결

### <a id="ts-debug-ios">iOS 정보 디버깅

사후 빌드 스크립트를 사용해도 프로젝트를 즉시 실행할 수 없는 경우가 있습니다.

필요한 경우 dSYM 파일을 사용하지 않도록 설정하십시오. `Project Navigator`에서 `Unity-iPhone` 프로젝트를 선택합니다. `Build Settings` 탭을 클릭하고 `debug information`을 검색합니다. `Debug Information Format` 또는 `DEBUG_INFORMATION_FORMAT` 옵션이 표시됩니다. 해당 옵션을 `DWARF with dSYM File`에서 `DWARF`로 변경하면 됩니다.


[dashboard]:  http://adjust.com
[adjust.com]: http://adjust.com

[en-readme]:  ../../README.md
[zh-readme]:  ../chinese/README.md
[ja-readme]:  ../japanese/README.md
[ko-readme]:  ../korean/README.md

[sdk2sdk-mopub]:    ../korean/sdk-to-sdk/mopub.md

[ios]:                     https://github.com/adjust/ios_sdk
[android]:                 https://github.com/adjust/android_sdk
[releases]:                https://github.com/adjust/adjust_unity_sdk/releases
[google_ad_id]:            https://developer.android.com/google/play-services/id.html
[ios-deeplinking]:         https://github.com/adjust/sdks/blob/master/doc/ko-sdks/ios_sdk_readme.md#deeplinking
[attribution_data]:        https://github.com/adjust/sdks/blob/master/doc/attribution-data.md
[special-partners]:        https://docs.adjust.com/en/special-partners
[unity-purchase-sdk]:      https://github.com/adjust/unity_purchase_sdk
[android-deeplinking]:     https://github.com/adjust/sdks/blob/master/doc/ko-sdks/android_sdk_readme.md#deeplinking
[google_play_services]:    http://developer.android.com/google/play-services/setup.html
[android_sdk_download]:    https://developer.android.com/sdk/index.html#Other
[android-custom-receiver]: https://github.com/adjust/android_sdk/blob/master/doc/referrer.md

[menu_android]:             https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/menu_android.png
[adjust_editor]:            https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/adjust_editor.png
[import_package]:           https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/import_package.png
[android_sdk_location]:     https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download.png
[android_sdk_location_new]: https://raw.github.com/adjust/adjust_sdk/master/Resources/unity/v4/android_sdk_download_new.png

[install referrer library]: https://maven.google.com/com/android/installreferrer/installreferrer/1.0/installreferrer-1.0.aar

## <a id="license"></a>라이선스

mod_pbxproj.py 파일은 Apache License 2.0 버전(이하 "라이선스")에 따라 사용이 허가되며, 이 파일을 라이선스를 준수하지 않고 사용해서는 안 됩니다.
라이선스의 복사본은 http://www.apache.org/licenses/LICENSE-2.0에서 다운로드할 수 있습니다.

Adjust SDK는 MIT 라이선스에 따라 사용이 허가됩니다.

Copyright (c) 2012-2019 Adjust GmbH, http://www.adjust.com

이로써 본 소프트웨어와 관련 문서 파일(이하 "소프트웨어")의 복사본을 받는 사람에게는 아래 조건에 따라 소프트웨어를 제한 없이 다룰 수 있는 권한이 무료로 부여됩니다. 이 권한에는 소프트웨어를 사용, 복사, 수정, 병합, 출판, 배포 및/또는 판매하거나 2차 사용권을 부여할 권리와 소프트웨어를 제공 받은 사람이 소프트웨어를 사용, 복사, 수정, 병합, 출판, 배포 및/또는 판매하거나 2차 사용권을 부여하는 것을 허가할 수 있는 권리가 제한 없이 포함됩니다.

위 저작권 고지문과 본 권한 고지문은 소프트웨어의 모든 복사본이나 주요 부분에 포함되어야 합니다.

소프트웨어는 상품성, 특정 용도에 대한 적합성 및 비침해에 대한 보증 등을 비롯한 어떤 종류의 명시적이거나 암묵적인 보증 없이 "있는 그대로" 제공됩니다. 어떤 경우에도 저작자나 저작권 보유자는 소프트웨어와 소프트웨어의 사용 또는 기타 취급에서 비롯되거나 그에 기인하거나 그와 관련하여 발생하는 계약 이행 또는 불법 행위 등에 관한 배상 청구, 피해 또는 기타 채무에 대해 책임지지 않습니다.
--END--
