//
//  AdjustUnityDelegate.h
//  Adjust
//
//  Created by uerceg on 12/05/16.
//  Copyright (c) 2012-2016 adjust GmbH. All rights reserved.
//

#import "Adjust.h"

@interface AdjustUnityDelegate : NSObject<AdjustDelegate>

@property (nonatomic) BOOL shouldLaunchDeferredDeeplink;

@property (nonatomic, copy) NSString *adjustUnitySceneName;

+ (id)getInstanceWithSwizzleOfAttributionCallback:(BOOL)swizzleAttributionCallback
						   eventSucceededCallback:(BOOL)swizzleEventSucceededCallback
							  eventFailedCallback:(BOOL)swizzleEventFailedCallback
						 sessionSucceededCallback:(BOOL)swizzleSessionSucceededCallback
						    sessionFailedCallback:(BOOL)swizzleSessionFailedCallback
					     deferredDeeplinkCallback:(BOOL)swizzleDeferredDeeplinkCallback
					 shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink
					     withAdjustUnitySceneName:(NSString *)adjustUnitySceneName;

@end
