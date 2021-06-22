# Track AdMob ad revenue with Adjust SDK

[Adjust Unity SDK README][unity-readme]

Minimum SDK version required for this feature:

- **Adjust SDK v4.29.0**

> Note: In order to enable this feature, please reach out to your Google point of contact. Your point of contact will be able to activate the feature for you to access it.

If you want to track your ad revenue with the Admob SDK, you can use our SDK-to-SDK integration to pass this information to the Adjust backend. To do this, you will need to construct an Adjust ad revenue object containing the information you wish to record, then pass the object to the `trackAdRevenue` method.

> Note: If you have any questions about ad revenue tracking with Admob, please contact your dedicated account manager or send an email to [support@adjust.com](mailto:support@adjust.com).

### Example

```cs
private void RequestRewardedAd()
{
    // create a rewarded ad
    this.rewardedAd = new RewardedAd(<your-ad-unit-id>);
    // register for ad paid events
    this.rewardedAd.OnPaidEvent += this.HandleAdPaidEvent;
    // load a rewarded ad
    this.rewardedAd.LoadAd(new AdRequest.Builder().Build());
}
// register for ad paid events
this.rewardedAd.OnPaidEvent += this.HandleAdPaidEvent;
public void HandleAdPaidEvent(object sender, AdValueEventArgs args)
{
    // ...
    AdValue adValue = args.AdValue;
    // send ad revenue info to Adjust
    AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
    adRevenue.setRevenue(adValue.Value / 1000000, adValue.CurrencyCode);
    Adjust.trackAdRevenue(adRevenue);
}
```

[unity-readme]:    ../../../README.md
