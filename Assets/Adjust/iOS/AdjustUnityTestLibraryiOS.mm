#import "AdjustUnityTestLibraryiOS.h"

@implementation AdjustUnityTestLibraryiOS

- (id)init {
    self = [super init];
    return self;
}

@end

extern "C"
{
	void _ATLStartTestSession(const char* clientSdk) {
        NSString *stringClientSdk = [NSString stringWithUTF8String:clientSdk];

        [ATLTestLibrary startTestSession:stringClientSdk];
    }

	void _ATLAddInfoToSend(const char* key, const char* paramValue) {
		NSString *stringKey = [NSString stringWithUTF8String:key];
		NSString *stringValue = [NSString stringWithUTF8String:paramValue];

		[ATLTestLibrary addInfoToSend:stringKey value:stringValue];
	}

	void _ATLSendInfoToServer(const char* basePath) {
		NSString *stringBasePath = [NSString stringWithUTF8String:basePath];

		[ATLTestLibrary sendInfoToServer:stringBasePath];
	}
}