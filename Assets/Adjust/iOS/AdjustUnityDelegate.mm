//
//  AdjustUnityDelegate.mm
//  Adjust SDK
//
//  Created by Uglješa Erceg (@uerceg) on 5th December 2016.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import <objc/runtime.h>
#import "AdjustUnityDelegate.h"

static dispatch_once_t onceToken;
static AdjustUnityDelegate *defaultInstance = nil;

@implementation AdjustUnityDelegate

#pragma mark - Object lifecycle methods

- (id)init {
    self = [super init];
    if (nil == self) {
        return nil;
    }
    return self;
}

#pragma mark - Public methods

+ (id)getInstanceWithSwizzleOfAttributionCallback:(BOOL)swizzleAttributionCallback
                             eventSuccessCallback:(BOOL)swizzleEventSuccessCallback
                             eventFailureCallback:(BOOL)swizzleEventFailureCallback
                           sessionSuccessCallback:(BOOL)swizzleSessionSuccessCallback
                           sessionFailureCallback:(BOOL)swizzleSessionFailureCallback
                         deferredDeeplinkCallback:(BOOL)swizzleDeferredDeeplinkCallback
                   conversionValueUpdatedCallback:(BOOL)swizzleConversionValueUpdatedCallback
                     shouldLaunchDeferredDeeplink:(BOOL)shouldLaunchDeferredDeeplink
                         withAdjustUnitySceneName:(NSString *)adjustUnitySceneName; {
    dispatch_once(&onceToken, ^{
        defaultInstance = [[AdjustUnityDelegate alloc] init];

        // Do the swizzling where and if needed.
        if (swizzleAttributionCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustAttributionChanged:)
                                        withSelector:@selector(adjustAttributionChangedWannabe:)];
        }
        if (swizzleEventSuccessCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustEventTrackingSucceeded:)
                                        withSelector:@selector(adjustEventTrackingSucceededWannabe:)];
        }
        if (swizzleEventFailureCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustEventTrackingFailed:)
                                        withSelector:@selector(adjustEventTrackingFailedWannabe:)];
        }
        if (swizzleSessionSuccessCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustSessionTrackingSucceeded:)
                                        withSelector:@selector(adjustSessionTrackingSucceededWannabe:)];
        }
        if (swizzleSessionFailureCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustSessionTrackingFailed:)
                                        withSelector:@selector(adjustSessionTrackingFailedWannabe:)];
        }
        if (swizzleDeferredDeeplinkCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustDeeplinkResponse:)
                                        withSelector:@selector(adjustDeeplinkResponseWannabe:)];
        }
        if (swizzleConversionValueUpdatedCallback) {
            [defaultInstance swizzleOriginalSelector:@selector(adjustConversionValueUpdated:)
                                        withSelector:@selector(adjustConversionValueUpdatedWannabe:)];
        }

        [defaultInstance setShouldLaunchDeferredDeeplink:shouldLaunchDeferredDeeplink];
        [defaultInstance setAdjustUnitySceneName:adjustUnitySceneName];
    });
    
    return defaultInstance;
}

+ (void)teardown {
    defaultInstance = nil;
    onceToken = 0;
}

#pragma mark - Private & helper methods

- (void)adjustAttributionChangedWannabe:(ADJAttribution *)attribution {
    if (attribution == nil) {
        return;
    }
    
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    [self addValueOrEmpty:attribution.trackerToken forKey:@"trackerToken" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.trackerName forKey:@"trackerName" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.network forKey:@"network" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.campaign forKey:@"campaign" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.creative forKey:@"creative" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.adgroup forKey:@"adgroup" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.clickLabel forKey:@"clickLabel" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.adid forKey:@"adid" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.costType forKey:@"costType" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.costAmount forKey:@"costAmount" toDictionary:dictionary];
    [self addValueOrEmpty:attribution.costCurrency forKey:@"costCurrency" toDictionary:dictionary];

    NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                           length:[dataAttribution length]
                                                         encoding:NSUTF8StringEncoding];
    const char* charArrayAttribution = [stringAttribution UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeAttribution", charArrayAttribution);
}

