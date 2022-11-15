namespace AirQuality;
[NetDaemonApp]

public class OutdoorAirQualityPresence
{    
    public OutdoorAirQualityPresence(IHaContext ha, IScheduler scheduler)
    {
        
        var _myEntities = new Entities(ha);        

        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(30), () => AirQualityRingColour(_myEntities));

        AirQualityRingColour(_myEntities);
        
        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => AirQualityRingColour(_myEntities));
    }
    private void AirQualityRingColour(Entities entities)
    {

        var Pm25 = entities.Sensor.DomPm25.AsNumeric().State;
        var Pm10 = entities.Sensor.DomPm10.AsNumeric().State;
        string color = null;

        if (Pm25 >= 15 || Pm10 >= 35)
        {
            color = "red";
        }

        else if (Pm25 >= 12 && Pm25 < 15 || Pm10 >= 25 && Pm10 < 35)
        {
            color = "orange";
        }
        else
        {
            color = "green";
        }

        OutdoorAirQualityRing(entities, color);
    }
    private void OutdoorAirQualityRing(Entities entities, string color)
    {
        var ring = entities.Light.Airqualityoutdoorledring;
        var sun = entities.Sun.Sun.State;
        if (sun == "above_horizon")
        {
            ring.TurnOn(brightness: 255, colorName: color);
        }
        else
        {
            ring.TurnOn(brightness: 120, colorName: color);
        }
    }
}
