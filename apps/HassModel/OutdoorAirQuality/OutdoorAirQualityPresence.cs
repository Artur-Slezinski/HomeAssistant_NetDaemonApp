namespace NetDaemonApps.apps.HassModel.AirQuality;
[NetDaemonApp]

public class OutdoorAirQualityPresence
{
    public OutdoorAirQualityPresence(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        //scheduler.SchedulePeriodic(TimeSpan.FromSeconds(30), () => AirQualityRingColour(_myEntities, _services));

        //AirQualityRingColour(_myEntities, _services);
    }
    private void AirQualityRingColour(Entities entities, Services services)
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

        OutdoorAirQualityRing(entities, services, color);
    }
    private void OutdoorAirQualityRing(Entities entities, Services services, string color)
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
