namespace Lights;
[NetDaemonApp]

public class HallLights
{
    public HallLights(IHaContext ha)
    {
        var _entities = new Entities(ha);
        var motion = new[]
            {_entities.BinarySensor.Hallpir};

        _entities.BinarySensor.Hallpir
            .WhenTurnsOn(_ => TurnOnFromBathroom(_entities));        

        motion
            .StateChanges().Where(e => e.New?.State == "off")
            .Delay(TimeSpan.FromSeconds(10))
            .Subscribe(_ => TurnOff(_entities));
    }

    private static void TurnOnFromBathroom(Entities entities)
    {
        var light = entities.Light.Hallled;
        light.TurnOn(colorName: "Magenta", effect: "Wipe up on", brightness: 80);

    }

    private static void TurnOnFromBedroom(Entities entities)
    {
        var light = entities.Light.Hallled;

        light.TurnOn(colorName: "Magenta", effect: "Wipe down on", brightness: 80);

    }

    private static void TurnOff(Entities entities)
    {
        var light = entities.Light.Hallled;
        string motion = entities.BinarySensor.Hallpir.State;

        if (motion == "off" && light.State == "on" && light.Attributes.Effect == "Wipe up on")
        {
            light.TurnOn(effect: "Wipe up off");
        }
        else if (motion == "off" && light.State == "on" && light.Attributes.Effect == "Wipe down on")
        {
            light.TurnOn(effect: "Wipe down off");
        }
    }
}
