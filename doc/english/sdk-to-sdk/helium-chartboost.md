# Track Helium Chartboost ad revenue with Adjust SDK

[Adjust Unity SDK README][unity-readme]

Minimum SDK version required for this feature:

- **Adjust SDK v4.29.7**

If you want to track your ad revenue with the Helium SDK, you can use our SDK-to-SDK integration to pass this information to the Adjust backend. To do this, you will need to construct an Adjust ad revenue object containing the information you wish to record, then pass the object to the `trackAdRevenue` method.

> Note: If you have any questions about ad revenue tracking with Helium Chartboost, please contact your dedicated account manager or send an email to [support@adjust.com](mailto:support@adjust.com).

### Example

```cs
void DidReceiveImpressionLevelRevenueData(string placement, Hashtable impressionData)
{
    var json = HeliumJSON.Serialize(impressionData);

    ParsedJsonObject parsedJsonObject = foobar.parse(json); //app developer defined function to parse Helium impressionData JSON string
    
    AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceHeliumChartboost);
    adjustAdRevenue.setRevenue(parsedJsonObject.ad_revenue, parsedJsonObject.currency_type);

    // optional fields
    adjustAdRevenue.setAdRevenueNetwork(parsedJsonObject.network_name);     // Helium demand network name
    adjustAdRevenue.setAdRevenueUnit(parsedJsonObject.placement_name);      // Helium placement name
    adjustAdRevenue.setAdRevenuePlacement(parsedJsonObject.line_item_name); // Helium line item name
    // track Adjust ad revenue
    Adjust.trackAdRevenue(adjustAdRevenue);
}
```

[unity-readme]:    ../../../README.md
