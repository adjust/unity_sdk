//
//  AdjustUnity.mm
//  Adjust SDK
//
//  Created by Pedro Silva (@nonelse) on 27th March 2014.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import <AdjustSdk/Adjust.h>
#import "AdjustUnity.h"
#import "AdjustUnityDelegate.h"
#import "AdjustUnityAppDelegate.h"

@implementation AdjustUnity

#pragma mark - Typedefs

typedef void (*DelegateCallbackIdfvGetter)(const char* idfv);

#pragma mark - Object lifecycle methods

+ (void)load {
    // swizzle AppDelegate on the load
    // it should be done as early as possible
    [AdjustUnityAppDelegate swizzleAppDelegateCallbacks];
}

@end

#pragma mark - Helper C methods

// method for converting JSON stirng parameters into NSArray object
NSArray* convertArrayParameters(const char* cStringJsonArrayParameters) {
    if (cStringJsonArrayParameters == NULL) {
        return nil;
    }

    NSError *error = nil;
    NSArray *arrayParameters = nil;
    NSString *stringJsonArrayParameters = [NSString stringWithUTF8String:cStringJsonArrayParameters];

    if (stringJsonArrayParameters != nil) {
        NSData *dataJson = [stringJsonArrayParameters dataUsingEncoding:NSUTF8StringEncoding];
        arrayParameters = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:&error];
    }
    if (error != nil) {
        NSString *errorMessage = @"Failed to parse json parameters!";
        NSLog(@"%@", errorMessage);
    }

    return arrayParameters;
}

BOOL isStringValid(const char* cString) {
    if (cString == NULL) {
        return false;
    }

    NSString *objcString = [NSString stringWithUTF8String:cString];
    if (objcString == nil) {
        return false;
    }
    if ([objcString isEqualToString:@"ADJ_INVALID"]) {
        return false;
    }

    return true;
}

