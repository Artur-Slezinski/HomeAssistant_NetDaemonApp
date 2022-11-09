namespace NotifyService;
[NetDaemonApp]

public class OutdoorAirQualityNotify
{
    public OutdoorAirQualityNotify(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);
        
        scheduler.SchedulePeriodic(TimeSpan.FromHours(1), () => AirQuality(_myEntities, _services));        

        AirQuality(_myEntities, _services);
    }

    private void AirQuality(Entities entities, Services services)
    {
        var Pm25 = entities.Sensor.DomPm25.AsNumeric().State;
        var Pm10 = entities.Sensor.DomPm10.AsNumeric().State;
        string notifyTitle = "Jakość powietrza";
        string notifyMessage = null;
        string ttsMessage = "TTS";
        string ttsText = null;

        services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

        if (Pm25 >= 15 || Pm10 >= 35)
        {
            ttsText = "Jakość powietrza jest skrajnie zła, nie wychodź z domu!";

            notifyMessage = $"🌋 jest skrajnie zła!";

            VoiceNotify(services, ttsMessage, ttsText);
        }

        else if (Pm25 >= 12 && Pm25 < 15 || Pm10 >= 25 && Pm10 < 35)
        {
            ttsText = "Unikaj spacerów, podwyższone stężenie pyłów zawieszonych!";
                       
            notifyMessage = $"💨 podwyższone stężenie pyłów zawieszonych!";

            VoiceNotify(services, ttsMessage, ttsText);
        }
        else
        {
            notifyMessage = $"🌞 jest w porządku!";            
        }        

        TextNotify(services, notifyTitle, notifyMessage);
    }

    private void VoiceNotify(Services services, string message, string text)
    {
        services.Notify.MobileAppSmG996b
                (message: message,
                data: new { tts_text = text });
    }

    private void TextNotify(Services services, string title, string message)
    {
        services.Notify.MobileAppSmG996b
               (title: title,
               message: message,
               data: new { tag = "AirQualityNotification" });
    }
}

