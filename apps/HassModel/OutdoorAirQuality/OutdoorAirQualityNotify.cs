using System.Reactive.Concurrency;

namespace NotifyService;
[NetDaemonApp]
public class OutdoorAirQualityNotify
{
    private readonly Entities _entities;
    private readonly Services _services;
    private readonly IScheduler _scheduler;

    public OutdoorAirQualityNotify(IHaContext ha, IScheduler scheduler)
    {
        _entities = new Entities(ha);
        _services = new Services(ha);
        _scheduler = scheduler;

        Initialize();
    }

    private void Initialize()
    {
        AirQuality();

        _scheduler.SchedulePeriodic(TimeSpan.FromHours(1), () => AirQuality());
    }

    private void AirQuality()
    {
        var Pm25 = _entities.Sensor.DomPm25.AsNumeric().State;
        var Pm10 = _entities.Sensor.DomPm10.AsNumeric().State;
        string notifyTitle = "Jakość powietrza";
        string notifyMessage = null;
        string ttsMessage = "TTS";
        string ttsText = null;

        _services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

        if (Pm25 >= 15 || Pm10 >= 35)
        {
            ttsText = "Jakość powietrza jest skrajnie zła, nie wychodź z domu!";

            notifyMessage = $"🌋 jest skrajnie zła!";

            VoiceNotify(ttsMessage, ttsText);
        }

        else if (Pm25 >= 12 && Pm25 < 15 || Pm10 >= 25 && Pm10 < 35)
        {
            ttsText = "Unikaj spacerów, podwyższone stężenie pyłów zawieszonych!";

            notifyMessage = $"💨 podwyższone stężenie pyłów zawieszonych!";

            VoiceNotify(ttsMessage, ttsText);
        }
        else
        {
            notifyMessage = $"🌞 jest w porządku!";
        }

        TextNotify(notifyTitle, notifyMessage);
    }

    private void VoiceNotify(string message, string text)
    {
        _services.Notify.MobileAppSmG996b
                (message: message,
                data: new { tts_text = text });
    }

    private void TextNotify(string title, string message)
    {
        _services.Notify.MobileAppSmG996b
               (title: title,
               message: message,
               data: new { tag = "AirQualityNotification" });
    }
}