void addValueOrEmpty(NSMutableDictionary *dictionary, NSString *key, NSObject *value) {
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

#pragma mark - Publicly available C methods

extern "C"
{
    void _AdjustInitSdk(const char* gameObjectName,
                        const char* appToken,
                        const char* environment,
                        const char* sdkPrefix,
                        const char* defaultTracker,
                        const char* externalDeviceId,
                        const char* jsonUrlStrategyDomains,
                        int allowSuppressLogLevel,
                        int logLevel,
                        int attConsentWaitingInterval,
                        int eventDeduplicationIdsMaxSize,
                        int shouldUseSubdomains,
                        int isDataResidency,
                        int isSendingInBackgroundEnabled,
                        int isAdServicesEnabled,
                        int isIdfaReadingEnabled,
                        int isSkanAttributionEnabled,
                        int isLinkMeEnabled,
                        int isCostDataInAttributionEnabled,
                        int isDeviceIdsReadingOnceEnabled,
                        int isDeferredDeeplinkOpeningEnabled,
                        int isAttributionCallbackImplemented,
                        int isEventSuccessCallbackImplemented,
                        int isEventFailureCallbackImplemented,
                        int isSessionSuccessCallbackImplemented,
                        int isSessionFailureCallbackImplemented,
                        int isDeferredDeeplinkCallbackImplemented,
                        int isSkanUpdatedCallbackImplemented) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        NSString *strAppToken = isStringValid(appToken) == true ? [NSString stringWithUTF8String:appToken] : nil;
        NSString *strEnvironment = isStringValid(environment) == true ? [NSString stringWithUTF8String:environment] : nil;
        NSString *strSdkPrefix = isStringValid(sdkPrefix) == true ? [NSString stringWithUTF8String:sdkPrefix] : nil;
        NSString *strDefaultTracker = isStringValid(defaultTracker) == true ? [NSString stringWithUTF8String:defaultTracker] : nil;
        NSString *strExternalDeviceId = isStringValid(externalDeviceId) == true ? [NSString stringWithUTF8String:externalDeviceId] : nil;

        ADJConfig *adjustConfig;

        if (allowSuppressLogLevel != -1) {
            adjustConfig = [[ADJConfig alloc] initWithAppToken:strAppToken
                                                   environment:strEnvironment
                                           andSuppressLogLevel:(BOOL)allowSuppressLogLevel];
        } else {
            adjustConfig = [[ADJConfig alloc] initWithAppToken:strAppToken
                                                andEnvironment:strEnvironment];
        }

        // set SDK prefix
        [adjustConfig setSdkPrefix:strSdkPrefix];

        // check if user has selected to implement any of the callbacks
        if (isAttributionCallbackImplemented ||
            isEventSuccessCallbackImplemented ||
            isEventFailureCallbackImplemented ||
            isSessionSuccessCallbackImplemented ||
            isSessionFailureCallbackImplemented ||
            isDeferredDeeplinkCallbackImplemented ||
            isSkanUpdatedCallbackImplemented) {
            [adjustConfig setDelegate:
                [AdjustUnityDelegate getInstanceWithSwizzleOfAttributionCallback:isAttributionCallbackImplemented
                                                            eventSuccessCallback:isEventSuccessCallbackImplemented
                                                            eventFailureCallback:isEventFailureCallbackImplemented
                                                          sessionSuccessCallback:isSessionSuccessCallbackImplemented
                                                          sessionFailureCallback:isSessionFailureCallbackImplemented
                                                        deferredDeeplinkCallback:isDeferredDeeplinkCallbackImplemented
                                                             skanUpdatedCallback:isSkanUpdatedCallbackImplemented
                                                    shouldLaunchDeferredDeeplink:isDeferredDeeplinkOpeningEnabled
                                                    andAdjustUnityGameObjectName:strGameObjectName]];
        }

        // log level
        if (logLevel != -1) {
            [adjustConfig setLogLevel:(ADJLogLevel)logLevel];
        }

        // Send in background.
        if (isSendingInBackgroundEnabled != -1) {
            if ((BOOL)isSendingInBackgroundEnabled == YES) {
                [adjustConfig enableSendingInBackground];
            }
        }

        // AdServices.framework handling
        if (isAdServicesEnabled != -1) {
            if ((BOOL)isAdServicesEnabled == NO) {
                [adjustConfig disableAdServices];
            }
        }

        // SKAN attribution
        if (isSkanAttributionEnabled != -1) {
            if ((BOOL)isSkanAttributionEnabled == NO) {
                [adjustConfig disableSkanAttribution];
            }
        }

        // IDFA reading
        if (isIdfaReadingEnabled != -1) {
            if ((BOOL)isIdfaReadingEnabled == NO) {
                [adjustConfig disableIdfaReading];
            }
        }

        // LinkMe
        if (isLinkMeEnabled != -1) {
            if ((BOOL)isLinkMeEnabled == YES) {
                [adjustConfig enableLinkMe];
            }
        }

        // ATT dialog delay
        if (attConsentWaitingInterval != -1) {
            [adjustConfig setAttConsentWaitingInterval:attConsentWaitingInterval];
        }

        // deduplication IDs max number
        if (eventDeduplicationIdsMaxSize != -1) {
            [adjustConfig setEventDeduplicationIdsMaxSize:eventDeduplicationIdsMaxSize];
        }

        // cost data in attribution callback
        if (isCostDataInAttributionEnabled != -1) {
            if ((BOOL)isCostDataInAttributionEnabled == YES) {
                [adjustConfig enableCostDataInAttribution];
            }
        }

        // read device info only once
        if (isDeviceIdsReadingOnceEnabled != -1) {
            if ((BOOL)isDeviceIdsReadingOnceEnabled == YES) {
                [adjustConfig enableDeviceIdsReadingOnce];
            }
        }

        // default tracker
        if (strDefaultTracker != nil) {
            [adjustConfig setDefaultTracker:strDefaultTracker];
        }

        // external device identifier
        if (strExternalDeviceId != nil) {
            [adjustConfig setExternalDeviceId:strExternalDeviceId];
        }

        // URL strategy
        if (shouldUseSubdomains != -1 && isDataResidency != -1) {
            NSArray *urlStrategyDomains = convertArrayParameters(jsonUrlStrategyDomains);
            if (urlStrategyDomains != nil) {
                [adjustConfig setUrlStrategy:urlStrategyDomains
                              withSubdomains:(BOOL)shouldUseSubdomains
                            andDataResidency:(BOOL)isDataResidency];
            }
        }

        // initialize the SDK
        [Adjust initSdk:adjustConfig];
    }

    void _AdjustTrackEvent(const char* eventToken,
                           double revenue,
                           const char* currency,
                           const char* receipt,
                           const char* productId,
                           const char* transactionId,
                           const char* callbackId,
                           const char* deduplicationId,
                           const char* jsonCallbackParameters,
                           const char* jsonPartnerParameters) {
        NSString *strEventToken = isStringValid(eventToken) == true ? [NSString stringWithUTF8String:eventToken] : nil;
        ADJEvent *event = [[ADJEvent alloc] initWithEventToken:strEventToken];

        // revenue and currency
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [event setRevenue:revenue currency:stringCurrency];
        }

        // callback parameters
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [event addCallbackParameter:key value:value];
            }
        }

        // partner parameters
        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [event addPartnerParameter:key value:value];
            }
        }

        // transaction ID
        if (transactionId != NULL) {
            NSString *strTransactionId = [NSString stringWithUTF8String:transactionId];
            [event setTransactionId:strTransactionId];
        }

        // product ID
        if (productId != NULL) {
            NSString *strProductId = [NSString stringWithUTF8String:productId];
            [event setProductId:strProductId];
        }

        // receipt (base64 encoded string)
        if (receipt != NULL) {
            NSString *strReceipt = [NSString stringWithUTF8String:receipt];
            NSData *dataReceipt = [[NSData alloc] initWithBase64EncodedString:strReceipt options:0];
            [event setReceipt:dataReceipt];
        }

        // callback ID
        if (callbackId != NULL) {
            NSString *strCallbackId = [NSString stringWithUTF8String:callbackId];
            [event setCallbackId:strCallbackId];
        }

        // deduplication ID
        if (deduplicationId != NULL) {
            NSString *strDeduplicationId = [NSString stringWithUTF8String:deduplicationId];
            [event setDeduplicationId:strDeduplicationId];
        }

        // track event
        [Adjust trackEvent:event];
    }

    void _AdjustTrackSubsessionStart() {
        [Adjust trackSubsessionStart];
    }

    void _AdjustTrackSubsessionEnd() {
        [Adjust trackSubsessionEnd];
    }

    void _AdjustEnable() {
        [Adjust enable];
    }

    void _AdjustDisable() {
        [Adjust disable];
    }

    void _AdjustEnableCoppaCompliance() {
        [Adjust enableCoppaCompliance];
    }

    void _AdjustDisableCoppaCompliance() {
        [Adjust disableCoppaCompliance];
    }

    void _AdjustIsEnabled(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust isEnabledWithCompletionHandler:^(BOOL isEnabled) {
            NSString *strIsEnabled = isEnabled ? @"true" : @"false";
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustIsEnabledGetter",
                             [strIsEnabled UTF8String]);
        }];
    }

    void _AdjustSwitchToOfflineMode() {
        [Adjust switchToOfflineMode];
    }

    void _AdjustSwitchBackToOnlineMode() {
        [Adjust switchBackToOnlineMode];
    }

    void _AdjustSetPushToken(const char* pushToken) {
        if (pushToken != NULL) {
            NSString *strPushToken = [NSString stringWithUTF8String:pushToken];
            [Adjust setPushTokenAsString:strPushToken];
        }
    }

    void _AdjustProcessDeeplink(const char* deeplink) {
        if (deeplink != NULL) {
            NSString *strDeeplink = [NSString stringWithUTF8String:deeplink];
            NSURL *urlDeeplink;
            if ([NSString instancesRespondToSelector:@selector(stringByAddingPercentEncodingWithAllowedCharacters:)]) {
                urlDeeplink = [NSURL URLWithString:[strDeeplink stringByAddingPercentEncodingWithAllowedCharacters:[NSCharacterSet URLFragmentAllowedCharacterSet]]];
            } else {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
                urlDeeplink = [NSURL URLWithString:[strDeeplink stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
            }
#pragma clang diagnostic pop

            [Adjust processDeeplink:urlDeeplink];
        }
    }

    void _AdjustGetIdfa(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust idfaWithCompletionHandler:^(NSString * _Nullable idfa) {
            // TODO: nil checks
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustIdfaGetter",
                             [idfa UTF8String]);
        }];
    }

    // void _AdjustGetIdfv(DelegateCallbackIdfvGetter callback) {
    void _AdjustGetIdfv(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust idfvWithCompletionHandler:^(NSString * _Nullable idfv) {
            // TODO: nil checks
            // callback([idfv UTF8String]);
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustIdfvGetter",
                             [idfv UTF8String]);
        }];
    }

    void _AdjustGetAdid(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust adidWithCompletionHandler:^(NSString * _Nullable adid) {
            // TODO: nil checks
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustAdidGetter",
                             [adid UTF8String]);
        }];
    }

    void _AdjustGetSdkVersion(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust sdkVersionWithCompletionHandler:^(NSString * _Nullable sdkVersion) {
            // TODO: nil checks
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustSdkVersionGetter",
                             [sdkVersion UTF8String]);
        }];
    }

    void _AdjustGetAttribution(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        [Adjust attributionWithCompletionHandler:^(ADJAttribution * _Nullable attribution) {
            // TODO: nil checks
            NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
            addValueOrEmpty(dictionary, @"trackerToken", attribution.trackerToken);
            addValueOrEmpty(dictionary, @"trackerName", attribution.trackerName);
            addValueOrEmpty(dictionary, @"network", attribution.network);
            addValueOrEmpty(dictionary, @"campaign", attribution.campaign);
            addValueOrEmpty(dictionary, @"creative", attribution.creative);
            addValueOrEmpty(dictionary, @"adgroup", attribution.adgroup);
            addValueOrEmpty(dictionary, @"clickLabel", attribution.clickLabel);
            addValueOrEmpty(dictionary, @"costType", attribution.costType);
            addValueOrEmpty(dictionary, @"costAmount", attribution.costAmount);
            addValueOrEmpty(dictionary, @"costCurrency", attribution.costCurrency);
            NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dictionary
                                                                      options:0
                                                                        error:nil];
            NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                                   length:[dataAttribution length]
                                                                 encoding:NSUTF8StringEncoding];
            const char* attributionCString = [stringAttribution UTF8String];
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustAttributionGetter",
                             attributionCString);
        }];
    }

    void _AdjustGdprForgetMe() {
        [Adjust gdprForgetMe];
    }

    void _AdjustAddGlobalPartnerParameter(const char* key, const char* value) {
        if (key != NULL && value != NULL) {
            NSString *strKey = [NSString stringWithUTF8String:key];
            NSString *strValue = [NSString stringWithUTF8String:value];
            [Adjust addGlobalPartnerParameter:strValue forKey:strKey];
        }
    }

    void _AdjustAddGlobalCallbackParameter(const char* key, const char* value) {
        if (key != NULL && value != NULL) {
            NSString *strKey = [NSString stringWithUTF8String:key];
            NSString *strValue = [NSString stringWithUTF8String:value];
            [Adjust addGlobalCallbackParameter:strValue forKey:strKey];
        }
    }

    void _AdjustRemoveGlobalPartnerParameter(const char* key) {
        if (key != NULL) {
            NSString *strKey = [NSString stringWithUTF8String:key];
            [Adjust removeGlobalPartnerParameterForKey:strKey];
        }
    }

    void _AdjustRemoveGlobalCallbackParameter(const char* key) {
        if (key != NULL) {
            NSString *strKey = [NSString stringWithUTF8String:key];
            [Adjust removeGlobalCallbackParameterForKey:strKey];
        }
    }

    void _AdjustRemoveGlobalPartnerParameters() {
        [Adjust removeGlobalPartnerParameters];
    }

    void _AdjustRemoveGlobalCallbackParameters() {
        [Adjust removeGlobalCallbackParameters];
    }

    void _AdjustTrackAdRevenue(const char* source,
                               double revenue,
                               const char* currency,
                               int adImpressionsCount,
                               const char* adRevenueNetwork,
                               const char* adRevenueUnit,
                               const char* adRevenuePlacement,
                               const char* jsonCallbackParameters,
                               const char* jsonPartnerParameters) {
        NSString *stringSource = isStringValid(source) == true ? [NSString stringWithUTF8String:source] : nil;
        ADJAdRevenue *adRevenue = [[ADJAdRevenue alloc] initWithSource:stringSource];

        // revenue and currency
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [adRevenue setRevenue:revenue currency:stringCurrency];
        }

        // ad impressions count
        if (adImpressionsCount != -1) {
            [adRevenue setAdImpressionsCount:adImpressionsCount];
        }

        // ad revenue network
        if (adRevenueNetwork != NULL) {
            NSString *stringAdRevenueNetwork = [NSString stringWithUTF8String:adRevenueNetwork];
            [adRevenue setAdRevenueNetwork:stringAdRevenueNetwork];
        }

        // ad revenue unit
        if (adRevenueUnit != NULL) {
            NSString *stringAdRevenueUnit = [NSString stringWithUTF8String:adRevenueUnit];
            [adRevenue setAdRevenueUnit:stringAdRevenueUnit];
        }

        // ad revenue placement
        if (adRevenuePlacement != NULL) {
            NSString *stringAdRevenuePlacement = [NSString stringWithUTF8String:adRevenuePlacement];
            [adRevenue setAdRevenuePlacement:stringAdRevenuePlacement];
        }

        // callback parameters
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [adRevenue addCallbackParameter:key value:value];
            }
        }

        // partner parameters
        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [adRevenue addPartnerParameter:key value:value];
            }
        }

        // track ad revenue
        [Adjust trackAdRevenue:adRevenue];
    }

    // TODO: consider non-string types for some fields?
    void _AdjustTrackAppStoreSubscription(const char* price,
                                          const char* currency,
                                          const char* transactionId,
                                          const char* receipt,
                                          const char* transactionDate,
                                          const char* salesRegion,
                                          const char* jsonCallbackParameters,
                                          const char* jsonPartnerParameters) {
        // mandatory fields
        NSDecimalNumber *mPrice;
        NSString *mCurrency;
        NSString *mTransactionId;
        NSData *mReceipt;

        // price
        if (price != NULL) {
            mPrice = [NSDecimalNumber decimalNumberWithString:[NSString stringWithUTF8String:price]];
        }

        // currency
        if (currency != NULL) {
            mCurrency = [NSString stringWithUTF8String:currency];
        }

        // transaction ID
        if (transactionId != NULL) {
            mTransactionId = [NSString stringWithUTF8String:transactionId];
        }

        // TODO: check if this is equivalent to event receipt logic
        // receipt
        if (receipt != NULL) {
            mReceipt = [[NSString stringWithUTF8String:receipt] dataUsingEncoding:NSUTF8StringEncoding];
        }

        ADJAppStoreSubscription *subscription =
        [[ADJAppStoreSubscription alloc] initWithPrice:mPrice
                                              currency:mCurrency
                                         transactionId:mTransactionId
                                            andReceipt:mReceipt];

        // optional fields below

        // transaction date
        if (transactionDate != NULL) {
            NSTimeInterval transactionDateInterval = [[NSString stringWithUTF8String:transactionDate] doubleValue] / 1000.0;
            NSDate *oTransactionDate = [NSDate dateWithTimeIntervalSince1970:transactionDateInterval];
            [subscription setTransactionDate:oTransactionDate];
        }

        // sales region
        if (salesRegion != NULL) {
            NSString *oSalesRegion = [NSString stringWithUTF8String:salesRegion];
            [subscription setSalesRegion:oSalesRegion];
        }

        // callback parameters
        NSArray *arrayCallbackParameters = convertArrayParameters(jsonCallbackParameters);
        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i++];
                NSString *value = arrayCallbackParameters[i++];
                [subscription addCallbackParameter:key value:value];
            }
        }

        // partner parameters
        NSArray *arrayPartnerParameters = convertArrayParameters(jsonPartnerParameters);
        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];
            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i++];
                NSString *value = arrayPartnerParameters[i++];
                [subscription addPartnerParameter:key value:value];
            }
        }
        
        // track subscription
        [Adjust trackAppStoreSubscription:subscription];
    }

    void _AdjustTrackThirdPartySharing(int enabled, const char* jsonGranularOptions, const char* jsonPartnerSharingSettings) {
        NSNumber *nEnabled = enabled >= 0 ? [NSNumber numberWithInt:enabled] : nil;
        ADJThirdPartySharing *adjustThirdPartySharing = [[ADJThirdPartySharing alloc] initWithIsEnabled:nEnabled];

        NSArray *arrayGranularOptions = convertArrayParameters(jsonGranularOptions);
        if (arrayGranularOptions != nil) {
            NSUInteger count = [arrayGranularOptions count];
            for (int i = 0; i < count;) {
                NSString *partnerName = arrayGranularOptions[i++];
                NSString *granularOptions = arrayGranularOptions[i++];
                // granularOptions is now NSString which pretty much contains array of partner key-value pairs
                if (granularOptions != nil) {
                    NSData *dataJson = [granularOptions dataUsingEncoding:NSUTF8StringEncoding];
                    NSArray *partnerGranularOptions = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:nil];
                    if (partnerGranularOptions != nil) {
                        // in here we have partner and key-value pair for it
                        for (int j = 0; j < [partnerGranularOptions count];) {
                            [adjustThirdPartySharing addGranularOption:partnerName
                                                                   key:partnerGranularOptions[j++]
                                                                 value:partnerGranularOptions[j++]];
                        }
                    }
                }
            }
        }
        NSArray *arrayPartnerSharingSettings = convertArrayParameters(jsonPartnerSharingSettings);
        if (arrayPartnerSharingSettings != nil) {
            NSUInteger count = [arrayPartnerSharingSettings count];
            for (int i = 0; i < count;) {
                NSString *partnerName = arrayPartnerSharingSettings[i++];
                NSString *sharingSettings = arrayPartnerSharingSettings[i++];
                // sharingSettings is now NSString which pretty much contains array of partner key-value pairs
                if (sharingSettings != nil) {
                    NSData *dataJson = [sharingSettings dataUsingEncoding:NSUTF8StringEncoding];
                    NSArray *partnerSharingSettings = [NSJSONSerialization JSONObjectWithData:dataJson options:0 error:nil];
                    if (partnerSharingSettings != nil) {
                        // in here we have partner and key-value pair for it
                        for (int j = 0; j < [partnerSharingSettings count];) {
                            [adjustThirdPartySharing addPartnerSharingSetting:partnerName
                                                                          key:partnerSharingSettings[j++]
                                                                        value:[partnerSharingSettings[j++] boolValue]];
                        }
                    }
                }
            }
        }

        [Adjust trackThirdPartySharing:adjustThirdPartySharing];
    }

    void _AdjustTrackMeasurementConsent(int enabled) {
        BOOL bEnabled = (BOOL)enabled;
        [Adjust trackMeasurementConsent:bEnabled];
    }

    void _AdjustRequestAppTrackingAuthorizationWithCompletionHandler(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        if (strGameObjectName == nil) {
            return;
        }

        [Adjust requestAppTrackingAuthorizationWithCompletionHandler:^(NSUInteger status) {
            NSString *stringStatus = [NSString stringWithFormat:@"%tu", status];
            const char* charStatus = [stringStatus UTF8String];
            UnitySendMessage([stringStatus UTF8String],
                             "UnityAdjustAttDialogCallback",
                             charStatus);
        }];
    }

    void _AdjustUpdateSkanConversionValue(int conversionValue,
                                          const char* coarseValue,
                                          int lockWindow,
                                          const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;

        if (coarseValue != NULL) {
            NSString *strCoarseValue = [NSString stringWithUTF8String:coarseValue];
            BOOL bLockWindow = (BOOL)lockWindow;
            [Adjust updateSkanConversionValue:conversionValue
                                  coarseValue:strCoarseValue
                                   lockWindow:[NSNumber numberWithBool:bLockWindow]
                        withCompletionHandler:^(NSError * _Nullable error) {
                if (strGameObjectName == nil) {
                    return;
                }

                // TODO: nil checks
                NSString *errorString = [error localizedDescription];
                const char* errorChar = [errorString UTF8String];
                UnitySendMessage([strGameObjectName UTF8String],
                                 "UnityAdjustSkanErrorCallback",
                                 errorChar);
            }];
        }
    }

    int _AdjustGetAppTrackingAuthorizationStatus() {
        return [Adjust appTrackingAuthorizationStatus];
    }

    void _AdjustGetLastDeeplink(const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        if (strGameObjectName == nil) {
            return;
        }

        [Adjust lastDeeplinkWithCompletionHandler:^(NSURL * _Nullable lastDeeplink) {
            // TODO: nil checks
            NSString *strLastDeeplink = [lastDeeplink absoluteString];
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustLastDeeplinkGetter",
                             [strLastDeeplink UTF8String]);
        }];
    }

    void _AdjustVerifyAppStorePurchase(const char* transactionId,
                                       const char* productId,
                                       const char* receipt,
                                       const char* gameObjectName) {
        NSString *strGameObjectName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        if (strGameObjectName == nil) {
            return;
        }

        NSString *strTransactionId;
        NSString *strProductId;
        NSData *dataReceipt;

        // transaction ID
        if (transactionId != NULL) {
            strTransactionId = [NSString stringWithUTF8String:transactionId];
        }

        // product ID
        if (productId != NULL) {
            strProductId = [NSString stringWithUTF8String:productId];
        }

        // receipt (base64 encoded string)
        if (receipt != NULL) {
            NSString *strReceipt = [NSString stringWithUTF8String:receipt];
            dataReceipt = [[NSData alloc] initWithBase64EncodedString:strReceipt options:0];
        }


        // verify the purchase
        ADJAppStorePurchase *purchase =
        [[ADJAppStorePurchase alloc] initWithTransactionId:strTransactionId
                                                 productId:strProductId
                                                andReceipt:dataReceipt];

        [Adjust verifyAppStorePurchase:purchase
                 withCompletionHandler:^(ADJPurchaseVerificationResult * _Nonnull verificationResult) {
            // TODO: nil checks
            NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
            addValueOrEmpty(dictionary, @"verificationStatus", verificationResult.verificationStatus);
            addValueOrEmpty(dictionary, @"code", [NSString stringWithFormat:@"%d", verificationResult.code]);
            addValueOrEmpty(dictionary, @"message", verificationResult.message);

            NSData *dataVerificationInfo = [NSJSONSerialization dataWithJSONObject:dictionary
                                                                           options:0
                                                                             error:nil];
            NSString *strVerificationInfo = [[NSString alloc] initWithBytes:[dataVerificationInfo bytes]
                                                                     length:[dataVerificationInfo length]
                                                                   encoding:NSUTF8StringEncoding];
            const char* verificationInfoCString = [strVerificationInfo UTF8String];
            UnitySendMessage([strGameObjectName UTF8String],
                             "UnityAdjustPurchaseVerificationCallback",
                             verificationInfoCString);
        }];
    }

    void _AdjustProcessAndResolveDeeplink(const char* deeplink, const char* gameObjectName) {
        NSString *strSceneName = isStringValid(gameObjectName) == true ? [NSString stringWithUTF8String:gameObjectName] : nil;
        if (deeplink != NULL) {
            NSString *strDeeplink = [NSString stringWithUTF8String:deeplink];
            NSURL *urlDeeplink;
            if ([NSString instancesRespondToSelector:@selector(stringByAddingPercentEncodingWithAllowedCharacters:)]) {
                urlDeeplink = [NSURL URLWithString:[strDeeplink stringByAddingPercentEncodingWithAllowedCharacters:[NSCharacterSet URLFragmentAllowedCharacterSet]]];
            } else {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
                urlDeeplink = [NSURL URLWithString:[strDeeplink stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
            }
#pragma clang diagnostic pop

            [Adjust processAndResolveDeeplink:urlDeeplink withCompletionHandler:^(NSString * _Nullable resolvedLink) {
                // TODO: nil checks
                const char* resolvedLinkCString = [resolvedLink UTF8String];
                UnitySendMessage([strSceneName UTF8String],
                                 "UnityAdjustResolvedDeeplinkCallback",
                                 resolvedLinkCString);
            }];
        }
    }

    void _AdjustSetTestOptions(const char* overwriteUrl,
                               const char* extraPath,
                               long timerIntervalInMilliseconds,
                               long timerStartInMilliseconds,
                               long sessionIntervalInMilliseconds,
                               long subsessionIntervalInMilliseconds,
                               int teardown,
                               int deleteState,
                               int noBackoffWait,
                               int adServicesFrameworkEnabled,
                               int attStatus,
                               const char *idfa) {
        NSMutableDictionary *testOptions = [NSMutableDictionary dictionary];

        NSString *strOverwriteUrl = isStringValid(overwriteUrl) == true ? [NSString stringWithUTF8String:overwriteUrl] : nil;
        if (strOverwriteUrl != nil) {
            testOptions[@"testUrlOverwrite"] = strOverwriteUrl;
        }
        NSString *strExtraPath = isStringValid(extraPath) == true ? [NSString stringWithUTF8String:extraPath] : nil;
        if (strExtraPath != nil && [strExtraPath length] > 0) {
            testOptions[@"extraPath"] = strExtraPath;
        }
        NSString *strIdfa = isStringValid(idfa) == true ? [NSString stringWithUTF8String:idfa] : nil;
        if (strIdfa != nil && [strIdfa length] > 0) {
            testOptions[@"idfa"] = strIdfa;
        }

        testOptions[@"timerIntervalInMilliseconds"] = [NSNumber numberWithLong:timerIntervalInMilliseconds];
        testOptions[@"timerStartInMilliseconds"] = [NSNumber numberWithLong:timerStartInMilliseconds];
        testOptions[@"sessionIntervalInMilliseconds"] = [NSNumber numberWithLong:sessionIntervalInMilliseconds];
        testOptions[@"subsessionIntervalInMilliseconds"] = [NSNumber numberWithLong:subsessionIntervalInMilliseconds];
        testOptions[@"attStatusInt"] = [NSNumber numberWithInt:attStatus];

        if (teardown != -1) {
            [AdjustUnityDelegate teardown];
            testOptions[@"teardown"] = [NSNumber numberWithInt:teardown];
        }
        if (deleteState != -1) {
            testOptions[@"deleteState"] = [NSNumber numberWithInt:deleteState];
        }
        if (noBackoffWait != -1) {
            testOptions[@"noBackoffWait"] = [NSNumber numberWithInt:noBackoffWait];
        }
        if (adServicesFrameworkEnabled != -1) {
            testOptions[@"adServicesFrameworkEnabled"] = [NSNumber numberWithInt:adServicesFrameworkEnabled];
        }

        [Adjust setTestOptions:testOptions];
    }
}
