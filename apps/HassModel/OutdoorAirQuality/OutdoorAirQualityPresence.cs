using System.Reactive.Concurrency;

namespace AirQuality;
[NetDaemonApp]

public class OutdoorAirQualityPresence
{
    private readonly Entities _entities;
    private readonly IScheduler _scheduler;
    public OutdoorAirQualityPresence(IHaContext ha, IScheduler scheduler)
    {        
        _entities = new Entities(ha);        
        _scheduler= scheduler;
        
        Initialize();
    }
    private void Initialize() 
    {
        _scheduler.SchedulePeriodic(TimeSpan.FromSeconds(30), () => AirQualityRingColour());

        AirQualityRingColour();

        _entities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => AirQualityRingColour());
    }
    private void AirQualityRingColour()
    {

        var Pm25 = _entities.Sensor.DomPm25.AsNumeric().State;
        var Pm10 = _entities.Sensor.DomPm10.AsNumeric().State;
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

        OutdoorAirQualityRing(color);
    }
    private void OutdoorAirQualityRing(string color)
    {
        var ring = _entities.Light.Airqualityoutdoorledring;
        var sun = _entities.Sun.Sun.State;
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
