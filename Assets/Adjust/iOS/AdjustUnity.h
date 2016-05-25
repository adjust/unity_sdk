#import "Adjust.h"

@interface AdjustUnity : NSObject<AdjustDelegate>

- (void)adjustAttributionChanged:(ADJAttribution *)attribution;
- (void)adjustEventTrackingSucceeded:(ADJEventSuccess *)eventSuccessResponseData;
- (void)adjustEventTrackingFailed:(ADJEventFailure *)eventFailureResponseData;
- (void)adjustSessionTrackingSucceeded:(ADJSessionSuccess *)sessionSuccessResponseData;
- (void)adjustSessionTrackingFailed:(ADJSessionFailure *)sessionFailureResponseData;
- (BOOL)adjustDeeplinkResponse:(NSURL *)deeplink;

@end
