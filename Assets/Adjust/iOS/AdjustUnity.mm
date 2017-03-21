#import "AdjustUnity.h"
#import "AdjustUnityDelegate.h"

#import "ADJEvent.h"
#import "ADJConfig.h"

@implementation AdjustUnity

- (id)init {
    self = [super init];
    return self;
}

@end

// Method for converting JSON stirng parameters into NSArray object.
NSArray* ConvertArrayParameters(const char* cStringJsonArrayParameters) {
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

extern "C"
{
    void addValueOrEmpty(NSMutableDictionary *dictionary, NSString *key, NSObject *value) {
        if (nil != value) {
            [dictionary setObject:[NSString stringWithFormat:@"%@", value] forKey:key];
        } else {
            [dictionary setObject:@"" forKey:key];
        }
    }

    void _AdjustLaunchApp(const char* appToken, const char* environment, const char* sdkPrefix, int allowSuppressLogLevel, int logLevel, 
        int eventBuffering, int sendInBackground, double delayStart, const char* userAgent, int launchDeferredDeeplink, const char* sceneName,
        int isAttributionCallbackImplemented, int isEventSuccessCallbackImplemented, int isEventFailureCallbackImplemented,
        int isSessionSuccessCallbackImplemented, int isSessionFailureCallbackImplemented, int isDeferredDeeplinkCallbackImplemented) {
        NSString *stringSdkPrefix = [NSString stringWithUTF8String:sdkPrefix];
        NSString *stringAppToken = [NSString stringWithUTF8String:appToken];
        NSString *stringEnvironment = [NSString stringWithUTF8String:environment];
        NSString *stringUserAgent = [NSString stringWithUTF8String:userAgent];
        NSString *stringSceneName = [NSString stringWithUTF8String:sceneName];

        ADJConfig *adjustConfig;

        if (allowSuppressLogLevel != -1) {
            adjustConfig = [ADJConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment
                                   allowSuppressLogLevel:(BOOL)allowSuppressLogLevel];
        } else {
            adjustConfig = [ADJConfig configWithAppToken:stringAppToken
                                             environment:stringEnvironment];
        }

        [adjustConfig setSdkPrefix:stringSdkPrefix];

        // Attribution delegate & other delegates
        if (isAttributionCallbackImplemented || isEventSuccessCallbackImplemented ||
            isEventFailureCallbackImplemented || isSessionSuccessCallbackImplemented ||
            isSessionFailureCallbackImplemented || isDeferredDeeplinkCallbackImplemented) {
            [adjustConfig setDelegate:
                [AdjustUnityDelegate getInstanceWithSwizzleOfAttributionCallback:isAttributionCallbackImplemented
                                                          eventSucceededCallback:isEventSuccessCallbackImplemented
                                                             eventFailedCallback:isEventFailureCallbackImplemented
                                                        sessionSucceededCallback:isSessionSuccessCallbackImplemented
                                                           sessionFailedCallback:isSessionFailureCallbackImplemented
                                                        deferredDeeplinkCallback:isDeferredDeeplinkCallbackImplemented
                                                    shouldLaunchDeferredDeeplink:launchDeferredDeeplink
                                                        withAdjustUnitySceneName:stringSceneName]];
        }

        // Optional fields.
        if (logLevel != -1) {
            [adjustConfig setLogLevel:(ADJLogLevel)logLevel];
        }

        if (eventBuffering != -1) {
            [adjustConfig setEventBufferingEnabled:(BOOL)eventBuffering];
        }

        if (sendInBackground != -1) {
            [adjustConfig setSendInBackground:(BOOL)sendInBackground];
        }

        if (delayStart != -1) {
            [adjustConfig setDelayStart:delayStart];
        }

        if (stringUserAgent != NULL) {
            if ([stringUserAgent length] > 0) {
                [adjustConfig setUserAgent:stringUserAgent];
            }
        }

        // NSLog(@"%@, %@, %@, %d, %d, %d, %d, %.1f, %@, %d, %@", stringAppToken, stringEnvironment, stringSdkPrefix, allowSuppressLogLevel, logLevel, eventBuffering, sendInBackground, delayStart, stringUserAgent, launchDeferredDeeplink, stringSceneName);

        // Launch adjust instance.
        [Adjust appDidLaunch:adjustConfig];

        // Since v4.7.0 session is not automatically started after calling appDidLaunch, thus calling trackSubsessionStart.
        [Adjust trackSubsessionStart];
    }

    void _AdjustTrackEvent(const char* eventToken, double revenue, const char* currency, const char* receipt, const char* transactionId, int isReceiptSet, const char* jsonCallbackParameters, const char* jsonPartnerParameters) {
        NSString *stringEventToken = [NSString stringWithUTF8String:eventToken];

        ADJEvent *event = [ADJEvent eventWithEventToken:stringEventToken];

        // Optional fields.
        if (revenue != -1 && currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [event setRevenue:revenue currency:stringCurrency];
        }

        NSArray *arrayCallbackParameters = ConvertArrayParameters(jsonCallbackParameters);

        if (arrayCallbackParameters != nil) {
            NSUInteger count = [arrayCallbackParameters count];

            for (int i = 0; i < count;) {
                NSString *key = arrayCallbackParameters[i];
                i++;

                NSString *value = arrayCallbackParameters[i];
                i++;

                [event addCallbackParameter:key value:value];
            }
        }

        NSArray *arrayPartnerParameters = ConvertArrayParameters(jsonPartnerParameters);

        if (arrayPartnerParameters != nil) {
            NSUInteger count = [arrayPartnerParameters count];

            for (int i = 0; i < count;) {
                NSString *key = arrayPartnerParameters[i];
                i++;

                NSString *value = arrayPartnerParameters[i];
                i++;

                [event addPartnerParameter:key value:value];
            }
        }

        if ([[NSNumber numberWithInt:isReceiptSet] boolValue]) {
            NSString *stringReceipt = nil;
            NSString *stringTransactionId = nil;

            if (receipt != NULL) {
                stringReceipt = [NSString stringWithUTF8String:receipt];
            }

            if (transactionId != NULL) {
                stringTransactionId = [NSString stringWithUTF8String:transactionId];
            }

            [event setReceipt:[stringReceipt dataUsingEncoding:NSUTF8StringEncoding] transactionId:stringTransactionId];
        } else {
            if (transactionId != NULL) {
                NSString *stringTransactionId = [NSString stringWithUTF8String:transactionId];
                [event setTransactionId:stringTransactionId];
            }
        }

        [Adjust trackEvent:event];
    }

    void _AdjustSetEnabled(int enabled) {
        BOOL bEnabled = (BOOL)enabled;

        [Adjust setEnabled:bEnabled];
    }

    int _AdjustIsEnabled() {
        BOOL isEnabled = [Adjust isEnabled];
        int iIsEnabled = (int)isEnabled;

        return iIsEnabled;
    }

    void _AdjustSetOfflineMode(int enabled) {
        BOOL bEnabled = (BOOL)enabled;

        [Adjust setOfflineMode:bEnabled];
    }

    void _AdjustSetDeviceToken(const char* deviceToken) {
        NSString *stringDeviceToken = [NSString stringWithUTF8String:deviceToken];

        [Adjust setDeviceToken:[stringDeviceToken dataUsingEncoding:NSUTF8StringEncoding]];
    }

    char* _AdjustGetIdfa() {
        NSString *idfa = [Adjust idfa];

        if (nil == idfa) {
            return NULL;
        }

        const char* idfaCString = [idfa UTF8String];

        if (NULL == idfaCString) {
            return NULL;
        }

        char* idfaCStringCopy = strdup(idfaCString);

        return idfaCStringCopy;
    }

    char* _AdjustGetAdid() {
        NSString *adid = [Adjust adid];

        if (nil == adid) {
            return NULL;
        }

        const char* adidCString = [adid UTF8String];

        if (NULL == adidCString) {
            return NULL;
        }

        char* adidCStringCopy = strdup(adidCString);

        return adidCStringCopy;
    }

    char* _AdjustGetAttribution() {
        ADJAttribution *attribution = [Adjust attribution];

        if (nil == attribution) {
            return NULL;
        }

        NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];

        addValueOrEmpty(dictionary, @"trackerToken", attribution.trackerToken);
        addValueOrEmpty(dictionary, @"trackerName", attribution.trackerName);
        addValueOrEmpty(dictionary, @"network", attribution.network);
        addValueOrEmpty(dictionary, @"campaign", attribution.campaign);
        addValueOrEmpty(dictionary, @"creative", attribution.creative);
        addValueOrEmpty(dictionary, @"adgroup", attribution.adgroup);
        addValueOrEmpty(dictionary, @"clickLabel", attribution.clickLabel);
        addValueOrEmpty(dictionary, @"adid", attribution.adid);

        NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
        NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                               length:[dataAttribution length]
                                                             encoding:NSUTF8StringEncoding];

        const char* attributionCString = [stringAttribution UTF8String];
        char* attributionCStringCopy = strdup(attributionCString);

        return attributionCStringCopy;
    }

    void _AdjustSendFirstPackages() {
        [Adjust sendFirstPackages];
    }

    void _AdjustAddSessionPartnerParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];

        [Adjust addSessionPartnerParameter:stringKey value:stringValue];
    }

    void _AdjustAddSessionCallbackParameter(const char* key, const char* value) {
        NSString *stringKey = [NSString stringWithUTF8String:key];
        NSString *stringValue = [NSString stringWithUTF8String:value];

        [Adjust addSessionCallbackParameter:stringKey value:stringValue];
    }

    void _AdjustRemoveSessionPartnerParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];

        [Adjust removeSessionPartnerParameter:stringKey];
    }

    void _AdjustRemoveSessionCallbackParameter(const char* key) {
        NSString *stringKey = [NSString stringWithUTF8String:key];

        [Adjust removeSessionCallbackParameter:stringKey];
    }

    void _AdjustResetSessionPartnerParameters() {
        [Adjust resetSessionPartnerParameters];
    }

    void _AdjustResetSessionCallbackParameters() {
        [Adjust resetSessionCallbackParameters];
    }
}
