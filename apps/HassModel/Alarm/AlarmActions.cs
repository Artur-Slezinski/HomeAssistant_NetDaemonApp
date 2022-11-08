namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    public AlarmActions(IHaContext ha)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        //_myEntities.AlarmControlPanel.Alarm
        //    .StateChanges().Where(e => e.New?.State == "armed_away")
        //    .Subscribe(_ => AlarmSound(_myEntities), _ => FlashingLights(_myEntities));

        //_myEntities.AlarmControlPanel.Alarm
        //    .StateChanges().Where(e => e.New?.State == "armed_away")
        //    .Subscribe(_ => AlarmSound(_myEntities));

        //_myEntities.AlarmControlPanel.Alarm
        //    .StateChanges().Where(e => e.New?.State == "armed_away")
        //    .Subscribe(_ => FlashingLights(_myEntities));

        AlarmNotification(_services);
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

    private void AlarmNotification(Services services)
    {
        var telNumbers = new[] { "+48xxxxxx", "+48726xxxxxx" };
        //services.Notify.HuaweiLte(target: telNumbers, message: "Wykryto intruza!") ;
        //services.Notify.HuaweiLte(target: "++48xxxxxx", message: "Wykryto intruza!") ;
    }
}