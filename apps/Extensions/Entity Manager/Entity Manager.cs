﻿using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.MqttEntityManager.Models;

namespace Entity_Manager;
[NetDaemonApp]

public class Entity_Manager
{
    private readonly IMqttEntityManager _entityManager;
    public Entity_Manager(IMqttEntityManager entityManager, IHaContext ha)
    {
        _entityManager = entityManager;
        var _entities = new Entities(ha);

        _entities.Sensor.Weatherdisplaydistance
            .StateAllChanges()
            .Subscribe(_ => OledState(entityManager, _entities));

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
    private void OledState(IMqttEntityManager entityManager, Entities entities)
    {
        var entityId = "sensor.oledDistance";


        if (entities.Sensor.Weatherdisplaydistance.State >= 0 && entities.Sensor.Weatherdisplaydistance.State <= 0.4)
        {
            SetStateAsync(entityId, "1");

        }
        else
        {
            SetStateAsync(entityId, "0");
        }
    }
}