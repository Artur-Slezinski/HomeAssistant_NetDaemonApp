// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names

namespace HassModel;
[NetDaemonApp]

public class TempAndHumidity
{

    public TempAndHumidity(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(10), StartedPeriodiclyAt10Seconds);

        void StartedPeriodiclyAt10Seconds()
        {
            AirQuality(_myEntities, _services);
            TempRingColour(_myEntities, _services);
        }
    }

    private void TempRingColour(Entities entities, Services services)
    {
        string color = null;
        var temp = entities.Sensor.Outdoortemp.AsNumeric().State;
        if (temp <= -15)
        {
            color = "darkslateblue";
            TempRing(entities, services, color);
        }
        else if (temp > -15 && temp <= -5)
        {
            color = "blue";
            TempRing(entities, services, color);
        }
        else if (temp > -5 && temp <= 5)
        {
            color = "aqua";
            TempRing(entities, services, color);
        }
        else if (temp > 5 && temp <= 15)
        {
            color = "greenyellow";
            TempRing(entities, services, color);
        }
        else if (temp > 15 && temp <= 25)
        {
            color = "green";
            TempRing(entities, services, color);
        }
        else if (temp > 25 && temp <= 35)
        {
            color = "orange";
            TempRing(entities, services, color);
        }
        else if (temp > 35)
        {
            color = "red";
            TempRing(entities, services, color);
        }
    }

    private void TempRing(Entities entities, Services services, string color)
    {
        var _myEntities = entities;
        var ring = _myEntities.Light.Led;
        ring.TurnOn(brightness: 100, colorName: color);
    }

    private void AirQuality(Entities entities, Services services)
    {
        services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

        if (entities.Sensor.DomPm25.AsNumeric().State >= 15 && entities.Sensor.DomPm10.AsNumeric().State >= 35)
        {
            services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = "Sąsiedzi palą śmieciami, nie wychodź z domu!" });
            services.Notify.MobileAppSmG996b(title: "Jakość powietrza", message: $"🌋 jest tragiczna", data: new { tag = "AirQualityNotification" });
        }

        else if (entities.Sensor.DomPm25.AsNumeric().State >= 10 && entities.Sensor.DomPm25.AsNumeric().State < 15
            && entities.Sensor.DomPm10.AsNumeric().State >= 25 && entities.Sensor.DomPm10.AsNumeric().State < 35)
        {
            services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = "Unikaj spacerów, podwyższone stężenie pyłów zawieszonych!" });
            services.Notify.MobileAppSmG996b(title: "Jakość powietrza", message: $"💨 podwyższone stężenie pyłów zawieszonych!", data: new { tag = "AirQualityNotification" });
        }
    }
    public void MyTtsApp(string message, Services services)
    {
        services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = message });
    }

    private void upTemp(Entities entities, Services services)
    {
        NumericSensorEntity outdoorTemperature = entities.Sensor.Outdoortemp;
        outdoorTemperature
            .StateChanges()
            .Where(e => outdoorTemperature.AsNumeric().State <= 25.0)
            .Subscribe(_ => services.Switch.TurnOn(ServiceTarget.FromEntities("switch.outdoormcuinternalled", "switch.outdoormcuinternalled")));
    }

    private void downTemp(Entities entities, Services services)
    {
        NumericSensorEntity outdoorTemperature = entities.Sensor.Outdoortemp;
        outdoorTemperature
            .StateChanges()
            .Where(e => outdoorTemperature.AsNumeric().State > 25.0)
            .Subscribe(_ => services.Switch.TurnOff(ServiceTarget.FromEntities("switch.outdoormcuinternalled", "switch.outdoormcuinternalled")));
    }


    private void LED(Entities entities, Services services)
    {
        if (entities.Switch.Outdoormcuinternalled.State == "on")
        {
            services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = "TEST" });
        }
    }

    public void InformTemp(Services services)
    {
        services.Switch.TurnOn(ServiceTarget.FromEntities("switch.outdoormcuinternalled", "switch.outdoormcuinternalled"));
    }
}
