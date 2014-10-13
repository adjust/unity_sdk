#import "NSString+AIAdditions.h"
#import "AIAdjustFactory.h"
#import "AdjustUnity.h"

@implementation AdjustUnity

static char *adjustSceneName = nil;
static id<AdjustDelegate> adjustUnityInstance = nil;

- (id) init {
    self = [super init];
    return self;
}

- (void)adjustFinishedTrackingWithResponse:(AIResponseData *)responseData {
    NSDictionary *dicResponseData = [responseData dictionary];
    NSData *dResponseData = [NSJSONSerialization dataWithJSONObject:dicResponseData options:0 error:nil];
    NSString *sResponseData = [[NSString alloc] initWithBytes:[dResponseData bytes]
                                                       length:[dResponseData length]
                                                     encoding:NSUTF8StringEncoding];
    const char * cResponseData= [sResponseData UTF8String];

    UnitySendMessage(adjustSceneName, "getNativeMessage", cResponseData);
}

@end

NSDictionary* ConvertParameters (const char* cJsonParameters)
{
    if (cJsonParameters == nil) {
        return nil;
    }
    NSString *sJsonParameters = [NSString stringWithUTF8String: cJsonParameters];

    NSDictionary * parameters = nil;
    NSError *error = nil;

    if (sJsonParameters != nil) {
        NSData *jsonData = [sJsonParameters dataUsingEncoding:NSUTF8StringEncoding];
        parameters = [NSJSONSerialization JSONObjectWithData:jsonData options:0 error:&error];
    }
    if (error != nil) {
        NSString *errorMessage = [NSString stringWithFormat:@"Failed to parse json parameters: %@, (%@)", sJsonParameters.aiTrim, [error localizedDescription]];
        [AIAdjustFactory.logger error:errorMessage];
    }

    return parameters;
}

extern "C"
{
    void _AdjustLaunchApp(const char* appToken, const char* environment, const char* sdkPrefix, int logLevel, int eventBuffering) {
        NSString* sAppToken = [NSString stringWithUTF8String: appToken];
        NSString* sEnvironment = [NSString stringWithUTF8String: environment];
        NSString* sSdkPrefix = [NSString stringWithUTF8String: sdkPrefix];
        AILogLevel eLogLevel = (AILogLevel)logLevel;
        BOOL bEventBuffering = (BOOL) eventBuffering;

        NSLog(@"%@, %@, %d, %d", sAppToken, sEnvironment, eLogLevel, bEventBuffering);
        [Adjust appDidLaunch:sAppToken];
        [Adjust setEnvironment:sEnvironment];
        [Adjust setLogLevel:eLogLevel];
        [Adjust setSdkPrefix:sSdkPrefix];
    }

    void _AdjustTrackEvent(const char* eventToken, const char* cJsonParameters) {
        NSString *sEventToken = [NSString stringWithUTF8String: eventToken];
        NSDictionary * parameters = ConvertParameters(cJsonParameters);

        if (parameters == nil) {
            [Adjust trackEvent:sEventToken];
        } else {
            [Adjust trackEvent:sEventToken withParameters:parameters];
        }
    }

    void _AdjustTrackRevenue(double cents, const char* eventToken, const char* cJsonParameters) {
        NSString *sEventToken = nil;
        if (eventToken != nil) {
            sEventToken = [NSString stringWithUTF8String: eventToken];
        }

        NSDictionary * parameters = ConvertParameters(cJsonParameters);

        if (sEventToken == nil) {
            [Adjust trackRevenue:cents];
        } else if (parameters == nil) {
            [Adjust trackRevenue:cents forEvent:sEventToken];
        } else {
            [Adjust trackRevenue:cents forEvent:sEventToken withParameters:parameters];
        }
    }

    void _AdjustOnPause() {
        [Adjust trackSubsessionEnd];
    }

    void _AdjustOnResume() {
        [Adjust trackSubsessionStart];
    }

    void _AdjustSetResponseDelegate(const char* sceneName) {
        adjustSceneName = strdup(sceneName);
        adjustUnityInstance = [[AdjustUnity alloc] init];
        [Adjust setDelegate:adjustUnityInstance];
    }

    void _AdjustSetEnabled(int enabled) {
        BOOL bEnabled = (BOOL) enabled;
        [Adjust setEnabled:bEnabled];
    }

    int _AdjustIsEnabled() {
        BOOL isEnabled = [Adjust isEnabled];
        int iIsEnabled = (int) isEnabled;
        return iIsEnabled;
    }
}
