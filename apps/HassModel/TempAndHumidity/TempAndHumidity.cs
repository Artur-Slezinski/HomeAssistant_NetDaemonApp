// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names

namespace HassModel;
[NetDaemonApp]

public class TempAndHumidity
{

    private readonly ILogger<TempAndHumidity> _log;
    private readonly Entities _myEntities;
    private readonly Services _services;
    public readonly ITextToSpeechService tts;

    public TempAndHumidity(IHaContext ha, ITextToSpeechService tts )
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha); 
        var _tts = tts;


        //ha.Entity("switch.nodemcu_internal_led")
        //    .StateChanges().Where(e => e.New?.State == "off")
        //    .Subscribe(_ => ha.CallService("notify", "persistent_notification", data: new { message = "LED", title = "ON" }));

        //upTemp(_myEntities, _services);
        //downTemp(_myEntities, _services);

        // InformTemp(_services);         
        
       
        LED(_myEntities, _services, _tts);
    }

    public void MyTtsApp(string message, Services services)
    {
        
        services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = message });
        // This uses the google service you may use some other like google cloud version, google_cloud_say
        //tts.Speak("SmG996b", "Hello this is first queued message", "notify.mobile_app_SmG996b", "pl");

        //tts.Speak("Sm_G996b", "Hello this is first queued message", "notify.mobile", "pl");
        //tts.Speak("NotifyMobileAppSmG996bParameters", "test", "google_say");
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


    private void LED(Entities entities, Services services, ITextToSpeechService tts)
    {
        string message = "test";
        SwitchEntity led = entities.Switch.Outdoormcuinternalled;
        led
            .StateChanges()
            .Where(e => e.New?.State == "off")
            .Subscribe(_ => services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = message }));

       

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
