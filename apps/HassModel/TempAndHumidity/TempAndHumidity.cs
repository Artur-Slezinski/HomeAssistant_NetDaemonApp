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
        var _scheduler = scheduler;

        _scheduler.Schedule(TimeSpan.FromSeconds(5), XLED);

        void XLED()
        {
            LED(_myEntities, _services);
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
