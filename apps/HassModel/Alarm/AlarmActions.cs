
using HomeAssistantGenerated;
using System.Configuration;

namespace Alarm;
[NetDaemonApp]
public class AlarmActions
{
    public AlarmActions(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        //_myEntities.AlarmControlPanel.Alarm
        //    .StateChanges().Where(e => e.New?.State == "armed_away")
        //    .Subscribe(_ => AlarmSound(_myEntities), _ => FlashingLights(_myEntities));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => AlarmSound(_myEntities));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => FlashingLights(_myEntities));

        //AlarmSound(_myEntities, _services);
    }

    private void FlashingLights(Entities entities)
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
    private void AlarmSound(Entities entities)
    {
        var MBS = entities.MediaPlayer.VlcTelnet;

        while (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            MBS.VolumeSet(0.2);
            MBS.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
            System.Threading.Thread.Sleep(5000);
        }
        MBS.MediaStop();
    }
}





