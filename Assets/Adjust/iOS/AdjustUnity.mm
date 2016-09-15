#import "AdjustUnity.h"

#import "ADJEvent.h"
#import "ADJConfig.h"

@implementation AdjustUnity

static char* adjustSceneName = nil;
static BOOL launchAdjustDeferredDeeplink = YES;
static id<AdjustDelegate> adjustUnityInstance = nil;

- (id)init {
    self = [super init];
    return self;
}

- (void)adjustAttributionChanged:(ADJAttribution *)attribution {
    NSDictionary *dicAttribution = [attribution dictionary];
    NSData *dataAttribution = [NSJSONSerialization dataWithJSONObject:dicAttribution options:0 error:nil];
    NSString *stringAttribution = [[NSString alloc] initWithBytes:[dataAttribution bytes]
                                                           length:[dataAttribution length]
                                                         encoding:NSUTF8StringEncoding];

    const char* charArrayAttribution = [stringAttribution UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeAttribution", charArrayAttribution);
}

- (void)adjustEventTrackingSucceeded:(ADJEventSuccess *)eventSuccessResponseData {
    NSMutableDictionary *eventSuccessDataDict = [NSMutableDictionary dictionary];

    if (eventSuccessResponseData.message != nil) {
        [eventSuccessDataDict setObject:eventSuccessResponseData.message forKey:@"message"];
    }

    if (eventSuccessResponseData.timeStamp != nil) {
        [eventSuccessDataDict setObject:eventSuccessResponseData.timeStamp forKey:@"timestamp"];
    }

    if (eventSuccessResponseData.adid != nil) {
        [eventSuccessDataDict setObject:eventSuccessResponseData.adid forKey:@"adid"];
    }

    if (eventSuccessResponseData.eventToken != nil) {
        [eventSuccessDataDict setObject:eventSuccessResponseData.eventToken forKey:@"eventToken"];
    }

    if (eventSuccessResponseData.jsonResponse != nil) {
        [eventSuccessDataDict setObject:eventSuccessResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataEventSuccess = [NSJSONSerialization dataWithJSONObject:eventSuccessDataDict options:0 error:nil];
    NSString *stringEventSuccess = [[NSString alloc] initWithBytes:[dataEventSuccess bytes]
                                                            length:[dataEventSuccess length]
                                                          encoding:NSUTF8StringEncoding];

    const char* charArrayEventSuccess = [stringEventSuccess UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeEventSuccess", charArrayEventSuccess);
}

- (void)adjustEventTrackingFailed:(ADJEventFailure *)eventFailureResponseData {
    NSMutableDictionary *eventFailureDataDict = [NSMutableDictionary dictionary];

    if (eventFailureResponseData.message != nil) {
        [eventFailureDataDict setObject:eventFailureResponseData.message forKey:@"message"];
    }

    if (eventFailureResponseData.timeStamp != nil) {
        [eventFailureDataDict setObject:eventFailureResponseData.timeStamp forKey:@"timestamp"];
    }

    if (eventFailureResponseData.adid != nil) {
        [eventFailureDataDict setObject:eventFailureResponseData.adid forKey:@"adid"];
    }

    if (eventFailureResponseData.eventToken != nil) {
        [eventFailureDataDict setObject:eventFailureResponseData.eventToken forKey:@"eventToken"];
    }

    if (eventFailureResponseData.jsonResponse != nil) {
        [eventFailureDataDict setObject:eventFailureResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    [eventFailureDataDict setObject:(eventFailureResponseData.willRetry == YES ? @"true" : @"false")
                             forKey:@"willRetry"];

    NSData *dataEventFailure = [NSJSONSerialization dataWithJSONObject:eventFailureDataDict options:0 error:nil];
    NSString *stringEventFailure = [[NSString alloc] initWithBytes:[dataEventFailure bytes]
                                                            length:[dataEventFailure length]
                                                          encoding:NSUTF8StringEncoding];

    const char* charArrayEventFailure = [stringEventFailure UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeEventFailure", charArrayEventFailure);
}

- (void)adjustSessionTrackingSucceeded:(ADJSessionSuccess *)sessionSuccessResponseData {
    NSMutableDictionary *sessionSuccessDataDict = [NSMutableDictionary dictionary];

    if (sessionSuccessResponseData.message != nil) {
        [sessionSuccessDataDict setObject:sessionSuccessResponseData.message forKey:@"message"];
    }

    if (sessionSuccessResponseData.timeStamp != nil) {
        [sessionSuccessDataDict setObject:sessionSuccessResponseData.timeStamp forKey:@"timestamp"];
    }

    if (sessionSuccessResponseData.adid != nil) {
        [sessionSuccessDataDict setObject:sessionSuccessResponseData.adid forKey:@"adid"];
    }

    if (sessionSuccessResponseData.jsonResponse != nil) {
        [sessionSuccessDataDict setObject:sessionSuccessResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    NSData *dataSessionSuccess = [NSJSONSerialization dataWithJSONObject:sessionSuccessDataDict options:0 error:nil];
    NSString *stringSessionSuccess = [[NSString alloc] initWithBytes:[dataSessionSuccess bytes]
                                                              length:[dataSessionSuccess length]
                                                            encoding:NSUTF8StringEncoding];

    const char* charArraySessionSuccess = [stringSessionSuccess UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeSessionSuccess", charArraySessionSuccess);
}

- (void)adjustSessionTrackingFailed:(ADJSessionFailure *)sessionFailureResponseData {
    NSMutableDictionary *sessionFailureDataDict = [NSMutableDictionary dictionary];

    if (sessionFailureResponseData.message != nil) {
        [sessionFailureDataDict setObject:sessionFailureResponseData.message forKey:@"message"];
    }

    if (sessionFailureResponseData.timeStamp != nil) {
        [sessionFailureDataDict setObject:sessionFailureResponseData.timeStamp forKey:@"timestamp"];
    }

    if (sessionFailureResponseData.adid != nil) {
        [sessionFailureDataDict setObject:sessionFailureResponseData.adid forKey:@"adid"];
    }

    if (sessionFailureResponseData.jsonResponse != nil) {
        [sessionFailureDataDict setObject:sessionFailureResponseData.jsonResponse forKey:@"jsonResponse"];
    }

    [sessionFailureDataDict setObject:(sessionFailureResponseData.willRetry == YES ? @"true" : @"false")
                               forKey:@"willRetry"];

    NSData *dataSessionFailure = [NSJSONSerialization dataWithJSONObject:sessionFailureDataDict options:0 error:nil];
    NSString *stringSessionFailure = [[NSString alloc] initWithBytes:[dataSessionFailure bytes]
                                                              length:[dataSessionFailure length]
                                                            encoding:NSUTF8StringEncoding];

    const char* charArraySessionFailure = [stringSessionFailure UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeSessionFailure", charArraySessionFailure);
}

- (BOOL)adjustDeeplinkResponse:(NSURL *)deeplink {
    NSString *stringDeeplink = [deeplink absoluteString];
    const char* charDeeplink = [stringDeeplink UTF8String];

    UnitySendMessage(adjustSceneName, "GetNativeDeferredDeeplink", charDeeplink);

    return launchAdjustDeferredDeeplink;
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
    void _AdjustLaunchApp(const char* appToken, const char* environment, const char* sdkPrefix, int allowSuppressLogLevel, int logLevel, int eventBuffering, int sendInBackground, double delayStart, const char* userAgent, int launchDeferredDeeplink, const char* sceneName) {
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

        if (launchDeferredDeeplink != -1) {
            launchAdjustDeferredDeeplink = (BOOL)launchDeferredDeeplink;
        }

        if (stringUserAgent != NULL) {
            if ([stringUserAgent length] > 0) {
                [adjustConfig setUserAgent:stringUserAgent];
            }
        }

        if (sceneName != NULL) {
            if ([stringSceneName length] > 0) {
                adjustSceneName = strdup(sceneName);
                adjustUnityInstance = [[AdjustUnity alloc] init];
                [adjustConfig setDelegate:(id)adjustUnityInstance];
            }
        }

        NSLog(@"%@, %@, %@, %d, %d, %d, %d, %.1f, %@, %d, %@", stringAppToken, stringEnvironment, stringSdkPrefix, allowSuppressLogLevel, logLevel, eventBuffering, sendInBackground, delayStart, stringUserAgent, launchDeferredDeeplink, stringSceneName);

        // Launch adjust instance.
        [Adjust appDidLaunch:adjustConfig];

        // Since v4.7.0 session is not automatically started after calling appDidLaunch, thus calling trackSubsessionStart.
        [Adjust trackSubsessionStart];
    }

    void _AdjustTrackEvent(const char* eventToken, double revenue, const char* currency, const char* receipt, const char* transactionId, int isReceiptSet, const char* jsonCallbackParameters, const char* jsonPartnerParameters) {
        NSString *stringEventToken = [NSString stringWithUTF8String:eventToken];

        ADJEvent *event = [ADJEvent eventWithEventToken:stringEventToken];

        // Optional fields.
        if (revenue != -1 || currency != NULL) {
            NSString *stringCurrency = [NSString stringWithUTF8String:currency];
            [event setRevenue:revenue currency:stringCurrency];
        }

        NSArray *arrayCallbackParameters = ConvertArrayParameters(jsonCallbackParameters);

        if (arrayCallbackParameters != nil) {
            int count = [arrayCallbackParameters count];

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
            int count = [arrayPartnerParameters count];

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
        const char* idfaCString = [idfa UTF8String];
        char* idfaCStringCopy = strdup(idfaCString);

        return idfaCStringCopy;
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
