#import "Adjust.h"

@interface AdjustUnity : NSObject<AdjustDelegate>

- (void)adjustFinishedTrackingWithResponse:(AIResponseData *)responseData;
- (void)setResponseDelegate;

@end

