# Track AppLovin MAX ad revenue with Adjust SDK

[Adjust Unity SDK README][unity-readme]

Minimum SDK version required for this feature:

- **Adjust SDK v4.29.0**

If you want to track your ad revenue with the AppLovin MAX SDK, you can use our SDK-to-SDK integration to pass this information to the Adjust backend. To do this, you will need to construct an Adjust ad revenue object containing the information you wish to record, then pass the object to the `trackAdRevenue` method.

> Note: If you have any questions about ad revenue tracking with AppLovin MAX, please contact your dedicated account manager or send an email to [support@adjust.com](mailto:support@adjust.com).

### Example

> Note: In order to successfully use SDK to SDK ad revenue tracking with MAX SDK, please make sure that your `AdjustConfig` instance you use for Adjust SDk initialization is configured so that [background tracking](../../../README.md#ad-background-tracking) is enabled.

```cs
// Adjust SDK initialization
AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
adjustConfig.setSendInBackground(true);
Adjust.start(adjustConfig);

// ...

// pass MAX SDK ad revenue data to Adjust SDK
public static void OnInterstitialAdRevenuePaidEvent(string adUnitId)
{
    var info = MaxSdk.GetAdInfo(adUnitId);

    var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
    adRevenue.setRevenue(info.Revenue, "USD");
    adRevenue.setAdRevenueNetwork(info.NetworkName);
    adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
    adRevenue.setAdRevenuePlacement(info.Placement);

    Adjust.trackAdRevenue(adRevenue);
}
```

[unity-readme]:    ../../../README.md
