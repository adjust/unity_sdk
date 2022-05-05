//
//  AdjustUnityAppDelegate.h
//  Adjust SDK
//
//  Copyright © 2012-2021 Adjust GmbH. All rights reserved.
//

/**
 * @brief The interface to Adjust App Delegate. Used to do callback methods swizzling for deep linking.
 */
@interface AdjustUnityAppDelegate : NSObject

/**
 * @brief Swizzle AppDelegate deep linking callbacks.
 */
+ (void)swizzleAppDelegateCallbacks;

@end
