
using LivingRoomStatePresence;
using NotifyService;

namespace Tests;

public class LivingRoomStatePresenceTests : RxAppMock
{
    [Fact]
    public async Task WhenTempIsBelowMinus15()
    {
        // 1. Instance the app, preferable you make new class for implementation
        //    and initialize it       
       // var app = new LivingRoomStatePresenceAPP(Object);
        
        


        // 3. Trigger a change event to simulate update in state
        TriggerStateChange("Light.Led", "off", "on");

        // 3. Use the built-in verify functions to verify actions
        VerifyEntityTurnOn("Light.Led");
        
    }
   
}
