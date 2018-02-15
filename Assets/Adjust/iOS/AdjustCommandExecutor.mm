//
//  ATAAdjustCommandExecutor.m
//  AdjustTestApp
//
//  Created by Pedro da Silva (@nonelse) on 23rd August 2017.
//  Copyright © 2017 Adjust GmbH. All rights reserved.
//

#import "AdjustCommandExecutor.h"

@implementation AdjustCommandExecutor

- (id)init {
    self = [super init];

    if (self == nil) {
        return nil;
    }

    return self;
}

- (void)executeCommandRawJson:(NSString *)json {
    NSLog(@"executeCommand json: %@", json);

    const char* charJsonCommand = [json UTF8String];

    UnitySendMessage("TestApp", "ExecuteCommand", charJsonCommand);
}

@end
