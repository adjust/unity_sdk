//
//  AdjustUnityDelegate.h
//  Adjust SDK
//
//  Created by Uglješa Erceg (@uerceg) on 5th December 2016.
//  Copyright © 2012-Present Adjust GmbH. All rights reserved.
//

#import <AdjustSdk/AdjustSdk.h>
#import "AdjustUnity.h"

/**
 * @brief The main interface to Adjust Unity delegate. Used to do callback methods swizzling where needed.
 */
@interface AdjustUnityDelegate : NSObject<AdjustDelegate>

/**
 * @brief Boolean indicating whether deferred deep link should be launched by SDK or not.
 */
@property (nonatomic) BOOL shouldLaunchDeferredDeeplink;

@property (nonatomic) AdjustDelegateAttributionCallback attributionCallback;
@property (nonatomic) AdjustDelegateEventSuccessCallback eventSuccessCallback;
@property (nonatomic) AdjustDelegateEventFailureCallback eventFailureCallback;
@property (nonatomic) AdjustDelegateSessionSuccessCallback sessionSuccessCallback;
@property (nonatomic) AdjustDelegateSessionFailureCallback sessionFailureCallback;
@property (nonatomic) AdjustDelegateDeferredDeeplinkCallback deferredDeeplinkCallback;
@property (nonatomic) AdjustDelegateSkanUpdatedCallback skanUpdatedCallback;

/**
 * @brief Get instance of the AdjustUnityDelegate with properly swizzled callback methods.
 *
 * @param swizzleAttributionCallback        Indicator whether attribution callback should be swizzled or not.
 * @param swizzleEventSuccessCallback       Indicator whether event success callback should be swizzled or not.
 * @param swizzleEventFailureCallback       Indicator whether event failure callback should be swizzled or not.
 * @param swizzleSessionSuccessCallback     Indicator whether session success callback should be swizzled or not.
 * @param swizzleSessionFailureCallback     Indicator whether session failure callback should be swizzled or not.
 * @param swizzleDeferredDeeplinkCallback   Indicator whether deferred deep link callback should be swizzled or not.
 * @param swizzleSkanUpdatedCallback        Indicator whether SKAD conversion value update callback should be swizzled or not.
 * @param shouldLaunchDeferredDeeplink      Indicator whether SDK should launch deferred deep link by default or not.
 * @param adjustUnityGameObjectName         Name of the Unity game object that loads Adjust script.
 *
 * @return AdjustUnityDelegate object instance with properly swizzled callback methods.
 */
+ (id)getInstanceWithAttributionCallback:(AdjustDelegateAttributionCallback)attributionCallback
                    eventSuccessCallback:(AdjustDelegateEventSuccessCallback)eventSuccessCallback
                    eventFailureCallback:(AdjustDelegateEventFailureCallback)eventFailureCallback
                  sessionSuccessCallback:(AdjustDelegateSessionSuccessCallback)sessionSuccessCallback
                  sessionFailureCallback:(AdjustDelegateSessionFailureCallback)sessionFailureCallback
                deferredDeeplinkCallback:(AdjustDelegateDeferredDeeplinkCallback)deferredDeeplinkCallback
                     skanUpdatedCallback:(AdjustDelegateSkanUpdatedCallback)skanUpdatedCallback
            shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink;

/**
 * @brief Teardown method used to reset static AdjustUnityDelegate instance.
 *        Used for testing purposes only.
 */
+ (void)teardown;

@end
