//
//  iOSBridge.h
//  iOSBridge
//
//  Created by Supersonic.
//  Copyright (c) 2015 Supersonic. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/IronSource.h>
static NSString *  UnityGitHash = @"df964f3";

@interface iOSBridge : NSObject<ISRewardedVideoDelegate,ISDemandOnlyRewardedVideoDelegate, ISInterstitialDelegate,ISDemandOnlyInterstitialDelegate, ISOfferwallDelegate, ISBannerDelegate, ISSegmentDelegate,ISImpressionDataDelegate, ISConsentViewDelegate>

@end


