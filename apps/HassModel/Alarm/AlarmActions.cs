
namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    public AlarmActions(IHaContext ha, IScheduler scheduler)
    {

    }

    private void FlashingLights(Entities entities, IScheduler scheduler)
    {
        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(2), () => FlashingLights(entities, scheduler));
        entities.Light.Led.Toggle(colorName: "Red", brightness: 255);
    }
}





