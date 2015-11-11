#import "AdjustUnity.h"

#import "ADJEvent.h"
#import "ADJConfig.h"

@implementation AdjustUnity

static char *adjustSceneName = nil;
static id<AdjustDelegate> adjustUnityInstance = nil;

- (id) init {
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

    UnitySendMessage(adjustSceneName, "getNativeMessage", charArrayAttribution);
}

@end

// Method for converting JSON stirng parameters into NSArray object.
NSArray* ConvertArrayParameters (const char* cStringJsonArrayParameters) {
    if (cStringJsonArrayParameters == NULL) {
        return nil;
    }

    NSString *stringJsonArrayParameters = [NSString stringWithUTF8String:cStringJsonArrayParameters];

    NSError *error = nil;
    NSArray *arrayParameters = nil;


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
    void _AdjustLaunchApp(const char* appToken, const char* environment, const char* sdkPrefix, int logLevel, int eventBuffering, int macMd5TrackingEnabled, const char* sceneName) {
        NSString *stringSdkPrefix = [NSString stringWithUTF8String:sdkPrefix];
        NSString *stringAppToken = [NSString stringWithUTF8String:appToken];
        NSString *stringEnvironment = [NSString stringWithUTF8String:environment];
        NSString *stringSceneName = [NSString stringWithUTF8String:sceneName];

        ADJConfig *adjustConfig = [ADJConfig configWithAppToken:stringAppToken
                                                    environment:stringEnvironment];

        [adjustConfig setSdkPrefix:stringSdkPrefix];

        // Optional fields.
        if (logLevel != -1) {
            [adjustConfig setLogLevel:(ADJLogLevel)logLevel];
        }

        if (eventBuffering != -1) {
            [adjustConfig setEventBufferingEnabled:(BOOL)eventBuffering];
        }

        if (macMd5TrackingEnabled != -1) {
            [adjustConfig setMacMd5TrackingEnabled:(BOOL)macMd5TrackingEnabled];
        }

        if (sceneName != NULL && [stringSceneName length] > 0) {
            adjustSceneName = strdup(sceneName);
            adjustUnityInstance = [[AdjustUnity alloc] init];
            [adjustConfig setDelegate:(id)adjustUnityInstance];
        }

        NSLog(@"%@, %@, %@, %d, %d, %d, %@", stringAppToken, stringEnvironment, stringSdkPrefix, logLevel, eventBuffering, macMd5TrackingEnabled, stringSceneName);

        // Launch adjust instance.
        [Adjust appDidLaunch:adjustConfig];
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
}