- (void)adjustEventTrackingSucceededWannabe:(ADJEventSuccess *)eventSuccessResponseData {
    if (nil == eventSuccessResponseData) {
        return;
    }

    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    [self addValueOrEmpty:eventSuccessResponseData.message forKey:@"message" toDictionary:dictionary];
    [self addValueOrEmpty:eventSuccessResponseData.timeStamp forKey:@"timestamp" toDictionary:dictionary];
    [self addValueOrEmpty:eventSuccessResponseData.adid forKey:@"adid" toDictionary:dictionary];
    [self addValueOrEmpty:eventSuccessResponseData.eventToken forKey:@"eventToken" toDictionary:dictionary];
    [self addValueOrEmpty:eventSuccessResponseData.callbackId forKey:@"callbackId" toDictionary:dictionary];
    if (eventSuccessResponseData.jsonResponse != nil) {
        [dictionary setObject:eventSuccessResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataEventSuccess = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *stringEventSuccess = [[NSString alloc] initWithBytes:[dataEventSuccess bytes]
                                                            length:[dataEventSuccess length]
                                                          encoding:NSUTF8StringEncoding];
    const char* charArrayEventSuccess = [stringEventSuccess UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeEventSuccess", charArrayEventSuccess);
}

- (void)adjustEventTrackingFailedWannabe:(ADJEventFailure *)eventFailureResponseData {
    if (nil == eventFailureResponseData) {
        return;
    }

    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    [self addValueOrEmpty:eventFailureResponseData.message forKey:@"message" toDictionary:dictionary];
    [self addValueOrEmpty:eventFailureResponseData.timeStamp forKey:@"timestamp" toDictionary:dictionary];
    [self addValueOrEmpty:eventFailureResponseData.adid forKey:@"adid" toDictionary:dictionary];
    [self addValueOrEmpty:eventFailureResponseData.eventToken forKey:@"eventToken" toDictionary:dictionary];
    [self addValueOrEmpty:eventFailureResponseData.callbackId forKey:@"callbackId" toDictionary:dictionary];
    [dictionary setObject:(eventFailureResponseData.willRetry ? @"true" : @"false") forKey:@"willRetry"];
    if (eventFailureResponseData.jsonResponse != nil) {
        [dictionary setObject:eventFailureResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataEventFailure = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *stringEventFailure = [[NSString alloc] initWithBytes:[dataEventFailure bytes]
                                                            length:[dataEventFailure length]
                                                          encoding:NSUTF8StringEncoding];
    const char* charArrayEventFailure = [stringEventFailure UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeEventFailure", charArrayEventFailure);
}

- (void)adjustSessionTrackingSucceededWannabe:(ADJSessionSuccess *)sessionSuccessResponseData {
    if (nil == sessionSuccessResponseData) {
        return;
    }

    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    [self addValueOrEmpty:sessionSuccessResponseData.message forKey:@"message" toDictionary:dictionary];
    [self addValueOrEmpty:sessionSuccessResponseData.timeStamp forKey:@"timestamp" toDictionary:dictionary];
    [self addValueOrEmpty:sessionSuccessResponseData.adid forKey:@"adid" toDictionary:dictionary];
    if (sessionSuccessResponseData.jsonResponse != nil) {
        [dictionary setObject:sessionSuccessResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataSessionSuccess = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *stringSessionSuccess = [[NSString alloc] initWithBytes:[dataSessionSuccess bytes]
                                                              length:[dataSessionSuccess length]
                                                            encoding:NSUTF8StringEncoding];
    const char* charArraySessionSuccess = [stringSessionSuccess UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeSessionSuccess", charArraySessionSuccess);
}

- (void)adjustSessionTrackingFailedWannabe:(ADJSessionFailure *)sessionFailureResponseData {
    if (nil == sessionFailureResponseData) {
        return;
    }

    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    [self addValueOrEmpty:sessionFailureResponseData.message forKey:@"message" toDictionary:dictionary];
    [self addValueOrEmpty:sessionFailureResponseData.timeStamp forKey:@"timestamp" toDictionary:dictionary];
    [self addValueOrEmpty:sessionFailureResponseData.adid forKey:@"adid" toDictionary:dictionary];
    [dictionary setObject:(sessionFailureResponseData.willRetry ? @"true" : @"false") forKey:@"willRetry"];
    if (sessionFailureResponseData.jsonResponse != nil) {
        [dictionary setObject:sessionFailureResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataSessionFailure = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *stringSessionFailure = [[NSString alloc] initWithBytes:[dataSessionFailure bytes]
                                                              length:[dataSessionFailure length]
                                                            encoding:NSUTF8StringEncoding];
    const char* charArraySessionFailure = [stringSessionFailure UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeSessionFailure", charArraySessionFailure);
}

- (BOOL)adjustDeeplinkResponseWannabe:(NSURL *)deeplink {
    NSString *stringDeeplink = [deeplink absoluteString];
    const char* charDeeplink = [stringDeeplink UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeDeferredDeeplink", charDeeplink);
    return _shouldLaunchDeferredDeeplink;
}

- (void)adjustConversionValueUpdatedWannabe:(NSNumber *)conversionValue {
    NSString *stringConversionValue = [conversionValue stringValue];
    const char* charConversionValue = [stringConversionValue UTF8String];
    UnitySendMessage([self.adjustUnitySceneName UTF8String], "GetNativeConversionValueUpdated", charConversionValue);
}

- (void)swizzleOriginalSelector:(SEL)originalSelector
                   withSelector:(SEL)swizzledSelector {
    Class className = [self class];
    Method originalMethod = class_getInstanceMethod(className, originalSelector);
    Method swizzledMethod = class_getInstanceMethod(className, swizzledSelector);

    BOOL didAddMethod = class_addMethod(className,
                                        originalSelector,
                                        method_getImplementation(swizzledMethod),
                                        method_getTypeEncoding(swizzledMethod));
    if (didAddMethod) {
        class_replaceMethod(className,
                            swizzledSelector,
                            method_getImplementation(originalMethod),
                            method_getTypeEncoding(originalMethod));
    } else {
        method_exchangeImplementations(originalMethod, swizzledMethod);
    }
}

- (void)addValueOrEmpty:(NSObject *)value
                 forKey:(NSString *)key
           toDictionary:(NSMutableDictionary *)dictionary {
    if (nil != value) {
        if ([value isKindOfClass:[NSString class]]) {
            [dictionary setObject:[NSString stringWithFormat:@"%@", value] forKey:key];
        } else if ([value isKindOfClass:[NSNumber class]]) {
            [dictionary setObject:[NSString stringWithFormat:@"%@", [((NSNumber *)value) stringValue]] forKey:key];
        } else {
            [dictionary setObject:@"" forKey:key];
        }
    } else {
        [dictionary setObject:@"" forKey:key];
    }
}

@end
