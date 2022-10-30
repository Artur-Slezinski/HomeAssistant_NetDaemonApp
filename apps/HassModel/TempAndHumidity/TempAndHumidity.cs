﻿//// Use unique namespaces for your apps if you going to share with others to avoid
//// conflicting names

//namespace HassModel;
//[NetDaemonApp]

//public class TempAndHumidity
//{

//    public TempAndHumidity(IHaContext ha, IScheduler scheduler)
//    {
//        var _myEntities = new Entities(ha);
//        var _services = new Services(ha);

//        scheduler.SchedulePeriodic(TimeSpan.FromSeconds(10), StartedPeriodiclyAt10Seconds);

//        void StartedPeriodiclyAt10Seconds()
//        {
//            AirQuality(_myEntities, _services);
            
//        }
//    }   

//    private void AirQuality(Entities entities, Services services)
//    {
//        var Pm25 = entities.Sensor.DomPm25.AsNumeric().State;
//        var Pm10 = entities.Sensor.DomPm10.AsNumeric().State;

//        services.Notify.MobileAppSmG996b(message: "clear_notification", data: new { tag = "AirQualityNotification" });

//        if (Pm25 >= 15 || Pm10 >= 35)
//        {
//            services.Notify.MobileAppSmG996b
//                ("TTS",
//                data: new { tts_text = "Sąsiedzi palą śmieciami, nie wychodź z domu!" });
//            services.Notify.MobileAppSmG996b
//                (title: "Jakość powietrza",
//                message: $"🌋 jest tragiczna",
//                data: new { tag = "AirQualityNotification" });
//        }

//        else if (Pm25 >= 12 && Pm25 < 15 || Pm10 >= 25 && Pm10 < 35)
//        {
//            services.Notify.MobileAppSmG996b
//                ("TTS", 
//                data: new { tts_text = "Unikaj spacerów, podwyższone stężenie pyłów zawieszonych!" });
//            services.Notify.MobileAppSmG996b
//                (title: "Jakość powietrza", message: $"💨 podwyższone stężenie pyłów zawieszonych!",
//                data: new { tag = "AirQualityNotification" });
//        }
//        else
//        {
//            services.Notify.MobileAppSmG996b
//                (title: "Jakość powietrza",
//                message: $"🌞 jest w porządku!",
//                data: new { tag = "AirQualityNotification" });
//        }
//    }
//    public void MyTtsApp(string message, Services services)
//    {
//        services.Notify.MobileAppSmG996b("TTS", data: new { tts_text = message });
//    }

//    private void upTemp(Entities entities, Services services)
//    {
//        NumericSensorEntity outdoorTemperature = entities.Sensor.Outdoortemp;
//        outdoorTemperature
//            .StateChanges()
//            .Where(e => outdoorTemperature.AsNumeric().State <= 25.0)
//            .Subscribe(_ => services.Switch.TurnOn(ServiceTarget.FromEntities("switch.outdoormcuinternalled", "switch.outdoormcuinternalled")));
//    }
//}
