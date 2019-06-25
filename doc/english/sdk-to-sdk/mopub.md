## Track MoPub ad revenue with Adjust SDK

[Adjust Unity SDK README][unity-readme]

[MoPub Unity documentation][mopub-docs]

Minimal SDK version required for this feature:

- **Adjust SDK v4.18.0**
- **MoPub SDK v5.7.0**

Inside of your MoPub SDK `OnImpressionTrackedEvent` callback method implementation, make sure to invoke `trackAdRevenue` method of Adjust SDK like this:

```csharp
private void OnImpressionTrackedEvent(string adUnitId, MoPub.ImpressionData impressionData) {
    // Pass impression data JSON to Adjust SDK.
    Adjust.trackAdRevenue(AdjustConfig.AdjustAdRevenueSourceMopub, impressionData.JsonRepresentation);
}
```

In case you have any questions about ad revenue tracking with MoPub, please contact your dedicated account manager or send an email to support@adjust.com.

[mopub-docs]:        https://developers.mopub.com/publishers/unity/impression-data/
[unity-readme]:    ../../../README.md
