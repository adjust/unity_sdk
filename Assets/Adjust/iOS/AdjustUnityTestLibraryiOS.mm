#import "AdjustUnityTestLibraryiOS.h"
#import "ATLTestLibrary.h"
#import "AdjustCommandExecutor.h"

static ATLTestLibrary *testLibrary;
static id<AdjustCommandDelegate> commandDelegate;

@implementation AdjustUnityTestLibraryiOS

- (id)init {
    self = [super init];
    return self;
}

@end

extern "C"
{
	void _ATLInitialize(const char* baseUrl) {
		NSString *stringBaseUrl = [NSString stringWithUTF8String:baseUrl];

		commandDelegate = [[AdjustCommandExecutor alloc] init];
		testLibrary = [ATLTestLibrary testLibraryWithBaseUrl:stringBaseUrl
								          andCommandDelegate:commandDelegate];
	}

	void _ATLStartTestSession(const char* clientSdk) {
        NSString *stringClientSdk = [NSString stringWithUTF8String:clientSdk];

        [testLibrary startTestSession:stringClientSdk];
    }

	void _ATLAddInfoToSend(const char* key, const char* paramValue) {
		NSString *stringKey = [NSString stringWithUTF8String:key];

		if (paramValue == NULL) {
	        [testLibrary addInfoToSend:stringKey value:nil];
	    } else {
			NSString *stringValue = [NSString stringWithUTF8String:paramValue];
			[testLibrary addInfoToSend:stringKey value:stringValue];
		}
	}

	void _ATLSendInfoToServer(const char* basePath) {
		NSString *stringBasePath = [NSString stringWithUTF8String:basePath];

		[testLibrary sendInfoToServer:stringBasePath];
	}

	void _ATLAddTest(const char* testName) {
		NSString *stringTestName = [NSString stringWithUTF8String:testName];

		[testLibrary addTest:stringTestName];	
	}

	void _ATLAddTestDirectory(const char* testDirectory) {
		NSString *stringTestDirectory = [NSString stringWithUTF8String:testDirectory];
		
		[testLibrary addTestDirectory:stringTestDirectory];	
	}
}
