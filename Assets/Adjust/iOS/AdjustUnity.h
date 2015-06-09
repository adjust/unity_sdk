#import "Adjust.h"

@interface AdjustUnity : NSObject<AdjustDelegate>

- (void)adjustAttributionChanged:(ADJAttribution *)attribution;

@end
