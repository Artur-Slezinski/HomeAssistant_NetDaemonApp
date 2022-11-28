namespace Alarm;
[NetDaemonApp]

public class AlarmStateNotify
{
    private readonly Entities _entities;
    private readonly Services _services;
    private readonly ITextToSpeechService _tts;

    public AlarmStateNotify(IHaContext ha, ITextToSpeechService tts)
    {
        _entities = new Entities(ha);
        _services = new Services(ha);
        _tts = tts;

        Initialize();
    }

    private void Initialize()
    {
        _entities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away") 
            .Subscribe(_ =>
            {
                TtsAlarmArmedNotify();
                WhatsAppAlarmArmedNotify();
            });

        _entities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed") 
            .Subscribe(_ =>
            {
                TtsAlarmDisarmedNotify();
                WhatsAppAlarmDisarmedNotify();
            });
    }

    private void WhatsAppAlarmArmedNotify() 
    {
        _services.Notify.Whatsapp
            (message: "Alarm uzbrojony!");
    }

    private void WhatsAppAlarmDisarmedNotify()
    {
        _services.Notify.Whatsapp
            (message: "Alarm rozbrojony!");
    }

    private void TtsAlarmArmedNotify() 
    {
        var mediaPlayer = _entities.MediaPlayer.VlcTelnet;
        mediaPlayer.VolumeSet(0.3);
        _tts.Speak("media_player.vlc_telnet", "Alarm uzbrojony!", "google_say", "pl");
    }

    private void TtsAlarmDisarmedNotify()
    {
        var mediaPlayer = _entities.MediaPlayer.VlcTelnet;
        mediaPlayer.VolumeSet(0.3);
        _tts.Speak("media_player.vlc_telnet", "Alarm rozbrojony!", "google_say", "pl");
    }
}