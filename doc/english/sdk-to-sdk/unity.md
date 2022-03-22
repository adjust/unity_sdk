# Track Unity ad revenue with Adjust SDK

[Adjust Unity SDK README][unity-readme]

Minimum SDK version required for this feature:

- **Adjust SDK v4.29.6**

If you want to track your ad revenue with the Unity SDK, you can use our SDK-to-SDK integration to pass this information to the Adjust backend. To do this, you will need to construct an Adjust ad revenue object containing the information you wish to record, then pass the object to the `trackAdRevenue` method.

> Note: If you have any questions about ad revenue tracking with Unity, please contact your dedicated account manager or send an email to [support@adjust.com](mailto:support@adjust.com).

For more information, see the Unity Mediation [API documentation](https://docs.unity.com/mediation/APIReferenceUnity.html) and [impression event documentation](https://docs.unity.com/mediation/SDKIntegrationUnityImpressionEvents.html).

### Example

```cs
static void OnImpression(object sender, ImpressionEventArgs e)
{
    var impressionData = e.ImpressionData != null ? JsonUtility.ToJson(e.ImpressionData, true) : "null";
    Debug.Log($"Impression event from ad unit id {e.AdUnitId} : {impressionData}");

    // send impression data to Adjust 
    if (e.ImpressionData != null)
    {
        AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceUnity);
        adjustAdRevenue.setRevenue(e.ImpressionData.PublisherRevenuePerImpression, e.ImpressionData.Currency);
        // optional fields
        adjustAdRevenue.setAdRevenueNetwork(e.ImpressionData.AdSourceName);
        adjustAdRevenue.setAdRevenueUnit(e.ImpressionData.AdUnitId);
        adjustAdRevenue.setAdRevenuePlacement(e.ImpressionData.AdSourceInstance);
        // track Adjust ad revenue
        Adjust.trackAdRevenue(adjustAdRevenue);
    }    
}
```

[unity-readme]:    ../../../README.md
