using HomeAssistantGenerated;

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
            .Subscribe(_ => TtsAlarmArmedNotify(_myEntities, tts), _ => WhatsAppAlarmArmedNotify(_services, _myEntities));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => TtsAlarmDisarmedNotify(_myEntities, tts), _ => WhatsAppAlarmDisarmedNotify(_services, _myEntities));
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
        //mediaPlayer.VolumeSet(0.3);
        mediaPlayer.VolumeSet(0);
        tts.Speak("media_player.vlc_telnet", "Alarm uzbrojony!", "google_say", "pl");
    }

    private void TtsAlarmDisarmedNotify(Entities entities, ITextToSpeechService tts)
    {
        var mediaPlayer = entities.MediaPlayer.VlcTelnet;
        //mediaPlayer.VolumeSet(0.3);
        mediaPlayer.VolumeSet(0);
        tts.Speak("media_player.vlc_telnet", "Alarm rozbrojony!", "google_say", "pl");
    }
}