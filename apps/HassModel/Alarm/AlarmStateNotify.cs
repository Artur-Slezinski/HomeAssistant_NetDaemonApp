using LivingRoom;

namespace Alarm;
[NetDaemonApp]

public class AlarmStateNotify
{
    public AlarmStateNotify(IHaContext ha)
    {        
        var _myEntities = new Entities(ha);
        var _services = new Services(ha);

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "armed_away")
            .Subscribe(_ => WhatsAppAlarmArmedNotify(_services, _myEntities));

        _myEntities.AlarmControlPanel.Alarm
            .StateChanges().Where(e => e.New?.State == "disarmed")
            .Subscribe(_ => WhatsAppAlarmDisarmedNotify(_services, _myEntities));        
    }

    private void WhatsAppAlarmArmedNotify(Services services, Entities entities)
    {
        services.Notify.Whatsapp
        (message: "Alarm uzbrojony!");

        FlashingLights(entities);
    }

    private void WhatsAppAlarmDisarmedNotify(Services services, Entities entities)
    {
        services.Notify.Whatsapp
                 (message: "Alarm rozbrojony!");       
    }

    private void FlashingLights(Entities entities)
    {

        while (entities.AlarmControlPanel.Alarm.State == "armed_away")
        {
            entities.Light.Led.TurnOn(transition: 0, colorName: "Red", brightness: 255);
            System.Threading.Thread.Sleep(500);
            entities.Light.Led.TurnOn(transition: 0, colorName: "Blue", brightness: 255);
            System.Threading.Thread.Sleep(500);
        }
    }
}