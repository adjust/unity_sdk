//
//  AdjustUnityDelegate.h
//  Adjust SDK
//
//  Created by Uglješa Erceg (@uerceg) on 5th December 2016.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import "Adjust.h"

/**
 * @brief The main interface to Adjust Unity delegate. Used to do callback methods swizzling where needed.
 */
@interface AdjustUnityDelegate : NSObject<AdjustDelegate>

/**
 * @brief Boolean indicating whether deferred deep link should be launched by SDK or not.
 */
@property (nonatomic) BOOL shouldLaunchDeferredDeeplink;

/**
 * @brief Name of the Unity scene that loads Adjust SDK.
 */
@property (nonatomic, copy) NSString *adjustUnitySceneName;

/**
 * @brief Get instance of the AdjustUnityDelegate with properly swizzled callback methods.
 *
 * @param swizzleAttributionCallback                  Indicator whether attribution callback should be swizzled or not.
 * @param swizzleEventSuccessCallback                 Indicator whether event success callback should be swizzled or not.
 * @param swizzleEventFailureCallback                 Indicator whether event failure callback should be swizzled or not.
 * @param swizzleSessionSuccessCallback               Indicator whether session success callback should be swizzled or not.
 * @param swizzleSessionFailureCallback               Indicator whether session failure callback should be swizzled or not.
 * @param swizzleDeferredDeeplinkCallback             Indicator whether deferred deep link callback should be swizzled or not.
 * @param swizzleConversionValueUpdatedCallback       Indicator whether SKAD conversion value update callback should be swizzled or not.
 * @param swizzleSkad4ConversionValueUpdatedCallback  Indicator whether SKAD4 conversion value update callback should be swizzled or not.
 * @param shouldLaunchDeferredDeeplink                Indicator whether SDK should launch deferred deep link by default or not.
 * @param adjustUnitySceneName                        Name of the Unity scene that loads Adjust SDK.
 *
 * @return AdjustUnityDelegate object instance with properly swizzled callback methods.
 */
+ (id)getInstanceWithSwizzleOfAttributionCallback:(BOOL)swizzleAttributionCallback
                             eventSuccessCallback:(BOOL)swizzleEventSuccessCallback
                             eventFailureCallback:(BOOL)swizzleEventFailureCallback
                           sessionSuccessCallback:(BOOL)swizzleSessionSuccessCallback
                           sessionFailureCallback:(BOOL)swizzleSessionFailureCallback
                         deferredDeeplinkCallback:(BOOL)swizzleDeferredDeeplinkCallback
                   conversionValueUpdatedCallback:(BOOL)swizzleConversionValueUpdatedCallback
              skad4ConversionValueUpdatedCallback:(BOOL)swizzleSkad4ConversionValueUpdatedCallback
                     shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink
                         withAdjustUnitySceneName:(NSString *)adjustUnitySceneName;

/**
 * @brief Teardown method used to reset static AdjustUnityDelegate instance.
 *        Used for testing purposes only.
 */
+ (void)teardown;

@end
