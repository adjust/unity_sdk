//
//  AdjustUnityCommandExecutor.mm
//  Adjust SDK
//
//  Created by Srdjan Tubin (@2beens) on 13th February 2018.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

#import "AdjustUnityCommandExecutor.h"

@implementation AdjustUnityCommandExecutor

- (id)init {
    self = [super init];
    if (self == nil) {
        return nil;
    }
    return self;
}

- (void)executeCommandRawJson:(NSString *)json {
    NSLog(@"executeCommandRawJson: %@", json);
    const char* cJsonCommand = [json UTF8String];
    UnitySendMessage("TestApp", "ExecuteCommand", cJsonCommand);
}

@end
