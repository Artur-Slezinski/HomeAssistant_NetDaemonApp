using NetDaemon.Extensions.MqttEntityManager;

namespace Entity_Manager;
[NetDaemonApp]

public class OledDistanceHelper
{
    private readonly Entities _entities;
    private readonly IMqttEntityManager _entityManager;

    public OledDistanceHelper(IMqttEntityManager entityManager, IHaContext ha)
    {
        _entityManager = entityManager;
        _entities = new Entities(ha);

        Initialize();
    }

    private void Initialize()
    {
        var entityId = "sensor.oledDistance";

        CreateAsync(entityId);

        _entities.Sensor.Weatherdisplaydistance
            .StateAllChanges()
            .Delay(TimeSpan.FromSeconds(2))
            .Subscribe(_ => OledState(entityId));        
    }

    async Task CreateAsync(string entityId, EntityCreationOptions? options = null, object? additionalConfig = null)
    {
        await _entityManager.CreateAsync(entityId)
            .ConfigureAwait(false);
    }
    async Task SetStateAsync(string entityId, string state)
    {
        await _entityManager.SetStateAsync(entityId, state)
            .ConfigureAwait(false);
    }

    private void OledState(string entityId)
    {
        //var entityId = "sensor.oledDistance";
        var distance = _entities.Sensor.Weatherdisplaydistance.State;
        var sunPosition = _entities.Sun.Sun.State;

        while (distance >= 0 && distance <= 0.8)
        {
            if (sunPosition == "above_horizon")
            {
                SetStateAsync(entityId, "1");
            }
            else
            {
                SetStateAsync(entityId, "0.2");
            }
        }

        SetStateAsync(entityId, "0");
    }
}