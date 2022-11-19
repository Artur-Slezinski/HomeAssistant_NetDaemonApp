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
        if (_entities.AlarmControlPanel.Alarm.State == "armed_away")
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
        var allLights = new[] {
         _entities.Light.Airqualityoutdoorledring,
         _entities.Light.Led
        };

        while (_entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            allLights.TurnOn(transition: 0, colorName: "Gold", brightness: 255);
            Thread.Sleep(500);
            allLights.TurnOn(transition: 0, colorName: "Blue", brightness: 255);
            Thread.Sleep(500);
        }
    }
    private void AlarmSound()
    {

        var mediaPlayer = _entities.MediaPlayer.VlcTelnet;

        while (_entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            mediaPlayer.VolumeSet(0.2);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
            Thread.Sleep(1000);
        }
        mediaPlayer.MediaStop();
    }

    public void AlarmNotification()
    {
        var numbers = SMSNotification.telNumbers;        
        //_services.Notify.HuaweiLte(target: numbers, message: "Wykryto intruza!") ;
        //_services.Notify.HuaweiLte(target: "++48xxxxxx", message: "Wykryto intruza!") ;
    }

}