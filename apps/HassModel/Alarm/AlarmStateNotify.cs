namespace Alarm;
[NetDaemonApp]

public class AlarmStateNotify
{
    public AlarmStateNotify(IHaContext ha, ITextToSpeechService tts)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => AlarmArmed(_myEntities, _services, tts));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => AlarmDisarmed(_myEntities, _services, tts));
    }

    private void AlarmArmed(Entities entities, Services services, ITextToSpeechService tts)
    {
        TtsAlarmArmedNotify(entities, tts);
        WhatsAppAlarmArmedNotify(services, entities);
    }

    private void AlarmDisarmed(Entities entities, Services services, ITextToSpeechService tts)
    {
        TtsAlarmDisarmedNotify(entities, tts);
        WhatsAppAlarmDisarmedNotify(services, entities);
    }

    private void WhatsAppAlarmArmedNotify(Services services, Entities entities)
    {
        services.Notify.Whatsapp
            (message: "Alarm uzbrojony!");
    }

    private void WhatsAppAlarmDisarmedNotify(Services services, Entities entities)
    {
        services.Notify.Whatsapp
            (message: "Alarm rozbrojony!");
    }

    private void TtsAlarmArmedNotify(Entities entities, ITextToSpeechService tts)
    {
        var mediaPlayer = entities.MediaPlayer.VlcTelnet;
        mediaPlayer.VolumeSet(0.3);
        mediaPlayer.VolumeSet(0);
        tts.Speak("media_player.vlc_telnet", "Alarm uzbrojony!", "google_say", "pl");
    }

    private void TtsAlarmDisarmedNotify(Entities entities, ITextToSpeechService tts)
    {
        var mediaPlayer = entities.MediaPlayer.VlcTelnet;
        mediaPlayer.VolumeSet(0.3);
        mediaPlayer.VolumeSet(0);
        tts.Speak("media_player.vlc_telnet", "Alarm rozbrojony!", "google_say", "pl");
    }
}