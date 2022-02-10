//
//  ISMintegralAdapter.h
//  ISMintegralAdapter
//
//  Created by Guy Lis on 04/07/2019.
//

#import <Foundation/Foundation.h>
#import "IronSource/ISBaseAdapter+Internal.h"

static NSString * const MintegralAdapterVersion = @"4.1.4";
static NSString * GitHash = @"b69e35f6c";

//System Frameworks For Mintegral Adapter

@import AdSupport;
@import AVFoundation;
@import CoreGraphics;
@import CoreTelephony;
@import MobileCoreServices;
@import QuartzCore;
@import StoreKit;
@import UIKit;
@import WebKit;

@interface ISMintegralAdapter : ISBaseAdapter

@end
