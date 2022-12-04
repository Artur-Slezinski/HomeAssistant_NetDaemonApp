using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.MqttEntityManager.Models;

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

        Task initialize = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var entityId = "sensor.oledDistance";

        await CreateAsync(entityId);

        _entities.Sensor.Weatherdisplaydistance
            .StateAllChanges()
            .Delay(TimeSpan.FromSeconds(2))
            .Subscribe(async _ => await OledStateAsync(entityId));
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
    

    private async Task OledStateAsync(string entityId)
    {
        var distance = _entities.Sensor.Weatherdisplaydistance.State;
        var sunPosition = _entities.Sun.Sun.State;

        if (distance >= 0 && distance <= 0.8)
        {
            if (sunPosition == "above_horizon")
            {
                await SetStateAsync(entityId, "1");
                
            }
            else
            {
                await SetStateAsync(entityId, "0.2");                
            }           
        }        
        else await SetStateAsync(entityId, "0");
    }
}