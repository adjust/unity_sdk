#import "Adjust.h"

@interface AdjustUnity : NSObject<AdjustDelegate>

- (void)adjustFinishedTrackingWithResponse:(AIResponseData *)responseData;

@end

