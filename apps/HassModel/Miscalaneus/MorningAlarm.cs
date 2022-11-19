namespace Miscalaneus;
[NetDaemonApp]

public class MorningAlarm
{
    private readonly Entities _entities;
    private readonly IScheduler _scheduler;

    private string alarmTime = "";

    public MorningAlarm(IHaContext ha, IScheduler scheduler)
    {
        _entities = new Entities(ha);
        _scheduler = scheduler;

        Initialize();
    }

    private void Initialize()
    {
        _entities.Sensor.SmG996bNextAlarm
            .StateChanges()
            .Subscribe(_ => MornigMusic());
    }

    private void MornigMusic()
    {
        var mediaPlayer = _entities.MediaPlayer.VlcTelnet;
        var alarmState = _entities.Sensor.SmG996bNextAlarm.State.Remove(16, 9);
        var date = DateTime.UtcNow.AddMinutes(5);
        var isoDate = date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

        if (alarmTime == isoDate)
        {
            mediaPlayer.VolumeSet(0.6);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "https://stream.open.fm/21");
            _scheduler.Schedule(TimeSpan.FromMinutes(15), mediaPlayer.MediaStop);
        }

        alarmTime = alarmState;
    }
}
