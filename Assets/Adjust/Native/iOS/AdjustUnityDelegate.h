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
@property (nonatomic) AdjustDelegateRemoteTriggerCallback remoteTriggerCallback;
@property (nonatomic) AdjustDelegateSkanUpdatedCallback skanUpdatedCallback;

/**
 * @brief Get instance of the AdjustUnityDelegate with properly swizzled callback methods.
 *
 * @param attributionCallback           Attribution callback function pointer.
 * @param eventSuccessCallback          Event success callback function pointer.
 * @param eventFailureCallback          Event failure callback function pointer.
 * @param sessionSuccessCallback        Session success callback function pointer.
 * @param sessionFailureCallback        Session failure callback function pointer.
 * @param deferredDeeplinkCallback      Deferred deep link callback function pointer.
 * @param remoteTriggerCallback         Remote trigger callback function pointer.
 * @param skanUpdatedCallback           SKAdNetwork conversion value update callback function pointer.
 * @param shouldLaunchDeferredDeeplink  Indicator whether SDK should launch deferred deep link by default or not.
 *
 * @return AdjustUnityDelegate object instance with properly swizzled callback methods.
 */
+ (id)getInstanceWithAttributionCallback:(AdjustDelegateAttributionCallback)attributionCallback
                    eventSuccessCallback:(AdjustDelegateEventSuccessCallback)eventSuccessCallback
                    eventFailureCallback:(AdjustDelegateEventFailureCallback)eventFailureCallback
                  sessionSuccessCallback:(AdjustDelegateSessionSuccessCallback)sessionSuccessCallback
                  sessionFailureCallback:(AdjustDelegateSessionFailureCallback)sessionFailureCallback
                deferredDeeplinkCallback:(AdjustDelegateDeferredDeeplinkCallback)deferredDeeplinkCallback
                   remoteTriggerCallback:(AdjustDelegateRemoteTriggerCallback)remoteTriggerCallback
                     skanUpdatedCallback:(AdjustDelegateSkanUpdatedCallback)skanUpdatedCallback
            shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink;

/**
 * @brief Teardown method used to reset static AdjustUnityDelegate instance.
 *        Used for testing purposes only.
 */
+ (void)teardown;

@end
