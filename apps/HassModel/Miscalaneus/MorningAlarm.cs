namespace Miscalaneus;
[NetDaemonApp]

public class MorningAlarm
{    
    string alarmTime ="";      

    public MorningAlarm(IHaContext ha, IScheduler scheduler)
    {
        var _entities = new Entities(ha);
        var _scheduler = scheduler;

        _entities.Sensor.SmG996bNextAlarm
            .StateChanges()
            .Subscribe(_ => MornigMusic(_entities, _scheduler));
    }

    private void MornigMusic(Entities entities, IScheduler scheduler)
    {
        var mediaPlayer = entities.MediaPlayer.VlcTelnet;
        var alarmState = entities.Sensor.SmG996bNextAlarm.State.Remove(16, 9);        
        var date = DateTime.UtcNow.AddMinutes(5);              
        var isoDate = date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");        

        if (alarmTime == isoDate)
        {
            mediaPlayer.VolumeSet(0.6);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "https://stream.open.fm/21");
            scheduler.Schedule(TimeSpan.FromMinutes(15), mediaPlayer.MediaStop);
        }

        alarmTime = alarmState;        
    }
}
