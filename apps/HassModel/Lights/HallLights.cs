namespace Lights;
[NetDaemonApp]

public class HallLights
{
    private readonly Entities _entities;

    public HallLights(IHaContext ha)
    {
        _entities = new Entities(ha);
        var motion = new[]
            {_entities.BinarySensor.Hallpir};

        _entities.BinarySensor.Hallpir
            .WhenTurnsOn(_ => TurnOnFromBathroom());

        motion
            .StateChanges().Where(e => e.New?.State == "off")
            .Delay(TimeSpan.FromSeconds(10))
            .Subscribe(_ => TurnOff());
    }

    private void TurnOnFromBathroom()
    {
        var light = _entities.Light.Hallled;
        light.TurnOn(colorName: "Magenta", effect: "Wipe up on", brightness: 80);

    }

    private void TurnOnFromBedroom()
    {
        var light = _entities.Light.Hallled;

        light.TurnOn(colorName: "Magenta", effect: "Wipe down on", brightness: 80);

    }

    private void TurnOff()
    {
        var light = _entities.Light.Hallled;
        string motion = _entities.BinarySensor.Hallpir.State;

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
