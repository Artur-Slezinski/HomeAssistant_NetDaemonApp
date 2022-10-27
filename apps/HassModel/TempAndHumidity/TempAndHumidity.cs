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
        
        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(10), XLED);

        void XLED()
        {
            AirQuality(_myEntities, _services);
        }
    }

    private void AirQuality(Entities entities, Services services)
    {
        services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

        if (entities.Sensor.DomPm25.AsNumeric().State >= 15 || entities.Sensor.DomPm10.AsNumeric().State >= 35)
        {
            services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = "Sąsiedzi palą śmieciami, nie wychodź z domu!" });
            services.Notify.MobileAppSmG996b(title: "Jakość powietrza", message: $"🌋 jest tragiczna", data: new { tag = "AirQualityNotification" });
        }

        else if (entities.Sensor.DomPm25.AsNumeric().State >= 10 && entities.Sensor.DomPm25.AsNumeric().State < 15
            || entities.Sensor.DomPm10.AsNumeric().State >= 25 && entities.Sensor.DomPm10.AsNumeric().State < 35)
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
