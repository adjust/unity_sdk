//
//  AdjustUnity.h
//  Adjust SDK
//
//  Created by Pedro Silva (@nonelse) on 27th March 2014.
//  Copyright © 2012-2018 Adjust GmbH. All rights reserved.
//

/**
 * @brief The main interface to Adjust Unity bridge.
 */
@interface AdjustUnity : NSObject

// app callbacks as method parameters
typedef void (*AdjustDelegateIsEnabledGetter)(bool isEnabled);
typedef void (*AdjustDelegateAttributionGetter)(const char* attribution);
typedef void (*AdjustDelegateAdidGetter)(const char* adid);
typedef void (*AdjustDelegateIdfaGetter)(const char* idfa);
typedef void (*AdjustDelegateIdfvGetter)(const char* idfv);
typedef void (*AdjustDelegateLastDeeplinkGetter)(const char* lastDeeplink);
typedef void (*AdjustDelegateSdkVersionGetter)(const char* sdkVersion);
typedef void (*AdjustDelegateAttCallback)(int status);
typedef void (*AdjustDelegatePurchaseVerificationCallback)(const char* verificationResult);
typedef void (*AdjustDelegateVerifyAndTrackCallback)(const char* verificationResult);
typedef void (*AdjustDelegateResolvedDeeplinkCallback)(const char* deeplink);
typedef void (*AdjustDelegateSkanErrorCallback)(const char* error);

// app callbacks as subscriptions
typedef void (*AdjustDelegateAttributionCallback)(const char* attribution);
typedef void (*AdjustDelegateSessionSuccessCallback)(const char* sessionSuccess);
typedef void (*AdjustDelegateSessionFailureCallback)(const char* sessionFailure);
typedef void (*AdjustDelegateEventSuccessCallback)(const char* eventSuccess);
typedef void (*AdjustDelegateEventFailureCallback)(const char* eventFailure);
typedef void (*AdjustDelegateDeferredDeeplinkCallback)(const char* deeplink);
typedef void (*AdjustDelegateSkanUpdatedCallback)(const char* skanData);

@end
