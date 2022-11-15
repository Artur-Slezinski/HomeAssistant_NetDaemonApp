namespace LivingRoom;
[NetDaemonApp]

public class LivingRoomStatePresence
{
    private readonly Entities _entities;

    public LivingRoomStatePresence(IHaContext ha, IScheduler scheduler)
    {
        _entities = new Entities(ha);

        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(120), () => TempRingColour());

        TempRingColour();

        _entities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
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
        var ring = _entities.Light.Led;
        var sun = _entities.Sun.Sun.State;

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
