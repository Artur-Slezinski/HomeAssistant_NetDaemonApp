using NetDaemon.HassModel.Entities;

namespace Lights;
[NetDaemonApp]

public class HallLights
{
    private readonly Entities _entities;

    public HallLights(IHaContext ha)
    {
        _entities = new Entities(ha);

        Initialize();        
    }

    private void Initialize()
    {
        var motion = new[]
            {_entities.BinarySensor.Hallbathroompir,
             _entities.BinarySensor.Hallbedroompir};

        _entities.BinarySensor.Hallbathroompir
            .WhenTurnsOn(_ => TurnOnFromBathroom());
        _entities.BinarySensor.Hallbedroompir
            .WhenTurnsOn(_ => TurnOnFromBedroom());

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
       
        var motion = new[]
            {_entities.BinarySensor.Hallbathroompir.State,
             _entities.BinarySensor.Hallbedroompir.State};    

        if (motion[0] == "off" && motion[1] == "off" && light.Attributes.Effect == "Wipe up on") 
        {
            
            light.TurnOn(effect: "Wipe up off");
        }
        else if (motion[0] == "off" && motion[1] == "off" && light.Attributes.Effect == "Wipe down on") 
        {
            light.TurnOn(effect: "Wipe down off");
        }
    }
}
