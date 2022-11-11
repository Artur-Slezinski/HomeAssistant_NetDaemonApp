using HomeAssistantGenerated;
using NetDaemonApps.apps.HassModel.Alarm;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    public AlarmActions(IHaContext ha)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        _myEntities.BinarySensor.Hallpir
        .StateChanges().Where(e => e.New?.State == "on")
        .Subscribe(_ => AlarmArmed(_myEntities, _services));
    }

    private void AlarmArmed(Entities entities, Services services)
    {
        var hallPirSensor = entities.BinarySensor.Hallpir;

        if (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            Thread alarmSound = new Thread(() => AlarmSound(entities));
            alarmSound.Start();

            Thread flashingLights = new Thread(() => FlashingLights(entities));
            flashingLights.Start();

            SMSNotification alarm = new SMSNotification();
            Thread alarmNotification = new Thread(() => alarm.AlarmNotification(services));
            alarmNotification.Start();
        }               
    }

    private static void FlashingLights(Entities entities)
    {
        var allLights = new[] {
         entities.Light.Airqualityoutdoorledring,
         entities.Light.Led
        };

        while (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            allLights.TurnOn(transition: 0, colorName: "Gold", brightness: 255);
            System.Threading.Thread.Sleep(500);
            allLights.TurnOn(transition: 0, colorName: "Blue", brightness: 255);
            System.Threading.Thread.Sleep(500);
        }       
    }
    private static void AlarmSound(Entities entities)
    {

        var mediaPlayer = entities.MediaPlayer.VlcTelnet;

        while (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            mediaPlayer.VolumeSet(0.08);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
            System.Threading.Thread.Sleep(1000);
        }
        mediaPlayer.MediaStop();
    }       
}