//
//  AdjustUnityTestLibrary.mm
//  Adjust SDK
//
//  Created by Srdjan Tubin (@2beens) on 13th February 2018.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import "ATLTestLibrary.h"
#import "AdjustUnityTestLibrary.h"
#import "AdjustUnityCommandExecutor.h"

static ATLTestLibrary *testLibrary;
static id<AdjustCommandDelegate> commandDelegate;

@implementation AdjustUnityTestLibrary

#pragma mark - Object lifecycle methods

- (id)init {
    self = [super init];
    if (nil == self) {
        return nil;
    }
    return self;
}

@end

#pragma mark - Publicly available C methods

extern "C"
{
    void _ATLInitialize(const char* baseUrl, const char* controlUrl) {
        NSString *sBaseUrl = [NSString stringWithUTF8String:baseUrl];
        NSString *sControlUrl = [NSString stringWithUTF8String:controlUrl];
        commandDelegate = [[AdjustUnityCommandExecutor alloc] init];
        testLibrary = [ATLTestLibrary testLibraryWithBaseUrl:sBaseUrl
                                               andControlUrl:sControlUrl
                                          andCommandDelegate:commandDelegate];
    }

    void _ATLStartTestSession(const char* clientSdk) {
        NSString *sClientSdk = [NSString stringWithUTF8String:clientSdk];
        [testLibrary startTestSession:sClientSdk];
    }

    void _ATLAddInfoToSend(const char* key, const char* value) {
        NSString *sKey = [NSString stringWithUTF8String:key];
        if (value == NULL) {
            [testLibrary addInfoToSend:sKey value:nil];
        } else {
            NSString *sValue = [NSString stringWithUTF8String:value];
            [testLibrary addInfoToSend:sKey value:sValue];
        }
    }

    void _ATLSendInfoToServer(const char* basePath) {
        NSString *sBasePath = [NSString stringWithUTF8String:basePath];
        [testLibrary sendInfoToServer:sBasePath];
    }

    void _ATLAddTest(const char* testName) {
        NSString *sTestName = [NSString stringWithUTF8String:testName];
        [testLibrary addTest:sTestName];
    }

    void _ATLAddTestDirectory(const char* testDirectory) {
        NSString *sTestDirectory = [NSString stringWithUTF8String:testDirectory];
        [testLibrary addTestDirectory:sTestDirectory];
    }
}
