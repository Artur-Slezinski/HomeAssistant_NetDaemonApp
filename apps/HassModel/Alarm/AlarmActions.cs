namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    private readonly Entities _entities;
    private readonly Services _services;
    public AlarmActions(IHaContext ha)
    {
        _entities = new Entities(ha);
        _services = new Services(ha);

        Initialize();
    }

    private void Initialize()
    {
        var motion = new[]
            {_entities.BinarySensor.Hallbathroompir,
             _entities.BinarySensor.Hallbedroompir};

        motion
        .StateChanges().Where(e => e.New?.State == "on")
        .Subscribe(_ => AlarmArmed());
    }

    private void AlarmArmed()
    {
        var alarmState = _entities.AlarmControlPanel.Alarm.State;

        if (alarmState == "armed_away")
        {
            Thread alarmSound = new Thread(() => AlarmSound());
            alarmSound.Start();

            Thread flashingLights = new Thread(() => FlashingLights());
            flashingLights.Start();

            Thread alarmNotification = new Thread(() => AlarmNotification());
            alarmNotification.Start();
        }
    }

    private void FlashingLights()
    {
        var alarmState = _entities.AlarmControlPanel.Alarm.State;
        var allLights = new[] {
         _entities.Light.Airqualityoutdoorledring,
         _entities.Light.Outdoortempled,
         _entities.Light.Hallled
        };

        while (alarmState == "armed_away")
        {
            allLights.TurnOn(transition: 0, colorName: "Gold", brightness: 255);
            Thread.Sleep(500);
            allLights.TurnOn(transition: 0, colorName: "Blue", brightness: 255);
            Thread.Sleep(500);
            alarmState = _entities.AlarmControlPanel.Alarm.State;
        }
    }
    private void AlarmSound()
    {
        var alarmState = _entities.AlarmControlPanel.Alarm.State;
        var mediaPlayer = _entities.MediaPlayer.VlcTelnet;

        while (alarmState == "armed_away")
        {
            mediaPlayer.VolumeSet(0.2);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
            Thread.Sleep(1000);
            alarmState = _entities.AlarmControlPanel.Alarm.State;
        }
        mediaPlayer.MediaStop();
    }

    public void AlarmNotification()
    {
        var numbers = SMSNotification.telNumbers;
        //_services.Notify.HuaweiLte(target: numbers, message: "Wykryto intruza!");
    }

}