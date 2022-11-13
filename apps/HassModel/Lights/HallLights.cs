using System.Threading;

namespace Lights;
[NetDaemonApp]

public class HallLights
{
    public HallLights(IHaContext ha)
    {
        var _entities = new Entities(ha);

        _entities.BinarySensor.Hallpir
            .StateChanges().Where(e => e.New?.State == "on")
            .Subscribe(_ => TurnOnFromBathroom(_entities));

        _entities.BinarySensor.Hallpir
            .StateChanges().Where(e => e.New?.State == "off")
            .Delay(TimeSpan.FromSeconds(10))
            .Subscribe(_ => TurnOffFromBathroom(_entities));
    }

    private static void TurnOnFromBathroom(Entities entities)
    {
        var light = entities.Light.Hallled;
        if (light.State == "on" && light.Attributes.Effect == "Wipe up off")
        {
            light.TurnOn(colorName: "Magenta", effect: "Wipe up on", brightness: 80);
        }
    }

    private static void TurnOffFromBathroom(Entities entities)
    {
        var light = entities.Light.Hallled;
        if (light.State == "on" && light.Attributes.Effect == "Wipe up on")
        {            
            light.TurnOn(effect: "Wipe up off");
        }
    }
}
