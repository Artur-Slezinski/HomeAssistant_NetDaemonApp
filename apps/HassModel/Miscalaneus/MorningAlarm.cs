using Microsoft.Extensions.Hosting.Internal;
using NetDaemon.HassModel.Entities;
using Newtonsoft.Json.Converters;

namespace Miscalaneus;

[NetDaemonApp]

public class MorningAlarm
{
    string alarm ="";

    public MorningAlarm(IHaContext ha, IScheduler scheduler)
    {
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);
        //MornigMusic(_myEntities);
        var date = DateTime.UtcNow;
        var isoDate = date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'+'00':'00");
        //var isoDate = date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");


        _myEntities.Sensor.SmG996bNextAlarm
            .StateChanges()
            .Subscribe(_ => MornigMusic(_myEntities));



    }

    private void MornigMusic(Entities entities)
    {
        var mediaPlayer = entities.MediaPlayer.VlcTelnet;
        var _myEntities = entities;
        var date = DateTime.UtcNow.AddMinutes(5);
        var isoDate = date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'+'00':'00");       

        Console.WriteLine("Alarm: " + alarm);
        Console.WriteLine("Teraz: " + isoDate);

        if (alarm == isoDate)
        {
            mediaPlayer.VolumeSet(0.08);
            mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
            Console.WriteLine("ALARM DZIAŁA!");
            alarm = entities.Sensor.SmG996bNextAlarm.State;
        }
        else
            alarm = entities.Sensor.SmG996bNextAlarm.State;

        

        //mediaPlayer.VolumeSet(0.08);
        //    mediaPlayer.PlayMedia(mediaContentType: "music", mediaContentId: "http://192.168.2.5:8123/local/sounds/alarm.mp3");
        //    Console.WriteLine("ALARM DZIAŁA!");


    }


}
