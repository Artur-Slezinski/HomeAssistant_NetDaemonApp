using HomeAssistantGenerated;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reactive.Concurrency;

namespace LivingRoom;
[NetDaemonApp]

public class LivingRoomStatePresence
{
    private readonly Entities _entities;
    private readonly IScheduler _scheduler;

    public LivingRoomStatePresence(IHaContext ha, IScheduler scheduler)
    {
        _entities = new Entities(ha);
        _scheduler = scheduler;

        Initialize();
    }

    private void Initialize()
    {
        TempRingColour();

        _scheduler.SchedulePeriodic(TimeSpan.FromSeconds(120), () => TempRingColour());
        
        _entities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Delay(TimeSpan.FromSeconds(1))
            .Subscribe(_ => TempRingColour());
    }

    public void TempRingColour()
    {
        string color = null;
        var temp = _entities.Sensor.Outdoortemp.AsNumeric().State;

        if (temp <= -15)
        {
            color = "darkslateblue";
        }
        else if (temp > -15 && temp <= -5)
        {
            color = "blue";
        }
        else if (temp > -5 && temp <= 5)
        {
            color = "aqua";
        }
        else if (temp > 5 && temp <= 15)
        {
            color = "greenyellow";
        }
        else if (temp > 15 && temp <= 25)
        {
            color = "green";
        }
        else if (temp > 25 && temp <= 35)
        {
            color = "orange";
        }
        else if (temp > 35)
        {
            color = "red";
        }
        TempRing(color);
    }
    private void TempRing(string color)
    {
        var ring = _entities.Light.Outdoortempled;
        var sun = _entities.Sun.Sun.State;

        if (sun == "above_horizon")
        {
            ring.TurnOn(brightness: 150, colorName: color);
        }
        else
        {
            ring.TurnOn(brightness: 50, colorName: color);
        }
    }
}
