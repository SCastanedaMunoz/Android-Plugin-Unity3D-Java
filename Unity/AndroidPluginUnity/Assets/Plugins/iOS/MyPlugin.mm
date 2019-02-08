#import <Foundation/Foundation.h>

@inteface MyPlugin: NSObject
{

}
@end

@implementation MyPlugin

static MyPlugin *_sharedInstance;

+(MyPlugin*) sharedInstance
{
	static dispatch_once_t onceToken;
	dispatch_once(&onceToken, ^{
		NSLog(@"Creating MyPlugin shared instance");
		_sharedInstance = [[MyPlugin alloc] init];
	});

	return _sharedInstance;
}

-(id)init
{
	self = [super init];
	if(self)
		[self initHelper];
	return self;
}

-(void)initHelper
{
	NSLog(@"initHelper called");
}
@end