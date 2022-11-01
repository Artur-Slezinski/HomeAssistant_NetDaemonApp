﻿
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
        string notifyMessage = null;       

        services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

        if (Pm25 >= 15 || Pm10 >= 35)
        {
            services.Notify.MobileAppSmG996b
                (message: "TTS",
                data: new { tts_text = "Sąsiedzi palą śmieciami, nie wychodź z domu!" });

            notifyMessage = $"🌋 jest tragiczna";
        }

        else if (Pm25 >= 12 && Pm25 < 15 || Pm10 >= 25 && Pm10 < 35)
        {
            services.Notify.MobileAppSmG996b
                (message: "TTS",
                data: new { tts_text = "Unikaj spacerów, podwyższone stężenie pyłów zawieszonych!" });
                       
            notifyMessage = $"💨 podwyższone stężenie pyłów zawieszonych!";
        }
        else
        {
            notifyMessage = $"🌞 jest w porządku!";            
        }

        services.Notify.MobileAppSmG996b
               (title: "Jakość powietrza",
               message: notifyMessage,
               data: new { tag = "AirQualityNotification" });
    }
}
