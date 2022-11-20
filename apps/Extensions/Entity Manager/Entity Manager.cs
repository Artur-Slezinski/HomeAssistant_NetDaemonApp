using NetDaemon.Extensions.MqttEntityManager;

namespace Entity_Manager;
[NetDaemonApp]

public class Entity_Manager
{
    private readonly Entities _entities;
    private readonly IMqttEntityManager _entityManager;

    public Entity_Manager(IMqttEntityManager entityManager, IHaContext ha)
    {
        _entityManager = entityManager;
        _entities = new Entities(ha);

        Initialize();
    }

    private void Initialize()
    {
        _entities.Sensor.Weatherdisplaydistance
            .StateAllChanges()
            .Delay(TimeSpan.FromSeconds(2))
            .Subscribe(_ => OledState());

        var entityId = "sensor.oledDistance";

        CreateAsync(entityId);
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

    private void OledState()
    {
        var entityId = "sensor.oledDistance";
        var distance = _entities.Sensor.Weatherdisplaydistance.State;
        var sunPosition = _entities.Sun.Sun.State;

        if (distance >= 0 && distance <= 0.8)
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
        else
        {
            SetStateAsync(entityId, "0");
        }
    }
}