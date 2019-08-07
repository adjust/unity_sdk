## 通过 Adjust SDK 跟踪 MoPub 广告收入

[Adjust Unity SDK 自述文件][unity-readme]

[MoPub Unity 文档][mopub-docs]

此功能最低 SDK 版本要求：

- **Adjust SDK v4.18.0**
- **MoPub SDK v5.7.0**

在实施 MoPub SDK `OnImpressionTrackedEvent` 回传方法时，请确保按照如下方式调用 Adjust SDK 的 `trackAdRevenue` 方法：

```csharp
private void OnImpressionTrackedEvent(string adUnitId, MoPub.ImpressionData impressionData) {
    // Pass impression data JSON to Adjust SDK.
    Adjust.trackAdRevenue(AdjustConfig.AdjustAdRevenueSourceMopub, impressionData.JsonRepresentation);
}
```

如果您对 MoPub 广告收入跟踪有任何疑问，请联系您的专属客户经理，或发送邮件至 support@adjust.com。

[mopub-docs]:        https://developers.mopub.com/publishers/unity/impression-data/
[unity-readme]:    ../../chinese/README.md
