## MoPubの広告収益をAdjust SDKで計測

[Adjust Unity SDK README][unity-readme]

[MoPub Unity documentation][mopub-docs]

本機能には以下のSDKバージョンとそれ以降のバージョンが必須となります：

- **Adjust SDK v4.18.0**
- **MoPub SDK v5.7.0**

MoPub SDKの `OnImpressionTrackedEvent` コールバックメソッドの実装内で、以下のようにAdjust SDKの`trackAdRevenue` メソッドを呼び出す必要があります。

```csharp
private void OnImpressionTrackedEvent(string adUnitId, MoPub.ImpressionData impressionData) {
    // Pass impression data JSON to Adjust SDK.
    Adjust.trackAdRevenue(AdjustConfig.AdjustAdRevenueSourceMopub, impressionData.JsonRepresentation);
}
```

MoPub連携による広告収益計測についてご質問がございましたら、担当のアカウントマネージャーもしくはsupport@adjust.comまでお問い合わせください。

[mopub-docs]:        https://developers.mopub.com/publishers/unity/impression-data/
[unity-readme]:    ../../japanese/README.md
