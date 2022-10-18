// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names


using HomeAssistantGenerated;
using NetDaemon.HassModel;
using Serilog;


namespace HassModel;
[NetDaemonApp]

public class TempAndHumidity
{

    private readonly ILogger<TempAndHumidity> _log;
    private readonly Entities _myEntities;
    private readonly Services _services;

    public TempAndHumidity(IHaContext ha)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        //ha.Entity("switch.nodemcu_internal_led")
        //    .StateChanges().Where(e => e.New?.State == "off")
        //    .Subscribe(_ => ha.CallService("notify", "persistent_notification", data: new { message = "LED", title = "ON" }));

        upTemp(_myEntities, _services);
        downTemp(_myEntities, _services);
        // InformTemp(_services);
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

    public void InformTemp(Services services)
    {
        services.Switch.TurnOn(ServiceTarget.FromEntities("switch.outdoormcuinternalled", "switch.outdoormcuinternalled"));
    }
}
