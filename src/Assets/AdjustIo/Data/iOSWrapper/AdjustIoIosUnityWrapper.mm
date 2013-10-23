#import "AdjustIo.h"

extern "C"
{
	void AdjustIoInit(const char* appToken){
		[AdjustIo appDidLaunch:[NSString stringWithUTF8String:appToken]];
	}
	
	void AdjustIoSetLogLevel(int logLevel){
		if(logLevel == 0){
			[AdjustIo setLogLevel:AILogLevelVerbose]; // enable all logging
		}else if(logLevel == 1){
			[AdjustIo setLogLevel:AILogLevelDebug];   // enable more logging
		}else if(logLevel == 2){
			[AdjustIo setLogLevel:AILogLevelInfo];    // the default
		}else if(logLevel == 3){
			[AdjustIo setLogLevel:AILogLevelWarn];    // disable info logging
		}else if(logLevel == 4){
			[AdjustIo setLogLevel:AILogLevelError];   // disable warnings as well
		}else if(logLevel == 5){
			[AdjustIo setLogLevel:AILogLevelAssert];  // disable errors as well
		}
	}
	
	void AdjustIoSetEnvironment(int environment){
		if(environment == 0){
			[AdjustIo setEnvironment:AIEnvironmentSandbox];
		}else if(environment == 1){
			[AdjustIo setEnvironment:AIEnvironmentProduction];
		}
	}
	
	void AdjustIoTrackEvent(const char* eventToken){
		[AdjustIo trackEvent:[NSString stringWithUTF8String:eventToken]];
	}
	
	NSMutableDictionary* parameters = [NSMutableDictionary dictionary];
	
	void AdjustIoClearParameters(){
		[parameters removeAllObjects];
	}
	
	void AdjustIoAddParameter(const char* key, const char* value){
		[parameters setObject:[NSString stringWithUTF8String:value] forKey:[NSString stringWithUTF8String:key]];
	}
	
	void AdjustIoTrackEventWithParameters(const char* eventToken){
		[AdjustIo trackEvent:[NSString stringWithUTF8String:eventToken] withParameters:parameters];
	}
	
	void AdjustIoTrackRevenue(double cents){
		[AdjustIo trackRevenue:cents];
	}
	
	void AdjustIoTrackRevenueForEvent(double cents, const char* eventToken){
		[AdjustIo trackRevenue:cents forEvent:[NSString stringWithUTF8String:eventToken]];
	}
	
	void AdjustIoTrackRevenueForEventWithParameters(double cents, const char* eventToken){
		[AdjustIo trackRevenue:cents forEvent:[NSString stringWithUTF8String:eventToken] withParameters:parameters];
	}
	
	void AdjustIoSetEventBufferingEnabled(bool enabled){
		if(enabled){
			[AdjustIo setEventBufferingEnabled:YES];
		}else{
			[AdjustIo setEventBufferingEnabled:NO];
		}
	}
}