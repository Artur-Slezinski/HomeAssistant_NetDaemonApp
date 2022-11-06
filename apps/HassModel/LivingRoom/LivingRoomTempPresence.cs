namespace LivingRoom;
[NetDaemonApp]

public class LivingRoomStatePresence
{
    public  LivingRoomStatePresence(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        var schedule = scheduler.SchedulePeriodic(TimeSpan.FromSeconds(600), () => TempRingColour(_myEntities));

        TempRingColour(_myEntities);

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => TempRingColour(_myEntities));
    }


    public void TempRingColour(Entities entities)
    {
        string color = null;
        var temp = entities.Sensor.Outdoortemp.AsNumeric().State;

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
        TempRing(entities, color);
    }
    private void TempRing(Entities entities, string color)
    {
        var ring = entities.Light.Led;
        var sun = entities.Sun.Sun.State;

        if (sun == "above_horizon")
        {
            ring.TurnOn(brightness: 150, colorName: color);
        }
        else
        {
            ring.TurnOn(brightness: 70, colorName: color);            
        }
    }
}
